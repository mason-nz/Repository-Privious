using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo
{
    /// <summary>
    /// SurveyInfoListHandle 的摘要说明
    /// </summary>
    public class SurveyInfoListHandle : IHttpHandler, IRequiresSessionState
    {
        #region 属性

        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        private string RequestSCID
        {
            get { return HttpContext.Current.Request["SCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SCID"].ToString()); }
        }
        private string RequestBGID
        {
            get { return HttpContext.Current.Request["BGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BGID"].ToString()); }
        }
        private string RequestName
        {
            get { return HttpContext.Current.Request["Name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Name"].ToString()); }
        }
        private string RequestStatus
        {
            get { return HttpContext.Current.Request["Status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString()); }
        }

        //surveyInfo表
        private string RequestSIID
        {
            get { return HttpContext.Current.Request["SIID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SIID"].ToString()); }
        }
        private string RequestDescription
        {
            get { return HttpContext.Current.Request["Description"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Description"].ToString()); }
        }
        private string RequestIsAvailable
        {
            get { return HttpContext.Current.Request["IsAvailable"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsAvailable"].ToString()); }
        }
        private string RequestCreateTime
        {
            get { return HttpContext.Current.Request["CreateTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreateTime"].ToString()); }
        }
        private string RequestCreateUserID
        {
            get { return HttpContext.Current.Request["CreateUserID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreateUserID"].ToString()); }
        }


        private string RequestCategoryName
        {
            get { return HttpContext.Current.Request["CategoryName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CategoryName"].ToString()); }
        }

        #endregion

        public string TypeId
        {
            get
            {
                if (HttpContext.Current.Request["TypeId"] != null)
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request["TypeId"].ToString());
                }
                else
                {
                    return "1";
                }
            }
        }


        int userID = 0;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            userID = BLL.Util.GetLoginUserID();

            switch (RequestAction.ToLower())
            {
                case "surveycategoryupdate":
                    if (BLL.Util.CheckRight(userID, "SYS024MOD5008") || BLL.Util.CheckRight(userID, "SYS024MOD5102") || BLL.Util.CheckRight(userID, "SYS024MOD500601"))  // 由于此方法被分类管理、数据模板、项目管理使用，所以需要这样判断 
                    {
                        surveyCategoryUpdate(out msg);
                    }
                    else
                    {
                        msg = "{msg:'您没有执行此操作的权限！'}";
                    }
                    break;
                case "surveycategoryinsert":
                    if (BLL.Util.CheckRight(userID, "SYS024MOD5102") || BLL.Util.CheckRight(userID, "SYS024BUT5009") || BLL.Util.CheckRight(userID, "SYS024MOD500802") || BLL.Util.CheckRight(userID, "SYS024MOD500601")) //分类管理、数据模板、项目管理、调查问卷管理
                    {
                        surveyCategoryInsert(out msg);
                    }
                    else
                    {
                        msg = "{msg:'您没有执行此操作的权限！'}";
                    }
                    break;
                case "surveyinfoupdate":
                    if (BLL.Util.CheckRight(userID, "SYS024BUT5004") || BLL.Util.CheckRight(userID, "SYS024BUT5006") || BLL.Util.CheckRight(userID, "SYS024BUT5007")) //调查问卷——删除、停用、启用
                    {
                        surveyInfoUpdate(out msg);
                    }
                    else
                    {
                        msg = "{msg:'您没有执行此操作的权限！'}";
                    }
                    break;
                case "surveycreatenewquestionpaper":
                    if (!BLL.Util.CheckRight(userID, "SYS024BUT5005"))
                    {
                        msg = "{msg:'您没有执行此操作的权限！'}";
                    }
                    else
                    {
                        surveyCreateNewQuestionPaper(out msg);
                    }
                    break;
                case "updatesurveycategorystatus":
                    UpdateSurveyCategoryStatus(out msg);
                    break;

                case "isexistscategoryname":  //是否存在分类名称相同
                    IsExistsCategoryName(out msg);
                    break;
            }

            context.Response.Write(msg);
        }

        //调查问卷修改（包括删除、停用、启用）
        private void surveyInfoUpdate(out string msg)
        {
            msg = string.Empty;
            int _siid;
            if (int.TryParse(RequestSIID, out _siid))
            {
                Entities.SurveyInfo model = BLL.SurveyInfo.Instance.GetSurveyInfo(_siid);
                if (model != null)
                {
                    //select存储过程会找如果是已使用的赋值为2，此时数据库仍然为1，所以应改回到1-未使用
                    if (model.Status == 2)
                    {
                        model.Status = 1;
                    }

                    if (RequestStatus != "")
                    {
                        model.Status = int.Parse(RequestStatus);
                    }
                    string oldAvailable = string.Empty;
                    string newAvailable = string.Empty;
                    if (RequestIsAvailable != "")
                    {
                        oldAvailable = model.IsAvailable == 1 ? "启用" : "停用";

                        model.IsAvailable = int.Parse(RequestIsAvailable);

                        newAvailable = model.IsAvailable == 1 ? "启用" : "停用";
                    }
                    try
                    {
                        int result = BLL.SurveyInfo.Instance.Update(model);
                        if (result == 1)
                        {
                            msg = "{msg:'操作成功'}";

                            //插入日志
                            string logType = string.Empty;

                            if (model.Status == -1)//删除
                            {
                                logType += "调查问卷【删除】主键【" + model.SIID + "】问卷名称【" + model.Name + "】问卷";
                            }
                            else if (oldAvailable != string.Empty && newAvailable != string.Empty)  //启用、停用
                            {
                                logType += "调查问卷【修改】主键【" + model.SIID + "】问卷名称【" + model.Name + "】是否可用由【" + oldAvailable + "】修改为【" + newAvailable + "】";
                            }

                            BLL.Util.InsertUserLog(logType);
                        }
                        else
                        {
                            msg = "{msg:'操作失败'}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{msg:'" + ex.Message + "' 操作失败}";
                    }
                }
            }
        }

        //生成新问卷
        private void surveyCreateNewQuestionPaper(out string msg)
        {
            msg = string.Empty;
            int _siid;
            if (int.TryParse(RequestSIID, out _siid))
            {
                //生成新的调查问卷
                Entities.SurveyInfo model_newSurveyInfo = new Entities.SurveyInfo();
                //生成该调查问卷新的调查问卷试题列表
                List<Entities.SurveyQuestion> list_newSurveyQuestion = new List<Entities.SurveyQuestion>();
                //生成该调查问卷选项列表
                List<Entities.SurveyOption> list_newSurveyOption = new List<Entities.SurveyOption>();
                //生成该调查问卷矩阵标题列表
                List<Entities.SurveyMatrixTitle> list_newSurveyMatrixTitle = new List<Entities.SurveyMatrixTitle>();

                #region 准备数据

                Entities.SurveyInfo model_surveyInfo = new Entities.SurveyInfo();
                model_surveyInfo = BLL.SurveyInfo.Instance.GetSurveyInfo(_siid);
                if (model_surveyInfo == null)
                {
                    msg = "{msg:'未找到该问卷，生成新问卷失败'}";
                }

                //准备 新调查问卷 数据

                //判断该问卷是不是之前新生成问卷，如果是，就不再加(新生成问卷) 

                if (model_surveyInfo.Name.Length >= 7)
                {
                    if (model_surveyInfo.Name.Substring(model_surveyInfo.Name.Length - 7, 7) != "(新生成问卷)")
                    {
                        model_newSurveyInfo.Name = model_surveyInfo.Name + "(新生成问卷)";
                    }
                    else
                    { model_newSurveyInfo.Name = model_surveyInfo.Name; }
                }
                else
                {
                    model_newSurveyInfo.Name = model_surveyInfo.Name + "(新生成问卷)";
                }
                model_newSurveyInfo.BGID = model_surveyInfo.BGID;
                model_newSurveyInfo.SCID = model_surveyInfo.SCID;
                model_newSurveyInfo.Description = model_surveyInfo.Description;
                model_newSurveyInfo.Status = 0;     //未完成
                model_newSurveyInfo.IsAvailable = -1;//不可用
                model_newSurveyInfo.CreateTime = DateTime.Now;
                model_newSurveyInfo.CreateUserID = userID;

                //准备 新调查问卷试题 数据
                List<Entities.SurveyQuestion> list_surveyQuestion = new List<Entities.SurveyQuestion>();
                list_surveyQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestionList(_siid);
                foreach (Entities.SurveyQuestion o in list_surveyQuestion)
                {
                    Entities.SurveyQuestion model_surveyQuestion = new Entities.SurveyQuestion();
                    model_surveyQuestion.SQID = o.SQID;
                    model_surveyQuestion.Ask = o.Ask;
                    model_surveyQuestion.AskCategory = o.AskCategory;
                    model_surveyQuestion.ShowColumnNum = o.ShowColumnNum;
                    model_surveyQuestion.MaxTextLen = o.MaxTextLen;
                    model_surveyQuestion.MinTextLen = o.MinTextLen;
                    model_surveyQuestion.Status = 0;
                    model_surveyQuestion.OrderNum = o.OrderNum;
                    model_surveyQuestion.CreateTime = DateTime.Now;
                    model_surveyQuestion.CreateUserID = userID;
                    model_surveyQuestion.IsMustAnswer = o.IsMustAnswer;
                    model_surveyQuestion.IsStatByScore = o.IsStatByScore;

                    list_newSurveyQuestion.Add(model_surveyQuestion);
                }

                //准备 新调查问卷选项 数据
                List<Entities.SurveyOption> list_surveyOption = new List<Entities.SurveyOption>();
                list_surveyOption = BLL.SurveyOption.Instance.GetSurveyOptionList(_siid);
                foreach (Entities.SurveyOption o in list_surveyOption)
                {
                    Entities.SurveyOption model_surveyOption = new Entities.SurveyOption();
                    model_surveyOption.SQID = o.SQID;
                    model_surveyOption.OptionName = o.OptionName;
                    model_surveyOption.IsBlank = o.IsBlank;
                    model_surveyOption.Score = o.Score;
                    model_surveyOption.OrderNum = o.OrderNum;
                    model_surveyOption.Status = 0;
                    model_surveyOption.CreateTime = DateTime.Now;
                    model_surveyOption.CreateUserID = userID;

                    list_newSurveyOption.Add(model_surveyOption);
                }

                //准备 新调查问卷矩阵标题 数据
                List<Entities.SurveyMatrixTitle> list_surveyMatrixTitle = new List<Entities.SurveyMatrixTitle>();
                list_surveyMatrixTitle = BLL.SurveyMatrixTitle.Instance.GetMatrixTitleList(_siid);
                foreach (Entities.SurveyMatrixTitle o in list_surveyMatrixTitle)
                {
                    Entities.SurveyMatrixTitle model_surveyMatrixTitle = new Entities.SurveyMatrixTitle();
                    model_surveyMatrixTitle.SQID = o.SQID;
                    model_surveyMatrixTitle.TitleName = o.TitleName;
                    model_surveyMatrixTitle.Status = 0;
                    model_surveyMatrixTitle.Type = o.Type;
                    model_surveyMatrixTitle.CreateTime = DateTime.Now;
                    model_surveyMatrixTitle.CreateUserID = userID;

                    list_newSurveyMatrixTitle.Add(model_surveyMatrixTitle);
                }

                #endregion

                #region 事务处理 插入

                string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
                SqlConnection connection = new SqlConnection(connectionstrings);
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
                try
                {
                    //插入调查问卷
                    int newSIID = BLL.SurveyInfo.Instance.Insert(tran, model_newSurveyInfo);

                    //插入调查问卷试题
                    int newSQID = 0;
                    int oldSQID = 0;
                    for (int i = 0; i < list_newSurveyQuestion.Count; i++)
                    {
                        oldSQID = list_newSurveyQuestion[i].SQID;

                        list_newSurveyQuestion[i].SIID = newSIID;
                        newSQID = BLL.SurveyQuestion.Instance.Insert(tran, list_newSurveyQuestion[i]);

                        //插入调查问卷选项 
                        for (int j = 0; j < list_newSurveyOption.Count; j++)
                        {
                            if (list_newSurveyOption[j].SQID == oldSQID)
                            {
                                list_newSurveyOption[j].SIID = newSIID;
                                list_newSurveyOption[j].SQID = newSQID;
                                BLL.SurveyOption.Instance.Insert(tran, list_newSurveyOption[j]);
                            }
                        }

                        //插入调查问卷矩阵标题
                        for (int k = 0; k < list_newSurveyMatrixTitle.Count; k++)
                        {
                            if (list_newSurveyMatrixTitle[k].SQID == oldSQID)
                            {
                                list_newSurveyMatrixTitle[k].SIID = newSIID;
                                list_newSurveyMatrixTitle[k].SQID = newSQID;
                                BLL.SurveyMatrixTitle.Instance.Insert(tran, list_newSurveyMatrixTitle[k]);
                            }
                        }

                    }

                    //插入日志
                    BLL.Util.InsertUserLog(tran, "【生成新问卷】操作成功，生成调查名称【" + model_newSurveyInfo.Name + "】业务分组ID【" + model_newSurveyInfo.BGID + "】分类ID【" + model_newSurveyInfo.SCID + "】的新问卷");

                    msg = "{msg:'success'}";

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    if (tran.Connection != null)
                    {
                        tran.Rollback();
                    }
                    msg = ex.Message.ToString();
                }
                finally
                {
                    connection.Close();
                }

                #endregion
            }
        }

        //问卷分类修改(包括编辑和删除)
        private void surveyCategoryUpdate(out string msg)
        {
            msg = string.Empty;
            int _scid;
            if (int.TryParse(RequestSCID, out _scid))
            {
                Entities.SurveyCategory model = BLL.SurveyCategory.Instance.GetSurveyCategory(_scid);
                if (model != null)
                {
                    string oldName = string.Empty;
                    string newName = string.Empty;

                    if (RequestName != "")
                    {
                        oldName = model.Name;
                        model.Name = RequestName;
                        newName = model.Name;
                    }

                    if (RequestStatus != "")
                    {
                        int count;

                        if (TypeId == "1")
                        {
                            //判断该分类在调查问卷或调查项目中使用，则不能删除
                            Entities.QuerySurveyInfo query_surveyInfo = new Entities.QuerySurveyInfo();
                            query_surveyInfo.SCID = _scid;
                            DataTable dt_surveyInfo = BLL.SurveyInfo.Instance.GetSurveyInfo(query_surveyInfo, "", 1, 10000, out count);
                            if (dt_surveyInfo.Rows.Count > 0)
                            {
                                msg = "{msg:'该分类在调查问卷中有使用，无法删除'}";
                                return;
                            }

                            //判断该分类在调查问卷或调查项目中使用，则不能删除
                            Entities.QuerySurveyProjectInfo query_projectInfo = new Entities.QuerySurveyProjectInfo();
                            query_projectInfo.SCID = _scid;
                            query_projectInfo.Status = 0;
                            DataTable dt_projectInfo = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query_projectInfo, "", 1, 10000, out count);
                            if (dt_projectInfo.Rows.Count > 0)
                            {
                                msg = "{msg:'该分类在调查项目中有使用，无法删除'}";
                                return;
                            }
                        }
                        else if (TypeId == "2")
                        {

                            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
                            query.PCatageID = _scid;
                            query.Status = 0;
                            DataTable dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, "", 1, 9999, out count);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                msg = "{msg:'该分类在数据清洗项目中有使用，无法删除'}";
                                return;
                            }
                        }

                        model.Status = int.Parse(RequestStatus);
                    }
                    int result = 0;
                    try
                    {
                        result = BLL.SurveyCategory.Instance.Update(model);

                        if (RequestStatus != "" && TypeId == "2")
                        {
                            //如果是其他任务的删除
                            BLL.CallRecord_ORIG_Business.Instance.DeleteBusinessUrl((int)model.BGID, (int)model.SCID);
                        }

                        if (result == 1)
                        {
                            msg = "{msg:'操作成功'}";

                            string logType = string.Empty;

                            if (TypeId == "1")
                            {
                                //插入日志
                                if (model.Status == -1)//删除
                                {
                                    logType += "问卷分类【删除】主键【" + model.SCID + "】分类名称【" + model.Name + "】问卷";
                                }
                                else if (oldName != string.Empty && newName != string.Empty)
                                {
                                    logType += "问卷分类【修改】主键【" + model.SCID + "】分类名称由【" + oldName + "】修改为【" + newName + "】";
                                }
                            }
                            else if (TypeId == "2")
                            {
                                //插入日志
                                if (model.Status == -1)//删除
                                {
                                    logType += "项目分类【删除】主键【" + model.SCID + "】分类名称【" + model.Name + "】问卷";
                                }
                                else if (oldName != string.Empty && newName != string.Empty)
                                {
                                    logType += "项目分类【修改】主键【" + model.SCID + "】分类名称由【" + oldName + "】修改为【" + newName + "】";
                                }
                            }

                            BLL.Util.InsertUserLog(logType);
                        }
                        else
                        {
                            msg = "{msg:'操作失败'}";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "{msg:'" + ex.Message + "'}";
                    }
                }
            }
        }

        //问卷分类新增
        private void surveyCategoryInsert(out string msg)
        {
            msg = string.Empty;
            string validator = string.Empty;
            string name = string.Empty;
            int bgid;

            #region 验证数据格式
            if (RequestName == "")
            {
                validator += "分类名称不能为空<br/>";
            }
            else
            {
                name = RequestName;
            }
            if (!int.TryParse(RequestBGID, out bgid))
            {
                validator += "所属分组格式不正确<br/>";
            }
            if (validator != string.Empty)
            {
                msg = "{msg:'" + validator + "'操作失败}";
                return;
            }
            #endregion

            Entities.SurveyCategory model = new Entities.SurveyCategory();
            model.Name = name;
            model.BGID = bgid;
            model.Status = 0;
            model.CreateTime = DateTime.Now;
            model.CreateUserID = userID;
            model.Level = -2;
            model.Pid = -2;
            model.TypeId = int.Parse(TypeId);


            int result = 0;
            try
            {

                result = BLL.SurveyCategory.Instance.Insert(model);

                if (TypeId == "2")
                {
                    //如果是其他任务的项目，在 CallRecord_ORIG_BusinessURL 添加一个URL
                    string webBaseUrl = ConfigurationUtil.GetAppSettingValue("ExitAddress");
                    webBaseUrl = webBaseUrl + "/OtherTask/OtherTaskDealView.aspx?OtherTaskID={0}";
                    BLL.CallRecord_ORIG_Business.Instance.AddBusinessUrl(bgid, result, webBaseUrl);
                }

                msg = "{msg:'操作成功'}";

                string logStr = string.Empty;
                logStr = "问卷分类【新增】一条分类名称【" + model.Name + "】所属分组【" + model.BGID + "】的记录";

                BLL.Util.InsertUserLog(logStr);
            }
            catch (Exception ex)
            {
                msg = "{msg:'" + ex.Message + "'}";
            }

        }

        private void UpdateSurveyCategoryStatus(out string msg)
        {
            msg = string.Empty;
            int _scid = 0;
            if (int.TryParse(RequestSCID, out _scid))
            {
                Entities.SurveyCategory model = BLL.SurveyCategory.Instance.GetSurveyCategory(_scid);
                if (model != null)
                {
                    int count;
                    if (model.Status == 0)
                    {
                        if (model.TypeId == 1)
                        {
                            //判断该分类在调查问卷或调查项目中使用，则不能删除
                            Entities.QuerySurveyInfo query_surveyInfo = new Entities.QuerySurveyInfo();
                            query_surveyInfo.SCID = _scid;
                            DataTable dt_surveyInfo = BLL.SurveyInfo.Instance.GetSurveyInfo(query_surveyInfo, "", 1, 10000, out count);
                            if (dt_surveyInfo.Rows.Count > 0)
                            {
                                msg = "{msg:'该分类在调查问卷中有使用，无法停用'}";
                                return;
                            }

                            //判断该分类在调查问卷或调查项目中使用，则不能删除
                            Entities.QuerySurveyProjectInfo query_projectInfo = new Entities.QuerySurveyProjectInfo();
                            query_projectInfo.SCID = _scid;
                            query_projectInfo.Status = 0;
                            DataTable dt_projectInfo = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query_projectInfo, "", 1, 10000, out count);
                            if (dt_projectInfo.Rows.Count > 0)
                            {
                                msg = "{msg:'该分类在调查项目中有使用，无法停用'}";
                                return;
                            }
                        }
                        else if (model.TypeId == 2)
                        {
                            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
                            query.PCatageID = _scid;
                            query.Status = 0;
                            DataTable dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, "", 1, 9999, out count);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                msg = "{msg:'该分类在数据清洗项目中有使用，无法停用'}";
                                return;
                            }
                        }

                        model.Status = 1;
                    }
                    else if (model.Status == 1)
                    {
                        model.Status = 0;
                    }
                    int result = 0;
                    try
                    {
                        result = BLL.SurveyCategory.Instance.Update(model);
                        msg = "{msg:'success'}";
                    }
                    catch (Exception ex)
                    {
                        msg = "{msg:'" + ex.Message + "'}";
                    }
                }
            }
        }

        /// <summary>
        /// 分类名称是否重复
        /// </summary>
        /// <param name="msg"></param>
        private void IsExistsCategoryName(out string msg)
        {
            try
            {
                bool result = BLL.SurveyCategory.Instance.IsExistsCategoryName(RequestCategoryName);
                if (result)
                {
                    msg = "{msg:'fail'}";
                }
                else
                {
                    msg = "{msg:'success'}";
                }

            }
            catch (Exception ex)
            {
                msg = "{msg:'" + ex.Message + "'}";
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}