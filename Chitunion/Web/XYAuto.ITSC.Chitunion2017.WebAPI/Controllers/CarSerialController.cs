using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.CarSerialInfo.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    public class CarSerialController : ApiController
    {
        /// <summary>
        /// Auth:lixiong
        /// Desc:车型品牌
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult QueryBrand([FromUri] RequestCarBrandDto request)
        {
            var jsonResult = new JsonResult();

            //todo:默认进来，查询的是master品牌
            //todo:第二次查询的是根据MasterBrandId 查询品牌
            if (request.MasterBrandId > 0)
            {
                jsonResult.Result = BLL.CarSerialInfo.CarSerial.Instance.GetBrandList(request.MasterBrandId);
                return jsonResult;
            }
            jsonResult.Result = BLL.CarSerialInfo.CarSerial.Instance.GetMasterBrandList();
            return jsonResult;
        }

        /// <summary>
        /// Auth:lixiong
        /// Desc:车系列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult QuerySerialList([FromUri] RequestCarBrandDto request)
        {
            var jsonResult = new JsonResult();

            if (request == null || request.BrandId <= 0)
            {
                return jsonResult;
            }
            jsonResult.Result = BLL.CarSerialInfo.CarSerial.Instance.GetCarSerialList(request.BrandId);

            return jsonResult;
        }
    }
}