using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.GroupOrder
{
    public partial class GroupOrderList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //导入Excel回写
        public bool ImportExcelWriteBack = false;

        //qizq add 分配任务
        public bool AssignTask = false;

        /// <summary>
        /// 收回任务
        /// </summary>
        public bool RevokeTask = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                AssignTask = BLL.Util.CheckButtonRight("SYS024BUT1205");
                RevokeTask = BLL.Util.CheckButtonRight("SYS024BUT1206");
                ImportExcelWriteBack = BLL.Util.CheckButtonRight("SYS024BUT1207");
                //绑定处理人
                ddlDealPersonBind();
                //绑定任务状态
                BindTaskStatus();
                //绑定处理状态
                BindIsReturnVisit();
                //绑定失败理由
                BindingFailReason();
            }
        }

        private void BindTaskStatus()
        {
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.GroupTaskStatus));
            ddlTaskStatus.DataValueField = "value";
            ddlTaskStatus.DataTextField = "name";
            ddlTaskStatus.DataSource = dt;
            ddlTaskStatus.DataBind();
            ddlTaskStatus.Items.Insert(0, new ListItem("请选择", "-2"));
        }

        private void BindIsReturnVisit()
        {
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.IsReturnVisit));
            ddlIsReturnVisit.DataValueField = "value";
            ddlIsReturnVisit.DataTextField = "name";
            ddlIsReturnVisit.DataSource = dt;
            ddlIsReturnVisit.DataBind();
            ddlIsReturnVisit.Items.Insert(0, new ListItem("请选择", "-2"));
        }

        private void BindingFailReason()
        {
            DataTable dt = BLL.GO_FailureReason.Instance.GetAll();
            ddlFailReason.DataValueField = "RecID";
            ddlFailReason.DataTextField = "ReasonName";
            ddlFailReason.DataSource = dt;
            ddlFailReason.DataBind();
            ddlFailReason.Items.Insert(0, new ListItem("请选择", "-2"));
        }

        /// <summary>
        /// 绑定处理人
        /// </summary>
        private void ddlDealPersonBind()
        {
            ddlDealPerson.DataSource = BLL.GroupOrderTask.Instance.GetDealPerson();
            ddlDealPerson.DataTextField = "Name";
            ddlDealPerson.DataValueField = "UserID";
            ddlDealPerson.DataBind();
            ddlDealPerson.Items.Insert(0, new ListItem("请选择", "-2"));
        }
        

    }
}