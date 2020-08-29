using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;
using System.Configuration;
using BitAuto.DSC.IM_2015.WebService.CC;
namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class OnlineMessageData : System.Web.UI.Page
    {
        #region 属性
        private string UsertName
        {
            get
            {
                return HttpContext.Current.Request["UsertName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UsertName"].ToString());
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
        private string TypeID
        {
            get
            {
                return HttpContext.Current.Request["TypeID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TypeID"].ToString());
            }
        }
        private string SourceType
        {
            get
            {
                return HttpContext.Current.Request["SourceType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SourceType"].ToString());
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

        public int RecordCount;
        public string WorkOrderUrl = ConfigurationManager.AppSettings["WorkOrderUrl"];
        public string WorkOrderViewUrl = ConfigurationManager.AppSettings["WorkOrderViewUrl"];
        public string WorkOrderViewUrlNew = ConfigurationManager.AppSettings["WorkOrderViewUrlNew"];
        #endregion

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
            query.UsertName = UsertName == "" ? Constant.STRING_INVALID_VALUE : UsertName;
            query.Status = MessageState == "" ? Constant.INT_INVALID_VALUE : int.Parse(MessageState);
            query.TypeID = TypeID == "" ? Constant.INT_INVALID_VALUE : int.Parse(TypeID);
            query.SourceType = SourceType == "" ? Constant.INT_INVALID_VALUE : int.Parse(SourceType);
            query.QueryStarttime = QueryStarttime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryStarttime).ToString();
            query.QueryEndTime = QueryEndTime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryEndTime).AddDays(1).ToString();
            query.LastModifyUserName = LastModifyUserName == "" ? Constant.STRING_INVALID_VALUE : LastModifyUserName;
            query.UserID = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.UserMessage.Instance.GetUserMessage(query, "um.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            Rt_CSData.DataSource = dt;
            Rt_CSData.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //备注显示
        public string RemarkMenuContrl(string RecID, string strStatus, string objRemarks)
        {
            if (strStatus == "3" && objRemarks == "")
            {
                return "无备注";
            }
            if (strStatus == "3" && objRemarks != "")
            {
                return "<a href=\"#\" title=\"" + objRemarks + "\" onclick=\"javascript:OpenAddRemarkLayer(" + RecID + ",'sel')\" >" + (objRemarks.ToString().Length > 8 ? objRemarks.ToString().Substring(0, 8) + "……" : objRemarks.ToString()) + "</a>";
            }
            if (strStatus != "3" && objRemarks == "")
            {
                return "<a  href=\"#\" onclick=\"javascript:OpenAddRemarkLayer(" + RecID + ",'add')\">点击添加</a>";
            }
            if (strStatus != "3")
            {
                return "<a href=\"#\" title=\"" + objRemarks + "\" onclick=\"javascript:OpenAddRemarkLayer(" + RecID + ",'upd')\">" + (objRemarks.ToString().Length > 8 ? objRemarks.ToString().Substring(0, 8) + "……" : objRemarks.ToString()) + "</a>";
            }
            return objRemarks;
        }

        //工单显示
        //public string WorkOrderMenuContrl(string strOrderID, string strStatus, string strPhone, string strUserName, string RecID)
        //{

        //    string str = string.Empty;
        //    if (strStatus == "3" && string.IsNullOrEmpty(strOrderID))
        //    {
        //        str = "无工单";
        //    }
        //    else if (!string.IsNullOrEmpty(strOrderID))
        //    {
        //        str = " <a href=\"" + WorkOrderViewUrl + "?OrderID=" + HttpUtility.UrlEncode(strOrderID) + "\" target=\"_blank\">" + strOrderID + "</a> ";
        //    }
        //    else if (strStatus != "3")
        //    {
        //        str = " <a href=\"" + WorkOrderUrl + "?IsClientOpen=1&SYSType=isIM2&ctype=4&CallType=3"
        //            + "&CalledNum=" + HttpUtility.UrlEncode(strPhone, System.Text.Encoding.UTF8)
        //            + "&CustName=" + HttpUtility.UrlEncode(strUserName, System.Text.Encoding.UTF8)
        //            + "&LYID=" + HttpUtility.UrlEncode(RecID, System.Text.Encoding.UTF8)
        //            + "\" target=\"_blank\" >点击添加</a> ";
        //    }
        //    return str;
        //}
        //工单显示
        public string WorkOrderMenuContrl(string strOrderID, string strStatus, string strPhone, string strUserName, string RecID)
        {

            string str = string.Empty;
            if (strStatus == "3" && string.IsNullOrEmpty(strOrderID))
            {
                str = "无工单";
            }
            else if (!string.IsNullOrEmpty(strOrderID))
            {
                //根据工单编号位数区分新老工单。
                string urlView = WorkOrderViewUrlNew;
                if (strOrderID.Length != 17)
                {
                    urlView = WorkOrderViewUrl;
                }
                str = " <a href=\"" + urlView + "?OrderID=" + HttpUtility.UrlEncode(strOrderID) + "\" target=\"_blank\">" + strOrderID + "</a> ";
            }
            else if (strStatus != "3")
            {
                //str = " <a href=\"" + WorkOrderUrl + "?IsClientOpen=1&SYSType=isIM2&ctype=4&CallType=3"
                //    + "&CalledNum=" + HttpUtility.UrlEncode(strPhone, System.Text.Encoding.UTF8)
                //    + "&CustName=" + HttpUtility.UrlEncode(strUserName, System.Text.Encoding.UTF8)
                //    + "&LYID=" + HttpUtility.UrlEncode(RecID, System.Text.Encoding.UTF8)
                //    + "\" target=\"_blank\" >点击添加</a> ";

                //调用cc添加工单接口
                string workOrderUrl = string.Empty;
                BLL.Loger.Log4Net.Info(string.Format("调用CC个人版添加工单接口，参数callNum:{0},csID:{1},custName:{2},cbsex:{3},cbprovinceID:{4},cbCityID:{5},cbCounty:{6},businessTypeID:{7},tagID:{8}", strPhone, strPhone, strUserName, -1, -1, -1, -1, -1, -1));
                workOrderUrl = CCWebServiceHepler.Instance.CCDataInterface_GetAddWOrderComeIn_IMGR_URL(strPhone, RecID, strUserName, -1, -1, -1, -1, -1, -1, 2);
                BLL.Loger.Log4Net.Info(string.Format("调用CC个人版添加工单接口,返回值：{0}", workOrderUrl));
                str = " <a href=\"" + workOrderUrl + "\" target=\"blank\">点击添加</a> ";
            }
            return str;
        }

        //操作显示
        public string GetOptions(string strStatus)
        {
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

        //获取咨询类型
        public string GetTypeName(string tyepID)
        {
            return BLL.UserMessage.Instance.GetTypeName(tyepID);
        }

        //获取业务线来源名称
        public string GetSourceTypeName(string sourceTypeID)
        {
            var list = BLL.Util.GetAllSourceType(true);
            var sourceTypeInfo = list.FirstOrDefault(i => i.SourceTypeValue == sourceTypeID);
            return sourceTypeInfo == null ? "" : sourceTypeInfo.SourceTypeName;
        }
    }
}