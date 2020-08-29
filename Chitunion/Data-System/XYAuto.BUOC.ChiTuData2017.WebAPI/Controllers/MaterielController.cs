using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Export;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    [CrossSite]
    public class MaterielController : ApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:物料分发列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDistributeList([FromUri] RequestDistributeQueryDto request)
        {
            var jsonResult = new JsonResult();
            if ((!string.IsNullOrEmpty(request.StartDate) && string.IsNullOrEmpty(request.EndDate)) || (string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate)))
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请同时选择开始、结束时间";
            }
            else
            {
                jsonResult.Result = new DistributeQuery(new ConfigEntity()).GetQueryList(request);

                jsonResult.Message = "success";
            }
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:物料详情
        /// </summary>
        /// <param name="rqeuest"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetInfo([FromUri] RequestGetMaterielInfoDto rqeuest)
        {
            var jsonResult = new JsonResult() { Status = -1, Message = "请输入参数" };
            if (rqeuest == null)
                return jsonResult;
            if (rqeuest.DistributeType <= 0 || rqeuest.MaterielId <= 0)
            {
                jsonResult.Message = "请确认参数DistributeType，MaterielId";
                return jsonResult;
            }
            jsonResult.Result = new DistributeProvider().GetMaterielInfo(rqeuest.MaterielId, rqeuest.DistributeType);
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:分发日结数据统计
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDailyQuery([FromUri] RequestDistributeQueryDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new DistributeQueryProxy(null, request).GetQuery()
            };

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:物料详情数据统计(日结统计之下)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDetailsQuery([FromUri] RequestDistributeQueryDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new DetailedStatisticsQuery(null).GetQueryList(request)
            };

            return jsonResult;
        }

        [HttpGet]
        [ApiLog]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS004BUT1001")]
        public JsonResult Export([FromUri] DistributeExportDto request)
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
                var distributeExportProvider = new DistributeExportProvider(request).Export();
                var retValue = distributeExportProvider.Item1;
                if (retValue.HasError)
                {
                    response.Status = retValue.ErrorCode.ToInt(-1);
                    response.Message = retValue.Message;
                    return response;
                }
                response.Status = 0;
                response.Result = new { Url = distributeExportProvider.Item4 };
            }
            catch (Exception exception)
            {
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Message = exception.ToString();
            }
            return response;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:导出数据：分发列表导出，分发明细导出，物料详情明细导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS004BUT1001")]
        public HttpResponseMessage ExportByPost([FromBody]DistributeExportDto request)
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
                var distributeExportProvider = new DistributeExportProvider(request).Export();
                var retValue = distributeExportProvider.Item1;
                if (retValue.HasError)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent(JsonConvert.SerializeObject(retValue));
                    return response;
                }
                ContentDispositionHeaderValue disposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName =
                      System.Web.HttpUtility.UrlEncode(distributeExportProvider.Item3, System.Text.Encoding.UTF8),
                    Name = System.Web.HttpUtility.UrlEncode(distributeExportProvider.Item3.Replace(".xlsx", ""), System.Text.Encoding.UTF8),
                };
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(new FileStream(distributeExportProvider.Item2, FileMode.Open, FileAccess.Read));
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
    }
}