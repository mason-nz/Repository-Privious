using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class ReturnVisitCustList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 定义属性
        public bool right_btn1;   //分配
        public bool right_btn2;  //回收
        public bool right_btn3;   //批量分配
        private int userID = 0;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                right_btn1 = BLL.Util.CheckRight(userID, "SYS024BUT140101");
                right_btn2 = BLL.Util.CheckRight(userID, "SYS024BUT140102");
                right_btn3 = BLL.Util.CheckRight(userID, "SYS024BUT140103");//批量分配
            }
        }
        #endregion
    }
}