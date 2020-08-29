using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.Statistics.MemberIDImport
{
    public partial class Main : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// 导入类型 Member/CustIDYP
        /// <summary>
        /// 导入类型 Member/CustIDYP
        /// </summary>
        public string ExportTyle
        {
            get
            {
                return string.IsNullOrEmpty(this.Request["Type"]) ? string.Empty : this.Request["Type"].ToString();
            }
        }

        public string TitleStr
        {
            get
            {
                if (ExportTyle == "Member")
                {
                    return "导出会员信息";
                }
                else if (ExportTyle == "CustIDYP")
                {
                    return "导出客户ID";
                }
                else
                {
                    return "其他导出";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}