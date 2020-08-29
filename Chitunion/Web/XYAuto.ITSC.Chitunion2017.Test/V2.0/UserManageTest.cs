using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.Test.V2._0
{
    [TestClass]
    public class UserManageTest
    {
        private UserMangeController ctl = new UserMangeController();
        [TestMethod]
        public void TestQueryUserBasicInfo()
        {
            var ret = ctl.QueryUserBasicInfo(new BLL.UserManage.Dto.ReqQueryUserBasicInfoDto() { UserID = 1299 });
            string str = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void TestEditUserBasicInfo()
        {
            var ret = ctl.EditUserBasicInfo(new BLL.UserManage.Dto.EditUserBasicInfo.ReqDto() {
                UserID=1299,
                UserName="lihf",
                ProvinceID=9,
                CityID=910,
                Address="IAFJAF"
            });
            string str = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestEditUserAuthenticationInfo()
        {
            var ret = ctl.EditUserAuthenticationInfo(new BLL.UserManage.Dto.EditUserAuthenticationInfo.ReqDto() {
                UserID=1299,
                Type=1002,
                TrueName="李鹤峰",
                BLicenceURL="公司营业执照URL",
                IdentityNo="130989878675654566",
                IDCardFrontURL="身份证照片URL"
            });
            string str = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestEditUserPasswordInfo()
        {
            var ret = ctl.EditUserPasswordInfo(new BLL.UserManage.Dto.EditUserPasswordInfo.ReqDto() {
                UserID = 1299,
                OldPassword = "1111",
                NewPassword = "123456",
                ConfirmPassword= "123456"
            });
            string str = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestEditUserMobileInfo()
        {
            var ret = ctl.EditUserMobileInfo(new BLL.UserManage.Dto.EditUserMobileInfo.ReqDto()
            {
                UserID = 1299,
                Mobile="13581797617",
                ValidateCode="1111"
            });
            string str = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }
    }
}
