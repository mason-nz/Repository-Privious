using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class CallRecord : PageBase
    {
        public bool right_export = false;
        public string beginTime = string.Empty;
        public string endTime = string.Empty;
        public string RequestGroup
        {
            get { return HttpContext.Current.Request["SelGroup"] == null ? "-2" : HttpContext.Current.Request["SelGroup"].ToString(); }
        }
        public string RequestBeginTime
        {
            get { return HttpContext.Current.Request["BeginTime"] == null ? null : HttpContext.Current.Request["BeginTime"].ToString(); }
        }
        public string RequestEndTime
        {
            get { return HttpContext.Current.Request["EndTime"] == null ? null : HttpContext.Current.Request["EndTime"].ToString(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                right_export = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD40031");

                if (!string.IsNullOrEmpty(RequestBeginTime) && !string.IsNullOrEmpty(RequestEndTime))
                {
                    beginTime = RequestBeginTime;
                    endTime = RequestEndTime;
                }
                else
                {
                    endTime = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");
                    beginTime = DateTime.Now.AddDays(-6).Date.ToString("yyyy-MM-dd HH:mm:ss");

                    //endTime = DateTime.Now.ToString("yyyy-MM-dd ");
                    //beginTime = DateTime.Now.AddDays(-6).Date.ToString("yyyy-MM-dd");
                }
            }
        }
    }
}