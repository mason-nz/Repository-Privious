using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.App.Common;
using XYAuto.ChiTu2018.API.App.Filter;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.Task;
using XYAuto.ChiTu2018.Service.App.Task.Dto.GetDataByPage;
using XYAuto.CTUtils.Log;
using XYAuto.ITSC.Chitunion2017.Common;
using Util = XYAuto.ChiTu2018.API.App.Common.Util;

namespace XYAuto.ChiTu2018.API.App.Controllers
{
    /// <summary>
    /// 任务相关
    /// </summary>
    [CrossSite]
    public class TaskController : BaseApiController
    {
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="reqDto"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetTaskListByUserId([FromUri]ReqDto reqDto)
        {
            DateTime dtStart = DateTime.Now;
            Log4NetHelper.Default().Info($"GetTaskListByUserId->开始时间：{dtStart}");
            TimeSpan ts1 = new TimeSpan(dtStart.Ticks);
            JsonResult jsonResult = null;
            try
            {
                int userId = -2;
                try
                {
                    userId = UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    userId = 3430;
                }
                reqDto.UserID = userId;
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                Log4NetHelper.Default().Info($"GetTaskListByUserId->获取用户ID耗时：{ts1.Subtract(ts2).Duration().TotalMilliseconds}毫秒");

                var list = LeTaskInfoService.Instance.GetDataByPage(reqDto);
                TimeSpan ts3 = new TimeSpan(DateTime.Now.Ticks);
                Log4NetHelper.Default().Info($"GetTaskListByUserId->获取数据耗时：{ts2.Subtract(ts3).Duration().TotalMilliseconds}毫秒");
                jsonResult = Util.GetJsonDataByResult(list, "Success", 0);
                TimeSpan ts4 = new TimeSpan(DateTime.Now.Ticks);
                Log4NetHelper.Default().Info($"GetTaskListByUserId->格式化数据耗时：{ts3.Subtract(ts4).Duration().TotalMilliseconds}毫秒");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetTaskListByUserId]报错", ex);
                jsonResult = Util.GetJsonDataByResult(null, $"出错：{ex.StackTrace}", -1);
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts5 = new TimeSpan(dtEnd.Ticks);
            Log4NetHelper.Default().Info($"GetTaskListByUserId->结束时间：{dtEnd} 总耗时：{ts1.Subtract(ts5).Duration().TotalMilliseconds}毫秒");
            return jsonResult;
        }

        /// <summary>
        /// 根据ID获取分享信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetShareOrderInfo([FromUri]int OrderId)
        {
            try
            {
                var ret = Service.App.Task.LeTaskInfoService.Instance.GetShareOrderInfo(OrderId);
                return ret == null
                    ? Util.GetJsonDataByResult(null, "操作失败没有订单", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetShareOrderInfo]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
        }

        /// <summary>
        /// 获取临时订单URL
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderUrl(int materialId)
        {
            try
            {
                string errorMsg = string.Empty;
                var ret =
                    Service.App.Task.LeTaskInfoService.Instance.GetOrderUrl(materialId, out errorMsg);
                return !string.IsNullOrEmpty(errorMsg)
                    ? Util.GetJsonDataByResult(null, $"操作失败:{errorMsg}", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetOrderUrl]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
        }

        /// <summary>
        /// 提交订单URL
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        [ApiLog]
        public JsonResult SubmitOrderUrl([FromBody]XYAuto.ChiTu2018.Service.App.Task.Dto.SubmitOrderUrl.ReqDto reqDto)
        {
            try
            {
                string errorMsg = string.Empty;
                var ret =
                    Service.App.Task.LeTaskInfoService.Instance.SubmitOrderUrl(reqDto, out errorMsg);
                return !string.IsNullOrEmpty(errorMsg)
                    ? Util.GetJsonDataByResult(null, $"操作失败:{errorMsg}", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[SubmitOrderUrl]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
        }

    }
}
