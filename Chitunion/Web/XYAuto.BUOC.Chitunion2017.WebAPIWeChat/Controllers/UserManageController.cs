using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.UserManage.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.PayInfo;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]

    public class UserManageController : ApiController
    {
        /// <summary>
        /// •查询账号基本信息接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult QueryUserBasicInfo()
        {
            try
            {
                string errorMsg = string.Empty;
                ReqQueryUserBasicInfoDto req = new ReqQueryUserBasicInfoDto() { UserID = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID() };
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.QueryUserBasicInfo_M(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"QueryUserBasicInfo出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// •获取手机号状态
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <returns>手机号状态 （0：不存在 1：该手机号码已注册，请更换手机号 2：该手机号已在pc端注册，是否做信息合并，-1：其他错误）</returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetMobileStatus(string Mobile)
        {
            try
            {
                string errorMsg = string.Empty;
                object obj = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetMobileStatus(Mobile, out errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(obj) : Common.Util.GetJsonDataByResult(null, errorMsg, -1);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetMobileStatus:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 根据手机号查询用户信息
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult QueryUserInfo(string Mobile)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.QueryUserInfo(Mobile, out errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"QueryUserInfo:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 验证支付宝账号是否可用
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult VerifBankAccount([FromBody]BankAccountReqDto req)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.VerifBankAccount(req);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"VerifBankAccount:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 获取审核状态和微信昵称
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetExamineStatus()
        {
            try
            {
                Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetExamineStatus();
                return Common.Util.GetJsonDataByResult(dic);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetExamineStatus出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 修改用户认证信息
        /// </summary>
        /// <param name="DTO"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult ModifyMobileAccount(ModifyMobileReqDto DTO)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.ModifyMobileAccount(DTO);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"ModifyMobileAccount:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 获取绑定微信按钮的显示
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetWxBindConfig()
        {
            var jsonResult = new Common.JsonResult();
            var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            var retValue = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetWxBindConfig(userId);
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 获取消息通知设置配置信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetMsgPushConfig([FromBody]ReqAppPushSwitchDto request)
        {
            var jsonResult = new Common.JsonResult();
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
        public Common.JsonResult ClosedMsgPushTips([FromBody]ReqAppPushSwitchDto request)
        {
            var jsonResult = new Common.JsonResult();
            new AppPushMsgSwitchLogProvider(request).ClosedPushTips();
            return jsonResult;
        }


        #region  V2.5微信版
        /// <summary>
        ///查询个人信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public Common.JsonResult QueryUserInfo()
        {
            try
            {
                Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.QueryUserInfo();
                return Common.Util.GetJsonDataByResult(dic);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"QueryUserInfo:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 查询认证信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult QueryUserDetailInfo()
        {
            try
            {
                Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.QueryUserDetailInfo();
                return Common.Util.GetJsonDataByResult(dic);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"QueryUserDetailInfo:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 保存支付宝账号
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SavePayInfo(PayInfoDto Dto)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.SavePayInfo(Dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("保存失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"SavePayInfo:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 保存认证信息
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult SaveUserDetail(ReqUserDetailDto Dto)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.SaveUserDetail(Dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("保存失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"SaveUserDetail:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        #endregion

        #region H5版本
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult Exit()
        {
            try
            {
                int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"H5页用户：{userId}退出登录开始");
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"H5页用户：{userId}退出登录结束");
            }
            catch (Exception ex)
            {
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("H5页退出赤兔系统时出错", ex);
            }
            return Common.Util.GetJsonDataByResult(null);
        }
        /// <summary>
        /// 快捷登录
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult Login(ReqLoginDTO Dto)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.Login(Dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult(null, errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"Login:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public Common.JsonResult SendValidateCode(ReqLoginDTO Dto)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.SendValidateCode(Dto);

                return errorMsg == "0" ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult(errorMsg, "发送失败", -1);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"SendValidateCode:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        #endregion

        #region V2.1.0
        /// <summary>
        ///查询个人信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetUserInfo()
        {
            try
            {
                Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfo();
                return Common.Util.GetJsonDataByResult(dic);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetUserInfo:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        ///根据手机号查询个人信息
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult GetUserInfoByMobile(string mobile)
        {
            try
            {
                Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByMobile(mobile);
                return Common.Util.GetJsonDataByResult(dic);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"GetUserInfoByMobile:{ex.Message},StackTrace:{ex.StackTrace}");
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
        public Common.JsonResult SelectMobileStatus(string Mobile)
        {
            try
            {
                string errorMsg = string.Empty;
                object obj = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetMobileStatus(Mobile);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(obj) : Common.Util.GetJsonDataByResult(null, errorMsg, -1);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"SelectMobileStatus:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult AddUserInfo(ModifyAttestationReqDto Dto)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.AddUserInfo(Dto);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult("保存失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error($"AddUserInfo出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 查看用户是否是黑名单用户
        /// </summary>
        /// <returns>若是返回True，否则返回False</returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public Common.JsonResult IsBlackUser()
        {
            int userid = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
            //根据UserID查询  SELECT * FROM dbo.LE_User_Blacklist WHERE Status=0 AND UserID={}
            return Common.Util.GetJsonDataByResult(ITSC.Chitunion2017.BLL.IpBlacklist.IpBlacklist.Instance.IsBlack(userid));
        }
        #endregion

    }
}
