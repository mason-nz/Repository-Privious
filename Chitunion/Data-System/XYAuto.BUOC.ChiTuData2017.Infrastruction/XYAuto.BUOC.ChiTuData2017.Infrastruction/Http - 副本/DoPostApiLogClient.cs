/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 16:03:24
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using Newtonsoft.Json;

namespace XYAuto.BUOC.ChiTuData2017.Infrastruction.Http
{
    /// <summary>
    /// auth:lixiong
    /// desc:接口请求AOP简单模式，打印请求，返回日志信息
    /// </summary>
    public class DoPostApiLogClient
    {
        private readonly string _requestUrl;
        private readonly string _requestParmas;

        public DoPostApiLogClient(string requestUrl, string requestParmas)
        {
            _requestUrl = requestUrl;
            _requestParmas = requestParmas;
        }

        public T GetPostResult<T>(Func<string, string> actionHttp, Action<string> actionLog)
        {
            try
            {
                //var httpResult = "{\"code\":0,\"message\":\"\",\"data\":{\"list\":[{\"fund_type\":\"GENARAL_CASH\",\"trade_type\":\"CHARGE\",\"time\":1403243242,\"amount\":4000,\"description\":\"充值\"}]}}";
                //return GetResult<T>(httpResult);
                return OperationTraction<T>(_requestUrl, _requestParmas, actionHttp, actionLog);
            }
            catch (Exception exception)
            {
                return OperationExceptionMessage<T>(exception, _requestUrl, _requestParmas, actionLog);
            }
        }

        private T OperationTraction<T>(string requestUrl, string requestParmas,
            Func<string, string> actionHttp, Action<string> actionLog)
        {
            //请求接口开始日志
            var start = DateTime.Now;
            var httpResult = actionHttp(string.Empty);
            //请求接口返回日志
            actionLog?.Invoke(string.Format("{0}请求url地址:{1}{0}请求参数:{2}{0}花费时间:{3}ms{0}接口返回数据:{4}{0}", System.Environment.NewLine,
                                        requestUrl, requestParmas, (DateTime.Now - start).TotalMilliseconds, httpResult));
            return GetResult<T>(httpResult);
        }

        private T OperationExceptionMessage<T>(Exception exception, string postUrl,
            string postData, Action<string> actionLog)
        {
            var errorMessage = string.Format("{0}请求url地址:{1}{0}请求参数:{2}{0}异常信息:{3}{0}", System.Environment.NewLine,
                postUrl, postData, exception.Message + exception.StackTrace ?? string.Empty);

            actionLog?.Invoke(errorMessage);
            //这里为了能收到错误邮件
            Loger.Log4Net.Error(errorMessage);

            //var responseInfo = new RespBaseDto<T>()
            //{
            //    Message = errorMessage,
            //    Code = 500,
            //};
            return default(T);
        }

        private T GetResult<T>(RespBaseDto<T> respBaseDto)
        {
            if (respBaseDto == null)
                return default(T);
            return respBaseDto.Data;
        }

        private T GetResult<T>(string httpResult)
        {
            var baseDto = JsonConvert.DeserializeObject<T>(httpResult);

            return baseDto;
        }

        private RespBaseDto<T> GetBaseResult<T>(string httpResult)
        {
            return JsonConvert.DeserializeObject<RespBaseDto<T>>(httpResult);
        }

        protected void LogInfo(string info)
        {
            Loger.Log4Net.Info(info);
        }

        protected void LogErrorInfo(string info, Exception exception)
        {
            Loger.Log4Net.ErrorFormat(info);
        }
    }

    public class RespBaseDto
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class RespBaseDto<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public enum HttpMethodEnum
    {
        Post,
        Get
    }
}