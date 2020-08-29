using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class SelectSurveyInfo : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string SurveyName
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SurveyName"));
            }
        }
        public string BGID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BGID"));
            }
        }
        public string SCID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SCID"));
            }
        }
        public string CreateUserID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CreateUserID"));
            }
        }
        private int PageSize = 10;
        public int GroupLength = 8;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SurveyInfoBind();
                UserGroupBind();
                CreateUserBind();
            }
        }


        private void SurveyInfoBind()
        {
            Entities.QuerySurveyInfo query = new Entities.QuerySurveyInfo();
            if (!string.IsNullOrEmpty(SurveyName))
            {
                query.Name = SurveyName;
            }
            int bgId = 0;
            if (int.TryParse(BGID, out bgId) && bgId > 0)
            {
                query.BGID = bgId;
            }
            int scId = 0;
            if (int.TryParse(SCID, out scId) && scId > 0)
            {
                query.SCID = scId;
            }
            int userId = 0;
            if (int.TryParse(CreateUserID, out userId) && userId > 0)
            {
                query.CreateUserID = userId;
            }
            query.IsAvailable = 1;
            query.LoginID = BLL.Util.GetLoginUserID();
            int totalCount = 0;
            DataTable dt = BLL.SurveyInfo.Instance.GetSurveyInfo(query, "SurveyInfo.CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, out totalCount);
            rptSurvey.DataSource = dt;
            rptSurvey.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        private void UserGroupBind()
        {
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId);
            sltUserGroupPop.DataSource = dt;
            sltUserGroupPop.DataTextField = "Name";
            sltUserGroupPop.DataValueField = "BGID";
            sltUserGroupPop.DataBind();
            sltUserGroupPop.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        private void CreateUserBind()
        {
            DataTable dt = BLL.SurveyInfo.Instance.getCreateUser();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dt.Rows[i]["CreateUserID"].ToString()));
                if (userName != string.Empty)
                {
                    sltCreateUserPop.Items.Add(new ListItem(userName, dt.Rows[i]["CreateUserID"].ToString()));
                }
            }
            sltCreateUserPop.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        public string ShowBussniessGroupName(string bgIdStr)
        {
            int bgId = -1;
            string name = string.Empty;
            if (int.TryParse(bgIdStr, out bgId))
            {
                DataTable bgBt = BLL.BusinessGroup.Instance.GetBusinessGroupByBGID(bgId);
                if (bgBt != null && bgBt.Rows.Count > 0)
                {
                    name = bgBt.Rows[0]["Name"].ToString();
                }
            }

            return name;
        }

        public string ShowCreateUserName(string createUserId)
        {
            string userName = string.Empty;
            int userId = -1;
            if (int.TryParse(createUserId, out userId))
            {
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userId);
            }

            return userName;
        }
    }
}