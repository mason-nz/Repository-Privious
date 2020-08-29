using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.Utils.Config;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class EncapsulateMaterialTest
    {
        /// <summary>
        /// 图表渲染
        /// </summary>
        [TestMethod]
        public void RenderEncapsulate()
        {
            BasicQueryArgs entity = new BasicQueryArgs()
            {
                DateType = 30,
                BeginTime = "2017-11-27",
                EndTime = "2017-11-03",
                ChartType = "fz_head_pie,fz_head_bar,fz_scenc,fz_account,fz_essay,fz_condition"
            };
            var result = EncapsulateMaterialBll.Instance.BusinessMap(entity);

            var text = Util.GetJsonDataByResult(result, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
        /// <summary>
        /// 统计列表
        /// </summary>
        [TestMethod]
        public void GetEncapsulateStatisticsList()
        {
            var table = EncapsulateMaterialBll.Instance.GetEncapsulateStatisticsList(new Entities.DataCenter.ListQueryArgs
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

            BasicResultDto resultInfo = EncapsulateMaterialBll.Instance.GetEncapsulateDetailList(new ListQueryArgs { BeginTime = "2017-11-12", EndTime = "2017-11-13", PageIndex = 1, PageSize = 10 });// GetClueDetailList(ListQueryArgs query)

            var text = Util.GetJsonDataByResult(resultInfo, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
    }
}
