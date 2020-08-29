using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017New.Test.Media
{
    [TestClass]
    public class WeiXinVisvitTest
    {
        [TestMethod]
        public void GetWithdrawalsStatisticsList()
        {
            WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(13021, "http://wxs.chitunion.com/moneyManager/sign.html");

            WeiXinVisvitBll.Instance.AddWeiXinVisvitInfo(new WeiXinVisvitModel { UserID = 1302, ChannelCode = "ctlmcaidan", Type = 1, Url = "baidu.com" });
        }
    }
}
