/********************************************************
*创建人：lixiong
*创建时间：2017/12/1 16:37:39
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider;

namespace XYAuto.BUOC.ChiTuData2017.Test.Statistics.TabProvider
{
    [TestClass]
    public class StatTabPrimaryProviderTest
    {
        public StatTabPrimaryProviderTest()
        {
            BLL.AutoMapperConfig.AutoMapperConfig.Configure();
        }

        [TestMethod]
        public void GetCsLeftPieNestTest()
        {
            var info = new StatTabPrimaryProvider(7, "cs_bar_brush").GetGrabData();
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }
    }
}