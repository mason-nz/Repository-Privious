using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using BitAuto.YanFa.OASysRightManager2011.Common;
using BitAuto.Utils.Security;

namespace BitAuto.DSC.APPReport2016.WebAPI
{
    public partial class MoniLogin : System.Web.UI.Page
    {
        protected static string cookieName = "app-report";//仪表盘报表登录验证cookies的key
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string adName = txtAdName.Value;
            if (string.IsNullOrEmpty(adName))
            {
                spanError.InnerText = "请输入域账号";
            }
            else
            {
                string result = string.Empty;

                int userid = BitAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetEmployeeIDByDomainAccount(adName);
                if (userid>0)
                {
                    var model = BitAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetEmployeeInfoByUserID(userid);
                    if (model!=null&&string.IsNullOrEmpty(model.EmployeeNumber)==false)
                    {
                        string genEncrypt = DESEncryptor.MD5Hash(DateTime.Now.ToString("MM|yyyy|dd") + model.EmployeeNumber + cookieName, Encoding.GetEncoding("utf-8"));
                        genEncrypt += "|" + model.EmployeeNumber;

                        System.Web.HttpContext WEBHTTP = System.Web.HttpContext.Current;
                        WEBHTTP.Response.Cookies[cookieName].Value = genEncrypt.ToLower();
                        //WEBHTTP.Response.Cookies[cookieName].Domain = Util.GetDomain();

                        result += "<br/>模拟登陆成功，写入Cookies成功！";
                    }
                    else
                    {
                        result += "<br/>模拟登陆失败！根据UserID找不到人员信息，或员工编号为空！";
                    }
                }
                else
                {
                    result += "<br/>模拟登陆失败！根据域账号找不到人员信息！";
                }
                


                //string url = "http://api.hr.oa1.bitauto.com/Authorize/Login";
                //string postDataStr = "domainAccount=test01\\" + adName + "&password=123.abc";
                //string result = HttpHelper.HttpGet(url, postDataStr);
                
                //try
                //{
                //    JsonResult jr = (JsonResult)Newtonsoft.Json.JsonConvert.DeserializeObject(result, typeof(JsonResult));
                //    if (jr.Status == 0 && jr.Message == "Success")
                //    {
                //        LoginInfo li = (LoginInfo)Newtonsoft.Json.JsonConvert.DeserializeObject(jr.Result.ToString(), typeof(LoginInfo));
                //        string cookies = li.LoginCookies + "|" + li.EmployeeNumber;

                //        System.Web.HttpContext WEBHTTP = System.Web.HttpContext.Current;
                //        WEBHTTP.Session["userid"] = li.UserID;
                //        WEBHTTP.Session["username"] = adName;
                //        WEBHTTP.Response.Cookies[cookieName].Value = cookies;
                //        WEBHTTP.Response.Cookies[cookieName].Domain = Util.GetDomain();

                //        result += "<br/>调用接口成功";
                //    }
                //    else
                //    {
                //        result += "<br/>调用接口失败";
                //    }
                //}
                //catch (Exception ex)
                //{
                //    result="解析json异常："+ex.Message;
                //}

                spanError.InnerHtml = result;
            }
        }
    }



    public class HttpHelper
    {
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }


    }

    [Serializable]
    public class JsonResult 
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }

        public bool? IsOverdue { get; set; }
    }

    [Serializable]
    public class LoginInfo
    {
        public int LoginResult { get; set; }

        public int UserID { get; set; }

        public string EmployeeNumber { get; set; }

        public string LoginCookies { get; set; }
    }
}