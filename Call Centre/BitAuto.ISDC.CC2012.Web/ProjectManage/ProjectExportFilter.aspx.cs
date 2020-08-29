using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class ProjectExportFilter : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string projectid
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }
        public string Browser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }
        public string bussytype
        {
            get
            {

                return HttpContext.Current.Request["bussytype"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["bussytype"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           int userID = BLL.Util.GetLoginUserID();

           if (!BLL.Util.CheckRight(userID, "SYS024BUT500605"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}