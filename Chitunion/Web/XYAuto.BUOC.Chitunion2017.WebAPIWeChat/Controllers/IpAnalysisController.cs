using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.WeChat;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]
    public class IpAnalysisController : ApiController
    {
        [ApiLog]
        [HttpGet]
        public void GetAreaAddressByIp()
        {
            string url = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() : HttpContext.Current.Request.Url + string.Empty;
            IpAnalysisBll.Instance.GetAreaAddressByIp(
              ITSC.Chitunion2017.BLL.Util.GetIP("获取APP客户端IP地理位置："), url
                );
        }
    }
}
