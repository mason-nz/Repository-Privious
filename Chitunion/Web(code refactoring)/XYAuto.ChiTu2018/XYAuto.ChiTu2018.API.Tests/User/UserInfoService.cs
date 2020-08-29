using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.BO.User;

namespace XYAuto.ChiTu2018.API.Tests.User
{
    /// <summary>
    /// 注释：UserInfoService
    /// 作者：lix
    /// 日期：2018/5/11 9:48:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class UserInfoService
    {
        public UserInfoService()
        {
            EntityFrameworkProfiler.Initialize();
        }

        [TestMethod]
        public void GetUserInfo()
        {
            var info = Service.User.UserInfoService.Instance.GetUserInfo(1661);
            Console.WriteLine(JsonConvert.SerializeObject(info));
            Console.WriteLine(JsonConvert.SerializeObject(info.UserDetailInfo));
            //Assert.IsNull(info);
        }
    }
}
