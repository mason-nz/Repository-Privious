using System;
using System.Collections.Generic;
using System.Linq;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Service;
using XYAuto.ChiTu2018.Service.LE;
using XYAuto.ChiTu2018.Service.Task.Dto.GetOrderInfo;
using XYAuto.CTUtils.Sys;

namespace XYAuto.ChiTu2018.API.Tests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            // Initialize the profiler
            EntityFrameworkProfiler.Initialize();

            // You can also use the profiler in an offline manner.
            // This will generate a file with a snapshot of all the EntityFramework activity in the application,
            // which you can use for later analysis by loading the file into the profiler.
            var filename = @"D:\profiler-log";
            EntityFrameworkProfiler.InitializeOfflineProfiling(filename);

            // You can use the following for production profiling.
            //EntityFrameworkProfiler.InitializeForProduction(11234, "A strong password like: ze38r/b2ulve2HLQB8NK5AYig");
        }

        [Description("测试-多表join查询")]
        [TestMethod]
        public void GetJoinQuerys()
        {
            //var list = LeWithdrawalsDetailService.Instance.GetJoinQuerys();
            //Console.WriteLine(JsonConvert.SerializeObject(list));
            //Assert.IsTrue(list.Any());
        }

        [Description("测试-查询")]
        [TestMethod]
        public void GetById()
        {
            var info = LeWithdrawalsDetailService.Instance.GetById(24);
            Console.WriteLine("INFO:" + JsonConvert.SerializeObject(info));
           /// Console.WriteLine("LeDisbursementPays:" + JsonConvert.SerializeObject(info.LeDisbursementPays));
        }

        [Description("测试-update部分字段")]
        [TestMethod]
        public void UpdateStatus()
        {
            var recId = 24;
            LeWithdrawalsDetailService.Instance.UpdateStatus(recId);
            var info = LeWithdrawalsDetailService.Instance.GetById(recId);

            Assert.IsTrue(info.IsActive == 0);
        }

        [Description("测试-分页")]
        [TestMethod]
        public void GetPageList()
        {
            //var query = new QueryPageBase<LE_WithdrawalsDetail> { };
            //var list = LeWithdrawalsDetailService.Instance.GetPageList(new QueryPageBase<LE_WithdrawalsDetail> { });
            //Console.WriteLine(JsonConvert.SerializeObject(query.Total));
            //Console.WriteLine(JsonConvert.SerializeObject(query.DataList));

            Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                {"userinfo","/userManager/userInfo.html"},
                {"userManager","/userManager/userInfo.html"}
            };
            Console.WriteLine(JsonConvert.SerializeObject(dic));
            List<dynamic> list = new List<dynamic>()
            {
                new { key = "userinfo",value = "/userManager/userInfo.html"},
                new { key = "userManager",value = "/userManager/userInfo.html"}
            };
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void IntNullable()
        {
            int? value = new int?();

            Console.WriteLine(value - 10);
            Console.WriteLine(100 - value.GetValueOrDefault(0) - 10);

            var dt = new DateTime();

            Console.WriteLine(dt == default(DateTime));

        }
        [TestMethod]
        public void GenerateRandomCode()
        {
            var forCount = 100000;
            var list = new List<string>();
            for (var i = 0; i < forCount; i++)
            {
                list.Add(XYAuto.CTUtils.Sys.RandomHelper.GenerateRandomCode(10, GenerateRandomType.LetterAndNum));
            }
            //循环forCount次，去重复，对比
            var count = list.Distinct().ToList().Count;
            Assert.AreEqual(forCount, count);
        }

        [TestMethod]
        public void GenerateRandomCode1()
        {
            var forCount = 1000000;
            var list = new List<string>();
            for (var i = 0; i < forCount; i++)
            {
                list.Add(XYAuto.CTUtils.Sys.RandomHelper.GenerateRandomCode(5, GenerateRandomType.Num));
            }
            //循环forCount次，去重复，对比
            var count = list.Distinct().ToList().Count;
            Assert.AreEqual(forCount, count);
        }
    }
}
