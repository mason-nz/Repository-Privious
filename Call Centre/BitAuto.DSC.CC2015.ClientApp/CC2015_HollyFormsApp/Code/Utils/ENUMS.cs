using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils;

namespace CC2015_HollyFormsApp
{
    /// 电话状态
    /// <summary>
    /// 电话状态
    /// </summary>
    public enum PhoneStatus
    {
        PS01_就绪 = 1,
        PS02_签出 = 2,
        PS03_置闲 = 3,
        PS04_置忙 = 4,
        PS05_休息 = 5,
        PS06_话后 = 6,
        PS07_来电振铃 = 7,
        PS08_普通通话 = 8,
        PS09_咨询通话_发起方 = 9,
        PS10_咨询方通话_接受者 = 10,
        PS11_会议通话_发起方 = 11,
        PS12_会议方通话_接受者 = 12,
        PS13_保持 = 13,
        PS14_呼出拨号中 = 14,
        PS15_咨询拨号中 = 15,
        PS16_会议拨号中 = 16,
        PS17_转接拨号中 = 17,
        PS18_客服被锁定 = 18,
        PS19_监听振铃 = 19,
        PS20_监听中 = 20,
        PS21_强插振铃 = 21,
        PS22_强插中 = 22,
    }
    /// 客服状态（入库使用）
    /// <summary>
    /// 客服状态（入库使用）
    /// </summary>
    public enum AgentState
    {
        [EnumTextValue("签出")]
        AS1_签出 = 1,
        [EnumTextValue("签入")]
        AS2_签入 = 2,
        [EnumTextValue("置闲")]
        AS3_置闲 = 3,
        [EnumTextValue("置忙")]
        AS4_置忙 = 4,
        [EnumTextValue("话后")]
        AS5_话后 = 5,
        [EnumTextValue("振铃")]
        AS8_振铃 = 8,
        [EnumTextValue("通话中")]
        AS9_通话中 = 9,
        [EnumTextValue("未知")]
        AS0_未知 = -2
    }
    /// 置忙状态
    /// <summary>
    /// 置忙状态
    /// </summary>
    public enum BusyStatus
    {
        [EnumTextValue("自动")]
        BS0_自动 = 0,
        [EnumTextValue("小休")]
        BS1_小休 = 1,
        [EnumTextValue("任务回访")]
        BS2_任务回访 = 2,
        [EnumTextValue("业务处理")]
        BS3_业务处理 = 3,
        [EnumTextValue("会议")]
        BS4_会议 = 4,
        [EnumTextValue("培训")]
        BS5_培训 = 5,
        [EnumTextValue("离席")]
        BS6_离席 = 6
    }
    /// 通话类型（数据库）
    /// <summary>
    /// 通话类型（数据库）
    /// </summary>
    public enum Calltype
    {
        [EnumTextValue("未知")]
        C0_未知 = -2,
        [EnumTextValue("呼入")]
        C1_呼入 = 1,
        [EnumTextValue("呼出")]
        C2_呼出 = 2,
        [EnumTextValue("转接")]
        C3_转接 = 3,
        [EnumTextValue("拦截")]
        C4_拦截 = 4
    }
    /// 呼出类型
    /// <summary>
    /// 呼出类型
    /// </summary>
    public enum OutBoundTypeEnum
    {
        OT0_未知 = -2,
        OT1_页面呼出 = 1,
        OT2_客户端呼出 = 2,
        OT3_转接 = 3,
        OT4_自动外呼 = 4,
    }
    /// 发送给web端的事件
    /// <summary>
    /// 发送给web端的事件
    /// </summary>
    public enum UserEvent
    {
        /// <summary>
        /// 呼入事件
        /// </summary>
        [EnumTextValue("呼入事件")]
        Transferred = 1,
        /// <summary>
        /// 呼出事件
        /// </summary>
        [EnumTextValue("呼出事件")]
        NetworkReached = 2,
        /// <summary>
        /// 通话事件
        /// </summary>
        [EnumTextValue("通话事件")]
        Established = 3,
        /// <summary>
        /// 外拨事件
        /// </summary>
        [EnumTextValue("外拨事件")]
        Initiated = 4,
        /// <summary>
        /// 挂断事件
        /// </summary>
        [EnumTextValue("挂断事件")]
        Released = 5,
        /// <summary>
        /// 自动外呼
        /// </summary>
        [EnumTextValue("自动外呼")]
        AutoCall = 6,
        /// <summary>
        /// 通知
        /// </summary>
        [EnumTextValue("通知")]
        Notice = 7
    }
    /// 挂断类型
    /// <summary>
    /// 挂断类型
    /// </summary>
    public enum ReleaseType
    {
        客服挂断, 用户挂断
    }

    /// 排序方式
    /// <summary>
    /// 排序方式
    /// </summary>
    public enum OrderBYForListen
    {
        状态, 工号
    }
    /// 数据来源
    /// <summary>
    /// 数据来源
    /// </summary>
    public enum DataSourceForListen
    {
        合力,
        CC
    }
    /// 呼入类型
    /// <summary>
    /// 呼入类型
    /// </summary>
    public enum CallInType
    {
        普通呼入 = 1,
        自动呼入 = 2,
        监控呼入 = 3,
    }
    /// 区域类型-目前只支持两种
    /// <summary>
    /// 区域类型-目前只支持两种
    /// </summary>
    public enum AreaType
    {
        西安, 北京
    }
    /// 选择方式
    /// <summary>
    /// 选择方式
    /// </summary>
    public enum Choose
    {
        手机号 = 0, 分机 = 1
    }
}
