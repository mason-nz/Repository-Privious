using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.User.Dto
{
    /// <summary>
    /// 注释：RespUserBasicInfoDto
    /// 作者：zhanglb
    /// 日期：2018/5/15 14:37:17
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespUserBasicInfoDto
    {
        public RespMobileDto BasicInfo { get; set; }
        public RespUserDetailDto AuthenticationInfo { get; set; }
        public RespBankAccountDto BankAccountInfo { get; set; }
    }
    /// <summary>
    /// 注释：RespMobileDto
    /// 作者：zhanglb
    /// 日期：2018/5/15 14:31:00
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespMobileDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
    }
    /// <summary>
    /// 注释：RespBankAccountDto
    /// 作者：zhanglb
    /// 日期：2018/5/15 14:35:58
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class RespBankAccountDto
    {
        public string AccountName { get; set; } = string.Empty;
        public int AccountType { get; set; } = -2;
    }
}
