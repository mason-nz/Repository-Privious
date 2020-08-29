using System.Web;
using System.Web.Mvc;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
