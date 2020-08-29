using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_DMS2014.Entities
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
        ChatMessage = 2,
        //离线消息
        [EnumTextValue("MLline")]
        MLline = 3,
        //发送坐席全忙消息
        [EnumTextValue("MAllBussy")]
        MAllBussy = 4,
        //排队顺序消息
        [EnumTextValue("MQueueSort")]
        MQueueSort = 5,
        //满意度消息
        [EnumTextValue("MSatisfaction")]
        MSatisfaction = 6,
        //上传文件消息
        [EnumTextValue("MSendFile")]
        MSendFile = 7,
        //系统异常消息
        [EnumTextValue("MError")]
        MError = 8
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
