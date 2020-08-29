using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class UCCstMemberUCount : System.Web.UI.UserControl
    {
        public int MemberID;
        public string OriginalCstMemberID;
        protected string UCount = "";
        protected string lastAddUbTime = "";
        protected string UbTotalAmount = "";
        protected string UbTotalExpend = "";
        protected string activeTime = "";
        protected string productOpenKey = "";
        protected string syncTime = "";
        public UCCstMemberUCount()
        {
        }
        public UCCstMemberUCount(int memberId)
            : this()
        {
            this.MemberID = memberId;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetProductOpenKey();
                Entities.ProjectTask_CSTMember info = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberModel(this.MemberID);
                if (info != null)
                {
                    string cstRecId = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectCstMemberIDByCSTRecID(info.OriginalCSTRecID);
                    int recId = -1;
                    if (int.TryParse(cstRecId, out recId))
                    {
                        BitAuto.YanFa.Crm2009.Entities.CstMember cstMemberInfo = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(info.OriginalCSTRecID);
                        OriginalCstMemberID = cstMemberInfo.CSTRecID;
                        this.syncTime = cstMemberInfo.SyncTime.ToShortDateString();
                    }
                    BitAuto.YanFa.Crm2009.Entities.CSTExpandInfo dstCSTExpandInfo = BitAuto.YanFa.Crm2009.BLL.CSTExpandInfo.Instance.GetModelByCSTRecID(info.OriginalCSTRecID);
                    if (dstCSTExpandInfo != null)
                    {
                        if (dstCSTExpandInfo.UCount != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            this.UCount = dstCSTExpandInfo.UCount.ToString();
                        }
                        if (dstCSTExpandInfo.LastAddUbTime != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.DATE_INVALID_VALUE)
                        {
                            this.lastAddUbTime = dstCSTExpandInfo.LastAddUbTime.ToShortDateString();
                        }
                        if (dstCSTExpandInfo.UBTotalAmount != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            this.UbTotalAmount = dstCSTExpandInfo.UBTotalAmount.ToString();
                        }
                        if (dstCSTExpandInfo.UBTotalExpend != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.INT_INVALID_VALUE)
                        {
                            this.UbTotalExpend = dstCSTExpandInfo.UBTotalExpend.ToString();
                        }
                        if (dstCSTExpandInfo.ActiveTime != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.DATE_INVALID_VALUE)
                        {
                            this.activeTime = dstCSTExpandInfo.ActiveTime.ToShortDateString();
                        }
                    }
                }
            }
        }
        public string CstMemberID()
        {
            Entities.ProjectTask_CSTMember info = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberModel(this.MemberID);
            if (info != null)
            {
                return BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectCstMemberIDByCSTRecID(info.OriginalCSTRecID);
            }
            return string.Empty;
        }
        //获取引用链接所需要的“Key”
        private void GetProductOpenKey()
        {
            this.productOpenKey = ConfigurationManager.AppSettings["CSTMemberServiceKey"].ToString();
        }
    }
}