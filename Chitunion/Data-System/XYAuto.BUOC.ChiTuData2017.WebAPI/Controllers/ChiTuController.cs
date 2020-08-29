using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu;
using XYAuto.BUOC.ChiTuData2017.BLL.Chitu.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class ChiTuController : ApiController
    {
        #region 数据统计--渠道
        [HttpGet]
        [ApiLog]
        public JsonResult ChannelSummary()
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                {
                    int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                    int channelid = BLL.TaskInfo.AppInfo.Instance.GeChannelIDByUserId(userID);
                    var resultQuery = BLL.Chitu.DataStatistics.Instance.GetDataByUserID(channelid);
                    jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限",-1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 数据统计接口 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 渠道每日汇总数据
        [HttpGet]
        [ApiLog]
        public JsonResult ChannelSummaryByDay([FromUri]RequestDataStatisticsByDate req)
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                {
                    var resultQuery = BLL.Chitu.DataStatisticsByDate.Instance.GetData(req);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限", -1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 渠道每日汇总数据 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 渠道订单
        [HttpGet]
        [ApiLog]
        public JsonResult Order([FromUri]RequestOrder req)
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                {
                var resultQuery = BLL.Chitu.Order.Instance.GetOrderList(req);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限", -1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 渠道订单 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 渠道订单导出
        [HttpGet]
        [ApiLog]
        public JsonResult OrderExcel([FromUri]RequestOrder req)
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                { 
                    var resultQuery = BLL.Chitu.Order.Instance.Export(req);
                    if (resultQuery == "暂无数据")
                    {
                        jsonResult = Util.GetJsonDataByResult("", "暂无数据",-1);
                    }
                    else
                    {
                        jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                    }
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限", -1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 渠道订单导出 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 订单详情
        [HttpGet]
        [ApiLog]
        public JsonResult OrderDetial([FromUri]int OrderId)
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                {
                    var resultQuery = BLL.Chitu.Order.Instance.GetOrderDetial(OrderId);
                    jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限", -1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 渠道详情 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 结算信息
        [HttpGet]
        [ApiLog]
        public JsonResult ChannelSummaryByMonth([FromUri]RequestSummaryByMonth req)
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                {
                    var resultQuery = BLL.Chitu.DataStatisticsByMonth.Instance.GetMonthStatistics(req);
                    jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限", -1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 结算信息 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 结算信息导出
        [HttpGet]
        [ApiLog]
        public JsonResult ChannelSummaryMonthExcel([FromUri]RequestSummaryByMonth req)
        {
            JsonResult jsonResult = null;
            try
            {
                if (RolesVerification.Instance.IsViewData() || RolesVerification.Instance.IsAllData())
                {
                    var resultQuery = BLL.Chitu.DataStatisticsByMonth.Instance.Export(req);
                    if (resultQuery == "暂无数据")
                    {
                        jsonResult = Util.GetJsonDataByResult("", "暂无数据",-1);
                    }
                    else
                    {
                        jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
                    }
                }
                else
                {
                    jsonResult = Util.GetJsonDataByResult(null, "无权限", -1);
                }
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 结算信息导出 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 获取渠道
        public JsonResult GetChannel()
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = BLL.Chitu.ChannelList.Instance.GetList();
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 获取渠道 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion
    }
}
