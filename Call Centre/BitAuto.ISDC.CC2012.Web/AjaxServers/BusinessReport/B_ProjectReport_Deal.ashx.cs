using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.BusinessReport;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    /// <summary>
    /// B_ProjectReport_Deal 的摘要说明
    /// </summary>
    public class B_ProjectReport_Deal : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string AgentID  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentID"); }
        }
        public string AgentNum
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentNum"); }
        }
        public string StartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("StartTime"); }
        }
        public string EndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndTime"); }
        }
        public string BusinessType
        {
            get { return BLL.Util.GetCurrentRequestStr("BusinessType"); }
        }
        public string ProjectID
        {
            get { return BLL.Util.GetCurrentRequestStr("ProjectID"); }
        }


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                //取当前人管辖分组下时间最近的项目
                case "getlastestproject":
                    GetLastestProject(out msg);
                    break;
                case "getprojectreprotsum":
                    GetProjectReportSum(out msg);
                    break;

            }
            context.Response.Write(msg);
        }
        /// <summary>
        /// 取项目报表汇总数据
        /// </summary>
        /// <param name="msg"></param>
        public void GetProjectReportSum(out string msg)
        {
            msg = string.Empty;
            QueryProjectReport model = new QueryProjectReport();
            if (!string.IsNullOrEmpty(AgentID))
            {
                int _userid = 0;
                if (int.TryParse(AgentID, out _userid))
                {
                    model.UserID = _userid;
                }
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                model.AgentNum = AgentNum;
            }
            if (!string.IsNullOrEmpty(ProjectID))
            {
                int _projectid = 0;
                if (int.TryParse(ProjectID, out _projectid))
                {
                    model.ProjectID = _projectid;
                }
            }
            if (!string.IsNullOrEmpty(BusinessType))
            {
                int _businesstype = 0;
                if (int.TryParse(BusinessType, out _businesstype))
                {
                    model.BusinessType = _businesstype;
                }
            }
            else
            {//默认是其他任务
                model.BusinessType = 4;
            }
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                DateTime _begintime = DateTime.Now;
                DateTime.TryParse(StartTime, out _begintime);
                DateTime _endtime = DateTime.Now;
                DateTime.TryParse(EndTime, out _endtime);
                model.BeginTime = _begintime;
                model.EndTime = _endtime;

            }
            int userID = BLL.Util.GetLoginUserID();
            DataTable dt1 = BLL.ProjectInfo.Instance.GetB_ProjectReportSum(model, userID);

            if (dt1 != null && dt1.Rows.Count > 0)
            {
                string projectname = string.Empty;
                projectname = dt1.Rows[0]["projectname"].ToString();
                string assigncount = string.Empty;
                assigncount = dt1.Rows[0]["assigncount"] == DBNull.Value ? "-" : dt1.Rows[0]["assigncount"].ToString();
                string tjcount = string.Empty;
                tjcount = dt1.Rows[0]["tjcount"] == DBNull.Value ? "0" : dt1.Rows[0]["tjcount"].ToString();
                string jtcount = string.Empty;
                jtcount = dt1.Rows[0]["jtcount"] == DBNull.Value ? "0" : dt1.Rows[0]["jtcount"].ToString();
                string successcount = string.Empty;
                successcount = dt1.Rows[0]["successcount"] == DBNull.Value ? "0" : dt1.Rows[0]["successcount"].ToString();
                string wjtcount = string.Empty;
                wjtcount = dt1.Rows[0]["wjtcount"] == DBNull.Value ? "0" : dt1.Rows[0]["wjtcount"].ToString();
                string jtfailcount = string.Empty;
                jtfailcount = dt1.Rows[0]["jtfailcount"] == DBNull.Value ? "0" : dt1.Rows[0]["jtfailcount"].ToString();

                string jtlv = string.Empty;
                jtlv = BLL.Util.ProduceLv(jtcount, tjcount);
                string cglv = string.Empty;
                cglv = BLL.Util.ProduceLv(successcount, jtcount);


                msg = "{\"projectname\":\"" + projectname + "\",\"assigncount\":\"" + assigncount + "\",\"tjcount\":\"" + tjcount + "\",\"jtcount\":\"" + jtcount + "\",\"successcount\":\"" + successcount + "\",\"wjtcount\":\"" + wjtcount + "\",\"jtfailcount\":\"" + jtfailcount + "\",\"jtlv\":\"" + jtlv + "\",\"cglv\":\"" + cglv + "\"}";
            }


        }

        //取当前人管辖分组下时间最近的项目
        public void GetLastestProject(out string msg)
        {
            int userid = BLL.Util.GetLoginUserID();
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
            //项目类型
            query.Sources = "4,6";
            int count;
            DataTable dt = BLL.ProjectInfo.Instance.GetLastestProjectByUserID(query, " a.createtime desc", 1, 1, out count, userid);
            msg = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                msg = "{projectid:" + dt.Rows[0]["ProjectID"].ToString()
                    + ",projectname:'" + dt.Rows[0]["Name"].ToString()
                    + "',source:" + dt.Rows[0]["Source"].ToString()
                    + ",ProjectTime:'" + dt.Rows[0]["createtime"].ToString() + "'}";
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}