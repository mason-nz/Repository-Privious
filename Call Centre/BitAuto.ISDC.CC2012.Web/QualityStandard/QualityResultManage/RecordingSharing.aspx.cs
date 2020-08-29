using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.QualityResultManage
{
    public partial class RecordingSharing : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindKCID2();
            }
        }

        /// <summary>
        /// 绑定录音共享2级分类
        /// </summary>
        private void BindKCID2()
        {
            int currentUserId = BLL.Util.GetLoginUserID();
            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(currentUserId);
            DataTable dt = BLL.KnowledgeCategory.Instance.GetCategoryByPName("录音共享",Convert.ToInt32(model.RegionID));
            if (dt != null && dt.Rows.Count != 0)
            {
                selKCID2.DataSource = dt;
                selKCID2.DataTextField = "Name";
                selKCID2.DataValueField = "KCID";
                selKCID2.DataBind();
            }
            selKCID2.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}