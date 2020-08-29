using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Profile;

namespace XYAuto.ITSC.Chitunion2017.Test.AutoMapper
{
    /// <summary>
    /// autoMapper 入口，配置多个Profile
    /// </summary>
    internal class Configuration
    {
        public static void Configure()
        {
            //初始化Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<UserInfoProfile>();
                cfg.AddProfile<MediaWeiXinProfile>();
                cfg.AddProfile<SourceProfile>();
            });
        }
    }


    /// <summary>
    /// 多对一处理
    /// </summary>
    public static class EntityMapper
    {
        public static T Map<T>(params object[] sources) where T : class
        {
            if (!sources.Any())
            {
                return default(T);
            }

            var initialSource = sources[0];
            var mappingResult = Map<T>(initialSource);

            // Now map the remaining source objects
            if (sources.Count() > 1)
            {
                Map(mappingResult, sources.Skip(1).ToArray());
            }
            return mappingResult;
        }

        private static void Map(object destination, params object[] sources)
        {
            if (!sources.Any())
            {
                return;
            }
            var destinationType = destination.GetType();
            foreach (var source in sources)
            {
                var sourceType = source.GetType();
                Mapper.Map(source, destination, sourceType, destinationType);
            }
        }

        private static T Map<T>(object source) where T : class
        {
            var destinationType = typeof(T);
            var sourceType = source.GetType();
            var mappingResult = Mapper.Map(source, sourceType, destinationType);
            return mappingResult as T;
        }
    }
}
