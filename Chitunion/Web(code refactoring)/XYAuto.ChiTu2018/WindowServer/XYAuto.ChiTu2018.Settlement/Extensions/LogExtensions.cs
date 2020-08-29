using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Settlement.Extensions
{
    /// <summary>
    /// 注释：LogExtensions
    /// 作者：lix
    /// 日期：2018/5/23 9:44:41
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public static class LogExtensions
    {
        public static void LogInfo(this string message)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(message);
        }

        public static void LogError(this string message)
        {
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Error(message);
        }
    }
}
