using System.Web;
using System.Web.Mvc;

namespace XYAuto.ITSC.Chitunion2017.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}