using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class DataSelect : PageBase
    {
        public string LoginUserID = "";

        /// <summary>
        /// 根据所选择的业务组和类型获取数据类型,显示/隐藏选择数据的对话框的单选按钮和导入模板的显示
        /// </summary>
        public string ProjectType
        {
            get
            {
                return HttpContext.Current.Request["projecttype"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["projecttype"].ToString());
            }
        }

        public string BGID
        {
            get
            {
                return HttpContext.Current.Request["BGID"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGID"].ToString());
            }
        }
        public string BGName
        {
            get
            {
                return HttpContext.Current.Request["BGName"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGName"].ToString());
            }
        }
        public string CID
        {
            get
            {
                return HttpContext.Current.Request["CID"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CID"].ToString());
            }
        }
        public string CName
        {
            get
            {
                return HttpContext.Current.Request["CName"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CName"].ToString());
            }
        }
        /// 是否进行黑名单验证
        /// <summary>
        /// 是否进行黑名单验证
        /// </summary>
        public string IsBlacklistCheck
        {
            get
            {
                return HttpContext.Current.Request["IsBlacklistCheck"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["IsBlacklistCheck"].ToString());
            }
        }
        /// 验证方式
        /// <summary>
        /// 验证方式
        /// </summary>
        public string BlackListCheckType
        {
            get
            {
                return HttpContext.Current.Request["BlackListCheckType"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BlackListCheckType"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginUserID = BLL.Util.GetLoginUserID().ToString();
        }
    }
}