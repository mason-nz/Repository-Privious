using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustCheck
{
    public partial class CheckTaskList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        public bool right_btn1;   //分配
        public bool right_btn2;  //回收
        public bool right_btn3;  //结束
        public bool right_btn4;  //导出
        public bool btnAudit;//审核

        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();

                //SYS024BUT130101
                right_btn1 = BLL.Util.CheckRight(userID, "SYS024BUT130101");
                right_btn2 = BLL.Util.CheckRight(userID, "SYS024BUT130102");
                right_btn3 = BLL.Util.CheckRight(userID, "SYS024BUT130103");
                right_btn4 = BLL.Util.CheckRight(userID, "SYS024BUT130104");
                btnAudit = BLL.Util.CheckRight(userID, "SYS024BUT130106");
                getAllCreater();
                getEmpleeUserId();
                getOptUserId();
                sltCustTypeBind();
                //rptDataSourceBind();
            }
        }


        //绑定所有问卷创建人
        private void getAllCreater()
        {
            DataTable dt = BLL.ProjectTaskInfo.Instance.getCreateUser();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dt.Rows[i]["CreateUserID"].ToString()));
                if (userName != string.Empty)
                {
                    selCreater.Items.Add(new ListItem(userName, dt.Rows[i]["CreateUserID"].ToString()));
                }
            }
            selCreater.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        /// <summary>
        /// 坐席
        /// </summary>
        private void getEmpleeUserId()
        {
            DataTable dt = BLL.ProjectTaskInfo.Instance.getEmpleeUser();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int userid = 0;
                int.TryParse(dt.Rows[i]["UserID"].ToString(), out userid);

                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userid);
                if (userName != string.Empty)
                {
                    selUserId.Items.Add(new ListItem(userName, dt.Rows[i]["UserID"].ToString()));
                }
            }
            selUserId.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        /// <summary>
        /// 最后操作人
        /// </summary>
        private void getOptUserId()
        {
            DataTable dt = BLL.ProjectTaskInfo.Instance.getOpterUser();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int userid = 0;
                int.TryParse(dt.Rows[i]["LastOptUserID"].ToString(), out userid);

                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userid);
                if (userName != string.Empty)
                {
                    selOptUserId.Items.Add(new ListItem(userName, dt.Rows[i]["LastOptUserID"].ToString()));
                }
            }
            selOptUserId.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        private void sltCustTypeBind()
        {
            List<string[]> customType = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOpt(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomType));
            foreach (string[] s in customType)
            {
                if (int.Parse(s[1]) == (int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Showroom)
                {
                    continue;
                }
                this.sltCustType.Items.Add(new ListItem(s[0], s[1]));
            }

            this.sltCustType.Items.Insert(0, new ListItem("请选择", "-1"));
            this.sltCustType.SelectedIndex = 0;
        }

        private void rptDataSourceBind()
        {
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.EnumProjectTaskStatus));

            foreach (DataRow dr in dt.Rows)
            {
                selStatus.Items.Add(new ListItem(dr["name"].ToString(), dr["value"].ToString()));
            }
            selStatus.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}