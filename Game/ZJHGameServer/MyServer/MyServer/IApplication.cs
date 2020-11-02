using System;
namespace MyServer
{
    public interface IApplication
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        void Disconnect(ClientPeer client);
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        void Receive(ClientPeer client,NetMsg msg);
    }
}
