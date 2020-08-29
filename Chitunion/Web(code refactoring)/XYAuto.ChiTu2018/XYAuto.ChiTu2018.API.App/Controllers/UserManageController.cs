using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;
using XYAuto.ChiTu2018.Service.App.AppInfo.Provider;
using XYAuto.ChiTu2018.Service.App.User;
using XYAuto.ChiTu2018.Service.App.User.Dto;
using XYAuto.CTUtils.Log;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    [CrossSite]
    public class UserManageController : ApiController
    {
        /// <summary>
        ///查询个人信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetUserInfo()
        {
            try
            {
                var dto = UserService.Instance.GetUserRelatedInfo(UserInfo.GetLoginUserID());
                return Common.Util.GetJsonDataByResult(dto);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->QueryUserInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 获取手机号状态（0：不存在 1：该手机号码已注册，请更换手机号 2：该手机号已在pc端注册，是否做信息合并，-1：手机号格式不正确）
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SelectMobileStatus(string Mobile)
        {
            try
            {
                string errorMsg = string.Empty;
                object obj = UserService.Instance.GetMobileStatus(Mobile, UserInfo.GetLoginUserID());
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(obj) : Common.Util.GetJsonDataByResult(null, errorMsg, -1);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info($"SelectMobileStatus:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AddUserInfo(ModifyAttestationReqDto dto)
        {
            try
            {
                string errorMsg = UserService.Instance.AddUserInfo(dto, UserInfo.GetLoginUserID());
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("保存失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error($"AddUserInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult SendValidateCode([FromBody]AppReqMobileDto dto)
        {
            try
            {
                var num = UserService.Instance.SendValidateCode(dto, false);
                return num == 0 ? Common.Util.GetJsonDataByResult(null, "OK", 0) : Common.Util.GetJsonDataByResult(null, "操作失败", num);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->SendValidateCode出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 获取消息通知设置配置信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMsgPushConfig([FromBody]ReqAppPushSwitchDto request)
        {
            var jsonResult = new JsonResult();
            jsonResult.Result = new AppPushMsgSwitchLogProvider(request).GetPushConfig();
            return jsonResult;
        }

        /// <summary>
        /// 用户手动关闭提示
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ClosedMsgPushTips([FromBody]ReqAppPushSwitchDto request)
        {
            var jsonResult = new JsonResult();
            new AppPushMsgSwitchLogProvider(request).ClosedPushTips();
            return jsonResult;
        }
    }
}
