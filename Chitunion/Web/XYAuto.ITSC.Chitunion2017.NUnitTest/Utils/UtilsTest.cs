using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.Utils
{
    [TestFixture]
    public class UtilsTest
    {
        [SetUp]
        public void Utils()
        {

        }

        [Test]
        public void GetQueryStringTest([Values("channel", "isauth", "source", "q")] string name,
            [Values("http://weixins.xingyuanwanli.com/cashManager/accountInfo.html?channel=ctlmca订单idan&isauth=1",
            "http://www.google.com.hk/search?hl=zh-CN&source=hp&q=%E5%8D%9A%E6%B1%87%E6%95%B0%E7%A0%81&aq=f&aqi=g2&aql=&oq=&gs_rfai=")] string url)
        {
            string val = XYAuto.ITSC.Chitunion2017.BLL.Util.GetQueryString(name, url);
            Assert.AreNotEqual(val, string.Empty);
        }
    }
}
