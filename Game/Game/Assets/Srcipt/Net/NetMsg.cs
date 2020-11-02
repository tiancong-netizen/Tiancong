using System;


    /// <summary>
    /// 网络消息类
    /// 每次发送消息都发送类，接收消息后，需要转换成这个类
    /// </summary>
    public class NetMsg
    {
        //操作码
        public int opCode { get; set; }
        //子操作码
        public int subCode { get; set;}
        //传递参数
        public object value { get; set; }

        public NetMsg()
        {

        }
        public NetMsg(int opCode,int subCode,object value)
        {
            this.opCode = opCode;
            this.subCode = subCode;
            this.value = value;
        }
        public void change(int opCode, int subCode, object value)
        {
            this.opCode = opCode;
            this.subCode = subCode;
            this.value = value;
        }
    }
 