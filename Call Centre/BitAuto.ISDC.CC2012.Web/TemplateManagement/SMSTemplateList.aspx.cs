using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class SMSTemplateList : PageBase
    {
        public bool right_btnAdd;   //添加
        private int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                //判断是否有添加模板权限
                right_btnAdd = BLL.Util.CheckRight(userID, "SYS024MOD510401");
                //绑定所有问卷创建人
                getAllCreater(userID);
            }
        }

        //绑定所有问卷创建人
        private void getAllCreater(int userid)
        {
            DataTable dt = BLL.SMSTemplate.Instance.GetCreateUserID(userid);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int _userID;
                    if (int.TryParse(dt.Rows[i]["CreateUserID"].ToString(), out _userID))
                    {
                        string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
                        if (userName != string.Empty)
                        {
                            selCreater.Items.Add(new ListItem(userName, dt.Rows[i]["CreateUserID"].ToString()));
                        }
                    }
                }
            }
            selCreater.Items.Insert(0, new ListItem("请选择", "-1"));
        }
    }
}