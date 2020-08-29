﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2018.AdminWeb
{
    public partial class login : System.Web.UI.Page
    {
        private string RequestGourl
        {
            get { return ITSC.Chitunion2017.BLL.Util.GetCurrentRequestQueryStr("gourl"); }
        }
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
                    Response.Redirect("/financemanager/presentmanagement-1.html");//跳转"财务管理-体现管理"页面
                    Response.End();
                }
                else
                {
                    string exitUrl = ConfigurationUtil.GetAppSettingValue("ExitAddress")+ "/login.aspx";
                    Response.Redirect(string.IsNullOrEmpty(RequestGourl) ? exitUrl : exitUrl + "?gourl=" + System.Web.HttpUtility.UrlEncode(RequestGourl), false);
                    Response.End();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}