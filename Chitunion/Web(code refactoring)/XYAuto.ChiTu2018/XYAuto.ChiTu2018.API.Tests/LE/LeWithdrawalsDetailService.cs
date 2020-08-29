/**
*
*创建人：lixiong
*创建时间：2018/5/9 20:47:33
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Entities.Extend.LE;
using XYAuto.ChiTu2018.Entities.Query;
using XYAuto.ChiTu2018.Infrastructure.Exceptions;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App;
using XYAuto.ChiTu2018.Service.App.AppInfo.Provider;
using XYAuto.ChiTu2018.Service.LE.Provider;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto;
using XYAuto.ChiTu2018.Service.LE.Provider.Dto.Request.Withdrawals;
using XYAuto.ChiTu2018.Service.LE.Query.Dto.Request;

namespace XYAuto.ChiTu2018.API.Tests.LE
{
    [TestClass]
    public class LeWithdrawalsDetailService : BaseTest
    {
        public LeWithdrawalsDetailService()
        {
            EntityFrameworkProfiler.Initialize();
        }

        [TestMethod]
        public void PriceCalc()
        {
            var info = new WxWithdrawalsProvider(new ConfigEntity() { UserId = 1613 }, new ReqWithdrawalsDto()
            {
                WithdrawalsPrice = 50
            }).PriceCalc();
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("测试-提现申请")]
        [TestMethod]
        public void Withdrawals()
        {
            var retValue = new WxWithdrawalsProvider(new ConfigEntity() { UserId = 1661 }, new ReqWithdrawalsDto()
            {
                Ip = "192.1.1.0",
                ApplySource = WithdrawalsApplySourceEnum.Pc,
                Mobile = "",
                WithdrawalsPrice = 50
            }).Withdrawals();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("测试-提现申请-校验逻辑")]
        [TestMethod]
        public void VerifyWithdrawals()
        {
            var retValue = new Service.App.PublicService.PsVerifyWithdrawlsService().VerifyWithdrawals(1632, "", 50);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.ResultCode == 0);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("测试-测试可空类型")]
        [TestMethod]
        public void GetIncomeInfo()
        {
            var info = Service.LE.LeWithdrawalsStatisticsService.Instance.GetIncomeInfo(1661);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }
        [TestMethod]
        public void GetIncomeWithdrawalsQuery()
        {
            var query = new ReqInComeDto
            {
                UserId = 1632,
                //PayType = 195001,
                //StartDate = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd"),
                //EndDate = DateTime.Now.ToString("yyyy-MM-dd")
            };
            var list = Service.LE.LeWithdrawalsDetailService.Instance.GetIncomeWithdrawalsQuery(query);
            Console.WriteLine(JsonConvert.SerializeObject(list));
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(query));
        }

        [TestMethod]
        public void PostWithdrawas()
        {
            var tp = Service.App.AppInfo.LeWithdrawalsDetailService.Instance.PostWithdrawas(new LE_WithdrawalsDetail
            {
                WithdrawalsPrice = 100, //提现金额
                IndividualTaxPeice = 100, //个税金额
                PracticalPrice = 100, //实际付款
                PayeeAccount = "lx501361941@qq.com",
                Status = (int)WithdrawalsStatusEnum.支付中,
                ApplicationDate = DateTime.Now,
                PayeeID = 1295,
                CreateTime = DateTime.Now,
                AuditStatus = (int)WithdrawalsAuditStatusEnum.待审核,
                ApplySource = (int)1,
                IsActive = 1,
                OrderID = 0,
                SyncPayStatus = 0
            });
            var query = JsonConvert.SerializeObject(tp);
        }
        [TestMethod]
        [System.ComponentModel.Description("app 提现申请")]
        public void PostWithdrawasForApp()
        {
            var dto = new Service.App.AppInfo.Dto.Request.Withdrawals.ReqWithdrawalsDto()
            {
                WithdrawalsPrice = 150,
                ApplySource = WithdrawalsApplySourceEnum.Android
            };
            var retValue = new AppWithdrawalsProvider(new ConfigEntity()
            {
                UserId = 1613,
                Ip = "127.0.0.1"
            }, dto).Withdrawals();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
                Service.App.AppInfo.LeWithdrawalsDetailService.Instance.PostWithdrawasTest();
            }
            catch (PostWithdrawasException exception)
            {
                Console.WriteLine("message:" + exception.Message);
                Console.WriteLine("StackTrace:" + exception.StackTrace ?? string.Empty);
            }

        }
        [TestMethod]
        public void OptionBootStarp()
        {

            //var load = AppDomain.CurrentDomain.GetAssemblies();
            //Assembly.GetEntryAssembly().GetReferencedAssemblies();
            //var assemblies =
            //        from file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
            //        where Path.GetExtension(file) == ".dll"
            //        select Assembly.LoadFrom(file);

            var binDirectory = String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath) ? AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.RelativeSearchPath;

            var assemblies1 = from file in Directory.GetFiles(binDirectory)
                              where Path.GetExtension(file) == ".dll"
                              select Assembly.LoadFrom(file);

            var assemblies = assemblies1.Where(n => n.FullName.StartsWith("XYAuto.ChiTu2018"));

            //var allAssemblies = (from name in Assembly.GetEntryAssembly().GetReferencedAssemblies()
            //                     select Assembly.Load(name)).ToList();
            //var assembles = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(n => n.FullName.StartsWith("XYAuto.ChiTu2018"));
            OptionBootStarp boot = new OptionBootStarp(assemblies);
            boot.Initialize();

            //todo test
            var info = Service.App.AppInfo.LeWithdrawalsDetailService.Instance.GetById(1);
            Console.WriteLine(JsonConvert.SerializeObject(info));

        }
        private void LoadReferencedAssembly(Assembly assembly)
        {
            foreach (AssemblyName name in assembly.GetReferencedAssemblies())
            {
                if (!AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == name.FullName))
                {
                    this.LoadReferencedAssembly(Assembly.Load(name));
                }
            }
        }
    }

}
