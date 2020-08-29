using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response.Withdrawals
{
    public class RespWithdrawalsPriceClc
    {
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal WithdrawalsPrice { get; set; }
        /// <summary>
        /// 个税金额
        /// </summary>
        public decimal IndividualTaxPeice { get; set; }

        /// <summary>
        /// 实际付款
        /// </summary>
        public decimal PracticalPrice { get; set; }
    }
}
