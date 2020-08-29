using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class SMSTemplateEdit : PageBase
    {
        public string RequestRecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }
        public string BGID = string.Empty;
        public string SCID = string.Empty;
        public string smstitle = string.Empty;
        public string smscontent = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD510401") && !BLL.Util.CheckRight(userID, "SYS024MOD510402"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                if (!string.IsNullOrEmpty(RequestRecID))
                {
                    int _recid = 0;
                    if (int.TryParse(RequestRecID, out _recid))
                    {
                        Entities.SMSTemplate model = BLL.SMSTemplate.Instance.GetSMSTemplate(_recid);
                        BGID = model.BGID.ToString();
                        SCID = model.SCID.ToString();
                        smstitle = model.Title;
                        smscontent = model.Content;
                    }
                }
            }
        }
    }
}