using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.Entities.User.Dto;
using XYAuto.ChiTu2018.Service.User.Dto;
using XYAuto.ChiTu2018.Service.UserBankAccount.Dto;

namespace XYAuto.ChiTu2018.API.Tests.User
{
    /// <summary>
    /// 注释：UserManageService
    /// 作者：zhanglb
    /// 日期：2018/5/17 10:25:36
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class UserManageService
    {
        /// <summary>
        /// 获取手机号状态
        /// </summary>
        [TestMethod]
        public void GetMobileStatus()
        {
            string errorMsg;

            Console.WriteLine("手机号为空");
            var dic1 = Service.User.UserManageService.Instance.GetMobileStatus("", out errorMsg);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic1));
            Console.WriteLine("\r\n");

            Console.WriteLine("手机号格式错误");
            var dic2 = Service.User.UserManageService.Instance.GetMobileStatus("1565", out errorMsg);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic2));
            Console.WriteLine("\r\n");

            Console.WriteLine("手机号正确");
            var dic3 = Service.User.UserManageService.Instance.GetMobileStatus("13717519733", out errorMsg);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic3));
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 根据手机号查询用户关联信息
        /// </summary>
        [TestMethod]
        public void GetUserRelatedInfoByMobile()
        {
            string errorMsg;

            Console.WriteLine("手机号为空");
            var dic1 = Service.User.UserManageService.Instance.GetUserRelatedInfoByMobile("", out errorMsg);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic1));
            Console.WriteLine("\r\n");

            Console.WriteLine("手机号不存在");
            var dic2 = Service.User.UserManageService.Instance.GetUserRelatedInfoByMobile("1565", out errorMsg);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic2));
            Console.WriteLine("\r\n");

            Console.WriteLine("手机号正确");
            var dic3 = Service.User.UserManageService.Instance.GetUserRelatedInfoByMobile("13717519733", out errorMsg);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic3));
            Console.WriteLine("\r\n");

        }
        /// <summary>
        /// 查询个人关联信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetUserRelatedInfo1()
        {
            var dic1 = Service.User.UserManageService.Instance.GetUserRelatedInfo();
            Console.WriteLine(JsonConvert.SerializeObject(dic1));
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 查询个人认证信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void GetUserDetail()
        {
            var dic1 = Service.User.UserManageService.Instance.GetUserDetail();
            Console.WriteLine(JsonConvert.SerializeObject(dic1));
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 保存保存支付账号
        /// </summary>
        [TestMethod]
        public void SavePayInfo()
        {
            //Console.WriteLine("保存支付宝：账号格式错误");
            //var dto1 = new ReqPayInfoDto()
            //{

            //    IsAdd = true,
            //    AccountName = "165465",
            //    AccountType = 96001
            //};

            //var dic1 = Service.UserBankAccount.UserBankAccountService.Instance.SavePayInfo(dto1);
            //Console.WriteLine(JsonConvert.SerializeObject(dic1));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("保存支付宝：账号格式正确:账号存在");
            //var dto2 = new ReqPayInfoDto()
            //{

            //    IsAdd = true,
            //    AccountName = "13717519733",
            //    AccountType = 96001
            //};

            //var dic2 = Service.UserBankAccount.UserBankAccountService.Instance.SavePayInfo(dto2);
            //Console.WriteLine(JsonConvert.SerializeObject(dic2));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("保存支付宝：账号格式正确:账号不存在");
            //var dto3 = new ReqPayInfoDto()
            //{

            //    IsAdd = true,
            //    AccountName = "13717519338",
            //    AccountType = 96001
            //};

            //var dic3 = Service.UserBankAccount.UserBankAccountService.Instance.SavePayInfo(dto3);
            //Console.WriteLine(JsonConvert.SerializeObject(dic3));
            //Console.WriteLine("\r\n");

            Console.WriteLine("修改支付宝");
            var dto4 = new ReqPayInfoDto()
            {

                OldAccountName = "13717519338",
                OldAccountType = 96001,
                AccountName = "13717519743",
                AccountType = 96001
            };

            var dic4 = Service.UserBankAccount.UserBankAccountService.Instance.SavePayInfo(dto4);
            Console.WriteLine(JsonConvert.SerializeObject(dic4));
            Console.WriteLine("\r\n");
        }
        // todo:重测场景
        /// <summary>
        /// 保存认证信息GetUserDetail
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void SaveUserDetail()
        {
            //Console.WriteLine("Type错误");
            //var dto1 = new RespUserDetailDto()
            //{
            //    Type = 1,
            //    TrueName = "TrueName",
            //    IdentityNo = "IdentityNo",
            //    IdCardFrontUrl = "IdCardFrontUrl",
            //    IdCardBackUrl = "IdCardBackUrl",
            //    BLicenceUrl = "BLicenceUrl",
            //    Reason = "Reason"
            //};
            //var dic1 = Service.User.UserManageService.Instance.SaveUserDetail(dto1);
            //Console.WriteLine(JsonConvert.SerializeObject(dic1));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("企业：正确格式");
            //var dto2 = new RespUserDetailDto()
            //{
            //    Type = 1001,
            //    TrueName = "TrueName",
            //    IdentityNo = "IdentityNo",
            //    IdCardFrontUrl = "IdCardFrontUrl",
            //    IdCardBackUrl = "IdCardBackUrl",
            //    BLicenceUrl = "BLicenceUrl",
            //    Reason = "Reason"
            //};
            //var dic2 = Service.User.UserManageService.Instance.SaveUserDetail(dto2);
            //Console.WriteLine(JsonConvert.SerializeObject(dic2));
            //Console.WriteLine("\r\n");


            Console.WriteLine("个人：正确格式");
            var dto3 = new RespUserDetailDto()
            {
                Type = 1002,
                TrueName = "TrueName",
                IdentityNo = "130533199301065356",
                IdCardFrontUrl = "IdCardFrontUrl",
                IdCardBackUrl = "IdCardBackUrl",
                BLicenceUrl = "BLicenceUrl",
                Reason = "Reason"
            };
            var dic3 = Service.User.UserManageService.Instance.SaveUserDetail(dto3);
            Console.WriteLine(JsonConvert.SerializeObject(dic3));
            Console.WriteLine("\r\n");

        }
        /// <summary>
        /// 发送或校验短信验证码||保存手机号
        /// </summary>
        [TestMethod]
        public void SmsValidateCode()
        {
            //    注册发送验证码 = 208001,
            //修改发送验证码 = 208002,
            //提现发送验证码 = 208003,
            //修改验证验证码 = 208004,
            //保存验证验证码 = 208005
            //Console.WriteLine("SmsAction错误");
            //var dto1 = new ReqMobileInfoDto()
            //{
            //    SmsAction = 45,
            //    Mobile = "13717519733",
            //    CheckCode = 5797
            //};
            //int error1 = Service.User.UserManageService.Instance.SmsValidateCode(dto1);
            //Console.WriteLine(JsonConvert.SerializeObject(error1));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("发送手机号格式错误");
            //var dto2= new ReqMobileInfoDto()
            //{
            //    SmsAction = 208001,
            //    Mobile = "1371719733",
            //    CheckCode = 5797
            //};
            //int error2 = Service.User.UserManageService.Instance.SmsValidateCode(dto2);
            //Console.WriteLine(JsonConvert.SerializeObject(error2));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("注册发送验证码");
            //var dto3= new ReqMobileInfoDto()
            //{
            //    SmsAction = 208001,
            //    Mobile = "13717519733",
            //    CheckCode = 5797
            //};
            //int error3 = Service.User.UserManageService.Instance.SmsValidateCode(dto3);
            //Console.WriteLine(JsonConvert.SerializeObject(error3));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("修改发送验证码");
            //var dto4 = new ReqMobileInfoDto()
            //{
            //    SmsAction = 208002,
            //    Mobile = "13717519733",
            //    CheckCode = 5797
            //};
            //int error4= Service.User.UserManageService.Instance.SmsValidateCode(dto4);
            //Console.WriteLine(JsonConvert.SerializeObject(error4));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("提现发送验证码");
            //var dto5 = new ReqMobileInfoDto()
            //{
            //    SmsAction = 208003,
            //    Mobile = "13717519733",
            //    CheckCode = 5797
            //};
            //int error5 = Service.User.UserManageService.Instance.SmsValidateCode(dto5);
            //Console.WriteLine(JsonConvert.SerializeObject(error5));
            //Console.WriteLine("\r\n");

            //Console.WriteLine("修改验证验证码");
            //var dto5 = new ReqMobileInfoDto()
            //{
            //    SmsAction = 208004,
            //    Mobile = "13717519733",
            //    CheckCode = 5797
            //};
            //int error5 = Service.User.UserManageService.Instance.SmsValidateCode(dto5);
            //Console.WriteLine(JsonConvert.SerializeObject(error5));
            //Console.WriteLine("\r\n");


            Console.WriteLine("保存验证验证码");
            var dto5 = new ReqMobileInfoDto()
            {
                SmsAction = 208005,
                Mobile = "13717519737",
                CheckCode = 5797
            };
            int error5 = Service.User.UserManageService.Instance.SmsValidateCode(dto5);
            Console.WriteLine(JsonConvert.SerializeObject(error5));
            Console.WriteLine("\r\n");
        }
        /// <summary>
        /// 查询用户关联信息
        /// </summary>
        [TestMethod]
        public void GetUserRelatedInfo()
        {
            string errorMsg;

            Console.WriteLine("用户ID存在");
            var dic2 = Service.User.UserManageService.Instance.GetUserRelatedInfoByMobile("", out errorMsg, 1645);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic2));
            Console.WriteLine("\r\n");

            Console.WriteLine("用户ID不存在");
            var dic1 = Service.User.UserManageService.Instance.GetUserRelatedInfoByMobile("", out errorMsg, 5);
            Console.WriteLine(errorMsg);
            Console.WriteLine(JsonConvert.SerializeObject(dic1));
            Console.WriteLine("\r\n");
        }
    }
}
