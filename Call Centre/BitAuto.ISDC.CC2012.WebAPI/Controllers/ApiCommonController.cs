using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using BitAuto.ISDC.CC2012.WebAPI.Models;
using Newtonsoft.Json;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{

    public class ApiCommonController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public string GetServerDateTime()
        {
            var res = new CommonJsonResult()
            {
                ErrorNumber = 0,
                Success = true,
                data = DateTime.Now
            };
            return JsonConvert.SerializeObject(res, Formatting.None);

        }

        [AllowAnonymous]
        [HttpGet]
        public string GetDbDateTime()
        {
            var res = new CommonJsonResult()
            {
                ErrorNumber = 0,
                Success = true,
                data = Util.GetDBDateTime()
            };
            return JsonConvert.SerializeObject(res, Formatting.None);

        }
    }
}
