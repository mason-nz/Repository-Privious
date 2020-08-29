using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.BLL;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;
using System.Data;
using System.Configuration;
using BitAuto.DSC.IM_DMS2014.WebService.CC;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.ContentManage
{
    public partial class OnlineMessageData : System.Web.UI.Page
    {
        private string MemberName
        {
            get
            {
                return HttpContext.Current.Request["MemberName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MemberName"].ToString());
            }
        }
        private string District
        {
            get
            {
                return HttpContext.Current.Request["District"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["District"].ToString());
            }
        }
        private string CityGroup
        {
            get
            {
                return HttpContext.Current.Request["CityGroup"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CityGroup"].ToString());
            }
        }
        private string LastModifyUserName
        {
            get
            {
                return HttpContext.Current.Request["LastModifyUserName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["LastModifyUserName"].ToString());
            }
        }
        private string MessageState
        {
            get
            {
                return HttpContext.Current.Request["MessageState"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MessageState"].ToString());
            }
        }
        private string QueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString());
            }
        }
        private string QueryEndTime
        {
            get
            {
                return HttpContext.Current.Request["QueryEndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryEndTime"].ToString());
            }
        }


        public int RecordCount;
        public string WorkOrderUrl = ConfigurationManager.AppSettings["WorkOrderUrl"];
        public string WorkOrderViewUrl = ConfigurationManager.AppSettings["WorkOrderViewUrl"];
        public string WorkOrderViewUrlNew = ConfigurationManager.AppSettings["WorkOrderViewUrlNew"];
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {

            RecordCount = 0;
            QueryUserMessage query = new QueryUserMessage();
            query.MemberName = MemberName == "" ? Constant.STRING_INVALID_VALUE : MemberName;
            query.DistrictID = District == "" ? Constant.STRING_INVALID_VALUE : District;

            query.LastModifyUserName = LastModifyUserName == "" ? Constant.STRING_INVALID_VALUE : LastModifyUserName;
            query.Status = MessageState == "" ? Constant.INT_INVALID_VALUE : int.Parse(MessageState);
            query.QueryStarttime = QueryStarttime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryStarttime).ToString();
            query.QueryEndTime = QueryEndTime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryEndTime).AddDays(1).ToString();
            query.AgentID = BLL.Util.GetLoginUserID();


            DataTable dt = BLL.UserMessage.Instance.GetUserMessage(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            Rt_CSData.DataSource = dt;
            Rt_CSData.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
        }

        public string RemarkMenuContrl(string RecID, string hasRemarks, string strStatus, object objRemarks)
        {
            string str = string.Empty;
            if (hasRemarks == "Y")
            {
                if (strStatus == "3")
                {
                    if (objRemarks != null)
                    {
                        str = "<a href=\"#\" title=\"" + objRemarks + "\" onclick=\"javascript:OpenAddRemarkLayer(" + RecID + ",'sel')\" >" + (objRemarks.ToString().Length > 8 ? objRemarks.ToString().Substring(0, 8) + "……" : objRemarks.ToString()) + "</a>";
                    }
                    else
                    {
                        str = "无备注";
                    }
                }
                else
                {
                    if (objRemarks != null)
                    {
                        str = "<a href=\"#\" title=\"" + objRemarks + "\" onclick=\"javascript:OpenAddRemarkLayer(" + RecID + ",'upd')\">" + (objRemarks.ToString().Length > 8 ? objRemarks.ToString().Substring(0, 8) + "……" : objRemarks.ToString()) + "</a>";
                    }
                    else
                    {
                        str = str = "无备注";
                    }
                }
            }
            else if (hasRemarks == "N")
            {
                if (strStatus == "3")
                {
                    str = "无备注";
                }
                else
                {
                    str = "<a  href=\"#\" onclick=\"javascript:OpenAddRemarkLayer(" + RecID + ",'add')\">点击添加</a>";
                }
            }
            return str;
        }
        public string WorkOrderMenuContrl(object strOrderID, object strHasWorkOrder, object strStatus1, object strPhone1, object strUserName, object strMemberCode, object strMemberName, object RecID)
        {
            string OrderID = (strOrderID == null) ? "" : strOrderID.ToString();
            string hasWorkOrder = (strHasWorkOrder == null) ? "" : strHasWorkOrder.ToString();
            string strStatus = (strStatus1 == null) ? "" : strStatus1.ToString();
            string strPhone = (strPhone1 == null) ? "" : strPhone1.ToString();
            string UserName = (strUserName == null) ? "" : strUserName.ToString();

            string MemberCode = (strMemberCode == null) ? "" : strMemberCode.ToString();
            string MemberName = (strMemberName == null) ? "" : strMemberName.ToString();
            string strRecID = (RecID == null) ? "" : RecID.ToString();

            string str = string.Empty;
            if (hasWorkOrder == "Y")
            {
                //根据工单编号位数区分新老工单。
                string urlView = WorkOrderViewUrlNew;
                if (OrderID.Length != 17)
                {
                    urlView = WorkOrderViewUrl;
                }

                str = " <a href=\"" + urlView + "?OrderID=" + HttpUtility.UrlEncode(OrderID) + "\" target=\"_blank\">" + OrderID + "</a> ";
            }
            else if (hasWorkOrder == "N")
            {
                if (strStatus == "3")
                {
                    str = "无工单";
                }
                else
                {
                    //str = " <a href=\"" + WorkOrderUrl + "?IsClientOpen=1&SYSType=isIM&CarType=2&ctype=3&CallType=IM"
                    //    + "&CalledNum=" + HttpUtility.UrlEncode(strPhone, System.Text.Encoding.UTF8)
                    //    + "&CustName=" + HttpUtility.UrlEncode(UserName, System.Text.Encoding.UTF8)
                    //    + "&MemberCode=" + HttpUtility.UrlEncode(MemberCode, System.Text.Encoding.UTF8)
                    //    + "&MemberName=" + HttpUtility.UrlEncode(MemberName, System.Text.Encoding.UTF8)
                    //    + "&LYID=" + HttpUtility.UrlEncode(strRecID, System.Text.Encoding.UTF8)
                    //    + "\" target=\"_blank\" >点击添加</a> ";

                    //调用cc添加工单接口
                    string workURL = string.Empty;
                    BLL.Loger.Log4Net.Info(string.Format("调用CC经销商二手车版添加工单接口，参数callNum:{0},csID:{1},custName:{2},cbsex:{3},cbprovinceID:{4},cbCityID:{5},cbCounty:{6},memberCode:{7}", strPhone, strRecID, UserName, -1, -1, -1, -1, MemberCode));
                    workURL = CCWebServiceHepler.Instance.CCDataInterface_GetAddWOrderComeIn_IMJXS_SC_URL(strPhone, strRecID, UserName, -1, -1, -1, -1, MemberCode,2);
                    BLL.Loger.Log4Net.Info(string.Format("调用CC经销商二手车车版添加工单接口,返回值：{0}", workURL));
                    //CustName,CsId,CallNum,MemberCode
                    str = "<a href=\"" + workURL + "\"  target=\"blank\" >点击添加</a>";
                }
            }

            return str;
        }
        public string GetOptions(string strStatus)
        {
            //<option>新建</option><option>处理</option><option>已完成</option>
            string str = string.Empty;
            if (strStatus == "1")
            {
                str = "<option value='1'>新建</option><option value='2'>处理中</option><option value='3'>已完成</option>";
            }
            else if (strStatus == "2")
            {
                str = "<option value='2'>处理中</option><option value='3'>已完成</option>";
            }
            else if (strStatus == "3")
            {
                str = "<option value='3'>已完成</option>";
            }
            return str;
        }


    }
}