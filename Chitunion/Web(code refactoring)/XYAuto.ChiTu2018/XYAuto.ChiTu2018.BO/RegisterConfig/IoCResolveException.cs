/**
*
*创建人：lixiong
*创建时间：2018/5/9 9:39:33
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.BO.RegisterConfig
{
    public class IoCResolveException : Exception
    {
        public IoCResolveException(string message) : base(message)
        {

        }
    }
}
