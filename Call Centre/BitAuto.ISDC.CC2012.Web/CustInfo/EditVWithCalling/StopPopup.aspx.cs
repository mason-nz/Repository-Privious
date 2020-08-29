using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class StopPopup : BitAuto.ISDC.CC2012.Web.Base.PageBase
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}