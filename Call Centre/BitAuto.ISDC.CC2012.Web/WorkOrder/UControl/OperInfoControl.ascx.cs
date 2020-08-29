using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder.UControl
{
    public partial class OperInfoControl : System.Web.UI.UserControl
    {
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string OrderID = string.Empty;
        /// <summary>
        /// ViewType:查看状态为1（回复工单）-表示只查看回复信息并按照操作时间倒序
        /// 查看状态为2（操作记录）-表示查看所有信息并按操作时间正序
        /// </summary>
        public int ViewType = 1;

        private string orderByDesc = " w.CreateTime Desc ";
        private string orderByAsc = " w.CreateTime Asc ";

        public Entities.WorkOrderRevert modelRevert = new Entities.WorkOrderRevert();


        protected string isEstablish = string.Empty;  //是否接通
        protected string notEstablishReason = string.Empty; //接通后失败原因

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["OrderID"] != null)
                {
                    OrderID = Request["OrderID"].ToString();
                }
                if (Request["ViewType"] != null)
                {
                    int vt = 0;
                    if (int.TryParse(Request["ViewType"].ToString(), out vt))
                    {
                        ViewType = int.Parse(Request["ViewType"].ToString());
                    }
                }

                BindData();
            }
        }

        private void BindData()
        {
            if (OrderID != string.Empty)
            {
                GetEstablishStr();
                string orderBy = ViewType == 1 ? orderByDesc : orderByAsc;
                DataTable dt = BLL.WorkOrderRevert.Instance.GetWorkOrderRevertByOrderID(OrderID, orderBy);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int worid;
                    if (ViewType == 2)
                    {
                        worid = int.Parse(dt.Rows[0]["RecID"].ToString());
                        dt.Rows.RemoveAt(0);

                        modelRevert = BLL.WorkOrderRevert.Instance.GetWorkOrderRevert(worid);

                        Entities.WorkOrderInfo info = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                        hlDemandDetails.Text = info.DemandID;
                        hlDemandDetails.NavigateUrl = ConfigurationManager.AppSettings["DemandDetailsUrl"] + "?DemandID=" + info.DemandID + "&r=" + new Random().Next();
                        hlDemandDetails.Target = "_blank";

                        if (string.IsNullOrEmpty(hlDemandDetails.Text))
                        {
                            liDemand.Visible = false;
                        }
                        else
                        {
                            liDemand.Visible = true;
                        }
                    }
                }
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
        }
        public string LinkUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return "<img src='/Images/callTel.png' style='position: relative; top: 4px; left: 2px;cursor:pointer;' alt='' href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + url + "\")' />";
            }
            return "";
        }

        public string GetLi(string CategoryName, string WorkOrderStatus, string ReceiverName, string PriorityLevelName, string TagName, string IsComplaintType)
        {
            string li = string.Empty;

            if (ViewType == 1)
            {
                return li;
            }

            li += "<ul class='clearfix'>";

            if (!string.IsNullOrEmpty(CategoryName))
            {
                li += "<li><label>工单分类：</label><span>" + CategoryName + "</span></li>";
            }
            if (!string.IsNullOrEmpty(IsComplaintType))
            {
                li += "<li><label>工单类型：</label><span>" + IsComplaintType + "</span></li>";
            }
            if (!string.IsNullOrEmpty(WorkOrderStatus))
            {
                li += "<li><label>工单状态：</label><span>" + WorkOrderStatus + "</span></li>";
            }
            if (!string.IsNullOrEmpty(ReceiverName))
            {
                li += "<li><label>接收人：</label><span>" + ReceiverName + "</span></li>";
            }
            if (!string.IsNullOrEmpty(PriorityLevelName))
            {
                li += "<li><label>优先级：</label><span>" + PriorityLevelName + "</span></li>";
            }
            if (!string.IsNullOrEmpty(TagName))
            {
                li += "<li style='width: 800px;'><label>标签：</label><span>" + TagName + "</span></li>";
            }
            li += "</ul>";

            return li;
        }

        public string userName(string id)
        {
            string userName = string.Empty;
            int _userID;
            if (int.TryParse(id, out _userID))
            {
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
            }
            return userName;
        }

        /// <summary>
        /// author:anyang
        /// datetime: 2015-11-20
        /// description:根据订单号查找是否接通和未接通原因
        /// </summary>
        private void GetEstablishStr()
        {
            CallResult_ORIG_TaskInfo mod = BLL.CallResult_ORIG_Task.Instance.GetCallResult_ORIG_TaskInfoByBusinessID(OrderID);
            if (mod == null)
            {
                return;
            }
            int? isEstab = mod.IsEstablish;
            int? notEstab = mod.NotEstablishReason;
            if (isEstab == 1)
            {
                isEstablish = "是";
                liIsEstablish.Visible = true;
            }
            else if (isEstab == 0)
            {
                liIsEstablish.Visible = true;
                isEstablish = "否";
                liNotEstablishReason.Visible = true;
                if (notEstab != null)
                {
                    notEstablishReason = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.NotEstablishReason), (int)notEstab);
                }
            }
        }
    }
}