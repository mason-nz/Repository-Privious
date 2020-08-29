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
    /// CarFinancial 的摘要说明
    /// </summary>
    public class CarFinancial : IHttpHandler, IRequiresSessionState
    {

        private static string Key = ConfigurationUtil.GetAppSettingValue("CarFinancial_GenURLParaMD5");//"@#$%^&*(";
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            int userid = BLL.Util.GetLoginUserID();
            //userid = 167697; //测试账号
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            try
            {
                string url = ConfigurationUtil.GetAppSettingValue("CarFinancial_URL");//"http://testcc.chedai.bitauto.com/Account/LogOn";
                // 加密字符串
                string enString = Encrypt(userid.ToString(), Key);
                msg = string.Format("{0}?u={1}&sign={2}", url, userid.ToString(), enString);
                msg = HttpUtility.UrlEncode(msg);

            }
            catch (Exception ex)
            {
                msg = "Error";
            }

            context.Response.Write(msg);
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