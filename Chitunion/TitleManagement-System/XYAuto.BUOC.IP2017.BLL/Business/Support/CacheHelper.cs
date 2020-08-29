using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace XYAuto.BUOC.IP2017.BLL.Business.Support
{
    /// <summary>
    /// auth:
    /// desc:轻量级缓存代理类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheHelper<T>
    {
        public static T Get(Cache cache,
                    Func<string> cacheKey,
                    Func<T> getData,
                    Func<object> setDependency, int cacheminute = 10)
        {
            object data = cache.Get(cacheKey());
            if (data == null)
            {
                data = getData();
                Set(cache, cacheKey, (T)data, setDependency, cacheminute);
            }
            return (T)data;
        }

        public static void Set(Cache cache,
                        Func<string> cacheKey,
                        Func<T> getData,
                        Func<object> setDependency,
                        int cacheminute)
        {
            Set(cache, cacheKey, getData(), setDependency, cacheminute);
        }

        public static void Set(Cache cache,
                        Func<string> cacheKey,
                        T data,
                        Func<object> setDependency,
                        int cacheminute)
        {
            object cacheDependency = setDependency == null ? null : setDependency();
            cache.Insert(cacheKey(), data, cacheDependency == null ? null : (CacheDependency)cacheDependency,
                         DateTime.Now.AddMinutes(cacheminute), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveAllCache(string cacheKey)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="likeOfName"></param>
        public static void RemoveCache(string likeOfName)
        {
            if (string.IsNullOrWhiteSpace(likeOfName)) return;
            likeOfName = likeOfName.ToLower();
            var keys = new List<string>();
            foreach (DictionaryEntry cache in HttpRuntime.Cache)
            {
                if (cache.Key.ToString().ToLower().IndexOf(likeOfName, StringComparison.Ordinal) > -1)
                {
                    keys.Add(cache.Key.ToString());
                }
            }
            keys.ForEach(key =>
            {
                HttpRuntime.Cache.Remove(key);
            });
        }

        /// <summary>
        /// 以列表形式返回已存在的所有缓存
        /// </summary>
        /// <returns></returns>
        public static List<string> ShowAllCache()
        {
            var allKeyList = new List<string>();
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            if (cache.Count <= 0) return allKeyList;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                allKeyList.Add(cacheEnum.Key.ToString());
            }
            return allKeyList;
        }
    }
}
