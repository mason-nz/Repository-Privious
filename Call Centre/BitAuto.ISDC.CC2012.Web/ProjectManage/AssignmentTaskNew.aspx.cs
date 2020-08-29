using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class AssignmentTaskNew : PageBase
    {
        #region 属性 
        public string ProjectID { get { return BLL.Util.GetCurrentRequestStr("ProjectID"); } }
        /// <summary>
        /// 任务总数
        /// </summary>
        public int TaskCount { get; set; }
        public int NotDistrictTaskCount { get; set; }

        public string rowCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAgentData();
            }
        }

        /// 查询有权限的坐席
        /// <summary>
        /// 查询有权限的坐席
        /// </summary>
        private void BindAgentData()
        {
            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentByLoginUser();
            rowCount = "可分配坐席数：" + dt.Rows.Count;
            AgentList.DataSource = dt;
            AgentList.DataBind();

            string[] arrCount = BLL.OtherTaskInfo.Instance.GetNotDistrictCountAndTaskCount(ProjectID).Split(',');
            if (arrCount.Length == 2)
            {
                int notdistrictcount, taskcount;
                if (int.TryParse(arrCount[0], out notdistrictcount))
                {
                    NotDistrictTaskCount = notdistrictcount;
                }
                if (int.TryParse(arrCount[1], out taskcount))
                {
                    TaskCount = taskcount;
                } 
            }
        }
    }
}