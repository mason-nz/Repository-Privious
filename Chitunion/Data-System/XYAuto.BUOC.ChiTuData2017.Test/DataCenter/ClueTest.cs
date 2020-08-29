using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Clue;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class ClueTest
    {
       
        /// <summary>
        /// 渲染列表
        /// </summary>
        [TestMethod]
        public void RenderClue()
        {

            var resultInfo = ClueMaterialBll.Instance.BusinessMap(new BasicQueryArgs
            {
                BeginTime = "2017-11-27",
                EndTime = "2017-11-03",
                DateType =7,
                ChartType = "xs_head_pie,xs_head_bar,xs_materia,xs_scenc,xs_account,xs_essay"
            });// GetClueDetailList(ListQueryArgs query)

            var text = Util.GetJsonDataByResult(resultInfo, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }

        /// <summary>
        /// 统计列表
        /// </summary>
        [TestMethod]
        public void GetClueStatisticsList()
        {



            BasicResultDto table = ClueMaterialBll.Instance.GetClueStatisticsList(new Entities.DataCenter.ListQueryArgs
            {
                PageIndex = 1,
                PageSize = 10,
                ChannelID=0,
                BeginTime = "2017-11-06",
                EndTime = "2017-12-05"
            });

            string tem = JsonConvert.SerializeObject(table);
        }

        /// <summary>
        /// 详细列表
        /// </summary>
        [TestMethod]
        public void GetClueDetailList()
        {

            BasicResultDto resultInfo = ClueMaterialBll.Instance.GetClueDetailList(new ListQueryArgs { BeginTime = "2017-11-12", EndTime = "2017-11-13", PageIndex = 1, PageSize = 10 });// GetClueDetailList(ListQueryArgs query)

            var text = Util.GetJsonDataByResult(resultInfo, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }

    }
}
