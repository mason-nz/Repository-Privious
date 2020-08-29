using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    /// <summary>
    /// 物料分发—控制器
    /// </summary>
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class DistributeController : ApiController
    {
        #region 分发图形接口
        [HttpGet]
        [ApiLog]
        public JsonResult RenderDistribute([FromUri] BasicQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = DistributeMaterialBll.Instance.BusinessMap(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试渲染分发页面图表时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 分发—汇总列表
        [HttpGet]
        [ApiLog]
        public JsonResult GetDistributeStatisticsList([FromUri] ListQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = DistributeMaterialBll.Instance.GetDistributeStatisticsList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—分发统计列表时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 分发—明细查询

        [HttpGet]
        [ApiLog]
        public JsonResult GetDistributeDetailList([FromUri] ListQueryArgs query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = DistributeMaterialBll.Instance.GetDistributeDetailList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—分发明细列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion
    }
}
