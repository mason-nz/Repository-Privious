using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder
{
    public partial class GroupOrderList : PageBase
    {
        #region 属性
        private string RequestCustName  //客户姓名
        {
            get { return HttpContext.Current.Request["CustName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString()); }
        }
        private string RequestOrderID //订单编号
        {
            get { return HttpContext.Current.Request["OrderID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["OrderID"].ToString()); }
        }
        private string RequestTaskID  //任务id
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        private string ProvinceID
        {
            get { return HttpContext.Current.Request["ProvinceID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProvinceID"].ToString()); }
        }
        private string CityID
        {
            get { return HttpContext.Current.Request["CityID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CityID"].ToString()); }
        }
        private string RequestDealPerson //处理人
        {
            get { return HttpContext.Current.Request["DealPerson"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DealPerson"].ToString()); }
        }
        private string RequestDealer //经销商
        {
            get { return HttpContext.Current.Request["Dealer"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Dealer"].ToString()); }
        }
        private string RequestTaskStatus //任务状态
        {
            get { return HttpContext.Current.Request["TaskStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskStatus"].ToString()); }
        }
        private string RequestIsReturnVisit //处理状态
        {
            get { return HttpContext.Current.Request["IsReturnVisit"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsReturnVisit"].ToString()); }
        }
        private string RequestCreatBeginTime //下单日期 （开始时间）
        {
            get { return HttpContext.Current.Request["CreatBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreatBeginTime"].ToString()); }
        }
        private string RequestCreatEndTime //任务创建时间 （结束时间）
        {
            get { return HttpContext.Current.Request["CreatEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreatEndTime"].ToString()); }
        }
        private string RequestSubBeginTime //任务提交时间 （开始时间）
        {
            get { return HttpContext.Current.Request["SubBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SubBeginTime"].ToString()); }
        }
        private string RequestSubEndTime //任务提交时间 （结束时间）
        {
            get { return HttpContext.Current.Request["SubEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SubEndTime"].ToString()); }
        }

        private string Reason
        {
            get { return HttpContext.Current.Request["Reason"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Reason"].ToString()); }
        }
        private string ddlArea
        {
            get { return HttpContext.Current.Request["ddlArea"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ddlArea"].ToString()); }
        }

        //任务类型，0是无主订单，不等于0是免费订单
        private string TaskType
        {
            get { return HttpContext.Current.Request["TaskType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskType"].ToString()); }
        }

        //为选择分页记录数
        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int CurrentUserId;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUserId = BLL.Util.GetLoginUserID();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                if (!int.TryParse(RequestPageSize, out PageSize))
                {
                    PageSize = 20;
                }

                QueryGroupOrderTask query = BLL.Util.BindQuery<QueryGroupOrderTask>(this.Context);

                int _userID = BLL.Util.GetLoginUserID();
                query.LoginID = _userID;
                DataTable dt = BLL.GroupOrderTask.Instance.GetGroupOrderTask(query, "gorder.OrderCreateTime desc", PageCommon.Instance.PageIndex, PageSize, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();

                litPagerDown.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, PageCommon.Instance.PageIndex, 2);
            }
            catch (Exception ex)
            {

            }
        }


        //操作
        public string getOperator(string taskID, string status, string AssignUserID)
        {
            //分配
            bool AssignTask = false;
            //回收
            bool RevokeTask = false;
            //当前登录人
            int userid = BLL.Util.GetLoginUserID();

            AssignTask = BLL.Util.CheckButtonRight("SYS024BUT1205");
            RevokeTask = BLL.Util.CheckButtonRight("SYS024BUT1206");
            string operatorStr = string.Empty;
            if (!string.IsNullOrEmpty(taskID) && !string.IsNullOrEmpty(status))
            {
                //未分配
                if (Convert.ToInt32(status) == (int)GroupTaskStatus.NoAllocation)
                {
                    if (AssignTask == true)
                    {
                        operatorStr += "<a href='javascript:void(0)' onclick='javascript:AssignTaskOne(" + taskID + ");'  class='linkBlue'>分配</a>";
                    }
                }
                //未处理
                else if (Convert.ToInt32(status) == (int)GroupTaskStatus.NoProcess || Convert.ToInt32(status) == (int)GroupTaskStatus.Processing)
                {
                    if (userid.ToString() == AssignUserID)
                    {
                        operatorStr += "<a href='../../GroupOrder/GroupOrderDeal.aspx?TaskID=" + taskID + "&Guid=" + Guid.NewGuid().ToString("N") + "' class='linkBlue' target='_bank'>处理</a>";
                    }

                }
                else if (Convert.ToInt32(status) == (int)GroupTaskStatus.Processed)
                {
                    operatorStr += "<a href='../../GroupOrder/GroupOrderView.aspx?TaskID=" + taskID + "&Guid=" + Guid.NewGuid().ToString("N") + "' class='linkBlue' target='_bank'>查看</a>";
                }

            }
            return operatorStr;
        }
        //链接到客户查看页URL
        public string getCustomerLink(string custID, string custName, string status)
        {
            string operatorStr = string.Empty;

            //已处理
            //if (Convert.ToInt32(status) == (int)GroupTaskStatus.Processed && !string.IsNullOrEmpty(custID))
            if (!string.IsNullOrEmpty(custID))
            {
                //operatorStr = "<a href='../../GroupOrder/GroupOrderView.aspx?TaskID=" + custID + "' class='linkBlue' target='_bank'>" + custName + "</a>";
                operatorStr = "<a href='../../TaskManager/CustInformation.aspx?CustID=" + custID + "' class='linkBlue' target='_bank'>" + custName + "</a>";
            }
            else
            {
                operatorStr = custName;
            }
            return operatorStr;
        }
    }
}