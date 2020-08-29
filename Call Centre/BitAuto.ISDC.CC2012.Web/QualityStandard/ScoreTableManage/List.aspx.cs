using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityScoring.ScoreTableManage
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public bool right_export = false;
        public bool right_range = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                 right_export = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT600204");
                 right_range = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT600205");
            }
        } 

    }
}