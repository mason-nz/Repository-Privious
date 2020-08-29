using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ClientLogRequire.Ajax
{
    public partial class AjaxList : System.Web.UI.Page
    {
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        public string Name
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("name");
            }
        }
        public string StartTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("date_st");
            }
        }
        public string EndTime
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("date_et");
            }
        }
        public string VendorID
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("vendor");
            }
        }
        public string Online
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("online");
            }
        }
        public string NoLogin
        {
            get
            {
                return BLL.Util.GetCurrentRequestQueryStr("nologin");
            }
        }

        public int PageIndex { get { return BLL.PageCommon.Instance.PageIndex; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (NoLogin == BitAuto.ISDC.CC2012.Web.ClientLogRequire.Ajax.LogRequireHandler.NoLoginKey)
                {
                    //从简化版本进来的，不用验证
                }
                else
                {
                    int userId = BLL.Util.GetLoginUserID();
                    if (!BLL.Util.CheckRight(userId, "SYS024MOD5009"))
                    {
                        Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                        Response.End();
                    }
                }
                BindData();
            }
        }

        private void BindData()
        {
            ClientLogRequireQuery query = new ClientLogRequireQuery();
            query.StartTime = CommonFunction.ObjectToDateTime(StartTime, DateTime.Today);
            query.EndTime = CommonFunction.ObjectToDateTime(EndTime, DateTime.Today);
            query.Name = Name;
            query.Vendor = VendorID;
            query.Online = Online;

            DataTable dt = BLL.ClientLogRequire.Instance.GetAllEmployeeAgent(query, BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
            }
        }

        public string GetOper(string logdate, string userid, string vendorid,
            string logstatus, string responsesuccess,
            string filepath, string agentname, string vendorname)
        {
            string info = "";
            if (logstatus == "0")
            {
                //请求日志
                info += "<a href=\"javascript:void(0)\" onclick=\"RequireLog('" + logdate + "','" + userid + "','" + vendorid + "')\">请求日志</a>&nbsp;&nbsp;";
            }
            else if (logstatus == "2")
            {
                //重新请求
                info += "<a href=\"javascript:void(0)\" onclick=\"RequireLog('" + logdate + "','" + userid + "','" + vendorid + "')\">重新请求</a>&nbsp;&nbsp;";
                if (responsesuccess == "1")
                {
                    //下载
                    info += "<a href=\"javascript:void(0)\" onclick=\"DownLoadFile(this,'" + filepath + "','" + agentname + "','" + vendorname + "')\">下载</a>&nbsp;&nbsp;";
                }
            }
            return info;
        }
    }
}