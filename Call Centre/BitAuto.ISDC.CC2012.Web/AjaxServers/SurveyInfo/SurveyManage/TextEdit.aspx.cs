using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage
{
    public partial class TextEdit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        ///  试题字符串
        /// </summary>
        public string JsonStr
        {
            get
            {
                //return HttpContext.Current.Request["JsonStr"] == null ? string.Empty : 
                //    HttpUtility.UrlDecode(HttpContext.Current.Request["JsonStr"].ToString()).Replace(@"\", @"\\");
                return HttpContext.Current.Request["JsonStr"] == null ? string.Empty :
                 HttpContext.Current.Request["JsonStr"].ToString();
            }
        }

        /// <summary>
        /// 试题ID
        /// </summary>
        public string SQID
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["sqid"]) ? (int.Parse(IndexNum) * -1).ToString() :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["sqid"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        ///  顺序，用来区分试题
        /// </summary>
        public string IndexNum
        {
            get
            {
                return HttpContext.Current.Request["indexNum"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["indexNum"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        /// 问卷ID
        /// </summary>
        public string SIID
        {
            get
            {
                return HttpContext.Current.Request["siid"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["siid"].ToString()).Replace(@"\", @"\\");
            }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string action
        {
            get
            {
                return HttpContext.Current.Request["action"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["action"].ToString()).Replace(@"\", @"\\");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}