using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers
{
    public partial class SelectCustUserPoper : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string CrmCustID
        {
            get { return BLL.Util.GetCurrentRequestStr("CrmCustID"); }
        }
        public string UserName
        {
            get { return BLL.Util.GetCurrentRequestStr("UserName"); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                CustUserBind();
            }
        }

        private void CustUserBind()
        {
            int totalCount = 0;
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(CrmCustID))
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustUserMapping query = new YanFa.Crm2009.Entities.QueryCustUserMapping();
                query.CustID = CrmCustID;
                query.UserStatus = 0;

                dt = BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.GetCustUserMapping(query, "", BLL.PageCommon.Instance.PageIndex, 10, out totalCount);
            }
            else
            {
                BitAuto.YanFa.Crm2009.Entities.QueryUserInfo query = new YanFa.Crm2009.Entities.QueryUserInfo();
                if (!string.IsNullOrEmpty(UserName))
                {
                    query.TrueName = UserName;
                }
                query.Status = "0";
                dt = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetUserInfo(query, "", BLL.PageCommon.Instance.PageIndex, 10, out totalCount);
            }
            dt = BindBGName(dt);
            rptUser.DataSource = dt.DefaultView;
            rptUser.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 10, totalCount, 10, BLL.PageCommon.Instance.PageIndex, 3);
        
        }

        private DataTable BindBGName(DataTable dt)
        {
            string userid = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                userid += dr["UserID"].ToString() + ",";
            }
            DataTable dtUserGroup = BLL.BusinessGroup.Instance.GetBusinessGroupByUserIDs(userid.Trim(','));
            if (dtUserGroup==null)
            {
                DataTable newdt = new DataTable();
                newdt.Columns.Add("UserID", typeof(int));
                newdt.Columns.Add("BGID", typeof(int));
                newdt.Columns.Add("BGName", typeof(string));
                dtUserGroup = newdt;
                dtUserGroup.TableName = "dtUserGroup";
            }
            return BLL.Util.JoinDataTable(dt, dtUserGroup, "UserID", "UserID",true,false);
        }



    }
}