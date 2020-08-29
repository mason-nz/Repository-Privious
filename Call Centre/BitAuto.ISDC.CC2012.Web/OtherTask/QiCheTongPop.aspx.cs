using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.OtherTask
{
    public partial class QiCheTongPop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //其他任务ID
        protected string TaskID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TaskID"]) == true ? "" : HttpContext.Current.Request["TaskID"].ToString();
            }
        }
        
        //手机号码
        protected string pop_Phone
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["phone"]) == true ? "" : HttpContext.Current.Request["phone"].ToString();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
        }
    }
}