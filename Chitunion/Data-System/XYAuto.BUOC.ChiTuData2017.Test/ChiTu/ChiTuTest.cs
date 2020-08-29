/********************************************************
*创建人：hant
*创建时间：2017/12/22 13:49:36 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.ChiTu
{
    [TestClass]
    public class ChiTuTest
    {
        [TestMethod]
        public void ChannelSummary()
        {
            int userid = 1419;
            var result = BLL.Chitu.DataStatistics.Instance.GetDataByUserID(userid);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }

        [TestMethod]
        public void ChannelSummaryByDay()
        {
            RequestDataStatisticsByDate req = new RequestDataStatisticsByDate()
            {
                //BeginDate = "2017-12-22",
                //EndDate = "2017-12-25",
                ChannelID = 101002,
                PageIndex = 1,
                PageSize = 20,
                StateOfSettlement = -2
            };
            var result = BLL.Chitu.DataStatisticsByDate.Instance.GetData(req);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }

        [TestMethod]
        public void Order()
        {
            RequestOrder req = new RequestOrder()
            {
                Status = 93002,
                BeginTime = "2017-12-25",
               EndTime ="2017-12-25",
                ChannelID = 101002,
                OrderType = -2,
                //OrderID:
                //TaskID:
                PageSize = 20,
                PageIndex = 1
            };
            var result = BLL.Chitu.Order.Instance.GetOrderList(req);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }

        [TestMethod]
        public void OrderExcel()
        {
            RequestOrder req = new RequestOrder()
            {

            };
            var result = BLL.Chitu.Order.Instance.Export(req);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }

        [TestMethod]
        public void OrderDetial()
        {
            int OrderId = 2;
            var result = BLL.Chitu.Order.Instance.GetOrderDetial(OrderId);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }

        [TestMethod]
        public void ChannelSummaryByMonth()
        {
            RequestSummaryByMonth req = new RequestSummaryByMonth()
            {
                ChannelID = 101002,
                //BeginTime:
                //EndTime:
                //
                StateOfSettlement = -2,
                PageSize = 20,
                PageIndex = 1
            };
            var result = BLL.Chitu.DataStatisticsByMonth.Instance.GetMonthStatistics(req);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }

        [TestMethod]
        public void ChannelSummaryMonthExcel()
        {
            RequestSummaryByMonth req = new RequestSummaryByMonth()
            {
                ChannelID = 101002,
                //BeginTime:
                //EndTime:
                //
                StateOfSettlement = -2,
                PageSize = 20,
                PageIndex = 1
            };
            var result = BLL.Chitu.DataStatisticsByMonth.Instance.Export(req);
            var text = Util.GetJsonDataByResult(result, "Success");
            string tem = JsonConvert.SerializeObject(text);
        }


    }
}
