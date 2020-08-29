using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.DSC.IM2014.Entities
{
    /// <summary>
    /// 消息类型
    /// </summary>
    [Serializable]
    public enum MessageType
    {
        //分配坐席
        [EnumTextValue("MAllocAgent")]
        MAllocAgent = 1,
        //对话消息
        [EnumTextValue("ChatMessage")]
        MTalk = 2,
        //发送XX在线消息
        [EnumTextValue("MOnline")]
        MOnline = 3,
        //网友离线消息
        [EnumTextValue("MLline")]
        MLline = 4,
        //发送坐席全忙消息
        [EnumTextValue("MAllBussy")]
        MAllBussy = 5,
        //排队达到上限
        [EnumTextValue("MaxQueue")]
        MaxQueue = 6
    }
    /// <summary>
    /// 坐席状态
    /// </summary>
    [Serializable]
    public enum AgentStatus
    {
        //[EnumTextValue("请选择")]
        //Choose = -1,
        [EnumTextValue("在线")]
        Online = 1,
        [EnumTextValue("离线")]
        Leaveline = 2
    }
    /// <summary>
    /// 用户类型
    /// </summary>
    [Serializable]
    public enum UserType
    {
        [EnumTextValue("坐席")]
        Agent = 1,
        [EnumTextValue("网友")]
        User = 2
    }
    /// <summary>
    /// 留言类型
    /// </summary>
    [Serializable]
    public enum LeaveMessageType
    {
        [EnumTextValue("购车咨询")]
        BuyCarConsultative = 1,
        [EnumTextValue("活动咨询")]
        ActivityConsultative = 2,
        [EnumTextValue("网站建议")]
        WebSitProposal = 3,
        [EnumTextValue("其他")]
        Other = 4
    }


}
