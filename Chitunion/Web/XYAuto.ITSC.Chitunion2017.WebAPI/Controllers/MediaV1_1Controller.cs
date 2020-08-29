using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;
using MediaCase = XYAuto.ITSC.Chitunion2017.Entities.Media.MediaCase;
using Util = XYAuto.ITSC.Chitunion2017.WebAPI.Common.Util;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class MediaV1_1Controller : ApiController
    {
        /// <summary>
        /// Auth:lixiong
        /// 微信资料补充与编辑
        /// </summary>
        /// <param name="publicParam"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Curd([FromBody] RequestMediaDto publicParam)
        {
            var jsonResult = new JsonResult();
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type,
                SourceTypeEnum = (SourceEnum)userInfo.Source
            };
            var retValue = new MediaOperateProxy(publicParam, config).Excute();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Result = new OperateResult
            {
                MediaId = retValue.ReturnObject == null
                    ? 0
                    : retValue.ReturnObject.ToString().ToInt()
            };
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主添加资质信息接口
        /// </summary>
        /// <param name="publicParam"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AddQualification([FromBody] RequestMediaDto publicParam)
        {
            var jsonResult = new JsonResult();
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type
            };

            var retValue = new BLL.Media.Business.V1_1.AppOperate(config, publicParam.App).DoQualificationExcute();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Result = new OperateResult
            {
                MediaId = retValue.ReturnObject == null
                    ? 0
                    : retValue.ReturnObject.ToString().ToInt()
            };
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// desc:app媒体列表查询接口（媒体主，AE，运营角色都有）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult BackGQuery([FromUri]RequestMediaAppQueryDto requestDto)
        {
            var jsonResult = Util.Verify<RequestMediaAppQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID();//后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId)
            };

            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new MediaQueryProxy(config, requestDto).GetQuery();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主列表需要统计信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStatisticsCount()
        {
            var jsonResult = new JsonResult();
            jsonResult.Message = "success";
            jsonResult.Status = 0;

            var userId = Chitunion2017.Common.UserInfo.GetLoginUserID();
            jsonResult.Result = new AdQueryProxy(new ConfigEntity()
            {
                CreateUserId = userId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userId)
            }, null).GetStatisticsCount(new PublishSearchAutoQuery<PublishStatisticsCount>()
            {
                BusinessType = (int)MediaType.APP,
                CreateUserId = userId
            });
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// app媒体新增校验接口,媒体主、AE角色输入app名称校验是否可以添加媒体
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult VerifyOfAdd([FromBody] RequestMeidaVerifyOfAdd requestDto)
        {
            var jsonResult = Util.Verify<RequestMeidaVerifyOfAdd>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type
            };

            var retValue = new AppOperate(config, new RequestAppDto()).VerifyOfAdd(requestDto.MediaName);

            jsonResult.Message = retValue.Message;
            jsonResult.Result = retValue.ReturnObject;
            jsonResult.Status = retValue.ErrorCode.ToInt(-1);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// app媒体添加模板或者广告的校验，校验当前媒体下是否有已审核通过的模板或自己添加的未通过的模板
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult VerfiyOfAppTemplate([FromBody] RequestMeidaVerifyOfAdd requestDto)
        {
            var jsonResult = new JsonResult();
            var userId = UserInfo.GetLoginUserID();

            var retValue = new AppOperate(new ConfigEntity(), new RequestAppDto()).VerfiyOfAppTemplate(requestDto.MediaId, userId
                , requestDto.MediaName);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Message = retValue.Message;
            jsonResult.Result = retValue.ReturnObject;
            jsonResult.Status = retValue.ErrorCode.ToInt();
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 媒体信息查询（微信补充资料、媒体修改页面）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetInfo([FromUri]RequestGetMeidaInfoDto requestDto)
        {
            var jsonResult = new JsonResult() { Status = 500, Message = "请输入参数" };
            requestDto.CreateUserId = UserInfo.GetLoginUserID();
            jsonResult.Result = new MediaOperateProxy(requestDto, new ConfigEntity()
            {
                RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId),
                CreateUserId = requestDto.CreateUserId
            }).QueryInfo();
            jsonResult.Message = string.Empty;
            jsonResult.Status = 0;
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// Desc:媒体添加，微信号模糊匹配查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SearchMedia([FromUri]RequestGetMeidaInfoDto requestDto)
        {
            var jsonResult = new JsonResult() { Status = 500, Message = "请输入参数" };
            if (string.IsNullOrWhiteSpace(requestDto.Number))
                return jsonResult;
            jsonResult.Result = BLL.Media.MediaWeixin.Instance.SearchMedia(new MediaQuery<RespSearchMediaDto>()
            {
                KeyWord = requestDto.Number.ToSqlFilter(),
                PageSize = requestDto.PageSize
            });
            jsonResult.Message = string.Empty;
            jsonResult.Status = 0;
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 媒体资质信息查询（微信补充资料、媒体修改页面）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetQualification([FromUri]RequestGetWeiXinQualificationDto requestDto)
        {
            var jsonResult = new JsonResult() { Status = 500, Message = "请输入参数" };

            var userId = UserInfo.GetLoginUserID();

            //如果是新增:先去查询资质表，没有，则再去查用户详情表
            if (requestDto.IsInsert)
            {
                jsonResult.Result = BLL.Media.MediaQualification.Instance.GetInfo(userId);
            }
            else
            {
                //查看
                if (requestDto.MediaId <= 0)
                {
                    jsonResult.Message = "请输入媒体id";
                    return jsonResult;
                }

                var roleEnum = RoleInfoMapping.GetUserRole(userId);
                if (roleEnum == RoleEnum.SupperAdmin || roleEnum == RoleEnum.YunYingOperate)
                {
                    userId = 0;
                }

                jsonResult.Result = BLL.Media.MediaQualification.Instance.GetEntity(requestDto.MediaId, userId);
            }

            jsonResult.Message = string.Empty;
            jsonResult.Status = 0;
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 审核页面详情接口（对比编辑前与现在的数据）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetAuditDetailInfo([FromUri] RequestGetCommonlyCalssDto requestDto)
        {
            var jsonResult = Util.Verify<RequestGetCommonlyCalssDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            jsonResult.Result = new WeiXinOperate(new RequestWeiXinDto(), new ConfigEntity())
                .GetAuditDetailInfo(requestDto.MediaId);
            jsonResult.Message = string.Empty;

            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 媒体查询详情相关（详情页面的基本参数）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetItem([FromUri] RequestGetCommonlyCalssDto requestDto)
        {
            var jsonResult = new JsonResult
            {
                Result = new MediaOperateProxy(requestDto, new ConfigEntity()
                {
                    CreateUserId = UserInfo.GetLoginUserID()
                }).GetItem(),
                Message = string.Empty
            };

            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 后台媒体查询详情相关
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetItemForBack([FromUri] RequestGetCommonlyCalssDto requestDto)
        {
            var jsonResult = Util.Verify<RequestGetCommonlyCalssDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type
            };

            jsonResult.Result = new WeiXinOperate(new RequestWeiXinDto(), config).GetItemForBack(requestDto);
            jsonResult.Message = string.Empty;
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 后台-媒体查询详情相关（关联的媒体主）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetItemForMediaRole([FromUri] RequestGetCommonlyCalssDto requestDto)
        {
            var jsonResult = Util.Verify<RequestGetCommonlyCalssDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            jsonResult.Result = MediaWeixin.Instance.GetItemForMediaRole(requestDto.MediaId);
            jsonResult.Message = string.Empty;
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// 媒体常见分类推荐查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetRecommendClass([FromUri] RequestGetCommonlyCalssDto requestDto)
        {
            var jsonResult = Util.Verify<RequestGetCommonlyCalssDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;
            jsonResult.Result = new WeiXinOperate(new RequestWeiXinDto(), new ConfigEntity())
                 .GetRecommendClass(new PublishSearchAutoQuery<GetRecommendClassListDto>()
                 {
                     PageSize = requestDto.PageSize,
                     MediaId = requestDto.MediaId
                 });
            jsonResult.Message = string.Empty;
            return jsonResult;
        }

        /// <summary>
        /// auth：lixiong
        /// 获取媒体/刊例审核日志信息(默认返回一条数据，TopSize 可控制)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetAuditInfo([FromUri] RequestAuditInfoQueryDto requestDto)
        {
            var jsonResult = new JsonResult();
            var info =
                PublishInfoQuery.Instance.GetPublishAuditInfoList(
                    new PublishAuditInfoQuery<RespPublishAuditInfoDto>()
                    {
                        MediaId = requestDto.MediaId,
                        PubId = requestDto.PubId,
                        TemplateId = requestDto.TemplateId,
                        PageSize = requestDto.TopSize
                    });
            jsonResult.Result = info;
            jsonResult.Message = string.Empty;
            return jsonResult;
        }

        /// <summary>
        /// 获取微信列表-后台 ls
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaListB([FromUri] GetMediaListBReqDTO req)
        {
            var res = MediaInfo.Instance.GetMediaListB(req);

            if (res.List.Count > 0)
            {
                res.List.ForEach(item =>
                {
                    item.AreaMedia = AppOperate.MapperToCoverageArea(item.AreaMapping);
                });
            }

            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 获取微信列表-前台 ls
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaListF([FromUri] GetMediaListFReqDTO req)
        {
            var res = MediaInfo.Instance.GetMediaListF(req);
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 获取前台详情页统计图数据 ls
        /// </summary>
        /// <param name="MediaType"></param>
        /// <param name="MediaID"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaDetailStatistic(int MediaType, int MediaID)
        {
            if (MediaType != 14001)
                return Util.GetJsonDataByResult(null, "媒体类型错误", 1);
            var res = WeixinOAuth.Instance.GetDetailStatisticByMediaID(MediaID);
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        /// 获取媒体各审核状态数量 ls
        /// </summary>
        /// <param name="MediaType"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetMediaAuditStatusCount(int MediaType = 14001)
        {
            if (MediaType != 14001)
                return Util.GetJsonDataByResult(null, "媒体类型错误", 1);
            var res = MediaInfo.Instance.GetAuditCount();
            return Util.GetJsonDataByResult(res);
        }

        /// <summary>
        ///添加编辑媒体案列 2017-04-22 张立彬
        /// </summary>
        /// <param name="CaseInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult InsertMediaCaseInfo(MediaCase CaseInfo)
        {
            string strJson = Json.JsonSerializerBySingleData(CaseInfo);
            BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******InsertMediaCaseInfo begin...->CaseInfo:" + strJson + "");
            string messageStr = "";
            try
            {
                messageStr = BLL.Media.Business.V1_1.MediaCase.Instance.InsertMediaCaseInfo(CaseInfo);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaV1_1Controller]*****InsertMediaCaseInfo ->CaseInfo:" + strJson + ",添加或修改媒体案例失败:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******InsertMediaCaseInfo end->");
            return jr;
        }

        /// <summary>
        /// 查询媒体案列  2017-04-22 张立彬
        /// </summary>
        /// <param name="MeidaID"></param>
        /// <param name="MediaType"></param>
        /// <param name="CaseStatus"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SelectMediaCaseInfo(int MediaID, int MediaType, int CaseStatus)
        {
            string Msg;
            DataTable dt;
            try
            {
                dt = BLL.Media.Business.V1_1.MediaCase.Instance.SelectMediaCaseInfo(MediaID, MediaType, CaseStatus, out Msg);
                if (Msg != "")
                {
                    return Util.GetJsonDataByResult(null, Msg, -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaV1_1Controller]*****SelectMediaCaseInfo ->MediaID:" + MediaID + " ->mediaType:" + MediaType + "->CaseStatus:" + CaseStatus + ",查询反馈数据信息出错:" + ex.Message);
                throw ex;
            }

            return Util.GetJsonDataByResult(dt, "Success", 0);
        }

        /// <summary>
        /// 媒体的审核 2017-04-25 张立彬
        /// </summary>
        /// <param name="MediaID"></param>
        /// <param name="MediaType"></param>
        /// <param name="RejectMsg"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ToExamineMedia(int MediaID, int MediaType, string RejectMsg, int Status)
        {
            BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******ToExamineMedia begin...->MediaType:" + MediaType + "->MediaID:" + MediaID + "->Status:" + Status + "->RejectMsg:" + RejectMsg);
            string messageStr = "";
            int NextMediaID = 0;
            try
            {
                messageStr = MediaStatusOperate.Instance.ToExamineMedia(MediaID, MediaType, RejectMsg, Status, out NextMediaID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******ToExamineMedia begin...->MediaType:" + MediaType + "->MediaID:" + MediaID + "->Status:" + Status + "->RejectMsg:" + RejectMsg + ",审核媒体失败:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Util.GetJsonDataByResult(NextMediaID, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******ToExamineMedia end->");
            return jr;
        }

        /// <summary>
        /// 媒体的删除操作 2017-04-24 张立彬
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ToDeleteMedia(int MediaType, int MediaID)
        {
            BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******ToDeleteMedia begin...->MediaType:" + MediaType + "->MediaID:" + MediaID);
            string messageStr = "";
            try
            {
                messageStr = MediaStatusOperate.Instance.ToDeleteMedia(MediaType, MediaID);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******ToDeleteMedia begin...->MediaType:" + MediaType + "->MediaID:" + MediaID + ",删除媒体失败:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (messageStr != "")
            {
                jr = Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[MediaV1_1Controller]******ToDeleteMedia end->");
            return jr;
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult CheckCanAdd(int WxID)
        {
            bool res = BLL.MediaInfo.Instance.CheckCanAdd(WxID);
            return Util.GetJsonDataByResult(res);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetBasicDict(string Key)
        {
            var res = BLL.MediaInfo.Instance.GetWeixin_OAuthDict(Key);
            return Util.GetJsonDataByResult(res);
        }

        #region V1.1.4 Ls

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetAppQualification(int MediaType, int MediaID)
        {
            //dynamic res = new
            //{
            //    MediaRelations = 1,
            //    MediaRelationsName = "xx",
            //    OperatingType = 1,
            //    OperatingTypeName = "xxx",
            //    EnterpriseName = "企业名称",
            //    BusinessLicense = "营业执照",
            //    BLicenceFile = "营业执照附件",
            //    Q1 = "资质1",
            //    Q2 = "资质2",
            //    CanEdit = true
            //};
            //return Util.GetJsonDataByResult(res);
            if (!MediaType.Equals((int)MediaTypeEnum.APP))
                return Util.GetJsonDataByResult(null, "媒体类型错误", 1);
            var qua = BLL.MediaInfo.Instance.GetAppQualificationInfo(MediaID, MediaType);
            if (qua == null)
                return Util.GetJsonDataByResult(null, "请求失败", 1);
            return Util.GetJsonDataByResult(qua);
        }

        /// <summary>
        /// 2017-06-05 zlb
        /// 查询拉黑或收藏媒体列表
        /// </summary>
        /// <param name="SelectType">0收藏 1拉黑</param>
        /// <param name="PageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SelectCollectionPullBlack(int SelectType, int PageIndex = 1, int pageSize = 20)
        {
            string Msg;
            ListTotal lt = null;
            try
            {
                lt = BLL.Media.Business.V1_4.MediaCommonInfo.Instance.SelectCollectionPullBlack(SelectType, PageIndex, pageSize, out Msg);
                if (Msg != "")
                {
                    return Util.GetJsonDataByResult(null, Msg, -1);
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MediaV1_1Controller]*****SelectCollectionPullBlack ->SelectType:" + SelectType + " ->PageIndex:" + PageIndex + "->pageSize:" + pageSize + ",查询收藏拉黑媒体列表出错:" + ex.Message);
                throw ex;
            }

            return Util.GetJsonDataByResult(lt, "Success", 0);
        }

        /// <summary>
        /// 2017-06-05 zlb
        /// 查询上架广告最多的前十的城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SelectTopTenCitys()
        {
            DataTable dt = Chitunion2017.BLL.AppTemplate.Instance.SelectTopTenCitys();
            return Util.GetJsonDataByResult(dt, "Success");
        }

        /// <summary>
        /// 2017-06-05 zlb
        /// 查询上架的城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult SelectPublishCitys()
        {
            DataTable dt = Chitunion2017.BLL.AppTemplate.Instance.SelectPublishCitys();
            return Util.GetJsonDataByResult(dt, "Success");
        }

        #endregion V1.1.4 Ls

        #region V1.1.6

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public JsonResult GetOwnWeixinList()
        {
            var res = BLL.WxEditor.WxArticleGroup.Instance.GetOwnWeixinList();
            return Util.GetJsonDataByResult(res, "Success");
            List<OwnWeixinItemDTO> list = new List<OwnWeixinItemDTO>();
            list.Add(new OwnWeixinItemDTO()
            {
                WxID = 1,
                WxName = "赤兔联盟",
                WxNumber = "chitunews"
            });
            list.Add(new OwnWeixinItemDTO()
            {
                WxID = 2,
                WxName = "迷蒙",
                WxNumber = "mimeng008"
            });
            list.Add(new OwnWeixinItemDTO()
            {
                WxID = 3,
                WxName = "搜达足球",
                WxNumber = "soudafootball"
            });
            return Util.GetJsonDataByResult(list, "Success");
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public JsonResult GetWeixinAuthorityList(int WxID)
        {
            var res = BLL.WeixinOAuth.Instance.GetWeixinAuthorityList(WxID);
            //res = new List<string>() { "用户管理", "素材管理", "权限管理" };
            return Util.GetJsonDataByResult(res, "Success");
        }

        #endregion V1.1.6

        /// <summary>
        /// Auth:lixiong
        /// Desc:获取分类（媒体没有引用的分类不展示）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetCategoryList([FromUri]RequestAuditInfoQueryDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null || request.BusinessType <= 0)
            {
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }

            jsonResult.Result = BLL.Ranking.StatWeixinRankList.Instance.GetCategoryList(request.BusinessType);

            return jsonResult;
        }

        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult VerifyWxNumber([FromBody] RequestMeidaVerifyOfAdd requestDto)
        {
            var jsonResult = new JsonResult();
            if (string.IsNullOrWhiteSpace(requestDto?.MediaName))
            {
                jsonResult.Message = "请输入参数:MediaName";
                return jsonResult;
            }
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type,
                SourceTypeEnum = (SourceEnum)userInfo.Source
            };
            var retValue = new ReturnValue();
            retValue = new BLL.Media.Business.V1_1.WeiXinOperate(new RequestWeiXinDto()
            {
                Number = requestDto.MediaName,
                CreateUserID = config.CreateUserId
            }, config).VerifyNumber(retValue);

            if (retValue.HasError)
            {
                jsonResult.Status = retValue.ErrorCode.ToInt(-2);
                jsonResult.Message = retValue.Message;
            }

            return jsonResult;
        }
    }
}