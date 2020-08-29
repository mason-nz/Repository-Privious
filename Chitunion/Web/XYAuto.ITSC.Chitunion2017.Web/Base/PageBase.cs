using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.Web.Base
{
    public class PageBase : FootPage
    {
        public static string getModuleID()
        {
            string pid = "";
            return Chitunion2017.Common.UserInfo.CheckUserRight(ref pid);
        }
        public static string getPid()
        {
            string pid = "";
            Chitunion2017.Common.UserInfo.CheckUserRight(ref pid);
            return pid;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            string pid = "";
            string moduleID = Chitunion2017.Common.UserInfo.CheckUserRight(ref pid);
        }
    }
}