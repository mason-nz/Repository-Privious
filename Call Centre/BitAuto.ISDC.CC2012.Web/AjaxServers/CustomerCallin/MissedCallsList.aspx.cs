using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustomerCallin
{
    public partial class MissedCallsList : PageBase
    {
        #region 参数
        private string ANI
        {
            get
            {
                return Request["ANI"] == null ? "" :
                HttpUtility.UrlDecode(Request["ANI"].ToString().Trim());
            }
        }
        private string BeginTime
        {
            get
            {
                return Request["BeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["BeginTime"].ToString().Trim());
            }
        }
        private string EndTime
        {
            get
            {
                return Request["EndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["EndTime"].ToString().Trim());
            }
        }
        private string selBusinessType
        {
            get
            {
                return Request["selBusinessType"] == null ? "" :
                HttpUtility.UrlDecode(Request["selBusinessType"].ToString().Trim());
            }
        }
        private string Agent
        {
            get
            {
                return Request["Agent"] == null ? "" :
                HttpUtility.UrlDecode(Request["Agent"].ToString().Trim());
            }
        }
        private string PRBeginTime
        {
            get
            {
                return Request["PRBeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["PRBeginTime"].ToString().Trim());
            }
        }
        private string PREndTime
        {
            get
            {
                return Request["PREndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["PREndTime"].ToString().Trim());
            }
        }
        private string prStatus
        {
            get
            {
                return Request["prStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["prStatus"].ToString().Trim());
            }
        }
        private string HasSkill
        {
            get
            {
                return Request["HasSkill"] == null ? "" :
                HttpUtility.UrlDecode(Request["HasSkill"].ToString().Trim());
            }
        }
        private string Hasaudio
        {
            get
            {
                return Request["Hasaudio"] == null ? "" :
                HttpUtility.UrlDecode(Request["Hasaudio"].ToString().Trim());
            }
        }
        private string IsExclusive
        {
            get
            {
                return Request["IsExclusive"] == null ? "" :
                HttpUtility.UrlDecode(Request["IsExclusive"].ToString().Trim());
            }
        }
        #endregion

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int Page = BLL.PageCommon.Instance.PageIndex;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            //查询数据
            QueryCustomerVoiceMsg query = new QueryCustomerVoiceMsg();
            query.ANI = ANI;
            query.BeginTime = DateTimeToString(BeginTime, "00:00:00");
            query.EndTime = DateTimeToString(EndTime, "23:59:59");
            query.selBusinessType = selBusinessType;
            query.Agent = Agent;
            query.PRBeginTime = DateTimeToString(PRBeginTime, "00:00:00");
            query.PREndTime = DateTimeToString(PREndTime, "23:59:59");
            query.prStatus = prStatus;
            query.HasSkill = HasSkill;
            query.Hasaudio = Hasaudio;
            query.IsExclusive = IsExclusive;
            DataTable dt = BLL.CustomerVoiceMsg.Instance.GetCustomerVoiceMsgData(query, "a.StartTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        /// 时间处理和转换
        /// <summary>
        /// 时间处理和转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string DateTimeToString(string value, string hh)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            else return CommonFunction.ObjectToDateTime(value).ToString("yyyy-MM-dd") + " " + hh;
        }
        /// 获取操作信息
        /// <summary>
        /// 获取操作信息
        /// </summary>
        /// <param name="status_nm"></param>
        /// <returns></returns>
        public string GetLinkOper(string status_nm, string recid, string tel, string datasource,
            string orderversion, string orderid,
            string old_Category, string old_OrderStatus,
            string new_OrderStatus, string new_LastRecid, string new_createuserid)
        {
            string info = "";
            int v = CommonFunction.ObjectToInteger(orderversion);
            if (status_nm == "待处理" || status_nm == "处理中")
            {
                //编辑状态 处理
                info += "<span id='span1_" + recid + "' style='display:inline-block'><a href=\"javascript:void(0);\" onclick=\"ModifyStatus('" + recid + "')\">编辑状态</a>&nbsp;&nbsp;</span>";
                info += "<span id='span2_" + recid + "' style='display:none'><a href=\"javascript:void(0);\" onclick=\"SaveStatus('" + recid + "')\">保存</a>&nbsp;&nbsp;";
                info += "<a href=\"javascript:void(0);\" onclick=\"CancelStatus('" + recid + "')\">取消</a>&nbsp;&nbsp;</span>";
                //没有工单，需要处理按钮
                if (v == 0)
                {
                    WorkOrderDataSource d = (WorkOrderDataSource)Enum.Parse(typeof(WorkOrderDataSource), CommonFunction.ObjectToInteger(datasource, -1).ToString());
                    BLL.WOrderRequest req = BLL.WOrderRequest.AddWOrderComeIn_MissedCall(CommonFunction.ObjectToInteger(recid), tel, d);
                    info += "<span id='span_cl_" + recid + "' style='display:inline-block'><a href=\"/WOrderV2/AddWOrderInfo.aspx?" +
                        req.ToString() + " \" target='_blank'>添加工单</a>&nbsp;&nbsp;</span>";
                }
            }
            if (v == 1)
            {
                info += Old_Order(status_nm, CommonFunction.ObjectToInteger(recid), orderid, CommonFunction.ObjectToInteger(old_Category), CommonFunction.ObjectToInteger(old_OrderStatus));
            }
            else if (v == 2)
            {
                info += New_Order(status_nm, CommonFunction.ObjectToInteger(recid), orderid, CommonFunction.ObjectToInteger(new_OrderStatus), CommonFunction.ObjectToInteger(new_LastRecid), CommonFunction.ObjectToInteger(new_createuserid));
            }
            return info;
        }

        private string Old_Order(string status_nm, int recid, string orderid, int workcategory, int workorderstatus)
        {
            string url = "";
            string name = "";
            //个人-查看
            if (workcategory == 1)
            {
                url = "/WorkOrder/WorkOrderView.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                name = "查看";
            }
            //经销商
            if (workcategory == 2)
            {
                //经销商-处理
                if ((status_nm == "待处理" || status_nm == "处理中") &&
                    (workorderstatus == (int)WorkOrderStatus.Untreated || workorderstatus == (int)WorkOrderStatus.Processing))
                {
                    url = "/WorkOrder/SalesProcess.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                    name = "处理";
                }
                //经销商-查看
                else
                {
                    url = "/WorkOrder/WorkOrderView.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                    name = "查看";
                }
            }
            return "<span id='span_ck_" + recid + "' style='display:inline-block' orderid='" + orderid + "'>" +
                "<a href='" + url + "' target='_blank'>" + name + "</a>&nbsp;&nbsp;</span>";
        }
        private string New_Order(string status_nm, int recid, string orderid, int workorderstatus, int lastrecid, int createuserid)
        {
            string url = "";
            string name = "";
            //处理
            if (status_nm == "待处理" || status_nm == "处理中")
            {
                string msg = "";
                WOrderOperTypeEnum oper = BLL.WOrderProcess.Instance.ValidateWOrderProcessRight(orderid, workorderstatus, lastrecid, createuserid, ref msg);
                switch (oper)
                {
                    case WOrderOperTypeEnum.L03_审核:
                        name = "审核";
                        url = "/WOrderV2/WOrderProcess.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                        break;
                    case WOrderOperTypeEnum.L04_处理:
                        name = "处理";
                        url = "/WOrderV2/WOrderProcess.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                        break;
                    case WOrderOperTypeEnum.L05_回访:
                        name = "回访";
                        url = "/WOrderV2/WOrderProcess.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                        break;
                    default:
                        name = "查看";
                        url = "/WOrderV2/WorkOrderView.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                        break;
                }
            }
            //查看
            else
            {
                url = "/WOrderV2/WorkOrderView.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                name = "查看";
            }
            return "<span id='span_ck_" + recid + "' style='display:inline-block' orderid='" + orderid + "'>" +
                "<a href='" + url + "' target='_blank'>" + name + "</a>&nbsp;&nbsp;</span>";
        }
    }
}