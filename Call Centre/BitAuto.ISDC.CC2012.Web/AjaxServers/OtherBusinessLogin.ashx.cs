using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.Utils.Config;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.EasyPass;
using System.Security.Cryptography;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// OtherBusinessLogin 的摘要说明
    /// </summary>
    public class OtherBusinessLogin : IHttpHandler, IRequiresSessionState
    {
        private static string YCCDKey = ConfigurationUtil.GetAppSettingValue("CarFinancial_GenURLParaMD5");//"@#$%^&*(";易车车贷MD5验证码
        private static string JZGGKey = ConfigurationUtil.GetAppSettingValue("EasySetOff_GenURLParaMD5");//"@#$%^&*(";
        private static string YICHEHUILoginURL = ConfigurationUtil.GetAppSettingValue("YICHEHUILoginURL");//易车惠业务_授权URL
        private static string YICHEHUILoginKey = ConfigurationUtil.GetAppSettingValue("YICHEHUILoginKey");//易车惠业务_授权key
        private static string EasypassLoginURL = ConfigurationUtil.GetAppSettingValue("EasypassLoginURL");//易湃业务_授权URL
        private static string EasypassLoginKey = ConfigurationUtil.GetAppSettingValue("EasypassLoginKey");//易湃业务_授权key
        protected static string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");

        /// <summary>
        /// 惠买车登录授权URL
        /// </summary>
        private string RequestYPFanXianURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("YPFanXianURL"); }
        }
        /// <summary>
        /// 惠买车业务后台默认URL
        /// </summary>
        private string RequestHuimaicheTaskURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("TaskURL"); }
        }
        /// <summary>
        /// 惠买车APPID
        /// </summary>
        private string RequestEPEmbedCC_APPID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("EPEmbedCC_APPID"); }
        }
        /// <summary>
        /// 登录业务具体类型，如（huimaiche，yichechedai等）
        /// </summary>
        private string RequestBusinessType
        {
            get { return BLL.Util.GetCurrentRequestFormStr("businessType"); }
        }

        /// <summary>
        /// 业务ID
        /// </summary>
        private string RequestBusinessID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("businessID"); }
        }

        /// <summary>
        /// 精准广告业务详情URL
        /// </summary>
        private string RequestGoToEPURL
        {
            get { return BLL.Util.GetCurrentRequestFormStr("GoToEPURL"); }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            int userid = BLL.Util.GetLoginUserID();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            switch (RequestBusinessType.ToLower())
            {
                case "huimaiche":
                    msg = HuimaicheVerifyLogin(userid, msg);
                    break;
                case "yichechedai":
                    msg = CheDaiVerifyLogin(userid, msg);
                    break;
                case "jingzhunguanggao":
                    msg = JZGGVerifyLogin(userid, msg);
                    break;
                case "yichehui":
                    msg = YCHVerifyLogin(userid, msg);
                    break;
                case "easypass":
                    msg = EasyPassVerifyLogin(userid, msg);
                    break;
                default:
                    break;
            }
            context.Response.Write(msg);
        }

        #region 易湃业务验证登录
        /// <summary>
        /// 易湃业务验证登录
        /// </summary>
        /// <param name="userid">登录UserID</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string EasyPassVerifyLogin(int userid, string msg)
        {
            Random r = new Random();
            int rInt = r.Next(1000, 10000);
            string url = EasypassLoginURL + string.Format("?username={0}&sign={1}&r={2}",
                BLL.Util.GetLoginUserName(), GetEasyPassSign(EasypassLoginKey, rInt), rInt);
            if (!string.IsNullOrEmpty(RequestBusinessID))
            {
                url += "&wofollowid=" + RequestBusinessID;
            }
            return HttpUtility.UrlEncode(url);
        }

        private string GetEasyPassSign(string guid,int rInt)
        {
            var key = string.Format("{0}:{1}:{2}", guid, DateTime.Now.ToString("yyyy-MM-dd"), rInt);

            var md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var source = Encoding.UTF8.GetBytes(key.ToLower());
            return BitConverter.ToString(md5Provider.ComputeHash(source)).Replace("-","");
        }
        #endregion

        #region 易车惠业务验证登录
        /// <summary>
        /// 易车惠业务验证登录
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string YCHVerifyLogin(int userid, string msg)
        {
            BLL.Loger.Log4Net.Info("YICHEHUILoginURL:" + YICHEHUILoginURL + ";YICHEHUILoginKey:" + YICHEHUILoginKey + ";UserID:" + userid);
            msg = string.Format("{0}?sign={1}&userid={2}", YICHEHUILoginURL, GetMD5(userid.ToString() + YICHEHUILoginKey, "UTF-8"), userid.ToString());
            return HttpUtility.UrlEncode(msg);
        }

        /// <summary>
        ///   MD5加密
        /// </summary>
        /// <param name="encypStr">加密字符串</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>
        internal string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
        #endregion

        #region 精准广告验证登录
        /// <summary>
        /// 精准广告验证登录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string JZGGVerifyLogin(int userid, string msg)
        {
            try
            {
                string url = ConfigurationUtil.GetAppSettingValue("EasySetOff_URL");//"http://testcc.chedai.bitauto.com/Account/LogOn";
                if (!string.IsNullOrEmpty(RequestGoToEPURL) && RequestGoToEPURL!="#")
                {
                    url = RequestGoToEPURL;
                }
                // 加密字符串
                string enString = GetMd5Hash("u=" + userid.ToString() + ",key=" + JZGGKey);
                msg = string.Format("{0}{3}u={1}&sign={2}", url, userid.ToString(), enString,
                    (url.Contains("?")?"&":"?"));
                msg = HttpUtility.UrlEncode(msg);

            }
            catch (Exception ex)
            {
                msg = "Error";
            }
            return msg;
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
        #endregion

        #region 易车车贷登录逻辑
        /// <summary>
        /// 易车车贷登录逻辑
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string CheDaiVerifyLogin(int userid, string msg)
        {
            try
            {
                string url = ConfigurationUtil.GetAppSettingValue("CarFinancial_URL");//"http://testcc.chedai.bitauto.com/Account/LogOn";
                // 加密字符串
                string enString = Encrypt(userid.ToString(), YCCDKey);
                msg = string.Format("{0}?u={1}&sign={2}", url, userid.ToString(), enString);
                msg = HttpUtility.UrlEncode(msg);
            }
            catch (Exception ex)
            {
                msg = "Error";
            }
            return msg;
        }

        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(YCCDKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(YCCDKey);
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
        #endregion

        #region 惠买车登录逻辑
        /// <summary>
        /// 惠买车登录逻辑
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string HuimaicheVerifyLogin(int userid, string msg)
        {
            string RoleID = string.Empty;
            DataTable dtRole = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
            if (dtRole != null && dtRole.Rows.Count > 0)
            {
                RoleID = dtRole.Rows[0]["RoleID"].ToString();
            }
            string ReturnURL = string.Empty;
            string paraMsg = string.Empty;
            int ret = FanXianHelper.Instance.EPEmbedCC_AuthService(userid, RoleID, RequestHuimaicheTaskURL, out paraMsg, RequestEPEmbedCC_APPID);
            if (ret == 1)
            {
                msg = RequestYPFanXianURL + "?" + paraMsg;
            }
            else
            {
                msg = "Error";
            }
            return msg;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}