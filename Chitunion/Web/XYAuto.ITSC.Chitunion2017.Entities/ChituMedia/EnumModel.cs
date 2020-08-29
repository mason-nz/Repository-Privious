using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class EnumInfo
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public enum SortInde
    {
        [Description("升序")]
        asc = 1,
        [Description("降序")]
        desc = 2
    }
    /// <summary>
    /// 媒体刊位枚举
    /// </summary>
    public enum PositionEnum
    {
        [Description("P60018001,P60018002")]
        单图文 = 6001,
        [Description("P60028001,P60028002")]
        多图文头条 = 6002,
        [Description("P60038001,P60038002")]
        多图文第二条 = 6003,
        [Description("P60048001,P60048002")]
        多图文3_N条 = 6004
    }

    public enum SortEnum
    {
        [Description("粉丝数")]
        FansCount = 1001,
        [Description("平均阅读数")]
        ReadNum = 1002,
        [Description("平均转发数")]
        ForwardAvg = 1003,
        [Description("平均评论数")]
        CommentAvg = 1004,
        [Description("平均点赞数")]
        LikeAvg = 1005,
        [Description("日活")]
        DailyLive = 1006
    }

    public enum UserCategoryEnum
    {
        广告主 = 29001,
        媒体主 = 29002,
        内部用户 = 29003,
    }
    public enum UsereCategory
    {
        /// <summary>
        /// 广告主
        /// </summary>
        g_user = 29001,
        /// <summary>
        /// 媒体主
        /// </summary>
        m_user = 29002
    }
    public enum MediaUserOrder
    {
        [Description(" CreateTime DESC ")]
        未认证 = 0,
        [Description(" ApplyTime DESC ")]
        待审核 = 1,
        [Description(" AuditTime DESC ")]
        已认证 = 2,
        [Description(" AuditTime DESC ")]
        认证未通过 = 3
    }
    /// <summary>
    /// 收入管理排序枚举
    /// </summary>
    public enum IncomeOrder
    {
        [Description(" AccumulatedIncome ASC ")]
        累计收益升序 = 1001,
        [Description(" AccumulatedIncome DESC ")]
        累计收益降序 = 1002,
        [Description(" RemainingAmount ASC ")]
        账户余额升序 = 1011,
        [Description(" RemainingAmount DESC ")]
        账户余额降序 = 1012,
        [Description(" HaveWithdrawals ASC ")]
        已提现金额升序 = 1021,
        [Description(" HaveWithdrawals DESC ")]
        已提现金额降序 = 1022,
        [Description(" WithdrawalsProcess ASC ")]
        提现中金额升序 = 1031,
        [Description(" WithdrawalsProcess DESC ")]
        提现中金额降序 = 1032,
    }
}
