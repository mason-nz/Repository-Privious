using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ConsultManager
{
    public partial class ConsultPFeedbackView : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
            if (int.TryParse(RequestRecID, out recID))
            {
                ConsultPFeedback model = BLL.ConsultPFeedback.Instance.GetConsultPFeedback(recID);
                if (model != null)
                {
                    ConsultID.InnerText = "个人反馈";
                    RecordType.InnerText = RequestRecordType == "1" ? "呼入" : "呼出";
                    string questionType = string.Empty;
                    string [] array_type=model.QuestionType.Split(',');
                    for (int i = 0; i < array_type.Length; i++)
                    {
                        switch (array_type[i])
                        {
                            case "80001": questionType += "论坛,";
                                break;
                            case "80002": questionType += "编辑,";
                                break;
                            case "80003": questionType += "经销商,";
                                break;
                            case "80004": questionType += "产品,";
                                break;
                            case "80005": questionType += "活动,";
                                break;
                            case "80006": questionType += "呼叫中心,";
                                break;
                        }
                    }
                    QuestionType.InnerText=questionType.TrimEnd(',');
                    CallRecord.InnerText = model.CallRecord;
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