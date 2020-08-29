using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ConsultManager
{
    //********
    //add user: qizq
    //DateTime: 2012-12-20
    //Remark:把经销商合作，经销商反馈，经销商其他分三个页面查看,此页为经销商其他页
    //********
    public partial class ConsultDCoopOtherView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestRecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }
        private string RequestRecordType
        {
            get { return HttpContext.Current.Request["RecordType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecordType"].ToString()); }
        }
        public string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int recID;
            int count;
            if (int.TryParse(RequestRecID, out recID))
            //&& int.TryParse(RequestType, out type))
            {
                QueryConsultDOther query = new QueryConsultDOther();
                query.RecID = recID;
                DataTable dt = BLL.ConsultDOther.Instance.GetConsultDOther(query, "", 1, 10000, out count);

                if (dt.Rows.Count > 0)
                {
                    ConsultID.InnerText = "经销商其他";
                    RecordType.InnerText = RequestRecordType == "1" ? "呼入" : "呼出";
                    //switch (dt.Rows[0]["QuestionType"].ToString())
                    //{
                    //    case "100001": QuestionType.InnerText = "新车";
                    //        break;
                    //    case "100002": QuestionType.InnerText = "二手车";
                    //        break;
                    //    case "100003": QuestionType.InnerText = "汽车用品周边";
                    //        break;
                    //    case "100004": QuestionType.InnerText = "咨询";
                    //        break;
                    //    case "100005": QuestionType.InnerText = "投诉";
                    //        break;
                    //    case "100006": QuestionType.InnerText = "DSA";
                    //        break;
                    //}
                    CallRecord.InnerText = dt.Rows[0]["CallRecord"].ToString();
                }
            }
        }

        public string ShowCallRecord()
        {
            string returnStr = string.Empty;
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                Entities.CallRecordInfo callRecordInfo = BLL.CallRecordInfo.Instance.GetCallRecordInfoByTaskID(RequestTaskID);
                if (callRecordInfo != null)
                {
                    returnStr = "<a href=\"" + callRecordInfo.AudioURL + "\" style=\"vertical-align:middle\">  <img  src=\"/Images/callTel.png\" border=\"0\" /></a>";
                }
            }
            return returnStr;
        }
    }
}