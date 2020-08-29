using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils.Config;
namespace BitAuto.ISDC.CC2012.Web.AjaxServers.EmployeeAgentExclusive
{
    public partial class EmployeeAgentExclusiveList : System.Web.UI.Page
    {
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public bool updateRoleAuth = false;
        public bool AreaManageAuth = false;

        private DataTable bgdt = null;
        private DataTable Bgdt
        {
            get
            {
                if (bgdt == null)
                {
                    bgdt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();
                }
                return bgdt;
            }
        }

        public int PageIndex
        {

            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["page"]))
                {
                    try
                    {
                        return Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
                    }
                    catch
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD5010"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                BindData();
            }
        }

        private void BindData()
        {
            //分页参数赋值
            Entities.QueryEmployeeAgentExclusive query = BLL.Util.BindQuery<Entities.QueryEmployeeAgentExclusive>(HttpContext.Current);

            int userId = BLL.Util.GetLoginUserID();
            query.LoginID = userId;

            //按条件找人：条件-部门，角色-
            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgentExclusive(query, "", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);

            if (dt != null && dt.Rows.Count > 0)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
            }
        }

        public string GetGroupNameByBGID(string BGID)
        {
            string GroupName = "";
            if (Bgdt != null && BGID.Trim() != "")
            {
                DataRow[] drs = Bgdt.Select("BGID='" + BGID + "'");
                if (drs.Length != 0)
                {
                    GroupName = drs[0]["Name"].ToString();
                }
            }
            return GroupName;
        }

        public string getLoginUserID()
        {
            return BLL.Util.GetLoginUserID().ToString();
        }

        public string getCurrentPage()
        {
            return BLL.PageCommon.Instance.PageIndex.ToString();
        }

        /// 返回业务分类
        /// <summary>
        /// 返回业务分类
        /// </summary>
        /// <param name="businesstype"></param>
        /// <returns></returns>
        public string GetBusinessType(string businesstype)
        {
            return BLL.Util.GetMutilEnumDataNames(CommonFunction.ObjectToInteger(businesstype), typeof(BusinessTypeEnum));
        }
        /// 获取管辖分组名称列表
        /// <summary>
        /// 获取管辖分组名称列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userid)
        {
            return BitAuto.ISDC.CC2012.BLL.UserGroupDataRigth.Instance.GetUserGroupNamesStr(userid);
        }
    }
}