using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    public partial class QuestionManage : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int RegionID = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userid = BLL.Util.GetLoginUserID();
                EmployeeAgent a = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
                if (a.RegionID.HasValue)
                {
                    RegionID = a.RegionID.Value;
                }
            }
        }
        
    }
}