/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 18:56:34
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_2
{
    [TestClass]
    public class LogicTransferProviderTest
    {
        private readonly LogicTransferProvider _logicTransferProvider;

        public LogicTransferProviderTest()
        {
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
            _logicTransferProvider = new LogicTransferProvider();
        }

        [TestMethod]
        public void PushUser_Test()
        {
            var retValue = _logicTransferProvider.PushUser(new RequestPushUserDto()
            {
                ContactsPerson = "lixiong",
                CorporationName = "xingyuan",
                Mobile = "13113413333",
                OrganizeId = 1,
                UserName = "lixiong"
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void PushDemand_Test()
        {
            var retValue = _logicTransferProvider.PushDemand(new RequestPushDemandDto()
            {
                AreaInfo = new List<AreaInfoDto>()
                {
                    new AreaInfoDto()
                    {
                        ProvinceName = "北京",
                        ProvinceId = 201,
                        City = new List<CityDto>()
                        {
                            new CityDto()
                            {
                                CityId = 20101,
                                CityName = "北京"
                            }
                        }
                    }
                },
                BeginDate = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd"),
                EndDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
                ClueNumber = 4000,
                DayBudget = 2009900,
                DemandBillNo = 11,
                DemandName = "test需求11",
                OrganizeId = 1,
                PromotionPolicy = "促销政策"
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void RechargeReceipt_Test()
        {
            var retValue = _logicTransferProvider.RechargeReceipt(new RequestRechargeReceiptDto()
            {
                Amount = 10,
                DemandBillNo = 11,
                OrganizeId = 1,
                RechargeNumber = "xxxxx00001"
            });
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void GetToZhyReport()
        {
            _logicTransferProvider.GetToZhyReport(new RequestReportByZhyDto()
            {
                DemandBillNo = 11
            });
        }

        [TestMethod]
        public void DateTest()
        {
            var date = DateTime.Now;

            Console.WriteLine(date.ToString("yyyy-MM-dd"));

            Console.WriteLine(date.Date);
            var str = date.ToString("yyyy-MM-dd");

            var dt = DateTime.ParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Console.WriteLine(dt);
            Console.WriteLine(date.Date);
            Console.WriteLine(DateTime.Today);
            Console.WriteLine(date.Day);
            Console.WriteLine((date.Date - DateTime.Now.AddHours(-3).Date).TotalDays);
        }
    }
}