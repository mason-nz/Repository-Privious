using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class DataViewController : ApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:数据概括-历史数据统计
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult History()
        {
            var jsonResult = new JsonResult { Result = new DataViewProvider(-1, string.Empty).GetDvYesterday() };
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:数据概括-图表接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult Charts([FromUri]ReqStatTabGrabDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }
            jsonResult.Result = new DataViewProvider(request.LatelyDays, request.Category).GetGrabData();
            return jsonResult;
        }
    }
}