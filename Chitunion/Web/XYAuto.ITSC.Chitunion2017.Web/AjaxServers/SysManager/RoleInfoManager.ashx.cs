using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;
using System.Data;
using XYAuto.Utils;
using System.Reflection;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager
{
    /// <summary>
    /// RoleInfoManager 的摘要说明
    /// </summary>
    public class RoleInfoManager : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            Chitunion2017.Common.UserInfo.Check();
            int loginUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();

            if (context.Request.Form["add"] != null && context.Request.Form["add"] == "yes")
            {
                string msg = "";
                int o;
                QueryRoleInfo queryRoleInfo = new QueryRoleInfo();
                queryRoleInfo.RoleName = context.Request.Form["RoleName"].ToString();
                queryRoleInfo.SysID = context.Request.Form["SysID"].ToString();
                DataTable dt = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(queryRoleInfo, 1, 10, out o);
                if (dt != null && dt.Rows.Count > 0)
                {
                    msg = "在 " + context.Request.Form["SysID"].ToString() + " 系统中角色名称已经存在！";
                }
                if (msg != "")
                {
                    ScriptHelper.ShowAlertScript(msg);
                    return;
                }
                else
                {
                    //插入数据
                    RoleInfo roleInfo = new RoleInfo();
                    roleInfo.SysID = context.Request.Form["SysID"].ToString();
                    roleInfo.CreateTime = DateTime.Now;
                    roleInfo.RoleName = context.Request.Form["RoleName"].ToString();
                    roleInfo.Intro = context.Request.Form["Intro"].ToString();
                    //roleInfo.RoleType = Convert.ToInt32(Request.Form["RoleType"]);
                    roleInfo.CreateUserID = loginUserID;
                    roleInfo.Status = 0;
                    int id = BLL.SysRight.RoleInfo.Instance.InsertRoleInfo(roleInfo);
                    if (id > 0)
                    {
                        //#region 记录日志
                        //string LogModuleID = XYAuto.YanFa.SysRightsManager.Entities.SRMActionType.RoleManage;
                        //int ActionType = (int)XYAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Add;
                        //int o_1;
                        //QueryRoleInfo queryRoleInfo_1 = new QueryRoleInfo();
                        //queryRoleInfo_1.RecID = id;
                        //DataTable dt_1 = Bll.RoleInfo.Instance.GetRoleInfo(queryRoleInfo_1, 1, 10, out o_1);
                        //if (dt_1 != null && dt_1.Rows.Count > 0)
                        //{
                        //    string Content = "添加【" + getSysNameBySysID(roleInfo.SysID) + "】角色【" + roleInfo.RoleName + "(" + dt_1.Rows[0]["RoleID"].ToString() + ")" + "】信息成功";
                        //    XYAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(LogModuleID, ActionType, Content);
                        //}
                        //#endregion
                        //UserLoginInfo.Instance.InsertLog("添加角色名称为(" + roleInfo.RoleName + ")的纪录成功");
                        Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Add, "添加角色名称为(" + roleInfo.RoleName + ")的纪录成功");
                        context.Response.Write("{add:'yes'}");
                        context.Response.End();
                    }
                    else
                    {
                        Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Add, "添加角色名称为(" + roleInfo.RoleName + ")的纪录失败");
                        context.Response.Write("{add:'no'}");
                        context.Response.End();
                    }
                }
            }
            else if (context.Request.Form["show"] != null && context.Request.Form["show"] == "yes" && context.Request.Form["RoleID"] != null)
            {
                RoleInfo roleInfo = null;
                if (context.Request.Form["RoleID"] != null && context.Request.Form["RoleID"].Trim().Length > 0)
                {
                    roleInfo = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(context.Request.Form["RoleID"]);
                }
                if (roleInfo != null)
                {
                    string values = "";
                    Type t = typeof(RoleInfo);
                    PropertyInfo[] properties = t.GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values += properties[i].Name + ":'" + BLL.Util.GetPropertyByName(roleInfo, properties[i].Name) + "',";
                    }

                    values = values.Substring(0, values.Length - 1);
                    values = "{" + values + "}";

                    context.Response.Write(values);
                    context.Response.End();
                }
            }
            else if (context.Request.Form["updata"] != null && context.Request.Form["updata"] == "yes" && context.Request.Form["RoleID"] != null && context.Request.Form["RoleName"] != null)
            {
                RoleInfo roleInfo = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(context.Request.Form["RoleID"]);

                if (roleInfo != null)
                {
                    #region 记录日志
                    //string LogModuleID = XYAuto.YanFa.SysRightsManager.Entities.SRMActionType.RoleManage;
                    //int ActionType = (int)XYAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update;
                    string upmsg = "";
                    if (roleInfo.RoleName != context.Request.Form["RoleName"])
                    {
                        upmsg += "角色名称由【" + roleInfo.RoleName + "】改为【" + context.Request.Form["RoleName"] + "】，";
                    }
                    //if (roleInfo.RoleType != Convert.ToInt32(Request.Form["RoleType"]))
                    //{
                    //    string oldType = roleInfo.RoleType.ToString();
                    //    string newType = Request.Form["RoleType"];
                    //    if (oldType == "0")
                    //    {
                    //        oldType = "业务角色";
                    //    }
                    //    else if (oldType == "1")
                    //    {
                    //        oldType = "系统角色";
                    //    }
                    //    if (newType == "0")
                    //    {
                    //        newType = "业务角色";
                    //    }
                    //    else if (newType == "1")
                    //    {
                    //        newType = "系统角色";
                    //    }
                    //    upmsg += "角色类型由【" + oldType + "】改为【" + newType + "】，";
                    //}
                    if (roleInfo.Intro != context.Request.Form["Intro"])
                    {
                        upmsg += "角色描述由【" + roleInfo.Intro + "】改为【" + context.Request.Form["Intro"] + "】，";
                    }
                    #endregion

                    string rootName = roleInfo.RoleName;
                    string rootSys = roleInfo.SysID;
                    if (roleInfo.RoleName == context.Request.Form["RoleName"] && roleInfo.SysID == context.Request.Form["SysID"])
                    {
                        roleInfo.Intro = context.Request.Form["Intro"];
                        //roleInfo.RoleType = Convert.ToInt32(Request.Form["RoleType"]);
                    }
                    else
                    {
                        int o;
                        QueryRoleInfo queryRoleInfo = new QueryRoleInfo();
                        queryRoleInfo.RoleName = context.Request.Form["RoleName"];
                        queryRoleInfo.SysID = context.Request.Form["SysID"];
                        DataTable dt = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(queryRoleInfo, 1, 10, out o);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            context.Response.Write("{updata:'exist'}");
                            context.Response.End();
                        }
                        else
                        {
                            roleInfo.SysID = context.Request.Form["SysID"];
                            roleInfo.RoleName = context.Request.Form["RoleName"];
                            //roleInfo.RoleType = Convert.ToInt32(Request.Form["RoleType"]);
                        }
                    }



                    if (BLL.SysRight.RoleInfo.Instance.UpdataRoleInfo(roleInfo) > 0)
                    {
                        //#region 记录日志
                        //if (upmsg != "")
                        //{
                        //    string Content = "修改[" + getSysNameBySysID(roleInfo.SysID) + "]角色[" + roleInfo.RoleName + "(" + roleInfo.RoleID + ")" + "]信息:" + upmsg;
                        //    XYAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(LogModuleID, ActionType, Content);
                        //}
                        //#endregion
                        Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, "修改权限ID为(" + roleInfo.RoleID + "),权限名称由(" + rootName + ")更改为(" + roleInfo.RoleName + "),系统ID由(" + rootSys + ")更改为(" + roleInfo.SysID + ")的纪录成功");
                        //UserLoginInfo.Instance.InsertLog("修改权限ID为(" + roleInfo.RoleID + "),权限名称由(" + rootName + ")更改为(" + roleInfo.RoleName + "),系统ID由(" + rootSys + ")更改为(" + roleInfo.SysID + ")的纪录成功");
                        context.Response.Write("{updata:'yes'}");
                        context.Response.End();
                    }
                    else
                    {
                        Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, "修改权限ID为(" + roleInfo.RoleID + "),权限名称由(" + rootName + ")更改为(" + roleInfo.RoleName + "),系统ID由(" + rootSys + ")更改为(" + roleInfo.SysID + ")的纪录失败");
                        //UserLoginInfo.Instance.InsertLog("修改权限ID为(" + roleInfo.RoleID + "),权限名称由(" + rootName + ")更改为(" + roleInfo.RoleName + "),系统ID由(" + rootSys + ")更改为(" + roleInfo.SysID + ")的纪录失败");
                        context.Response.Write("{updata:'no'}");
                        context.Response.End();
                    }
                }
            }
            else if (context.Request.Form["view"] != null && context.Request.Form["view"] == "yes" && context.Request.Form["RoleID"] != null)
            {
                DataTable dt = BLL.SysRight.RoleInfo.Instance.GetUserInfoByRoleId(context.Request.Form["RoleID"].ToString());
                if (dt.Rows.Count > 0)
                {
                    string values = "owner" + ":'";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        values += dt.Rows[i]["TrueName"].ToString() + "；";
                    }
                    values = values.Substring(0, values.Length - 1);
                    values += "'";
                    values = "{" + values + "}";

                    context.Response.Write(values);
                    context.Response.End();
                }
            }
            else if (context.Request.Form["dele"] != null && context.Request.Form["dele"] == "yes" && context.Request.Form["ID"] != null)
            {
                string ids = context.Request.Form["ID"].Trim(',');
                if (ids.Length > 0)
                {
                    string[] id = ids.Split(',');
                    for (int i = 0; i < id.Length; i++)
                    {
                        if (BLL.SysRight.RoleInfo.Instance.UserRoleIsUse(ids) > 0)
                        {
                            context.Response.Write("{dele:'exist'}");
                            context.Response.End();
                        }
                        else
                        {
                            //#region 记录日志
                            ////string LogModuleID = XYAuto.YanFa.SysRightsManager.Entities.SRMActionType.RoleManage;
                            ////int ActionType = (int)XYAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Delete;
                            //RoleInfo roleInfo = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(id[i]);
                            //if (roleInfo != null)
                            //{
                            //    string Content = "删除【" + roleInfo.SysID + "】【" + roleInfo.RoleName + "(" + roleInfo.RoleID + ")" + "】角色";
                            //    //XYAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(LogModuleID, ActionType, Content);
                            //    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Delete, Content);

                            //}
                            //#endregion
                            Delete(id[i]);
                        }
                    }
                    context.Response.Write("{dele:'yes'}");
                    context.Response.End();
                }
                else
                {
                    context.Response.Write("{dele:'no'}");
                    context.Response.End();
                }
            }
            //设置权限
            else if (context.Request.Form["SetRole"] != null && context.Request.Form["SetRole"] == "yes" && context.Request.Form["moduleID"] != null)
            {
                #region 记录日志
                //string LogModuleID = XYAuto.YanFa.SysRightsManager.Entities.SRMActionType.RoleManage;
                //int ActionType = (int)XYAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update;
                string content = string.Empty;
                RoleInfo roleInfo = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(context.Request.Form["roleID"]);
                if (roleInfo != null)
                {
                    string old_qx = "";
                    int o;
                    QueryRoleModule QueryRoleModule = new QueryRoleModule();
                    QueryRoleModule.RoleID = roleInfo.RoleID;
                    DataTable dt = BLL.SysRight.RoleModule.Instance.GetRoleModule(QueryRoleModule, 1, 10000, out o);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            old_qx += dt.Rows[i]["ModuleID"].ToString() + ",";
                        }
                    }
                    string new_qx = "";
                    string moduleID = context.Request.Form["moduleID"].ToString();
                    string[] moduleIDS = moduleID.Split(':');
                    foreach (string str in moduleIDS)
                    {
                        new_qx += str + ",";
                    }
                    content = "修改[" + roleInfo.SysID + "]角色[" + roleInfo.RoleName + "(" + roleInfo.RoleID + ")" + "]的权限信息:管理模块由【" + old_qx + "】改为【" + new_qx + "】";
                    //XYAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(LogModuleID, ActionType, Content);
                }

                #endregion
                if (BLL.SysRight.RoleModule.Instance.InsertRoleModuleAll(context.Request["roleID"].ToString(), context.Request["moduleID"].ToString(), context.Request["sysID"].ToString()) > 0)
                {
                    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, content + "，操作成功");
                    //Bll.SysOperationLog.Instance.Insert("设置角色权限SysID:(" + Request.Form["sysID"].ToString() + "),RoleID:(" + Request.Form["roleID"].ToString() + ")的ModuleID为(" + Request.Form["moduleID"].ToString() + ")的纪录成功.");
                    //UserLoginInfo.Instance.InsertLog("设置角色权限SysID:(" + context.Request.Form["sysID"].ToString() + "),RoleID:(" + context.Request.Form["roleID"].ToString() + ")的ModuleID为(" + context.Request.Form["moduleID"].ToString() + ")的纪录成功.");
                    context.Response.Write("{setRole:'yes'}");
                    context.Response.End();
                }
                else
                {
                    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, content + "，操作失败");
                    //Bll.SysOperationLog.Instance.Insert("设置角色权限SysID:(" + Request.Form["sysID"].ToString() + "),RoleID:(" + Request.Form["roleID"].ToString() + ")的ModuleID为(" + Request.Form["moduleID"].ToString() + ")的纪录失败.");
                    //UserLoginInfo.Instance.InsertLog("设置角色权限SysID:(" + context.Request.Form["sysID"].ToString() + "),RoleID:(" + context.Request.Form["roleID"].ToString() + ")的ModuleID为(" + context.Request.Form["moduleID"].ToString() + ")的纪录失败.");
                    context.Response.Write("{setRole:'no'}");
                    context.Response.End();
                }
            }


        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>是否成功</returns>
        private static int Delete(object key)
        {
            //标记删除数据字典索引

            //写日志
            RoleInfo roleInfo = BLL.SysRight.RoleInfo.Instance.GetRoleInfo(key.ToString());
            if (roleInfo != null)
            {
                roleInfo.Status = -1;
                //return Bll.RoleInfo.Instance.UpdataRoleInfo(roleInfo);
                if (BLL.SysRight.RoleInfo.Instance.UpdataRoleInfo(roleInfo) > 0)
                {
                    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Delete, "删除角色编号ID为(" + key + "),角色名称为(" + roleInfo.RoleName + ")的纪录成功");
                    //Bll.SysOperationLog.Instance.Insert("删除角色编号ID为(" + key + "),角色名称为(" + roleInfo.RoleName + ")的纪录成功");
                    //UserLoginInfo.Instance.InsertLog("删除角色编号ID为(" + key + "),角色名称为(" + roleInfo.RoleName + ")的纪录成功");
                }
                else
                {
                    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Delete, "删除角色编号ID为(" + key + "),角色名称为(" + roleInfo.RoleName + ")的纪录失败");
                    //Bll.SysOperationLog.Instance.Insert("删除角色编号ID为(" + key + "),角色名称为(" + roleInfo.RoleName + ")的纪录失败");
                    //UserLoginInfo.Instance.InsertLog("删除角色编号ID为(" + key + "),角色名称为(" + roleInfo.RoleName + ")的纪录失败");
                }
                return 1;
            }
            return -1;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}