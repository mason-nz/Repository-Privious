using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.WebService.GDT
{
    [TestFixture]
    public class ServiceHelperTest
    {
        private Chitunion2017.WebService.GDT.ServiceHelper gdt;

        [SetUp]
        public void ServiceHelper()
        {
            gdt = new Chitunion2017.WebService.GDT.ServiceHelper();
        }

        [Test]
        public void GetAdvertiserInfoTest([Values(5385895)] int account_id)
        {
            var json = gdt.GetAdvertiserInfo(new ReqReportDto()
            {
                //AccountId = account_id
            })
            ;
            Assert.AreNotEqual(null, json);
        }

        [Test]
        public void GetGdtAccessTokenTest()
        {
            string accessToken = gdt.GetGdtAccessToken();
            //string accessToken = gdt.GetAccessTokenByAuthorizationCode();
            Assert.AreNotEqual(null, accessToken);
        }

        [Test]
        public void GetFundsInfoTest([Values(1308)] int account_id)
        {
            //var jsonStr = gdt.GetFundsInfo(account_id);
            //Assert.AreNotEqual(string.Empty, jsonStr);
        }

        [Test]
        public void GetImagesInfoTest([Values(1308)] int account_id)
        {
            string jsonStr = gdt.GetImagesInfo(account_id);
            Assert.AreNotEqual(string.Empty, jsonStr);
        }

        [Test]
        public void AddImagesInfoTest([Values(1308)] int account_id)
        {
            string jsonStr = gdt.AddImagesInfo(account_id);
            Assert.AreNotEqual(string.Empty, jsonStr);
        }

        [Test]
        public void AddAdvertiserTest([Values("企业名称——测试2")] string corporation_name, [Values("http://www.chitunion.com")] string website)
        {
            string jsonStr = gdt.AddAdvertiser_Test(corporation_name, website);
            Assert.AreNotEqual(string.Empty, jsonStr);
        }

        [Test]
        public void AddCampaignsTest([Values(100000611, 100000612)] int account_id, [Values("推广计划—测试1", "推广计划—测试2", "推广计划—测试3")] string campaign_name,
            [Values("CAMPAIGN_TYPE_NORMAL", "CAMPAIGN_TYPE_WECHAT_OFFICIAL_ACCOUNTS", "CAMPAIGN_TYPE_WECHAT_MOMENTS")] string campaign_type)
        {
            string jsonStr = gdt.AddCampaigns_Test(account_id, campaign_name, campaign_type);
            Assert.AreNotEqual(string.Empty, jsonStr);
        }

        [Test]
        public void AddAdgroupsTest([Values(100000611, 100000612)] int account_id, [Values(960921)] int campaign_id, [Values("广告组名称-测试1")] string adgroup_name,
            [Values("")] string site_set, [Values("PRODUCT_TYPE_LINK")] string product_type,
            [Values("2017-09-01")] string begin_date, [Values("2017-10-01")] string end_date,
            [Values(199)] int bid_amount, [Values("OPTIMIZATIONGOAL_CLICK")] string optimization_goal,
            [Values("BILLINGEVENT_CLICK")] string billing_event)
        {
            string jsonStr = gdt.AddAdgroups_Test(account_id, campaign_id, adgroup_name,
                new string[] { "SITE_SET_MOBILE_INNER" }, product_type, begin_date, end_date, bid_amount,
                optimization_goal, billing_event);
            Assert.AreNotEqual(string.Empty, jsonStr);
        }

        [Test]
        public void AddTargetingsTest([Values(100000611, 100000612)] int account_id, [Values("定向名称1", "定向名称2", "定向名称3")] string targeting_name)
        {
            string jsonStr = gdt.AddTargetings_Test(account_id, targeting_name);
            Assert.AreNotEqual(string.Empty, jsonStr);
        }
    }
}