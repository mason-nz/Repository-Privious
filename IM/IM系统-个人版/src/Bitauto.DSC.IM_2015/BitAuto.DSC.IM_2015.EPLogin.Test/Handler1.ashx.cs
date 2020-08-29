using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.Utils;
using System.Data;
namespace BitAuto.DSC.IM_2015.EPLogin.Test
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string msg = string.Empty;
            context.Response.ContentType = "text/plain";
            if (action == "produce")
            {
                Deal(out msg);
            }
            else
            {
                //GetMemebercode(out msg);
            }
            context.Response.Write(msg);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public string useridstr
        {
            get { return HttpContext.Current.Request.Form["txtUserID"]; }
        }
        public string action
        {
            get { return HttpContext.Current.Request.Form["action"]; }
        }
        public string membercodestr
        {
            get { return HttpContext.Current.Request.Form["txtMemberCode"]; }
        }
        public string keystr
        {
            get { return HttpContext.Current.Request.Form["txtKey"]; }
        }
        public string provinceid
        {
            get { return HttpContext.Current.Request.Form["provinceid"]; }
        }
        protected void Deal(out string msg)
        {
            msg = string.Empty;
            try
            {
                EP_DESEncryptor code = new EP_DESEncryptor(keystr);
                string[] useridarray = useridstr.Split(',');
                string[] usermemercode = membercodestr.Split(',');
                for (int i = 0; i < usermemercode.Length; i++)
                {
                    msg = code.DesEncrypt("{\r\n" +
                      "\"UserId\": " + useridarray[i].Trim() + ",\r\n" +
                      "\"UserName\": \"hello-hellokity@sina.com\",\r\n" +
                      "\"Mobile\": \"15001112627\",\r\n" +
                      "\"Email\": \"hello-hellokity@sina.com\",\r\n" +
                      "\"Post\": \"市场经理\",\r\n" +
                      "\"DealerId\": " + usermemercode[i].Trim() + ",\r\n" +
                      "\"DateTimeFormat\": \"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\",\r\n" +
                      "\"AppId\": \"E6803316-A286-4417-97BC-213F13973207\"\r\n" +
                      "}");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
        //protected void GetMemebercode(out string msg)
        //{
        //    //msg = string.Empty;
        //    //if (provinceid != "-1")
        //    //{
        //    //    DataTable dt = BLL.EPVisitLog.Instance.GetDMSMemberByProvince(provinceid);
        //    //    if (dt != null && dt.Rows.Count > 0)
        //    //    {
        //    //        for (int i = 0; i < dt.Rows.Count; i++)
        //    //        {
        //    //            msg += dt.Rows[i]["membercode"].ToString() + ",";
        //    //        }
        //    //        msg = msg.Substring(0, msg.Length - 1);
        //    //    }
        //    //}
        //}
    }
}