using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using UnityEngine;

public class NetMsgCenter : MonoBehaviour
{

    private ClientPeer client;
    public static NetMsgCenter Instance;
    private void Awake()
    {
        ClientPeer client = new ClientPeer();
        client.Connect("127.0.0.1", 6666);
    }
    //如果消息不为空，一直执行
    private void FixedUpdate()
    {
        if (client == null) return;
        //如果消息队列不为空，一直执行处理的方法
        while (client.netMsgQueue.Count > 0)
        {
            NetMsg msg = client.netMsgQueue.Dequeue();
            ProcessServerSendMsg(msg);
        }
    }
    #region 发送消息
    public void SendMsg(int opCode, int subCode, object value)
    {
        client.SendMsg(opCode, subCode, value);
    }
    public void SendMsg(NetMsg msg)
    {
        client.SendMsg(msg);
    }
    #endregion

    #region 处理服务器发来的消息  
    AccountHandler accountHander = new AccountHandler();
    MatchHandler matchHander = new MatchHandler();
    ChatHandler chatHander = new ChatHandler();
    FightHandler fightHander = new FightHandler();
    /// <summary>
    /// 处理服务器发来的消息
    /// </summary>
    /// <param name="msg"></param>
    private void ProcessServerSendMsg(NetMsg msg)
    {
        switch (msg.opCode)
        {
            case OpCode.Account:
                accountHander.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Chat:
                matchHander.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Match:
                chatHander.OnReceive(msg.subCode, msg.value);
                break;
            case OpCode.Fight:
                fightHander.OnReceive(msg.subCode, msg.value);
                break;

            default:
                break;
        }
    }
    #endregion


}

