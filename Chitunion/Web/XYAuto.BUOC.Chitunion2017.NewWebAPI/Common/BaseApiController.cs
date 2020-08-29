/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 16:06:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Common
{
    public class BaseApiController : ApiController
    {
        public XYAuto.ITSC.Chitunion2017.Common.LoginUser GetUserInfo =>
                XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUser();
    }

    public static class ApiJsonResultExtend
    {
        public static JsonResult GetReturn(this JsonResult jsonResult, ReturnValue retValue)
        {
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                jsonResult.Status = retValue.ErrorCode.ToInt(-1);
                jsonResult.Result = retValue.ReturnObject;
                return jsonResult;
            }
            jsonResult.Status = retValue.ErrorCode.ToInt();
            jsonResult.Message = jsonResult.Message ?? "操作成功";
            jsonResult.Result = retValue.ReturnObject;
            return jsonResult;
        }
    }
}