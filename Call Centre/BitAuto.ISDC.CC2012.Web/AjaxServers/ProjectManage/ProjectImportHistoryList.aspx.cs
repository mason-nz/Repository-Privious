using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class ProjectImportHistoryList : PageBase
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["ProjectID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["ProjectID"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ImportHistoryDataBind(ProjectID);
            }

        }
        public void ImportHistoryDataBind(string ProjectID)
        {
            Entities.QueryProjectImportHistory query = new Entities.QueryProjectImportHistory();
            int _project = 0;
            int.TryParse(ProjectID, out _project);
            query.ProjectID = _project;
            int rowcount = 0;
            DataTable dt = BLL.ProjectImportHistory.Instance.GetProjectImportHistory(query, " createtime desc", 1, 100000, out rowcount);
            this.repeater_ImportHistory.DataSource = dt;
            this.repeater_ImportHistory.DataBind();
        }


        //获取操作人
        public string getOperator(string userID)
        {
            string _operator = string.Empty;
            int _userID;
            if (int.TryParse(userID, out _userID))
            {
                _operator = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
            }
            return _operator;
        }
    }
}