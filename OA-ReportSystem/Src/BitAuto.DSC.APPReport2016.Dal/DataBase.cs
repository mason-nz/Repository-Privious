using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class DataBase
    {
        public static readonly string ConnectionStrings = ConfigurationManager.ConnectionStrings["ConnectionStrings"].ConnectionString;
    }
}
