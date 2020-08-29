/********************************************************
*创建人：lixiong
*创建时间：2017/11/30 15:03:07
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
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;

namespace XYAuto.BUOC.ChiTuData2017.Test.Query.Statistics
{
    [TestClass]
    public class StatDetailsQueryTest
    {
        [TestMethod]
        public void StatDetailsCarMatchQueryTest()
        {
            var list = new StatDetailsCarMatchQuery(new ConfigEntity()).GetQueryList(new ReqDetailsDto()
            {
                StartDate = "2017-11-19",
                EndDate = "2017-11-20"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}