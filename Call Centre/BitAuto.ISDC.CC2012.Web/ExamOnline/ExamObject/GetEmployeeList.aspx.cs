using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.Services.Organization.Remoting;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
namespace BitAuto.ISDC.CC2012.Web.ExamObject
{
    //现行
    public partial class GetEmployeeList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 定义属性
        public int pageSize = 10;
        public int RecordCount;
        public int GroupLength = 8;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        { 
            BindData();
            if (!IsPostBack)
            {
                RoleBind();
                //自动加载，不用手动加载
                //LoadSelectedEmployees();
            }
        }

        #region 加载已选人员
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
                    string agentNum = "";
                    string groupname = "";

                    Entities.QueryEmployeeAgent agentQuery = new QueryEmployeeAgent();
                    agentQuery.UserID = EID_Int;
                    int totle = 0;
                    DataTable agent_dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(agentQuery, "", 1, 1, out totle);

                    if (agent_dt != null && agent_dt.Rows.Count > 0)
                    {
                        agentNum = agent_dt.Rows[0]["AgentNum"].ToString();
                        groupname = agent_dt.Rows[0]["GroupName"].ToString();
                        name = agent_dt.Rows[0]["TrueName"].ToString();
                    }

                    selectedStr += "<tr class=\"back\" onmouseout=\"this.className='back'\" onmouseover=\"this.className='hover'\">"
                        + "<td><a id='" + EID + "' name='" + name + "' href='javascript:DelSelectCustBrand(\"" + EID + "\");\'>"
                            + "<img title='删除' src='/Images/close.png'></a></td>"
                        + "<td class='l'> " + name + " </td>"
                        + "<td class='l'><span style='display:none;'>" + EID_Int + "</span><label>" + agentNum + "</label></td>"
                         + "<td class='l'> " + getRolesByUserID(EID_Int.ToString()) + " </td>"
                        + "<td class='l' style='padding-right:10px;'> " + groupname + " </td>"
                        + "</tr>";
                }
            }
            return selectedStr;
        }
        #endregion

        #region 分组绑定
        public void RoleBind()
        {
            int userid = BLL.Util.GetLoginUserID();
            DataTable db = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userid);
            Rpt_Group.DataSource = db;
            Rpt_Group.DataBind();
        }
        #endregion

        #region 员工列表绑定
        private void BindData()
        {
            QueryEmployeeSuper query = new QueryEmployeeSuper();
            //分页参数赋值
            if (Request.QueryString["EmName"] != null)
            {
                query.TrueName = Request.QueryString["EmName"] == "" ?
                    Entities.Constants.Constant.STRING_INVALID_VALUE : Request.QueryString["EmName"].ToString();
            }
            int intval = 0;
            if (Request.QueryString["GroupID"] != null && int.TryParse(Request.QueryString["GroupID"].ToString(), out intval))
            {
                query.BGID = Request.QueryString["GroupID"] == "-1" ?
                Entities.Constants.Constant.INT_INVALID_VALUE : int.Parse(Request.QueryString["GroupID"].ToString());
            }
            int userid = BLL.Util.GetLoginUserID();
            //当用户选择请选择时，查询条件：数据中的分组 in  （ 用户的管辖分组和所属分组  ）
            if (query.BGID == Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                query.BGIDs = BLL.EmployeeSuper.Instance.GetCurrentUserGroupIDs(userid);
            }
            //弹出层没有区域查询条件 强斐 2014-12-4
            //query.RegionID = BitAuto.ISDC.CC2012.BLL.EmployeeAgent.Instance.GetEmployeeAgentRegionID(userid);
            //按条件找人：条件-姓名，角色
            DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);

            RptEm.DataSource = dt;
            RptEm.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        #endregion

        #region 由员工ID得到角色名
        public string getRolesByUserID(string userID)
        {
            string roleNames = "";
            string ss = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID
                (Convert.ToInt32(userID), ConfigurationUtil.GetAppSettingValue("ThisSysID"));
            if (db != null && db.Rows.Count > 0)
            {
                foreach (DataRow dr in db.Rows)
                {
                    roleNames += dr["RoleName"].ToString() + ",";
                }
            }
            if (roleNames.Length > 0)
            {
                roleNames = roleNames.Substring(0, roleNames.Length - 1);
            }
            return roleNames;
        }
        #endregion
    }
}