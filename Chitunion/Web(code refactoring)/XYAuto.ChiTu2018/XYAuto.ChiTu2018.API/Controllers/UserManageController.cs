using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Entities.User.Dto;
using XYAuto.ChiTu2018.Service.User;
using XYAuto.ChiTu2018.Service.User.Dto;
using XYAuto.ChiTu2018.Service.UserBankAccount;
using XYAuto.ChiTu2018.Service.UserBankAccount.Dto;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 用户相关操作控制器
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class UserManageController : ApiController
    {
        /// <summary>
        ///查询个人信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult QueryUserInfo()
        {
            try
            {
                RespUserBasicDto dto = UserManageService.Instance.GetUserRelatedInfo();
                return Common.Util.GetJsonDataByResult(dto);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->QueryUserInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 查询认证信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult QueryUserDetailInfo()
        {
            try
            {
                RespUserDetailDto dto = UserManageService.Instance.GetUserDetail();
                return Common.Util.GetJsonDataByResult(dto);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->QueryUserDetailInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 保存支付宝账号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult SavePayInfo([FromBody]ReqPayInfoDto dto)
        {
            try
            {
                string errorMsg = UserBankAccountService.Instance.SavePayInfo(dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("保存失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->SavePayInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 保存认证信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult SaveUserDetail([FromBody]RespUserDetailDto dto)
        {
            try
            {
                string errorMsg = UserManageService.Instance.SaveUserDetail(dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("保存失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->SaveUserDetail出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •获取手机号状态
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>手机号状态 （0：不存在 1：该手机号码已注册，请更换手机号 2：该手机号已在pc端注册，是否做信息合并，-1：其他错误）</returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetMobileStatus(string mobile)
        {
            try
            {
                string errorMsg;
                object obj = UserManageService.Instance.GetMobileStatus(mobile, out errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(obj) : Common.Util.GetJsonDataByResult(null, errorMsg, -2);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->GetMobileStatus出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 发送或校验短信验证码||保存手机号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult SmsValidateCode([FromBody]ReqMobileInfoDto dto)
        {
            try
            {
                var num = UserManageService.Instance.SmsValidateCode(dto);
                return num == 0 ? Common.Util.GetJsonDataByResult(null, "OK", 0) : Common.Util.GetJsonDataByResult(null, "操作失败", num);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->SmsValidateCode出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 查询用户关联信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult QueryUserBasicInfo()
        {
            try
            {
                string errorMsg;
                var userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                var ret = UserManageService.Instance.GetUserRelatedInfoByMobile(string.Empty, out errorMsg, userId);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult(null, errorMsg, -2);
            }
            catch (Exception ex)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($"UserManageController->QueryUserBasicInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
    }
}
