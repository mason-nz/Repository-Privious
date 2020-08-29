using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Common;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.AppInfo;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    [CrossSite]
    public class FeedbackController : ApiController
    {/// <summary>
     /// 添加意见反馈
     /// </summary>
     /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AddFeedbackInfo([FromBody]LeFeedbackDto feedBackinfo)
        {
            var ResultNum = LeFeedbackService.Instance.AddFeedbackInfo(feedBackinfo);
            return Util.GetJsonDataByResult(null, ResultNum <= 0 ? "操作失败" : "OK", ResultNum <= 0 ? -1 : 0);
        }
    }
}
