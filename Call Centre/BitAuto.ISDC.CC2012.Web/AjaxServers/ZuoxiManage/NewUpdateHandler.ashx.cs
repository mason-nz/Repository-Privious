using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils.Config;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage
{
    /// <summary>
    /// NewUpdateHandler 的摘要说明
    /// </summary>
    public class NewUpdateHandler : IHttpHandler, IRequiresSessionState
    {
        #region 参数

        public string UserID
        {
            get
            {
                return HttpContext.Current.Request["UserID"] == null ?
                    string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserID"].ToString());
            }
        }
        public string AgentNum
        {
            get
            {
                return HttpContext.Current.Request["AgentNum"] == null ?
                    string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString());
            }
        }
        public string UserRolesID
        {
            get
            {
                return HttpContext.Current.Request["UserRolesID"] == null ?
                    string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["UserRolesID"].ToString());
            }
        }
        public string AtGroupID
        {
            get
            {
                return HttpContext.Current.Request["AtGroupID"] == null ?
                    string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["AtGroupID"].ToString());
            }
        }
        public string ManagerGroupIDs
        {
            get
            {
                return HttpContext.Current.Request["ManagerGroupIDs"] == null ?
                    string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ManagerGroupIDs"].ToString());
            }
        }
        public string AreaID
        {
            get
            {
                return HttpContext.Current.Request["AreaID"] == null ?
                    string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["AreaID"].ToString());
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/html";
            string msg = "";
            string jsonStr = "";

            try
            {
                SaveInfo(out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (msg == "")
            {
                jsonStr = "{\"Success\":\"True\",\"Message\":\"\"}";
            }
            else
            {
                jsonStr = "{\"Success\":\"False\",\"Message\":\"" + msg + "\"}";
            }

            context.Response.Write(jsonStr);
        }

        private void SaveInfo(out string errMsg)
        {
            errMsg = "";

            #region 保存对权限的设置

            BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.InsertUserRole(int.Parse(UserID), ConfigurationUtil.GetAppSettingValue("ThisSysID"), UserRolesID);

            #endregion

            #region 修改所在分组、工号

            SaveEmpleeInfo();

            #endregion

            #region 修改管辖分组

            SaveManagerGroup();

            #endregion
        }

        private void SaveManagerGroup()
        {
            BLL.UserGroupDataRigth.Instance.DeleteByUserID(int.Parse(UserID));

            if (ManagerGroupIDs != "")
            {
                int userId = BLL.Util.GetLoginUserID();
                string[] groupRightArry = ManagerGroupIDs.Split(',');

                foreach (string groupRight in groupRightArry)
                {
                    if (groupRight != "")
                    {
                        Entities.UserGroupDataRigth groupDataRigthModel = new Entities.UserGroupDataRigth();
                        groupDataRigthModel.CreateTime = DateTime.Now;
                        groupDataRigthModel.CreateUserID = userId;
                        groupDataRigthModel.UserID = int.Parse(UserID);
                        groupDataRigthModel.BGID = int.Parse(groupRight);
                        BLL.UserGroupDataRigth.Instance.Insert(groupDataRigthModel);
                    }
                }
            }

        }

        private void SaveEmpleeInfo()
        {
            #region 判断工号是否已被使用，如果已经被使用，把别人的设置为空

            Entities.EmployeeAgent model = new Entities.EmployeeAgent();
            Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
            query.AgentNum = AgentNum;

            int total = 0;
            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, "", 1, 10, out total);

            if (dt.Rows.Count > 0 && dt.Rows[0]["UserID"].ToString() != UserID)
            {
                //与别人的工号有重复,把别人的置为空
                model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(int.Parse(dt.Rows[0]["UserID"].ToString()));
                if (model != null)
                {
                    model.AgentNum = "";
                    BLL.EmployeeAgent.Instance.Update(model);
                }
            }
            #endregion

            model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(int.Parse(UserID));
            if (model != null)
            {
                //存在，更新
                model.AgentNum = AgentNum;
                model.BGID = int.Parse(AtGroupID);
                model.RegionID = int.Parse(AreaID);

                BLL.EmployeeAgent.Instance.Update(model);
            }
            else
            {
                //不存在，插入
                model = new Entities.EmployeeAgent();
                model.AgentNum = AgentNum;
                model.BGID = int.Parse(AtGroupID);
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.UserID = int.Parse(UserID);
                model.RegionID = int.Parse(AreaID);

                BLL.EmployeeAgent.Instance.Insert(model);
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