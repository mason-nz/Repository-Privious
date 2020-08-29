using System.Web;
using System.Web.Mvc;

namespace BitAuto.DSC.APPReport2016.WebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}