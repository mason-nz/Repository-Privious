using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupManage
{
    public partial class CategoryManageList : PageBase
    {
        public bool AddAuth = false;
        public int pageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        #region 属性
        public string RequestPopGroup
        {
            get { return HttpContext.Current.Request["popGroup"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["popGroup"].ToString()); }
        }
        #endregion

        public string TypeId
        {
            get
            {
                if (Request["typeId"] != null)
                {
                    return HttpUtility.UrlDecode(Request["typeId"].ToString());
                }
                else
                {
                    return "";
                }
            }
        }

        public string Status
        {
            get
            {
                if (Request["status"] != null)
                {
                    return HttpUtility.UrlDecode(Request["status"].ToString());
                }
                else
                {
                    return "";
                }
            }
        }
        int userID = 0;
        public string categoryName = "";

        public string getCurrentPage()
        {
            return BLL.PageCommon.Instance.PageIndex.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD500802"))//"分类管理"功能验证逻辑
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                AddAuth = BLL.Util.CheckRight(userID, "SYS024BUT500801");
                BindData();
            }
        }

        private void BindData()
        {
            Entities.QuerySurveyCategory query = new Entities.QuerySurveyCategory();
            if (RequestPopGroup != "-1")
            {
                query.BGID = int.Parse(RequestPopGroup);
            }

            if (TypeId == "0") //默认分组
            {
                if (Status == "1")
                {
                    query.Status = 8;  //没有交集
                }
                else
                {
                    query.Status = -3;
                }
            }
            else if (TypeId == "1") //自定义分组
            {
                if (Status == "0")
                {
                    query.Status = int.Parse(Status);
                    //query.GroupStatus = "(0,1)";
                }
                else if (Status == "1")
                {
                    query.Status = int.Parse(Status);
                    //query.GroupStatus = "(0,1)";
                }
                else
                {
                    query.GroupStatus = "(0,1)";
                }
            }
            else if (TypeId == "0,1" || TypeId == "-1")
            {
                if (Status == "-1" || Status == "0,1")
                {
                    query.GroupStatus = "(0,1,-3)";
                }
                else if (Status == "0")
                {
                    //query.Status = int.Parse(Status);
                    query.GroupStatus = "(0,-3)";
                }
                else
                {
                    query.Status = int.Parse(Status);
                }
            }

            //if (Status != "-1" && Status != "0,1")
            //{
            //    query.Status = int.Parse(Status);
            //}
            query.TypeId = 2;  //TypeId
            query.LoginID = userID;
            query.SelectType = 1;
            
            DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "SurveyCategory.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                categoryName += dt.Rows[i]["Name"].ToString() + ",";
            }
            categoryName = categoryName.TrimEnd(',');

            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
            }
        }

        /// 获取状态名称
        /// <summary>
        /// 获取状态名称
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetStatusName(string status)
        {
            return status == "0" ? "在用" : "停用";
        }
    }
}