using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class CallRecordList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string RVID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["RVID"]))
                {
                    return Request["RVID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                BindData();

            }
        }
        private void BindData()
        {
            if (!string.IsNullOrEmpty(RVID))
            {
                DataTable table = BLL.CallRecordInfo.Instance.GetCallRecordByRVID(RVID);
                repeater_Contact.DataSource = table;
                //绑定列表数据
                repeater_Contact.DataBind();
            }
        }
    }
}