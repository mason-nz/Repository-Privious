using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class KnowledgeLibList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public bool DataAddButton = false;//添加权限
        public bool right_btnCategory;  //分类管理
        public bool right_NewFAQ;  //分类管理
        public bool right_NewQuestion;  //分类管理
        public int RegionID = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                DataAddButton = BLL.Util.CheckButtonRight("SYS024MOD3201");
                right_btnCategory = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD3204");
                right_NewFAQ = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD3202");
                right_NewQuestion = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024MOD3203");

                bindCreateUser();

                int userid = BLL.Util.GetLoginUserID();
                EmployeeAgent a = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
                if (a.RegionID.HasValue)
                {
                    RegionID = a.RegionID.Value;
                }
            }
        }

        //得到创建人列表
        public void bindCreateUser()
        {
            DataTable dt = BLL.KnowledgeLib.Instance.getCreateUser();
            DataTable dtModify = BLL.KnowledgeLib.Instance.getModifyUser();

            Hashtable hdt = new Hashtable();
            string userNameT;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                userNameT = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dt.Rows[i]["CreateUserID"].ToString()));
                if (userNameT != string.Empty)
                {
                    hdt.Add(dt.Rows[i]["CreateUserID"].ToString(), userNameT);
                    selCreateUser.Items.Add(new ListItem(userNameT, dt.Rows[i]["CreateUserID"].ToString()));
                }
            }
            for (int j = 0; j < dtModify.Rows.Count; j++)
            {
                string mid = dtModify.Rows[j]["LastModifyUserID"].ToString();
                if (hdt.ContainsKey(mid))
                {
                    selModifier.Items.Add(new ListItem(hdt[mid].ToString(), mid));
                }
                else
                {
                    userNameT = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(mid));
                    if (userNameT != string.Empty)
                    {
                        hdt.Add(mid, userNameT);
                        selModifier.Items.Add(new ListItem(userNameT, mid));
                    }
                }
            }
            selCreateUser.Items.Insert(0, new ListItem("请选择", "-1"));
            selModifier.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}