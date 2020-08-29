using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.Utils.Config;

namespace XYAuto.ITSC.Chitunion2017.Common.Dal
{
    public class DataBase
    {
        protected static string SYSCONNECTIONSTRINGS = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_ITSC");
    }
}
