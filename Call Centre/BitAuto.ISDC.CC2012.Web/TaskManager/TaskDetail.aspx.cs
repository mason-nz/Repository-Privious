using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TaskManager
{
    public partial class TaskDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        public string RequestConsultID;
        public string RequestConsultDataID;
        public string RequestRecordType;
        public string RequestCustID;
        public TaskDetail()
        {
            //根据任务ID获取主键ID 通过用户控件获取用户基本信息
            QueryCustHistoryInfo query = new QueryCustHistoryInfo();
            query.TaskID = RequestTaskID;
            int count;
            DataTable dt = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query, "", 1, 10000, Entities.CustHistoryInfo.SelectFieldStr, out count);
            if (dt.Rows.Count >0)
            {
                Entities.CustHistoryInfo Model_CustHistoryInfo = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(long.Parse(dt.Rows[0]["RecID"].ToString()));
                RequestConsultDataID = Model_CustHistoryInfo.ConsultDataID.ToString();
                RequestRecordType = Model_CustHistoryInfo.RecordType.ToString();
                RequestConsultID = Model_CustHistoryInfo.ConsultID.ToString();
                RequestCustID = Model_CustHistoryInfo.CustID;
            }
        }

        public string Source = string.Empty;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //查询数据来源属于新车还是置换订单 
                if (RequestTaskID != "")
                {
                    int _taskID;
                    if (int.TryParse(RequestTaskID, out _taskID))
                    {
                        Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(_taskID);
                        if (model != null)
                        {
                            Source = model.Source.ToString();
                            ContactRecordView1.TaskID = RequestTaskID;
                            ContactRecordView1.Source = Source;
                            divDeal.Style.Add("display", "none");
                            ulDeal.Style.Add("display", "none");
                            BindData();
                        }
                    }
                    else if (Source == string.Empty)
                    {
                        if (HiddenDiv())
                        {
                            BindData();
                        }
                    }
                }
            }
        }

        //判断是否进行过转发，如果没有 隐藏处理记录
        private bool HiddenDiv()
        {
            QueryCustHistoryLog query_Log = new QueryCustHistoryLog();
            query_Log.TaskID = RequestTaskID;
            int count;
            DataTable dt = BLL.CustHistoryLog.Instance.GetCustHistoryLog(query_Log, "", 1, 10000, out count);
            if (dt.Rows.Count == 0)
            {
                divLine.Style.Add("display", "none");
                ulDeal.Style.Add("display", "none");
                divDeal.Style.Add("display", "none");
                return false;
            }
            else
            {
                return true;
            }
        }

        //绑定数据
        private void BindData()
        {
            if (RequestTaskID != "")
            {
                DataTable dt = new DataTable();
                QueryCustHistoryInfo query = new QueryCustHistoryInfo();
                query.TaskID = RequestTaskID;

                int count;
                dt = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query, "", 1, 10000, Entities.CustHistoryInfo.SelectFieldStr, out count);
                if (dt.Rows.Count > 0)
                {
                    switch (int.Parse(dt.Rows[0]["QuestionQuality"].ToString()))
                    {
                        case (int)Entities.QuestionNature.NatureCommon:
                            QuestionQuality.InnerText = "普通";
                            break;
                        case (int)Entities.QuestionNature.NatureUrgent:
                            QuestionQuality.InnerText = "紧急";
                            break;
                    }
                    LastTreatmentTime.InnerText = dt.Rows[0]["LastTreatmentTime"].ToString();
                    if (dt.Rows[0]["IsComplaint"].ToString().ToLower() == "true")
                    {
                        IsComplaint.InnerText = "是";
                    }
                    else if (dt.Rows[0]["IsComplaint"].ToString().ToLower() == "false")
                    {
                        IsComplaint.InnerText = "否";
                    }
                }
            }
        }
    }
}