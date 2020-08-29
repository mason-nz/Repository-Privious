/********************************************************
*创建人：lixiong
*创建时间：2017/8/21 19:07:01
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Support;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_2
{
    [TestClass]
    public class GdtLogicProviderTest
    {
        private readonly GdtLogicProvider _provider;

        public GdtLogicProviderTest()
        {
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
            _provider = new GdtLogicProvider();
        }

        [Description("账户拉取")]
        [TestMethod]
        public void DoPullAccountUserInfo_Test()
        {
            //1308
            var retValue = _provider.DoPullAccountUserInfo(0);
            Assert.IsFalse(retValue.HasError);
        }

        [Description("资金账户信息拉取入库")]
        [TestMethod]
        public void DoPullGetFundsInfo_Test()
        {
            var retValue = _provider.DoPullGetFundsInfo(1308);
            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取今日消耗")]
        [TestMethod]
        public void DoPullGetRealtimeCost_Test()
        {
            var retValue = _provider.DoPullGetRealtimeCost(new ReqReportDto
            {
                AccountId = 1308,
                Date = "2017-08-20",
                Level = GdtLevelTypeEnum.ADVERTISER
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取资金账户日结明细")]
        [TestMethod]
        public void DoPullGetFundStatementsDaily_Test()
        {
            var retValue = _provider.DoPullGetFundStatementsDaily(new ReqFundDto()
            {
                AccountId = 1308,
                FundType = GdtFundTypeEnum.GENERAL_CASH,
                Date = "2017-08-14",
                TradeType = GdtTradeTypeEnum.CHARGE
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取资金账户流水")]
        [TestMethod]
        public void DoPullGetFundStatementsDetailed_Test()
        {
            var retValue = _provider.DoPullGetFundStatementsDetailed(new ReqFundDetaileDto()
            {
                AccountId = 1308,
                FundType = GdtFundTypeEnum.BANK,
                DateRange = new DateRangeDto() { EndDate = "2017-08-11", StartDate = "2017-08-14" },
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description(" 获取广告组列表(因为会修改，所以，必须从头开始拉取数据)")]
        [TestMethod]
        public void DoPullGetAdGroupList_Test()
        {
            var retValue = _provider.DoPullGetAdGroupList(new ReqReportDto()
            {
                AccountId = 100000612,
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description(" 获取推广计划")]
        [TestMethod]
        public void DoPullGetAdCampaignsList_Test()
        {
            var retValue = _provider.DoPullGetAdCampaignsList(new ReqReportDto()
            {
                AccountId = 100000612,
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取日报表")]
        [TestMethod]
        public void DoPullGetReportDaily_Test()
        {
            var retValue = _provider.DoPullGetReportDaily(new ReqReportDto()
            {
                AccountId = 1308,
                DateRange = new DateRangeDto() { EndDate = "2017-08-16", StartDate = "2017-08-13" },
                Level = GdtLevelTypeEnum.ADVERTISER
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取小时报表")]
        [TestMethod]
        public void DoPullGetReportHourly_Test()
        {
            var retValue = _provider.DoPullGetReportHourly(new ReqReportDto()
            {
                AccountId = 1308,
                Date = "2017-08-16",
                Level = GdtLevelTypeEnum.ADVERTISER
            });
            Assert.IsFalse(retValue.HasError);
        }

        [Description("使用authorization_code获得access_token和refresh_token")]
        [TestMethod]
        public void DoPullGetAccessToken_Test()
        {
            //获取广点通信息
            var gdtAppInfo = BLL.GDT.GdtAccessToken.Instance.GetInfo((int)AuditRelationTypeEnum.Gdt, 1106267651);
            var retValue = _provider.DoPullGetAccessTokenByRefreshToken(gdtAppInfo);
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void SetCache_Test()
        {
            var val = SetCache();

            Console.WriteLine(val);

            val = SetCache();
            Console.WriteLine("1:" + val);
            val = SetCache();
            Console.WriteLine("2:" + val);

            var listKey = CacheHelper<string>.ShowAllCache();

            Console.WriteLine(JsonConvert.SerializeObject(listKey));

            RemoveKey("test1");

            Thread.Sleep(1000);

            val = SetCache();
            Console.WriteLine("1-1:" + val);
            val = SetCache();
            Console.WriteLine("2-2:" + val);
        }

        private string SetCache()
        {
            return CacheHelper<string>.Get(System.Web.HttpRuntime.Cache, () => "test", () =>
                 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss"), null);
        }

        private void RemoveKey(string key)
        {
            CacheHelper<string>.RemoveAllCache(key);
        }
    }
}