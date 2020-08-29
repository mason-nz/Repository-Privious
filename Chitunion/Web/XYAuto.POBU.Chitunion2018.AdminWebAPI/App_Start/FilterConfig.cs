using System.Web;
using System.Web.Mvc;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
