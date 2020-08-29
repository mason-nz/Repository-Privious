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
    public partial class TaskProcess : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        public string RequestConsultDataID;
        public string RequestRecordType;
        public string RequestConsultID;
        public string RequestCustID;
        public TaskProcess()
        {
            if (RequestTaskID != "")
            {
                //根据任务ID获取主键ID 通过用户控件获取用户基本信息
                QueryCustHistoryInfo query_info = new QueryCustHistoryInfo();
                query_info.TaskID = RequestTaskID;
                int count;
                DataTable dt_info = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query_info, "", 1, 10000, Entities.CustHistoryInfo.SelectFieldStr, out count);
                if (dt_info.Rows.Count == 1)
                {
                    Entities.CustHistoryInfo Model_CustHistoryInfo = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(long.Parse(dt_info.Rows[0]["RecID"].ToString()));
                    RequestConsultDataID = Model_CustHistoryInfo.ConsultDataID.ToString();
                    RequestRecordType = Model_CustHistoryInfo.RecordType.ToString();
                    RequestConsultID = Model_CustHistoryInfo.ConsultID.ToString();
                    RequestCustID = Model_CustHistoryInfo.CustID;
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断权限
                limit();

                BindData();
            }
        }
        //判断权限
        private void limit()
        {
            //1 如果权限在功能点内 该页面全可见；如果不在，则查找是否为当前代理人，如果不是，则隐藏按钮功能 
            //功能点：同意转出：SYS024BUT1101；结束任务：SYS024BUT1102
            bool right_AgreeTurnOut = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT1101");
            bool right_TaskTurnOver = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT1102");
            if (!right_AgreeTurnOut && !right_TaskTurnOver)     //坐席权限
            {
                spanHighOper.Visible = false;
                //判断登陆者是否是当前受理人且状态为有效
                int count;
                QueryTaskCurrentSolveUser query = new QueryTaskCurrentSolveUser();
                query.CurrentSolveUserEID = BLL.Util.GetLoginUserID();
                query.TaskID = RequestTaskID;
                query.Status = 1;
                DataTable dt = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query, "", 1, 10000, out count);
                if (dt.Rows.Count == 0)
                {
                    liNextSolveUser.Visible = false;
                    divDeal.Visible = false;
                    spanProcessStatus.Visible = false;
                    spanBtnSubmit.Visible = false;
                }
                chkIsComplaint.Attributes.Add("disabled", "disabled");
            }
            else           //组长权限
            {
                if (!right_AgreeTurnOut)
                {
                    btnTurnOut.Visible = false;
                }
                if (!right_TaskTurnOver)
                {
                    btnTurnOver.Visible = false;
                }
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
                    LastTreatmentTime.InnerText = CommonFunction.GetDateTimeStrForPage(dt.Rows[0]["LastTreatmentTime"].ToString());
                    if (dt.Rows[0]["IsComplaint"].ToString().ToLower() == "true")
                    {
                        chkIsComplaint.Checked = true;
                    }

                    //如果该任务已结束，则隐藏按钮
                    if (int.Parse(dt.Rows[0]["ProcessStatus"].ToString()) == (int)Entities.EnumTaskStatus.TaskStatusOver)
                    {
                        ulProcess.Visible = false;
                        divDeal.Visible = false;
                        spanBtnSubmit.Visible = false;
                        //spanHighOper.Visible = false;
                        //chkIsComplaint.Attributes.Add("disabled", "disabled");
                    }
                    else
                    {
                        bool right_AgreeTurnOut = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT1101");
                        bool right_TaskTurnOver = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT1102");
                        //如果有高级操作权限再判断
                        if (right_AgreeTurnOut && right_TaskTurnOver)
                        {
                            //判断是否是第一次转出，如果不是，则隐藏同意转出按钮  
                            QueryCustHistoryLog query_Log = new QueryCustHistoryLog();
                            query_Log.TaskID = RequestTaskID;
                            query_Log.Action = (int)Entities.Action.ActionAgreeApplyTurn;
                            DataTable dt_Log = BLL.CustHistoryLog.Instance.GetCustHistoryLog(query_Log, "", 1, 10000, out count);
                            if (dt_Log.Rows.Count > 0)
                            {
                                btnTurnOut.Visible = false;
                            }
                        }
                    }
                }
            }
        }
    }
}