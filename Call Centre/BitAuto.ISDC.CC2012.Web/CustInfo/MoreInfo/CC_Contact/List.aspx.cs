using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private CustInfoHelper ch = new CustInfoHelper();
        private Entities.ProjectTaskInfo task;

        private void BindData()
        {
            if (string.IsNullOrEmpty(ch.TID)) { return; }
            else
            {
                task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(ch.TID);
                if (task == null) { return; }
                this.TID = task.PTID.ToString();
            }

            int totalCount = 0;
            Entities.QueryProjectTask_Cust_Contact query = new Entities.QueryProjectTask_Cust_Contact();
            query.PTID= task.PTID;
            DataTable table = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(query, "", ch.CurrentPage, ch.PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_Contact.DataSource = table;
            }
            //绑定列表数据
            repeater_Contact.DataBind();
            //分页控件
            this.AjaxPager_Contact.InitPager(totalCount);
        }
        /// <summary>
        /// 根据contactID 获取负责会员名 add lxw 12.12.12
        /// </summary>
        /// <returns>string</returns>
        public string ShowManageMember(string contactid)
        {
            string strResult = string.Empty;
            if (!string.IsNullOrEmpty(contactid))
            {
                DataTable dt = BLL.ProjectTask_MemberContactMapping.Instance.GetList("CC_MCM.ContactID =" + contactid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        strResult += row["Abbr"] + "、";
                    }
                    strResult = strResult.TrimEnd('、');
                    if (strResult.Length > 16)
                    {
                        strResult = strResult.Substring(0, 16) + "...";
                    }
                }
            }
            return strResult;
        }
    }
}