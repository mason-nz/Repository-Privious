using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Filter;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class DictInfoController : ApiController
    {
        [HttpGet]
        public JsonResult GetDictInfoByTypeId(int typeId)
        {
            DataTable dt = BLL.DicInfo.DictInfo.Instance.GetDictInfoByTypeId(typeId);
            return Util.GetJsonDataByResult(dt, "Success");
        }

        /// <summary>
        /// auth:lixiong
        /// desc:获取父ip
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetBaseLable([FromUri] int typeId)
        {
            var jsonResult = new JsonResult();

            jsonResult.Result = BLL.DicInfo.TitleBasicInfo.Instance.GeTitleBasicInfos((LableTypeEnum)typeId);

            return jsonResult;
        }

        /// <summary>
        /// auth:lixiong
        /// desc:获取子ip
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiLog]
        public JsonResult GetChildrenLable([FromUri] int titleId)
        {
            var jsonResult = new JsonResult();

            jsonResult.Result = BLL.DicInfo.TitleBasicInfo.Instance.GetChildrenLable(titleId);

            return jsonResult;
        }
    }
}