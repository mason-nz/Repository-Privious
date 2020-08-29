using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils;

namespace HollyContact5_Demo
{
    /// 电话状态
    /// <summary>
    /// 电话状态
    /// </summary>
    public enum PhoneStatus
    {
        PS01_就绪,
        PS02_签出,
        PS03_置闲,
        PS04_置忙,
        PS05_休息,
        PS06_话后,
        PS07_来电震铃,
        PS08_普通通话,
        PS09_咨询通话_发起方,
        PS10_咨询方通话_接受者,
        PS11_会议通话_发起方,
        PS12_会议方通话_接受者,
        PS13_保持,
        PS14_呼出拨号中,
        PS15_咨询拨号中,
        PS16_会议拨号中,
        PS17_转接拨号中,
        PS18_监听中
    }

    /// 坐席状态（入库使用）
    /// <summary>
    /// 坐席状态（入库使用）
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
        C3_转接 = 3
    }

    /// 呼出类型
    /// <summary>
    /// 呼出类型
    /// </summary>
    public enum OutBoundTypeEnum
    {
        OT1_页面呼出 = 1,
        OT2_客户端呼出 = 2,
        OT3_转接 = 3
    }
}
