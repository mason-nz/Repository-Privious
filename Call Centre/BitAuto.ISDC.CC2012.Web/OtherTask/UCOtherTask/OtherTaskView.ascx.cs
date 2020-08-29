using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask
{
    public partial class OtherTaskView : System.Web.UI.UserControl
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
        //问卷名称
        private string _siidName = string.Empty;
        public string SIIDName
        {
            get
            {
                return _siidName;
            }
            set
            {
                _siidName = value;
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




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                //加载自定义表单信息
                LoadTPage();
                //加载问卷
                LoadSurvey();
            }
        }



        /// <summary>
        /// 加载问卷
        /// </summary>
        protected void LoadSurvey()
        {
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                DataTable dt = null;
                dt = BLL.ProjectSurveyMapping.Instance.GetSurveyinfoByOtherTaskID(RequestTaskID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int RowCount = 0;
                        QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
                        query.PTID = RequestTaskID;
                        int _siid = 0;

                        int.TryParse(dt.Rows[i]["SIID"].ToString(), out _siid);

                        query.SIID = _siid;
                        DataTable dtnew = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(query, "", 1, 1000000, out RowCount);
                        //当前时间不在周期里，并且问卷没有提交过答案
                        DateTime _beginTime = new DateTime();
                        DateTime _endTime = new DateTime();
                        DateTime.TryParse(dt.Rows[i]["begindate"].ToString(), out _beginTime);
                        DateTime.TryParse(dt.Rows[i]["endDate"].ToString(), out _endTime);
                        //问卷控件
                        Control ctl;

                        //答过问卷，或者问卷当前时间在问卷周期内
                        if (RowCount > 0 || (System.DateTime.Now >= _beginTime && System.DateTime.Now <= _endTime))
                        {
                            Entities.SurveyInfo model = new Entities.SurveyInfo();
                            model.TaskID = RequestTaskID;
                            model.SIID = _siid;
                            model.Num = SurveyCount;
                            model.Name = dt.Rows[i]["Name"].ToString();
                            ctl = this.LoadControl("~/OtherTask/UCOtherTask/UCSurveyInfoView.ascx", model);
                            //把问卷保存在数据组中
                            SIIDStr += model.SIID.ToString() + ",";
                            //共有多少问卷
                            SurveyCount = SurveyCount + 1;
                            this.PlaceHolderSurvey.Controls.Add(ctl);
                        }
                    }
                    if (!string.IsNullOrEmpty(SIIDStr))
                    {
                        SIIDStr = SIIDStr.Substring(0, SIIDStr.Length - 1);
                    }
                }
            }
        }


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

        /// <summary>
        /// 加载页面元素
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
    }
}