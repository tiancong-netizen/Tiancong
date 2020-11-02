using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyServer
{
    public class ServerPeer
    {
        //服务器Socket
        private Socket serverSocket;
        //计量器
        private Semaphore semaphore;
        /// 对象连接池
        private ClientPeerPool clientPeerPool;
        /// 应用层
        private IApplication application;
        //设置应用层
        public void SetApplication(IApplication application)
        {
            this.application = application;
        }
        /// <summary>
        /// 开启服务器
        /// </summary>
        public void StartServer(string ip, int port, int maxClient)
        {
            try
            {
                clientPeerPool = new ClientPeerPool(maxClient);
                semaphore = new Semaphore(maxClient, maxClient);
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //填满客服端对象连接池
                for (int i = 0; i < maxClient; i++)
                {
                    ClientPeer temp = new ClientPeer();
                    temp.receiveCompleted += ReceiveProcessCompleted;
                    temp.ReceiveArgs.Completed += ReceiveArgs_Copmleted;
                    clientPeerPool.Enqueue(temp);
                }
                //绑定到进程
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                //最大监听数
                serverSocket.Listen(maxClient);
                Console.WriteLine("服务器启动成功");
                StartAccept(null);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
             
        }
        #region 接受客服端连接请求
        /// <summary>
        /// 接受客服端连接请求
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += E_Completed;
            }
            //如果result为true，代表在接受连接
            //如果result为false,代表接受完成
            bool result = serverSocket.AcceptAsync(e);
            if (result == false)
            {

            }
        }
        #endregion
        #region 接受数据
        /// <summary>
        /// 异步接受客户端的连接完成后的触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void E_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        /// <summary>
        /// 发送连接请求
        /// </summary>
        /// <param name="e"></param>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            semaphore.WaitOne();
            ClientPeer client = new ClientPeer();
            client.clientSockert = e.AcceptSocket;
            Console.WriteLine(client.clientSockert.RemoteEndPoint + "客服端连接成功");
            //接收消息
            StartReceive(client);

            e.AcceptSocket = null;
            StartAccept(e);
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <param name="client"></param>
        private void StartReceive(ClientPeer client)
        {
            try
            {
                bool result = client.clientSockert.ReceiveAsync(client.ReceiveArgs);
                if (result == false)
                {
                    ProcessReceive(client.ReceiveArgs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// 异步接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceiveArgs_Copmleted(object sender, SocketAsyncEventArgs e)
        {

        }
        /// <summary>
        /// 处理数据的接收
        /// </summary>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            ClientPeer client = e.UserToken as ClientPeer;
            if (client.ReceiveArgs.SocketError == SocketError.Success && client.ReceiveArgs.BytesTransferred > 0)
            {
                byte[] packet = new byte[2048];
                Buffer.BlockCopy(client.ReceiveArgs.Buffer, 0, packet, 0, client.ReceiveArgs.BytesTransferred);

                //让ClientPeer自身处理接受到的事情
                client.ProcesReceive(packet);
                StartReceive(client);
            }
            //断开连接
            else
            {
                //没有传输的字节数，代表断开了连接
                if (client.ReceiveArgs.BytesTransferred == 0)
                {
                    //客服端主动断开连接
                    if (client.ReceiveArgs.SocketError == SocketError.Success)
                    {
                        Disconnect(client, "客服端主动断开连接");
                    }
                    //因为网络异常被迫断开连接
                    else
                    {
                        Disconnect(client, client.ReceiveArgs.SocketError.ToString());
                    }
                }

            }
        }
        /// <summary>
        /// 一条消息处理完成后的回调
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        private void ReceiveProcessCompleted(ClientPeer client,NetMsg msg)
        {
            //交给应用层处理这个数据
            application.Receive(client,msg);
        }
        #endregion
        #region 断开连接
        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason"></param>
        private void Disconnect(ClientPeer client, string reason)
        {
            try
            {
                if (client == null)
                {
                    throw new Exception("客服端为空，无法连接");
                }
                application.Disconnect(client);
                Console.WriteLine(client.clientSockert.RemoteEndPoint + "断开连接原因:" + reason);
                client.Disconnect();

                clientPeerPool.Enqueue(client);
                semaphore.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
        #endregion

}
