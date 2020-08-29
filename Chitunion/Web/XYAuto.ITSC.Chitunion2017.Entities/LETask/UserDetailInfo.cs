using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //用户详细信息
    public class UserDetailInfo
    {

        public int UserID { get; set; }

        //个人真实姓名或公司名称
        public string TrueName { get; set; }

        //行业枚举
        public int BusinessID { get; set; }

        //省份ID
        public int ProvinceID { get; set; }

        //城市ID
        public int CityID { get; set; }

        //区县ID
        public int CounntyID { get; set; }

        //联系人
        public string Contact { get; set; }

        //联系地址
        public string Address { get; set; }

        //营业执照上传图片URL地址
        public string BLicenceURL { get; set; }

        //法人身份证正面URL
        public string IDCardFrontURL { get; set; }

        //法人身份证反面URL
        public string IDCardBackURL { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int LastUpdateUserID { get; set; }

        //组织机构代码图片URL地址
        public string OrganizationURL { get; set; }

        //0-正常，2-媒体审核通过后，已同步
        public int Status { get; set; }

        public string IdentityNo { get; set; }

        public string Reason { get; set; }

        public string Mobile { get; set; }
        public int Type { get; set; }

    }
}