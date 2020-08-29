using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class DictInfoController : ApiController
    {
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDictInfoByTypeID(int typeID)
        {
            DataTable dt = Chitunion2017.Common.DictInfo.Instance.GetDictInfoByTypeID(typeID);
            return Util.GetJsonDataByResult(dt, "Success");
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDictInfoByAPP()
        {
            DataTable dt = Chitunion2017.Common.DictInfo.Instance.GetDictInfoByAPP();
            return Util.GetJsonDataByResult(dt, "Success");
        }
    }
}
