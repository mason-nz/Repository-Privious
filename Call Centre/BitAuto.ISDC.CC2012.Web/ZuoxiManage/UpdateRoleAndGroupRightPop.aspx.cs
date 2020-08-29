using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class UpdateRoleAndGroupRightPop : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string UserName
        {
            get
            {
                return Request.QueryString["userNames"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["userNames"].ToString());
            }

        }

        public string UserIDs
        {
            get
            {
                return Request.QueryString["userIDs"] == null ? String.Empty : HttpContext.Current.Server.UrlDecode(Request.QueryString["userIDs"].ToString());
            }

        }
        public string AgentNum
        {
            get
            {
                return Request.QueryString["agentNum"] == null ? String.Empty : Request.QueryString["agentNum"].ToString();
            }
        }

        /// <summary>
        /// 是否修改单个用户(false:批量修改，true：单个设置)（单个用户时，要修改工号）
        /// </summary>
        public string IsModfiySingle
        {
            get
            {
                return HttpContext.Current.Request["single"] == null ? string.Empty : HttpContext.Current.Request["single"].ToString();
            }
        }

        /// <summary>
        /// 数据权限ID
        /// </summary>
        public string DataRightTyle
        {
            get
            {
                return HttpContext.Current.Request["dataright"] == null ? string.Empty : HttpContext.Current.Request["dataright"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID
                (ConfigurationUtil.GetAppSettingValue("ThisSysID"));
            if (db != null && db.Rows.Count > 0)
            {
                Rpt_Role.DataSource = db;
                Rpt_Role.DataBind();
                GroupDataBind();
            }

            if (IsModfiySingle == "false")
            {
                //如果是批量修改
                this.divAgentNum.Visible = false;
                this.divData.Visible = false;

            }
            else
            {
                BingData();
            }

        }

        private void BingData()
        {
            int intVal = 0;
            if (int.TryParse(UserIDs, out intVal))
            {
                DataTable dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID
                    (int.Parse(UserIDs), ConfigurationUtil.GetAppSettingValue("ThisSysID"));
            }
        }


        private void GroupDataBind()
        {
            DataTable dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();
            rptGroup.DataSource = dt;
            rptGroup.DataBind();
        }

        /// <summary>
        /// 判断工号是否被其他员工占用
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <param name="AgentNum"></param>
        /// <returns></returns>
        public bool IsExistsAgentNum(string UserIDs, string AgentNum)
        {
            #region 判断工号是否重复
            bool flag = false;
            //判断是否重复                  
            Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
            query.AgentNum = AgentNum;

            int total = 0;
            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, "", 1, 10, out total);
            if (total != 0)
            {
                if (dt.Rows[0]["UserID"].ToString() != UserIDs)
                {
                    //与别人的工号有重复
                    flag = true;
                }
            }
            return flag;
            #endregion
        }
    }
}