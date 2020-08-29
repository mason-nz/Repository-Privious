using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using System.Data;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class EmployeeAgentOption : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// 当前编辑用户
        /// <summary>
        /// 当前编辑用户
        /// </summary>
        public string UserID
        {
            get
            {
                return Request.QueryString["UserID"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["UserID"].ToString());
            }
        }
        /// 当前编辑用户姓名
        /// <summary>
        /// 当前编辑用户姓名
        /// </summary>
        public string UserName
        {
            get
            {
                return Request.QueryString["UserName"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["UserName"].ToString());
            }
        }
        /// 当前编辑用户工号
        /// <summary>
        /// 当前编辑用户工号
        /// </summary>
        public string AgentNum
        {
            get
            {
                return Request.QueryString["AgentNum"] == null ? string.Empty: Request.QueryString["AgentNum"].ToString();
            }
        }

        //所在组
        public string AtGroupID = "";

        //拥有的技能组ids
        public string OwnSkillGroupIDS = "";
        /// <summary>
        /// 坐席工号生成，起点：1000，后续按照此点往后补漏，且自增加1
        /// </summary>
        private int genNewAgentID_StartPoint = ConfigurationManager.AppSettings["GenNewAgentID_StartPoint"] != null ? int.Parse(ConfigurationManager.AppSettings["GenNewAgentID_StartPoint"].ToString()) : 1000;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT5102"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindRoles();
                    GetEmployeeAgentInfo();
                    GetUserSkillGroupDIS();
                }
            }
        }

        /// 绑定角色（CC和IM共有的角色）
        /// <summary>
        /// 绑定角色（CC和IM共有的角色）
        /// </summary>
        private void BindRoles()
        {
            //绑定
            Rpt_Role.DataSource = BLL.EmployeeAgent.Instance.GetCCAndIMRoles();
            Rpt_Role.DataBind();
        }
        /// 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        private void GetEmployeeAgentInfo()
        {
            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(int.Parse(UserID));
            if (model != null)
            {
                //设置区域
                if (model.RegionID == 1)
                {
                    this.rdo_areaid_1.Checked = true;
                }
                else if (model.RegionID == 2)
                {
                    this.rdo_areaid_2.Checked = true;
                }

                //设置所属业务
                if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.CC) == (int)BusinessTypeEnum.CC)
                {
                    this.ckb_businesstype_1.Checked = true;
                }
                if ((CommonFunction.ObjectToInteger(model.BusinessType) & (int)BusinessTypeEnum.IM) == (int)BusinessTypeEnum.IM)
                {
                    this.ckb_businesstype_2.Checked = true;
                }

                //设置所属分组
                AtGroupID = model.BGID.ToString();
            }

            txtAgentNum.InnerText = string.IsNullOrEmpty(AgentNum) ? BLL.EmployeeAgent.Instance.GenNewAgentID(genNewAgentID_StartPoint) : AgentNum;
        }
        public void GetUserSkillGroupDIS()
        {
            int userid = Convert.ToInt32(UserID);
            if (userid > 0)
            {
                try
                {
                    OwnSkillGroupIDS = BLL.SkillGroupDataRight.Instance.GetUserSkillGroup(userid);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    Response.End();
                }
            }
        }
    }
}