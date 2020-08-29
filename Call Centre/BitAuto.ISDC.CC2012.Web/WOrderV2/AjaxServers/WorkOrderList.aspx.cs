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

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers
{
    public partial class WorkOrderList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        int userId = 0;
        public int totalCount = 0;
        public int PageSize = 20;
        public int GroupLength = 8;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userId = BLL.Util.GetLoginUserID();
                WorkOrderBind();
            }
        }

        private void WorkOrderBind()
        {
            QueryWOrderV2DataInfo query = BLL.Util.BindQuery<QueryWOrderV2DataInfo>(this.Context);
            query.CC_LoginID = userId;
            string orderstring = "a.WorkOrderStatus ASC,a.CreateTime DESC";
            DataTable dt = BLL.WOrderInfo.Instance.GetWorkOrderInfoForList(query, orderstring, PageCommon.Instance.PageIndex, PageSize, out totalCount);
            rptWorkOrderList.DataSource = dt;
            rptWorkOrderList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        public string GetOperStr(string orderId, int workOrderStatus, int lastrecid, int createUserId)
        {
            string msg = "";
            WOrderOperTypeEnum wOrderOperTypeEnum = BLL.WOrderProcess.Instance.ValidateWOrderProcessRight(orderId, workOrderStatus, lastrecid, createUserId, ref msg);
            string enumStr = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WOrderOperTypeEnum), (int)wOrderOperTypeEnum);
            if (wOrderOperTypeEnum == WOrderOperTypeEnum.None)
            {
                return "--";
            }
            string operStr = "<a href='/WOrderV2/WOrderProcess.aspx?OrderID=" + orderId + "'  target='_blank'>" + enumStr + "</a>";
            return operStr;
        }
    }
}