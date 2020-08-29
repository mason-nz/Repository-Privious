using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Funnel;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class FunnelController : ApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:漏斗分析-图表分析接口（文章的，头部文章，腰部文章）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetArticleChart([FromUri]ReqFunnelDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }
            jsonResult.Result = new FunnelStatProvider(request.LatelyDays, request.TabType, request).GetFunnelData();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:漏斗分析-图表分析接口（物料的）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetMaterielChart([FromUri]ReqFunnelDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }
            jsonResult.Result = new FunnelStatProvider(request.LatelyDays, request.TabType, request).GetMaterielChart();
            return jsonResult;
        }

        #region 漏斗头部列表

        [HttpGet]
        [ApiLog]
        public JsonResult GetFunnelHeadDetailList([FromUri] BasicQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = FunnelMaterialBll.Instance.GetFunnelHeadList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—漏斗头部列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }

        #endregion 漏斗头部列表

        #region 漏斗腰部列表

        [HttpGet]
        [ApiLog]
        public JsonResult GetFunnelWaistDetailList([FromUri] BasicQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = FunnelMaterialBll.Instance.GetFunnelWaistDetailList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—漏斗腰部列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }

        #endregion 漏斗腰部列表

        #region 漏斗物料列表

        [HttpGet]
        [ApiLog]
        public JsonResult GetFunnelMaterialDetailList([FromUri] BasicQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = FunnelMaterialBll.Instance.GetFunnelMaterialList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—漏斗物料列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }

        #endregion 漏斗物料列表

        #region 漏斗头部列表—导出

        [HttpGet]
        [ApiLog]
        public JsonResult FunnelHeadExport([FromUri]BasicQueryArgs queryArgs)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = FunnelMaterialBll.Instance.FunnelHeadExport(queryArgs);
               
                jsonResult = Util.GetJsonDataByResult(resultQuery.Url, resultQuery.Message, resultQuery.Status);
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试导出—漏斗头部列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }

        #endregion 漏斗头部列表—导出

        #region 漏斗腰部列表—导出

        [HttpGet]
        [ApiLog]
        public JsonResult FunnelWaistExport([FromUri]BasicQueryArgs queryArgs)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = FunnelMaterialBll.Instance.FunnelWaistExport(queryArgs);
                jsonResult = Util.GetJsonDataByResult(resultQuery.Url, resultQuery.Message, resultQuery.Status);

            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试导出—漏斗腰部列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }

        #endregion 漏斗腰部列表—导出

        #region 漏斗物料导出—导出

        [HttpGet]
        [ApiLog]
        public JsonResult FunnelMaterialExport([FromUri]BasicQueryArgs queryArgs)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = FunnelMaterialBll.Instance.FunnelMaterialExport(queryArgs);
                jsonResult = Util.GetJsonDataByResult(resultQuery.Url, resultQuery.Message, resultQuery.Status);

            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试导出—漏斗物料列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }

        #endregion 
    }
}