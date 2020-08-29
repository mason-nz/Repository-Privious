using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class AuthorizeTest
    {
        private AuthorizeController ctl = new AuthorizeController();
        private XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult ret = null;
        [TestMethod]
        public void Test()
        {
            ret = ctl.GetTempTokenFromOtherSys(1, "encyptStr");
            int appid = 1;
            string accessToken = "83ed48d10a3c9dfa05db4c57479926f7";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("UserId", "1300037");
            //dict.Add("EnterpriseName", "企业名称");
            //dict.Add("Contact", "张三");
            //dict.Add("Mobile", "15800000000");

            dict.Add("UserId", "301345");
            dict.Add("EnterpriseName", "会员:100038791");
            dict.Add("Contact", "100412422");
            dict.Add("Mobile", "12223301345");
            dict.Add("businessLicence", "http://192.168.3.51:8003/BusinessLicense/UploadFiles/BLImages/2011/1/5/17/418€5314a4bb-603e-4927-ad5f-5ba88d108ce4.jpg|http://192.168.3.51:8003/BusinessLicense/UploadFiles/BLImages/2017/5/9/18/dc3d5a53-376a-4e23-8099-112f5769656a.jpg");

            string EmbedChiTu_DesStr = "1234567890.abc";
            string json = JsonConvert.SerializeObject(dict);
            string p = XYAuto.Utils.Security.DESEncryptor.Encrypt(JsonConvert.SerializeObject(dict), "1234567890.abc");
            p = HttpUtility.UrlEncode(p);
            string c = "ilcUCTRLx19j8ikx75v9aHNxgvCzXbt%252bJT7HMXmE0CB2QdWaWMF0lz1TvHx3fyWO6o7W7kPDEHybJMMcJhFisPbF4nVoSb7lEFjk6fB0kKwWv3tg5bEG%252bl5iHbtpT8t%252b7v5jTVzQN8wdbWd77lpuomLy6uAY90LhzFQFbCLGIP74TG1ZWzIcgxE81wX7w%252bc0u1xnM7cDx8TFIEQG1P5iCnaCwjR7jZWiA%252bwhRZ1LC0G5Di1eleIhAAEK6FY27%252bQMrXCXh%252ftlmdEqGcTLLNQ61g1zp%252fpxZw%252fi%252fmoCtW54xinvZF5z1Z2lrxfKtzo3vYAUT4WbRC1kJdv09UGFTkkAlQ%253d%253d";
            string a = "ilcUCTRLx19gk1pOla7LWrZkYKhdJmGvfO+Aa4EBCPMzba8QcFbRgxpiPdrYJ/cL4RqUTUWrlF9jeT+iTeTlr7ujNIvJucfXZ8HdydsmJ/4r7KnC/LOCsE4Qn+FSK+HIsjBSyqQHLVTh52Zhe4S2++sh5pklMQ67P4JeUl5vT1ApyEppqvQrgS2tbEPLxpuUVf9U1e0cfbIyoY7XugYieaddov4kf9JbWL/fL+77B3g+SbS0+wl8Wf62yPTK2Hy6puQexAJC1cZrvQgJGtwIeQAA06v56baKd4npl+vAiyfeZcbnaaiXNZkGHg6QQDdBwJr4JRO7Dvu8w7vVe6icBllLDUohXOZS/b6bXl/XGwLZNVnReYEeYK1GcaetUy1ZLH+6m+6AAyWSyE3EdCqwi0UwZDf+Ldpjpu8Jw6+LZuYXlZpB6lqOKDX6R+cCHL4ckahlQ7MDovEnVlj8kHs+6HvVNhm9nOI5";
            string b = HttpUtility.UrlEncode(a);
            c = HttpUtility.UrlDecode(a);
            string sign = XYAuto.Utils.Security.DESEncryptor.MD5Hash(appid + accessToken + p + EmbedChiTu_DesStr);
            sign = XYAuto.Utils.Security.DESEncryptor.MD5Hash("15926fabfb766f43ec5dbba7b0d632af1cqxOiml9QZs+bwlnY+T1ojDNOpkwcfOIHQ9+hoodWUuZNM+I8bNywS+d/W3mbLCYmOTzPvlw3YZUyaVYxGcImLp3uGIWsAc/IDaB7AZg5LazqnPRyqIa0L4dS9IlhYiEWaYER5xZWyYfXgQ27iQrWg==8A40B180-5E3A-41EA-9EC6-4BE8F810C961",Encoding.UTF8);
            AuthorizeController test = new AuthorizeController();
            //test.SyncLoginFromOtherSys(p, sign, appid, accessToken);
        }

        [TestMethod]
        public void MD5HashTest()
        {
            string appid = "1";
            string accessToken = "339a6eea1ea44e742b342ed93d4e7bec";
            string p = "VUovimSU63zL49mzNRmc5bSHadlRZWpp6tC6UTu4Eud6nLxwLKrwHDX7JHNlHFiFO0vKce7XQNQ3Y3ZqSkOLJiyWjlgyYbWCcRjyKA9ECqZYj0FePEN9Nz3ildt9vdeg";
            string EmbedChiTu_DesStr = "1234567890.abc";
            string sign = XYAuto.Utils.Security.DESEncryptor.MD5Hash(appid + accessToken + p + EmbedChiTu_DesStr);
            AuthorizeController test = new AuthorizeController();
            //test.SyncLoginFromOtherSys(p, sign, appid, accessToken);
        }

        [TestMethod]
        public void GetTempTokenFromOtherSysTest()
        {
            ret = ctl.GetTempTokenFromOtherSys(1, "encyptStr");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            List<Dictionary<string, string>> dictList = new List<Dictionary<string, string>>();
            dictList = (List < Dictionary < string, string>>) ret.Result;
            //var obj = JsonConvert.DeserializeObject<dynamic>(ret.Result.ToString());
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void SyncLoginFromOtherSysTest()
        {
            ret = ctl.SyncLoginFromOtherSys("3743d048b02d5aa16e104fbe1dc920b6", 1001,"13500000001");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
    }
}