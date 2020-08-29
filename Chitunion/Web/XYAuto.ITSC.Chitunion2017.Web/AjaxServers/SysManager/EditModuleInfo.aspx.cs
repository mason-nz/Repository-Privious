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
    public partial class EditModuleInfo : PageBase
    {
        #region Page_Load
        public string ShowType
        {
            get { return Request.QueryString["ShowType"] != null ? Request.QueryString["ShowType"].ToString().Trim() : string.Empty; }
        }
        public string ModuleID
        {
            get { return Request.QueryString["ModuleID"] != null ? Request.QueryString["ModuleID"].ToString().Trim() : string.Empty; }
        }
        public string Pid
        {
            get { return Request.QueryString["Pid"] != null ? Request.QueryString["Pid"].ToString().Trim() : string.Empty; }
        }
        public string SysID
        {
            get { return Request.QueryString["SysID"] != null ? Request.QueryString["SysID"].ToString().Trim() : string.Empty; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setTitle();
                DataBinds();
            }
        }
        #endregion

        #region Functional
        //绑定数据
        private void DataBinds()
        {
            DataTable dtDomain = BLL.SysRight.DomainInfo.Instance.GetDomainInfoBySysID(SysID);
            if (dtDomain != null && dtDomain.Rows.Count > 0)
            {
                selectDomainCode.DataSource = dtDomain;
                selectDomainCode.DataTextField = "name";
                selectDomainCode.DataValueField = "DomainCode";
                selectDomainCode.DataBind();
            }

            if (ModuleID != null && ModuleID.Trim().Length > 0)
            {
                QueryModuleInfo queryModuleInfo = new QueryModuleInfo();
                queryModuleInfo.ModuleID = ModuleID;
                int o;
                DataTable dt = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(queryModuleInfo,string.Empty, 1, 1, out o);

                if (dt != null && dt.Rows.Count > 0)
                {
                    txtModuleName.Value = dt.Rows[0]["ModuleName"].ToString();
                    txtModuleURL.Value = dt.Rows[0]["Url"].ToString();
                    txtModuleID.Value = dt.Rows[0]["ModuleID"].ToString();
                    txtLinks.Text = dt.Rows[0]["Links"].ToString();
                    txtModuleIntro.Text = dt.Rows[0]["Intro"].ToString();
                    txtOrderNum.Value = dt.Rows[0]["OrderNum"].ToString();
                    selectDomainCode.Value = dt.Rows[0]["DomainID"] + "";
                    txtModuleID.Disabled = true;
                    //selBlank.Value = dt.Rows[0]["blank"].ToString();


                }
            }

        }
        private void setTitle()
        {
            if (ShowType == "add")
            {
                litTitle.Text = "添加";
            }
            else if (ShowType == "edit")
            {
                litTitle.Text = "修改";
            }
        }
        #endregion 
    }
}