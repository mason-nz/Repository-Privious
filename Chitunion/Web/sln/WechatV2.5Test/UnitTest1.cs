using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers;
using System.Collections.Generic;

namespace WechatV2._5Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetProfitList()
        {
            Dictionary<string, object> dicAll =   XYAuto.ITSC.Chitunion2017.BLL.Profit.Profit.Instance.GetProfitList(20, 600000);
        }
        [TestMethod]
        public void QueryUserInfo()
        {
            Dictionary<string, object> dicAll = XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.QueryUserInfo();
        }
        [TestMethod]
        public void sdfds()
        {
          XYAuto.ITSC.Chitunion2017.BLL.Util.CheckIDCard("411323199010142140");
        }
    }
}
