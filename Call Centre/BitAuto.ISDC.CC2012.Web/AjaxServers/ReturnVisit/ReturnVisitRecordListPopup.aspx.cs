using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class ReturnVisitRecordListPopup : PageBase
    {
        #region 定义属性

        //public string RequestPageSize
        //{
        //    get { return Request.QueryString["pageSize"] == null ? PageCommon.Instance.PageSize.ToString() : Request.QueryString["pageSize"].Trim(); }
        //}
        public string RequestAction
        {
            get { return BLL.Util.GetCurrentRequestFormStr("Action"); }
        }
        public string RequestCustID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustID"); }
        }
        public string CustName = string.Empty;
        public int PageSize = 20;
        private int userID = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (RequestCustID != string.Empty)
                {
                    BindCustName(RequestCustID);
                    BindData(RequestCustID);
                }
            }
        }

        private void BindData(string custID)
        {
            DataTable dt = null;
            int count = 0;
            if (RequestAction.ToLower().Equals("viewcrmreturnvisit"))//查看CRM系统中所有回访记录
            {
                BitAuto.YanFa.Crm2009.Entities.QueryReturnVisit QueryReturnVisit = new BitAuto.YanFa.Crm2009.Entities.QueryReturnVisit();
                QueryReturnVisit.CustID = custID;
                dt = BitAuto.YanFa.Crm2009.BLL.ReturnVisit.Instance.GetReturnVisit(QueryReturnVisit, PageCommon.Instance.PageIndex, PageSize, out count);
            }
            else
            {
                Entities.QueryCallRecordInfo query = new Entities.QueryCallRecordInfo();
                query.CustID = custID;
                dt = BLL.CallRecordInfo.Instance.GetCC_CallRecordsByRV(query, PageCommon.Instance.PageIndex, PageSize, out count);
            }

            repeaterList.DataSource = dt;
            repeaterList.DataBind();
            AjaxPager_Custs.InitPager(count, "divReturnVisitRecordListPopup", PageSize);
        }

        private void BindCustName(string custID)
        {
            BitAuto.YanFa.Crm2009.Entities.CustInfo model = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(custID);
            if (model != null)
            {
                CustName = model.CustName;
            }
        }

        protected string getTypeStr(string type)
        {
            string RVType = "";
            switch (type)
            {
                case "1":
                    RVType = "短信联系";
                    break;
                case "2":
                    RVType = "电话联系";
                    break;
                case "3":
                    RVType = "发送传真";
                    break;
                case "4":
                    RVType = "电子邮件";
                    break;
                case "5":
                    RVType = "信件邮递";
                    break;
                case "6":
                    RVType = "一般会议";
                    break;
                case "7":
                    RVType = "上门拜访";
                    break;
                case "8":
                    RVType = "网络即时通信";
                    break;
            }
            return RVType;
        }

        protected string getVisitTypeStr(string type)
        {
            return BitAuto.YanFa.Crm2009.BLL.DictInfo.Instance.GetDictNameByDictID(type);
        }
    }
}