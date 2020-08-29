/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 19:11:03
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;

namespace XYAuto.BUOC.BOP2017.Infrastruction.Verification
{
    /// <summary>
    /// auth:lixiong
    /// desc:基本方法提供
    /// </summary>
    public class VerifyOperateBase
    {
        /// <summary>
        /// auth:lixiong
        /// desc:查询参数校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        protected ReturnValue VerifyOfNecessaryParameters<T>(T requestDto) where T : class, new()
        {
            var jsonResult = new ReturnValue() { Message = "请输入参数", HasError = true, ErrorCode = "500" };
            if (requestDto == null)
                return jsonResult;
            var retValue = new VerifyOfNecessaryParameters<T>().VerifyNecessaryParameters(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.HasError = false;
            jsonResult.Message = "success";
            jsonResult.ErrorCode = String.Empty;
            return jsonResult;
        }

        public ReturnValue CreateFailMessage(ReturnValue retValue, string errorCode, string errorMsg)
        {
            return CreateFailMessage(retValue, errorCode, errorMsg, null);
        }

        public ReturnValue CreateFailMessage(ReturnValue retValue, string errorCode, string errorMsg, object returnObj)
        {
            return CreateMessage(retValue, true, errorCode, errorMsg, returnObj);
        }

        public ReturnValue CreateSuccessMessage(ReturnValue retValue, string errorCode, string errorMsg)
        {
            return CreateSuccessMessage(retValue, errorCode, errorMsg, null);
        }

        public ReturnValue CreateSuccessMessage(ReturnValue retValue, string errorCode, string errorMsg, object returnObj)
        {
            return CreateMessage(retValue, false, errorCode, errorMsg, returnObj);
        }

        private ReturnValue CreateMessage(ReturnValue retValue, bool hasError, string errorCode, string errorMsg, object returnObj)
        {
            var returnValue = retValue ?? new ReturnValue();
            returnValue.HasError = hasError;
            returnValue.ErrorCode = errorCode ?? "0";
            returnValue.Message = errorMsg;
            //retValue.ReturnObject = retValue.ReturnObject ?? returnObj;
            returnValue.ReturnObject = returnObj;
            return returnValue;
        }

        /// <summary>
        /// 获取当前分页的页数
        /// </summary>
        /// <param name="totleCount"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public int GetOffsetPageCount(int totleCount, int pageSize)
        {
            var pageCount = 0;
            if (totleCount % pageSize == 0)
            {
                pageCount = totleCount / pageSize;
            }
            else
            {
                pageCount = totleCount / pageSize + 1;
            }
            return pageCount;
        }

        /// <summary>
        /// 获取数组中的元素
        /// </summary>
        /// <param name="arrStrings">数组:1,安徽省</param>
        /// <param name="index">索引值</param>
        /// <param name="spChar"></param>
        /// <returns></returns>
        public static string GetArrayContent(string[] arrStrings, int index, char spChar = ',')
        {
            if (arrStrings.Length > index)
            {
                var value = arrStrings[index];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim(spChar);
                }
            }
            return string.Empty;
        }
    }
}