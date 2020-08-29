using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class ReturnMemberDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string DealerId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["DealerID"]))
                {
                    return Request["DealerID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DealerId))
            {
                Response.Write(@"<script language='javascript'>alert('参数错误！');try {
                  window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
            }
            else
            {
                BitAuto.YanFa.Crm2009.Entities.DMSMember DMSModel = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(DealerId);
                if (DMSModel != null)
                {
                    string DealerName = DMSModel.Name;
                    string MemberID = DMSModel.ID.ToString();
                    string CustID = DMSModel.CustID;
                    Response.Redirect("../../../CustCheck/CrmCustSearch/MemberDetail.aspx?MemberID=" + MemberID + "&CustID=" + CustID);
                }
                else
                {
                    Response.Write(@"<script language='javascript'>alert('参数错误！');try {
                  window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
            }
        }
    }
}