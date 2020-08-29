using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Services.Organization.Remoting;

namespace BitAuto.ISDC.CC2012.Web.ExternalTask.TaskManager
{
    public partial class SelectEmployees : System.Web.UI.Page
    {
        #region 定义属性
        public string RequestRealName
        {
            get { return Request.QueryString["CnName"]; }
        }
        public string RequestSearchByCnName
        {
            get { return Request.QueryString["SearchByCnName"] == null ? string.Empty : Request.QueryString["SearchByCnName"].ToString().Trim(); }
        }


        public int GroupLength = 8;
        //public int RecordCount = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadEmployeeData(RequestSearchByCnName);
            }
        }
        private void LoadEmployeeData(string searchByCnName)
        {
            if (searchByCnName.ToLower().Equals("yes"))
            {
                int count = 0;
                IList<Employee> list = BLL.EmployeeService.SearchByName(RequestRealName, 1, 10, out count);

                repeaterEmployeeList.DataSource = list;
                repeaterEmployeeList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, 10, BLL.PageCommon.Instance.PageIndex, 3);

            }
        }

        protected string GetDepartmentFullName(string employeeNumber)
        {
            return BLL.DepartMentService.GetDepartmentFullPath(employeeNumber);
        }
    }
}