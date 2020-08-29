using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response
{
    public class RespIncomeInfoDto
    {
        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal CanWithdrawalsMoney { get; set; }
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal EarningsPrice { get; set; }
        /// <summary>
        /// 已提现
        /// </summary>
        public decimal AlreadyWithdrawalsMoney { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public decimal WithdrawalsMoneyIng { get; set; }
        /// <summary>
        /// 今日收益
        /// </summary>
        public decimal TodayIncome { get; set; }

    }
}
