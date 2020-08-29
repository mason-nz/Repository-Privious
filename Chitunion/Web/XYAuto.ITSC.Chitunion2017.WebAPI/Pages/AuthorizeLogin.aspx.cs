using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Pages
{
    public partial class AuthorizeLogin : System.Web.UI.Page
    {
        private string KeyFromOtherSys = Utils.Config.ConfigurationUtil.GetAppSettingValue("KeyFromOtherSys");
        private string EmbedChiTu_DesStr = Utils.Config.ConfigurationUtil.GetAppSettingValue("EmbedChiTu_DesStr");
        private string ExitAddress = Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress");
        public string RetvalMsg = string.Empty;
        public string p
        {
            get
            {
                return HttpContext.Current.Request["p"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["p"]);
            }
        }
        public string sign
        {
            get
            {
                return HttpContext.Current.Request["sign"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["sign"]);
            }
        }
        public int appid
        {
            get
            {
                return HttpContext.Current.Request["appid"] == null ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["appid"]));
            }
        }
        public string accessToken
        {
            get
            {
                return HttpContext.Current.Request["accessToken"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["accessToken"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsCallback)
            {
                string username = string.Empty;
                RetvalMsg = LoginFromOtherSys(p, sign, appid, accessToken, out username);
                if (string.IsNullOrEmpty(RetvalMsg))
                {
                    Login(username, "111111");
                }
            }
        }

        public string LoginFromOtherSys(string p, string sign, int appid, string accessToken, out string username)
        {
            /*
             1.AccessToken有效性
             2.AccessToken中的APPID与参数APPID是否匹配
             3.MD5(APPID+AccessToken+p+EncyptStr)是否等于Sign
             4.验证通过后更新AuthorizeLogin_Token表字段：Status、ModifyTime
             5.获取经纪人相关信息，匹配赤兔帐号，进行登录
             */

            //Stream s = System.Web.HttpContext.Current.Request.InputStream;
            //byte[] b = new byte[s.Length];
            //s.Read(b, 0, (int)s.Length);
            //string reqParam = Encoding.UTF8.GetString(b);
            //BLL.Loger.Log4Net.Info($"[{nameof(LoginFromOtherSys)}]请求参数reqParam:{reqParam}");
            username = string.Empty;
            try
            {
                BLL.Loger.Log4Net.Info($"[{nameof(LoginFromOtherSys)}]请求参数20170704p:{p},sign:{sign},appid:{appid},accessToken:{accessToken}");
                username = string.Empty;
                if (!Chitunion2017.Common.AuthorizeLogin.Instance.Verification(appid, accessToken))
                {
                    BLL.Loger.Log4Net.Error($"[{nameof(LoginFromOtherSys)}]APPID + AccessToken验证信息过期或不存在,appid:{appid},accessToken:{accessToken}");
                    return "APPID + AccessToken验证信息过期或不存在";
                }

                p = p.Replace(" ", "+");
                BLL.Loger.Log4Net.Info($"[{nameof(LoginFromOtherSys)}]Replace p:{p}");
                string md5code = MD5Hash(appid + accessToken + p + EmbedChiTu_DesStr, Encoding.UTF8);//XYAuto.Utils.Security.DESEncryptor.MD5Hash(appid + accessToken + p + EmbedChiTu_DesStr,Encoding.UTF8);
                if (md5code != sign)
                {
                    BLL.Loger.Log4Net.Error($"[{nameof(LoginFromOtherSys)}]sign验证信息不匹配,md5code MD5前:{appid + accessToken + p + EmbedChiTu_DesStr}");
                    BLL.Loger.Log4Net.Error($"[{nameof(LoginFromOtherSys)}]sign验证信息不匹配,md5code MD5后:{md5code}");
                    return "sign验证信息不匹配";
                }

                Chitunion2017.Common.Entities.AuthorizeLogin model = new Chitunion2017.Common.Entities.AuthorizeLogin()
                {
                    APPID = appid,
                    MD5Code = accessToken
                };
                Chitunion2017.Common.AuthorizeLogin.Instance.Update(model);
                //string decryptStr = XYAuto.Utils.Security.DESEncryptor.Decrypt(p, EmbedChiTu_DesStr);
                string decryptStr = Decrypt(p, EmbedChiTu_DesStr);


                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptStr);
                string vmsg = string.Empty;
                if (!dict.ContainsKey("UserId"))
                    vmsg = "经纪人ID是必填项!\n";
                if (!dict.ContainsKey("EnterpriseName"))
                    vmsg = "企业名称是必填项!\n";
                if (!dict.ContainsKey("Contact"))
                    vmsg = "联系人是必填项!\n";
                if (!dict.ContainsKey("Mobile"))
                    vmsg = "手机号是必填项!\n";
                if (!dict.ContainsKey("businessLicence"))
                    vmsg = "营业执照图片地址是必填项!\n";

                if (!string.IsNullOrEmpty(vmsg))
                    return vmsg;
                /*
                 用户操作：
                 1.存在，登录
                 2.不存在，先生成用户，再登录
                 */
                int userid = -2;
                userid = Chitunion2017.Common.AuthorizeLogin.Instance.p_UserBroker_Insert(dict, out username);
                if (userid == -10)
                {
                    BLL.Loger.Log4Net.Info($"[{nameof(LoginFromOtherSys)}]赤兔用户:{username}，跳转到登录页面");
                    //赤兔用户，跳转到登录页面
                    //Response.Redirect("http://www.chitunion.com/Login.aspx");
                    string url = ExitAddress + "/Login.aspx";
                    Response.Write(url);
                    BLL.Loger.Log4Net.Info($"[{nameof(LoginFromOtherSys)}]赤兔用户:{username}，跳转到url{url}");
                    return url;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error($"[{nameof(LoginFromOtherSys)}]LoginFromOtherSys出错:{ex.Message}");
                return ex.Message;
            }
        }

        public void Login(string username, string pwd)
        {
            int ret = Chitunion2017.Common.UserInfo.Instance.Login(username, pwd, 29001);
            if (ret > 0)//登陆成功
            {
                Chitunion2017.Common.UserInfo.Instance.Passport(ret);
                string gourl = ExitAddress + "/OrderManager/wx_list.html";
                string content = string.Format("用户{1}(ID:{0})登录成功。", ret, username);
                ret = 1;//登陆成功
                Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Select, content);
                Response.Redirect(gourl);
            }
        }

        private DESCryptoServiceProvider CreateDESProvider(string encryptKey)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int length = encryptKey.Length;
            if (length < 8)
            {
                for (int i = 0; i < (8 - length); i++)
                {
                    encryptKey = encryptKey + "@";
                }
            }
            provider = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, provider.KeySize / 8))
            };
            byte[] destinationArray = new byte[provider.BlockSize / 8];
            byte[] DefaultIV = new byte[provider.BlockSize / 8];
            Array.Copy(DefaultIV, 0, destinationArray, 0, destinationArray.Length);
            provider.IV = destinationArray;
            return provider;
        }

        public string Decrypt(string encryptedText, string decryptKey)
        {
            /*  encryptedText = "ilcUCTRLx19j8ikx75v9aHNxgvCzXbt + JT7HMXmE0CB2QdWaWMF0lz1TvHx3fyWO6o7W7kPDEHybJMMcJhFisPbF4nVoSb7lEFjk6fB0kKwWv3tg5bEG + l5iHbtpT8t + 7v5jTVzQN8wdbWd77lpuomLy6uAY90LhzFQFbCLGIP74TG1ZWzIcgxE81wX7w + c0u1xnM7cDx8TFIEQG1P5iCnaCwjR7jZWiA + whRZ1LC0G5Di1eleIhAAEK6FY27 + QMrXCXh / tlmdEqGcTLLNQ61g1zp / pxZw / i / moCtW54xinvZF5z1Z2lrxfKtzo3vYAUT4WbRC1kJdv09UGFTkkAlQ ==";*/

            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }
            DESCryptoServiceProvider provider = CreateDESProvider(decryptKey);
            byte[] buffer = Convert.FromBase64String(encryptedText);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return new UTF8Encoding().GetString(stream.ToArray());
        }

        public string MD5Hash(string strText, Encoding encoding)
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] bytes = encoding.GetBytes(strText);
            byte[] buffer2 = md.ComputeHash(bytes);
            string str = null;
            for (int i = 0; i < buffer2.Length; i++)
            {
                string str2 = buffer2[i].ToString("x");
                if (str2.Length == 1)
                {
                    str2 = "0" + str2;
                }
                str = str + str2;
            }
            return str;
        }
    }
}