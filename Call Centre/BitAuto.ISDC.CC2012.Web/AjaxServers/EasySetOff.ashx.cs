using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.WebService.EasyPass;
using BitAuto.Utils.Config;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// EasySetOff 的摘要说明
    /// </summary>
    public class EasySetOff : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;
        private static string Key = ConfigurationUtil.GetAppSettingValue("EasySetOff_GenURLParaMD5");//"@#$%^&*(";
        private string RequestGoToEPURL
        {
            get { return currentContext.Request["GoToEPURL"] == null ? string.Empty : currentContext.Request["GoToEPURL"].Trim(); }
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            int userid = BLL.Util.GetLoginUserID();
            context.Response.ContentType = "text/plain";
            currentContext = context;
            string msg = string.Empty;

            try
            {
                string url = ConfigurationUtil.GetAppSettingValue("EasySetOff_URL");//"http://testcc.chedai.bitauto.com/Account/LogOn";
                if (!string.IsNullOrEmpty(RequestGoToEPURL))
                {
                    url = RequestGoToEPURL;
                }
                // 加密字符串
                string enString = GetMd5Hash("u="+userid.ToString()+ ",key="+ Key);
                msg = string.Format("{0}?u={1}&sign={2}", url, userid.ToString(), enString);
                msg = HttpUtility.UrlEncode(msg);

            }
            catch (Exception ex)
            {
                msg = "Error";
            }

            context.Response.Write(msg);
        }

        /// <summary>
        /// 获取MD5加密字符串
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns>加密结果</returns>
        private string GetMd5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}