using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // UserGroupBind();
                CreateUserBind();
            }
        }

        //private void UserGroupBind()
        //{
        //    int userId = BLL.Util.GetLoginUserID();
        //    DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId, "and bg.Status=0");
        //    sltUserGroup.DataSource = dt;
        //    sltUserGroup.DataTextField = "Name";
        //    sltUserGroup.DataValueField = "BGID";
        //    sltUserGroup.DataBind();
        //    sltUserGroup.Items.Insert(0, new ListItem("请选所属分组", "-1"));
        //}

        private void CreateUserBind()
        {
          List<int> list =  BLL.SurveyProjectInfo.Instance.GetAllCreateUserID();
          foreach (int userId in list)
          {
              string trueName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userId);
              sltCreateUser.Items.Add(new ListItem(trueName, userId.ToString()));
          }
          sltCreateUser.Items.Insert(0, new ListItem("请选择", ""));
        }
    }
}