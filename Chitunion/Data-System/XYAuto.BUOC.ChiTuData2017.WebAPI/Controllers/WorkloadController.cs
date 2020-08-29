using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Workload;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{   
    /// <summary>
    /// 工作量统计—控制器
    /// </summary>
    [CrossSite]
    public class WorkloadController : ApiController
    {

        #region 工作量统计列表
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult WorkloadStatisticsList([FromUri] WorkloadQuery query)
        {
            JsonResult jsonResult = null;
            try
            {

                var resultQuery = WorkloadBll.Instance.GetWorkloadList(query);
                jsonResult = Util.GetJsonDataByResult(resultQuery, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试获取—工作量统计列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion

        #region 工作量统计导出
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult WorkloadStatisticsExport([FromUri] WorkloadQuery query)
        {
            JsonResult jsonResult = null;
            try
            {
                var resultQuery = WorkloadBll.Instance.WorkloadStatisticsExport(query);
                if (resultQuery.Status < 0)
                {
                    return Util.GetJsonDataByResult(null, "数据量过大，请分批导出", -1);
                }
                jsonResult = Util.GetJsonDataByResult(resultQuery.Url, "Success");
            }
            catch (Exception ex)
            {
                int userID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
                Loger.Log4Net.Error($"用户 {userID} 尝试导出—工作量统计列表出时 引发了一个 {ex.GetType()} 类型的异常：{ex.Message}");
                jsonResult = Util.GetJsonDataByResult(string.Empty, "操作失败", -1);
            }
            return jsonResult;
        }
        #endregion
    }
}
