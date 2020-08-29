using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class Edit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID = string.Empty;

        public string CRMStopCustApplyID = string.Empty;

        public bool stopStatus = false;

        public string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"]);
            }
        }

        public string op
        {
            get
            {
                return HttpContext.Current.Request["op"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["op"]);
            }
        }

        public string operType = string.Empty;

        public string UserID = "";
        public string BGID = "2";   //数据清洗组
        public string SCID = "";
        public string CustName = string.Empty;
        private string DeleteMemberID = string.Empty;

        public string CRMCustID = "";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断登录
                UserID = BLL.Util.GetLoginUserID().ToString();
                SCID = BLL.SurveyCategory.Instance.GetSCIDByName("客户核实").ToString();
                GetInfoByTaskID();

                BitAuto.YanFa.Crm2009.Entities.CustInfo ci = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustID);
                if (ci != null)
                {
                    CustName = ci.CustName;
                    CRMCustID = ci.CustID;
                    this.UCCust1.CustInfo = ci;
                    this.UCCust1.DeleteMemberID = this.DeleteMemberID;
                }
            }
        }

        private void GetInfoByTaskID()
        {
            OrderCRMStopCustTaskInfo model_task = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(TaskID);
            if (model_task != null)
            {
                Entities.StopCustApply model_cust = BLL.StopCustApply.Instance.GetStopCustApply((int)model_task.RelationID);
                if (model_cust != null)
                {
                    DeleteMemberID = model_cust.DeleteMemberID;
                    CustID = model_cust.CustID;
                    CRMStopCustApplyID = model_cust.CRMStopCustApplyID.ToString();
                    stopStatus = model_cust.StopStatus == (int)Entities.StopCustStopStatus.Pending ? true : false;
                }
            }
            string[] arrayBGID = new string[1];
            arrayBGID[0] = "2";
            //验证权限
            int typeLimit = AjaxServers.CommonFun.judgeLimit((int)model_task.AssignUserID, arrayBGID);
            switch (typeLimit)
            {
                case 3:
                    operType = op;
                    break;
                case 2:
                    stopStatus = false;
                    break;
                case 1: Response.Write(@"<script language='javascript'>alert('您没有该任务的访问权限，无法访问该页面。');try {
                                                           window.external.MethodScript('/browsercontrol/closepagereloadppage');
                                                       }
                                                       catch (e) {
                                                           window.opener = null; window.open('', '_self'); window.close();
                                                       }</script>");
                    return;
            }
        }
    }
}