using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities.BusinessReport;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    /// <summary>
    /// B_ReturnVisitReport_Deal 的摘要说明
    /// </summary>
    public class B_ReturnVisitReport_Deal : IHttpHandler, IRequiresSessionState
    {

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        public string AgentID  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentID"); }
        }
        public string AgentNum
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentNum"); }
        }
        public string Year
        {
            get { return BLL.Util.GetCurrentRequestStr("Year"); }
        }
        public string Month
        {
            get { return BLL.Util.GetCurrentRequestStr("Month"); }
        }
        public string BGID
        {
            get { return BLL.Util.GetCurrentRequestStr("BGID"); }
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
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "getsurveycategory"://（问卷管理页面）根据业务组ID查询此业务组下的分类
                    //GetSurveyCategoryByBGID(out msg);
                    break;
                case "getreturnvisitsum":
                    GetReturnVisitSum(out msg);
                    break;

            }
            context.Response.Write(msg);
        }
        private void GetReturnVisitSum(out string msg)
        {

            msg = string.Empty;
            QueryReturnVisitReport model = new QueryReturnVisitReport();
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
            if (!string.IsNullOrEmpty(BGID))
            {
                int _bgid = 0;
                if (int.TryParse(BGID, out _bgid))
                {
                    model.BGID = _bgid;
                }
            }
            if (!string.IsNullOrEmpty(Year))
            {
                int _year = 0;
                if (int.TryParse(Year, out _year))
                {
                    model.Year = _year;
                }
            }
            if (!string.IsNullOrEmpty(Month))
            {
                int _month = 0;
                if (int.TryParse(Month, out _month))
                {
                    model.Month = _month;
                }
            }
            DataTable dt1 = BLL.ProjectInfo.Instance.GetB_ReturnVisitReportSum(model);
            if (dt1 != null && dt1.Rows.Count > 0 && dt1.Rows[0]["hfcount"] != DBNull.Value)
            {
                string dyfzmembercount = "0";
                string hfmembercount = "0";
                string hfcount = "0";
                string wjtcount = "0";
                string fglv = string.Empty;
                dyfzmembercount = dt1.Rows[0]["dyfzmembercount"] == DBNull.Value ? "0" : dt1.Rows[0]["dyfzmembercount"].ToString();
                hfmembercount = dt1.Rows[0]["hfmembercount"] == DBNull.Value ? "0" : dt1.Rows[0]["hfmembercount"].ToString();
                fglv = BLL.Util.ProduceLv(hfmembercount, dyfzmembercount);
                hfcount = dt1.Rows[0]["hfcount"] == DBNull.Value ? "0" : dt1.Rows[0]["hfcount"].ToString();
                wjtcount = dt1.Rows[0]["wjtcount"] == DBNull.Value ? "0" : dt1.Rows[0]["wjtcount"].ToString();
                msg = "{\"dyfzmembercount\":\"" + dyfzmembercount + "\",\"hfmembercount\":\"" + hfmembercount + "\",\"hfcount\":\"" + hfcount + "\",\"wjtcount\":\"" + wjtcount + "\",\"fglv\":\"" + fglv + "\"}";
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