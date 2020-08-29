using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.PublicApi.Common;
using XYAuto.ChiTu2018.PublicApi.Filter;
using XYAuto.ChiTu2018.PublicApi.Models;
using XYAuto.ChiTu2018.Service.App.PublicService.Dto.Request.User;

namespace XYAuto.ChiTu2018.PublicApi.Controllers
{
    /// <summary>
    /// 公共业务接口-用户相关逻辑
    /// </summary>
    [CrossSite]
    public class UserController : ApiController
    {
        /// <summary>
        /// 微信授权操作用户
        /// </summary>
        /// <param name="requset"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult WxUserOperation([FromBody]PsReqPostWxUserOperationDto requset)
        {
            var jsonResult = new JsonResult();

            var retValue = Service.App.PublicService.PsWxUserAuthService.Instance.WeiXinUserOperation(requset);

            return jsonResult.GetReturn(retValue);
        }
    }
}
