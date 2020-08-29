using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_4
{
    [TestClass]
    public class StockBrokerTest
    {
        /**
         * 登陆接口的：
            username:15011446355,1810707374,1011465937
            password:446355,1234.abc

            获取经销商营业执照图片的
            dealerID:100116043
         * **/
        [TestMethod]
        public void Login()
        {
            string errorMsg = string.Empty;
            XYAuto.ITSC.Chitunion2017.Entities.StockBroker.LoginDto ret = BLL.StockBroker.StockBroker.Instance.Login("1810707374", "1234.abc", out errorMsg);
            NUnit.Framework.Assert.AreNotEqual(null, ret);
        }
        [TestMethod]
        public void DealerBusinessLicence()
        {
            string errorMsg = string.Empty;
            var ret = BLL.StockBroker.StockBroker.Instance.DealerBusinessLicence(100116043,out errorMsg);
            NUnit.Framework.Assert.AreNotEqual(null, ret);
        }
        [TestMethod]
        public void isStockBrokerUser()
        {
            var ret = BLL.StockBroker.StockBroker.Instance.isStockBrokerUser("15800000000");
            Console.WriteLine(ret);

            var ret2 = BLL.StockBroker.StockBroker.Instance.isStockBrokerUser("15800000001");
            Console.WriteLine(ret2);
        }
        [TestMethod]
        public void Test()
        {
            string postdata = string.Empty;
            string httpurl = "http://www.chitunion.com/api/ShoppingCart/ADScheduleOpt_ShoppingCart?v=1_1";
            postdata = "OptType=1&MediaType=14002&CartID=1&RecID=14002&BeginTime=2017-4-18 8:00&EndTime=2017-4-18 8:00";
            
            var obj = XYAuto.ITSC.Chitunion2017.BLL.StockBroker.Util.HttpWebRequestCreate<dynamic>(httpurl, postdata);
        }
    }
}
