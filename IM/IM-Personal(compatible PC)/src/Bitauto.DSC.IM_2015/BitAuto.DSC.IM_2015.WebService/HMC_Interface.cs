using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace BitAuto.DSC.IM_2015.WebService
{
    public class HMC_Interface
    {
        #region Instance
        public static readonly HMC_Interface Instance = new HMC_Interface();
        private string strurl = System.Configuration.ConfigurationManager.AppSettings["HMC_CookieURL"].ToString();
        /// <summary>
        /// 根据cookieid取惠买车用户
        /// </summary>
        /// <param name="cookieid"></param>
        /// <returns></returns>
        public Entities.HMC_Entity GetUserInfoByCookie(string cookieid)
        {
            string url= strurl+"?mccookie=" + cookieid;
            //strurl += "?mccookie=965fb1d5-1144-41d9-b0f9-2cf4afd2d3b1%2611958%26110_2%265D5CEF9E40AB20711FE912253706BDC8";
            try
            {
                WebClient mywebclient = new WebClient();
                byte[] cheklist = mywebclient.DownloadData(url);
                Encoding big5 = Encoding.GetEncoding("utf-8");
                string strcheck = string.Empty;
                if (cheklist != null)
                {
                    strcheck = big5.GetString(cheklist);
                }
                if (!string.IsNullOrEmpty(strcheck))
                {
                    return (Entities.HMC_Entity)JsonConvert.DeserializeObject(strcheck, typeof(Entities.HMC_Entity));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
