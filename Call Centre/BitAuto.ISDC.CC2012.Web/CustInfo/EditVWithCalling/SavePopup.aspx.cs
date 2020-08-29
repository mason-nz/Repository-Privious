using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class SavePopup : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID
        {
            get
            {
                return (Request["TID"] + "").Trim();
            }
        }

        /// <summary>
        /// 弹出框名称
        /// </summary>
        public string PopupName
        {
            get
            {
                string str = (Request["PopupName"] + "").Trim();
                if (string.IsNullOrEmpty(str)) { str = "AnonymousPopup"; }
                return str;
            }
        }

        protected string AdditionalStatus = "";
        protected string AdditionalStatusDescription = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(TID))
                {
                    BLL.ProjectTaskInfo.Instance.GetTaskAdditionalStatus(TID, out this.AdditionalStatus, out this.AdditionalStatusDescription);
                }
            }
        }
    }
}