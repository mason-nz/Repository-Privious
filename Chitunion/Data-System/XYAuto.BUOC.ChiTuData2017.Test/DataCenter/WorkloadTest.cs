using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Workload;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class WorkloadTest
    {

        /// <summary>
        /// 工作量统计-导出
        /// </summary>
        [TestMethod]
        public void WorkloadStatisticsExport()
        {
            var text = Util.GetJsonDataByResult(WorkloadBll.Instance.WorkloadStatisticsExport(new WorkloadQuery() { Operator = 1003 }), "Success");
            string tem = JsonConvert.SerializeObject(text);
            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 总做量统计列表
        /// </summary>
        [TestMethod]
        public void GetWorkloadList()
        {
            var text = WorkloadBll.Instance.GetWorkloadList(new WorkloadQuery() { Operator = 1002, PageIndex=1,PageSize=20,BeginTime= "2017-10-31",EndTime= "2017-12-21" });
            string tem = JsonConvert.SerializeObject(text);
            Assert.AreEqual(0, 0);
        }
    }
}
