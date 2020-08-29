using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Task;
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeTask;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using JsonResult = XYAuto.BUOC.Chitunion2017.NewWebAPI.Common.JsonResult;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class TaskController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:领取任务-贴片广告-选择微信列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetSelectWxList([FromUri] ReqMediaBindingsDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new TaskSelectWeiXinQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// 领取任务操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult Receive([FromBody]ReqTaskReceiveDto request)
        {
            var jsonResult = new JsonResult();

            if (GetUserInfo.Category != 29002)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "没有权限访问";
                return jsonResult;
            }
            request.IP = TaskProvider.GetIP();
            var retValue = new TaskProvider(new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }, request).Receive();
            return jsonResult.GetReturn(retValue);
        }

        
        /// <summary>
        /// auth:lixiong
        /// desc:领取任务-内容分发列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDistrbuteList([FromUri]ReqOrderCoverImageDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }

            //throw new Exception("test");

            request.TaskType = LeTaskTypeEnum.ContentDistribute;
            jsonResult.Result = new TaskRecCoverImageQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:领取任务-贴片广告列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetCoverImageList([FromUri]ReqOrderCoverImageDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.TaskType = LeTaskTypeEnum.CoverImage;
            jsonResult.Result = new TaskRecCoverImageQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:领取任务-校验订单数量-一个用户一天领任务没有上限，但只对前5个订单进行结算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult VerifyEffective()
        {
            var jsonResult = new JsonResult();
            var retValue = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto())
                 .VerifyReceiveTaskCount(GetUserInfo.UserID);
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:领取任务-内容分发任务列表-分类列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetCategoryList()
        {
            var jsonResult = new JsonResult();

            jsonResult.Result = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()).GeTaskCategories();
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:H5任务-内容分发任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetH5List([FromUri]ReqTaskH5Dto request)
        {
            var jsonResult = new JsonResult();

            jsonResult.Result = new TaskH5Query(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }



    }
}