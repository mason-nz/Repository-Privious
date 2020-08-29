using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager
{
    public partial class EditRoleInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID
        {
            get { return Request.QueryString["roleID"] + ""; }
        }
        string _sysid;
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SysID
        {
            get
            {
                _sysid = Request.QueryString["sysID"] == null ? "" : Request.QueryString["sysID"];
                return _sysid;
            }
            set { _sysid = value; }
        }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string ShowType
        {
            get { return Request.QueryString["ShowType"] + ""; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindSysDDL();
                setTitle();
                BindData();
            }
        }
        private void setTitle()
        {
            if (ShowType == "add")
            {
                litTitle.Text = "添加";
            }
            else if (ShowType == "updata")
            {
                litTitle.Text = "修改";
            }
        }
        ///// <summary>
        ///// 绑定系统的下拉列表
        ///// </summary>
        //public  void BindSysDDL()
        //{
        //    DataTable dt = SysInfo.Instance.GetSysInfoAll();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        dllEditSysID.DataSource = dt;
        //        dllEditSysID.DataValueField = "SysID";
        //        dllEditSysID.DataTextField = "SysName";
        //        dllEditSysID.DataBind();
        //    }
        //    dllEditSysID.Items.Insert(0, new ListItem("请选择系统", "-1"));
        //}
        /// <summary>
        /// 绑定要编辑的数据
        /// </summary>
        public void BindData()
        {
            RoleInfo roleInfo = null;
            if (RoleID != "")
            {
                roleInfo = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(RoleID);
            }
            if (roleInfo != null)
            {
                txtEditRoleName.Value = roleInfo.RoleName;
                //dllEditSysID.SelectedValue = roleInfo.SysID;
                //ddlRoleType.Value = roleInfo.RoleType.ToString();
                SysID = roleInfo.SysID;
                txtEditIntro.Text = roleInfo.Intro;
            }
        }
    }
}