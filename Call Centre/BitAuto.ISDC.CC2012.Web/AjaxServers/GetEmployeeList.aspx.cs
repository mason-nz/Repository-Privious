using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.Services.Organization.Remoting;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public partial class GetEmployeeList : PageBase
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
            if (searchByCnName.ToLower().Equals("yes") && !String.IsNullOrEmpty(RequestRealName))
            {
                int count = 0;
                IList<Employee> list = BLL.EmployeeService.SearchByName(RequestRealName, 1, 10, out count);
                repeaterEmployeeList.DataSource = list;
                repeaterEmployeeList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, 10, BLL.PageCommon.Instance.PageIndex, 3);
            }
        }

        public string LoadSelectedEmployees()
        {
            string selectedStr = "";
            string EmployeesIDs = "";
            if (Request.QueryString["UserIDs"] == null)
            {
                return "";
            }
            else
            {
                EmployeesIDs = Request.QueryString["UserIDs"].ToString();
            }

            if (EmployeesIDs.Trim() != "")
            {
                string[] EmplloyeeIDArr = EmployeesIDs.Split(',');
                foreach (string EID in EmplloyeeIDArr)
                {
                    int EID_Int = Convert.ToInt32(EID);
                    string name = "";
                    name = GetEmployeeNameByEID(EID_Int);
                    selectedStr += "<tr class=\"back\" onmouseout=\"this.className='back'\" onmouseover=\"this.className='hover'\">"
                        + "<td style=\"border-right:1px solid #DDDDDD;border-left:1px solid #DDDDDD\"><a id='" + EID + "' name='" + name + "' href='javascript:DelSelectCustBrand(\"" + EID + "\");\'>"
                            + "<img title='删除' src='/Images/close.png'></a></td>"
                        + "<td class='l' style=\"border-right:1px solid #DDDDDD;\"> " + name + " </td>"
                        + "<td class='l' style='display:none;border-right:1px solid #DDDDDD;'><span style='display:none;'>" + EID + "</span><label>" + EID + "</label></td>"
                        + "<td class='l' style=\"border-right:1px solid #DDDDDD;\"> " + GetDepartmentFullNameByEmployeeNumber(EID_Int) + " </td>"
                        + "</tr>";
                }
            }
            return selectedStr;
        }

        private string GetEmployeeNameByEID(int EID_Int)
        {
            //功能废弃，暂不实现
            throw new NotImplementedException();
        }

        private string GetDepartmentFullNameByEmployeeNumber(int EID_Int)
        {
            //功能废弃，暂不实现
            throw new NotImplementedException();
        }

        protected string GetDepartmentFullName(string employeeNumber)
        {
            return BLL.DepartMentService.GetDepartmentFullPath(employeeNumber);
        }
    }
}