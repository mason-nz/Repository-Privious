using System;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.Infrastructure.HttpLog
{
    /// <summary>
    /// auth:lixiong
    /// 接口请求AOP简单模式，打印请求，返回日志信息
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
            //这里为了能收到错误邮件 才写了具体的实现日志
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Error(errorMessage);

            //var responseInfo = new RespBaseDto<T>()
            //{
            //    Message = errorMessage,
            //    Code = 500,
            //};
            return default(T);
        }

        private T GetResult<T>(string httpResult)
        {
            var baseDto = JsonConvert.DeserializeObject<T>(httpResult);

            return baseDto;
        }
        
    }

    public enum HttpMethodEnum
    {
        Post,
        Get
    }
}