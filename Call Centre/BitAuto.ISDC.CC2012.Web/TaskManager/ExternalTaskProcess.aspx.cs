using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TaskManager
{
    public partial class ExternalTaskProcess : System.Web.UI.Page
    {
        #region 属性
        public string RequestCustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }
        public string RequestCHIRecID
        {
            get { return HttpContext.Current.Request["RecID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecID"].ToString()); }
        }
        public string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        public string RequestConsultID
        {
            get { return HttpContext.Current.Request["ConsultID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ConsultID"].ToString()); }
        }
        public string RequestConsultDataID
        {
            get { return HttpContext.Current.Request["ConsultDataID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ConsultDataID"].ToString()); }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewCustBaseInfo1.CustID = RequestCustID;
                //判断权限
                limit();

                BindData();
            }
        }

        //判断权限
        private void limit()
        {
            //判断是否登录 及登陆者是否是当前受理人且状态为有效 
            string username = HttpContext.Current.User.Identity.Name.ToLower(); 
            int count;
            QueryTaskCurrentSolveUser query = new QueryTaskCurrentSolveUser();

            query.CurrentSolveUserID = BLL.Util.GetEmployeeEidByDomainAccount(username);
            query.TaskID = RequestTaskID;
            query.Status = 1;
            DataTable dt = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query, "", 1, 10000, out count);
            if (dt.Rows.Count != 1)
            {
                liNextSolveUser.Visible = false;
                divDeal.Visible = false;
                spanBtnSubmit.Visible = false;
                chkIsComplaint.Attributes.Add("disabled", "disabled");
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
                    LastTreatmentTime.InnerText = dt.Rows[0]["LastTreatmentTime"].ToString() == "1900-1-1 0:00:00" ? "" : dt.Rows[0]["LastTreatmentTime"].ToString();
                    if (dt.Rows[0]["IsComplaint"].ToString().ToLower() == "true")
                    {
                        chkIsComplaint.Checked = true;
                    }

                    //如果该任务已结束，则隐藏按钮
                    if (int.Parse(dt.Rows[0]["ProcessStatus"].ToString()) == (int)Entities.EnumTaskStatus.TaskStatusOver)
                    {
                        ulProcess.Visible = false;
                        //divDeal.Visible = false;
                        //spanBtnSubmit.Visible = false;
                    }
                }
                else
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    Response.Write("<script language='javascript'>alert('无权限访问该页面');window.close();</script>");
                }
            }
        }
    }
}