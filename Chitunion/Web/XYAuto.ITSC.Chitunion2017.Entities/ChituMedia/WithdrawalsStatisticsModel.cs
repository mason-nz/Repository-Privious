using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class WithdrawalsStatisticsModel
    {
        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用戶名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 用戶手机号
        /// </summary>
        public string Mobile { get; set; } = string.Empty;
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; } = string.Empty;
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal AccumulatedIncome { get; set; }
        /// <summary>
        /// 已提现
        /// </summary>
        public decimal HaveWithdrawals { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public decimal WithdrawalsProcess { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal RemainingAmount { get; set; }
    }
    public class QueryWithdrawalsArgs
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int CategoryID { get; set; }

        public string IncomeBeginTime { get; set; }

        public string IncomeEndTime { get; set; }

        public int OrderBy { get; set; }

        public int UserID { get; set; }
    }

    public class WithdrawalsTitle
    {
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal AccountBalanceTotal { get; set; }
        /// <summary>
        /// 已提现
        /// </summary>
        public decimal HaveWithdrawalsTotal { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public decimal WithdrawalsProcessTotal { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal RemainingAmountTotal { get; set; }
    }
}
