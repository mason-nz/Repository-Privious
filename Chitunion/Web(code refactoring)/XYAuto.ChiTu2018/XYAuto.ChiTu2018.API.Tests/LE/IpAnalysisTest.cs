/********************************
* 项目名称 ：XYAuto.ChiTu2018.API.Tests.LE
* 类 名 称 ：IpAnalysisTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/6/5 11:10:17
********************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Service.App.IpMonitor;

namespace XYAuto.ChiTu2018.API.Tests.LE
{
    [TestClass]
    public class IpAnalysisTest
    {
        [TestMethod]
        public void GetAreaAddressByIp()
        {
            IpAnalysisService.Instance.GetAreaAddressByIp("118.144.37.981", "baidu.com");
        }
    }
}
