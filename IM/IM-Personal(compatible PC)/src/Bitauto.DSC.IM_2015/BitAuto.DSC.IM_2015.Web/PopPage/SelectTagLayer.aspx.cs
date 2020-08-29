using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace BitAuto.DSC.IM_2015.Web.PopPage
{
    public partial class SelectTagLayer : System.Web.UI.Page
    {
        public string BusiTypeId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("busitypeid");
            }
        }
        public string TagId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("tagid");
            }
        }
        public string ParentID;

        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                try
                {
                    BindBussyType();
                    BindData(ddlBussyType.Value);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("选择标签出错，" + ex.Message.ToString());
                }
            }
        }
        private void BindBussyType()
        {
            DataTable dt = BLL.UserSendMessage.Instance.GetWOrderBusiType();
            ddlBussyType.DataSource = dt;
            ddlBussyType.DataValueField = "RecID";
            ddlBussyType.DataTextField = "BusiTypeName";
            ddlBussyType.DataBind();
            if (!string.IsNullOrEmpty(BusiTypeId)&&BusiTypeId!="0")
            {
                ddlBussyType.Value = BusiTypeId;
            }
        }
        private void BindData(string busiTypeId)
        {
            DataTable dt = BLL.UserSendMessage.Instance.GetWOrderTab(busiTypeId);
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            if (!string.IsNullOrEmpty(TagId) && TagId != "0")
            {
                DataRow[] drtag = dt.Select(" RecID=" + TagId, " SortNum asc");
                if (drtag != null && drtag.Length > 0)
                {
                    ParentID = drtag[0]["PID"].ToString();
                }
            }
        }

        //private string GetLeveOne(DataRow[] drs, string oneSelected)
        //{
        //    if (drs == null || drs.Length == 0)
        //    {
        //        return "";
        //    }
        //    StringBuilder html = new StringBuilder();
        //    html.Append("<ul>");
        //    for (int i = 0; i < drs.Length; i++)
        //    {
        //        string selectStyle = "";
        //        string recid = drs[i]["RecID"].ToString();
        //        if (recid == oneSelected)//已选中
        //        {
        //            selectStyle = " class='hover'";
        //        }
        //        html.AppendFormat("<li {3} id='cctabone{0}' onclick=\"setCCTab('cctabone',{0},{1})\" >{2}</li>", i + 1, drs.Length, drs[i]["TagName"].ToString(), selectStyle);
        //    }
        //    html.Append("</ul>");
        //    return html.ToString();
        //}

        //private string GetLeveTwo(DataTable dt, DataRow[] drsone, string oneSelected)
        //{
        //    if (drsone == null || drsone.Length == 0 || dt == null || dt.Rows.Count == 0)
        //    {
        //        return "";
        //    }
        //    StringBuilder html = new StringBuilder();
        //    DataRow[] drstwo;
        //    html.Append("<ul>");
        //    for (int i = 0; i < drsone.Length; i++)
        //    {
        //        string oneSelectStyle = "";
        //        string oneRecid = drsone[i]["RecID"].ToString();
        //        if (oneRecid == oneSelected)//已选中
        //        {
        //            oneSelectStyle = " class='hover'";
        //            html.AppendFormat("<div {1} id='con_cctabone_{0}'  style='display: block;'>", i + 1, oneSelectStyle);
        //        }
        //        else
        //        {
        //            html.AppendFormat("<div {1} id='con_cctabone_{0}'  style='display: none;'>", i + 1, oneSelectStyle);
        //        }
        //        drstwo = dt.Select(" PID='" + oneRecid + "'", " sortnum asc");

        //        html.Append("<ul>");
        //        for (int j = 0; j < drstwo.Length; j++)
        //        {
        //            string selectStyle = "";
        //            string recid = drstwo[j]["RecID"].ToString();
        //            if (recid == TagId)//已选中
        //            {
        //                selectStyle = " class='current'";
        //            }
        //            html.AppendFormat(" <li {2} tagid='{1}' parentname='{3}' tagname='{0}'><a href='#'>{0}</a></li>", drstwo[j]["TagName"].ToString(), recid, selectStyle, drsone[i]["TagName"].ToString());
        //        }
        //        html.Append("</ul>");
        //        html.Append("</div>");
        //    }
        //    return html.ToString();
        //}
    }
}