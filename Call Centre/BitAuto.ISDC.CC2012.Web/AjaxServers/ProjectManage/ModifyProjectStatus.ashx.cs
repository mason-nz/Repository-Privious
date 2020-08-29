using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// ModifyProjectStatus 的摘要说明
    /// </summary>
    public class ModifyProjectStatus : IHttpHandler, IRequiresSessionState
    {

        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }

        private string Status
        {
            get
            {
                return HttpContext.Current.Request["status"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";

            if (Status == "-1")
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT500603"))
                {
                    msg += "您没有执行该操作的的权限";
                    context.Response.Write(msg);
                    return;
                }
            }
            else if (Status == "2")
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT500607"))
                {
                    msg += "您没有执行该操作的的权限";
                    context.Response.Write(msg);
                    return;
                }
            }


            CheckPro(out msg);

            if (msg == "")
            {
                int userID = BLL.Util.GetLoginUserID();
                int totalCount = 0;

                Entities.ProjectInfo model = new Entities.ProjectInfo();
                model = BLL.ProjectInfo.Instance.GetProjectInfo(int.Parse(ProjectID));
                if (model != null)
                {
                    if (Status == "2")
                    {
                        if (model.Source == 1 || model.Source == 2)
                        {
                            Entities.QueryProjectTaskInfo query = new Entities.QueryProjectTaskInfo();
                            query.ProjectID = int.Parse(ProjectID);
                            query.TaskStatus_s = "180012,180000,180001,180003,180004,180010";

                            DataTable dt = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(query, 1, 999999, out totalCount, userID);

                            foreach (DataRow dr in dt.Rows)
                            {
                                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(dr["PTID"].ToString(), EnumProjectTaskStatus.STOPTask, EnumProjectTaskOperationStatus.TaskFinish, DateTime.Now);
                            }
                        }
                        else
                        {
                            // 结束其他任务
                            DataTable dt = BLL.OtherTaskInfo.Instance.GetStopForOtherTaskInfoByList(int.Parse(ProjectID));
                            foreach (DataRow dr in dt.Rows)
                            {
                                BLL.OtherTaskInfo.Instance.UpdateTaskStatus(dr["PTID"].ToString(), Entities.OtheTaskStatus.StopTask, Entities.EnumProjectTaskOperationStatus.TaskFinish, "结束项目", userID);
                            }
                        }
                    }
                    if (Status == "-1")
                    {
                        //删除问卷
                        Entities.QueryProjectSurveyMapping query = new QueryProjectSurveyMapping();
                        query.ProjectID = int.Parse(ProjectID);

                        List<Entities.ProjectSurveyMapping> mapList = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMappingList(query, out totalCount);

                        if (mapList != null && mapList.Count > 0)
                        {
                            foreach (Entities.ProjectSurveyMapping item in mapList)
                            {
                                item.Status = -1;
                                BLL.ProjectSurveyMapping.Instance.Update(item);
                            }
                        }
                    }

                    #region 修改项目状态
                    model.Status = int.Parse(Status);
                    BLL.ProjectInfo.Instance.Update(model);
                    BLL.ProjectLog.Instance.InsertProjectLog(model.ProjectID, ProjectLogOper.L4_结束项目, "结束项目-" + model.Name);
                    #endregion

                    #region 停止自动外呼
                    int a = BLL.ProjectInfo.Instance.EndAutoCallProject(model.ProjectID);
                    if (a > 0)
                    {
                        BLL.ProjectLog.Instance.InsertProjectLog(model.ProjectID, ProjectLogOper.Z6_结束自动外呼, "");
                    }
                    #endregion
                }
                else
                {
                    msg += "没有找到对应的项目";
                }
            }

            if (msg == "")
            {
                msg = "success";
            }
            context.Response.Write(msg);
        }

        private void CheckPro(out string msg)
        {
            msg = "";
            int intval = 0;

            if (ProjectID == "")
            {
                msg += "缺少项目ID参数";
            }
            if (!int.TryParse(ProjectID, out intval))
            {
                msg += "项目ID参数格式不正确";
            }
            if (Status == "")
            {
                msg += "缺少状态ID参数";
            }
            if (!int.TryParse(Status, out intval))
            {
                msg += "状态ID参数格式不正确";
            }
            if (intval != 0 && intval != 1 && intval != 2 && intval != -1)
            {
                msg += "状态值不正确";
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