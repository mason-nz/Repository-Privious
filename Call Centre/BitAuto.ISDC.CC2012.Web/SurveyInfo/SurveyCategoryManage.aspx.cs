using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo
{
    public partial class SurveyCategoryManage : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
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
                if (Request["TypeId"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TypeId"].ToString());
                }
                else
                {
                    return "1";
                }
            }
        }

        int userID = 0;
        public string categoryName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID(); 
                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            Entities.QuerySurveyCategory query = new Entities.QuerySurveyCategory();
            if (RequestPopGroup != "")
            {
                query.BGID = int.Parse(RequestPopGroup);
            }
            else if (ddlGroup.Value != "-1" && ddlGroup.Value != "")
            {
                query.BGID = int.Parse(ddlGroup.Value);
            }

            query.TypeId = int.Parse(TypeId);
            query.LoginID = userID;
            query.SelectType = 1;
            ////判断数据权限，数据权限如果为 2-全部，则绑定所有业务组数据
            //Entities.UserDataRigth model_userDataRight = BLL.UserDataRigth.Instance.GetUserDataRigth(userID);
            //if (model_userDataRight != null)
            //{
            //    if (model_userDataRight.RightType != 2)//数据权限不为 2-全部,则加载该登陆者所属业务组
            //    {
            //        query.LoginID = userID;
            //        query.SelectType = 1;
            //    }
            //}

            int count;
            DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "SurveyCategory.CreateTime Desc", 1, 10000, out count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                categoryName += dt.Rows[i]["Name"].ToString() + ",";
            }
            categoryName = categoryName.TrimEnd(',');

            repeaterCategoryList.DataSource = dt;
            repeaterCategoryList.DataBind();
        }
    }
}