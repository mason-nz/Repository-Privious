using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Exceptions
{
    /// <summary>
    /// 注释：KrPayCreateDisbursementException
    /// 作者：lix
    /// 日期：2018/6/11 10:08:59
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class KrPayCreateDisbursementException : Exception
    {
        public KrPayCreateDisbursementException(string message) : base(message)
        {
        }
    }
}
