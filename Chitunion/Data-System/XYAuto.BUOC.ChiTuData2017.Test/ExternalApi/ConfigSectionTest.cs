/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 19:30:01
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
using XYAuto.BUOC.ChiTuData2017.ExternalApi.Config;
using XYAuto.ChiTu.Config;

namespace XYAuto.BUOC.ChiTuData2017.Test.ExternalApi
{
    [TestClass]
    public class ConfigSectionTest
    {
        [TestMethod]
        public void ConfigSection()
        {
            var config = SectionInvoke<ZhyApiConfigSection>.GetConfig(ZhyApiConfigSection.SectionName);

            Console.WriteLine(JsonConvert.SerializeObject(config));

            Assert.IsNotNull(config, "config != null");
        }

        [TestMethod]
        public void OpApiNoteDistributeConfigSection_Test()
        {
            var config = SectionInvoke<OpApiNoteDistributeConfigSection>.GetConfig(OpApiNoteDistributeConfigSection.SectionName);

            Console.WriteLine(JsonConvert.SerializeObject(config));
        }
    }
}