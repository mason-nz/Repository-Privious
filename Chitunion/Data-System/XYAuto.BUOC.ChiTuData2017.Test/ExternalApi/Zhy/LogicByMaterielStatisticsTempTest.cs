/********************************************************
*创建人：lixiong
*创建时间：2017/10/24 11:29:29
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
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi;
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.ZHY;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.Test.ExternalApi.Zhy
{
    [TestClass]
    public class LogicByMaterielStatisticsTempTest
    {
        [Description("临时拉取")]
        [TestMethod]
        public void PullStatistics_Temp_Test()
        {
            var provider = new LogicByMaterielStatisticsTemp(new Infrastruction.Http.DoHttpClient(new System.Net.Http.HttpClient()),
                new PullDataConfig()
                {
                    PullDataQueryDateOffset = 5,
                    DateOffset = -3 //9.12
                });

            provider.LoopPullStatistics();
            //Console.WriteLine(JsonConvert.SerializeObject(retValue));
            //Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void DateTest()
        {
            var dateOffset = -35;
            var loopEnd = Math.Abs(dateOffset);//34
            var dt = DateTime.Now.AddDays(dateOffset);
            for (var i = 0; i <= loopEnd - 2; i++)
            {
                var dayQuery = dt.AddDays(i).ToString("yyyy-MM-dd");
                Console.WriteLine(dayQuery);
            }
        }
    }
}