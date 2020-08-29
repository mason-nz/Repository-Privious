using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo
{
    public partial class GroupManage : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string RequestPopGroup
        {
            get { return HttpContext.Current.Request["popGroup"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["popGroup"].ToString()); }
        }
        public string RegionID
        {
            get { return BLL.Util.GetCurrentRequestStr("RegionID"); }
        }
        public string Status
        {
            get { return BLL.Util.GetCurrentRequestStr("Status"); }
        }
        #endregion

        int userID = 0;
        DataTable tbArea = new DataTable();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataColumn dcName = new DataColumn("Name", typeof(string));
                DataColumn dcValue = new DataColumn("Value", typeof(string));
                tbArea.Columns.Add(dcName);
                tbArea.Columns.Add(dcValue);

                GetArea();
                rptRegion.DataSource = tbArea;
                rptRegion.DataBind();
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            DataTable dt = null;
            if (tbArea.Rows.Count > 1)
            {
                dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();
            }
            else if (tbArea.Rows.Count == 1)
            {
                dt = BLL.BusinessGroup.Instance.GetBusinessGroupByAreaID(int.Parse(tbArea.Rows[0]["Value"].ToString()));
            }
            if (dt != null)
            {
                string where = string.Empty;
                if (RegionID != "" && RegionID != "-1")
                {
                    where += "RegionID=" + RegionID;
                }
                if (Status != string.Empty)
                {
                    if (where != string.Empty)
                    {
                        where += " And ";
                    }
                    where += "Status=" + Status;
                }

                dt.DefaultView.RowFilter = where;
                rptGroup.DataSource = dt.DefaultView;
                rptGroup.DataBind();
            }
        }

        private void GetArea()
        {
            AreaManageConfig config = new AreaManageConfig(HttpContext.Current.Server);
            List<string> list = config.GetCurrentUserArea();
            if (list != null && list.Count > 0)
            {
                foreach (string s in list)
                {
                    DataRow dr = tbArea.NewRow();
                    int value = 1;
                    if (s == "西安")
                    {
                        value = 2;
                    }
                    dr["Name"] = s;
                    dr["Value"] = value;

                    tbArea.Rows.Add(dr);
                }
            }

        }

        protected void rptSelectBind(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptSelect = e.Item.FindControl("rptSelect") as Repeater;
                DataTable dt = BLL.BusinessGroup.Instance.GetCallDisplay();
                rptSelect.DataSource = dt;
                rptSelect.DataBind();

                if (tbArea.Rows.Count > 0)
                {
                    Repeater rptSelectArea = e.Item.FindControl("rptSelectArea") as Repeater;

                    rptSelectArea.DataSource = tbArea;
                    rptSelectArea.DataBind();
                }
            }
        }


        public int GetUserCountByGroup(string bgid)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(bgid))
            {
                string where = " And BGID=" + bgid;
                where += " and ui.Status=0 ";
                string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID");
                int DepCount = PartIDs.Split(',').Length;
                if (DepCount > 0)
                {
                    where += " and (";
                    for (int i = 0; i < DepCount; i++)
                    {
                        if (i != 0)
                        {
                            where += " or ";
                        }
                        where += " DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                    }
                    where += " )";
                }
                count = BLL.EmployeeAgent.Instance.GetUserCountByGroup(where);
            }

            return count;
        }

        public int CanStop(object Bgid)
        {
            string where = string.Empty;
            where += " and BGID=" + Bgid;
            if (RegionID != "" && RegionID != "-1")
            {
                where += " and RegionID=" + RegionID;
            }
            //add by qizq 2014-12-1取分组下在职人员，并且部门在配置文件部门及其子部门下
            where += " and ui.Status=0 ";
            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID");
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                where += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        where += " or ";
                    }
                    where += " DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                where += " )";
            }

            return BLL.EmployeeAgent.Instance.GetUserCountByGroup(where);
        }

    }
}