using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.VerifyArgs
{
    /// <summary>
    /// 注释：VerifyOfNecessaryParameters
    /// 作者：lix
    /// 日期：2018/5/15 15:39:39
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class VerifyOfNecessaryParameters<T> where T : class, new()
    {
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
                sbBuilder.AppendFormat("{0},", item.Value);
            }
            retValue.HasError = true;
            retValue.Message = sbBuilder.ToString().Trim(',');
            return retValue;
        }
    }
}
