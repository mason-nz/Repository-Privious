using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.AppAuth
{
    public class AppAuthHelper
    {
        /// <summary>
        /// 赤兔APP，安卓端调用后端api接口密钥
        /// </summary>
        private string ChiTuApp_API_AppIDSecret = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ChiTuApp_API_AppIDSecret", false);
        /// <summary>
        /// 当前配置签名列表
        /// </summary>
        private List<AppAuth> appAuthList;

        #region
        public static readonly AppAuthHelper Instance = new AppAuthHelper();
        #endregion

        public AppAuthHelper()
        {
            try
            {
                appAuthList = JsonConvert.DeserializeObject<List<AppAuth>>(ChiTuApp_API_AppIDSecret);
            }
            catch (Exception ex)
            {
                appAuthList = null;
                BLL.Loger.Log4Net.Error("解析Json配置[ChiTuApp_API_AppIDSecret]出错", ex);
            }
        }

        /// <summary>
        /// 验证配置中的AppID和secret的匹配逻辑是否一致
        /// </summary>
        /// <param name="appID"></param>
        /// <returns>若一致，则返回AppAuth对象，否则返回null</returns>
        private AppAuth VerifyAppID(string appID)
        {
            var result = appAuthList.FindAll(s => s.Appid == appID && s.Status == 0);
            if (result != null && result.Count == 1)
            {
                return result[0];
            }
            return null;
        }


        /// <summary>
        /// 请求参数验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool VerifyPara(Dictionary<string, object> list, ref EnumAppAuthRequestVerify code)
        {
            bool restlt = false;
            if (list == null)
            {
                //msg = "参数错误";
                //code = "-203";
                code = EnumAppAuthRequestVerify.参数错误;
                return restlt;
            }
            //System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (list.Count <= 0)
            {
                code = EnumAppAuthRequestVerify.参数错误;
                return restlt;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string name = list.Keys.ElementAt(i);
                object value = list[name];

                //if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                //{
                if (value == null)
                {
                    code = EnumAppAuthRequestVerify.参数不能为空;
                    return restlt;
                }
                if (name == "version")
                {
                    if (value.ToString() != "1.0")
                    {
                        code = EnumAppAuthRequestVerify.参数错误;
                        return restlt;
                    }
                }
                else if (name == "sign" && string.IsNullOrEmpty(value.ToString()))
                {
                    code = EnumAppAuthRequestVerify.参数错误;
                    return restlt;
                }
                //}
            }
            restlt = true;
            return restlt;

        }


        /// <summary>
        /// 签名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool VerifySign(Dictionary<string, object> list, ref EnumAppAuthRequestVerify code)
        {
            try
            {
                //bool flag = false;
                StringBuilder values = new StringBuilder();
                string appid = string.Empty;
                string originSign = string.Empty;
                long originTimestamp = 0;//签名中的时间戳
                Dictionary<string, string> ret = new Dictionary<string, string>();
                if (list == null)
                {
                    code = EnumAppAuthRequestVerify.参数错误;
                    return false;
                }
                //System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                if (list.Count <= 0)
                {
                    code = EnumAppAuthRequestVerify.参数错误;
                    return false;
                }
                appid = list["appid"].ToString();
                AppAuth aa = VerifyAppID(appid);
                if (aa == null)
                {
                    code = EnumAppAuthRequestVerify.AppID无效;
                    return false;
                }
                originSign = DESHelper.ToDESDecrypt(list["sign"].ToString(), aa.Secret);
                originTimestamp = long.Parse(originSign.ToLower().Substring(originSign.ToLower().IndexOf("timestamp") + 9, 13));

                for (int i = 0; i < list.Count; i++)
                {
                    string name = list.Keys.ElementAt(i);
                    object value = list[name];
                    if (value == null)
                    {
                        continue;
                    }

                    //if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    //{
                    if (name == "sign")
                    {
                        continue;
                    }
                    else if (name == "timestamp")
                    {
                        long currentTimestamp = long.Parse(value.ToString());
                        DateTime? dt = BLL.Util.ConvertStringToDateTime(currentTimestamp.ToString());
                        if (currentTimestamp == originTimestamp && dt != null)
                        {
                            TimeSpan ts = DateTime.Now - dt.Value;
                            if (Math.Abs(ts.TotalSeconds) > aa.VerifyTimeOut)
                            {
                                code = EnumAppAuthRequestVerify.请求过期;
                                return false;
                            }
                            ret.Add(name, value.ToString());
                        }
                        //if (dt == null)
                        //{
                        //    code = EnumAppAuthRequestVerify.参数错误;
                        //    return false;
                        //}
                        continue;
                    }
                    else
                    {
                        if (name == "appid")
                        {
                            appid = value.ToString();
                        }
                        if (value.GetType() == typeof(bool))
                        {
                            ret.Add(name, value.ToString().ToLower());
                        }
                        else
                        {
                            ret.Add(name, value.ToString());
                        }
                    }
                    //}
                }
                //var DicOrder = ret.OrderBy(o => o.Key);
                var DicOrder = ret.OrderBy(x => x.Key, new OrdinalComparer()).ToDictionary(x => x.Key, y => y.Value);
                foreach (var item in DicOrder)
                {
                    values.Append(item.Key + item.Value);
                }
                //string sign = GetMd5Hash(values.ToString() + appid);
                string sign = values.ToString() + appid;
                if (sign != originSign)
                {
                    code = EnumAppAuthRequestVerify.签名错误;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("验证APP调用后端API接口签名异常", ex);
                code = EnumAppAuthRequestVerify.签名错误;
                return false;
            }

        }


        ///// <summary>
        ///// 请求参数验证
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="t"></param>
        ///// <param name="code"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public bool VerifyPara<T>(T t, ref EnumAppAuthRequestVerify code)
        //{
        //    bool restlt = false;
        //    if (t == null)
        //    {
        //        //msg = "参数错误";
        //        //code = "-203";
        //        code = EnumAppAuthRequestVerify.参数错误;
        //        return restlt;
        //    }
        //    System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        //    if (properties.Length <= 0)
        //    {
        //        code = EnumAppAuthRequestVerify.参数错误;
        //        return restlt;
        //    }
        //    foreach (System.Reflection.PropertyInfo item in properties)
        //    {
        //        string name = item.Name;
        //        if (item.GetValue(t, null) == null)
        //        {
        //            continue;
        //        }
        //        string value = item.GetValue(t, null).ToString();

        //        if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
        //        {
        //            if (value == null || value.Length == 0)
        //            {
        //                code = EnumAppAuthRequestVerify.参数不能为空;
        //                return restlt;
        //            }
        //            if (name == "version")
        //            {
        //                if (value != "1.0")
        //                {
        //                    code = EnumAppAuthRequestVerify.参数错误;
        //                    return restlt;
        //                }
        //            }
        //            else if (name == "sign" && string.IsNullOrEmpty(value))
        //            {
        //                code = EnumAppAuthRequestVerify.参数错误;
        //                return restlt;
        //            }
        //        }
        //    }
        //    restlt = true;
        //    return restlt;

        //}


        //public string GetSign<T>(T t, ref EnumAppAuthRequestVerify code)
        //{
        //    StringBuilder values = new StringBuilder();
        //    string appid = string.Empty;
        //    string originSign = string.Empty;
        //    Dictionary<string, string> ret = new Dictionary<string, string>();
        //    if (t == null)
        //    {
        //        code = EnumAppAuthRequestVerify.参数错误;
        //        return string.Empty;
        //    }
        //    System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        //    if (properties.Length <= 0)
        //    {
        //        code = EnumAppAuthRequestVerify.参数错误;
        //        return string.Empty;
        //    }
        //    foreach (System.Reflection.PropertyInfo item in properties)
        //    {
        //        string name = item.Name;
        //        string value = string.Empty;
        //        if (item.GetValue(t, null) == null)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            value = item.GetValue(t, null).ToString();
        //        }

        //        if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
        //        {
        //            if (name == "sign")
        //            {
        //                originSign = value;
        //                continue;
        //            }
        //            else
        //            {
        //                if (name == "appid")
        //                {
        //                    appid = value;
        //                }
        //                ret.Add(name, value);
        //            }


        //        }
        //    }
        //    var DicOrder = ret.OrderBy(o => o.Key);
        //    foreach (var item in DicOrder)
        //    {
        //        values.Append(item.Key + item.Value);
        //    }
        //    //return GetMd5Hash(values.ToString() + appid);

        //    return XYAuto.Utils.Security.DESEncryptor.Encrypt(values.ToString() + appid, ChiTuApp_APIMiYao);
        //}


        public string GetSign(Dictionary<string, object> list, ref EnumAppAuthRequestVerify code)
        {
            StringBuilder values = new StringBuilder();
            string appid = string.Empty;
            string originSign = string.Empty;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (list == null)
            {
                code = EnumAppAuthRequestVerify.参数错误;
                return string.Empty;
            }
            //System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (list.Count <= 0)
            {
                code = EnumAppAuthRequestVerify.参数错误;
                return string.Empty;
            }
            appid = list["appid"].ToString();
            AppAuth aa = VerifyAppID(appid);
            if (aa == null)
            {
                code = EnumAppAuthRequestVerify.AppID无效;
                return string.Empty;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string name = list.Keys.ElementAt(i);
                object value = list[name];
                if (value == null)
                {
                    continue;
                }
                //else
                //{
                //    value = item.GetValue(t, null).ToString();
                //}

                //if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                //{
                if (name == "sign")
                {
                    originSign = value.ToString();
                    continue;
                }
                else
                {
                    if (name == "appid")
                    {
                        appid = value.ToString();
                    }
                    ret.Add(name, value.ToString());
                }


                //}
            }
            var DicOrder = ret.OrderBy(o => o.Key);
            foreach (var item in DicOrder)
            {
                values.Append(item.Key + item.Value);
            }
            //return GetMd5Hash(values.ToString() + appid);

            //return XYAuto.Utils.Security.DESEncryptor.Encrypt(values.ToString() + appid, aa.Secret);
            return DESHelper.ToDESEncrypt(values.ToString() + appid, aa.Secret);
        }


        ///// <summary>
        ///// 签名
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public bool VerifySign<T>(T t, ref EnumAppAuthRequestVerify code)
        //{
        //    //bool flag = false;
        //    StringBuilder values = new StringBuilder();
        //    string appid = string.Empty;
        //    string originSign = string.Empty;
        //    Dictionary<string, string> ret = new Dictionary<string, string>();
        //    if (t == null)
        //    {
        //        code = EnumAppAuthRequestVerify.参数错误;
        //        return false;
        //    }
        //    System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        //    if (properties.Length <= 0)
        //    {
        //        code = EnumAppAuthRequestVerify.参数错误;
        //        return false;
        //    }
        //    foreach (System.Reflection.PropertyInfo item in properties)
        //    {
        //        string name = item.Name;
        //        string value = string.Empty;
        //        if (item.GetValue(t, null) == null)
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            value = item.GetValue(t, null).ToString();
        //        }

        //        if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
        //        {
        //            //if (name == "page_index" && Convert.ToInt32(value) > 0)
        //            //{
        //            //    ret.Add(name, value);
        //            //}
        //            //else if (name == "page_index" && Convert.ToInt32(value) == 0)
        //            //{
        //            //    continue;
        //            //}
        //            //else if (name == "appkey")
        //            //{
        //            //    ret.Add(name, value);
        //            //}
        //            //else if (name == "tasktype" && Convert.ToInt32(value) > 0)
        //            //{
        //            //    ret.Add(name, value);
        //            //}
        //            //else if (name == "tasktype" && Convert.ToInt32(value) == 0)
        //            //{
        //            //    continue;
        //            //}
        //            if (name == "sign")
        //            {
        //                originSign = value;
        //                continue;
        //            }
        //            //else if (name == "appId")
        //            //{
        //            //    appid = Convert.ToInt32(value);
        //            //    ret.Add(name, value);
        //            //}
        //            else
        //            {
        //                if (name == "appid")
        //                {
        //                    appid = value;
        //                }
        //                ret.Add(name, value);
        //            }


        //        }
        //    }
        //    var DicOrder = ret.OrderBy(o => o.Key);
        //    foreach (var item in DicOrder)
        //    {
        //        values.Append(item.Key + item.Value);
        //    }
        //    //string sign = GetMd5Hash(values.ToString() + appid);
        //    string sign = XYAuto.Utils.Security.DESEncryptor.Decrypt(values.ToString() + appid, ChiTuApp_APIMiYao);
        //    if (sign != originSign)
        //    {
        //        code = EnumAppAuthRequestVerify.签名错误;
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        ///// <summary>
        ///// md5加密
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //private static string GetMd5Hash(string input)
        //{
        //    MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        //    byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
        //    StringBuilder sBuilder = new StringBuilder();
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("x2"));
        //    }
        //    return sBuilder.ToString();
        //}
    }
}
