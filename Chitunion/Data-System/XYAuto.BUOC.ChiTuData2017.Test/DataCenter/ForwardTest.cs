using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Forward;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class ForwardTest
    {
        [TestMethod]
        public void RenderEncapsulate()
        {


            var result = ForwardMaterialBll.Instance.BusinessMap(new BasicQueryArgs
            {
                DateType = 7,
                BeginTime = "2017-11-27",
                EndTime = "2017-11-03",
                ChartType = "zf_head_pie,zf_head_bar,zf_materia,zf_scenc,zf_account"
            });

            var text = Util.GetJsonDataByResult(result, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }

        /// <summary>
        /// 统计列表
        /// </summary>
        [TestMethod]
        public void GetForwardStatisticsList()
        {



            var table = ForwardMaterialBll.Instance.GetForwardStatisticsList(new Entities.DataCenter.ListQueryArgs
            {
                PageIndex = 1,
                PageSize = 10,
                BeginTime = "2010-11-11",
                EndTime = "2017-12-12"
            });

            string tem = JsonConvert.SerializeObject(table);
        }

        /// <summary>
        /// 明细列表
        /// </summary>
        [TestMethod]
        public void GetClueDetailList()
        {

            BasicResultDto resultInfo = ForwardMaterialBll.Instance.GetForwardDetailList(new ListQueryArgs { BeginTime = "2017-11-28", EndTime = "2017-12-13", PageIndex = 1, PageSize = 10 });// GetClueDetailList(ListQueryArgs query)

            var text = Util.GetJsonDataByResult(resultInfo, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
    }
}
