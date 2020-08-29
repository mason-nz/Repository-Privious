using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils.Config;
using BitAuto.Utils;

namespace BitAuto.DSC.IM_2015.Dal
{
    public class DataBase
    {
        protected static string CONNECTIONSTRINGS = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_IMDMS2014");
        protected static string ConnectionStrings_CRM = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CRM");
        protected static string ConnectionStrings_SYS = ConfigurationUtil.GetAppSettingValue("Sys_ConnectionStrings");
        protected static string ConnectionStrings_CC = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
        //BusinessDataStatistics
        protected static string BD_ConnectionStrings = ConfigurationUtil.GetAppSettingValue("BD_ConnectionStrings");

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
    }
}
