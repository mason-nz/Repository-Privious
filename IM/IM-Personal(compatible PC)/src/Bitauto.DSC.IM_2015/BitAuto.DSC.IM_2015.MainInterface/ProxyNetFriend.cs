using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace BitAuto.DSC.IM_2015.MainInterface
{
    [DataContract]
    public class ProxyNetFriend
    {
        [DataMember]
        public string AgentToken;
        [DataMember]
        public int AgentID;

        [DataMember]
        public int CSID;
        [DataMember]
        public string Token;
        [DataMember]
        public string IISIP;
        [DataMember]
        public string BusinessLines;
        [DataMember]
        public int VisitID;
        [DataMember]
        public string contractphone;
        [DataMember]
        public string NetFName;
        [DataMember]
        public DateTime CreateTime;
        [DataMember]
        public DateTime ConverSTime;  //网友回复时间
        [DataMember]
        public DateTime LastActiveTime = DateTime.Now;
        [DataMember]
        public DateTime LastMessageTime;  //网友回复时间
        [DataMember]
        public int Type;//网友2，客服1
        [DataMember]
        public bool IsAgentReply;//坐席是否回复
        [DataMember]
        public string AgentNum;
        public bool IsTurnOut;
        public bool IsTurnIn;
        public long WaitNum;
        //关闭方式
        public int CloseType;
        //最后发消息时间
        public DateTime SendMessageTime;

        

    }
}
