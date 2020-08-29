using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.BLL.StockBroker
{
    public class StockBroker
    {
        public readonly static StockBroker Instance = new StockBroker();
        private readonly string StockBrokerUrlLogin = ConfigurationManager.AppSettings["StockBrokerUrlLogin"];
        private readonly string StockBrokerUrlDealer = ConfigurationManager.AppSettings["StockBrokerUrlDealer"];
        private readonly string appkey = ConfigurationManager.AppSettings["StockBrokerKey"];
        private readonly string appsecret = ConfigurationManager.AppSettings["StockBrokerAppSecret"];
        #region 签名参数生成规则
        /*
            * signature签名参数生成规则：
            采用SHA1加密方式进行加密  
            1将参数对应的key和分配的密钥按首字母进行升序排列，           
            2然后取出对应的value进行拼接，
            3再将拼接后的字符串传入加密方法中得到对应的签名，
            4最后将签名带入URL对应的参数中
            5进行Base64编码
            6urlencode编码
            */
        #endregion
        #region 库存平台登录验证接口
        /// <summary>
        /// 库存平台登录验证接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="errorMsg">错误原因</param>
        /// <returns>不为null则调用成功</returns>
        public XYAuto.ITSC.Chitunion2017.Entities.StockBroker.LoginDto Login(string username, string password, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                string timestamp = Util.GenerateTimeStamp();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("username", username);
                dict.Add("password", password);
                dict.Add("appkey", appkey);
                dict.Add("timestamp", timestamp);
                dict.Add("appsecret", appsecret);

                Dictionary<string, string> dictSortedASC = dict.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                string signature = string.Empty;
                foreach (var item in dictSortedASC)
                {
                    signature += item.Value;
                }
                BLL.Loger.Log4Net.Info($"[{nameof(Login)}]原始签名:{signature}");
                signature = Util.SHA1Base64(signature);
                BLL.Loger.Log4Net.Info($"[{nameof(Login)}]SHA1BASE64签名:{signature}");
                signature = HttpUtility.UrlEncode(signature, Encoding.UTF8);
                BLL.Loger.Log4Net.Info($"[{nameof(Login)}]UrlEncode签名:{signature}");

                string httpUrl = string.Format(StockBrokerUrlLogin, appkey, signature, timestamp);
                //string postData = $"&username={username}&password={XYAuto.Utils.Security.DESEncryptor.MD5Hash(XYAuto.Utils.Security.DESEncryptor.MD5Hash(password, Encoding.UTF8), Encoding.UTF8)}";
                string postData = $"username={username}&password={password}";

                BLL.Loger.Log4Net.Info($"[{nameof(Login)}]httpUrl:{httpUrl}");
                var obj = Util.HttpWebRequestCreate<dynamic>(httpUrl, postData);
                errorMsg = obj.ErrorMessage;
                BLL.Loger.Log4Net.Info($"[{nameof(Login)}]ErrorMessage:{errorMsg}");
                XYAuto.ITSC.Chitunion2017.Entities.StockBroker.LoginDto resDto = null;
                if (obj.ErrorCode == 0)
                {
                    BLL.Loger.Log4Net.Info($"[库存平台登录验证接口]经纪人信息:{Newtonsoft.Json.JsonConvert.SerializeObject(obj)}");
                    resDto = new XYAuto.ITSC.Chitunion2017.Entities.StockBroker.LoginDto()
                    {
                        dealerID = obj.Data.dealerID,
                        dealerName = obj.Data.dealerName,
                        contacts = obj.Data.contacts,
                        contactNumber = obj.Data.contactNumber,
                        businessLicence = obj.Data.businessLicence,
                        status = obj.Data.status
                    };
                    //更新数据
                    Dal.StockBroker.StockBroker.Instance.p_UserBrokerLogin_Update(resDto, out errorMsg);
                }

                return resDto;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"[{nameof(Login)}]库存平台登录验证接口调用出错:{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 物理清除StockBroker表数据
        /// </summary>
        /// <param name="userid">赤兔系统用户ID</param>
        public int DeleteByUserID(int userid)
        {
            return Dal.StockBroker.StockBroker.Instance.DeleteByUserID(userid);
        }
        #endregion
        #region 经销商营业执照信息接口
        /// <summary>
        /// 经销商营业执照信息接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dealerID">经销商ID</param>
        /// <param name="errorMsg">错误原因</param>
        /// <returns>
        /// 返回值不为null则调用成功
        /// 返回值：营业执照图片地址
        /// </returns>
        public string DealerBusinessLicence(int dealerID, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                string timestamp = Util.GenerateTimeStamp();
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("dealerID", dealerID.ToString());
                dict.Add("appkey", appkey);
                dict.Add("timestamp", timestamp);
                dict.Add("appsecret", appsecret);

                Dictionary<string, string> dictSortedASC = dict.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                string signature = string.Empty;
                foreach (var item in dictSortedASC)
                {
                    signature += item.Value;
                }
                //signature = HttpUtility.UrlEncode(Util.ConvertToBase64(Util.SHA1(signature)), Encoding.UTF8);
                BLL.Loger.Log4Net.Error($"[{nameof(DealerBusinessLicence)}]原始签名:{signature}");
                signature = Util.SHA1Base64(signature);
                BLL.Loger.Log4Net.Error($"[{nameof(DealerBusinessLicence)}]SHA1BASE64签名:{signature}");
                signature = HttpUtility.UrlEncode(signature, Encoding.UTF8);
                BLL.Loger.Log4Net.Error($"[{nameof(DealerBusinessLicence)}]UrlEncode签名:{signature}");

                string httpUrl = string.Format(StockBrokerUrlDealer, appkey, signature, timestamp);
                string postData = $"dealerID={dealerID}";
                var obj = Util.HttpWebRequestCreate<dynamic>(httpUrl, postData);
                errorMsg = obj.ErrorMessage;
                if (obj.ErrorCode == 0)
                    return obj.Data;

                return null;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"[{nameof(DealerBusinessLicence)}]经销商营业执照信息接口调用出错:{ex.Message}");
                return null;
            }

        }
        #endregion
        #region 查询是否为库存平台用户
        /// <summary>
        /// 查询是否为库存平台用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public int isStockBrokerUser(string username)
        {
            return Dal.StockBroker.StockBroker.Instance.isStockBrokerUser(username);
        }
        #endregion        
    }

    public class Util
    {
        private static HttpWebResponse SendPostRequest(string queryjson, HttpWebRequest request)
        {
            //发送POST数据  
            if (!string.IsNullOrEmpty(queryjson))
            {
                byte[] data = Encoding.ASCII.GetBytes(queryjson.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }
        #region httpPost
        public static T HttpWebRequestCreate<T>(string httpUrl, string postData)
        {
            try
            {
                var request = WebRequest.Create(httpUrl) as HttpWebRequest;
                //request.ContentType = "form-data/x-www-from-urlencoded";
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postData.Length;
                //request.ContentLength = 0;
                request.Method = "POST";
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                var response = request.GetResponse() as HttpWebResponse;
                string str = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                    str = sr.ReadToEnd();
                }
                response.Close();
                request.Abort();
                var obj = JsonConvert.DeserializeObject<T>(str);
                return obj;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"[{nameof(HttpWebRequestCreate)}]抓取错误", ex);
                return Activator.CreateInstance<T>();
            }
        }
        #endregion
        #region 获取时间戳
        public static string GenerateTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmm");
        }
        #endregion
        #region SHA1加密
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <returns>返回40位UTF8 大写</returns>  
        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }

        public static string SHA1Base64(string content)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = Encoding.UTF8.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();


                return Convert.ToBase64String(bytes_out);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"[{nameof(SHA1)}]SHA1加密出错：{ex.Message}");
                return ex.Message;
            }
        }
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                //result = result.Replace("-", "");

                return bytes_out.ToString();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"[{nameof(SHA1)}]SHA1加密出错：{ex.Message}");
                return ex.Message;
            }
        }
        #endregion
        #region BASE64编码解码
        public static string ConvertToBase64(string content)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        }
        public string ConvertFromBase64(string content)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(content));
        }
        #endregion
    }
}
