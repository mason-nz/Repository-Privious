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
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Media;
using JsonResult = XYAuto.BUOC.Chitunion2017.NewWebAPI.Common.JsonResult;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class MediaController : BaseApiController
    {

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主个人中心-统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetStatistInfo()
        {
            var jsonResult = new JsonResult {Result = new OrderProvider().GetUserStatInfo(GetUserInfo)};

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主-个人中心-已绑定微信列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetBindingsWxList([FromUri] ReqMediaBindingsDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new BindingsWeiXinQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主-个人中心-已绑定微博列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetBindingsWbList([FromUri] ReqMediaBindingsDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }
            request.UserId = GetUserInfo.UserID;
            jsonResult.Result = new BindingsWeiBoQuery(new ConfigEntity()).GetQueryList(request);
            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主-个人中心-已绑定微博列表-修改报价
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult UpdateWbOffer([FromBody]ReqMediaUpdateWbOfferDto request)
        {
            var jsonResult = new JsonResult();
            var retValue = new BindingWeiBoProvider(new ConfigEntity(), request).UpdateOffer();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:媒体主-个人中心-已绑定微信列表-修改报价
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult UpdateWxOffer([FromBody]ReqMediaUpdateWxOfferDto request)
        {
            var jsonResult = new JsonResult();
            var retValue = new BindingWeiXinProvider(new ConfigEntity(), request).UpdateOffer();
            return jsonResult.GetReturn(retValue);
        }


        /// <summary>
        /// auth:lixiong
        /// desc:媒体主-个人中心-微信报价信息加载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetBindingsWxInfo([FromUri] ReqMediaInfoDto request)
        {
            var jsonResult = new JsonResult();
            if (request == null)
            {
                jsonResult.Status = -1;
                jsonResult.Message = "请输入参数";
                return jsonResult;
            }

            jsonResult.Result = new BindingWeiXinProvider(new ConfigEntity(), null).GetInfo(request.MediaId);
            return jsonResult;
        }
    }
}