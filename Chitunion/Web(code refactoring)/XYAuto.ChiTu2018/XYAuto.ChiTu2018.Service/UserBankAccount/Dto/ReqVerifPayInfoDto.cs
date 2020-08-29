using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.UserBankAccount.Dto
{
    /// <summary>
    /// 注释：ReqVerifPayInfoDto
    /// 作者：zhanglb
    /// 日期：2018/5/15 17:40:47
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqVerifPayInfoDto
    {
        public int AccountType { get; set; }
        public string OldAccountName { get; set; }
        public string NewAccountName { get; set; }
    }
}
