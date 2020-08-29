using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Controllers
{
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    [CrossSite]
    public class ProfitController : ApiController
    {
        /// <summary>
        /// 获取大于0的收益信息列表
        /// </summary>
         /// <param name="TopCount">查询数量</param>
        /// <param name="RowNum">排序ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetProfitList(int TopCount = 20, int RowNum = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = XYAuto.ITSC.Chitunion2017.BLL.Profit.Profit.Instance.GetProfitList(TopCount, RowNum, false);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ProfitController]*****GetProfitList ->RowNum:" + RowNum + ",查询收益列表失败:" + ex.Message);
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        #region V2.1.0
        /// <summary>
        /// 获取所有收益信息列表
        /// </summary>
        /// <param name="TopCount">查询数量</param>
        /// <param name="RowNum">排序ID</param>
        /// <returns></returns>
        [HttpGet]
        public Common.JsonResult GetAllProfitList(int TopCount = 20, int RowNum = 0)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = XYAuto.ITSC.Chitunion2017.BLL.Profit.Profit.Instance.GetProfitList(TopCount, RowNum, true);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[ProfitController]*****GetAllProfitList ->RowNum:" + RowNum + ",查询全部收益列表失败:" + ex.Message);
            }
            return Common.Util.GetJsonDataByResult(dic, "Success");
        }
        #endregion
    }
}
