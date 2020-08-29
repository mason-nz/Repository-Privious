/********************************
* 项目名称 ：XYAuto.ChiTu2018.API.Tests.User
* 类 名 称 ：AppUserServiceTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/24 17:33:52
********************************/
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Entities.Enum.User;
using XYAuto.ChiTu2018.Service.App.User;
using XYAuto.ChiTu2018.Service.App.User.Dto;

namespace XYAuto.ChiTu2018.API.Tests.User
{
    [TestClass]
    public class AppUserServiceTest
    {
        [TestMethod]
        public void AddUserInfo()
        {
            EntityFrameworkProfiler.Initialize();
            string Message = UserService.Instance.AddUserInfo(new ModifyAttestationReqDto
            {
                Type = (int)UserTypeEnum.个人,
                AccountName = "zhang@123.com",
                AccountType = 96001,
                BLicenceURL = "www.baidu.com",
                IDCardFrontURL = "www.baidu.com",
                Mobile = "18518760365",
                IdentityNo = "410581198811289054",
                Sex = 1,
                TrueName = "张"
            }, 3483);
            Console.WriteLine(Message);
        }
    }
}
