using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using XYAuto.ITSC.Chitunion2017.Entities.SysRight;
using System.Web.UI;
using System.Reflection;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager
{
    /// <summary>
    /// ModuleInfoManager 的摘要说明
    /// </summary>
    public class ModuleInfoManager : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            Chitunion2017.Common.UserInfo.Check();

            //绑定一条记录
            #region 绑定一条记录
            if (context.Request.Form["show"] != null && context.Request.Form["show"] == "yes" && context.Request.Form["RecID"] != null)
            {
                ModuleInfo moduleInfo = null;
                if (context.Request.Form["RecID"] != null && context.Request.Form["RecID"].Trim().Length > 0)
                {
                    moduleInfo = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(Convert.ToInt32(context.Request.Form["RecID"]));
                }
                if (moduleInfo != null)
                {
                    string values = "";
                    Type t = typeof(ModuleInfo);
                    PropertyInfo[] properties = t.GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values += properties[i].Name + ":'" + BLL.Util.GetPropertyByName(moduleInfo, properties[i].Name) + "',";
                    }

                    values = values.Substring(0, values.Length - 1);
                    values = "{" + values + "}";

                    context.Response.Write(values);
                    context.Response.End();
                }
            }
            #endregion

            //系统-模块联动功能
            #region 系统-模块联动功能
            if (context.Request.Form["LoadModuleBySysID"] != null && context.Request.Form["LoadModuleBySysID"] == "yes" && context.Request.Form["sysID"] != null)
            {
                QueryModuleInfo moduleInfo = new QueryModuleInfo();
                DataTable dt = new DataTable();

                if (context.Request.Form["sysID"] != null && context.Request.Form["sysID"].Trim().Length > 0)
                {
                    moduleInfo.SysID = context.Request.Form["sysID"].ToString().Trim();
                    //dt = Bll.ModuleInfo.Instance.GetModuleNameRelation(moduleInfo);//根据SYSID，查询所以层级模块列表
                    dt = BLL.SysRight.ModuleInfo.Instance.GetRootModuleNameListBySysID(moduleInfo.SysID);//根据SYSID，查询根级模块列表
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    string values = "";
                    string text = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        values += dt.Rows[i]["ModuleID"] + "|";
                        text += dt.Rows[i]["ModuleName"] + "|";
                    }
                    if (values.Length > 1)
                    {
                        values = values.Substring(0, values.Length - 1);
                    }
                    if (text.Length > 1)
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                    context.Response.Write(text + ";" + values);
                    context.Response.End();
                }
            }

            #endregion

            #region add
            if (context.Request.Form["ShowType"] == "add")
            {
                string newModuleID = context.Request.Form["ModuleID"];
                if (newModuleID != null && newModuleID.Trim().Length > 0)
                {
                    int o;
                    QueryModuleInfo queryModuleInfo = new QueryModuleInfo();
                    queryModuleInfo.ModuleID = newModuleID.Trim();
                    DataTable dt = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(queryModuleInfo, string.Empty, 1, 1, out o);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        context.Response.Write("{add:'ModuleNameExist'}");
                        context.Response.End();
                        return;
                    }
                    if (BLL.SysRight.ModuleInfo.Instance.IsExistByModuleNameAndSysIDAndPid(context.Request.Form["ModuleName"], context.Request.Form["SysID"], context.Request.Form["Pid"]))
                    {
                        context.Response.Write("{add:'ExistModuleName'}");
                        context.Response.End();
                    }
                    else
                    {
                        if (context.Request.Form["Links"] != null && (!string.IsNullOrEmpty(context.Request.Form["Links"].Trim(','))))
                        {
                            string[] linkList = context.Request.Form["Links"].Trim(',').Split(',');
                            string msg = "";
                            for (int i = 0; i < linkList.Length; i++)
                            {
                                if (BLL.SysRight.ModuleInfo.Instance.IsExist(linkList[i].Trim(), context.Request.Form["sysid"]))
                                {
                                    msg += linkList[i].Trim() + ",";
                                }
                            }
                            if (msg != "")
                            {
                                context.Response.Write("{add:'urlExist'}");
                                context.Response.End();
                                return;
                            }
                        }

                        ModuleInfo model = new ModuleInfo();

                        model.ModuleID = newModuleID.Trim();// BLL.ModuleInfo.Instance.GenModuleID(dllAddSysCode.Items[dllAddSysCode.SelectedIndex].Value);
                        model.SysID = context.Request.Form["SysID"];
                        model.ModuleName = context.Request.Form["ModuleName"].Trim();
                        string addModuleID = context.Request.Form["Pid"];// dllAddModule.Items[dllAddModule.SelectedIndex].Value;

                        model.PID = addModuleID;
                        model.Level = BLL.SysRight.ModuleInfo.Instance.GetLevelByModuleID(addModuleID) + 1;
                        model.SysID = context.Request.Form["SysID"];
                        model.ModuleName = context.Request.Form["ModuleName"];
                        model.Intro = context.Request.Form["Intro"];
                        model.Url = context.Request.Form["URL"];
                        model.Links = context.Request.Form["Links"];
                        model.OrderNum = int.Parse(context.Request.Form["OrderNum"].ToString().Trim());
                        model.DomainID = int.Parse(context.Request.Form["DomainCode"].ToString().Trim());
                        model.Status = 0;
                        model.CreateTime = DateTime.Now;
                        //model.Blank = Convert.ToInt32(context.Request.Form["Blank"]);

                        if (model.Url.Length > 200)
                        {
                            context.Response.Write("{add:'UrlOutOfLength'}");
                            context.Response.End();
                        }

                        if (BLL.SysRight.ModuleInfo.Instance.InsertModuleInfo(model) > 0)
                        {
                            Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Add, "添加模块名称为(" + model.ModuleName + "),模块编号:(" + model.ModuleID + ")的记录成功");
                            //  .Instance.InsertLog("添加模块名称为(" + model.ModuleName + "),模块编号:(" + model.ModuleID + ")的记录成功");
                            context.Response.Write("{add:'yes'}");
                            context.Response.End();
                        }
                        else
                        {
                            Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Add, "添加模块名称为(" + model.ModuleName + "),模块编号:(" + model.ModuleID + ")的记录失败");
                            //UserLoginInfo.Instance.InsertLog("添加模块名称为(" + model.ModuleName + "),模块编号:(" + model.ModuleID + ")的记录失败");
                            context.Response.Write("{add:'no'}");
                            context.Response.End();
                        }
                    }
                }
            }

            #endregion

            #region edit
            if (context.Request.Form["ShowType"] == "edit")
            {
                ModuleInfo model = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(context.Request.Form["ModuleID"]);
                if (model != null)
                {
                    string rootModuleName = model.ModuleName;
                    string rootModuleIntro = model.Intro;

                    if (BLL.SysRight.ModuleInfo.Instance.IsExistByModuleNameAndSysID(context.Request.Form["ModuleName"], context.Request.Form["SysID"], model.ModuleID, model.PID))
                    {
                        context.Response.Write("{updata:'ExistModuleName'}");
                        context.Response.End();
                        return;
                    }
                    else
                    {
                        model.SysID = context.Request.Form["SysID"];
                        model.ModuleName = context.Request.Form["ModuleName"];
                        model.Intro = context.Request.Form["Intro"];
                        model.Url = context.Request.Form["URL"];
                        model.Links = context.Request.Form["Links"];
                        //model.Blank = Convert.ToInt32(context.Request.Form["Blank"]);
                        model.OrderNum = int.Parse(context.Request.Form["OrderNum"].ToString().Trim());

                        model.DomainID = int.Parse(context.Request.Form["DomainCode"].ToString().Trim());
                        if (model.Url.Length > 200)
                        {
                            context.Response.Write("{add:'UrlOutOfLength'}");
                            context.Response.End();
                        }

                        if (BLL.SysRight.ModuleInfo.Instance.UpdataModuleInfo(model) > 0)
                        {
                            Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, "修改模块编号为(" + model.ModuleID + "),模块名称为(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录成功");

                            //UserLoginInfo.Instance.InsertLog("修改模块编号为(" + model.ModuleID + "),模块名称为(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录成功");
                            context.Response.Write("{updata:'yes'}");
                            context.Response.End();
                        }
                        else
                        {
                            Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, "修改模块编号为(" + model.ModuleID + "),模块名称由(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录失败");

                            //UserLoginInfo.Instance.InsertLog("修改模块编号为(" + model.ModuleID + "),模块名称由(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录失败");
                            context.Response.Write("{updata:'no'}");
                            context.Response.End();
                        }
                    }
                }
            }

            #endregion

            #region dele
            if (context.Request.Form["dele"] != null && context.Request.Form["dele"] == "yes" && context.Request.Form["ID"] != null)
            {
                string ids = context.Request.Form["ID"].Trim(',');
                if (ids.Length > 0)
                {
                    bool flag = true;
                    string[] id = ids.Split(',');

                    for (int i = 0; i < id.Length; i++)
                    {
                        if (BLL.SysRight.RoleModule.Instance.IsExistByModuleID(id[i].ToString()))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        for (int i = 0; i < id.Length; i++)
                        {
                            Delete(id[i]);
                        }
                    }
                    else
                    {
                        context.Response.Write("{del:'exist'}");
                        context.Response.End();
                        return;
                    }
                    context.Response.Write("{del:'yes'}");
                    context.Response.End();
                    return;
                }
                else
                {
                    context.Response.Write("{del:'no'}");
                    context.Response.End();
                    return;
                }
            }

            #endregion

            #region updata
            //更新记录    
            if (context.Request.Form["updata"] != null && context.Request.Form["updata"] == "yes" && context.Request.Form["RecID"] != null)
            {
                ModuleInfo model = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(Convert.ToInt32(context.Request.Form["RecID"]));
                if (model != null)
                {
                    string rootModuleName = model.ModuleName;
                    string rootModuleIntro = model.Intro;
                    int roootLevel = model.Level;
                    if (BLL.SysRight.ModuleInfo.Instance.IsExistByModuleNameAndSysID(context.Request.Form["ModuleName"], context.Request.Form["dllSysCode"], model.ModuleID, model.PID))
                    {
                        context.Response.Write("{ExistModuleName:'yes'}");
                        context.Response.End();

                    }
                    else
                    {
                        model.SysID = context.Request.Form["dllSysCode"];
                        model.ModuleName = context.Request.Form["ModuleName"];
                        if (context.Request.Form["dllModule"] != null && context.Request.Form["dllModule"].ToString().Trim() == "-1")
                        {
                            model.PID = string.Empty;
                        }
                        else
                        {
                            model.PID = context.Request.Form["dllModule"];
                        }
                        model.Level = BLL.SysRight.ModuleInfo.Instance.GetLevelByModuleID(model.PID) + 1;
                        model.Intro = context.Request.Form["ModuleIntro"];
                        model.Url = context.Request.Form["URL"];
                        model.Links = context.Request.Form["Links"];
                        model.Status = 0;
                        model.OrderNum = int.Parse(context.Request.Form["OrderNum"].ToString().Trim());

                        if (model.ModuleID.Trim() == context.Request.Form["dllModule"].ToString().Trim() ||
                            roootLevel < model.Level - 1)
                        {
                            context.Response.Write("{SelfModuleIDSame:'yes'}");
                            context.Response.End();
                        }
                        else if (BLL.SysRight.ModuleInfo.Instance.UpdataModuleInfo(model) > 0)
                        {
                            Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, "修改模块编号为(" + model.ModuleID + "),模块名称为(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录成功");

                            //BLL.UserLoginInfo.Instance.InsertLog("修改模块编号为(" + model.ModuleID + "),模块名称为(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录成功");
                            context.Response.Write("{updata:'yes',level:" + model.Level + "}");
                            context.Response.End();
                        }
                        else
                        {
                            Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Modify, "修改模块编号为(" + model.ModuleID + "),模块名称由(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录失败");

                            //BLL.UserLoginInfo.Instance.InsertLog("修改模块编号为(" + model.ModuleID + "),模块名称由(" + rootModuleName + ")更改为(" + model.ModuleName + "),系统描述由(" + rootModuleIntro + ")更改为(" + model.Intro + ")的记录失败");
                            context.Response.Write("{updata:'no'}");
                            context.Response.End();
                        }
                    }
                }
            }

            #endregion


        }

        #region funtion
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>是否成功</returns>
        private static int Delete(object key)
        {
            //标记删除数据字典索引
            string id = key.ToString();
            //写日志
            ModuleInfo moduleInfo = BLL.SysRight.ModuleInfo.Instance.GetModuleInfo(id);
            if (moduleInfo != null)
            {
                moduleInfo.Status = -1;

                if (BLL.SysRight.ModuleInfo.Instance.UpdataModuleInfo(moduleInfo) > 0)
                {
                    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Delete, "删除模块编号ID为(" + id + "),模块名称为(" + moduleInfo.ModuleID + ")的记录成功");

                    //UserLoginInfo.Instance.InsertLog("删除模块编号ID为(" + id + "),模块名称为(" + moduleInfo.ModuleID + ")的记录成功");
                }
                else
                {
                    Chitunion2017.Common.LogInfo.Instance.InsertLog((int)Chitunion2017.Common.LogInfo.LogModuleType.角色权限管理, (int)Chitunion2017.Common.LogInfo.ActionType.Delete, "删除模块编号ID为(" + id + "),模块名称为(" + moduleInfo.ModuleID + ")的记录失败");

                    //UserLoginInfo.Instance.InsertLog("删除模块编号ID为(" + id + "),模块名称为(" + moduleInfo.ModuleID + ")的记录失败");
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