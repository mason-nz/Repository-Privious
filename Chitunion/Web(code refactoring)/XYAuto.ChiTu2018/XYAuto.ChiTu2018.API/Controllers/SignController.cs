using System;
using System.Web.Http;
using XYAuto.ChiTu2018.API.Common;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Service.Task;
using XYAuto.ChiTu2018.Service.Task.Dto.GetDataByPage;
using XYAuto.ChiTu2018.Service.Wechat;
using XYAuto.ChiTu2018.Service.Wechat.Dto;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.API.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class SignController : ApiController
    {
        /// <summary>
        /// 微信签到
        /// </summary>
        /// <param name="DTO">微信用户信息</param>>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult DaySign()
        {

            Log4NetHelper.Default().Info("[SignController]******DaySign begin->");
            RespLeDaySignDto tupleDaySign = null;
            try
            {
                int userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                tupleDaySign = WechatSignService.Instance.DaySign(CTUtils.Html.RequestHelper.GetIpAddress("127.0.0.1"), userId);

                tupleDaySign.List = LeTaskInfoService.Instance.GetDataByPage(new ReqDto { UserID = userId, PageIndex = -2, PageSize = 15, SceneID = -2 });
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("[SignController]*****DaySign ->签到失败:" + ex.Message);
                return Util.GetJsonDataByResult(null, $"出错:{ex.Message}", -1);
            }
            Log4NetHelper.Default().Info("[SignController]******DaySign end->");
            return Util.GetJsonDataByResult(tupleDaySign, tupleDaySign.Message == "" ? "Success" : tupleDaySign.Message, tupleDaySign.Message == "" ? 0 : -1);
        }

        /// <summary>
        /// 根据年月查询签到日期
        /// </summary>
        /// <param name="signYear"></param>
        /// <param name="signMonth"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult DaySignList(int signYear, int signMonth)
        {
            RespWeChatSignDto dto = null;
            try
            {
                dto = WechatSignService.Instance.DaySignList(signYear, signMonth, ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID());
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info("[SignController]*****DaySignList ->SignYear:" + signYear + " ->SignMonth:" + signMonth + ",查询微信签到信息失败:" + ex.Message);
            }
            return Util.GetJsonDataByResult(dto, "Success");
        }

        #region V2.5
        /// <summary>
        /// •判断活动是否在有效期内接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult IsValidActivity([FromUri]SignReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = WechatSignService.Instance.IsValidActivity(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Info($"IsValidActivity出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        #endregion
    }
}