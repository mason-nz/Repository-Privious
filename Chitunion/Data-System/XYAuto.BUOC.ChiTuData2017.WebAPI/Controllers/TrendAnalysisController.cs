using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Export;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class TrendAnalysisController : ApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:趋势分析-抓取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetGrabList([FromUri]ReqStatTabGrabDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }

            jsonResult.Result = new StatTabGrabProvider(request.LatelyDays, request.Category).GetGrabData();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:趋势分析-机洗入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetJxList([FromUri]ReqStatTabGrabDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }

            jsonResult.Result = new StatTabMachineCleanProvider(request.LatelyDays, request.Category).GetGrabData();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:趋势分析-车型匹配
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetCxppList([FromUri]ReqStatTabGrabDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }

            jsonResult.Result = new StatTabCarMatchProvider(request.LatelyDays, request.Category).GetGrabData();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:趋势分析-车型匹配
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetCsList([FromUri]ReqStatTabGrabDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }

            jsonResult.Result = new StatTabPrimaryProvider(request.LatelyDays, request.Category).GetGrabData();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:趋势分析-人工清洗
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetArtificialList([FromUri]ReqStatTabGrabDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Message = "请输入请求参数";
                jsonResult.Status = -1;
                return jsonResult;
            }

            jsonResult.Result = new StatTabArtificialProvider(request.LatelyDays, request.Category).GetGrabData();
            return jsonResult;
        }

        /// <summary>
        /// 趋势分析-Tab页-日汇总数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetDailyList([FromUri]ReqDailyDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new StatDailyQueryProxy(null, request).GetQuery()
            };

            return jsonResult;
        }

        /// <summary>
        /// 趋势分析-Tab页-明细数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetDetailsList([FromUri]ReqDetailsDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new StatDetailsQueryProxy(null, request).GetQuery()
            };

            return jsonResult;
        }

        /// <summary>
        /// 导出数据-日汇总数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult ExportDaily([FromUri]ReqDailyDto request)
        {
            var response = new JsonResult();
            if (request == null)
            {
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = "请输入参数";
                return response;
            }

            try
            {
                var retValue = new StatDailyExportProvider(request).DoExport();
                response.Status = 0;
                response.Result = new { Url = retValue.ReturnObject };
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = exception.ToString();
            }
            return response;
        }

        /// <summary>
        /// 导出数据-日汇总数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public HttpResponseMessage ExportDailyByPost([FromBody]ReqDailyDto request)
        {
            var response = new HttpResponseMessage();

            if (request == null)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("请输入参数");
                return response;
            }

            try
            {
                var retValue = new StatDailyExportProvider(request).DoExport();
                if (retValue.HasError)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent(JsonConvert.SerializeObject(retValue));
                    return response;
                }
                ContentDispositionHeaderValue disposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName =
                      System.Web.HttpUtility.UrlEncode(retValue.ReturnObject.ToString(), System.Text.Encoding.UTF8),
                    Name = System.Web.HttpUtility.UrlEncode(retValue.ReturnObject.ToString().Replace(".xlsx", ""), System.Text.Encoding.UTF8),
                };
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(new FileStream(retValue.Message, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = disposition;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception exception)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(exception.ToString());
            }
            return response;
        }

        /// <summary>
        /// 导出数据-明细数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult ExportDetails([FromUri]ReqDetailsDto request)
        {
            var response = new JsonResult();
            if (request == null)
            {
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = "请输入参数";
                return response;
            }

            try
            {
                var retValue = new StatDetailsExportProvider(request).DoExport();
                response.Status = 0;
                response.Result = new { Url = retValue.ReturnObject };
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = exception.ToString();
            }
            return response;
        }
    }
}