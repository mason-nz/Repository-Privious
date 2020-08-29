using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.ThirdApi;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class ThirdBusinessController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:对外提供api-任务入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult TaskStorage([FromBody] ReqTaskStorageDto request)
        {
            var jsonResult = new JsonResult();
            var retValue = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()).ThirdApiTaskStorage(request);
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:对外提供api-物料作废通知
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult MaterielToAbandoned([FromBody] ReqTaskStatusNoteDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            var retValue = new TaskProvider(new ConfigEntity(), new ReqTaskReceiveDto()).ToAbandoned(request.MaterielId);
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:对外提供api-内容分发-订单入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult OrderStorage([FromBody] ReqOrderStorageDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new TaskProvider(new ConfigEntity(), request).ThirdApiOrderStorage();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// 领取任务操作_微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        public JsonResult ReceiveByWx([FromBody]ReqTaskReceiveDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            var retValue = new TaskProvider(new ConfigEntity()
            {
                CreateUserId = request.UserId
            }, request).ReceiveByWeiXin();
            return jsonResult.GetReturn(retValue);
        }


        /// <summary>
        /// 模拟登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public Common.JsonResult SimulationLogin([FromUri]int userId)
        {
            var jsonResult = new JsonResult();

            if (userId <= 0)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入用户id";
                return jsonResult;
            }

            //模拟登录
            var loginCookie = XYAuto.ITSC.Chitunion2017.Common.UserInfo.Instance.Passport(userId);
            if (string.IsNullOrWhiteSpace(loginCookie))
            {
                jsonResult.Status = -1;
                jsonResult.Message = "Passport 用户模拟登录失败，未返回登录相关cookie";
                return jsonResult;
            }
            return jsonResult;
        }
      
    }
}