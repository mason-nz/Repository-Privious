using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.Service.App.IpMonitor;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    [CrossSite]
    public class IpAnalysisController : ApiController
    {
        [ApiLog]
        [HttpGet]
        public void GetAreaAddressByIp()
        {
            string url = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() : HttpContext.Current.Request.Url + string.Empty;

            IpAnalysisService.Instance.GetAreaAddressByIp(
                XYAuto.CTUtils.Html.RequestHelper.GetIpAddress(), url);
        }
    }
}
