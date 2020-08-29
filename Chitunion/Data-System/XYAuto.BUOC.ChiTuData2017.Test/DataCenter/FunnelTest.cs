using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Funnel;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class FunnelTest
    {
        /// <summary>
        /// 漏斗头部导出
        /// </summary>
        [TestMethod]
        public void FunnelHeadExport()
        {
           var result = FunnelMaterialBll.Instance.FunnelHeadExport(new BasicQueryArgs() { Operator = 1,DateType=7 });

            var text = Util.GetJsonDataByResult(result.Url, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 漏斗腰部导出
        /// </summary>
        [TestMethod]
        public void FunnelWaistExport()
        {
            dynamic result = FunnelMaterialBll.Instance.FunnelWaistExport(new BasicQueryArgs() { Operator = 1,DateType=30});

            var text = Util.GetJsonDataByResult(result.Url, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);

        }

        /// <summary>
        /// 漏斗物料导出
        /// </summary>
        [TestMethod]
        public void FunnelMaterialExport()
        {
            dynamic result = FunnelMaterialBll.Instance.FunnelMaterialExport(new BasicQueryArgs() { Operator = 1, EndTime = "2017-11-28", BeginTime = "2017-11-28" });

            var text = Util.GetJsonDataByResult(result.Url, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 头部列表
        /// </summary>
        [TestMethod]
        public void GetFunnelHeadList()
        {
            dynamic result = FunnelMaterialBll.Instance.GetFunnelHeadList(new BasicQueryArgs() { Operator = 1, DateType=7 });

            var text = Util.GetJsonDataByResult(result, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 腰部列表
        /// </summary>
        [TestMethod]
        public void GetFunnelWaistDetailList()
        {
            dynamic result = FunnelMaterialBll.Instance.GetFunnelWaistDetailList(new BasicQueryArgs() { Operator = 1, DateType = 7 });

            var text = Util.GetJsonDataByResult(result, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 物料列表
        /// </summary>
        [TestMethod]
        public void GetFunnelMaterialList()
        {
            dynamic result = FunnelMaterialBll.Instance.GetFunnelMaterialList(new BasicQueryArgs() { Operator = 1, DateType = 7 });

            var text = Util.GetJsonDataByResult(result, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
    }
}
