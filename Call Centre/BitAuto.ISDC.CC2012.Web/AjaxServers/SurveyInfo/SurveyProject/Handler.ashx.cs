using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyProject
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler,IRequiresSessionState
    {
        #region 属性定义
        public string Action
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Action"));
            }
        }
        public string SPIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SPIID"));
            }
        }
        public string ProjectName
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ProjectName"));
            }
        }
        public string BGID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BGIF"));
            }
        }
        public string SCID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SCID"));
            }
        }
        public string Description
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Description"));
            }
        }
        public string BusinessGroup
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BusinessGroup"));
            }
        }
        public string SIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SIID"));
            }
        }
        public string PersonIDS
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("PersonIDS"));
            }
        }
        public string BeginTime
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Begintime"));
            }
        }
        public string EndTime
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Endtime"));
            }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch(Action.ToLower())
            {
                case "submitsurveyproject":
                    try
                    {
                        SubmitSurveyProject(out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = "{Result:'error',RecID:'',ErrorMsg:'"+HttpUtility.UrlEncode(ex.Message)+"'}";
                    }
                    break;
                case "deletesurveyproject":
                    DeleteSurveyProject(out msg);
                    break;
                case "completesurveyproject":
                    CompleteSurveyProject(out msg);
                    break;
            }
            context.Response.Write(msg);
        }
        //保存调查项目
        private void SubmitSurveyProject(out string msg)
        {
            msg = string.Empty;
            string errorMsg = string.Empty;
            #region 信息验证
            int spiId = 0;
            if (!string.IsNullOrEmpty(SPIID))
            {
                if (!int.TryParse(SPIID, out spiId))
                {
                    errorMsg += "不存在此调查项目!</br>";
                }
            }
            if(string.IsNullOrEmpty(ProjectName.Trim()))
            {
                errorMsg += "调查名称不能为空!</br>";
            }
            int bgId = 0;
            if (!int.TryParse(BGID,out bgId))
            {
                errorMsg += "请选择所属分组!</br>";
            }
            int scId = 0;
            if (!int.TryParse(SCID, out scId))
            {
                errorMsg += "请选择分类!</br>";
            }
            if (string.IsNullOrEmpty(Description.Trim()))
            {
                errorMsg += "调查说明不能为空!</br>";
            }
            //if (string.IsNullOrEmpty(BusinessGroup.Trim()))
            //{
            //    errorMsg += "业务组不能为空!</br>";
            //}
            int siId = 0;
            if (!int.TryParse(SIID, out siId))
            {
                errorMsg += "请选择问卷!</br>";
            }
            List<int> personIdArry = new List<int>();
            if (string.IsNullOrEmpty(PersonIDS.Trim()))
            {
                errorMsg += "请选择调查对象!</br>";
            }
            else
            {
                string[] strArry = PersonIDS.Split(',');
                foreach (string str in strArry)
                {
                    int personId = -1;
                    if (int.TryParse(str, out personId))
                    {
                        personIdArry.Add(personId);
                    }
                    else
                    {
                        errorMsg += "调查对象选择有问题！</br>";
                        break;
                    }
                }
            }
            DateTime beginTime = Entities.Constants.Constant.DATE_INVALID_VALUE;
            if (string.IsNullOrEmpty(BeginTime))
            {
                errorMsg += "调查开始时间不能为空!</br>";
            }
            else
            {
                if (!DateTime.TryParse(BeginTime, out beginTime))
                {
                    errorMsg += "调查开始时间格式不正确!</br>";
                }
                else
                {
                    if (beginTime < DateTime.Now)
                    {
                        errorMsg += "调查开始时间不能小于当前时间!</br>";
                    }
                }
            }
            DateTime endTime=Entities.Constants.Constant.DATE_INVALID_VALUE;
            if (string.IsNullOrEmpty(EndTime))
            {
                errorMsg += "调查开始时间不能为空!</br>";
            }
            else
            {
                if (!DateTime.TryParse(EndTime, out endTime))
                {
                    errorMsg += "调查开始时间格式不正确!</br>";
                }
                else
                {
                    if (endTime <= beginTime)
                    {
                        errorMsg += "调查结束时间不能小于开始时间!</br>";
                    }
                }
            }
            #endregion
            if (string.IsNullOrEmpty(errorMsg))
            {
                //如果项目ID小于等于0，新增项目
                if (spiId <= 0)
                {
                    Entities.SurveyProjectInfo model = new Entities.SurveyProjectInfo();
                    model.BGID = bgId;
                    model.BusinessGroup = BusinessGroup;
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    model.Description = Description;
                    model.Name = ProjectName;
                    model.SCID = scId;
                    model.SIID = siId;
                    model.Status = 0;
                    model.SurveyEndTime = endTime;
                    model.SurveyStartTime = beginTime;
                    //新增问卷项目
                    int recId = BLL.SurveyProjectInfo.Instance.Insert(model);

                    //新增调查参与人
                    foreach (int personId in personIdArry)
                    {
                        Entities.SurveyPerson personModel = new Entities.SurveyPerson();
                        personModel.CreateTime = DateTime.Now;
                        personModel.CreateUserID = BLL.Util.GetLoginUserID();
                        personModel.ExamPersonID = personId;
                        personModel.SPIID = recId;

                        BLL.SurveyPerson.Instance.Insert(personModel);
                    }
                    msg = "{Result:'success',RecID:'" + recId + "',ErrorMsg:''}";
                }
                else
                {
                   Entities.SurveyProjectInfo info = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spiId);
                   if (info != null)
                   {
                       if (info.SurveyStartTime <= DateTime.Now)
                       {
                           errorMsg += "此问卷正在调查中，无法进行编辑操作！</br>";
                       }
                       else
                       {
                           info.BGID = bgId;
                           info.BusinessGroup = BusinessGroup;
                           info.Description = Description;
                           info.Name = ProjectName;
                           info.SCID = scId;
                           info.SIID = siId;
                           info.SurveyEndTime = endTime;
                           info.SurveyStartTime = beginTime;
                           info.ModifyTime = DateTime.Now;
                           info.ModifyUserID = BLL.Util.GetLoginUserID();
                           BLL.SurveyProjectInfo.Instance.Update(info);

                           BLL.SurveyPerson.Instance.DeleteBySPIID(spiId);
                           foreach (int personId in personIdArry)
                           {
                               Entities.SurveyPerson personModel = new Entities.SurveyPerson();
                               personModel.CreateTime = DateTime.Now;
                               personModel.CreateUserID = BLL.Util.GetLoginUserID();
                               personModel.ExamPersonID = personId;
                               personModel.SPIID = spiId;

                               BLL.SurveyPerson.Instance.Insert(personModel);
                           }
                           
                       }
                   }
                   else
                   {
                       errorMsg += "不存在此问卷项目！</br>";
                   }
                   if (string.IsNullOrEmpty(errorMsg))
                   {
                       msg = "{Result:'success',RecID:'" + spiId + "',ErrorMsg:'" + errorMsg + "'}";
                   }
                   else
                   {
                       msg = "{Result:'error',RecID:'',ErrorMsg:'" + errorMsg + "'}";
                   }
                }
            }
            else
            {
                msg = "{Result:'error',RecID:'',ErrorMsg:'" + errorMsg + "'}";
            }
        }
        //删除调查项目
        private void DeleteSurveyProject(out string msg)
        {
            msg = string.Empty;
            int spiId = -1;
            if (int.TryParse(SPIID, out spiId))
            {
                Entities.SurveyProjectInfo info = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spiId);
                if (info != null)
                {
                    if (info.SurveyStartTime > DateTime.Now)
                    {
                        BLL.SurveyProjectInfo.Instance.Delete(spiId);
                        msg = "success";
                    }
                    else
                    {
                        msg = "此调查项目已经使用，无法进行删除操作";
                    }
                }
                else
                {
                    msg = "此调查项目不存在";
                }
            }
            else
            {
                msg = "此调查项目不存在";
            }
        }

        private void CompleteSurveyProject(out string msg)
        {
            msg = string.Empty;
            int spiId = -1;
            if (int.TryParse(SPIID, out spiId))
            {
                Entities.SurveyProjectInfo info = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spiId);
                if (info != null)
                {
                    if (info.Status == 0)
                    {
                        if (info.SurveyStartTime > DateTime.Now)
                        {
                            msg = "此问卷项目还没有开始，无法进行完成操作！";
                        }
                        else
                        {
                            info.SurveyEndTime=DateTime.Now;
                            BLL.SurveyProjectInfo.Instance.Update(info);
                            msg = "success";
                        }
                    }
                }
                else
                {
                    msg = "此调查项目不存在";
                }
            }
            else
            {
                msg = "此调查项目不存在";
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