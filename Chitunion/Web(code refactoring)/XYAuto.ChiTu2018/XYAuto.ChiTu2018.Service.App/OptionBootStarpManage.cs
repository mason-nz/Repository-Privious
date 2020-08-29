using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace XYAuto.ChiTu2018.Service.App
{
    /// <summary>
    /// 注释：OptionBootStarpManage
    /// 作者：lix
    /// 日期：2018/6/7 11:39:26
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class OptionBootStarpManage
    {
        /// <summary>
        /// 获取程序集名称
        /// </summary>
        /// <param name="loadAssembType">引用类型</param>
        /// <param name="assembName">程序集名称：XYAuto.ChiTu2018</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssemblies(LoadAssembType loadAssembType, string assembName = "XYAuto.ChiTu2018")
        {
            if (loadAssembType == LoadAssembType.ApiOrService)
            {
                return GetByApiOrService(assembName);
            }
            return GetByOther(assembName);
        }

        private static IEnumerable<Assembly> GetByApiOrService(string assembName)
        {
            return BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(n => n.FullName.StartsWith(assembName));
        }

        private static IEnumerable<Assembly> GetByOther(string assembName)
        {
            var binDirectory = String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath) ? AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.RelativeSearchPath;

            var assemblies1 = from file in Directory.GetFiles(binDirectory)
                              where Path.GetExtension(file) == ".dll"
                              select Assembly.LoadFrom(file);

            return assemblies1.Where(n => n.FullName.StartsWith(assembName));
        }

    }

    public enum LoadAssembType
    {
        [Description("api/service")]
        ApiOrService,
        [Description("other")]
        Other
    }
}
