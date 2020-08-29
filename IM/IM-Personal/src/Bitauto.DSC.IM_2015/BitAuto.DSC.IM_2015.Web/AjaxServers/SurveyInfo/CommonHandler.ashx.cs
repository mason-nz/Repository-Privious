using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.DSC.IM_2015.WebService.CC;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SurveyInfo
{
    /// <summary>
    /// CommonHandler 的摘要说明
    /// </summary>
    public class CommonHandler : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        #region 属性定义
        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        //业务分组ID
        public string BGID
        {
            get
            {
                if (Request["BGID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["BGID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        //业务分类ID
        public string SCID
        {
            get
            {
                if (Request["SCID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["SCID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //短信模板ID
        public string TemplateID
        {
            get
            {
                if (Request["TemplateID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TemplateID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string IsFilterStop
        {
            get
            {
                if (Request["IsFilterStop"] != null)
                {
                    return HttpUtility.UrlDecode(Request["IsFilterStop"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

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

        //add by qizq 2014-4-17 在其他任务模板中根据分组，取分类需要过滤不能编辑的分类（特定业务的）
        public string SCStatus
        {
            get
            {
                if (Request["SCStatus"] != null)
                {
                    return HttpUtility.UrlDecode(Request["SCStatus"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion


        #region 发送短信相关属性
        public string Tels
        {
            get
            {
                if (Request["Tels"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Tels"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string SendContent
        {
            get
            {
                if (Request["SendContent"] != null)
                {
                    return HttpUtility.UrlDecode(Request["SendContent"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string CustID
        {
            get
            {
                if (Request["CustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //CRM客户ID
        public string CRMCustID
        {
            get
            {
                if (Request["CRMCustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CRMCustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string TaskType
        {
            get
            {
                if (Request["TaskType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string TaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
        private string ServiceURL = System.Configuration.ConfigurationManager.AppSettings["SMSServiceURL"].ToString();//服务URL
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "getsurveycategory"://（问卷管理页面）根据业务组ID查询此业务组下的分类
                    GetSurveyCategoryByBGID(out msg);
                    break;
                case "getusergroupbyloginuserid"://根据登陆者ID获取业务组数据
                    GetUserGroupByLoginUserID(out msg);
                    break;
                case "getusergroupnorightbyloginuserid"://根据登陆者ID获取业务组数据,不考虑数据权限，只考虑所在分组
                    GetUserGroupNoRightByLoginUserID(out msg);
                    break;
                case "getprojectinfobybgid": GetProjectInfoByBGID(out msg);//（问卷结果管理页面）根据业务组ID查询此业务组下的分类
                    break;
                case "getusergroupbydataright"://根据登陆者ID获取业务组数据(有数据权限)
                    GetUserGroupByDataRight(out msg);
                    break;
                case "getsmstemplate"://根据分类ID获取短信模板数据
                    GetSMSTemplateBySCID(out msg);
                    break;
                case "getsmstemplatebyrecid"://根据短信模板ID获取短信模板数据
                    GetSMSTemplateByRecID(out msg);
                    break;
                case "sendsms2":
                    try
                    {
                       SendSMSNew2(out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }
                    break;
            }
            context.Response.Write(msg);
        }

        internal void SendSMSNew2(out string msg)
        {
            msg = string.Empty;

            try
            {
                string telstr = Tels;
                Entities.SMSSendHistory smsLogModel = new Entities.SMSSendHistory();
                smsLogModel.CreateUserID = BLL.Util.GetLoginUserID();
                smsLogModel.CustID = CustID;
                smsLogModel.CRMCustID = CRMCustID;
                Entities.EmployeeAgent agent = new Entities.EmployeeAgent();
                agent = BLL.UserSendMessage.Instance.GetEmployeeAgentByUserID(Convert.ToInt32(smsLogModel.CreateUserID));
                if (agent != null && agent.BGID > 0)
                {
                    smsLogModel.BGID = agent.BGID;
                }
                //smsLogModel.BGID = Convert.ToInt32(BGID);
                if (!string.IsNullOrEmpty(TemplateID) && TemplateID != "null")
                {
                    smsLogModel.TemplateID = Convert.ToInt32(TemplateID);
                }
                if (!string.IsNullOrEmpty(TaskType))
                {
                    smsLogModel.TaskType = Convert.ToInt32(TaskType);
                }

                smsLogModel.TaskID = TaskID;
                smsLogModel.Phone = telstr;
                smsLogModel.Content = SendContent;
                smsLogModel.CreateTime = DateTime.Now;

                //发送短信        

                string md5 = (string)CCWebServiceHepler.Instance.MixMd5(ServiceURL, telstr, SendContent);
                int msgid = (int)CCWebServiceHepler.Instance.SendMsgImmediately(ServiceURL, telstr, SendContent, DateTime.Now.AddHours(1), md5);
                if (msgid > 0)
                {
                    //插入发送短信记录
                    //msg = "success";                
                    smsLogModel.Status = 0;
                    BLL.Loger.Log4Net.Error("给手机（" + telstr + "）发送短信成功");
                    msg = "{result:'true',msg:''}";
                }
                else
                {
                    smsLogModel.Status = -1;
                    msg = BLL.Util.GetEnumOptText(typeof(Entities.SendSMSInfo), msgid);
                    BLL.Loger.Log4Net.Error("给手机（" + telstr + "）发送短信失败【错误信息：" + msg + "】");
                    msg = "{result:'false',msg:'" + msg + "'}";
                }
                BLL.UserSendMessage.Instance.Insert(smsLogModel);
            }
            catch (Exception ex)
            {
                msg = "{result:'false',msg:'" + ex.Message + "'}";
            }

        }



        /// <summary>
        /// 根据登陆者ID获取业务组数据,不考虑数据权限，只考虑所在分组
        /// </summary>
        /// <param name="msg"></param>
        private void GetUserGroupNoRightByLoginUserID(out string msg)
        {
            msg = "";
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.UserSendMessage.Instance.GetUserGroupDataRigthByUserID(userId);

            if (dt.Rows.Count > 0)
            {
                msg = "[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{'BGID':'" + dt.Rows[i]["BGID"].ToString() + "','Name':'" + dt.Rows[i]["Name"].ToString() + "'},";

                if (dt.Rows.Count == (i + 1))
                {
                    msg = msg.TrimEnd(',') + "]"; ;
                }
            }
        }

        /// <summary>
        /// 根据分类ID获取短信模板
        /// </summary>
        /// <param name="msg"></param>
        private void GetSMSTemplateByRecID(out string msg)
        {
            msg = string.Empty;
            try
            {
                int recid = -1;
                if (int.TryParse(TemplateID, out recid))
                {
                    Entities.SMSTemplate model = BLL.UserSendMessage.Instance.GetSMSTemplate(recid);
                    if (model != null)
                    {
                        msg = model.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            
        }

        /// <summary>
        /// 根据分类ID获取短信模板
        /// </summary>
        /// <param name="msg"></param>
        private void GetSMSTemplateBySCID(out string msg)
        {
            msg = string.Empty;
            int scId = -1;
            if (int.TryParse(SCID, out scId))
            {
                Entities.QuerySMSTemplate query = new Entities.QuerySMSTemplate();
                query.SCID = scId;
                query.Status = 0;

                int totalCount = 0;
                DataTable dt = BLL.UserSendMessage.Instance.GetSMSTemplate(query, "", 1, 1000, out totalCount);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (i == 0)
                    {
                        msg += "[{RecID:'" + dr["RecID"] + "',Name:'" + dr["Title"] + "'}";
                    }
                    if (i > 0)
                    {
                        msg += ",{RecID:'" + dr["RecID"] + "',Name:'" + dr["Title"] + "'}";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "]";
                    }
                }
            }
        }

        /// <summary>
        /// （问卷管理页面）根据业务组ID查询此业务组下的分类
        /// </summary>
        /// <param name="msg"></param>
        private void GetSurveyCategoryByBGID(out string msg)
        {
            msg = string.Empty;
            int bgId = -1;
            if (int.TryParse(BGID, out bgId))
            {
                Entities.QuerySurveyCategory query = new Entities.QuerySurveyCategory();
                query.BGID = bgId;
                query.TypeId = int.Parse(TypeId);
                if (IsFilterStop == "1")
                {
                    query.IsFilterStop = true;
                }
                //add by qizq 2014-4-17过滤 分类的状态
                if (!string.IsNullOrEmpty(SCStatus))
                {
                    int _scstatus = 0;
                    if (int.TryParse(SCStatus, out _scstatus))
                    {
                        query.NoStatus = _scstatus;
                    }
                }

                int totalCount = 0;
                DataTable dt = BLL.UserSendMessage.Instance.GetSurveyCategory(query, "", 1, 1000, out totalCount);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (i == 0)
                    {
                        msg += "[{SCID:'" + dr["SCID"] + "',Name:'" + dr["Name"] + "'}";
                    }
                    if (i > 0)
                    {
                        msg += ",{SCID:'" + dr["SCID"] + "',Name:'" + dr["Name"] + "'}";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "]";
                    }
                }
            }
        }

        /// <summary>
        /// 根据登陆者ID查询其分组权限，获取业务组数据
        /// </summary>
        /// <param name="msg"></param>
        private void GetUserGroupByLoginUserID(out string msg)
        {
            msg = string.Empty;
            int userID = BLL.Util.GetLoginUserID();
            //管辖分组+所属分组 默认第一个：所属分组
            DataTable dt = BLL.UserSendMessage.Instance.GetCurrentUserGroups(userID);
            if (dt != null && dt.Rows.Count > 0)
            {
                msg = "[";
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                msg += "{'BGID':'" + dt.Rows[i]["BGID"].ToString() + "','Name':'" + dt.Rows[i]["Name"].ToString() + "'},";
                if (dt.Rows.Count == (i + 1))
                {
                    msg = msg.TrimEnd(',') + "]"; ;
                }
            }
        }


        private void GetUserGroupByDataRight(out string msg)
        {
            msg = string.Empty;
            int userID = BLL.Util.GetLoginUserID();

            string bgids = BLL.UserSendMessage.Instance.GetGroupStr(userID);
            if (!string.IsNullOrEmpty(bgids))
            {
                DataTable dt = BLL.UserSendMessage.Instance.GetBusinessGroupByBGIDS(bgids);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    msg += "{'BGID':'" + dt.Rows[i]["BGID"].ToString() + "','Name':'" + dt.Rows[i]["Name"].ToString() + "'},";

                    if (dt.Rows.Count == (i + 1))
                    {
                        msg = msg.TrimEnd(',') + "]"; ;
                    }
                }
            }
        }



        /// <summary>
        /// （问卷结果管理页面）根据业务组ID查询此业务组下的分类
        /// </summary>
        /// <param name="msg"></param>
        private void GetProjectInfoByBGID(out string msg)
        {
            msg = string.Empty;
            int bgId = -1;
            if (int.TryParse(BGID, out bgId))
            {
                Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
                query.BGID = bgId;
                int totalCount = 0;
                DataTable dt = BLL.UserSendMessage.Instance.GetProjectInfo(query, "", 1, 1000, out totalCount);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (i == 0)
                    {
                        msg += "[{SCID:'" + dr["SCID"] + "',Name:'" + dr["Name"] + "'}";
                    }
                    if (i > 0)
                    {
                        msg += ",{SCID:'" + dr["SCID"] + "',Name:'" + dr["Name"] + "'}";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "]";
                    }
                }
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