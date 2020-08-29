using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ChiTu2018.Service.Wechat;
using Newtonsoft.Json;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using XYAuto.CTUtils.Config;

namespace XYAuto.ChiTu2018.API.Tests.LE
{
    [TestClass]
    public class WechatSignTest
    {
        /// <summary>
        /// 微信签到
        /// </summary>
        [TestMethod]
        public void DaySignMethod()
        {



            EntityFrameworkProfiler.Initialize();
            var query = WechatSignService.Instance.DaySign("172.20.17.6", 1435);

            var JsonText = JsonConvert.SerializeObject(query);
        }
        /// <summary>
        /// 根据年月查询签到日期
        /// </summary>
        [TestMethod]
        public void DaySignListMethod()
        {
            EntityFrameworkProfiler.Initialize();
            var query = WechatSignService.Instance.DaySignList(2018,5,8888);

            var JsonText = JsonConvert.SerializeObject(query);
        }
    }
}
