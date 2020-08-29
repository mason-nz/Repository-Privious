using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Web.Base;
using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager
{
    public partial class ModuleInfoList : PageBase
    {
        #region Page_Load
        public string RequestModuleName
        {
            get { return Request.QueryString["ModuleName"] == null ? string.Empty : Request.QueryString["ModuleName"].ToString().Trim(); }
        }
        public string RequestSysID
        {
            get { return Request.QueryString["SysID"] == null ? "-1" : Request.QueryString["SysID"].ToString().Trim(); }
        }
        public string RequestModuleID
        {
            get { return Request.QueryString["ModuleID"] == null ? "-1" : Request.QueryString["ModuleID"].ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBinds();
            }
        }
        #endregion

        #region Functional
        //绑定数据
        private void DataBinds()
        {
            //查询
            int totalCount;
            DataTable table = Retrieve(out totalCount);
            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                //设置分页控件 
                repeater.DataSource = table;
            }
            ajaxPager.InitPager(totalCount, "divContent");
            //绑定列表数据
            repeater.DataBind();
        }

        private DataTable Retrieve(out int totalCount)
        {
            //查询
            QueryModuleInfo moduleInfo = new QueryModuleInfo();
            if (RequestModuleName != string.Empty)
            {
                moduleInfo.ModuleName = Server.UrlDecode(RequestModuleName);
                txtSeachModuleName.Value = Server.UrlDecode(RequestModuleName);
            }
            if (RequestSysID != string.Empty && RequestSysID != "-1")
            {
                moduleInfo.SysID = RequestSysID;
            }
            if (RequestModuleID != string.Empty && RequestModuleID != "-1")
            {
                moduleInfo.PID = RequestModuleID;
            }
            else
            {
                moduleInfo.PID = "";
            }

            DataTable dt = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(moduleInfo, "OrderNum", PageIndex, PageSize, out totalCount);

            return dt;
        }
        #endregion
    }
}