using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Text;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class ChatHistoryExport : System.Web.UI.Page
    {
        #region 属性
        private string ReqVisitorName
        {
            get
            {
                return HttpContext.Current.Request["ep_VisitorName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_VisitorName"].ToString());
            }
        }
        private string ReqPhone
        {
            get
            {
                return HttpContext.Current.Request["ep_Phone"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_Phone"].ToString());
            }
        }
        private string ReqProvinceID
        {
            get
            {
                return HttpContext.Current.Request["ep_ProvinceID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_ProvinceID"].ToString());
            }
        }
        private string ReqCityID
        {
            get
            {
                return HttpContext.Current.Request["ep_CityID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_CityID"].ToString());
            }
        }
        private string ReqAgentName
        {
            get
            {
                return HttpContext.Current.Request["ep_AgentName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_AgentName"].ToString());
            }
        }
        private string ReqOrderID
        {
            get
            {
                return HttpContext.Current.Request["ep_OrderID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_OrderID"].ToString());
            }
        }
        private string ReqQueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["ep_QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_QueryStarttime"].ToString());
            }
        }
        private string ReqQueryEndTime
        {
            get
            {
                return HttpContext.Current.Request["ep_QueryEndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ep_QueryEndTime"].ToString());
            }
        }

        #endregion

        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExportData();
            }
        }
        private void ExportData()
        {
            RecordCount = 0;

            QueryConversations query = new QueryConversations();
            if (!string.IsNullOrEmpty(ReqVisitorName))
            {
                query.VisitorName = ReqVisitorName;
            }
            if (!string.IsNullOrEmpty(ReqPhone))
            {
                query.VisitorPhone = ReqPhone;
            }
            int provinceID = Constant.INT_INVALID_VALUE;
            int cityID = Constant.INT_INVALID_VALUE;
            if (int.TryParse(ReqProvinceID, out provinceID))
            {
                query.VisitorProvinceID = provinceID;
            }
            if (int.TryParse(ReqCityID, out cityID))
            {
                query.VisitorCityID = cityID;
            }
            if (!string.IsNullOrEmpty(ReqAgentName))
            {
                query.UserName = ReqAgentName;
            }
            if (!string.IsNullOrEmpty(ReqOrderID))
            {
                query.OrderID = ReqOrderID;
            }
            DateTime dtStart = Constant.DATE_INVALID_VALUE;
            DateTime dtEnd = Constant.DATE_INVALID_VALUE;
            if (DateTime.TryParse(ReqQueryStarttime, out dtStart))
            {
                query.QueryStarttime = dtStart;
            }
            if (DateTime.TryParse(ReqQueryEndTime, out dtEnd))
            {
                query.QueryEndTime = dtEnd.AddSeconds(1);
            }

            query.UserID = BLL.Util.GetLoginUserID();


            DataTable dt = BLL.Conversations.Instance.ExportCSDataForLiaoTianJiLu(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 999999, out RecordCount);

            DataColumn newCol = new DataColumn("ChatHistory");
            dt.Columns.Add(newCol);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["ChatHistory"] = GetChatHistoryData(row["CSID"].ToString());

                    DateTime newdate = Constant.DATE_INVALID_VALUE; ;
                    object obj = BLL.ConverSationDetail.Instance.GetSourceTypeValue(row["VisitID"].ToString());
                    if (obj != null)
                    {
                        row["VisitID"] = BLL.Util.GetSourceTypeName(obj.ToString());
                    }
                }

                dt.Columns.Remove("RowNumber");
                dt.Columns.Remove("CSID");


                dt.Columns["VisitorName"].SetOrdinal(0);
                dt.Columns["VisitorType"].SetOrdinal(1);
                dt.Columns["Sex"].SetOrdinal(2);
                dt.Columns["ProvinceCityName"].SetOrdinal(3);
                dt.Columns["VisitorPhone"].SetOrdinal(4);

                dt.Columns["Remark"].SetOrdinal(5);
                dt.Columns["VisitID"].SetOrdinal(6);
                dt.Columns["UserReferTitle"].SetOrdinal(7);
                dt.Columns["ConversationStartTime"].SetOrdinal(8);
                dt.Columns["AgentStartTime"].SetOrdinal(9);

                dt.Columns["EndTime"].SetOrdinal(10);
                dt.Columns["Duration"].SetOrdinal(11);
                dt.Columns["TrueName"].SetOrdinal(12);
                dt.Columns["AgentNum"].SetOrdinal(13);
                dt.Columns["PerSatisfaction"].SetOrdinal(14);

                dt.Columns["ProSatisfaction"].SetOrdinal(15);
                dt.Columns["SatisfactionContents"].SetOrdinal(16);
                dt.Columns["ChatHistory"].SetOrdinal(17);
                dt.Columns["OrderID"].SetOrdinal(18);


                dt.Columns["VisitorName"].ColumnName = "访客姓名";
                dt.Columns["VisitorType"].ColumnName = "客户分类";
                dt.Columns["Sex"].ColumnName = "性别";
                dt.Columns["ProvinceCityName"].ColumnName = "地区";
                dt.Columns["VisitorPhone"].ColumnName = "电话";

                dt.Columns["Remark"].ColumnName = "备注";
                dt.Columns["VisitID"].ColumnName = "访客来源";
                dt.Columns["UserReferTitle"].ColumnName = "发起页面";
                dt.Columns["ConversationStartTime"].ColumnName = "对话开始时间";
                dt.Columns["AgentStartTime"].ColumnName = "应答时间";

                dt.Columns["EndTime"].ColumnName = "结束时间";
                dt.Columns["Duration"].ColumnName = "对话时长(秒)";
                dt.Columns["TrueName"].ColumnName="客服";
                dt.Columns["AgentNum"].ColumnName = "工号";
                dt.Columns["PerSatisfaction"].ColumnName = "对客服评价";

                dt.Columns["ProSatisfaction"].ColumnName = "对产品评价";
                dt.Columns["SatisfactionContents"].ColumnName = "满意度留言";
                dt.Columns["ChatHistory"].ColumnName = "本次对话内容";
                dt.Columns["OrderID"].ColumnName = "任务ID";

                BLL.Util.ExportToSCV("聊天记录" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }


            #region

            //QueryInComingCallDetails query = new QueryInComingCallDetails();
            //query.StartTime = this.StartTime;
            //query.EndTime = this.EndTime;
            //query.AgentID = newAgentIDS;
            //query.BusinessType = this.BusinessType;
            //query.QueryArea = this.QueryArea;
            //query.LoginUserId = BLL.Util.GetLoginUserID();

            //query.QueryType = "4";
            //DataTable dtSum = BLL.CallRecord_ORIG.Instance.GetInComingCallData(query, 1, 1, out RecordCount);

            //query.EndTime = this.EndTime;
            //query.QueryArea = this.QueryArea;
            //query.QueryType = this.QueryType;
            //DataTable dt = BLL.CallRecord_ORIG.Instance.GetInComingCallData(query, 1000000, 1, out RecordCount);

            //if (dt != null)
            //{
            //    dt.Columns.Remove("RowNumber");

            //    dt.Columns["StartTime"].SetOrdinal(0);
            //    dt.Columns["TrueName"].SetOrdinal(1);
            //    dt.Columns["AgentNum"].SetOrdinal(2);
            //    dt.Columns["N_CallIsQuantity"].SetOrdinal(3);
            //    dt.Columns["T_RingingTime"].SetOrdinal(4);
            //    dt.Columns["T_TalkTime"].SetOrdinal(5);
            //    dt.Columns["T_AfterworkTime"].SetOrdinal(6);
            //    dt.Columns["T_SetLogin"].SetOrdinal(7);
            //    dt.Columns["P_WorkTimeUse"].SetOrdinal(8);
            //    dt.Columns["A_AverageRingTime"].SetOrdinal(9);
            //    dt.Columns["A_AverageTalkTime"].SetOrdinal(10);
            //    dt.Columns["A_AfterworkTime"].SetOrdinal(11);
            //    dt.Columns["T_SetBuzy"].SetOrdinal(12);
            //    dt.Columns["N_SetBuzy"].SetOrdinal(13);
            //    dt.Columns["A_AverageSetBusy"].SetOrdinal(14);
            //    dt.Columns["N_TransferOut"].SetOrdinal(15);
            //    dt.Columns["N_TransferIn"].SetOrdinal(16);

            //    dt.Columns["StartTime"].ColumnName = "日期";
            //    dt.Columns["TrueName"].ColumnName = "客服";
            //    dt.Columns["AgentNum"].ColumnName = "工号";
            //    dt.Columns["N_CallIsQuantity"].ColumnName = "电话总接通量";
            //    dt.Columns["T_RingingTime"].ColumnName = "总振铃时长";
            //    dt.Columns["T_TalkTime"].ColumnName = "总通话时长";
            //    dt.Columns["T_AfterworkTime"].ColumnName = "总话后时长";
            //    dt.Columns["T_SetLogin"].ColumnName = "总在线时长";
            //    dt.Columns["P_WorkTimeUse"].ColumnName = "工时利用率";
            //    dt.Columns["A_AverageRingTime"].ColumnName = "平均振铃时长(秒)";
            //    dt.Columns["A_AverageTalkTime"].ColumnName = "平均通话时长(秒)";
            //    dt.Columns["A_AfterworkTime"].ColumnName = "平均话后时长(秒)";
            //    dt.Columns["T_SetBuzy"].ColumnName = "置忙总时长";
            //    dt.Columns["N_SetBuzy"].ColumnName = "置忙次数";
            //    dt.Columns["A_AverageSetBusy"].ColumnName = "平均置忙时长(秒)";
            //    dt.Columns["N_TransferOut"].ColumnName = "电话转出次数";
            //    dt.Columns["N_TransferIn"].ColumnName = "电话转入次数";




            //    BLL.Util.ExportToSCV("呼入报表" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            //}

            #endregion
        }

        private void GetCSData()
        {
            int RecordCount = 0;

            QueryConversations query = new QueryConversations();
            if (!string.IsNullOrEmpty(ReqVisitorName))
            {
                query.VisitorName = ReqVisitorName;
            }
            if (!string.IsNullOrEmpty(ReqPhone))
            {
                query.VisitorPhone = ReqPhone;
            }
            int provinceID = Constant.INT_INVALID_VALUE;
            int cityID = Constant.INT_INVALID_VALUE;
            if (int.TryParse(ReqProvinceID, out provinceID))
            {
                query.VisitorProvinceID = provinceID;
            }
            if (int.TryParse(ReqCityID, out cityID))
            {
                query.VisitorCityID = cityID;
            }
            if (!string.IsNullOrEmpty(ReqAgentName))
            {
                query.UserName = ReqAgentName;
            }
            if (!string.IsNullOrEmpty(ReqOrderID))
            {
                query.OrderID = ReqOrderID;
            }
            DateTime dtStart = Constant.DATE_INVALID_VALUE;
            DateTime dtEnd = Constant.DATE_INVALID_VALUE;
            if (DateTime.TryParse(ReqQueryStarttime, out dtStart))
            {
                query.QueryStarttime = dtStart;
            }
            if (DateTime.TryParse(ReqQueryEndTime, out dtEnd))
            {
                query.QueryEndTime = dtEnd;
            }

            query.UserID = BLL.Util.GetLoginUserID();


            DataTable dt = BLL.Conversations.Instance.GetCSData(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 9999999, out RecordCount);
        }

        public string GetChatHistoryData(string CSID)
        {
            QueryConversations query = new QueryConversations();
            query.CSID = CSID == "" ? Constant.INT_INVALID_VALUE : int.Parse(CSID);

            int recCount = 0;
            DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "", 1, 999999, out recCount);
            DataColumn col1 = new DataColumn("newName", typeof(string));
            dt.Columns.Add(col1);

            StringBuilder strChatInfo = new StringBuilder("");
            foreach (DataRow row in dt.Rows)
            {
                switch (row["Sender"].ToString())
                {
                    case "1":
                        //row["newName"] = "客服" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】";
                        strChatInfo.Append("客服" + row["AgentNum"].ToString() + "【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】：" + row["Content"] + "\r\n");
                        break;
                    case "2":
                        //row["newName"] = row["UserName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】";
                        strChatInfo.Append(row["UserName"].ToString() + "【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】：" + row["Content"] + "\r\n");
                        break;
                    default:
                        //row["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】";
                        strChatInfo.Append("系统消息【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】：" + row["Content"] + "\r\n");
                        break;
                }
            }
            return strChatInfo.ToString();

        }
    }
}