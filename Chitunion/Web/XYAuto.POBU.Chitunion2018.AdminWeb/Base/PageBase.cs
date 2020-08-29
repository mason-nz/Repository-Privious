using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.Web.Base;

namespace XYAuto.BUOC.Operation2017.Web.Base
{
    public class PageBase : FootPage
    {
        public static string getModuleID()
        {
            string pid = "";
            return ITSC.Chitunion2017.Common.UserInfo.CheckUserRight(ref pid);
        }
        public static string getPid()
        {
            string pid = "";
            ITSC.Chitunion2017.Common.UserInfo.CheckUserRight(ref pid);
            return pid;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            string pid = "";
            string moduleID = ITSC.Chitunion2017.Common.UserInfo.CheckUserRight(ref pid);
        }
    }
}