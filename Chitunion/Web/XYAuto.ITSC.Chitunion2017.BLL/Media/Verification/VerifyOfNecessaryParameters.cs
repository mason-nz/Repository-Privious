using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Enums;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification
{
    public class VerifyOfNecessaryParameters<T> where T : class, new()
    {
        /// <summary>
        /// 提交申请单验证的规则方法
        /// </summary>
        /// <param name="entity">需要验证的实体</param>
        /// <param name="funcDic">传入需要验证的规则</param>
        /// <param name="isOutParam">是否需要返回值</param>
        /// <returns></returns>
        public ReturnValue Verify(T entity, Dictionary<int, Func<T, ReturnValue>> funcDic, bool isOutParam = false)
        {
            //自定义规则
            return new Verifiction<T>(funcDic).Verify(entity, isOutParam);
        }
        
        /// <summary>
        /// 判断实体参数是否是必须传值
        /// </summary>
        /// <param name="enity"></param>
        /// <returns></returns>
        public ReturnValue VerifyNecessaryParameters(T enity)
        {
            var retValue = new ReturnValue();
            var paramDic = new DataAnnotationManage().DisplaySelfAttribute(enity);
            if (paramDic.Count == 0) return retValue;
            var sbBuilder = new StringBuilder();
            foreach (var item in paramDic)
            {
                sbBuilder.AppendFormat("{0},",item.Value);
            }
            retValue.HasError = true;
            retValue.Message = sbBuilder.ToString().Trim(',');
            return retValue;
        }
    }
}
