using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XYAuto.ChiTu2018.Infrastructure.MobileOperate
{
    /// <summary>
    /// 注释：MobileUtils
    /// 作者：zhanglb
    /// 日期：2018/5/16 17:33:36
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class MobileUtils
    {
        /// <summary>
        /// 根据手机号码，发送指定内容
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="content"></param>
        public static void AddWebCacheByMobile(string mobile, string content)
        {
            try
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"AddWebCacheByMobile begin 手机号:{mobile}，content:{content}");
                var c = HttpRuntime.Cache;
                var dt = DateTime.Now.AddMinutes(5);
                var dict = (Dictionary<string, object>)c.Get(mobile);
                if (dict == null)
                {
                    dict = new Dictionary<string, object>
                    {
                        {"StartTime", DateTime.Now},
                        {"EndTime", dt},
                        {"SmsContent", content},
                        {"Time", 1}
                    };
                }
                else
                {
                    var ts = DateTime.Parse(dict["EndTime"].ToString()) - DateTime.Now;
                    dt = DateTime.Parse(dict["EndTime"].ToString()).AddSeconds(-ts.TotalSeconds);
                    dict["StartTime"] = dt;
                    dict["Time"] = ((int)dict["Time"]) + 1;
                }
                c.Insert(mobile, dict, null, (DateTime)dict["EndTime"], System.Web.Caching.Cache.NoSlidingExpiration);

                XYAuto.CTUtils.Log.Log4NetHelper.Default($"AddWebCacheByMobile end 手机号:{mobile},get:{c.Get(mobile)}");
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"AddWebCacheByMobile出错 手机号:{mobile}");
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"AddWebCacheByMobile出错:{ex.Message},StackTrace:{ex.StackTrace}");
            }
        }

        public static string GetMobileCheckCodeByCache(string mobile)
        {
            try
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetMobileCheckCodeByCache begin mobile:{mobile}");
                var c = HttpRuntime.Cache;
                var dict = (Dictionary<string, object>)c.Get(mobile);
                if (dict == null)
                {
                    return null;
                }
                var times = (int)dict["Time"];
                string content = dict["SmsContent"].ToString();
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetMobileCheckCodeByCache end code:{content},Times={times}");
                return content;
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetMobileCheckCodeByCache出错 手机号:{mobile}");
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetMobileCheckCodeByCache出错:{ex.Message},StackTrace:{ex.StackTrace}");
                return null;
            }
        }

        public static int GetMobileCheckCodeTimesByCache(string mobile)
        {
            try
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetMobileCheckCodeByCache begin mobile:{mobile}");
                var c = HttpRuntime.Cache;
                var dict = (Dictionary<string, object>)c.Get(mobile);
                if (dict == null)
                {
                    return 0;
                }
                var times = (int)dict["Time"];
                string content = dict["SmsContent"].ToString();
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetMobileCheckCodeByCache end code:{content},Times={times}");
                return times;
            }
            catch (Exception ex)
            {
                //XYAuto.CTUtils.Log.Log4NetHelper.($"GetMobileCheckCodeByCache出错 手机号:{mobile}", ex);
                return -1;
            }
        }


        public static string GetCodeByCache(string cacheKey)
        {
            try
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetCodeByCache begin cacheKey:{cacheKey}");
                var c = HttpRuntime.Cache;
                string code = (string)c.Get(cacheKey);
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetCodeByCache end code:{code}");
                return code;
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetCodeByCache出错 key:{cacheKey}");
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"GetCodeByCache出错:{ex.Message},StackTrace:{ex.StackTrace ?? string.Empty}");
                return null;
            }
        }


        public static void InsertCache(string cacheKey, string content, int minutes)
        {
            try
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"InsertCache begin cacheKey:{cacheKey}，content:{content}");
                var c = HttpRuntime.Cache;
                c.Insert(cacheKey, content, null, DateTime.Now.AddMinutes(minutes), TimeSpan.Zero);
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"InsertCache end cacheKey:{cacheKey},get:{(string)c.Get(cacheKey)}");
            }
            catch (Exception ex)
            {
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"InsertCache出错 cacheKey:{cacheKey}");
                XYAuto.CTUtils.Log.Log4NetHelper.Default($"InsertCache出错:{ex.Message},StackTrace:{ex.StackTrace ?? string.Empty}");
            }
        }
    }
}
