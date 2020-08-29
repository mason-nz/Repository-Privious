using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class DataAdd : PageBase
    {
        public int RecID { set; get; }
        public string TTCode
        {
            get
            {
                return HttpContext.Current.Request["TTCode"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TTCode"].ToString());
            }
        }
        public string AddType
        {
            get
            {
                return HttpContext.Current.Request["AddType"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["AddType"].ToString());
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
        public string ExcelFilePath = "";
        public string ExcelFileName = "";
        public string LoginUserID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AddType == "1")
                {
                    ExcelFilePath = "/ProjectManage/CrmCustImport/Templet/CRM客户导入模版.xls";
                    ExcelFileName = "导入客户ID";
                }
                else
                {
                    Entities.TPage tpageMode = BLL.TPage.Instance.GetTPageByTTCode(TTCode);
                    if (tpageMode != null)
                    {
                        string path = "/upload/" + BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.Template, "/");
                        RecID = tpageMode.RecID;
                        ExcelFilePath = path + tpageMode.GenTempletPath;
                        ExcelFileName = tpageMode.TPName;
                    }
                }

                LoginUserID = BLL.Util.GetLoginUserID().ToString();
            }
        }
    }
}