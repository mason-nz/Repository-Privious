using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class ReturnVisitRecordList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public bool IsExport = false;//是否可以“导出”功能
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsExport = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT140401");
                BindVisitType();
            }

        }
        /// <summary>
        /// 绑定访问类型
        /// </summary>
        private void BindVisitType()
        {
            DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetVisitType();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.selVisitType.Items.Insert((i + 1), new ListItem(dt.Rows[i]["DictName"].ToString(), dt.Rows[i]["DictID"].ToString()));
                }
            }
        }
    }
}