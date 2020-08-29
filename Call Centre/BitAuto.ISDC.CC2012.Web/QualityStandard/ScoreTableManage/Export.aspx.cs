using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage
{
    public partial class Export : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        ///
        /// </summary>
        public string Name
        {
            get
            {
                return HttpContext.Current.Request["Name"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["Name"].ToString());
            }
        }
        public string RuleTableStatus
        {
            get
            {
                return HttpContext.Current.Request["RuleTableStatus"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["RuleTableStatus"].ToString());
            }
        }
        public string CreateUserID
        {
            get
            {
                return HttpContext.Current.Request["CreateUserID"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["CreateUserID"].ToString());
            }
        }
        public string BeginTime
        {
            get
            {
                return HttpContext.Current.Request["BeginTime"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["BeginTime"].ToString());
            }
        }
        public string EndTime
        {
            get
            {
                return HttpContext.Current.Request["EndTime"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["EndTime"].ToString());
            }
        }

        private int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT600204"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                BindData();
            }
        }


        private void BindData()
        {
            Entities.QueryQS_RulesTable query = new Entities.QueryQS_RulesTable();
            if (Name != "")
            {
                query.Name = Name;
            }
            if (RuleTableStatus != "")
            {
                query.RuleTableStatus = RuleTableStatus;
            }
            if (BeginTime != "")
            {
                query.BeginTime = BeginTime;
            }
            if (EndTime != "")
            {
                query.EndTime = EndTime;
            }

            #region 调整分组前数据权限
            /*
            //判断数据权限，数据权限如果为 2-全部，则查看所有数据
            Entities.UserDataRigth model_userDataRight = BLL.UserDataRigth.Instance.GetUserDataRigth(userID);
            if (model_userDataRight != null)
            {
                if (model_userDataRight.RightType != 2)//数据权限不为 2-全部
                {
                    query.LoginID = userID;
                    //判断分组权限，如果权限是2-本组，则能看到本组人创建的信息；如果权限是1-本人，则只能看本人创建的信息 
                    DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userID);
                    string ownGroup = string.Empty;//权限是本组的 组串
                    string oneSelf = string.Empty; //权限是本人的 组串
                    for (int i = 0; i < dt_userGroupDataRight.Rows.Count; i++)
                    {
                        if (dt_userGroupDataRight.Rows[i]["RightType"].ToString() == "2")
                        {
                            ownGroup += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
                        }
                        if (dt_userGroupDataRight.Rows[i]["RightType"].ToString() == "1")
                        {
                            oneSelf += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
                        }
                    }
                    query.OwnGroup = ownGroup.TrimEnd(',');
                    query.OneSelf = oneSelf.TrimEnd(',');
                }
            }
            */
            #endregion

            #region 判断数据权限
            //query.LoginID = userID;
            ////问题是说这个人离职了，然后别人看不到他的数据了,拿掉数据权限
            ////判断分组权限 
            //DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userID);
            //string ownGroup = string.Empty;//权限是本组的 组串
            //string oneSelf = string.Empty; //权限是本人的 组串
            //for (int i = 0; i < dt_userGroupDataRight.Rows.Count; i++)
            //{
            //    ownGroup += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
            //}
            //query.OwnGroup = ownGroup.TrimEnd(',');
            #endregion
            query.LoginID = BLL.Util.GetLoginUserID();
            int RecordCount = 0;

            DataTable dt = BLL.QS_RulesTable.Instance.GetQS_RulesTable(query, " QS_RulesTable.CreateTime desc ", 1, -1, out RecordCount);

            dt.Columns.Add("copy_isInUse");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["copy_isInUse"] = dt.Rows[i]["isInUse"].ToString() == "0" ? "未使用" : "已使用";

                string regionid = dt.Rows[i]["RegionID"].ToString();
                dt.Rows[i]["RegionID"] = regionid == "1" ? "北京" : regionid == "2" ? "西安" : regionid == "1,2" ? "北京、西安" : "";
            }

            //要导出的字段
            Dictionary<string, string> ExportColums = new Dictionary<string, string>();
            ExportColums.Add("name", "评分表名称");
            ExportColums.Add("createtime", "创建日期");
            ExportColums.Add("truename", "创建人");
            ExportColums.Add("groups", "应用范围");
            ExportColums.Add("statusname", "状态");
            ExportColums.Add("copy_isinuse", "使用状态");
            ExportColums.Add("regionid", "适用区域");

            //字段排序
            dt.Columns["Name"].SetOrdinal(0);
            dt.Columns["CreateTime"].SetOrdinal(1);
            dt.Columns["TrueName"].SetOrdinal(2);
            dt.Columns["Groups"].SetOrdinal(3);
            dt.Columns["StatusName"].SetOrdinal(4);
            dt.Columns["copy_isInUse"].SetOrdinal(5);
            dt.Columns["RegionID"].SetOrdinal(6);

            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (ExportColums.ContainsKey(dt.Columns[i].ColumnName.ToLower()))
                {
                    //字段时要导出的字段，改名
                    dt.Columns[i].ColumnName = ExportColums[dt.Columns[i].ColumnName.ToLower()];
                }
                else
                {
                    //不是要导出的字段，删除
                    dt.Columns.RemoveAt(i);
                }
            }
            BLL.Util.ExportToCSV("质检评分表", dt);
        }
    }
}