using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.UserBankAccount.Dto
{
    /// <summary>
    /// 注释：ReqPayInfoDto
    /// 作者：zhanglb
    /// 日期：2018/5/15 15:23:53
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqPayInfoDto
    {
        public bool IsAdd { get; set; }
        public string OldAccountName { get; set; }
        public int OldAccountType { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
    }
}
