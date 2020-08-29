using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.OtherTask
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string othertaskid;
        /// <summary>
        /// 其他任务ID
        /// </summary>
        public string OtherTaskID
        {
            get
            {
                //if (othertaskid == null)
                //{
                //    othertaskid = HttpUtility.UrlDecode((Request["OtherTaskID"] + "").Trim());
                //}
                //return othertaskid;
                return "af3e2";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    OtherTaskEdit1.RequestTaskID = OtherTaskID;
            //}
        }
    }
}