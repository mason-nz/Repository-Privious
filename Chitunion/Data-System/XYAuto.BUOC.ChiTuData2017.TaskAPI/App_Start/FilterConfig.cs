﻿using System.Web;
using System.Web.Mvc;

namespace XYAuto.BUOC.ChiTuData2017.TaskAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
