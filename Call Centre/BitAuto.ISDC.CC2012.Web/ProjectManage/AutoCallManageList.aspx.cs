﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class AutoCallManageList : PageBase
    {
        //public bool right_btnAdd;   //添加
        //public bool right_btnCategory;  //分类管理
        private int userID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                //right_btnAdd = BLL.Util.CheckRight(userID, "SYS024BUT500601");
                //right_btnCategory = BLL.Util.CheckRight(userID, "SYS024BUT5002");
                //getAllCreater();
            }
        }




        /*
        //绑定所有问卷创建人
        private void getAllCreater()
        {
            DataTable dt = BLL.ProjectInfo.Instance.getCreateUser();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string userName = BLL.Util.GetEmployeeNameByEid(BLL.Util.GetHrEIDByLimitEID(int.Parse(dt.Rows[i]["CreateUserID"].ToString())));
                if (userName != string.Empty)
                {
                    //selCreater.Items.Add(new ListItem(userName, dt.Rows[i]["CreateUserID"].ToString()));
                }
            }
            //selCreater.Items.Insert(0, new ListItem("请选择", "-1"));
        }
        */
    }
}