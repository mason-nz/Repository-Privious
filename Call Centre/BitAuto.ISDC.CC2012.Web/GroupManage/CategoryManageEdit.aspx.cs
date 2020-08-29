using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.GroupManage
{
    public partial class CategoryManageEdit : PageBase
    {
        public string SCID
        {
            get { return HttpContext.Current.Request["SCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SCID"].ToString()); }
        }
        public string CategoryName
        {
            get { return HttpContext.Current.Request["CategoryName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CategoryName"].ToString()); }
        }
        public string BGID
        {
            get { return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString()); }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024MOD500802"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            } 
        }
    }
}