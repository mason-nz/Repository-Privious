using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.Entities
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
        MError = 8,
        //转接客服
        [EnumTextValue("MTransfer")]
        MTransfer = 9
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
        Leaveline = 2,
        [EnumTextValue("暂离")]
        LeavingForAWhile = 3,
        [EnumTextValue("等待坐席响应")]
        Waiting = 4,
        [EnumTextValue("聊天中")]
        InCommunicating = 5,
        [EnumTextValue("对话关闭")]
        Closed = 6
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
    /// <summary>
    /// 发短信错误提示
    /// </summary>
    [Serializable]
    public enum SendSMSInfo
    {
        //        -2	传入的参数productcode不合法
        //-1	Md5校验不合法
        //-3	记录发送信息入库错误
        //-4	发送请求到短信通道的时候失败
        //-5	短信发送成功，但通道返回数据无法记录
        //-6	短信内包含非法字符。
        //-7	手机号码不合法
        //-99	未知错误

        [EnumTextValue("Md5校验不合法")]
        One = -1,
        [EnumTextValue("传入的参数productcode不合法")]
        Two = -2,
        [EnumTextValue("记录发送信息入库错误")]
        Three = -3,
        [EnumTextValue("发送请求到短信通道的时候失败")]
        Four = -4,
        [EnumTextValue("短信发送成功，但通道返回数据无法记录")]
        Five = -5,
        [EnumTextValue("短信内包含非法字符")]
        Six = -6,
        [EnumTextValue("手机号码不合法")]
        Seven = -7,
        [EnumTextValue("未知错误")]
        Eight = -99
    }

    /// <summary>
    /// 会话关闭方式
    /// </summary>
    [Serializable]
    public enum CloseType
    {

        [EnumTextValue("访客关闭")]
        NetFrindClose = 1,
        [EnumTextValue("客服关闭")]
        AgentClose = 2,
        [EnumTextValue("访客超时，系统关闭")]
        NetFrindTimeOut = 3,
        [EnumTextValue("客服超时，系统关闭")]
        AgentTimeOut = 4,
        [EnumTextValue("系统关闭")]
        SystemClose = 5,
        [EnumTextValue("转移网友，系统关闭")]
        TurnNetFrind = 6
    }


}
