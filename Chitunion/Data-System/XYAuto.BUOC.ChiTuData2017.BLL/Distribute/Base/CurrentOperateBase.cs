using System;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.ITSC.Chitunion2017.Common;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Distribute.Base
{
    public class CurrentOperateBase
    {
        /// <summary>
        /// Auth:lixiong
        /// 查询参数校验
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

        /// <summary>
        /// 获取对应的实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public T GetEntity<T>(ReturnValue returnValue) where T : class, new()
        {
            return returnValue.ReturnObject as T;
        }

        protected virtual ReturnValue VerifyOfRoleModule(ReturnValue retValue, string moduleId)
        {
            if (!UserInfo.CheckRight(moduleId, "SYS001"))
            {
                retValue.HasError = true;
                retValue.ErrorCode = "50020";
                retValue.Message = "您没有权限操作";
                return retValue;
            }

            retValue.HasError = false;
            retValue.Message = String.Empty;
            return retValue;
        }

        /// <summary>
        /// 获取数组中的元素
        /// </summary>
        /// <param name="arrStrings">数组:1,安徽省</param>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        public static string GetAppContent(string[] arrStrings, int index)
        {
            if (arrStrings.Length > index)
            {
                var value = arrStrings[index];
                if (!String.IsNullOrWhiteSpace(value))
                {
                    return value.Trim(',');
                }
            }
            return string.Empty;
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
        /// 获取字符串长度。与string.Length不同的是，该方法将中文作 2 个字符计算。
        /// </summary>
        /// <param name="str">目标字符串</param>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str)) { return 0; }

            int l = str.Length;
            int realLen = l;

            #region 计算长度

            int clen = 0;//当前长度
            while (clen < l)
            {
                //每遇到一个中文，则将实际长度加一。
                if ((int)str[clen] > 128) { realLen++; }
                clen++;
            }

            #endregion 计算长度

            return realLen;
        }
    }
}