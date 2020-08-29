using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class View : PageBase
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID = string.Empty;

        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null
                           ? ""
                           : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"]);
            }
        }

        private string DeleteMemberID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断登录
            if (!IsPostBack)
            {
                GetInfoByTaskID();

                BitAuto.YanFa.Crm2009.Entities.CustInfo ci = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustID);
                if (ci != null)
                {
                    this.UCCust1.CustInfo = ci;
                    this.UCCust1.DeleteMemberID = this.DeleteMemberID;
                }
            }
        }

        private void GetInfoByTaskID()
        {
            Entities.OrderCRMStopCustTaskInfo model_task = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(TaskID);
            if (model_task != null)
            {
                Entities.StopCustApply model_cust = BLL.StopCustApply.Instance.GetStopCustApply((int)model_task.RelationID);
                if (model_cust != null)
                {
                    DeleteMemberID = model_cust.DeleteMemberID;
                    CustID = model_cust.CustID;
                }
            }
        }
    }
}