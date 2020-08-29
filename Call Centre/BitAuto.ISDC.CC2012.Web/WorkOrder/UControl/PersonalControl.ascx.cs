using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.UControl
{
    public partial class PersonalControl : System.Web.UI.UserControl
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
        //IM系统：isIM
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

        public string RequestFrom
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("From");
            }
        }

        public string RequestTagID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("TagID");
            }
        }
        public string RequestTagName
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("TagName");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}