using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    /// <summary>
    /// Summary description for AgentTimeState
    /// </summary>
    public class AgentTimeState : IHttpHandler,IRequiresSessionState
    {
        #region 属性
        private int RequestBGID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["BGID"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"])); }
        }
        private string RequestAgentNum
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentNum"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString()); }
        }        
        private int RequestAgentUserID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentUserID"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["AgentUserID"])); }
        }
        private int RequestAgentState
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentState"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["AgentState"])); }
        }
        private int RequestLoginUserID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["LoginUserID"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["LoginUserID"])); }
        }
        private int RequestPageSize
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["PageSize"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["PageSize"])); }
        }
        private int RequestPageIndex
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["page"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["page"])); }
        }        
        #endregion

        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            currentContext = context;
            if ((context.Request["showAgentTimeState"] + "").Trim() == "yes")
            {
                //DataTable dt = BitAuto.ISDC.CC2012.BLL.AgentTimeState.Instance.GetAllAreaFromDB();
                Entities.QueryAgentState query = new Entities.QueryAgentState();
                query.AgentNum = RequestAgentNum;
                query.BGID = RequestBGID;
                query.UserID = RequestAgentUserID;
                query.State = RequestAgentState;
                query.LoginID = RequestLoginUserID;

                //将当前时间转换成时间戳s
                DateTime dtstart = new DateTime(1970, 1, 1);
                DateTime dtnow = DateTime.Now;
                int dtnowtime = (int)(dtnow - dtstart).TotalSeconds;

                string order = " c.State desc,AGTime asc";
                DataTable dt = BitAuto.ISDC.CC2012.BLL.AgentTimeState.Instance.GetAllStateFromDB(query, order, RequestPageIndex, RequestPageSize, out RecordCount);
                if (dt.Rows.Count>0)
                {

                    StringBuilder sb = new StringBuilder();
                    string agtime = "";
                    string state = "";

                    #region 读取置忙、通话显示自动变红时长参数
                    string redtime = System.Configuration.ConfigurationManager.AppSettings["CAgentRedSetting"];                    
                    #endregion

                    string pagerHtml = "";
                    pagerHtml = BLL.PageCommon.Instance.LinkStringByPost(GroupLength, RecordCount, RequestPageSize, RequestPageIndex, 1);

                    List<JSONAgentState> listAgentState = new List<JSONAgentState>();
                    foreach (DataRow row in dt.Rows)
                    {
                        state = GetAgentState(Convert.ToInt32(row["State"]),Convert.ToInt32(row["AgentAuxState"]));
                        //agtime = ConvertTime(Convert.ToInt32(row["AGTime"]));
                        
                        int totaltime = dtnowtime - Convert.ToInt32(row["AGTime"]);
                        bool isred = false;
                        if (totaltime > Convert.ToInt32(redtime) && (state.Contains("置忙") || state.Contains("工作中")))
                        {
                            isred = true;
                        }
                        
                        agtime = ConvertTime(totaltime);

                        JSONAgentState agentstate = new JSONAgentState();
                        agentstate.GroupName = row["Name"].ToString();
                        agentstate.AgentName = row["AgentName"].ToString();
                        agentstate.AgentNum = row["AgentNum"].ToString();
                        agentstate.AgentID = row["ExtensionNum"].ToString();
                        agentstate.State = state;
                        agentstate.StartTime = row["StartTime"].ToString();
                        agentstate.AGTime = agtime;
                        agentstate.IsRed = isred.ToString();
                        agentstate.PagerHtml = pagerHtml;

                        listAgentState.Add(agentstate);
                        //sb.Append("{'GroupName':'" + row["Name"].ToString() + "','AgentName':'" + row["AgentName"].ToString() + "','AgentNum':'" + row["AgentNum"].ToString() + "','AgentID':'" + row["ExtensionNum"].ToString() +
                        //          "','State':'" + state + "','StartTime':'" + row["StartTime"].ToString() + "','AGTime':'" + agtime + "','IsRed':'" + isred + "','PagerHtml':'" + pagerHtml +
                        //          "'},");
                    }
                    
                    //message = sb.ToString();
                    //if (message.EndsWith(","))
                    //    message = message.Substring(0, message.Length - 1);
                    message = Newtonsoft.Json.JavaScriptConvert.SerializeObject(listAgentState);
                   
                }
                //context.Response.Write("[" + message + "]");
                context.Response.Write(message);
                context.Response.End();
            }           
            else
            {
                success = false;
                message = "request error";
                BitAuto.ISDC.CC2012.BLL.AJAXHelper.WrapJsonResponse(success, result, message);
            }
        }

        private string GetAgentState(int state,int auxstate)
        {
            string retval = "";
            switch (state)
            {
                case 0:
                    retval = "未知";
                    break;
                case 1:
                    retval = "签出";
                    break;
                case 2:
                    retval = "签入";
                    break;
                case 3:
                    retval = "置闲";
                    break;
                case 4:
                    retval = "置忙";
                    if (auxstate == 1)
                    {
                        retval = retval + "(小休)";
                    }
                    else if (auxstate == 2)
                    {
                        retval = retval + "(任务回访)";
                    }
                    else if (auxstate == 3)
                    {
                        retval = retval + "(业务处理)";
                    }
                    else if (auxstate == 4)
                    {
                        retval = retval + "(会议)";
                    }
                    else if (auxstate == 5)
                    {
                        retval = retval + "(培训)";
                    }
                    else if (auxstate == 6)
                    {
                        retval = retval + "(离席)";
                    }
                    break;
                case 5:
                    retval = "话后";
                    break;
                case 7:
                    retval = "初始化";
                    break;
                case 8:
                    retval = "振铃";
                    break;
                case 9:
                    retval = "工作中";
                    break;
                default:
                    retval = "未知";
                    break;
            }
            return retval;
        }

        private string ConvertTime(int iSec)
        {    
            string retval = "";

            int iHour = 0;
            int iHour2 = 0;
            int iMinute = 0;
            int iSecond = 0;

            iHour = (iSec/3600);
            iHour2 = (iSec%3600);

            iMinute = (iHour2/60);
            iSecond = (iHour2%60);

            if (iSec < 0)
            {
                retval = "00:00:00";
            }
            else
            {
                string sHour = "", sMinute = "", sSecond = "";
                if (iHour < 10)
                    sHour = "0" + iHour.ToString();
                else
                {
                    sHour = iHour.ToString();
                }

                if (iMinute < 10)
                    sMinute = "0" + iMinute.ToString();
                else
                    sMinute = iMinute.ToString();

                if (iSecond < 10)
                    sSecond = "0" + iSecond.ToString();
                else
                    sSecond = iSecond.ToString();

                retval = sHour + ":" + sMinute + ":" + sSecond;
            }

            return retval;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class JSONAgentState
    {
        public string GroupName { get; set; }
        public string AgentName { get; set; }
        public string AgentNum { get; set; }
        public string AgentID { get; set; }
        public string State { get; set; }
        public string StartTime { get; set; }

        public string AGTime { get; set; }
        public string IsRed { get; set; }
        public string PagerHtml { get; set; }        
    }
}