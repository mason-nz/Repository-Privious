using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class LabelConfig : System.Web.UI.Page
    {
        private string RequestBGID
        {
            get { return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString()); }
        }
        public int MinLTID = 0;
        public int MaxLTID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGroup();
            //BindData();
        }

        private void BindGroup()
        {
            DataTable dt = null;

            dt = BLL.BaseData.Instance.GetUserGroupByUserID(BLL.Util.GetLoginUserID());
            if (dt != null)
            {
                selGroup.DataSource = dt;
                selGroup.DataValueField = "BGID";
                selGroup.DataTextField = "Name";
                selGroup.DataBind();
                //selGroup.Items.Insert(0, new ListItem("请选择", "-1"));
                selGroup.Value = RequestBGID;

                selGroup.Disabled = true;
            }
        }

        //private void BindData()
        //{
        //    DataTable dt = null;

        //    dt = BLL.LabelTable.Instance.GetLabelTableByBGID(Convert.ToInt32(RequestBGID));
        //    if (dt.Rows.Count > 0)
        //    {
        //        MinLTID = CommonFunc.ObjectToInteger(dt.Rows[0]["LTID"]);
        //        MaxLTID = CommonFunc.ObjectToInteger(dt.Rows[dt.Rows.Count - 1]["LTID"]);
        //    }
        //    repeaterConfig.DataSource = dt;
        //    repeaterConfig.DataBind();
        //}

        
    }
}