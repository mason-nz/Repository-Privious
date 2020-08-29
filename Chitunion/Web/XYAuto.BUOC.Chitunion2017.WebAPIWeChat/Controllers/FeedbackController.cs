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
    public class FeedbackController : ApiController
    {
        /// <summary>
        /// 添加意见反馈
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AddFeedbackInfo([FromBody]FeedbackModel feedBackinfo)
        {
            var ResultNum = FeedbackBll.Instance.AddFeedbackInfo(feedBackinfo);
            return Util.GetJsonDataByResult(null, ResultNum <= 0 ? "操作失败" : "OK", ResultNum <= 0 ? -1 : 0);
        }
    }
}
