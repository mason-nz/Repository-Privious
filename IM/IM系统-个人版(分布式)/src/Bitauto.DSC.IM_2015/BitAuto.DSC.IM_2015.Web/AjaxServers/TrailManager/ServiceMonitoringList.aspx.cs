using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;
using System.Configuration;
using BitAuto.DSC.IM_2015.Web.Channels;
namespace BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager
{
    public partial class ServiceMonitoringList : System.Web.UI.Page
    {
        //客服状态
        private string Status
        {
            get
            {
                return HttpContext.Current.Request["Status"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString());
            }
        }
        //客服ID
        private string UserID
        {
            get
            {
                return HttpContext.Current.Request["UserID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserID"].ToString());
            }
        }
        //员工编号
        private string AgentNum
        {
            get
            {
                return HttpContext.Current.Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString());
            }
        }
        //客服ID
        private string GroupID
        {
            get
            {
                return HttpContext.Current.Request["GroupID"] == null ? "-1" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["GroupID"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }
        //绑定数据
        private void BindData()
        {
            DataTable dt = DefaultChannelHandler.StateManager.GetServiceMonitoring(GroupID, UserID, AgentNum, Status);
            rpeList.DataSource = dt;
            rpeList.DataBind();
        }

        public string GetAvg(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return (float.Parse(str) * 100).ToString("N2") + "%";
            }
            return "-";
        }
    }
}