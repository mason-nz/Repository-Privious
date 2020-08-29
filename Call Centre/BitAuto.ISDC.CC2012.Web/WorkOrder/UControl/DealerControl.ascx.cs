using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.UControl
{
    public partial class DealerControl : System.Web.UI.UserControl
    {
        //IM系统：经销商版IM,车辆类型：1新车，2二手车
        public string RequestCarType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CarType");
            }
        }
        //访问系统在型
        //IM系统：isIM经销商，isIM2个人
        public string RequestSYSType
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SYSType");
            }
        }
        //IM添加工单传过来的会话ID
        public string RequestCSID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CSID");
            }
        }
        //IM添加工单传过来的留言表ID
        public string RequestLYID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LYID");
            }
        }
        public string RequestIsClientOpen
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("IsClientOpen");
            }
        }
        public string RequestCalledNum
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CalledNum");
            }
        }

        public string RequestFrom
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("From");
            }
        }

        protected string today = DateTime.Now.ToString("yyyy-MM-dd");
        protected string days7Before = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
        public string logmsg = "";

        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string dcCRMCustID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindMember();
            }
        }
        protected void bindMember()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            if (!string.IsNullOrEmpty(dcCRMCustID))
            {
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectByCustID(dcCRMCustID);
                popSelMemberName.DataSource = dt;
                popSelMemberName.DataTextField = "FullName";
                popSelMemberName.DataValueField = "MemberCode";
                popSelMemberName.DataBind();
            }

            logmsg = "（已" + sw.Elapsed.TotalSeconds.ToString("0.00") + "s）；";
            sw.Stop();
        }
    }
}