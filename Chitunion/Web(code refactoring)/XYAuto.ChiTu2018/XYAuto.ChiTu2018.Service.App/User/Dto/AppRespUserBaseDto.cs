/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.User.Dto
* 类 名 称 ：RespUserBaseDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 19:49:31
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.App.User.Dto
{
    public class AppRespUserBaseDto
    {
        public AppRespMobileDto BasicInfo { get; set; }
        public AppRespUserDto AuthenticationInfo { get; set; }
        public AppRespBankAccountDto BankAccountInfo { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppRespMobileDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppRespBankAccountDto
    {
        public string AccountName { get; set; } = string.Empty;
        public int AccountType { get; set; } = -2;
    }
}
