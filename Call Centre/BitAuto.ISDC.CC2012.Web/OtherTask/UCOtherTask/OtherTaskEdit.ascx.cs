using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Reflection;
using System.ComponentModel;

namespace BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask
{
    public partial class OtherTaskEdit : System.Web.UI.UserControl
    {
        //任务ＩＤ
        private string _requestTaskID;
        public string RequestTaskID
        {
            get
            {
                return _requestTaskID;
            }
            set
            {
                _requestTaskID = value;
            }
        }
        //问卷个数
        public int SurveyCount = 0;
        //问卷列表
        public string SIIDStr = null;
        //表单名称
        public string TPName = string.Empty;
        //自定义表编号
        public string TTCode = string.Empty;
        //自定义表主键
        public string RelationID = string.Empty;
        public string ProJectID = string.Empty;
        public string MyClientScript { get; set; }

        //是否测试版本
        public bool IsTest { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MyClientScript = Page.ClientScript.GetPostBackEventReference(Button_Async, "");

                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                //加载自定义表单信息
                LoadTPage();
                //缓存参数
                ViewState["RequestTaskID"] = RequestTaskID;
                //加载问卷
                //LoadSurvey();
                //确认版本
                string url = Request.Url.Host;
                if (url.ToLower().Contains("ncc.sys1.bitauto.com"))
                {
                    IsTest = true;
                }
                else
                {
                    IsTest = false;
                }
            }
        }

        /// 加载表单
        /// <summary>
        /// 加载表单
        /// </summary>
        protected void LoadTPage()
        {
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                Entities.OtherTaskInfo model = null;
                //根据任务id，取自定义数据表编号，自定义数据表主键
                model = BLL.OtherTaskInfo.Instance.GetOtherTaskInfo(RequestTaskID);
                if (model != null)
                {
                    RelationID = model.RelationID;
                    TTCode = model.RelationTableID;
                    ProJectID = model.ProjectID.ToString();
                    Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
                    query.ProjectID = model.ProjectID;
                    int totalCount = 0;
                    DataTable dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, "", 1, 999, out totalCount);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        TPName = dt.Rows[0]["Name"].ToString();
                    }
                }
            }
        }
        /// 加载问卷
        /// <summary>
        /// 加载问卷
        /// </summary>
        protected void LoadSurvey()
        {
            RequestTaskID = CommonFunction.ObjectToString(ViewState["RequestTaskID"]);
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                DataTable SurveyinfoDt = null;
                SurveyinfoDt = BLL.ProjectSurveyMapping.Instance.GetSurveyinfoByOtherTaskID(RequestTaskID);
                if (SurveyinfoDt != null && SurveyinfoDt.Rows.Count > 0)
                {
                    for (int i = 0; i < SurveyinfoDt.Rows.Count; i++)
                    {
                        QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
                        query.PTID = RequestTaskID;
                        int siid = CommonFunction.ObjectToInteger(SurveyinfoDt.Rows[i]["SIID"]);
                        query.SIID = siid;
                        //查询问卷个数：只取总数
                        int RowCount = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer_Count(query);
                        //当前时间不在周期里，并且问卷没有提交过答案
                        DateTime beginTime = CommonFunction.ObjectToDateTime(SurveyinfoDt.Rows[i]["begindate"]);
                        DateTime endTime = CommonFunction.ObjectToDateTime(SurveyinfoDt.Rows[i]["endDate"]);

                        //答过问卷，或者问卷当前时间在问卷周期内
                        if (RowCount > 0 || (System.DateTime.Now >= beginTime && System.DateTime.Now <= endTime))
                        {
                            //问卷控件
                            Control surveyCtl;
                            Entities.SurveyInfo model = new Entities.SurveyInfo();
                            model.TaskID = RequestTaskID;
                            model.SIID = siid;
                            model.Num = SurveyCount;
                            model.Name = SurveyinfoDt.Rows[i]["Name"].ToString();
                            //如果是查看
                            surveyCtl = this.LoadControl("~/OtherTask/UCOtherTask/UCSurveyInfoEdit.ascx", model);
                            //把问卷保存在数据组中
                            SIIDStr += model.SIID.ToString() + ",";
                            //共有多少问卷
                            SurveyCount = SurveyCount + 1;
                            this.PlaceHolderSurvey.Controls.Add(surveyCtl);
                        }
                    }
                    if (!string.IsNullOrEmpty(SIIDStr))
                    {
                        SIIDStr = SIIDStr.Substring(0, SIIDStr.Length - 1);
                    }

                    hiddenSurveyCount.Value = SurveyCount.ToString();
                    hiddenSIIDStr.Value = SIIDStr;
                }
            }

            //调用前台代码，初始化跳题逻辑
            GotoNumQuestion();
        }

        private void GotoNumQuestion()
        {
            //如果有UpdatePanel就用如下代码调用前台js
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.Page.GetType(), "", "GotoNumQuestion();", true);
            //如果没有就如下代码
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script>GotoNumQuestion();</script>", true);
        }
        /// 重写LoadControl，带参数。
        /// <summary>
        /// 重写LoadControl，带参数。
        /// </summary>
        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }
            // Finally return the fully initialized UC
            return ctl;
        }

        protected void Button_Async_Click(object sender, EventArgs e)
        {
            //加载问卷
            LoadSurvey();
        }
    }
}