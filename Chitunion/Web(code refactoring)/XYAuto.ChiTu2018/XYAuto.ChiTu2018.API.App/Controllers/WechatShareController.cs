using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Common;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.Service.App.AppInfo;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;
using XYAuto.ChiTu2018.Service.App.AppInfo.Provider;
using XYAuto.ChiTu2018.Service.App.ThirdApi.Dto.Request;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    /// <summary>
    /// auth:lixiong
    /// 分享奖励
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class WechatShareController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:分享之前的推广url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetPostOrderUrl([FromUri] ReqTaskReceiveDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            jsonResult.Result = new ShareProvider(new ConfigEntity(), new ReqCreateShareDto()).GetOrderUrl(request.TaskId).ReturnObject;
            return jsonResult;
        }


        /// <summary>
        /// auth:lixiong
        /// desc:app客户端分享（包括签到，首次欢迎奖励，提现成功奖励）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult CreateOrder([FromBody] ReqCreateShareDto reqCreate)
        {
            var jsonResult = new JsonResult();
            var retValue = new ShareProvider(new ConfigEntity()
            {
                UserId = GetUserInfo.UserID,
                Ip = XYAuto.CTUtils.Html.RequestHelper.GetIpAddress()
            }, reqCreate).Log();
            return jsonResult.GetReturn(retValue);
        }
    }
}
