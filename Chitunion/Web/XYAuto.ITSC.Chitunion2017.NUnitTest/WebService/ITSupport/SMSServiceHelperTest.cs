using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web.Configuration;
using XYAuto.ITSC.Chitunion2017.WebService.ITSupport;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.WebService.ITSupport
{
    [TestFixture]
    public class SMSServiceHelperTest
    {
        private string SMSServiceURL = "";//发短信接口API
        private SMSServiceHelper ssh;
        [SetUp]
        public void SMSServiceHelper()
        {
            SMSServiceURL = WebConfigurationManager.AppSettings["SMSServiceURL"];
            ssh = new Chitunion2017.WebService.ITSupport.SMSServiceHelper();
        }

        [Test]
        public void SendSMSTest([Values("13146611863", "13240110346", "15711176911")] string mobile)
        {
            //接口有限制，最多340个字符，多余70个字符，接口内部会自动拆分
            string content = @"据行圆汽车产业研究数据显示，中国汽车销量持续增长，新车销售增长的坡峰已经从二线城市过渡到三、四、五线城市，尤其是四、五线城市的购车需求正在迅速释放。在这一趋势下，汽车品牌厂商正在面临着渠道下沉成本高，难度大的挑战。在移动互联网时代，由于信息的高效传播造成用户价值观多元化，再加上中国地区市场差异大等特点，使得品牌营销成本非常高。同时，这一问题在现在汽车销售已从卖方市场过渡到买方市场的情况下，将会表现得更加突出。汽车品牌经销商作为汽车销售主体，自2012年以来盈利情况始终不乐观。品牌经销商的盈利能力不足，利润点不够等问题凸显。新车销售“以价换量”，想通过售后利润来弥补新车销售的薄利润和亏损，只能是形成恶性循环的局面。所以，新车销售环节到底要不要理直气壮的赚钱。";
            SendMsgResult smr = ssh.SendMsgImmediately(mobile, content);
            Assert.AreEqual(true, Convert.ToBoolean(smr.Result));
        }

        [Test]
        public void SendSMSTest([Values(new string[] { "13146611863", "13240110346" }, new string[] { "13146611863" })] string[] mobile)
        {
            string content = "test2";
            SendMsgResult smr = ssh.SendMsgImmediately(mobile, content);
            Assert.AreEqual(true, Convert.ToBoolean(smr.Result));
        }

        [Test]
        public void CleanImgTest()
        {
            string img = "http://wxtest-ct.qichedaquan.com/api/images/demoIcon.jpg";
            string imgurl = ITSC.Chitunion2017.Common.Util.CleanImg(img);
            Assert.AreEqual(true, !string.IsNullOrEmpty(imgurl));
        }

        [Test]
        public void DomainTest()
        {
            string OrderUrl = "http://news.chituh5.xingyuanwanli.comli.com/ct_m/20180420/194809.html?utm_source=chitu&utm_term=72hzwse63t";
            if (OrderUrl.Contains("ct_m"))
            {
                int index = OrderUrl.IndexOf("ct_m");
                OrderUrl = OrderUrl.Substring(index - 1);
            }
            Assert.AreEqual(true, !string.IsNullOrEmpty(OrderUrl));
        }
    }
}
