using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.Materiel;
using XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_2_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_2_1;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using Util = XYAuto.ITSC.Chitunion2017.WebAPI.Common.Util;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class MaterielController : ApiController
    {
        [HttpPost]
        public JsonResult UpdateContractNumber([FromBody]UpdateContractNumberReqDTO req)
        {
            string msg = "更新失败";
            bool res = BLL.Materiel.MaterielExtend.Instance.UpdateContractNumber(req, ref msg);
            return Util.GetJsonDataByResult(res, res ? "更新成功" : msg, res ? 0 : 1);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:物料列表
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetList([FromUri] RequestMaterielQueryDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new MaterielQuery(new ConfigEntity()).GetQueryList(request),
                Message = "success"
            };
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:物料详情
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetInfo([FromUri]RequestMaterielGetInfoDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new MaterielProvider(new ConfigEntity(), request).GetInfo(),
                Message = "success"
            };
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:任务列表
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetTaskList([FromUri]RequestTaskSchedulerDto request)
        {
            var jsonResult = new JsonResult() { };

            var userId = UserInfo.GetLoginUserID();
            var config = new ConfigEntity()
            {
                CreateUserId = userId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userId)
            };

            jsonResult.Result = new TaskSchedulerQueryProxy(config, request).GetQuery();

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:获取物料清洗详情
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetCleanInfo([FromUri]int groupId, [FromUri]bool isSee)
        {
            var jsonResult = new JsonResult
            {
                Result =
                    new MaterielTaskSchedulerProvider(new ConfigEntity(), new RequestDistributeDto()).GetCleanInfo(groupId, isSee)
            };

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:任务分配、收回操作
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult TaskDivide([FromBody]RequestDistributeDto request)
        {
            var jsonResult = new JsonResult();
            var userId = UserInfo.GetLoginUserID();
            var provider = new MaterielTaskSchedulerProvider(new ConfigEntity()
            {
                CreateUserId = userId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userId)
            }, request);
            var retValue = provider.DistributeAndDoRecovery();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Status = retValue.ErrorCode.ToInt();
            jsonResult.Result = retValue.ReturnObject;
            jsonResult.Message = jsonResult.Message ?? "success";
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:作废操作
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult ToAbandoned([FromBody]ReqBaseGroup request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Status = 500;
                jsonResult.Message = $"请输入参数";
                return jsonResult;
            }

            var userId = UserInfo.GetLoginUserID();
            var provider = new MaterielTaskSchedulerProvider(new ConfigEntity()
            {
                CreateUserId = userId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userId)
            }, new RequestDistributeDto());

            var retValue = provider.DoAbandoned(request.GroupId);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Status = retValue.ErrorCode.ToInt();
            jsonResult.Result = retValue.ReturnObject;
            jsonResult.Message = jsonResult.Message ?? "success";

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:物料包清洗提交
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        public JsonResult SubmitInfo([FromBody]RequestSubmitCleanDto request)
        {
            var jsonResult = new JsonResult();
            var userId = UserInfo.GetLoginUserID();
            var provider = new MaterielTaskSchedulerProvider(new ConfigEntity()
            {
                CreateUserId = userId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userId)
            }, request);
            var retValue = provider.SubmitCleanInfo();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Status = retValue.ErrorCode.ToInt();
            jsonResult.Result = retValue.ReturnObject;
            jsonResult.Message = jsonResult.Message ?? "success";
            return jsonResult;
        }

        /// <summary>
        /// auth:lihf
        /// desc:查询操作人列表
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetDivideUser()
        {
            try
            {
                var resDto = BLL.Materiel.MaterielExtend.Instance.GetDivideUser();
                return Util.GetJsonDataByResult(resDto, resDto.List.Count == 0 ? "没有数据" : string.Empty, 0);
            }
            catch (Exception ex)
            {
                return Util.GetJsonDataByResult("GetDivideUser出错：", ex.Message, -1);
            }
        }

        /// <summary>
        /// auth:lihf
        /// desc:获取文章详情
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetArticleInfo(int ArticleId)
        {
            try
            {
                var resDto = BLL.Materiel.MaterielExtend.Instance.GetArticleInfo(ArticleId);
                return Util.GetJsonDataByResult(resDto, resDto == null ? "没有数据" : string.Empty, 0);
            }
            catch (Exception ex)
            {
                return Util.GetJsonDataByResult("GetArticleInfo出错：", ex.Message, -1);
            }
        }

        /// <summary>
        /// auth:lihf
        /// desc:获取文章列表
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpGet]
        public JsonResult GetArticleList([FromUri] RequestArticleQueryDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new ArticleQuery(new ConfigEntity()).GetQueryList(request),
                Message = "success"
            };
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:生成二维码，且下载zip
        /// </summary>
        /// <returns></returns>
        [ApiLog]
        [HttpPost]
        //[HttpGet]
        public HttpResponseMessage DownloadZip([FromBody]RequestMaterielGetInfoDto request)
        {
            var response = new HttpResponseMessage();

            if (request == null || request.MaterielId <= 0 || string.IsNullOrWhiteSpace(request.ChannelIds))
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("请输入参数,MaterielId,ChannelIds");
                return response;
            }
            try
            {
                var provider = new MaterielProvider(new ConfigEntity(), request);

                var retValue = provider.GenerateDownload();

                if (string.IsNullOrWhiteSpace(retValue.Item1))
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StringContent(JsonConvert.SerializeObject(retValue));
                    return response;
                }
                ContentDispositionHeaderValue disposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName =
                        System.Web.HttpUtility.UrlEncode(retValue.Item3, System.Text.Encoding.UTF8),
                    Name = System.Web.HttpUtility.UrlEncode(retValue.Item3.Replace(".zip", ""), System.Text.Encoding.UTF8),
                };

                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(new FileStream(retValue.Item1, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = disposition;
                //new ContentDispositionHeaderValue("attachment") { FileName = retValue.Item3 };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
            }
            return response;
        }
    }
}