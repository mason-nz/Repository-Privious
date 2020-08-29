using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
namespace BitAuto.DSC.IM_2015.WebService
{
    public class SC_Interface
    {
        #region Instance
        public static readonly SC_Interface Instance = new SC_Interface();
        #endregion

        private string strurl = System.Configuration.ConfigurationManager.AppSettings["SC_CookieURL"].ToString();
        private string strurltoken = System.Configuration.ConfigurationManager.AppSettings["SC_TokenURL"].ToString();
        private string SC_AppID = System.Configuration.ConfigurationManager.AppSettings["SC_AppID"].ToString();
        private string SC_Appsecret = System.Configuration.ConfigurationManager.AppSettings["SC_Appsecret"].ToString();
        /// <summary>
        /// 根据cookieid取惠买车用户
        /// </summary>
        /// <param name="cookieid"></param>
        /// <returns></returns>
        public Entities.HMC_Entity GetUserInfoByCookie(string cookieid)
        {
            try
            {
                #region 请求token
                WebClient mywebclient = new WebClient();
                StringBuilder postData = new StringBuilder();
                postData.Append("appid=" + SC_AppID);
                postData.Append("&secret=" + SC_Appsecret);
                postData.Append("&grant_type=client_credential");
                byte[] sendData = Encoding.GetEncoding("utf-8").GetBytes(postData.ToString());
                mywebclient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                mywebclient.Headers.Add("ContentLength", sendData.Length.ToString());
                byte[] recData = mywebclient.UploadData(strurltoken, "POST", sendData);
                Encoding big5 = Encoding.GetEncoding("utf-8");
                string strcheck = string.Empty;
                if (recData != null)
                {
                    strcheck = big5.GetString(recData);
                }
                //请求token失败
                if (strcheck.IndexOf("errcode") > -1)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(strcheck))
                {
                    return null;
                }


                Entities.Token_Entity tokenmodel = (Entities.Token_Entity)JsonConvert.DeserializeObject(strcheck, typeof(Entities.Token_Entity));
                if (tokenmodel == null)
                {
                    return null;
                }
                string Token = tokenmodel.access_token;
                #endregion

                #region 取用户信息
                string userinfo = string.Empty;
                WebClient mywebclientuser = new WebClient();
                StringBuilder postDataUser = new StringBuilder();
                postDataUser.Append("userId=" + cookieid);
                byte[] sendDataUser = Encoding.GetEncoding("utf-8").GetBytes(postDataUser.ToString());
                mywebclientuser.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                mywebclientuser.Headers.Add("Token", Token);
                mywebclientuser.Headers.Add("ContentLength", sendData.Length.ToString());
                byte[] recDataUser = mywebclientuser.UploadData(strurl, "POST", sendDataUser);
                if (recDataUser != null)
                {
                    userinfo = big5.GetString(recDataUser);
                }
                if (userinfo.IndexOf("errcode") > -1)
                {
                    return null;
                }
                return (Entities.HMC_Entity)JsonConvert.DeserializeObject(userinfo, typeof(Entities.HMC_Entity));
                #endregion
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
