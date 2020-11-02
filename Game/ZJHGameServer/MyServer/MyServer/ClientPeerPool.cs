using System;
using System.Collections.Generic;

namespace MyServer
{
    /// <summary>
    /// 客服端对象池连接
    /// </summary>
    public class ClientPeerPool
    {
        private Queue<ClientPeer> clientPeerQueue;

        public ClientPeerPool(int maxCount)
        {
            clientPeerQueue = new Queue<ClientPeer>(maxCount);
        }
        public void Enqueue(ClientPeer client)
        {
            clientPeerQueue.Enqueue(client);
        }
        //public ClientPeer()
        //{
            
        //}
    }
}
