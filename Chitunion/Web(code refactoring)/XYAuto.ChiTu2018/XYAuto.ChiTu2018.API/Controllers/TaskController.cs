using System;
using System.IO;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.Filter;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Service.Task;
using XYAuto.ChiTu2018.Service.Task.Dto.GetDataByPage;
using XYAuto.CTUtils.Log;
using XYAuto.ITSC.Chitunion2017.Common;
using Util = XYAuto.ChiTu2018.API.Common.Util;

namespace XYAuto.ChiTu2018.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [CrossSite]
    public class TaskController : ApiController
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
                Log4NetHelper.Default().Info($"GetTaskListByUserId->获取用户ID耗时：{ts1.Subtract(ts2).Duration().TotalMilliseconds.ToString()}毫秒");

                var list = LeTaskInfoService.Instance.GetDataByPage(reqDto);
                TimeSpan ts3 = new TimeSpan(DateTime.Now.Ticks);
                Log4NetHelper.Default().Info($"GetTaskListByUserId->获取数据耗时：{ts2.Subtract(ts3).Duration().TotalMilliseconds.ToString()}毫秒");
                jsonResult = Util.GetJsonDataByResult(list, "Success", 0);
                TimeSpan ts4 = new TimeSpan(DateTime.Now.Ticks);
                Log4NetHelper.Default().Info($"GetTaskListByUserId->格式化数据耗时：{ts3.Subtract(ts4).Duration().TotalMilliseconds.ToString()}毫秒");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetTaskListByUserId]报错", ex);
                jsonResult = Util.GetJsonDataByResult(null, $"出错：{ex.StackTrace}", -1);
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts5 = new TimeSpan(dtEnd.Ticks);
            Log4NetHelper.Default().Info($"GetTaskListByUserId->结束时间：{dtEnd} 总耗时：{ts1.Subtract(ts5).Duration().TotalMilliseconds.ToString()}毫秒");
            return jsonResult;
        }

        [ApiLog]
        [HttpGet]
        public JsonResult IsSelectedSceneByUser()
        {
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
                    userId = 1299;
                }
                var result = LeTaskInfoService.Instance.IsSelectedSceneByUser(userId);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[IsSelectedSceneByUser]报错", ex);
                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }

        /// <summary>
        /// 更新用户场景
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult UpdateUserScene()
        {
            JsonResult jsonResult = null;
            try
            {
                var sr = new StreamReader(HttpContext.Current.Request.InputStream);
                var stream = sr.ReadToEnd();                
                Service.Task.Dto.UpdateUserScene.ReqDto reqDto = JsonConvert.DeserializeObject<Service.Task.Dto.UpdateUserScene.ReqDto>(stream);
                reqDto.UserID = UserInfo.GetLoginUserID();
                Log4NetHelper.Default().Info($"[UpdateUserScene]ReqDto：{JsonConvert.SerializeObject(reqDto)}");
                bool result = LeTaskInfoService.Instance.UpdateUserScene(reqDto);
                jsonResult = Util.GetJsonDataByResult(result, "Success", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[UpdateUserScene]报错", ex);
                jsonResult = Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
            return jsonResult;
        }

        /// <summary>
        /// 根据openid 获取UnionId 和 UserId
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetUnionAndUserId()
        {
            try
            {
                var currentUserId = -2;
                try
                {
                    currentUserId = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    currentUserId = 1299;
                }
                var user = Service.Task.LeTaskInfoService.Instance.GetUnionAndUserId(currentUserId);
                return user == null ? Util.GetJsonDataByResult("操作失败", $"UserID:{currentUserId},未找到用户", -2) : Util.GetJsonDataByResult(user, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetUnionAndUserId]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
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
                var ret = Service.Task.LeTaskInfoService.Instance.GetShareOrderInfo(OrderId);
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

        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderByStatus(int status, int pageindex = 1, int pagesize = 10)
        {
            try
            {
                var ret =
                    Service.Task.LeTaskInfoService.Instance.GetOrderByStatus(new Service.Task.Dto.GetOrderByStatus.
                        ReqDto()
                    {
                        Status = status,
                        PageIndex = pageindex,
                        PageSize = pagesize
                    });
                return ret == null
                    ? Util.GetJsonDataByResult(null, "操作失败", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetOrderByStatus]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
        }

        /// <summary>
        /// 获取临时订单URL
        /// </summary>
        /// <param name="MaterialID"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderUrl(int MaterialID)
        {
            try
            {
                string errorMsg = string.Empty;
                var ret =
                    Service.Task.LeTaskInfoService.Instance.GetOrderUrl(MaterialID, out errorMsg);
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
        public JsonResult SubmitOrderUrl([FromBody]XYAuto.ChiTu2018.Service.Task.Dto.SubmitOrderUrl.ReqDto reqDto)
        {
            try
            {
                string errorMsg = string.Empty;
                var ret =
                    Service.Task.LeTaskInfoService.Instance.SubmitOrderUrl(reqDto, out errorMsg);
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

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetOrderInfo(int orderid)
        {
            try
            {
                string errorMsg = string.Empty;
                var ret =
                    Service.Task.LeTaskInfoService.Instance.GetOrderInfo(orderid, out errorMsg);
                return !string.IsNullOrEmpty(errorMsg)
                    ? Util.GetJsonDataByResult(null, $"操作失败:{errorMsg}", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetOrderInfo]报错", ex);
                return Util.GetJsonDataByResult("接口出错", ex.Message, -1);
            }
        }

        /// <summary>
        /// 根据用户ID获取场景
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetSceneInfoByUserId()
        {
            try
            {
                string errorMsg = string.Empty;
                var ret =
                    Service.Task.LeTaskInfoService.Instance.GetSceneInfoByUserId(out errorMsg);
                return !string.IsNullOrEmpty(errorMsg)
                    ? Util.GetJsonDataByResult(null, $"操作失败:{errorMsg}", -1)
                    : Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetSceneInfoByUserId]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
        }

        /// <summary>
        ///获取用户前一天订单数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetUserDayOrderCount()
        {
            try
            {
                var ret =
                    Service.Task.LeTaskInfoService.Instance.GetUserDayOrderCount();
                return Util.GetJsonDataByResult(ret, "操作成功", 0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[GetUserDayOrderCount]报错", ex);
                return Util.GetJsonDataByResult("操作失败", ex.Message, -1);
            }
        }
    }
}
