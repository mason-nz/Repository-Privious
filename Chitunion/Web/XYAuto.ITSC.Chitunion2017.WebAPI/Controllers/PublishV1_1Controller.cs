using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class PublishV1_1Controller : ApiController
    {
        /// <summary>
        /// Auth:lixiong
        /// 刊例查询接口-后台页面
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult BackGQuery([FromUri]RequestPublishQueryDto requestDto)
        {
            var jsonResult = Util.Verify<RequestPublishQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();//后台查询-需注意角色,如果有用户Id ，则带入参数
            if (requestDto.CreateUserId == 0)
            {
                jsonResult.Message = "无此权限访问";
                jsonResult.Status = 400;
                return jsonResult;
            }

            var config = new ConfigEntity()
            {
                RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId)
            };

            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new PbQueryProxy(config, requestDto).GetQuery();
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 广告查询接口-后台页面
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AdQuery([FromUri]RequestAdQueryDto requestDto)
        {
            var jsonResult = Util.Verify<RequestAdQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID(); ;//后台查询-需注意角色,如果有用户Id ，则带入参数
            if (requestDto.CreateUserId == 0)
            {
                jsonResult.Message = "无此权限访问";
                jsonResult.Status = 400;
                return jsonResult;
            }

            var config = new ConfigEntity()
            {
                RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId)
            };

            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new AdQueryProxy(config, requestDto).GetQuery();
            return jsonResult;
        }

        /// <summary>
        /// --1.媒体主：只显示该媒体主下所有审核通过的刊例下含键关词创建时间最新的10个
        /// --2.AE：只显示该AE下审核通过的刊例下含键关词创建时间最新的10个
        /// --3.运营：只显示当前处于待审刊例下的刊例中含键关词创建时间最新的10个
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SearchAutoComplete([FromUri]RequestSearchTitleDto requestDto)
        {
            var jsonResult = Util.Verify<RequestSearchTitleDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();

            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new WeiXinOperate(new RequestGetMeidaInfoDto(), new ConfigEntity())
                .GetSearchTitle(new PublishSearchAutoQuery<SearchTitleResponse>()
                {
                    PageSize = requestDto.PageSize,
                    KeyWord = requestDto.Keyword,
                    CreateUserId = requestDto.CreateUserId
                }, RoleInfoMapping.GetUserRole(requestDto.CreateUserId));
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        ///获取刊例待审核、驳回的数据统计总数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetPublishStatisticsCount()
        {
            var jsonResult = new JsonResult();
            jsonResult.Message = "success";
            jsonResult.Status = 0;

            var userId = Chitunion2017.Common.UserInfo.GetLoginUserID();
            jsonResult.Result = new AdQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userId)
            }, null).GetStatisticsCount(new PublishSearchAutoQuery<PublishStatisticsCount>()
            {
                BusinessType = (int)MediaType.WeiXin,
                CreateUserId = userId
            });
            return jsonResult;
        }

        /// <summary>
        /// 审核、启用停用刊例 ls
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AuditPublish([FromBody]AuditPublishReqDTO req)
        {
            BLL.Loger.Log4Net.Info("开始开始:" + JsonConvert.SerializeObject(req));
            int nextPubID = 0;
            string msg = string.Empty;
            req.IsBatch = false;
            if (!req.CheckSelfModel(out msg))
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
            var basic = BLL.PublishInfo.Instance.GetDetail(req.PubID);
            if (basic != null && req.OpType.Equals((int)PublishBasicStatusEnum.上架))
            {
                if (DateTime.Now.Date > basic.EndTime.Date)
                    return Util.GetJsonDataByResult(null, "刊例已过期，不能上架!", -1);
                if (BLL.PublishInfo.Instance.CheckIsConflict(14001, basic.BeginTime, basic.EndTime, basic.MediaID, basic.TemplateID, req.PubID))
                    return Util.GetJsonDataByResult(null, "刊例有效期有冲突，请查看是否处于待审、驳回或已通过列表中!", -1);
            }
            int res = BLL.PublishInfo.Instance.AuditPublish(req, out nextPubID);
            XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了审核启用停用刊例（PubID：" + req.PubID + " OpType：" + req.OpType + "）接口");
            return Util.GetJsonDataByResult(new { NextPubID = nextPubID }, res > 0 ? "成功" : "失败", res > 0 ? 0 : 1);
        }

        /// <summary>
        /// 新增、修改刊例
        /// ls
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ModifyPublish([FromBody]ModifyPublishReqDTO request)
        {
            try
            {
                string msg = string.Empty;
                if (!request.CheckSelfModel(out msg))
                {
                    return Util.GetJsonDataByResult(null, msg, -1);
                }
                if (request.Publish.MediaType.Equals((int)MediaTypeEnum.微信) && BLL.PublishInfo.Instance.CheckIsConflict(request.Publish.MediaType, request.Publish.BeginTime, request.Publish.EndTime, request.Publish.MediaID, request.Publish.TemplateID, request.Publish.PubID))
                {
                    return Util.GetJsonDataByResult(null, "有效期有冲突，请查看是否处于待审、驳回或已通过列表中!", -1);
                }

                #region 按媒体类型验证

                //switch (request.Publish.MediaType)
                //{
                //case (int)MediaTypeEnum.微信:
                // if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一微信) : !FuncValidate(FuncModuleEnum.刊例编辑一微信))
                //return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                // break;
                //case MediaTypeEnum.微博:
                //    if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一微博) : !FuncValidate(FuncModuleEnum.刊例编辑一微博))
                //        return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                //    break;
                //case MediaTypeEnum.视频:
                //    if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一视频) : !FuncValidate(FuncModuleEnum.刊例编辑一视频))
                //        return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                //    break;
                //case MediaTypeEnum.直播:
                //    if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一直播) : !FuncValidate(FuncModuleEnum.刊例编辑一直播))
                //        return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                //    break;
                //case MediaTypeEnum.APP:
                //    if (request.Publish.PubID.Equals(0) ? !FuncValidate(FuncModuleEnum.刊例添加一APP) : !FuncValidate(FuncModuleEnum.刊例编辑一APP))
                //        return Util.GetJsonDataByResult(false, "功能权限验证失败", 200);
                //    break;
                //}

                #endregion 按媒体类型验证

                bool res = false;
                if (request.Publish.PubID.Equals(0))//新增
                {
                    int pubID = 0;
                    res = BLL.PublishInfo.Instance.AddPublishBasicInfoV1_1(request, ref msg, ref pubID);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Add, "调用了新增刊例（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.Publish.MediaType) + "）接口");
                    if (!res)
                        BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublish ->request:" + JsonConvert.SerializeObject(request));
                    return Util.GetJsonDataByResult(res ? new MediaExistsDTO() { PubID = pubID } : null, res ? "成功" : msg, res ? 0 : 1);
                }
                else//编辑
                {
                    res = BLL.PublishInfo.Instance.UpdatePublishBasicInfo_V1_1(request, ref msg);
                    XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Modify, "调用了编辑刊例（媒体类型：" + Enum.GetName(typeof(MediaTypeEnum), request.Publish.MediaType) + " ID：" + request.Publish.PubID + "）接口");
                    if (!res)
                    {
                        if (string.IsNullOrEmpty(msg))
                            msg = "操作失败";
                        BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublish ->request:" + JsonConvert.SerializeObject(request));
                    }
                    return Util.GetJsonDataByResult(res, res ? "操作成功" : msg, res ? 0 : 1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[PublishController]*****ModifyPublish ->request:" + JsonConvert.SerializeObject(request) + ",调用新增编辑刊例接口出错:" + ex.Message);
                return Util.GetJsonDataByResult(null, "系统异常:" + ex.Message, 500);
            }
        }

        /// <summary>
        /// 删除刊例 ls
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult DeletePublish([FromBody]AuditPublishReqDTO req)
        {
            bool res = BLL.PublishInfo.Instance.DeletePublishBasicInfo(req.PubID) > 0;
            XYAuto.ITSC.Chitunion2017.Common.LogInfo.Instance.InsertLog(XYAuto.ITSC.Chitunion2017.Common.LogInfo.LogModuleType.刊例管理, Chitunion2017.Common.LogInfo.ActionType.Delete, "调用了删除刊例（PubID：" + req.PubID + "）接口");

            return Util.GetJsonDataByResult(null, res ? "success" : "fail", res ? 0 : 1);
        }

        #region V1.1.4 Ls

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetADDetail(int MediaType, int PubID = 0, int MediaID = 0, int TemplateID = 0)
        {
            var res = BLL.MediaInfo.Instance.GetAppADItem(MediaType, PubID, MediaID, TemplateID);
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetADListB([FromUri] GetADListBReqDTO req)
        {
            var res = BLL.MediaInfo.Instance.GetAppADListB(req);
            return Util.GetJsonDataByResult(res, res != null ? "成功" : "失败", res == null ? 1 : 0);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetAuditADPriceList(int MediaType, int PubID)
        {
            var res = BLL.MediaInfo.Instance.GetAuditAppADList(PubID);
            return Util.GetJsonDataByResult(res,res == null ? "失败" : "成功",res == null ? 1 : 0);
        }

        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult BatchAuditPublish(AuditPublishReqDTO req)
        {
            string msg = string.Empty;
            req.IsBatch = true;
            if (!req.CheckSelfModel(out msg))
            {
                return Util.GetJsonDataByResult(null, msg, -1);
            }
            int nextPubID = 0;
            bool flag = BLL.PublishInfo.Instance.BatchAuditPublish(req, ref msg, ref nextPubID);
            return Util.GetJsonDataByResult(new { NextPubID = nextPubID}, msg, flag ? 0 : 1);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public JsonResult GetPublishListB([FromUri]GetADListBReqDTO parameters)
        {
            var res = BLL.PublishInfo.Instance.GetAppPublishList(parameters);
            return Util.GetJsonDataByResult(res, res != null ? "成功" : "失败", res == null ? 1 : 0);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetRecommendAD(int MediaType, int MediaID, int TemplateID)
        {
            var res = BLL.MediaInfo.Instance.GetSimilarAD(MediaID, TemplateID);
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetADListF([FromUri]GetADListFReqDTO parameters)
        {
            var res = BLL.MediaInfo.Instance.GetAppADListF(parameters);
            return Util.GetJsonDataByResult(res, res != null ? "成功" : "失败", res == null ? 1 : 0);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetPubDateList(int mediaID, int templateID) {
            var res = BLL.PublishInfo.Instance.GetPubDateList(mediaID, templateID);
            return Util.GetJsonDataByResult(res, "", 0);
        }
        #endregion V1.1.4 Ls

        /// <summary>
        /// 模块功能权限验证
        /// </summary>
        /// <returns></returns>
        private bool FuncValidate(FuncModuleEnum module)
        {
#if DEBUG
            return true;
#endif
            string moduleID = Chitunion2017.Common.UserInfo.SYSID + "BUT" + (int)module;
            return Chitunion2017.Common.UserInfo.CheckRight(moduleID, Chitunion2017.Common.UserInfo.SYSID);
        }
    }
}