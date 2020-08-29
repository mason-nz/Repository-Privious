using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.Entities;

namespace XYAuto.ChiTu2018.Service
{
    /// <summary>
    /// 注释：OptionBootStarp 系统初始化
    /// 作者：lix
    /// 日期：2018/6/6 17:47:22
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class OptionBootStarp
    {
        protected IEnumerable<Assembly> Assembles { get; }
        protected UnityContainer Ioc { get; }
        public OptionBootStarp(IEnumerable<Assembly> ass)
        {
            Assembles = ass;
            Ioc = IocMannager.UnityContainer;
        }

        protected IEnumerable<Type> Repository => Assembles.SelectMany(a => a.ExportedTypes.Where(t => t.GetInterfaces().Contains(typeof(IBaseRepository))));

        /// <summary>
        /// 预加载
        /// </summary>
        public void Initialize()
        {
            Dictionary<string, Type> dictInterface = new Dictionary<string, Type>();
            Dictionary<string, Type> dictRepImpl = new Dictionary<string, Type>();
            //加载所有服务
            Repository.ToList().ForEach(s =>
            {
                if (s.IsClass)
                {
                    //var stype = Activator.CreateInstance(s);
                    if (!dictRepImpl.ContainsKey(s.FullName))
                    {
                        dictRepImpl.Add(s.FullName, s);
                    }
                }
                else if (s.IsInterface)
                {
                    if (!dictInterface.ContainsKey(s.FullName))
                    {
                        dictInterface.Add(s.FullName, s);
                    }
                }
            });
            //根据注册的接口和接口实现集合，使用IOC容器进行注册
            foreach (var key in dictInterface.Keys)
            {
                Type interfaceType = dictInterface[key];
                foreach (string dalKey in dictRepImpl.Keys)
                {
                    Type dalType = dictRepImpl[dalKey];
                    if (interfaceType.IsAssignableFrom(dalType))//判断impl是否实现了某接口
                    {
                        Ioc.RegisterType(interfaceType, dalType);
                    }
                }
            }
        }
    }
}
