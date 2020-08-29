/********************************************************
*创建人：lixiong
*创建时间：2017/11/30 10:59:18
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
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;

namespace XYAuto.BUOC.ChiTuData2017.Test.Query.Statistics
{
    [TestClass]
    public class StatDailyQueryTest
    {
        [TestMethod]
        public void StatDailyGrabQueryTest()
        {
            var list = new StatDailyGrabQuery(new ConfigEntity()).GetQueryList(new ReqDailyDto()
            {
                StartDate = "2017-11-28",
                EndDate = "2017-12-04"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void StatDailyCsQueryTest()
        {
            var list = new StatDailyCsQuery(new ConfigEntity()).GetQueryList(new ReqDailyDto()
            {
                StartDate = "2017-11-05",
                EndDate = "2017-12-04"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void StatDailyRgqxQueryTest()
        {
            var list = new StatDailyRgqxQuery(new ConfigEntity()).GetQueryList(new ReqDailyDto()
            {
                StartDate = "2017-11-05",
                EndDate = "2017-12-04"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}