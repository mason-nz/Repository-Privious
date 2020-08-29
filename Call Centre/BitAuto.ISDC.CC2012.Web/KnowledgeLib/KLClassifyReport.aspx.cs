using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class KLClassifyReport : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bindKnowledgeCategory();
        }
        //获取分类下拉列表
        private void bindKnowledgeCategory()
        {
            //ListItem firstItem = new ListItem();
            //firstItem.Text = "";
            //firstItem.Value = "";

            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = 1;
            query.Pid = 0;
            int? regionId = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(BLL.Util.GetLoginUserID()).RegionID;
            if (regionId.HasValue)
            {
                query.Regionid = regionId.Value;
            }

            int count;
            DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(query, "isnull(sortNum,999),kcid", 1, 10000, out count);
            selKCID1.DataSource = dt;
            selKCID1.DataTextField = "Name";
            selKCID1.DataValueField = "KCID";
            selKCID1.DataBind();
            selKCID1.Items.Insert(0, new ListItem() { Text = "请选择", Value = "-1" });

        }

    }
}