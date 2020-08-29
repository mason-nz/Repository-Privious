using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class Dispose : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 
        public string SPIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SPIID"));
            }
        }
        #endregion
        public string surveyCategoryStr;
        public string BeginDateTime;
        public string EndDateTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                UserGroupBind();
                SurveyProjectInfoBind();
            }
        }

        private void SurveyProjectInfoBind()
        {
            int spIid = 0;
            if (int.TryParse(SPIID, out spIid))
            {
                Entities.QuerySurveyProjectInfo query = new QuerySurveyProjectInfo();
                //query.BGIDStr = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRightBgIDStrByUserID(BLL.Util.GetLoginUserID());
                query.LoginUserID = BLL.Util.GetLoginUserID();
                query.SPIID = spIid;
                int totalCount = 0;
                BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query, "", 1, 1, out totalCount);
                if (totalCount > 0)
                {
                    Entities.SurveyProjectInfo info = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spIid);
                    if (info != null)
                    {
                        if (info.Status == 1 || ((int)info.Status == 0 && DateTime.Compare((DateTime)info.SurveyStartTime, DateTime.Now) < 0))
                        {
                            Response.Write("<script>location.href='about:blank'</script>;");
                        }
                        txtProjectName.Value = info.Name;
                        sltUserGroup.Value = info.BGID.ToString();
                        sltSurveyCategory.Value = info.SCID.ToString();
                        surveyCategoryStr = info.SCID.ToString();
                        txtDescription.Value = info.Description;
                        Entities.SurveyInfo surveyInfo = BLL.SurveyInfo.Instance.GetSurveyInfo((int)info.SIID);
                        txtSurveyName.Value = surveyInfo.Name;
                        hdnSIID.Value = surveyInfo.SIID.ToString();
                        txtBusinessGroup.Value = info.BusinessGroup;
                        BeginDateTime = info.SurveyStartTime.ToString();
                        EndDateTime = info.SurveyEndTime.ToString();

                        DataTable dt = BLL.SurveyPerson.Instance.GetSurveyPersonBySPIID(info.SPIID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string personNames = string.Empty;
                            string personIds = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dt.Rows[i]["ExamPersonID"].ToString()));
                                if (i > 0)
                                {
                                    personNames += ",";
                                    personIds += ",";
                                }
                                personNames += userName;
                                personIds += dt.Rows[i]["ExamPersonID"].ToString();
                            }
                            txtExamPersonNames.Value = personNames;
                            hdnPersonIDS.Value = personIds;
                        }
                    }
                }
                else
                {
                    Response.Write("<script>location.href='about:blank'</script>;");
                }
            }
        }

        private void UserGroupBind()
        {
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userId);
            sltUserGroup.DataSource = dt;
            sltUserGroup.DataTextField = "Name";
            sltUserGroup.DataValueField = "BGID";
            sltUserGroup.DataBind();
        }
    }
}