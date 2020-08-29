using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish
{
    /// <summary>
    /// Auth:李雄
    /// 刊例查询接口抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class PublishInfoQueryClient<T, TResponse>
        where T : CreatePublishQueryBase, new()
        where TResponse : //BaseResponseEntity,
        new()
    {
        protected readonly ConfigEntity ConfigEntity;
        protected T RequetQuery;
        private QueryPageBase<TResponse> queryParams;

        protected PublishInfoQueryClient(ConfigEntity configEntity)
        {
            ConfigEntity = configEntity;
        }

        /// <summary>
        /// 对外公开查询方法
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseResponseEntity<TResponse> GetQueryList(T query)
        {
            RequetQuery = query;
            try
            {
              
                return QueryOperate(query);
            }
            catch (Exception exception)
            {
                //todo:log exception
                return OperationExceptionMessage(exception, null);
            }
        }

        /// <summary>
        /// 具体操作方法
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected BaseResponseEntity<TResponse> QueryOperate(T query)
        {
             queryParams = GetQueryParams();
            LogInfo(string.Format("{1}当前操作类型:{0}{1} 查询条件信息:{2}", ConfigEntity.BusinessType,
                    System.Environment.NewLine, JsonConvert.SerializeObject(query)));

            if (queryParams.StrSql.Contains("YanFaFROM"))
            {
                var list = PublishInfoQuery.Instance.QueryList<TResponse>(queryParams);
                return GetResult(list, queryParams);
            }
            else
            {
                var list = PublishInfoQuery.Instance.QueryListBySql2014Offset_Fetch(queryParams);
                return GetResult(list, queryParams);
            }
        }

        /// <summary>
        /// 获取查询条件参数
        /// </summary>
        /// <returns></returns>
        protected abstract PublishQuery<TResponse> GetQueryParams();

        /// <summary>
        /// 结果处理
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual BaseResponseEntity<TResponse> GetResult(List<TResponse> resultList, QueryPageBase<TResponse> query)
        {
            var responseData = new BaseResponseEntity<TResponse>
            {
                List = resultList,
                TotleCount = query.Total
            };
            return responseData;
        }

        /// <summary>
        /// 错误机制处理
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private BaseResponseEntity<TResponse> OperationExceptionMessage(Exception exception, Action<string> action)
        {
            var errorMessage = string.Format("{0}{1}{2}", exception.Source, System.Environment.NewLine,
                                               exception);
            var logError = string.Format("{0}异常信息:{1}", System.Environment.NewLine, errorMessage);

            logError += $"{System.Environment.NewLine}Sql语句：{queryParams.StrSql}" +
                        $"{RequetQuery.SqlWhere}";

            LogErrorInfo(logError, exception);
            var responseInfo = new BaseResponseEntity<TResponse>()
            {
                List = new List<TResponse>()
            };

            return responseInfo;
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
}