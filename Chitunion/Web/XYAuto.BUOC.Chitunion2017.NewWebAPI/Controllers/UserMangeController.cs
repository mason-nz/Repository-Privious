using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Containers;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeOrder;
using XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.QrLogin;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.User;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using JsonResult = XYAuto.BUOC.Chitunion2017.NewWebAPI.Common.JsonResult;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class UserMangeController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong

        /// desc:帐号管理-提现帐号首次添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]

        public JsonResult FillWithdrawalsInfo([FromBody]ReqFillWithdrawalsInfoDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID

            }, new ReqWithdrawalsDto()).FirstFillWithdrawalsAccount(request);

            return jsonResult.GetReturn(retValue);
        }


        /// <summary>
        /// auth:lixiong
        /// desc:帐号管理-提现帐号更改
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult UpdateWithdrawalsInfo([FromBody]ReqFillWithdrawalsInfoDto request)
        {
            var jsonResult = new JsonResult();
            var retValue = new WithdrawalsProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }, new ReqWithdrawalsDto()).UpdateWithdrawalsAccount(request);

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// •查询账号基本信息接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public Common.JsonResult QueryUserBasicInfo([FromUri]ReqQueryUserBasicInfoDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.QueryUserBasicInfo(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"QueryUserBasicInfo出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •修改用户基本信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult EditUserBasicInfo([FromBody]ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserBasicInfo.ReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.EditUserBasicInfo(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"EditUserBasicInfo出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •修改用户认证信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult EditUserAuthenticationInfo([FromBody]ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserAuthenticationInfo.ReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.EditUserAuthenticationInfo(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"EditUserAuthenticationInfo出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •修改用户密码信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult EditUserPasswordInfo([FromBody]ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserPasswordInfo.ReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.EditUserPasswordInfo(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"EditUserPasswordInfo出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •修改用户手机号
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult EditUserMobileInfo([FromBody]ITSC.Chitunion2017.BLL.UserManage.Dto.EditUserMobileInfo.ReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.EditUserMobileInfo(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"EditUserMobileInfo:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •修改用户手机号(短信验证码是否正确)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult CheckSMSCode([FromBody]ITSC.Chitunion2017.BLL.UserManage.Dto.CheckSMSCode.ReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = req.CheckSelfModel(out errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"CheckSMSCode:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 校验用户是否存在密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult VerifyPassword()
        {
            var jsonResult = new JsonResult();

            var retValue = new UserProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }, null).VerifyUserPassword(new ReturnValue());

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 首次添加密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult AddPassword(ReqUserPasswordDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new UserProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }, null).AddPassword(request);

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 校验是否合并用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult VerifyMobileBind(ReqUserDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new UserProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID,
                LoginUser = GetUserInfo
            }, request).VerifyMobileBind();

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 确认合并用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult BindUser(ReqUserDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new UserProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID,
                LoginUser = GetUserInfo
            }, request).ModifyAttestation();

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 首次补充信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public Common.JsonResult FirstUpdateUser(ReqUserDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new UserProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID,
                LoginUser = GetUserInfo
            }, request).UpdateUser();

            return jsonResult.GetReturn(retValue);
        }
    }

}