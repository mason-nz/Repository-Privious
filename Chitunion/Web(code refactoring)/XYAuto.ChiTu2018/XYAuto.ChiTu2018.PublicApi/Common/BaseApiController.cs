/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 16:06:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Web.Http;
using XYAuto.ChiTu2018.Infrastructure.Extensions;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.PublicApi.Models;

namespace XYAuto.ChiTu2018.PublicApi.Common
{
    /// <summary>
    /// api 基类
    /// </summary>
    public class BaseApiController : ApiController
    {
    }

    /// <summary>
    /// 返回类型扩展类
    /// </summary>
    public static class ApiJsonResultExtend
    {
        /// <summary>
        /// 返回类型扩展方法
        /// </summary>
        /// <param name="jsonResult"></param>
        /// <param name="retValue"></param>
        /// <returns></returns>
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
            jsonResult.Message = jsonResult.Message ?? "success";
            jsonResult.Result = retValue.ReturnObject;
            return jsonResult;
        }
    }
}