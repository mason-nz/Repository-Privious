using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.Utils.Config;
using BitAuto.Utils;

namespace BitAuto.DSC.IM2014.Dal
{
    public class DataBase
    {
        protected static string CONNECTIONSTRINGS = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
        
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
