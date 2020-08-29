using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class StatisticsExceTest
    {
        /// <summary>
        /// 汇总导出
        /// </summary>
        [TestMethod]
        public void StatisticsExport()
        {
            var query = StatisticsExceListBll.Instance.StatisticsExport(new ListQueryArgs { ListType = "ff" });

            var text = Util.GetJsonDataByResult(query.Url, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 汇总明细导出
        /// </summary>
        [TestMethod]
        public void DetailExport()
        {
            var query = StatisticsExceListBll.Instance.DetailExport(new ListQueryArgs { ListType = "ff", BeginTime="2017-11-01", EndTime="2017-11-28" });

            var text = Util.GetJsonDataByResult(query.Url, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
    }
}
