using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ConsultManager
{
    public partial class ConsultPUseCarView : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
                ConsultPUseCar model = BLL.ConsultPUseCar.Instance.GetConsultPUseCar(recID);
                if (model != null)
                {
                    ConsultID.InnerText = "个人用车";
                    RecordType.InnerText = RequestRecordType == "1" ? "呼入" : "呼出";
                    string questionType = string.Empty;
                    string [] array_type=model.QuestionType.Split(',');
                    for (int i = 0; i < array_type.Length; i++)
                    {
                        switch (array_type[i])
                        {
                            case "90001": questionType += "信贷,";
                                break;
                            case "90002": questionType += "保险,";
                                break;
                            case "90003": questionType += "养护维修,";
                                break;
                            case "90004": questionType += "自驾游,";
                                break;
                            case "90005": questionType += "其他,";
                                break;
                        }
                    }
                    QuestionType.InnerText = questionType.TrimEnd(',');
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