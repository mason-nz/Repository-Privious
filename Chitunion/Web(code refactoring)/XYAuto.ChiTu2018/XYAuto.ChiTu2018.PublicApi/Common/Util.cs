using System;
using System.Linq;
using System.Web;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.ChiTu2018.PublicApi.Models;

namespace XYAuto.ChiTu2018.PublicApi.Common
{
    /// <summary>
    /// 
    /// </summary>
    internal class Util
    {
       
        /// <summary>
        /// 根据查询结果，返回通用Json格式
        /// </summary>
        /// <param name="obj">查询结果</param>
        /// <param name="message">返回Message信息，默认为：OK</param>
        /// <param name="status">返回status信息，默认为：0</param>
        /// <returns>返回Json数据</returns>
        public static JsonResult GetJsonDataByResult(object obj, string message = "OK", int status = 0)
        {
            var jr = new JsonResult();
            jr.Status = status;
            jr.Message = message;
            if (obj != null)
            {
                jr.Result = obj;
            }

            return jr;
        }

        /// <summary>
        /// Auth:lixiong
        /// 查询参数校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public static JsonResult Verify<T>(T requestDto) where T : class, new()
        {
            var jsonResult = new JsonResult() { Message = "请输入参数", Status = 500 };
            if (requestDto == null)
                return jsonResult;
            var retValue = new VerifyOfNecessaryParameters<T>().VerifyNecessaryParameters(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            return jsonResult;
        }

    }
}