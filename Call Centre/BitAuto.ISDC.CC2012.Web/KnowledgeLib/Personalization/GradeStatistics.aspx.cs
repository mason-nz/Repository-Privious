using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    public partial class GradeStatistics : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private int userID;
        public bool IsExport = false;
        public string startTime;
        public string endTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                IsExport = BLL.Util.CheckButtonRight("SYS024MOD6315");

                startTime = DateTime.Now.ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");

                int userid = BLL.Util.GetLoginUserID();
                //所属分组
                DataTable bgdt = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userid);

                //添加请选择
                DataRow dr = bgdt.NewRow();
                dr[0] = "-1";
                dr[1] = "请选择";
                bgdt.Rows.InsertAt(dr, 0);

                //绑定数据
                ddlBussiGroup.DataSource = bgdt;
                ddlBussiGroup.DataTextField = "Name";
                ddlBussiGroup.DataValueField = "BGID";
                ddlBussiGroup.DataBind();
            }
        }
    }
}