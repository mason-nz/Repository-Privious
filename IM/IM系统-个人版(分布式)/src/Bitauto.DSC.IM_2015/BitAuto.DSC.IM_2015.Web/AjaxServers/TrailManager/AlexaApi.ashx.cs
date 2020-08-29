using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.DSC.IM_2015.BLL;
using Newtonsoft.Json;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager
{
    /// <summary>
    /// AlexaApi 的摘要说明
    /// </summary>
    public class AlexaApi : IHttpHandler, IRequiresSessionState
    {
        HttpContext curContext;
        #region

        //访客来源
        private string sourceType
        {
            get
            {
                return curContext.Request["SourceType"] == null ? "-1" :
                HttpUtility.UrlDecode(curContext.Request["SourceType"].ToString());
            }
        }
        //日期
        private string Starttime
        {
            get
            {
                return curContext.Request["Starttime"] == null ? "1900-01-01" :
                HttpUtility.UrlDecode(curContext.Request["Starttime"].ToString());
            }
        }
        //日期
        private string EndTime
        {
            get
            {
                return curContext.Request["EndTime"] == null ? "1900-01-01" :
                HttpUtility.UrlDecode(curContext.Request["EndTime"].ToString());
            }
        }
        //统计维度
        private string SelectType
        {
            get
            {
                return curContext.Request["SelectType"] == "0" ? "4" :
                HttpUtility.UrlDecode(curContext.Request["SelectType"].ToString());
            }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            try
            {
                curContext = context;
                string action = curContext.Request.QueryString["action"];
                curContext.Response.ContentType = "application/json";
                curContext.Response.Clear();
                switch (action)
                {
                    case "ShowChart":
                        ShowChart();
                        break;
                    default:
                        ShowChart();
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private QueryBussinessLineTotal Get()
        {
            QueryBussinessLineTotal query = new QueryBussinessLineTotal();
            query.SourceType = int.Parse(sourceType);
            query.BeginTime = DateTime.Parse(Starttime);
            query.EndTime = DateTime.Parse(EndTime);
            query.SelectType = int.Parse(SelectType);
            return query;
        }

        /// <summary>
        /// 图表显示
        /// </summary>
        private void ShowChart()
        {
            DataTable dt = BLL.Conversations.Instance.S_FlowForMap_Select(Get());
            dt.Columns.Add("DatePeriod");
            List<string> categories = new List<string>();
            List<Series> data = new List<Series>();
            List<int?> dataSumVisit = new List<int?>();
            List<int?> dataSumConversation = new List<int?>();
            int step = 1;
            var dateNow = DateTime.Now;
            foreach (DataRow item in dt.Rows)
            {
                if (SelectType == "4")
                {
                    item["DatePeriod"] = (item["hourtime"].ToString() == "" ? "" : item["hourtime"]);
                    if (item["DatePeriod"].ToString() != "")
                    {
                        var isToday = int.Parse(item["DatePeriod"].ToString()) > dateNow.Hour && item["hourtimename"].ToString().Substring(0, 10) == dateNow.ToString("yyyy-MM-dd");
                        if (isToday || dateNow.CompareTo(Convert.ToDateTime(item["hourtimename"].ToString().Substring(0, 10))) < 0)
                        {
                            item["SumConversation"] = DBNull.Value;
                            item["SumVisit"] = DBNull.Value;
                        }
                    }
                }
                else if (SelectType == "1")
                {
                    item["DatePeriod"] = (item["daytime"].ToString() == "" ? "" : Convert.ToDateTime(item["daytime"].ToString()).ToString("yyyy-MM-dd"));
                    if (item["DatePeriod"].ToString() != "")
                    {
                        if (dateNow.CompareTo(Convert.ToDateTime(item["DatePeriod"].ToString())) < 0)//大于今天都显示null
                        {
                            item["SumConversation"] = DBNull.Value;
                            item["SumVisit"] = DBNull.Value;
                        }
                    }
                }
                else if (SelectType == "2")
                {
                    item["DatePeriod"] = (item["weekb"].ToString() == "" ? "" : Convert.ToDateTime(item["weekb"].ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(item["weeke"].ToString()).ToString("yyyy-MM-dd"));
                    if (item["weekb"].ToString() != "")
                    {
                        if (dateNow.CompareTo(Convert.ToDateTime(item["weekb"].ToString())) < 0)//大于今天都显示null
                        {
                            item["SumConversation"] = DBNull.Value;
                            item["SumVisit"] = DBNull.Value;
                        }
                    }

                }
                else if (SelectType == "3")
                {
                    item["DatePeriod"] = (item["monthb"].ToString() == "" ? "" : Convert.ToDateTime(item["monthb"].ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(item["monthe"].ToString()).ToString("yyyy-MM-dd"));
                    if (item["monthb"].ToString() != "")
                    {
                        if (dateNow.CompareTo(Convert.ToDateTime(item["monthb"].ToString())) < 0)//大于今天都显示null
                        {
                            item["SumConversation"] = DBNull.Value;
                            item["SumVisit"] = DBNull.Value;
                        }
                    }

                }
                categories.Add(item["DatePeriod"].ToString());
                if (item["SumConversation"] == DBNull.Value)
                {
                    dataSumConversation.Add(null);
                }
                else
                {
                    dataSumConversation.Add(int.Parse(item["SumConversation"].ToString()));
                }

                if (item["SumVisit"] == DBNull.Value)
                {
                    dataSumVisit.Add(null);
                }
                else
                {
                    dataSumVisit.Add(int.Parse(item["SumVisit"].ToString()));
                }
            }
            if (dt.Rows.Count > 15 && SelectType == "1")
            {
                step = int.Parse(Math.Round(Decimal.Parse(dt.Rows.Count.ToString()) / 10, 0).ToString());
            }
            if (dt.Rows.Count > 7 && (SelectType == "3" || SelectType == "2"))
            {
                step = int.Parse(Math.Round(Decimal.Parse(dt.Rows.Count.ToString()) / 5, 0).ToString());
            }
            data.Add(new Series() { name = "页面访问量", data = dataSumVisit });
            data.Add(new Series() { name = "总对话量", data = dataSumConversation });
            Chart chart = new Chart() { categories = categories, series = data, step = step };
            string strJson = JsonConvert.SerializeObject(chart);
            curContext.Response.Write(strJson);
        }




        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


    public class Chart
    {
        public int step;
        public List<string> categories;
        public List<Series> series;
    }
    public class Series
    {
        public string name;
        public List<int?> data;
    }
}