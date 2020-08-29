using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Common;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.WechatSign;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{

    [CrossSite]
    public class SignController : ApiController
    {

        /// <summary>
        /// 微信签到
        /// </summary>
        /// <param name="DTO">微信用户信息</param>>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [ApiLog]
        [HttpPost]
        public JsonResult DaySign()
        {

            Loger.Log4Net.Info("[SignController]******DaySign begin->");
            try
            {
                var tupleDaySign = XYAuto.ITSC.Chitunion2017.BLL.WechatSign.WechatSign.Instance.DaySign();
                var userId = ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                var list = ITSC.Chitunion2017.BLL.LETask.V2_3.LE_Task.Instance.GetDataByPageV2_5(new ITSC.Chitunion2017.Entities.DTO.V2_3.TaskResDTO()
                {
                    UserID = userId,
                    SceneID = -2,
                    PageIndex = -2,
                    PageSize = 15
                });
                var dict = new Dictionary<string, object>
                    {
                        {"Amount", tupleDaySign.Item1},
                        {"isLuckDraw", tupleDaySign.Item3},
                        {"AlreadyOrderCount", tupleDaySign.Item4},
                        {"SignOrderCount", tupleDaySign.Item5},
                        {"List", list}
                    };
                return Common.Util.GetJsonDataByResult(dict, tupleDaySign.Item2 == "" ? "Success" : tupleDaySign.Item2, tupleDaySign.Item2 == "" ? 0 : -1);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[SignController]*****DaySign ->出错:", ex);
                return Common.Util.GetJsonDataByResult(null, $"出错:{ex.Message}", -1);
            }
        }
        ///// <summary>
        ///// 获取客户端IP地址
        ///// </summary>
        ///// <returns></returns>
        //public static string GetIP()
        //{
        //    //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
        //    string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    if (!string.IsNullOrEmpty(result))
        //    {
        //        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"获取当前客户端ID：HTTP_X_FORWARDED_FOR={result}");
        //        result = result.ToString().Split(',')[0].Trim();
        //    }
        //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"获取当前客户端ID：REMOTE_ADDR={HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]}");
        //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"获取当前客户端ID：UserHostAddress={HttpContext.Current.Request.UserHostAddress}");
        //    if (string.IsNullOrEmpty(result))
        //    {
        //        //否则直接读取REMOTE_ADDR获取客户端IP地址
        //        result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
        //    if (string.IsNullOrEmpty(result))
        //    {
        //        result = HttpContext.Current.Request.UserHostAddress;
        //    }
        //    //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
        //    if (!string.IsNullOrEmpty(result) &&
        //        System.Text.RegularExpressions.Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
        //    {
        //        //BLL.Loger.Log4Net.Info($"获取当前客户端ID：UserHostAddress={result}");
        //        return result;
        //    }
        //    return "0.0.0.0";
        //}
        /// <summary>
        /// 根据年月查询签到日期
        /// </summary>
        /// <param name="SignUserID">签到用户ID</param>
        /// <param name="SignYear">签到年份</param>
        /// <param name="SignMonth">签到月份</param>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [HttpGet]
        public Common.JsonResult DaySignList(int SignYear, int SignMonth)
        {
            WxSignRespDTO dto;
            try
            {
                dto = XYAuto.ITSC.Chitunion2017.BLL.WechatSign.WechatSign.Instance.DaySignList(SignYear, SignMonth);
            }
            catch (Exception ex)
            {

                Loger.Log4Net.Info("[SignController]*****DaySignList ->SignYear:" + SignYear + " ->SignMonth:" + SignMonth + ",查询微信签到信息失败:" + ex.Message);
                throw ex;
            }
            return Common.Util.GetJsonDataByResult(dto, "Success");
        }

        #region V2.5
        /// <summary>
        /// •判断活动是否在有效期内接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public Common.JsonResult IsValidActivity([FromUri]ITSC.Chitunion2017.BLL.Controller.Dto.IsValidActivity.ReqDto req)
        {
            try
            {
                string errorMsg = string.Empty;
                object ret = ITSC.Chitunion2017.BLL.Controller.Sign.Instance.IsValidActivity(req, ref errorMsg);
                return errorMsg == string.Empty ? Common.Util.GetJsonDataByResult(ret) : Common.Util.GetJsonDataByResult("操作失败", errorMsg, -2);
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info($"IsValidActivity出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return Common.Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }
        #endregion
    }
}
