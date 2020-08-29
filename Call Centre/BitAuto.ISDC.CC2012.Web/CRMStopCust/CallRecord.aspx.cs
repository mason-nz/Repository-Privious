using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.CustInfo;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class CallRecord : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TaskID
        {
            get
            {
                return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"]);
            }
        }
        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        OrderCRMStopCustTaskInfo task;
        string custId = string.Empty;
        private void BindData()
        {
            if (string.IsNullOrEmpty(TaskID))
            {
                return;
            }
            else
            {
                task = BLL.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(TaskID);
                if (task == null)
                {
                    return;
                }
            }
            QueryCallRecordInfo query = new QueryCallRecordInfo();
            //按照任务查询话务
            query.TaskTypeID = (int)Entities.ProjectSource.S2_客户核实;
            query.TaskID = this.TaskID;
            query.LoginID = -1;
            int totalCount = 0;
            string tableEndName = ""; //查询现在表数据
            DataTable dt = BLL.CallRecordInfo.Instance.GetCallRecordInfo(query, "c.CreateTime desc", 1, -1, tableEndName, out totalCount);
            //设置数据源
            if (dt != null && dt.Rows.Count > 0)
            {
                repeater_Contact.DataSource = dt;
            }
            //绑定列表数据
            repeater_Contact.DataBind();
        }
    }
}