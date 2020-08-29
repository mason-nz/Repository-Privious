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
    public partial class ExclusiveMissedCallsList : PageBase
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

        private string AgentID
        {
            get
            {
                return Request["AgentID"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentID"].ToString().Trim());
            }
        }
        private string AgentNum
        {
            get
            {
                return Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentNum"].ToString().Trim());
            }
        }

        private string AgentGroup
        {
            get
            {
                return Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentGroup"].ToString().Trim());
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
            ExclusiveMissedCalls query = new ExclusiveMissedCalls();
            query.ANI = ANI;
            query.BeginTime = DateTimeToString(BeginTime, "00:00:00");
            query.EndTime = DateTimeToString(EndTime, "23:59:59");
            query.AgentNum = AgentNum;
            query.AgentID = AgentID;
            if (AgentGroup != "" && CommonFunction.ObjectToInteger(AgentGroup)>0)
            {
                query.AgentGroup = AgentGroup;
            }
           
            DataTable dt = BLL.CustomerVoiceMsg.Instance.GetExclusiveMissedCallingsData(query, "a.StartTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
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
        public string GetLinkOper(string status_nm, string recid, string tel, string datasource, string orderid, string workcategory, string workorderstatus)
        {
            string info = "";
            if (status_nm == "待处理" || status_nm == "处理中")
            {
                //编辑状态 处理
                info += "<span id='span1_" + recid + "' style='display:inline-block'><a href=\"javascript:void(0);\" onclick=\"ModifyStatus('" + recid + "')\">编辑状态</a>&nbsp;&nbsp;</span>";
                info += "<span id='span2_" + recid + "' style='display:none'><a href=\"javascript:void(0);\" onclick=\"SaveStatus('" + recid + "')\">保存</a>&nbsp;&nbsp;";
                info += "<a href=\"javascript:void(0);\" onclick=\"CancelStatus('" + recid + "')\">取消</a>&nbsp;&nbsp;</span>";
                //没有工单，需要处理按钮
                if (string.IsNullOrEmpty(orderid))
                {
                    info += "<span id='span_cl_" + recid + "' style='display:inline-block'><a href=\"javascript:void(0);\" onclick=\"ProcessData('" + recid + "','" + tel + "','" + datasource + "',this)\">添加工单</a>&nbsp;&nbsp;</span>";
                }
            }
            //存在工单时，显示查看链接
            if (!string.IsNullOrEmpty(orderid))
            {
                string url = "";
                string name = "";
                //个人-查看
                if (workcategory == "1")
                {
                    url = "/WorkOrder/WorkOrderView.aspx?OrderID=" + orderid;
                    name = "查看";
                }
                //经销商
                if (workcategory == "2")
                {
                    //经销商-处理
                    if ((status_nm == "待处理" || status_nm == "处理中") && (workorderstatus == "2" || workorderstatus == "3"))//2:待处理 3:处理中
                    {
                        url = "/WorkOrder/SalesProcess.aspx?OrderID=" + orderid + "&r=" + new Random().NextDouble();
                        name = "处理";
                    }
                    //经销商-查看
                    else
                    {
                        url = "/WorkOrder/WorkOrderView.aspx?OrderID=" + orderid;
                        name = "查看";
                    }
                }
                info += "<span id='span_ck_" + recid + "' style='display:inline-block' orderid='" + orderid + "'><a href='" + url + "' target='_blank'>" + name + "</a>&nbsp;&nbsp;</span>";
            }
            return info;
        }

        public string DateTimeToString(string value)
        {
            DateTime date = CommonFunction.ObjectToDateTime(value, new DateTime());
            if (date == new DateTime())
            {
                return "";
            }
            else
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}