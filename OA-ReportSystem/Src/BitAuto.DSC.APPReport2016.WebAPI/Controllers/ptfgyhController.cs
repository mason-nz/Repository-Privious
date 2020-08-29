using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{    
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1002")]
    public class ptfgyhController : ApiController
    {
        //
        // GET: /ptfgyh/ 
        /// <summary>
        /// 获取平台覆盖数据饼图
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetData()
        {

            DateTime dtime = BLL.Traffic.Instance.GetLatestDate();

            var RetDate = new { RetDateVal = dtime.ToString("yyyy-MM-dd"), WeekDay = BLL.Util.GetWeekNameByDate(dtime) };
            List<object> list = BLL.Traffic.Instance.GetDataByDate(dtime);

            var dataList = new { RetDate = RetDate, dataList = list };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new RetJsonModel() { Success = true, ErrorMsg = "", RetData = dataList });
        }


        /// <summary>
        ///  获取平台覆盖数据片图
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataDetails()
        {

            DateTime dtime = BLL.Traffic.Instance.GetLatestDate();

            List<object> list = BLL.Traffic.Instance.GetDataDetails(dtime);

            var TotalTraffic = BLL.Traffic.Instance.GetWholeSiteByDate(dtime);

            var dataList = new { TotalTraffic = TotalTraffic, dataList = list };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new RetJsonModel() { Success = true, ErrorMsg = "", RetData = dataList });

        }


        /// <summary>
        /// 获取单个平台线图
        /// </summary>
        /// <param name="date"></param>
        /// <param name="TypeID"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataTrendByTypeID(DateTime date, int TypeID)
        {
            var dataList = BLL.Traffic.Instance.GetDataTrendBySiteIdAndDate(TypeID, date);

            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new RetJsonModel() { Success = true, ErrorMsg = "", RetData = dataList });
        }
    }
}
