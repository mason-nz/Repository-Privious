using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ReturnVisit
{
    public partial class CustInfoShow : Page
    {
        private string custId;
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID
        {
            get
            {
                if (custId == null)
                {
                    custId = HttpUtility.UrlDecode((Request["CustID"] + "").Trim());
                }
                return custId;
            }
        }
        public string CustName = string.Empty;
        public int LoginUserID = 0;
        public int BGID = 0;//分组ID
        public int SCID = 0;//分类ID

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                BitAuto.YanFa.Crm2009.Entities.CustInfo ci = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustID);
                if (ci != null)
                {
                    CustName = ci.CustName;

                    this.UCCust1.CustInfo = ci;
                }
                LoginUserID = BLL.Util.GetLoginUserID();
                BGID = BitAuto.ISDC.CC2012.BLL.SurveyCategory.Instance.GetSelfBGIDByUserID(LoginUserID);
                SCID = BitAuto.ISDC.CC2012.BLL.SurveyCategory.Instance.GetSelfSCIDByUserID(LoginUserID);
            }
        }
    }
}