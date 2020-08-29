using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder
{
    public partial class DeleteTask : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        ///  任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                if (HttpContext.Current.Request["TaskID"] != null)
                {
                    return HttpContext.Current.Request["TaskID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
        }
    }
}