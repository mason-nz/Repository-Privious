using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder
{
    public partial class MainList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {


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

                AssignTask = BLL.Util.CheckButtonRight("SYS024BUT1202");
                RevokeTask = BLL.Util.CheckButtonRight("SYS024BUT1203");
                ddlDealPersonBind();
                BindingR();
                ddlAreaBind();
                //selReasonPersonBind();
            }
        }

        private void BindingR()
        {
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.NoDealerReason));
            selReason.DataValueField = "value";
            selReason.DataTextField = "name";
            selReason.DataSource = dt;
            selReason.DataBind();
            selReason.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        /// <summary>
        /// 绑定处理人
        /// </summary>
        private void ddlDealPersonBind()
        {
            ddlDealPerson.DataSource = BLL.OrderTask.Instance.GetDealPerson();
            ddlDealPerson.DataTextField = "Name";
            ddlDealPerson.DataValueField = "UserID";
            ddlDealPerson.DataBind();
            ddlDealPerson.Items.Insert(0, new ListItem("请选择", "-1"));
        }
        ///// <summary>
        /////绑定不推荐经销商的理由
        ///// </summary>
        //private void selReasonPersonBind()
        //{
        //    ddlDealPerson.Items.Clear();
        //    ddlDealPerson.Items.Add(new ListItem(BLL.Util.GetEnumOptText(typeof(Entities.NoDealerReason), (int)Entities.NoDealerReason.Reason1), ((int)Entities.NoDealerReason.Reason1).ToString()));
        //    ddlDealPerson.Items.Add(new ListItem(BLL.Util.GetEnumOptText(typeof(Entities.NoDealerReason), (int)Entities.NoDealerReason.Reason2), ((int)Entities.NoDealerReason.Reason2).ToString()));
        //    ddlDealPerson.Items.Add(new ListItem(BLL.Util.GetEnumOptText(typeof(Entities.NoDealerReason), (int)Entities.NoDealerReason.Reason3), ((int)Entities.NoDealerReason.Reason3).ToString()));
        //    ddlDealPerson.Items.Add(new ListItem(BLL.Util.GetEnumOptText(typeof(Entities.NoDealerReason), (int)Entities.NoDealerReason.Reason4), ((int)Entities.NoDealerReason.Reason4).ToString()));
        //    ddlDealPerson.Items.Insert(0, new ListItem("请选择", "-1"));
        //}
        private void ddlAreaBind()
        {
            ddlArea.DataSource = BLL.Util.GetEnumDataTable(typeof(Entities.EnumArea));
            ddlArea.DataTextField = "name";
            ddlArea.DataValueField = "value";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("请选择", "-1"));
        }

    }
}