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
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;
using XYAuto.ChiTu2018.Service.App.AppInfo.Provider;
using XYAuto.ChiTu2018.Service.App.User;
using XYAuto.ChiTu2018.Service.App.User.Dto;
using XYAuto.ChiTu2018.Service.App.User.Dto.LoginForMobile;
using XYAuto.ChiTu2018.Service.App.User.Dto.LoginForWeChat;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    [CrossSite]
    public class AndroidController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:手机设备上报
        /// </summary>
        /// <param name="reqReportApp"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [CTAppAuth]
        public JsonResult Report([FromBody] ReqReportAppDto reqReportApp)
        {
            var jsonResult = new JsonResult();
            if (reqReportApp == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            reqReportApp.UserId = GetUserInfo == null ? 0 : GetUserInfo.UserID;
            var retValue = new ReportAppInfoProvider(reqReportApp).Report();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 消息通知设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public JsonResult SetMsgPushConfig([FromBody] ReqAppPushSwitchDto request)
        {
            var jsonResult = new JsonResult();
            var retValue = new AppPushMsgSwitchLogProvider(request).SetPushConfig();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public JsonResult LoginForWeChat([FromBody]ReqLoginDto request)
        {
            try
            {
                string errorMsg = string.Empty;
                request.Ip = System.Web.HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
                var resp = Service.App.User.UserService.Instance.LoginForWeChat(request, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(resp) : Common.Util.GetJsonDataByResult(null, errorMsg, -2);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("APP微信登录出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// APP退出登录
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public JsonResult ExitForAndroid()
        {
            try
            {
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("AndroidController->ExitForAndroid出错", ex);
            }
            return Common.Util.GetJsonDataByResult(null);
        }
        /// <summary>
        /// APP发送验证码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public JsonResult SendCodeForAndroid(ReqMobileLoginDto dto)
        {
            try
            {
                var errorMsg = UserService.Instance.SendValidateCode(new AppReqMobileDto() { Mobile = dto.mobile }, true);
                return errorMsg == 0 ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult(errorMsg.ToString(), "发送失败", -2);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("AndroidController->SendCodeForAndroid出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// APP登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public JsonResult LoginForAndroid(ReqMobileLoginDto dto)
        {
            try
            {
                string errorMsg;
                var resp = UserService.Instance.LoginForAndroid(dto, out errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(resp) : Common.Util.GetJsonDataByResult(resp, errorMsg, -2);
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error("AndroidController->LoginForAndroid", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }



    }
}
