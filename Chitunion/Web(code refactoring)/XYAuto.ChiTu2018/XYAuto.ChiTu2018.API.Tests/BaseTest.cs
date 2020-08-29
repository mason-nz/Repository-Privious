using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ChiTu2018.Service.App;

namespace XYAuto.ChiTu2018.API.Tests
{
    /// <summary>
    /// 注释：BaseTest
    /// 作者：lix
    /// 日期：2018/6/7 11:52:28
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class BaseTest
    {
        public BaseTest()
        {
            Service.App.AutoMapperConfig.MapperConfig.Configure();
        
            EntityFrameworkProfiler.Initialize();

            OptionBootStarp boot = new OptionBootStarp(OptionBootStarpManage.GetAssemblies(LoadAssembType.Other));
            boot.Initialize();
        }
    }
}
