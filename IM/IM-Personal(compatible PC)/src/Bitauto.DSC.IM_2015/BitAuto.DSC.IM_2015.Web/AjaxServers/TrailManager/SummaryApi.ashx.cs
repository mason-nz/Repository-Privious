using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.DSC.IM_2015.BLL;
using Newtonsoft.Json;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager
{
    /// <summary>
    /// SummaryApi 的摘要说明
    /// </summary>
    public class SummaryApi : IHttpHandler, IRequiresSessionState
    {
        HttpContext curContext;
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

        /// <summary>
        /// 图表显示
        /// </summary>
        private void ShowChart()
        {
            DataTable dt = DefaultChannelHandler.StateManager.GetAgentRealTime_OnlineHaveBGID();
            List<Series> list = new List<Series>();
            List<Data> data1 = new List<Data>();
            List<Data> data2 = new List<Data>();
            List<Data> data3 = new List<Data>();
            if (dt != null && dt.Rows.Count > 0)
            {
                int rowcount = dt.Rows.Count - 1;
                data1.Add(new Data() { name = "在线客服", y = decimal.Parse(dt.Rows[rowcount]["AgentOnlineCount"].ToString()) });
                data1.Add(new Data() { name = "暂离客服", y = decimal.Parse(dt.Rows[rowcount]["AgentBussyCount"].ToString()) });
                data1.Add(new Data() { name = "离线客服", y = decimal.Parse(dt.Rows[rowcount]["AgentLeaveCount"].ToString()) });
                data2.Add(new Data() { name = "接待客服", y = decimal.Parse(dt.Rows[rowcount]["AgentReceptionCount"].ToString()) });
                data2.Add(new Data() { name = "空闲客服", y = decimal.Parse(dt.Rows[rowcount]["AgentIdleount"].ToString()) });
                data3.Add(new Data() { name = "对话中访客量", y = DefaultChannelHandler.StateManager.GetConvertCountByBusinessLine("-1") });
                data3.Add(new Data() { name = "排队中访客量", y = DefaultChannelHandler.StateManager.GetQueueCountByBusinessLine("-1") });
                list.Add(new Series() { name = "", data = data1 });
                list.Add(new Series() { name = "", data = data2 });
                list.Add(new Series() { name = "", data = data3 });
            }
            else
            {
                data1.Add(new Data() { name = "在线客服", y = 0 });
                data1.Add(new Data() { name = "暂离客服", y = 0 });
                data1.Add(new Data() { name = "离线客服", y = 0 });
                data2.Add(new Data() { name = "接待客服", y = 0 });
                data2.Add(new Data() { name = "空闲客服", y = 0 });
                data3.Add(new Data() { name = "对话中访客量", y = DefaultChannelHandler.StateManager.GetConvertCountByBusinessLine("-1") });
                data3.Add(new Data() { name = "排队中访客量", y = DefaultChannelHandler.StateManager.GetQueueCountByBusinessLine("-1") });
                list.Add(new Series() { name = "", data = data1 });
                list.Add(new Series() { name = "", data = data2 });
                list.Add(new Series() { name = "", data = data3 });
            }
            string strJson = JsonConvert.SerializeObject(list);
            curContext.Response.Write(strJson);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public class Series
        {
            public string name;
            public List<Data> data;
        }

        public class Data
        {
            public string name;
            public decimal y;
        }
    }



}