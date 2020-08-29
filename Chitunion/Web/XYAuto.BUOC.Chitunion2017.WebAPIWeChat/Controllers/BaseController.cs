using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected string AppId
        {
            get
            {
                return WebConfigurationManager.AppSettings["WeixinAppId"];
            }
        }
    }
}