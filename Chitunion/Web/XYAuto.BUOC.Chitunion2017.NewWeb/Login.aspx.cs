using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.BUOC.Chitunion2017.NewWeb
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoginLogic();
            }
        }

        private void LoginLogic()
        {
            string gourl = string.Empty;
            //int userid = -1;
            //int category = -1;
            XYAuto.ITSC.Chitunion2017.Common.LoginUser lu = new XYAuto.ITSC.Chitunion2017.Common.LoginUser();
            try
            {
                if (XYAuto.ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                    lu.UserID > 0)
                {
                    //XYAuto.ITSC.Chitunion2017.Common.UserRole ur = new XYAuto.ITSC.Chitunion2017.Common.UserRole(lu.UserID, lu.RoleIDs);
                    if (lu.Category == 29001)
                    {
                        gourl = "/manager/advertister/personal/PersonCenter.html";
                        Response.Redirect(gourl, false);
                    }
                    else if (lu.Category == 29002)
                    {
                        gourl = "/manager/media/personal/personalCenter.html";
                        Response.Redirect(gourl, false);
                    }
                    else
                    {
                        gourl = "/usermanager/NotAccessMsgPage.html";
                        Response.Redirect(gourl, false);
                    }

                    //DataTable dtParent = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(lu.UserID);
                    //if (dtParent != null)
                    //{
                    //    if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                    //    {
                    //        DataTable dtChild = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(lu.UserID, dtParent.Rows[0]["moduleid"].ToString());
                    //        if (dtChild.Rows.Count > 0)
                    //        {
                    //            gourl = dtChild.Rows[0]["url"].ToString();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        gourl = dtParent.Rows[0]["url"].ToString();
                    //    }
                    //    Response.Redirect(gourl, false);
                    //}
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}