using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Service.User;
using XYAuto.ChiTu2018.Service.User.Dto;

namespace XYAuto.ChiTu2018.API.Controllers
{
    public class LoginInfoController : ApiController
    {
        /// <summary>
        /// 网页端手机号登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult MobileLogin(ReqMobileInfoDto dto)
        {
            try
            {
                var errorMsg = UserManageService.Instance.MobileLogin(dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult(null, errorMsg, -2);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"LoginInfoController->MobileLogin", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult Exit()
        {
            try
            {
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"LoginInfoController->Exit", ex);
            }
            return Common.Util.GetJsonDataByResult(null);
        }
    }
}
