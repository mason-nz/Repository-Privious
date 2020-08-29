using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class EditProject : PageBase
    {
        public string ProjectID
        {
            get
            {
                if (HttpContext.Current.Request["projectid"] == null)
                {
                    return "";
                }
                int intVal = 0;

                if (!int.TryParse(HttpContext.Current.Request["projectid"], out intVal))
                {
                    return "";
                }
                else
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
                }
            }
        }
        public string DataCount = "0"; //关联数据个数
        public string SurveryCount = "0";//关联问卷个数
        public string SourceID = "";//数据来源   1 excel导入  2 CRM选择
        public string statusId = "";//状态
        public string nowDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//当前时间
        public string selectDataIDs = "";//关联数据ID列表
        public string TTCode = "";
        public string BGID = "";
        public string CID = "";
        public string importhistroycss = "display:none";//是否显示数据导入日志
        /// 是否进行黑名单验证
        /// <summary>
        /// 是否进行黑名单验证
        /// </summary>
        public int IsBlacklistCheck { get; set; }
        /// 验证方式
        /// <summary>
        /// 验证方式
        /// </summary>
        public int BlackListCheckType { get; set; }

        public string OperName
        {
            get
            {
                if (ProjectID == "")
                {
                    return "新建";
                }
                else
                {
                    return "修改";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProjectID != "")
                {
                    Entities.ProjectInfo model = new Entities.ProjectInfo();
                    model = BLL.ProjectInfo.Instance.GetProjectInfo(int.Parse(ProjectID));
                    if (model != null)
                    {
                        if (model.Source == 1 || model.Source == 2 || model.Source == 3 || model.Source == 4)
                        {
                            importhistroycss = "display:block";
                        }
                        this.txtProjectName.Value = model.Name;
                        BGID = model.BGID.ToString();
                        CID = model.PCatageID.ToString();
                        this.txtDescription.Value = model.Notes;

                        SourceID = model.Source.ToString();
                        statusId = model.Status.ToString();
                        TTCode = model.TTCode;
                        IsBlacklistCheck = model.IsBlacklistCheck.GetValueOrDefault(0);
                        BlackListCheckType = model.BlacklistCheckType.GetValueOrDefault(0);

                        selectDataIDs = BLL.ProjectDataSoure.Instance.GetProjectDataSoureID(model.ProjectID, out DataCount, true);

                        #region 计算问卷个数

                        int totalCount = 0;
                        Entities.QueryProjectSurveyMapping mapQuery = new Entities.QueryProjectSurveyMapping();
                        mapQuery.ProjectID = int.Parse(ProjectID);
                        DataTable dt = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(mapQuery, "", 1, 999, out totalCount);
                        if (dt != null)
                        {
                            SurveryCount = dt.Rows.Count.ToString();
                        }

                        #endregion
                    }
                }
            }
        }

        private void UserGroupBind()
        {
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId);
            sltUserGroup.DataSource = dt;
            sltUserGroup.DataTextField = "Name";
            sltUserGroup.DataValueField = "BGID";
            sltUserGroup.DataBind();

            sltUserGroup.Value = "2";
        }
    }


}