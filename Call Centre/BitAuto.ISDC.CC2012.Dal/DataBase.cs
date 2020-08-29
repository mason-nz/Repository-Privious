using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils.Config;
using BitAuto.Utils;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class DataBase
    {
        //CC2012
        protected static string CONNECTIONSTRINGS = GetConnectionStrings("ConnectionStrings_CC");
        //CRM2009
        protected static string ConnectionStrings_CRM = GetConnectionStrings("ConnectionStrings");
        //SysRightsManager
        protected static string ConnectionStrings_SYS = GetConnectionStrings("Sys_ConnectionStrings");
        //BusinessDataStatistics
        protected static string BD_ConnectionStrings = GetConnectionStrings("BD_ConnectionStrings");
        //合力数据库-报表库
        protected static string ConnectionStrings_Holly_Report = GetConnectionStrings("ConnectionStrings_Holly_Report");
        //合力数据库-业务库
        protected static string ConnectionStrings_Holly_Business = GetConnectionStrings("ConnectionStrings_Holly_Business");

        protected static string SqlFilter(string str)
        {
            if (str != null)
            {
                return StringHelper.SqlFilter(str);
            }
            else
            {
                return null;
            }
        }

        private static string GetConnectionStrings(string key)
        {
            try
            {
                if (ConfigurationManager.AppSettings[key] != null)
                    return ConfigurationManager.AppSettings[key];
                else return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
