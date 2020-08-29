using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class CooperationProjectList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int CurrentPage
        {
            get
            {
                return this.AjaxPagerCooperationProjects.CurrentPage;
            }
        }

        public int PageSize
        {
            get
            {
                return this.AjaxPagerCooperationProjects.PageSize;
            }
        }

        public string CustID
        {
            get { return Request["CustID"] + ""; }
        }

        public string DepartID
        {
            get { return Session["departid"].ToString(); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dataBind();
                //增加 合作项 的页面权限
                 //int userId = BLL.Util.GetLoginUserID();
                 //if (BLL.Util.CheckRight(userId, "SYS024BUT2202"))
                 //{
                 //    dataBind();
                 //}
                 //else
                 //{
                 //    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                 //    Response.End();
                 //}
            }
        }

        private string GetUseStyleName(string UseStyle)
        {
            string temp = string.Empty;
            switch (UseStyle.Trim())
            {
                case "4001": temp = "销售"; break;
                case "4002": temp = "互换"; break;
                case "4003": temp = "配送"; break;
                case "4004": temp = "自用"; break;
                case "4005": temp = "试用"; break;
                default: break;
            }
            return temp;
        }

        /// <summary>
        /// Modify=2012-08-23 Masj 更改查询数据逻辑
        /// </summary>
        private void dataBind()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("saletype", typeof(string));
            dt.Columns.Add("period", typeof(string));
            dt.Columns.Add("createtime", typeof(DateTime));
            dt.Columns.Add("note", typeof(string));

            WebService.CRM.CRMCooperationInfo service = new WebService.CRM.CRMCooperationInfo();
            DataSet ds = service.GetCooperationByCustID(CustID);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt_edm = ds.Tables["edm"];
                if (dt_edm != null)
                {
                    for (int i = 0; i < dt_edm.Rows.Count; i++)
                    {
                        var iRow = dt_edm.Rows[i];
                        DataRow dr = dt.NewRow();
                        dr["type"] = "杂志直投";
                        dr["name"] = iRow["CooperationName"];
                        dr["saletype"] = "";
                        dr["period"] = iRow["ExecCycle"];
                        dr["createtime"] = iRow["CreateTime"];
                        dr["note"] = iRow["Remark"];
                        dt.Rows.Add(dr);
                    }
                }
                DataTable dt_order = ds.Tables["order"];
                if (dt_order != null)
                {
                    for (int i = 0; i < dt_order.Rows.Count; i++)
                    {
                        var iRow = dt_order.Rows[i];
                        DataRow dr = dt.NewRow();
                        dr["type"] =  iRow["OrderTypeName"];
                        dr["name"] = iRow["MemberCode"] + "（" + iRow["AdDateCode"] + "）";
                        dr["saletype"] =  iRow["UseStyle"];
                        dr["period"] = iRow["ExecPeriod"];
                        dr["createtime"] = iRow["CreateTime"];
                        dr["note"] = iRow["ProductName"];
                        dt.Rows.Add(dr);
                    }
                }

            }
             

            DataTable dt_page = new DataTable();
            dt_page = dt.Clone();
            int showCount = PageSize * CurrentPage >= dt.Rows.Count ? dt.Rows.Count : PageSize * CurrentPage;
            for (int k = PageSize * (CurrentPage - 1); k < showCount; k++)
            {
                dt_page.ImportRow(dt.Rows[k]);
            }

            repeater.DataSource = dt_page;
            repeater.DataBind();

            AjaxPagerCooperationProjects.PageSize = 5;
            AjaxPagerCooperationProjects.InitPager(dt.Rows.Count);
        }

        #region GetPagedTable DataTable分页
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns></returns>
        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0)
                return dt;
            DataTable newdt = dt.Copy();
            newdt.Clear();

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }
        #endregion
    }
}