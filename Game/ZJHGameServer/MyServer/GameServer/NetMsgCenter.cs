using System;
using GameServer.Logic;
using MyServer;
using Protocol.Code;

namespace GameServer
{
    //网络消息处理中心，分发消息到对应模块
    class NetMsgCenter : IApplication
    {
        private AccountHandler accountHandler = new AccountHandler();
        private MatchHandler matchHandler = new MatchHandler();
        private ChatHandler chatHandler = new ChatHandler();
        private FightHandler fightHandler = new FightHandler();

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        public void Disconnect(ClientPeer client)
        {
            fightHandler.Disconnect(client);
            chatHandler.Disconnect(client);
            matchHandler.Disconnect(client);
            accountHandler.Disconnect(client);

        }

        public void Receive(ClientPeer client, NetMsg msg)
        {
            switch (msg.opCode)
            {
                case OpCode.Account:
                    accountHandler.Receive(client,msg.subCode,msg.value);
                    break;
                case OpCode.Chat:
                    chatHandler.Receive(client, msg.subCode, msg.value);
                    break;
                case OpCode.Fight:
                    fightHandler.Receive(client, msg.subCode, msg.value);
                    break;
                case OpCode.Match:
                    matchHandler.Receive(client, msg.subCode, msg.value);
                    break;
                default:    
                    break;
            }
        }
    }
}
