/********************************************************
*创建人：lixiong
*创建时间：2017/10/11 9:50:49
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
using XYAuto.BUOC.BOP2017.BLL.GDT.PullConfigLable;

namespace XYAuto.BUOC.BOP2017.Test.Demand
{
    [TestClass]
    public class PullConfigLableProviderTest
    {
        private readonly PullConfigLableProvider _pullConfigLableProvider;

        public PullConfigLableProviderTest()
        {
            _pullConfigLableProvider = new PullConfigLableProvider();
        }

        [TestMethod]
        public void SetConfigTest()
        {
            _pullConfigLableProvider.SetConfig(PullCategoryEnum.GdtAccunt, new ConfigBaseInfo<int>
            {
                ConfigPageInfo = new ConfigPageInfo()
                {
                    Page = 1,
                    PageSize = 20
                }
            });
            //Assert.Fail();
        }

        [TestMethod]
        public void GetConfigTest()
        {
            var config = _pullConfigLableProvider.GetConfig<int>(PullCategoryEnum.GdtAccunt);
            Console.WriteLine(JsonConvert.SerializeObject(config));
        }
    }
}