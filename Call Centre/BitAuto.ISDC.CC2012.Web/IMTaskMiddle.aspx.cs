using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.EasyPass;
using System.Security.Cryptography;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;


namespace BitAuto.ISDC.CC2012.Web
{
    public partial class IMTaskMiddle : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected static string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        public string YPFanXianURL = ConfigurationUtil.GetAppSettingValue("EPEmbedCCHBugCar_URL");//易湃签入CC惠买车任务页面，APPID
        public string EPEmbedCC_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC惠买车页面，APPID        
        private static string EasypassLoginURL = ConfigurationUtil.GetAppSettingValue("EasypassLoginURL");//易湃业务_授权URL
        private static string EasypassLoginKey = ConfigurationUtil.GetAppSettingValue("EasypassLoginKey");//易湃业务_授权key
        private string RequestGoToEPURL
        {
            get { return System.Web.HttpContext.Current.Request["GoToEPURL"] == null ? string.Empty : System.Web.HttpContext.Current.Request["GoToEPURL"].Trim(); }
        }
        private string RequestUserID
        {
            get { return System.Web.HttpContext.Current.Request["UserID"] == null ? string.Empty : HttpUtility.UrlDecode(System.Web.HttpContext.Current.Request["UserID"].Trim()); }
        }
        private string RequestBussinessType
        {
            get { return System.Web.HttpContext.Current.Request["BussinessType"] == null ? string.Empty : System.Web.HttpContext.Current.Request["BussinessType"].Trim(); }
        }
        private string RequestTaskID
        {
            get { return System.Web.HttpContext.Current.Request["TaskID"] == null ? string.Empty : System.Web.HttpContext.Current.Request["TaskID"].Trim(); }
        }
        private string RequestWorkOrderStatus
        {
            get { return System.Web.HttpContext.Current.Request["WorkOrderStatus"] == null ? string.Empty : System.Web.HttpContext.Current.Request["WorkOrderStatus"].Trim(); }
        }
        private string RequestReceiverID
        {
            get { return System.Web.HttpContext.Current.Request["ReceiverID"] == null ? string.Empty : System.Web.HttpContext.Current.Request["ReceiverID"].Trim(); }
        }
        private string RequestBGID
        {
            get { return System.Web.HttpContext.Current.Request["BGID"] == null ? string.Empty : System.Web.HttpContext.Current.Request["BGID"].Trim(); }
        }
        private string RequestSCID
        {
            get { return System.Web.HttpContext.Current.Request["SCID"] == null ? string.Empty : System.Web.HttpContext.Current.Request["SCID"].Trim(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(Convert.ToInt32(RequestUserID));
            if (model == null)
            {
                Response.Write("ERROR:用户不存在");
                return;
            }
            string operStr = string.Empty;
            VisitBusinessTypeEnum bt = (VisitBusinessTypeEnum)CommonFunction.ObjectToInteger(RequestBussinessType);
            if (bt != VisitBusinessTypeEnum.S0_其他系统 && bt != VisitBusinessTypeEnum.None)
            {
                string url = "";
                url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(RequestTaskID, RequestBGID, RequestSCID);
                url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(RequestTaskID, url);
                operStr += url;
            }
            else
            {
                string url = "";
                url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(RequestTaskID, RequestBGID, RequestSCID);
                if (!string.IsNullOrEmpty(url))
                {
                    url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(RequestTaskID, url);
                    //如果是惠买车业务               
                    if (BLL.Util.IsMatchBGIDAndSCID(RequestBGID, RequestSCID, "EPEmbedCC_HMCBGIDSCID"))//惠买车
                    {
                        operStr += GetEPURL(YPFanXianURL, url);
                    }
                    else if (BLL.Util.IsMatchBGIDAndSCID(RequestBGID, RequestSCID, "EasySetOff_BGIDSCID"))//精准广告
                    {
                        operStr += GetEasyOffURL(url);
                    }
                    else if (BLL.Util.IsMatchBGIDAndSCID(RequestBGID, RequestSCID, "CarFinancial_BGIDSCID"))//易车车贷
                    {
                        operStr += GetCarFinancialURL(url);
                    }
                    else if (BLL.Util.IsMatchBGIDAndSCID(RequestBGID, RequestSCID, "EasyPass_BGIDSCID"))//易湃业务
                    {
                        operStr += GetEasyPassURL(RequestTaskID);
                    }
                    else
                    {
                        operStr += url;
                    }
                }
            }

            Response.Redirect(operStr);
        }

        private string GetEasyPassURL(string taskID)
        {
            Random r = new Random();
            int rInt = r.Next(1000, 10000);
            string url = EasypassLoginURL + string.Format("?username={0}&sign={1}&r={2}",
                BLL.Util.GetLoginUserName(), GetEasyPassSign(EasypassLoginKey, rInt), rInt);
            if (!string.IsNullOrEmpty(taskID))
            {
                url += "&wofollowid=" + taskID;
            }
            return url;
        }

        private string GetEasyPassSign(string guid, int rInt)
        {
            var key = string.Format("{0}:{1}:{2}", guid, DateTime.Now.ToString("yyyy-MM-dd"), rInt);

            var md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var source = Encoding.UTF8.GetBytes(key.ToLower());
            return BitConverter.ToString(md5Provider.ComputeHash(source)).Replace("-", "");
        }

        public string GetEPURL(string YPFanXianURL, string GoToEPURL)
        {
            string msg = string.Empty;
            try
            {
                string RoleID = string.Empty;
                int userid = Convert.ToInt32(RequestUserID);
                DataTable dtRole = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
                if (dtRole != null && dtRole.Rows.Count > 0)
                {
                    RoleID = dtRole.Rows[0]["RoleID"].ToString();
                }
                string ReturnURL = string.Empty;
                int ret = FanXianHelper.Instance.EPEmbedCC_AuthService(userid, RoleID, GoToEPURL, out msg, EPEmbedCC_APPID);
                if (ret == 1)
                {
                    msg = YPFanXianURL + "?" + msg;
                }
                else
                {
                    msg = "Error";
                }
            }
            catch (Exception ex)
            {
                msg = "Error：" + ex.Message;
            }
            return msg;
        }

        public string GetEasyOffURL(string GoToEPURL)
        {
            string msg = string.Empty;

            try
            {
                int userid = Convert.ToInt32(RequestUserID);
                string Key = ConfigurationUtil.GetAppSettingValue("EasySetOff_GenURLParaMD5");
                string url = ConfigurationUtil.GetAppSettingValue("EasySetOff_URL");
                if (!string.IsNullOrEmpty(GoToEPURL))
                {
                    url = GoToEPURL;
                }
                // 加密字符串
                string enString = GetMd5Hash("u=" + userid.ToString() + ",key=" + Key);
                msg = string.Format("{0}?u={1}&sign={2}", url, userid.ToString(), enString);
                msg = HttpUtility.UrlEncode(msg);

            }
            catch (Exception ex)
            {
                msg = "Error：" + ex.Message;
            }

            return msg;
        }

        public string GetCarFinancialURL(string GoToEPURL)
        {
            string msg = string.Empty;

            try
            {
                int userid = Convert.ToInt32(RequestUserID);
                string Key = ConfigurationUtil.GetAppSettingValue("CarFinancial_GenURLParaMD5");
                //string url = ConfigurationUtil.GetAppSettingValue("CarFinancial_URL");
                // 加密字符串
                string enString = Encrypt(userid.ToString(), Key);
                msg = string.Format("{0}?u={1}&sign={2}", GoToEPURL, userid.ToString(), enString);
                msg = HttpUtility.UrlEncode(msg);

            }
            catch (Exception ex)
            {
                msg = "Error：" + ex.Message;
            }

            return msg;
        }

        /// <summary>
        /// 精准广告获取MD5加密字符串
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


        //汽车金融加密
        public static string Encrypt(string Text, string sKey)
        {
            string Key = ConfigurationUtil.GetAppSettingValue("CarFinancial_GenURLParaMD5");
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
    }
}