using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    public class HomeController : Controller
    {
        
    }

    public class ReqLoginCallBackDto
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
