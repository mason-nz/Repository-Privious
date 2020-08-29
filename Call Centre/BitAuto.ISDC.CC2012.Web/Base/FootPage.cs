using System;
using BitAuto.Utils.Config;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.Base
{
    public class FootPage : System.Web.UI.Page
    {
        /* 
        public static string pid = "";
        public static string moduleID = "";
        */

        public static DataView GetNewDataTable(DataTable dt, string condition)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = condition;

            return dv;//返回的查询结果
        }

        public static DataView GetNewDataTable(DataTable dt, string condition, string orderby)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = condition;
            dv.Sort = orderby;

            return dv;//返回的查询结果
        }

        public static string GetModuleID()
        {
            string pid = "";
            return BitAuto.YanFa.SysRightManager.Common.UserInfo.checkUserRight(ref pid);
        }

        public static string GetPid()
        {
            string pid = ""; BitAuto.YanFa.SysRightManager.Common.UserInfo.checkUserRight(ref pid);
            return pid;
        }
    }
}