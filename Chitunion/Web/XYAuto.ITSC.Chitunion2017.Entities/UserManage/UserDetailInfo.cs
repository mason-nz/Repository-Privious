using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.UserManage
{
    public enum UserDetailStatus
    {
        未认证=0,
        待审核,
        已认证,
        认证未通过
    }
    public class UserDetailInfo
    {
        public int UserID { get; set; } = -2;
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
        public int Status { get; set; } = -2;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public int CreateUserID { get; set; } = -2;
        public DateTime LastUpdateTime { get; set; } = new DateTime(1900, 1, 1);
        public string IdentityNo { get; set; } = string.Empty;
    }
}
