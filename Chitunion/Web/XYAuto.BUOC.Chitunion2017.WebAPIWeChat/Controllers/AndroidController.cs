using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.App;
using XYAuto.ITSC.Chitunion2017.BLL.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [CrossSite]

    public class AndroidController : ApiController
    {
        public XYAuto.ITSC.Chitunion2017.Common.LoginUser GetUserInfo =>
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();

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
        /// 安卓端发送验证码
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public Common.JsonResult SendCodeForAndroid(ReqLoginDTO Dto)
        {
            try
            {
                string errorMsg = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.SendValidateCode(Dto);

                return errorMsg == "0" ? Common.Util.GetJsonDataByResult(null) : Common.Util.GetJsonDataByResult(errorMsg, "发送失败", -1);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"SendCodeForAndroid:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 安卓端登录
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public Common.JsonResult LoginForAndroid(ReqLoginDTO Dto)
        {
            try
            {
                Dto.Ip = System.Web.HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
                string errorMsg = string.Empty;
                Dictionary<string, object> dic = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.LoginForAndroid(Dto, out errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(dic) : Common.Util.GetJsonDataByResult(dic, errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error($"LoginForAndroid登录出错：", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        /// <summary>
        /// 安卓端退出登录
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public Common.JsonResult ExitForAndroid()
        {
            try
            {
                //int userId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                //XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"安卓端页用户：{userId}退出登录开始");
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.Clear();
                //XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"安卓端页用户：{userId}退出登录结束");
            }
            catch (Exception ex)
            {
                XYAuto.ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("安卓端页退出赤兔系统时出错", ex);
            }
            return Common.Util.GetJsonDataByResult(null);
        }
        ///// <summary>
        ///// 查询app用户是否为新用户
        ///// </summary>
        ///// <returns></returns>
        //[ApiLog]
        //[HttpGet]

        //public Common.JsonResult IsNewUser()
        //{
        //    return Common.Util.GetJsonDataByResult(true);
        //}

        /// <summary>
        /// •APP端，第三方（微信）登陆后，根据获取到的微信相关信息，调用此接口生成用户Cookies唯一标识
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public Common.JsonResult LoginForWeChat(ReqLoginDTO Dto)
        {
            try
            {
                string errorMsg = string.Empty;
                Dto.Ip = System.Web.HttpContext.Current.Request.ServerVariables.Get("Remote_Addr");
                var resp = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.LoginForWeChat(Dto, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(resp) : Common.Util.GetJsonDataByResult(null, errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("APP微信登录出错", ex);
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 微信绑定
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [CTAppAuth]
        public Common.JsonResult WxBind(ReqLoginDTO dto)
        {
            var jsonResult = new Common.JsonResult();
            var userId = GetUserInfo == null ? 0 : GetUserInfo.UserID;
            var retValue = ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.WxBind(dto, userId);
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
        public Common.JsonResult SetMsgPushConfig([FromBody] ReqAppPushSwitchDto request)
        {
            var jsonResult = new Common.JsonResult();
            var retValue = new AppPushMsgSwitchLogProvider(request).SetPushConfig();
            return jsonResult.GetReturn(retValue);
        }
    }
}
