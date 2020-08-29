using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfSamples.DynamicProxy;
using System.Reflection;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;
using BitAuto.ISDC.CC2012.WebService.HuiMaiChe.ServiceReference;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Web.Script.Serialization;

namespace BitAuto.ISDC.CC2012.WebService.EasyPass
{
    public class FanXianHelper
    {


        private string EPEmbedCC_AuthMD5 = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCC_AuthMD5"];//易湃签入CC页面，APPID
        private string EPEmbedCC_DesStr = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCC_DesStr"];//易湃签入CC页面，生成URL参数时，加密DES字符串
        private string EPEmbedCC_GenURLParaMD5 = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCC_GenURLParaMD5"];//易湃签入CC页面，生成URL参数时，跳转加密MD5加密串
        //add by qizq 2014-4-15 加易湃签入CC页面地址

        #region Instance
        public static readonly FanXianHelper Instance = new FanXianHelper();
        #endregion

        /// <summary>
        /// 根据地区ID、车款ID，返回经销商ID数组
        /// </summary>
        /// <param name="locationid">地区ID（省份ID或城市ID）</param>
        /// <param name="carid">车款ID</param>
        /// <returns>返回经销商ID数组</returns>
        public int[] GetFanxianDealers(int locationid, int carid)
        {
            Easypass.ServiceReference.FaixianForCCClient fc = new Easypass.ServiceReference.FaixianForCCClient();
            int[] dealerids = fc.GetFanxianDealers(locationid, carid);
            fc.Close();
            return dealerids;
            //return (int[])WebServiceHelper.InvokeWCF(YPFanXianURL, "GetFanxianDealers", new object[] { locationid, carid });
        }


        /// <summary>
        /// 提交订单接口
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="name">用户名，长度<6</param>
        /// <param name="mobile">用户手机号</param>
        /// <param name="carid">车款ID</param>
        /// <param name="membercode">经销商ID</param>
        /// <param name="locationid">用户所在城市ID</param>
        /// <returns>返回JSON形式，如{code:1,message:"OK"}。code，int，提交订单成功与否标识，1成功-1失败；message，String，辅助信息，如失败原因</returns>
        public string SubmitFanxianOrder(int userid, string name, string mobile, int carid, int membercode, int locationid)
        {
            Easypass.ServiceReference.FaixianForCCClient fc = new Easypass.ServiceReference.FaixianForCCClient();
            string result = fc.SubmitFanxianOrder(userid, name, mobile, carid, membercode, locationid);
            fc.Close();
            return result;
            //object[] objArray = new object[] { userid, name, mobile, carid, membercode, locationid };
            //string result = (string)WebServiceHelper.InvokeWCF(YPFanXianURL, "SubmitFanxianOrder", objArray);
            //return result;
        }

        /// <summary>
        /// 根据当前登录用户ID、当前登录者角色ID，授权易湃跳转URL
        /// </summary>
        /// <param name="userID">当前登录用户ID</param>
        /// <param name="userRoleID">当前登录者角色ID</param>
        /// <param name="redirectURL">验证成功后，跳转的URL</param>
        /// <param name="urlPara">若授权成功，则返回URL参数，如p={1}&sign={2}&appid={3}&tokenid={4}</param>
        /// <returns>返回授权结果（1：成功，-2：失败）</returns>
        public int EPEmbedCC_AuthService(int userID, string userRoleID, string redirectURL, out string urlPara, string EPEmbedCC_APPID)
        {
            urlPara = string.Empty;//最后构造url参数
            //构造参数（APPID、IP、当前时间、MD5加密key）
            ServerParas paras = new ServerParas();
            paras.AppID = int.Parse(EPEmbedCC_APPID);
            paras.IPAddress = BLL.Util.GetIPAddress();
            paras.Timespan = DateTime.Now.ToString();
            string input = paras.AppID + paras.IPAddress + paras.Timespan + EPEmbedCC_AuthMD5;
            paras.Sign = GetMd5Hash(input);

            HuiMaiChe.ServiceReference.AuthServiceSoapClient server = new AuthServiceSoapClient();


            //AuthService server = new AuthService();
            /*
             * 调用接口，返回验证结果
             * ResultCode:1(调用成功)，ResultInfo:返回临时令牌（如：b4f5cbe4-4330-4dc8-b810-9b2570349882）
             * ResultCode:-2(调用失败)，ResultInfo:无权限调用！
             */
            ServiceResult result = server.GetTempToken(paras);
            // 调用成功
            if (result.ResultCode > 0)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("UserId", userID.ToString());
                dict.Add("UserAuth", userRoleID);
                if (!string.IsNullOrEmpty(redirectURL))
                {
                    dict.Add("Url", redirectURL);
                }
                string json = SerializeJSON<Dictionary<string, string>>(dict);
                // 加密字符串
                string enString = DesEncrypt(json, EPEmbedCC_DesStr);
                string md5 = GetMd5Hash(EPEmbedCC_APPID + result.ResultInfo + enString + EPEmbedCC_GenURLParaMD5);
                //urlPara = string.Format("p={0}&sign={1}&appid={2}&tokenid={3}",
                //    HttpUtility.UrlEncode(enString), HttpUtility.UrlEncode(md5),
                //    HttpUtility.UrlEncode(EPEmbedCC_APPID), HttpUtility.UrlEncode(result.ResultInfo));
                urlPara = HttpUtility.UrlEncode(string.Format("p={0}&sign={1}&appid={2}&tokenid={3}",
                   (enString), (md5),
                   (EPEmbedCC_APPID), (result.ResultInfo)));
                //add by qizq 2014-4-15 加上EP登录地址
                //urlPara = redirectURL + "?" + urlPara;
            }
            return result.ResultCode;
        }

        #region 易湃签入CC页面,私有及保护类型方法
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

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="inputStr">需要加密的字符串</param>
        /// <param name="encryptKey">解密Key</param>
        /// <returns>加密后的字符串</returns>
        private string DesEncrypt(string inputStr, string encryptKey)
        {
            try
            {
                byte[] byKey = null;
                byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey.Substring(0, IV.Length));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(inputStr);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception)
            {
                return inputStr;
            }
        }

        /// <summary>
        /// 序列化成JSON字符串
        /// </summary>
        /// <param name="json">json对象</param>
        /// <returns>转换的字符串</returns>
        protected string SerializeJSON<T>(T json)
        {
            JavaScriptSerializer _javaScriptSerializer = new JavaScriptSerializer();
            return _javaScriptSerializer.Serialize(json);
        }
        #endregion
    }
}
