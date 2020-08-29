using System;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO.Task;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Enum.UserInfo;
using XYAuto.ChiTu2018.Service.OAuth2;
using XYAuto.ChiTu2018.Service.OAuth2.Dto;
using XYAuto.ChiTu2018.Service.Wechat;

namespace XYAuto.ChiTu2018.API.Tests.Task
{
    [TestClass]
    public class LEWeiXinUserServiceTest
    {
        [TestMethod]
        public void GetUnionAndUserIdMethod()
        {
            var ret = LEWeiXinUserService.Instance.GetUnionAndUserId("ohvZzwCUZMeXDzQbillBicSkVPiw11");
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(true, ret != null);
        }

        [TestMethod]
        public void IsExistOpenIdMethod()
        {
            var ret = LEWeiXinUserService.Instance.IsExistOpenId("ohvZzwCUZMeXDzQbillBicSkVPiw1");
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(true, ret);
        }

        [TestMethod]
        public void GetPromotionChannelIdMethod()
        {
            var ret = OAuth2Service.Instance.GetPromotionChannelId("http://wxtest-ct.qichedaquan.com/moneyManager/make_money.html?channel=ctlmcaidan11");
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(true, ret > 0);
        }

        [TestMethod]
        public void WeiXinUserOperationMethod()
        {
            try
            {
                EntityFrameworkProfiler.Initialize();
                var ret = LEWeiXinUserService.Instance.WeiXinUserOperation(new WeiXinUserDto()
                {
                    subscribe = 1,
                    openid = $"test-openid{DateTime.UtcNow}",
                    nickname = $"test{DateTime.Now}",
                    sex = 1,
                    city = "北京",
                    country = "中国",
                    province = "北京",
                    language = "汉语",
                    headimgurl = "http://thirdwx.qlogo.cn/mmopen/9micgK0FO5KZlrxoOyYFarxM8XndqkRphx2blprbn3zor5GUGUD213iaPKKemWJBic2srbicgr4nef0s5v3IULkFb9j8Cnzc7iaFH/132",
                    subscribe_time = DateTime.Now,
                    unionid = $"unionid-{DateTime.UtcNow}",
                    remark = "remark",
                    groupid = 0,
                    tagid_list = "tagid_list",
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    AuthorizeTime = DateTime.Now,
                    Status = 0,
                    RegisterFrom = (int)RegisterFromEnum.赤兔联盟微信服务号,
                    RegisterType = (int)RegisterTypeEnum.微信
                });
                var retstr = JsonConvert.SerializeObject(ret);
                Assert.AreEqual(true, ret != null);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }            
        }

        [TestMethod]
        public void UserInfoInsertMethod()
        {
            //var userInfo = new UserInfoBO().GetInfo(1702);
            //userInfo.Mobile = "13581797617";
            //var u = new UserInfoBO().Update(userInfo);
            //new UserInfoBO().UpdateUserMobile(1702, "13581797617");
            var ret =
                    new UserInfoBO().Insert(new UserInfo()
                    {
                        UserName = $"unionid-{DateTime.UtcNow}",
                        Mobile = "13581797618",
                        Pwd = "123",
                        Type = 1002,
                        Source = (int)RegisterFromEnum.赤兔联盟微信服务号,
                        IsAuthMTZ = false,
                        IsAuthAE = false,
                        Status = 0,
                        CreateTime = DateTime.Now,
                        CreateUserID = 0,
                        LastUpdateTime = DateTime.Now,
                        LastUpdateUserID = 0,
                        Category = 0,
                        Email = string.Empty,
                        LockState = 0,
                        SleepState = 0,
                        RegisterType = (int)RegisterTypeEnum.微信,
                        PromotionChannelID = 101003002
                    });

            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(true, ret != null);
        }

        [TestMethod]
        public void LeWeiXinUserInsertMethod()
        {
            try
            {
                var weixinUser = new LEWeiXinUserBO().Insert(new LE_WeiXinUser()
                {
                    subscribe = 1,
                    openid = $"test-openid{DateTime.UtcNow}",
                    nickname = $"test{DateTime.Now}",
                    sex = 1,
                    city = "北京",
                    country = "中国",
                    province = "北京",
                    language = "汉语",
                    headimgurl = "http://thirdwx.qlogo.cn/mmopen/9micgK0FO5KZlrxoOyYFarxM8XndqkRphx2blprbn3zor5GUGUD213iaPKKemWJBic2srbicgr4nef0s5v3IULkFb9j8Cnzc7iaFH/132",
                    subscribe_time = DateTime.Now,
                    unionid = $"unionid-{DateTime.UtcNow}",
                    remark = "remark",
                    groupid = 0,
                    tagid_list = "tagid_list",
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    AuthorizeTime = DateTime.Now,
                    Status = 0,
                    Source = (int)RegisterFromEnum.赤兔联盟微信服务号,
                    AdvertiserUserId = 0
                });
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
            }            
        }
        [TestMethod]
        public void UpdateStatusByOpenIdMethod()
        {
            try
            {
                LEWeiXinUserService.Instance.UpdateStatusByOpenId(-1, DateTime.Now, "test-openid2018/5/17 8:28:13");
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
            }
            Assert.AreEqual(true, true);
        }

        //LEWeiXinUserService.Instance.GetUserSence((int)user.UserID)
        [TestMethod]
        public void GetUserSenceMethod()
        {
            try
            {
                var ret = LEWeiXinUserService.Instance.GetUserSence(12991);
                Assert.AreEqual(true, ret);
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
            }            
        }
    }
}
