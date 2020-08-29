using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization
{
    public partial class FrequencyStatistics : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private int userID;
        public bool IsExport = false;
        public string startTime;
        public string endTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                startTime = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                endTime = DateTime.Now.ToString("yyyy-MM-dd");
                IsExport = BLL.Util.CheckButtonRight("SYS024MOD6314");

               // BindSel();
            }
        }

        private void BindSel()
        {
            throw new NotImplementedException();
        }
    }
}