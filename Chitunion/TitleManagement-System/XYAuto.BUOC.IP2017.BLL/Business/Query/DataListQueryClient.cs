using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.BUOC.IP2017.BLL.Business.Query
{
    public abstract class DataListQueryClient<T, TResponse>
        where T : CreateQueryBase, new()
        where TResponse : //BaseResponseEntity,
        new()
    {
        //protected readonly ConfigEntity ConfigEntity;
        protected T RequestQuery;

        protected DataListQueryClient()
        {
        }

        /// <summary>
        /// 对外公开查询方法
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public BaseResponseEntity<TResponse> GetQueryList(T query)
        {
            try
            {
                RequestQuery = query;
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
            var queryParams = GetQueryParams();
            LogInfo(string.Format("{0}当前操作类型:{0} 查询条件信息:{1}", 
                    System.Environment.NewLine, JsonConvert.SerializeObject(query)));

            if (queryParams.StrSql.Contains("YanFaFROM"))
            {
                var list = DataListQuery.Instance.QueryList(queryParams);
                return GetResult(list, queryParams);
            }
            else
            {
                var list = DataListQuery.Instance.QueryListBySql2014Offset_Fetch(queryParams);
                return GetResult(list, queryParams);
            }
        }

        /// <summary>
        /// 获取查询条件参数
        /// </summary>
        /// <returns></returns>
        protected abstract Entities.Query.DataListQuery<TResponse> GetQueryParams();

        /// <summary>
        /// 结果处理
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual BaseResponseEntity<TResponse> GetResult(List<TResponse> resultList, Entities.Query.DataListQuery<TResponse> query)
        {
            var responseData = new BaseResponseEntity<TResponse>
            {
                List = resultList,
                TotalCount = query.Total
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
            var logError = string.Format("{0}当前操作类型:{0}异常信息:{1}",
             System.Environment.NewLine, errorMessage);
            LogErrorInfo(logError, exception);
            var responseInfo = new BaseResponseEntity<TResponse>()
            {
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
