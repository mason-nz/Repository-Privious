/********************************************************
*创建人：lixiong
*创建时间：2017/9/18 19:51:27
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
using XYAuto.BUOC.ChiTuData2017.BLL.ExternalApi.QingNiao;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;

namespace XYAuto.BUOC.ChiTuData2017.Test.ExternalApi.QingNiao
{
    [TestClass]
    public class MaterielStatisticsProviderTest
    {
        private readonly MaterielStatisticsProvider _provider;
        private readonly string _queryDate;

        public MaterielStatisticsProviderTest()
        {
            BLL.AutoMapperConfig.AutoMapperConfig.Configure();
            _queryDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            _provider = new MaterielStatisticsProvider(new PullDataConfig() { Date = _queryDate });
        }

        [TestMethod]
        public void PullStatistics_Test()
        {
            var retValue = _provider.PullStatistics(_queryDate, DateTime.Now);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("临时拉取")]
        [TestMethod]
        public void PullStatistics_Temp_Test()
        {
            var provider = new MaterielStatisticsProvider(
                new PullDataConfig()
                {
                    PullDataQueryDateOffset = 5,
                    DateOffset = -35 //9.12
                });

            provider.LoopPullStatistics();
            //Console.WriteLine(JsonConvert.SerializeObject(retValue));
            //Assert.IsFalse(retValue.HasError);
        }

        [Description("临时拉取")]
        [TestMethod]
        public void ChituMaterialStatQuery_Test()
        {
            var provider = new ChituMaterialStatQuery(new ConfigEntity());
            var materielList = provider.GetQueryList(new RequestChituChannelDto()
            {
                Date = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd")
            });
            Console.WriteLine(JsonConvert.SerializeObject(materielList));
        }

        [Description("汽车大全-正式的拉取逻辑")]
        [TestMethod]
        public void LoopPullStatistics_Test()
        {
            var provider = new MaterielStatisticsProviderExt(
                new PullDataConfig()
                {
                    DateOffset = -35 //9.12
                });

            provider.LoopPullStatistics();
        }

        [TestMethod]
        public void ChituClickStatQuery_Test()
        {
            var provider = new MaterielStatisticsProviderExt(
               new PullDataConfig()
               {
                   DateOffset = -35 //9.12
               });
            var queryDate = "2017-10-27";
            provider.ChituClickStatQuery(Convert.ToDateTime(queryDate));
        }
    }
}