using System;
using MyServer;

namespace GameServer.Logic
{
    public interface IHander
    {
        //断开连接
        void Disconnect(ClientPeer client);
        //接收数据
        void Receive(ClientPeer client, int subCode, object value);


    }
}
