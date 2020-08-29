using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.BUOC.Chitunion2017.NewWebAPI;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class DictInfoController : ApiController
    {
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public JsonResult GetDictInfoByTypeID(int typeID)
        {
            DataTable dt = XYAuto.ITSC.Chitunion2017.Common.DictInfo.Instance.GetDictInfoByTypeID(typeID);
            return Util.GetJsonDataByResult(dt, "Success");
        }
    }
}
