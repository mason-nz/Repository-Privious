using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustCheck.NewCustCheck
{
    public partial class SecondCarCheck : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID = "";
        public string CustName = "";
        CustCheckHelper helper = new CustCheckHelper();
        public string QueryParams { get { return HttpUtility.UrlEncode(helper.QueryParams); } }
        public int TaskStatus = 0;
        public string NewCustID = "";
        /// <summary>
        /// 行为：包括（查看）
        /// </summary>
        public string RequestAction
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("Action").ToLower(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    
                    CustCheckHelper h = new CustCheckHelper();
                    if (!string.IsNullOrEmpty(h.TID))
                    {
                        //CustVerify(taskId);
                        this.TID = h.TID;
                        this.UCCustInfo.Task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(h.TID);
                        CustName = this.UCCustInfo.Task.CustName;
                        if (RequestAction.Equals("view"))
                        {
                            UCCustInfo.IsShowAddMembers = false;
                        }
                        TaskStatus = this.UCCustInfo.Task.TaskStatus;
                        NewCustID = this.UCCustInfo.Task.RelationID;
                    }
                    else
                    {
                        throw new Exception("无法找到此任务");
                    }
                }
            }
            catch (Exception ex) { }
        }

        protected bool CustVerify(string tid)
        {
            bool result = true;
            Entities.ProjectTaskInfo taskInfo = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(tid);
            if (taskInfo != null)
            {
                if (taskInfo.Source == 1)
                {
                    Entities.ProjectTask_Cust custInfo = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(tid);
                    if (custInfo != null)
                    {
                        if (custInfo.CarType != 2)
                        {
                            Response.Write("<script type='text/javascript'>$.jAlert('此客户不是二手车客户，请进入相应的核实页进行操作！');window.location.href='/CustCheck/NewCustCheck/Main.aspx';</script>");
                            result = false;
                        }
                    }
                }
                else if (taskInfo.Source == 2)
                {
                    Response.Write("<script type='text/javascript'>$.jAlert('此客户属于Crm客户，请进入Crm客户核实页进行核实操作！');window.location.href='/CustCheck/NewCustCheck/Main.aspx';</script>");
                    result = false;
                }
            }
            else
            {
                Response.Write("<script type='text/javascript'>$.jAlert('不存在此任务ID');window.location.href='/CustCheck/CrmCustCheck/Main.aspx';</script>");
                result = false;
            }

            return result;
        }
    }
}