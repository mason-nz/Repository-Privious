using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Controllers
{
    public class TestController : ApiController
    {
        public string Get(int id)
        {
            Loger.Log4Net.Info($" this is Log4Net info");
            Loger.ZhyLogger.Info($" this is ZhyLogger info");
            Loger.ZhyLogger.Error($" this is ZhyLogger Error");
            return "value";
        }
    }
}
