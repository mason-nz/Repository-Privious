using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.AjaxServers
{
    public partial class WorkOrderList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string RequestOrderCreateTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("OrderCreateTime"); }
        }
        public bool canPend = false;
        public bool canHandler = false;
        public bool canAdd = false;
        public bool canExport = false;
        //public bool canAddReturnVisit = false;
        int userId = 0;
        public int totalCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                canPend = BLL.Util.CheckButtonRight("SYS024BUG100601");
                canHandler = BLL.Util.CheckButtonRight("SYS024BUG100602");
                canAdd = BLL.Util.CheckButtonRight("SYS024BUG100604");
                canExport = BLL.Util.CheckButtonRight("SYS024BUG100603");
                //canAddReturnVisit = BLL.Util.CheckButtonRight("SYS024BUG100605 ");
                userId = BLL.Util.GetLoginUserID();
                WorkOrderBind();
            }
        }

        private void WorkOrderBind()
        {
            QueryWorkOrderInfo query = BLL.Util.BindQuery<QueryWorkOrderInfo>(this.Context);
            if (query.WorkCategory < 0)
            {
                canExport = false;
            }
            query.LoginID = userId;
            string orderstring = "";
            if (RequestOrderCreateTime == "1")
            {
                orderstring = " OrderNum ASC,CreateTime Desc,LastProcessDate ASC";
            }
            else
            {
                orderstring = " OrderNum ASC,LastProcessDate ASC,CreateTime Desc";
            }
            DataTable dt = BLL.WorkOrderInfo.Instance.GetWorkOrderInfoForList(query, orderstring, PageCommon.Instance.PageIndex, PageCommon.Instance.PageSize, out totalCount);


            rptWorkOrderList.DataSource = dt;
            rptWorkOrderList.DataBind();
            this.AjaxPager.InitPager(totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            int id = 0;
            string userName = string.Empty;
            if (int.TryParse(userId, out id))
            {
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(id);
            }

            return userName;
        }

        public string GetTitleStyle(string title, string lastProcessDate, string WorkOrderStatus)
        {
            DateTime date;
            if (DateTime.TryParse(lastProcessDate, out date))
            {
                if (date.Add(new TimeSpan(23, 59, 59)) < DateTime.Now && int.Parse(WorkOrderStatus) < (int)Entities.WorkOrderStatus.Processed)
                {
                    title = "<span style='color:red;'>" + title + "</span>";
                }
            }
            return title;
        }

        public string GetOperStr(int workOrderStatus, int reveiverId, int createUserId, string orderId)
        {
            string operStr = string.Empty;
            bool isOper = isOperater();
            if (workOrderStatus == (int)Entities.WorkOrderStatus.Pending && canPend)
            {
                operStr = "<a href='/WorkOrder/CCProcess.aspx?OrderID=" + orderId + "&r=" + new Random().Next() + "' target='_blank'>审核<a>";
            }
            else if ((workOrderStatus == (int)Entities.WorkOrderStatus.Untreated ||
                workOrderStatus == (int)Entities.WorkOrderStatus.Processing) && (canHandler || reveiverId == userId || createUserId == userId))
            {
                if (isOper)
                {
                    operStr = "<a href='/WorkOrder/SalesProcess.aspx?OrderID=" + orderId + "&r=" + new Random().Next() + "' target='_blank'>处理<a>";
                }
                else
                {

                    DataTable dt = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetUserInfoByUserID(userId);
                    string[] departIds = ConfigurationManager.AppSettings["YichePartID"].Split(',');
                    DataTable dtAllDeparts = BLL.WorkOrderInfo.Instance.GetChildDepartMent(departIds);
                    foreach (DataRow departRow in dtAllDeparts.Rows)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (departRow[0] != null && row["DepartID"] != null && departRow[0].ToString() == row["DepartID"].ToString())
                            {
                                operStr = "<a href='/WorkOrder/CCProcess.aspx?OrderID=" + orderId + "&r=" + new Random().Next() + "' target='_blank'>处理<a>";
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(operStr))
                        {
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(operStr))
                    {
                        operStr = "<a href='/WorkOrder/SalesProcess.aspx?OrderID=" + orderId + "&r=" + new Random().Next() + "' target='_blank'>处理<a>";
                    }

                }

            }

            return operStr;
        }

        //判断是否是运营客服
        private bool isOperater()
        {
            BitAuto.YanFa.Crm2009.Entities.UserInfo user = BitAuto.YanFa.Crm2009.BLL.UserInfo.Get(userId);
            //运营客服
            return (user.UserClass == 2 || user.UserClass == 7) ? false : true;
        }

    }
}