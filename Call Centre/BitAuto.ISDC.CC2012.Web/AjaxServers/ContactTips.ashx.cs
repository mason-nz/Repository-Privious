using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;


namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// ContactTips 的摘要说明
    /// </summary>
    public class ContactTips : IHttpHandler, IRequiresSessionState
    {
        #region 属性

        public string keyWord
        {
            get
            {
                return HttpContext.Current.Request["keyWord"] == null ? string.Empty :
                    HttpContext.Current.Request["keyWord"].ToString();
            }

        }

        public string Tel
        {
            get
            {
                return HttpContext.Current.Request["Tel"] == null ? string.Empty :
                    HttpContext.Current.Request["Tel"].ToString();
            }

        }

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            getContact(out msg);

            context.Response.Write(msg);
        }

        //获取联系人
        public void getContact(out string msg)
        {
            msg = string.Empty;

            if (Tel == string.Empty)
            {
                return;
            }

            DataTable dt = BLL.CustBasicInfo.Instance.GetCustBasicInfosByTelAndName(Tel, keyWord);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    msg += "{[";
                }
                msg += "{'CustID':'" + dt.Rows[i]["CustID"] + "','CustName':'" + dt.Rows[i]["CustName"] + "'},";
                if (i == dt.Rows.Count - 1)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }
            }
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