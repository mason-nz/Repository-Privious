using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto
{
    public class ResQueryUserBasicInfoDto
    {
        public ResBasicinfoDto BasicInfo { get; set; }
        public ResAuthenticationinfoDto AuthenticationInfo { get; set; }
        public ResBankaccountinfoDto BankAccountInfo { get; set; }
    }

    public class ResBasicinfoDto
    {
        public int Category { get; set; } = -2;
        public string UserName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public string Address { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public int RegisterType { get; set; }
    }

    public class ResAuthenticationinfoDto
    {
        public int Type { get; set; } = -2;
        public string TrueName { get; set; } = string.Empty;
        public string IdentityNo { get; set; } = string.Empty;
        public string IDCardFrontURL { get; set; } = string.Empty;
        public string IDCardBackURL { get; set; } = string.Empty;
        public string BLicenceURL { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public string Reason { get; set; } = string.Empty;
        public int Sex { get; set; }
    }

    public class ResBankaccountinfoDto
    {
        public string AccountName { get; set; } = string.Empty;
        public int AccountType { get; set; } = -2;
    }

}
