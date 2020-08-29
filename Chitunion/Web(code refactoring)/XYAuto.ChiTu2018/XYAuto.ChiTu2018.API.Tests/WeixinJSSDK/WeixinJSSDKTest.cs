using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.Controllers;

namespace XYAuto.ChiTu2018.API.Tests.WeixinJSSDK
{
    [TestClass]
    public class WeixinJSSDKTest
    {
        XYAuto.ChiTu2018.API.Controllers.WeixinJSSDKController ctl = new WeixinJSSDKController();
        [TestMethod]
        public void TestShareLog()
        {
            var ret = ctl.ShareLog($"Test Log output...{DateTime.UtcNow}");
            var retStr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }
    }
}
