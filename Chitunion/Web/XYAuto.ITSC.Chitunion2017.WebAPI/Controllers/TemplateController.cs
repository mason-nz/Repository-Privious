using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using Ionic.Zip;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Common;
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
    public class TemplateController : ApiController
    {
        [HttpPost]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult AuditTemplate(AuditTemplateDTOReq req)
        {
            string msg = string.Empty;
            var res = BLL.AppTemplate.Instance.AuditTemplate(req.TemplateID, req.OpType, req.RejectReason, ref msg);
            return Common.Util.GetJsonDataByResult(res, msg, res != null ? 0 : 1);
        }

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult CheckCanAddModifyTemplate(int baseTemplateID)
        {
            var res = BLL.AppTemplate.Instance.CheckCanAddModifyTemplate(baseTemplateID);
            return Common.Util.GetJsonDataByResult(res, "Success", 0);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:模板新增、编辑
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Curd([FromBody]RequestMediaDto requestDto)
        {
            var jsonResult = new JsonResult();
            var userInfo = UserInfo.GetLoginUser();
            var config = new ConfigEntity()
            {
                CreateUserId = userInfo.UserID,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(userInfo.UserID),
                UserType = (UserTypeEnum)userInfo.Type
            };

            var retValue = new MediaOperateProxy(requestDto, config).Excute();
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Result = new
            {
                AdTemplateId = retValue.ReturnObject == null
                    ? 0
                    : retValue.ReturnObject.ToString().ToInt()
            };
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:模板新增、编辑的名称校验
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult VerifyAdTemplateName([FromBody]RequestTemplateInfoDto requestDto)
        {
            var jsonResult = new JsonResult();
            if (requestDto == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }

            requestDto.CreateUserId = UserInfo.GetLoginUserID();

            if (!Enum.IsDefined(typeof(OperateType), requestDto.OperateType))
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请确定是新增还是编辑操作";
                return jsonResult;
            }
            var provider =
                new AdTemplateProvider(new ConfigEntity()
                {
                    CureOperateType = (OperateType)requestDto.OperateType,
                    CreateUserId = requestDto.CreateUserId,
                    RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId),
                }, requestDto).VerifyAdTemplateName();

            if (provider.HasError)
            {
                jsonResult.Result = provider.ReturnObject;
                jsonResult.Message = provider.Message;
                jsonResult.Status = provider.ErrorCode.ToInt(-1);
                return jsonResult;
            }
            jsonResult.Result = provider.ReturnObject;
            jsonResult.Message = provider.Message;

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:模板详情接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetInfo([FromUri]RequestGetMeidaInfoDto requestDto)
        {
            var jsonResult = new JsonResult
            {
                Result = new MediaOperateProxy(requestDto, new ConfigEntity()
                {
                    CreateUserId = UserInfo.GetLoginUserID()
                }).QueryInfo(),
                Message = string.Empty,
                Status = 0
            };

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:模板列表接口(运营角色)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Query([FromUri]RequestAdQueryDto requestDto)
        {
            var jsonResult = Common.Util.Verify<RequestAdQueryDto>(requestDto);
            if (jsonResult.Status != 0)
                return jsonResult;

            requestDto.CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID(); ;//后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId)
            };

            jsonResult.Message = "success";
            jsonResult.Status = 0;
            jsonResult.Result = new AdQueryProxy(config, requestDto).GetQuery();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:获取模板待审核、驳回的数据统计总数（暂只有运营角色存在）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
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
                BusinessType = (int)MediaType.Template,
                CreateUserId = userId
            });
            return jsonResult;
        }

        /// <summary>
        /// 2017-06-05 zlb
        /// 删除APP模板
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <returns></returns>
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult ToDeleteAppTemplate(int TemplateID)
        {
            BLL.Loger.Log4Net.Info("[TemplateController]******ToDeleteAppTemplate begin...->TemplateID:" + TemplateID);
            int result = 0;
            string messageStr = "";
            try
            {
                result = AppTemplate.Instance.ToDeleteAppTemplate(TemplateID, out messageStr);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[TemplateController]******ToDeleteAppTemplate begin...->TemplateID:" + TemplateID + "删除模板失败:" + ex.Message);
                throw ex;
            }
            JsonResult jr;
            if (result <= 0)
            {
                jr = Common.Util.GetJsonDataByResult(null, messageStr, -1);
            }
            else
            {
                jr = Common.Util.GetJsonDataByResult(null, "Success", 0);
            }
            BLL.Loger.Log4Net.Info("[TemplateController]******ToDeleteAppTemplate end->");
            return jr;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:模板审核页面-左边的最新模板接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetParentInfo([FromUri]RequestTemplateInfoDto requestDto)
        {
            //todo:
            /*
             场景一：首先判断带过来的adTempateId 是否存在BaseAdId,是套用的公共模板，存在就展示在页面，然后继续获取引用这个公共模板的列表
             场景二：如果页面是多个勾选过来的adTempateId，则展示这些id的列表
             */

            var adProvider = new AdTemplateProvider(new ConfigEntity()
            {
                CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID()
            }, requestDto).GetAuditViewList(true);

            return new JsonResult()
            {
                Status = adProvider == null ? -1 : 0,
                Result = adProvider
            };
        }

        /// <summary>
        /// auth:lixiong
        /// desc:模板审核页面-右边的模板列表接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult List([FromUri]RequestTemplateInfoDto requestDto)
        {
            //todo:
            /*
                1.传过来的AdTempId是待审核的id，查找当前id的信息，查找是否有BaseAdID，如果存在有效的id则是正常的数据
                2.如果BaseAdID没有，则返回status为-1，告知数据不显示
                3.再根绝BaseAdID获取列表，查找BaseAdID=@BaseAdID 的条件
             */

            var adProvider = new AdTemplateProvider(new ConfigEntity()
            { CreateUserId = Chitunion2017.Common.UserInfo.GetLoginUserID() }
            , requestDto).GetAuditViewList(false);

            return new JsonResult()
            {
                Result = adProvider
            };
        }
    }
}