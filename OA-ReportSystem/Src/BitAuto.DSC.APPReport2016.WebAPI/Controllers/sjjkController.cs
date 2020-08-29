using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BitAuto.DSC.OASysRightManager2016.Common.WebAPI;
using System.Data;
using System.Collections;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1003")]
    public class sjjkController : ApiController
    {
        /// <summary>
        ///获取平台/业务线维度Leads饼图数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">1:平台维度；2：业务线维度</param>
        /// <returns></returns>
        [HttpGet]
        //http://localhost:42241/api/sjjk/GetLeadsPieChartData?date=2016-01-01%2010:00:00.000&type=1
        public HttpResponseMessage GetLeadsPieChartData(DateTime? date, int type)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }
            date = date.Value.Date;
            RetPieChartData retVal = new RetPieChartData();
            if (type > 0 && type < 3)
            {
                DateTime dateMax = new DateTime();
                DataTable dt = BLL.Leads.Instance.GetLeadsPieChartData(date.Value, type, out dateMax);
                if (dt != null && dt.Rows.Count > 0)
                {
                    retVal.dataDate = dateMax.ToString("yyyy/MM/dd");
                    retVal.dataWeek = BLL.Util.GetWeekNameByDate(dateMax);
                    retVal.data = dt;
                }
            }
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(retVal);
        }

        /// <summary>
        /// 获取平台/业务线维度Leads数趋势和导向构成总览_详细数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">1:平台维度；2：业务线维度</param>
        /// <returns></returns>
        [HttpGet]
        //http://localhost:42241/api/sjjk/GetLeadsBlockData?date=2016-01-01%2010:00:00.000&type=1
        public HttpResponseMessage GetLeadsBlockData(DateTime? date, int type)
        {
            if (type > 0 && type < 3)
            {
                if (!date.HasValue)
                {
                    date = DateTime.Now;
                }
                date = date.Value.Date;
                DataSet ds = BLL.Leads.Instance.GetLeadsOverView_Data(date.Value, type);
                if (ds != null && ds.Tables.Count == 2)
                {
                    ds.Tables[1].TableName = "data";
                    var objData = new { summary = new { totalCount = (ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["totalCount"] : 0), lastWeek = (ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["lastWeek"] : 0), yesterday = (ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["yesterday"] :0)}, data = ds.Tables[1] };
                    return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(objData);
                }
            }
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult("参数异常");
        }

        /// <summary>
        ///  获取平台/业务线维度Leads导向构成环形图数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="siteId">平台id（>=0）</param>
        /// <param name="lineId">业务线id（>=0）</param>
        /// <returns></returns>
        [HttpGet]
        //http://localhost:42241/api/sjjk/GetLeadsCircularChartData?date=2016-01-01%2010:00:00.000&siteId=10001&lineId=0
        public HttpResponseMessage GetLeadsCircularChartData(DateTime? date, int siteId, int lineId)
        {
            if (siteId >= 0 && lineId >= 0)
            {
                if (!date.HasValue)
                {
                    date = DateTime.Now;
                }
                date = date.Value.Date;
                DataTable dt = BLL.Leads.Instance.GetLeadsCircleChartData(date.Value, siteId, lineId);
                return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dt);
            }
            else
            {
                return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult("参数异常");
            }
        }

        /// <summary>
        /// 获取平台/业务线维度Leads数趋势折线图数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="siteId">平台id（>=0）</param>
        /// <param name="lineId">业务线id（>=0）</param>
        /// <returns></returns>
        [HttpGet]
        //http://localhost:42241/api/sjjk/GetLeadsLineChartData?date=2016-01-03%2010:00:00.000&siteId=10001&lineId=0
        public HttpResponseMessage GetLeadsLineChartData(DateTime? date, int siteId, int lineId)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now;
            }
            RetLineChartData retData = new RetLineChartData();
            string[] arrSName = { "Leads数", "下单用户数" };
            retData.seriesname = arrSName;
            if (siteId >= 0 && lineId >= 0)
            {
                date = date.Value.Date;
                DataTable dt = BLL.Leads.Instance.GetLeadsLineChartData(date.Value, siteId, lineId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string[] arrKey = new string[dt.Rows.Count];
                    int[,] arrVal = new int[2, dt.Rows.Count];
                    List<List<int>> listVal = new List<List<int>>();
                    List<int> list1 = new List<int>();
                    List<int> list2 = new List<int>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        arrKey[i] = dt.Rows[i]["date"].ToString();
                        list1.Add(Convert.ToInt32(dt.Rows[i]["leadsCount"].ToString()));
                        list2.Add(Convert.ToInt32(dt.Rows[i]["orderUsersCount"].ToString()));
                    }
                    listVal.Add(list1);
                    listVal.Add(list2);
                    retData.datekey = arrKey;
                    retData.dataval = listVal;
                }
            }
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(retData);
        }
    }
    public class RetLineChartData
    {
        public string[] seriesname { get; set; }
        public string[] datekey { get; set; }
        public List<List<int>> dataval { get; set; }
    }

    public class RetPieChartData
    {
        public string dataDate { get; set; }
        public string dataWeek { get; set; }
        public object data { get; set; }
    }
}
