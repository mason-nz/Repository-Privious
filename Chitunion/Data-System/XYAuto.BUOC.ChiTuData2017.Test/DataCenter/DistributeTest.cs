using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]

    public class DistributeTest
    {
        [TestMethod]
        public void RenderDistribute()
        {
            BasicQueryArgs entity = new BasicQueryArgs()
            {
                DateType = 7,
                BeginTime = "2017-11-27",
                EndTime = "2017-11-03",
                ChartType = "fenf_head,fenf_materia,fenf_scenc,fenf_account,fenf_essay"
            };

            var result = DistributeMaterialBll.Instance.BusinessMap(entity);

            var text = Util.GetJsonDataByResult(result, "Success");

            string tem = JsonConvert.SerializeObject(text);

        }
        /// <summary>
        /// 统计列表
        /// </summary>
        [TestMethod]
        public void GetDistributeStatisticsList()
        {



            BasicResultDto table = DistributeMaterialBll.Instance.GetDistributeStatisticsList(new Entities.DataCenter.ListQueryArgs
            {
                PageIndex = 1,
                PageSize = 10,
                
                BeginTime = "2017-12-05",
                EndTime = "2017-12-05"
            });

            string tem = JsonConvert.SerializeObject(table);
        }

        /// <summary>
        /// 详细列表
        /// </summary>
        [TestMethod]
        public void GetDistributeDetailList()
        {

            BasicResultDto resultInfo = DistributeMaterialBll.Instance.GetDistributeDetailList(new ListQueryArgs { BeginTime = "2017-12-05", EndTime = "2017-12-05", PageIndex = 1, PageSize = 10 });// GetClueDetailList(ListQueryArgs query)

            var text = Util.GetJsonDataByResult(resultInfo, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
    }
}
