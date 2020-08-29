using System.Collections;
using System.Collections.Generic;
using System.Data;
using AutoMapper;

namespace XYAuto.ChiTu2018.Infrastructure.AutoMapper
{
    /// <summary>
    /// 注释：AutoMapper
    /// 作者：lix
    /// 日期：2018/5/15 15:33:04
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperHelper
    {
        /// <summary>
        ///  类型映射
        /// </summary>
        public static T MapTo<T>(this object obj)
        {
            Mapper.Reset();
            if (obj == null) return default(T);
            Mapper.Initialize(t => t.CreateMap(obj.GetType(), typeof(T)));
            return Mapper.Map<T>(obj);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            Mapper.Reset();
            foreach (var first in source)
            {
                var type = first.GetType();
                Mapper.Initialize(t => t.CreateMap(type, typeof(TDestination)));
                break;
            }
            return Mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 集合列表类型映射
        /// </summary>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            Mapper.Reset();
            //IEnumerable<T> 类型需要创建元素的映射
            Mapper.Initialize(t => t.CreateMap<TSource, TDestination>());
            return Mapper.Map<List<TDestination>>(source);
        }
        /// <summary>
        /// 类型映射
        /// </summary>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
                    where TSource : class
                    where TDestination : class
        {
            Mapper.Reset();
            if (source == null) return destination;
            Mapper.Initialize(t => t.CreateMap<TSource, TDestination>());
            return Mapper.Map(source, destination);
        }
        /// <summary>
        /// DataReader映射
        /// </summary>
        public static IEnumerable<T> DataReaderMapTo<T>(this IDataReader reader)
        {
            Mapper.Reset();
            Mapper.Initialize(t => t.CreateMap<IDataReader, IEnumerable<T>>());
            return Mapper.Map<IDataReader, IEnumerable<T>>(reader);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TDestination MapToMap<TDestination, TSource>(this TSource source)
            where TDestination : class
            where TSource : class
        {
            Mapper.Reset();
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }
    }
}
