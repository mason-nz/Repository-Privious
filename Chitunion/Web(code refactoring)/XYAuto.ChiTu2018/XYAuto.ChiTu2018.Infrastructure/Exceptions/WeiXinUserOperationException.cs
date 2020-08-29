using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Infrastructure.Exceptions
{
    /// <summary>
    /// 注释：WeiXinUserOperationException
    /// 作者：lix
    /// 日期：2018/6/8 14:26:42
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class WeiXinUserOperationException : Exception
    {
        public WeiXinUserOperationException(string message) : base(message)
        {

        }
    }
}
