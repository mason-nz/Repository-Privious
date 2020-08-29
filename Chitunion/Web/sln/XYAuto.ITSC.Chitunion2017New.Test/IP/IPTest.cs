/********************************
* 项目名称 ：XYAuto.ITSC.Chitunion2017New.Test.IP
* 类 名 称 ：IPTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/25 9:46:48
********************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;
using XYAuto.ITSC.Chitunion2017.WebService.IP;

namespace XYAuto.ITSC.Chitunion2017New.Test.IP
{

    [TestClass]
    public class IPTest
    {
        [TestMethod]
        public void GetIpArea()
        {
            IpAnalysisBll.Instance.GetAreaAddressByIp("223.104.170.145", "");


        }
    }
}
