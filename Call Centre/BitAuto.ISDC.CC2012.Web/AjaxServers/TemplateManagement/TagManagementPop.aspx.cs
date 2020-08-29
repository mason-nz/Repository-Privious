using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    public partial class TagManagementPop : PageBase
    {
        public string BGID
        {

            get
            {
                return HttpContext.Current.Request["bgid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["bgid"].ToString());
                //return "6";
            }
        }

        private string TagCID
        {
            //get { return "1"; }
            get { return HttpContext.Current.Request["c"] == null ? "0" : HttpUtility.UrlDecode(HttpContext.Current.Request["c"].ToString()); }

        }


        private string Status
        {
            //get { return "1"; }
            get { return HttpContext.Current.Request["s"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["s"].ToString()); }

        }

        public int userID
        {
            get { return BLL.Util.GetLoginUserID(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userID, "SYS024MOD5103"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
            else
            {
                DataBind();
                BindCatalog();
            }
        }

        private void DataBind()
        {
            DataTable dt = BLL.BusinessGroup.Instance.GetTagsByBG_Pid(BGID, TagCID, Status);
            rptTags.DataSource = dt;
            rptTags.DataBind();
        }

        public string GetZYTitle(string title)
        {
            if (title.Trim() == "1")
            {
                return "启用";
            }
            else if (title.Trim() == "0")
            {
                return "停用";
            }
            else
            {
                return ""; //删除
            }
        }

        public string GetDelTitle(string title)
        {
            if (title.Trim() == "-1")
            {
                return "";
            }
            else
            {
                return "删除"; //删除
            }
        }

        private void BindCatalog()
        {

            DataTable dt = BLL.BusinessGroup.Instance.GetTagsCatalogsByBG_Pid(BGID, TagCID);

            //retC.DataSource = dt;
            //retC.DataBind();
            rptCatalog.DataSource = dt;

            rptCatalog.DataBind();
        }
    }
}
