using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class AddConSentence : System.Web.UI.Page
    {
        private string RequestisEdit
        {
            get { return HttpContext.Current.Request["isEdit"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["isEdit"].ToString()); }
        }
        private string RequestisCSID
        {
            get { return HttpContext.Current.Request["CSID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CSID"].ToString()); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindsltGroup();

                int itemp = 0;
                if (int.TryParse(RequestisCSID, out itemp))
                {
                    //sltLabel.Value = RequestisCSID;
                    //Entities.ComSentence model = null;
                    //model = BLL.ComSentence.Instance.GetComSentence(itemp);

                    //if (model != null)
                    //{
                    //    sltLabel.Value = model.LTID.ToString();
                    //    textMemo.Value = model.Name;
                    //}
                }
            }
        }

        public void BindsltGroup()
        {
            DataTable dt = null;

            Entities.QueryLabelTable query = new Entities.QueryLabelTable();
            query.Status = 0;

            int total;
            dt = BLL.LabelTable.Instance.GetLabelTable(query, " LTID DESC", 1, 999999, out total);

            sltLabel.DataSource = dt;

            if (dt != null && dt.Rows.Count != 0)
            {
                sltLabel.DataSource = dt;
                sltLabel.DataTextField = "Name";
                sltLabel.DataValueField = "LTID";
                sltLabel.DataBind();
            }
            sltLabel.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}