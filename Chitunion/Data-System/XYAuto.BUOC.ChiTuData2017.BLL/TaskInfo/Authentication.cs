/********************************************************
*创建人：hant
*创建时间：2017/12/18 14:28:22 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo
{
    public class Authentication
    {
        #region
        public static readonly Authentication Instance = new Authentication();
        #endregion


        /// <summary>
        /// 请求参数验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ParaValid<T>(T t,ref string code,ref string msg)
        {
            bool restlt = false;
            if (t == null)
            {
                msg = "参数错误";
                code = "-203";
                return restlt;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                msg = "参数错误";
                code = "-203";
                return restlt;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                if (item.GetValue(t, null) == null)
                {
                    continue;
                }
                string value = item.GetValue(t, null).ToString();

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value == null || value.Length == 0)
                    {
                        msg = "参数不能为空";
                        code = "-204";
                        return restlt;
                    }
                    if (name =="version")
                    {
                        if (value != "1.0")
                        {
                            msg = "版本错误";
                            code = "-204";
                            return restlt;
                        }
                    }
                }
            }
            restlt = true;
            return restlt;

        }

        /// <summary>
        /// 验证访问权限
        /// </summary>
        /// <param name="appid">应用程序ID</param>
        /// <param name="appkey">应用程序秘钥</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public bool Access(int appid, string appkey, ref string msg,ref string code)
        {
            bool result = false;
            Entities.Task.AppInfo entity = BLL.TaskInfo.AppInfo.Instance.GeAppInfo(appid, appkey);
            if (entity != null)
            {
                if (entity.ValidDate > DateTime.Now)
                {
                    result = true;
                }
                else
                {
                    msg = "授权码过期";
                    code = "-302";
                }
            }
            else
            {
                msg = "授权失败";
                code = "-301";
            }
            return result;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public  string GetSign<T>(T t)
        {
            StringBuilder values = new StringBuilder();
            int appid = 0;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                string value = string.Empty;
                if (item.GetValue(t, null) == null)
                {
                    continue;
                }
                else
                {
                    value = item.GetValue(t, null).ToString();
                }

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (name == "page_index" && Convert.ToInt32(value) > 0)
                    {
                        ret.Add(name, value);
                    }
                    else if (name == "page_index" && Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    else if (name == "appkey")
                    {
                        ret.Add(name, value);
                    }
                    else if (name == "tasktype" && Convert.ToInt32(value) > 0)
                    {
                        ret.Add(name, value);
                    }
                    else if (name == "tasktype" && Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    else if (name == "sign")
                    {
                        continue;
                    }
                    else if (name == "appId")
                    {
                        appid = Convert.ToInt32(value);
                        ret.Add(name, value);
                    }
                    else
                    {
                        ret.Add(name, value);
                    }


                }
            }
            var DicOrder = ret.OrderBy(o => o.Key);
            foreach (var item in DicOrder)
            {
                values.Append(item.Key + item.Value);
            }
            return GetMd5Hash(values.ToString() + GetAppKeyByAppId(appid));

        }

        /// <summary>
        /// 单元测试用-签名逻辑
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appkey"></param>
        /// <returns></returns>
        public string GetSign<T>(T t,string appkey)
        {
            StringBuilder values = new StringBuilder();
            int appid = 0;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                string value = string.Empty;
                if (item.GetValue(t, null) == null)
                {
                    continue;
                }
                else
                {
                    value = item.GetValue(t, null).ToString();
                }

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (name == "page_index" && Convert.ToInt32(value) > 0)
                    {
                        ret.Add(name, value);
                    }
                    else if (name == "page_index" && Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    else if (name == "appkey")
                    {
                        ret.Add(name, value);
                    }
                    else if (name == "tasktype" && Convert.ToInt32(value) > 0)
                    {
                        ret.Add(name, value);
                    }
                    else if (name == "tasktype" && Convert.ToInt32(value) == 0)
                    {
                        continue;
                    }
                    else if (name == "sign")
                    {
                        continue;
                    }
                    else if (name == "appId")
                    {
                        appid = Convert.ToInt32(value);
                        ret.Add(name, value);
                    }
                    else
                    {
                        ret.Add(name, value);
                    }


                }
            }
            var DicOrder = ret.OrderBy(o => o.Key);
            foreach (var item in DicOrder)
            {
                values.Append(item.Key + item.Value);
            }
            return GetMd5Hash(values.ToString() + appkey);

        }

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string GetMd5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public bool SignValid(string original, string md5hash, ref string code, ref string msg)
        {
            bool restlt = false;
            if (original != md5hash)
            {
                msg = "签名错误";
                code = "-102";
            }
            else
            {
                restlt = true;
            }
            return restlt;
        }

        public bool CallNumber(int appid, string appkey, ref string msg, ref string code)
        {
            bool result = false;
            int callnumber = Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CallNumber"));
            if (callnumber == -1)
            {
                return true;
            }
            int time = DateTime.Now.Hour;
            string mediakey = appkey + time.ToString();
            Entities.Task.MediaChannel media =(Entities.Task.MediaChannel)XYAuto.Utils.Caching.SingletonCache.Cache.Get(mediakey);
            if (media == null)
            {
                Entities.Task.MediaChannel mediaadd = new Entities.Task.MediaChannel();
                mediaadd.AppKey = mediakey;
                mediaadd.Number = 1;
                XYAuto.Utils.Caching.SingletonCache.Cache.Remove(appkey + (time - 1).ToString());
                XYAuto.Utils.Caching.SingletonCache.Cache.Add(mediakey, mediaadd, null, DateTime.Now.AddHours(1), System.TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
                code = "1";
                msg = "success";
                result = true;
            }
            else
            {
                if (media.Number > callnumber)
                {
                    code = "-401";
                    msg = "接口调用频次超限";
                }
                else
                {
                    media.Number += 1;
                    XYAuto.Utils.Caching.SingletonCache.Cache.Remove(mediakey);
                    XYAuto.Utils.Caching.SingletonCache.Cache.Insert(mediakey, media);
                   code = "1";
                    msg = "success";
                    result = true;
                }

            }
            return result;
        }


        public string GetAppKeyByAppId(int appid)
        {
          return  BLL.TaskInfo.AppInfo.Instance.GetAppKeyByChannelID(appid);
        }
    }
}
