using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ConsultManager
{
    public partial class ConsultActivityView : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
                ConsultActivity model = BLL.ConsultActivity.Instance.GetConsultActivity(recID);
                if (model != null)
                {
                    ConsultID.InnerText = "活动";
                    RecordType.InnerText = RequestRecordType == "1" ? "呼入" : "呼出";
                    switch (model.QuestionType)
                    {
                        case 1: QuestionType.InnerText = "活动前";
                            break;
                        case 2: QuestionType.InnerText = "活动后";
                            break;
                    }
                    BrandName.InnerText = model.BrandName;
                    ShowActivity.InnerText = model.ShowActivity;
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