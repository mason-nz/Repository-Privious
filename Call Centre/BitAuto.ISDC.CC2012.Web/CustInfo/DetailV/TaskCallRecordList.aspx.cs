using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;


namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class TaskCallRecordList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string TaskID = "";
        //public string DataSource = "";//数据来源，1-CRM库，2-CRM库及Excel新增
        public bool IsBind = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private CustInfoHelper ch = new CustInfoHelper();
        ProjectTaskInfo task;
        string custId = string.Empty;

        private void BindData()
        {
            if (string.IsNullOrEmpty(ch.TID)) { return; }
            else
            {
                this.TaskID = ch.TID;
                task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(this.TaskID);

                if (task == null)
                {
                    return;
                }

                //custId = task.RelationID;
                //DataSource = (task.Source == 1 ? "2" : "1");
                switch (task.TaskStatus)
                {
                    case 180000://未处理
                    case 180001://处理中
                    case 180002://审核拒绝
                    case 180009://已驳回
                        IsBind = true;
                        break;
                    default: break;
                }
                if (Request["Action"] != null)
                {
                    if (Request["Action"] != null && Request["Action"] == "view")
                    {
                        IsBind = false;
                    }
                }
            }
            //QueryCallRecordInfo query = new QueryCallRecordInfo();
            //query.TaskTypeID = 1;
            //if (task.Source == 2)//CRM库
            //{
            //    query.CustID = task.RelationID;
            //    query.TaskID = this.TaskID;
            //}
            //else if (task.Source == 1)//Excel导入
            //{
            //    query.NewCustID = int.Parse(task.RelationID);
            //    query.TaskID = this.TaskID;
            //    query.IsRelationIDOrCRMCustID = true;
            //}
            //int totalCount = 0;
            //string tableEndName = "";//只查询现表数据
            DataTable table = null; // BLL.CallRecordInfo.Instance.GetCallRecordsForRecord(query, 1, 100000, tableEndName, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_Contact.DataSource = table;
            }
            //绑定列表数据
            repeater_Contact.DataBind();
        }
    }
}