using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo
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

        /// <summary>
        /// 排除选项 强斐 2016-5-17
        /// </summary>
        public string Exclude
        {
            get
            {
                if (Request["Exclude"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Exclude"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        //add by lixiaomin  所属分组是否显示自己所在的分组
        public bool ShowSelfGroup
        {
            get
            {
                bool flag = false;
                if (Request["ShowSelfGroup"] != null)
                {
                    try
                    {
                        flag = bool.Parse(HttpUtility.UrlDecode(Request["ShowSelfGroup"].ToString()));
                    }
                    catch
                    {

                    }

                }
                return flag;
            }

        }

        #endregion

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
                case "getsmstemplate"://根据分类ID获取短信模板数据
                    GetSMSTemplateBySCID(out msg);
                    break;
                case "getsmstemplatebyrecid"://根据短信模板ID获取短信模板数据
                    GetSMSTemplateByRecID(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        /// <summary>
        /// 根据登陆者ID获取业务组数据,不考虑数据权限，只考虑所在分组
        /// </summary>
        /// <param name="msg"></param>
        private void GetUserGroupNoRightByLoginUserID(out string msg)
        {
            msg = "";
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId);

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
                    Entities.SMSTemplate model = BLL.SMSTemplate.Instance.GetSMSTemplate(recid);
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
                Entities.QuerySMSTemplate query = new QuerySMSTemplate();
                query.SCID = scId;
                query.Status = 0;

                int totalCount = 0;
                //DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "", 1, 1000, out totalCount);
                DataTable dt = BLL.SMSTemplate.Instance.GetSMSTemplate(query, "", 1, 1000, out totalCount);
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
                Entities.QuerySurveyCategory query = new QuerySurveyCategory();
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
                query.Exclude = Exclude;

                int totalCount = 0;
                DataTable dt = BLL.SurveyCategory.Instance.GetSurveyCategory(query, "", 1, 1000, out totalCount);

                //排序
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "Name ASC";
                    dt = dv.ToTable();
                }

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
            DataTable dt = null;
            if (ShowSelfGroup)
            {
                //管辖分组+所属分组 默认第一个：所属分组
                dt = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userID);
            }
            else
            {
                //管辖分组
                dt = BitAuto.ISDC.CC2012.BLL.EmployeeSuper.Instance.GetCurrentUseBusinessGroup(userID);
            }

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
                Entities.QueryProjectInfo query = new QueryProjectInfo();
                query.BGID = bgId;
                int totalCount = 0;
                DataTable dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, "", 1, 1000, out totalCount);
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