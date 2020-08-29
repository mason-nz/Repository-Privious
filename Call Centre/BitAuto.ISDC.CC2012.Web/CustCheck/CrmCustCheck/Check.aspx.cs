using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustCheck.CrmCustCheck
{
    public partial class Check : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TID = "";
        public string CRMCustID = "";
        public string CustName = "";
        public int TaskStatus = 0;
        CustCheckHelper helper = new CustCheckHelper();
        public string QueryParams { get { return HttpUtility.UrlEncode(helper.QueryParams); } }
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
                    //int taskId = -1;
                    if (!string.IsNullOrEmpty(h.TID))
                    {
                        CustVerify(h.TID);
                        this.TID = h.TID;
                        //this.UCEditCust1.Task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(h.TID);
                        //CRMCustID = this.UCEditCust1.Task.CrmCustID;
                        //CustName = this.UCEditCust1.Task.CustName;
                        if (RequestAction.Equals("view"))
                        {
                            //UCEditCust1.IsShowAddMembers = false;
                        }
                        //TaskStatus = this.UCEditCust1.Task.TaskStatus;
                    }
                    else
                    {
                        throw new Exception("无法找到此任务");
                    }
                }
            }
            catch (Exception ex)
            {
                //日志
            }
        }

        protected bool CustVerify(string tid)
        {
            bool result = true;
            Entities.ProjectTaskInfo taskInfo = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(tid);
            if (taskInfo != null)
            {
                if (taskInfo.Source == 1)
                {
                    Response.Write("<script type='text/javascript'>$.jAlert('此客户属于新增客户，请进入新增客户核实页进行核实操作！');window.location.href='/CustCheck/CrmCustCheck/Main.aspx';</script>");
                    result = false;
                }
                else if (taskInfo.Source == 2)
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(taskInfo.CrmCustID);
                    if (custInfo != null)
                    {
                        if (custInfo.CarType == 2)
                        {
                            Response.Write("<script type='text/javascript'>$.jAlert('此客户是二手车客户，请进入相应的核实页进行操作！');window.location.href='/CustCheck/CrmCustCheck/Main.aspx';</script>");
                            result = false;
                        }
                    }
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