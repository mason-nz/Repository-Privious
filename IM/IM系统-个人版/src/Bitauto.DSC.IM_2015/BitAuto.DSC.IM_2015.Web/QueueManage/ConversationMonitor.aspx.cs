using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.QueueManage
{
    public partial class ConversationMonitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSourceType();
                BindBusinessGroup();
            }
        }
        // <summary>
        /// 绑定业务来源
        /// </summary>
        private void BindSourceType()
        {
            selSourceType.DataSource = BLL.Util.GetAllSourceType(true);
            selSourceType.DataValueField = "SourceTypeValue";
            selSourceType.DataTextField = "SourceTypeName";
            selSourceType.DataBind();
        }

        private void BindBusinessGroup()
        {
            DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(BLL.Util.GetLoginUserID());
            string bgids = "";
            if(dt!=null && dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    bgids += "," + row["BGID"].ToString();
                }
            }
            if(bgids!="")
            {
                bgids = bgids.Substring(1);
            }
            else{
                bgids = "-1";
            }
         
            selBusinessGroup.DataSource = dt;
            selBusinessGroup.DataValueField = "BGID";
            selBusinessGroup.DataTextField = "Name";
            selBusinessGroup.DataBind();

            selBusinessGroup.Items.Insert(0, new ListItem("请选择",bgids));
        }
    }
}