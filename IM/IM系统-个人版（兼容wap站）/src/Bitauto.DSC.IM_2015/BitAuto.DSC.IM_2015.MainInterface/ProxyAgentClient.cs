using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BitAuto.DSC.IM_2015.MainInterface
{
    [DataContract]
    public class ProxyAgentClient
    {
        [DataMember]
        public string AgentToken;

        [DataMember]
        public int AgentID;

        [DataMember]
        public string AgentNum;
        [DataMember]
        public string AgentName;

        [DataMember]
        public string InBGID;

        [DataMember]
        public string InBGIDName;
        /// <summary>
        /// 坐席状态表主键
        /// </summary>
        [DataMember]
        public int AgentStatusRecID;

        /// <summary>
        /// 1：在线，2：离线，3：暂离
        /// </summary>
        [DataMember]
        public int Status;      //存储坐席当前状态。        
        [DataMember]
        public string BusinessLines;
        [DataMember]
        public int MaxDialogNum;

        /// <summary>
        /// 所属IIS标记
        /// </summary>
        [DataMember]
        public string IISIP;

        [DataMember]
        public DateTime LastActiveTime = DateTime.Now;
        [DataMember]
        public DateTime LastRequestTime = DateTime.Now;

        [DataMember]
        public int Type;//网友2，客服1

        public ConcurrentDictionary<string, ProxyNetFriend> TalkUserList;




    }
}
