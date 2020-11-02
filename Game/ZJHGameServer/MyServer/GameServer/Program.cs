using System;
using MyServer;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            server.SetApplication(new NetMsgCenter());
            server.StartServer("127.0.0.1", 6666,10);
            Console.ReadKey();
        }
    }
}
 