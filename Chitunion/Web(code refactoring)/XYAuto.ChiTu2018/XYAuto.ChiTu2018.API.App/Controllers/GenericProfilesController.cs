using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Common;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.Profiles;
using XYAuto.ChiTu2018.Service.App.Profiles.Dto;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    [CrossSite]
    public class GenericProfilesController : ApiController
    {
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetAppGenericProfiles([FromBody]ConfigurationDto paramQuery)
        {
            var resultList = GenericProfilesService.Instance.GetConfigurationInfo(paramQuery);
            return Util.GetJsonDataByResult(resultList);
        }
    }
}
