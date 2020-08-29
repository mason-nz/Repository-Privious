using System;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    /// <summary>
    /// 物料封装—控制器
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class EncapsulateController : ApiController
    {
        #region 封装图形接口
        [HttpGet]
        [ApiLog]
        public JsonResult RenderEncapsulate([FromUri] BasicQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = EncapsulateMaterialBll.Instance.BusinessMap(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试渲染封装页面图表时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 封装—汇总列表
        [HttpGet]
        [ApiLog]
        public JsonResult GetEncapsulateStatisticsList([FromUri] ListQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = EncapsulateMaterialBll.Instance.GetEncapsulateStatisticsList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—封装统计列表时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 封装—明细查询

        [HttpGet]
        [ApiLog]
        public JsonResult GetEncapsulateDetailList([FromUri] ListQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = EncapsulateMaterialBll.Instance.GetEncapsulateDetailList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—封装明细列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 导出汇总列表
        [HttpGet]
        [ApiLog]
        public JsonResult StatisticsExport([FromUri] ListQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = StatisticsExceListBll.Instance.StatisticsExport(query);

                jsonResult = Util.GetJsonDataByResult(resultQuery.Url, resultQuery.Message, resultQuery.Status);
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试导出{ query.ListType }列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 导出明细列表
        [HttpGet]
        [ApiLog]
        public JsonResult DetailExport([FromUri] ListQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = StatisticsExceListBll.Instance.DetailExport(query);

                jsonResult = Util.GetJsonDataByResult(resultQuery.Url, resultQuery.Message, resultQuery.Status);
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试导出{ query.ListType } 明细列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

    }
}
