using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.Demand.Resolver;
using XYAuto.BUOC.BOP2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.BOP2017.Entities.Dto;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;
using XYAuto.BUOC.BOP2017.WebAPI.App_Start;
using XYAuto.BUOC.BOP2017.WebAPI.Common;
using XYAuto.BUOC.BOP2017.WebAPI.Filter;

namespace XYAuto.BUOC.BOP2017.WebAPI.Controllers
{
    [CrossSite]
    public class GroundPageController : BaseApiController
    {
        /// <summary>
        /// auth:lixiong
        /// desc:落地页查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetList([FromUri] RequestGroundPage request)
        {
            var jsonResult = new JsonResult
            {
                Result = new GroundPageProvider(null, null, null).GetGroundPages(request.DemandBillNo),
                Message = "success"
            };

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:落地页添加操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS005BUT3002")]
        public JsonResult AddPage([FromBody] RequestGroundPageDto request)
        {
            var jsonResult = new JsonResult();
            var provider = new GroundPageProvider(request, null, new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            });
            var retValue = provider.ExcutePage();

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:落地页删除操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS005BUT3003")]
        public JsonResult DeletePage([FromBody]RequestDeletePageDto request)
        {
            var jsonResult = new JsonResult();
            var provider = new GroundPageProvider(null, request, new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            });
            var retValue = provider.Delete();

            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:落地页加参添加操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS005BUT3004")]
        public JsonResult AddDelivery([FromBody]RequestGroundDeliveryDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new DemandGroundDeliveryProvider(request, null, new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }).Excute();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:获取加参列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDeliverys([FromUri] RequestGroundDeliveryDto request)
        {
            var jsonResult = new JsonResult
            {
                Result = new DemandGroundDeliveryProvider(null, null, null).GetGroundDeliverys(request.DemandBillNo),
                Message = "success"
            };

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:删除加参
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS005BUT3005")]
        public JsonResult DeleteDelivery([FromBody] RequestDeleteDeliveryDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new DemandGroundDeliveryProvider(null, request, new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }).Delete();
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:落地页加参-关联广告组(默认加载列表)
        /// </summary>
        /// <param name="deliveryId">加参id</param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetWaittingAdList(int deliveryId)
        {
            string msg = string.Empty;
            List<ADGroupDTO> res = BLL.Demand.Demand.Instance.GetWaittingADList(deliveryId, ref msg);
            return Util.GetJsonDataByResult(res, msg, res == null ? 1 : 0);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:关联广告组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true, CheckModuleRight = "SYS005BUT3006")]
        public JsonResult RelateToAdGroup([FromBody]RequestRelateToAdGroupDto request)
        {
            var jsonResult = new JsonResult();

            var retValue = new DemandGroundDeliveryProvider(null, null, new ConfigEntity()
            {
                CreateUserId = GetUserInfo.UserID
            }).RelateToAdGroup(request);
            return jsonResult.GetReturn(retValue);
        }

        /// <summary>
        /// auth:lixiong
        /// desc:需求下的品牌车型，城市信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetCarAndCityInfos([FromUri]RequestGroundPage request)
        {
            var jsonResult = new JsonResult();

            if (request == null || request.DemandBillNo <= 0)
            {
                jsonResult.Message = "请输入参数：DemandBillNo";
                return jsonResult;
            }
            jsonResult.Result = new DemandJsonAnalysis().GetDemandCarAndCityInfos(request.DemandBillNo);

            jsonResult.Message = "success";
            return jsonResult;
        }
    }
}