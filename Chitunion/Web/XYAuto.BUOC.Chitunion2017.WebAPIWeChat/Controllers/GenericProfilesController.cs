using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.APP;
using XYAuto.ITSC.Chitunion2017.Entities.APP;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]
    public class GenericProfilesController : ApiController
    {
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetAppGenericProfiles([FromBody]ConfigurationModel paramQuery)
        {
            var resultList = GenericProfilesBll.Instance.GetConfigurationInfo(paramQuery);
            return Util.GetJsonDataByResult(resultList);
        }
    }
}
