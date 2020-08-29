using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Filter;
using XYAuto.ITSC.Chitunion2017.BLL.CarSerialInfo.Dto.Request;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Controllers
{
    [CrossSite]
    public class CarSerialController : ApiController
    {

        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult QueryBrand([FromUri] RequestCarBrandDto request)
        {
            var jsonResult = new JsonResult();

            //根据MasterBrandId 查询品牌
            //if (request.MasterBrandId > 0)
            //{
            //    jsonResult.Result = ITSC.Chitunion2017.BLL.CarSerialInfo.CarSerial.Instance.GetBrandSerialList(request.MasterBrandId);
            //}
            //else 

            if (!string.IsNullOrEmpty(request.MasterBrandName))//根据主品牌名称查询主品牌信息
            {
                //jsonResult.Result = ITSC.Chitunion2017.BLL.CarSerialInfo.CarSerial.Instance.GetMasterListByName(request.MasterBrandName);
                jsonResult.Result = ITSC.Chitunion2017.BLL.CarSerialInfo.CarSerial.Instance.GetCarAllInfoList(request.MasterBrandName);
            }
            else//查询的是master品牌
            {
                jsonResult.Result = ITSC.Chitunion2017.BLL.CarSerialInfo.CarSerial.Instance.GetCarAllInfoListCache(1440);
                //jsonResult.Result = ITSC.Chitunion2017.BLL.CarSerialInfo.CarSerial.Instance.GetMasterBrandList();
            }

            return jsonResult;
        }
    }
}
