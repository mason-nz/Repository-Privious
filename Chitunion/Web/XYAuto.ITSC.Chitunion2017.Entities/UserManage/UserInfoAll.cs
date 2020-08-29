using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.UserManage
{
    public class UserInfoAll
    {
        public int UserID { get; set; } = -2;
        public string UserName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Pwd { get; set; } = string.Empty;
        public int Type { get; set; } = -2;
        public int Category { get; set; } = -2;
        public int Source { get; set; } = -2;
        public bool IsAuthMTZ { get; set; } = false;
        public int AuthAEUserID { get; set; } = -2;
        public bool IsAuthAE { get; set; } = false;
        public int SysUserID { get; set; } = -2;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
        public DateTime LastUpdateTime { get; set; } = new DateTime(1900, 1, 1);
        public int LastUpdateUserID { get; set; } = -2;
        public string IdentityNo { get; set; } = string.Empty;

        public string TrueName { get; set; } = string.Empty;
        public int BusinessID { get; set; } = -2;
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public int CounntyID { get; set; } = -2;
        public string Contact { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string BLicenceURL { get; set; } = string.Empty;
        public string OrganizationURL { get; set; } = string.Empty;
        public string IDCardFrontURL { get; set; } = string.Empty;
        public string IDCardBackURL { get; set; } = string.Empty;
        public int UDStatus { get; set; } = -2;
        public int Sex { get; set; } = -2;
        public string AccountName { get; set; } = string.Empty;
        public int AccountType { get; set; } = -2;
    }
}
