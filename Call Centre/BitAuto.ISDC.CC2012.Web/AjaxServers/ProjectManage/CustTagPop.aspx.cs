using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class CustTagPop : PageBase
    {
        public string CustID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CustID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CustID"]);
            }
        }
        public int userid;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userid = BLL.Util.GetLoginUserID();

                DataTable dtUserTags = BitAuto.YanFa.Crm2009.BLL.CustTag.Instance.GetCustTagByUserID(userid);
                if (dtUserTags != null)
                {
                    DataColumn newcol = new DataColumn("HasThisTag", typeof(string));
                    dtUserTags.Columns.Add(newcol);
                    //
                    DataTable dtCustUserTag = BitAuto.YanFa.Crm2009.BLL.CustTag.Instance.GetCustTagNameByCustIDAndUserID(userid, CustID);
                    foreach (DataRow row in dtCustUserTag.Rows)
                    {
                        foreach (DataRow row2 in dtUserTags.Rows)
                        {
                            if (row["TagName"].ToString() == row2["TagName"].ToString())
                            {
                                row2["HasThisTag"] = "1";
                            }
                        }
                    }

                    rp_CustTags.DataSource = dtUserTags;
                    rp_CustTags.DataBind();
                }
               
            }
        }
    }
}