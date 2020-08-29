using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
 
using System.Reflection;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.Common
{
    /// <summary>
    /// GetFromEnum 的摘要说明
    /// </summary>
    public class GetFromEnum : IHttpHandler, IRequiresSessionState
    {

        #region 属性
        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"]).ToString(); }
        }
        private string RequestEnumName  //具体Enum类名
        {
            get { return HttpContext.Current.Request["EnumName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EnumName"]).ToString(); }
        }
        private string RequestValue      //具体Enum类的枚举值
        {
            get { return HttpContext.Current.Request["Value"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Value"]).ToString(); }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (RequestAction)
            {
                case "GetListByEnum": GetListByEnum(out msg);
                    break;
                case "GetNameByValue": GetNameByValue(out msg);
                    break;
            }

            context.Response.Write("{" + msg + "}");
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="msg"></param>
        private void GetListByEnum(out string msg)
        {
            msg = string.Empty;

            System.Type enumClass = System.Type.GetType("BitAuto.ISDC.CC2012.Entities." + RequestEnumName + ",BitAuto.ISDC.CC2012.Entities");

            DataTable dt = BLL.Util.GetEnumDataTable(enumClass);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    msg += "root:[";
                }

                msg += "{name:'" + dt.Rows[i]["name"].ToString() + "',value:'" + dt.Rows[i]["value"].ToString() + "'},";

                if (i == dt.Rows.Count - 1)
                {
                    msg = msg.TrimEnd(',') + "]";
                }
            }

        }

        /// <summary>
        /// 根据枚举值获取枚举名
        /// </summary>
        /// <param name="msg"></param>
        private void GetNameByValue(out string msg)
        {
            msg = "result:'false'";

            System.Type enumClass = System.Type.GetType("BitAuto.ISDC.CC2012.Entities." + RequestEnumName + ",BitAuto.ISDC.CC2012.Entities");

            DataTable dt = BLL.Util.GetEnumDataTable(enumClass);
            dt.DefaultView.RowFilter = "value=" + RequestValue;
            dt = dt.DefaultView.ToTable();
            if (dt.Rows.Count > 0)
            {
                msg = "result:'true',name:'" + dt.Rows[0]["name"].ToString() + "'";
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