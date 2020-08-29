using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Response
{
    /// <summary>
    /// 注释：RespWithdrawalsPriceClc
    /// 作者：lix
    /// 日期：2018/5/23 18:57:41
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
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
