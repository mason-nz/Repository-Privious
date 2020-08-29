using System.Web.Http;
using System.Data;
using XYAuto.BUOC.BOP2017.WebAPI.App_Start;
using XYAuto.BUOC.BOP2017.WebAPI.Filter;
using XYAuto.BUOC.BOP2017.WebAPI.Common;

namespace XYAuto.BUOC.BOP2017.WebAPI.Controllers
{
    [CrossSite]
    public class DictInfoController : ApiController
    {
        [HttpGet]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetDictInfoByTypeId(int typeId)
        {
            DataTable dt = BLL.DicInfo.DictInfo.Instance.GetDictInfoByTypeId(typeId);
            return Common.Util.GetJsonDataByResult(dt, "Success");
        }
    }
}