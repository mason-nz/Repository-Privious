using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage
{
    public partial class ZuoxiTableListAll : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public bool updateRoleAuth = false;
        public bool AreaManageAuth = false;
        public bool updateGroupAuth = false;

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

        public string Pwd
        {
            get { return BLL.Util.GetCurrentRequestStr("pwd"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024MOD5001"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                if (!Pwd.ToLower().Equals("3219jdhliweruy@hfkjhi@oghg"))
                {
                    BitAuto.Utils.ScriptHelper.ShowAlertScript("您无此页面权限！");
                    Response.End();
                }
                else
                {
                    BindData();
                    updateRoleAuth = BLL.Util.CheckRight(userId, "SYS024BUT5101");
                    updateGroupAuth = BLL.Util.CheckRight(userId, "SYS024BUT5102");
                    AreaManageAuth = BLL.Util.CheckRight(userId, "SYS024BUT5103");
                }
            }
            //Util.GetLoginUserID().ToString();
        }

        private void BindData()
        {
            //分页参数赋值
            Entities.QueryEmployeeSuper query = BLL.Util.BindQuery<Entities.QueryEmployeeSuper>(HttpContext.Current);

            AreaManageConfig config = new AreaManageConfig(HttpContext.Current.Server);
            List<string> list = config.GetCurrentUserArea();
            if (list.Count > 0)
            {
                int regionid = 1;
                if (list.Count == 1)
                {
                    if (list[0] == "西安")
                    {
                        regionid = 2;
                    }

                    query.RegionID = regionid;
                }

                query.OnlyCCDepart =false ;
                //按条件找人：条件-部门，角色-
                DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    repeaterTableList.DataSource = dt;
                    repeaterTableList.DataBind();
                    litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
                }
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