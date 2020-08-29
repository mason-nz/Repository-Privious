using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils.Config;
using System.Data;
using System.Web.SessionState;
using System.Collections;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    /// <summary>
    /// UpdateUsersRight1 的摘要说明
    /// </summary>
    public class UpdateUsersRight1 : IHttpHandler, IRequiresSessionState
    {
        #region 参数

        /// <summary>
        /// 角色列表
        /// </summary>
        public string RoleIDs
        {
            get
            {
                return HttpContext.Current.Request["roleIDs"] == null ? string.Empty : HttpContext.Current.Request["roleIDs"].ToString();
            }
        }
        /// <summary>
        /// 所属分组列表
        /// </summary>
        public string BGIDS
        {
            get
            {
                return HttpContext.Current.Request["bgids"] == null ? string.Empty : HttpContext.Current.Request["bgids"].ToString();
            }
        }

        /// <summary>
        /// 用户ID 列表
        /// </summary>
        public string UserIDs
        {
            get
            {
                return HttpContext.Current.Request["userIDs"] == null ? string.Empty : HttpContext.Current.Request["userIDs"].ToString();
            }
        }

        /// <summary>
        /// 工号
        /// </summary>
        public string AgentNum
        {
            get
            {
                return HttpContext.Current.Request["agentnum"] == null ? string.Empty : HttpContext.Current.Request["agentnum"].ToString();
            }
        }

        /// <summary>
        /// 是否修改单个用户(false:批量修改，true：单个设置)（单个用户时，要修改工号）
        /// </summary>
        public string IsModfiySingle
        {
            get
            {
                return HttpContext.Current.Request["single"] == null ? string.Empty : HttpContext.Current.Request["single"].ToString();
            }
        }

        /// <summary>
        /// 数据权限类型(1.本人  2.全部)（单个用户时，可能要修改数据权限）
        /// </summary>
        public string RightType
        {
            get
            {
                return HttpContext.Current.Request["righttype"] == null ? string.Empty : HttpContext.Current.Request["righttype"].ToString();
            }
        }
        //判断是否取消工号占用验证
        public string IsContinute
        {
            get
            {
                return HttpContext.Current.Request["IsContinute"] == null ? string.Empty : HttpContext.Current.Request["IsContinute"].ToString();
            }
        }
        public string GroupRightStr
        {
            get
            {
                return HttpContext.Current.Request["GroupRightStr"] == null ? string.Empty : HttpContext.Current.Request["GroupRightStr"].ToString();
            }
        }
        /// <summary>
        /// 分组
        /// </summary>
        public int ReginId
        {
            get
            {
                return HttpContext.Current.Request["regionId"] == null ? -2 :
                    int.Parse(
                    string.IsNullOrEmpty(HttpContext.Current.Request["regionId"].ToString().Trim())
                    ? "-2" : HttpContext.Current.Request["regionId"].ToString().Trim()
                    );
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            //修改用户权限

            string errMsg = "";

            #region 检测参数

            CheckedPar(out errMsg);
            if (errMsg != "")
            {
                context.Response.Write(errMsg);
                return;
            }

            #endregion

            #region 保存对权限的设置
            errMsg = SaveRolseSetting(errMsg);
            #endregion

            if (IsModfiySingle == "false")
            {
                SaveBusinessGroupSetting(errMsg);
            }

            #region 如果是单个修改

            if (IsModfiySingle == "true")
            {
                ModifyOther();
            }
            #endregion

            if (errMsg == "")
            {
                errMsg = "succeed";
            }
            context.Response.Write(errMsg);

        }

        /// <summary>
        /// 当时单个修改时，保存工号和数据权限
        /// </summary>
        private void ModifyOther()
        {
            int retNum = 0;

            #region 修改工号

            Entities.EmployeeAgent model = new Entities.EmployeeAgent();
            Entities.EmployeeAgent oldModel = null;

            Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
            query.UserID = int.Parse(UserIDs);
            int totalCount = 0;
            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, "", 1, 10, out totalCount);

            oldModel = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(int.Parse(UserIDs));


            //*add by qizq 2013-7-16 如果工号已存在，删除工号员工对应关系（程序走到这步说明用户已经选择要替换已有工号对应员工）
            if (!string.IsNullOrEmpty(AgentNum))
            {
                Entities.QueryEmployeeAgent queryHave = new Entities.QueryEmployeeAgent();
                queryHave.AgentNum = AgentNum;
                int total = 0;
                DataTable dtHave = BLL.EmployeeAgent.Instance.GetEmployeeAgent(queryHave, "", 1, 10, out total);
                if (total != 0)
                {
                    if (dtHave.Rows[0]["UserID"].ToString() != UserIDs)
                    {
                        BLL.EmployeeAgent.Instance.Delete(int.Parse(dtHave.Rows[0]["RecID"].ToString()));
                        string logStr = "删除用户工号：把" + BLL.Util.GetNameInHRLimitEID(Convert.ToInt32(dtHave.Rows[0]["UserID"].ToString())) + "的工号" + AgentNum + "删除";
                        BLL.Util.InsertUserLog(logStr);
                    }
                }
            }
            //*




            if (dt == null || dt.Rows.Count == 0)
            {
                model.UserID = int.Parse(UserIDs);
                model.AgentNum = AgentNum;
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();

                //如果没有，则插入
                if (model.AgentNum != "")
                {
                    retNum = BLL.EmployeeAgent.Instance.Insert(model);

                    model.RecID = retNum;
                    employeeAgentInsertLog(null, model);
                }
            }
            else
            {
                //如果有，则修改
                if (dt.Rows[0]["AgentNum"].ToString() != AgentNum)
                {
                    //有变动，要修改
                    model = BLL.EmployeeAgent.Instance.GetEmployeeAgent(int.Parse(dt.Rows[0]["RecID"].ToString()));
                    model.AgentNum = AgentNum;
                    if (model.AgentNum != "")
                    {
                        retNum = BLL.EmployeeAgent.Instance.Update(model);
                        employeeAgentInsertLog(oldModel, model);
                    }
                    //*add by qizq2013-7-16如果工号为空，删除人员坐席对应关系。
                    else
                    {
                        retNum = BLL.EmployeeAgent.Instance.Delete(model.RecID);
                        string logStr = "删除用户工号：把" + BLL.Util.GetNameInHRLimitEID(Convert.ToInt32(oldModel.UserID)) + "的工号" + oldModel.AgentNum + "删除";
                        BLL.Util.InsertUserLog(logStr);
                    }
                    //*
                }
            }


            #endregion

            #region 修改数据权限

            Entities.UserDataRigth modeldata = new Entities.UserDataRigth();

            Entities.UserDataRigth oldModel_UserData = null;

            Entities.QueryUserDataRigth dataQuery = new Entities.QueryUserDataRigth();
            dataQuery.UserID = int.Parse(UserIDs);

            DataTable datadt = BLL.UserDataRigth.Instance.GetUserDataRigth(dataQuery, "", 1, 10, out totalCount);

            oldModel_UserData = BLL.UserDataRigth.Instance.GetUserDataRigth(int.Parse(UserIDs));

            if (datadt == null || datadt.Rows.Count == 0)
            {
                modeldata.UserID = int.Parse(UserIDs);
                modeldata.RightType = int.Parse(RightType);
                modeldata.CreateTime = DateTime.Now;
                modeldata.CreateUserID = BLL.Util.GetLoginUserID();

                //如果没有，则插入
                BLL.UserDataRigth.Instance.Insert(modeldata);

                userDataRigthInsertLog(null, modeldata);
            }
            else
            {
                //如果有，则修改

                if (datadt.Rows[0]["RightType"].ToString() != RightType)
                {
                    //如果数据权限有变动，则更新
                    modeldata = BLL.UserDataRigth.Instance.GetUserDataRigth(int.Parse(datadt.Rows[0]["UserID"].ToString()));
                    modeldata.RightType = int.Parse(RightType);
                    retNum = BLL.UserDataRigth.Instance.Update(modeldata);

                    userDataRigthInsertLog(oldModel_UserData, modeldata);
                }

            }


            #endregion

            #region 分组设置
            if (!string.IsNullOrEmpty(GroupRightStr))
            {
                //先删除用户的分组
                BLL.UserGroupDataRigth.Instance.DeleteByUserID(int.Parse(UserIDs));
                BLL.Util.InsertUserLog("删除用户ID为：" + UserIDs + "，姓名为：" + BLL.Util.GetNameInHRLimitEID(int.Parse(UserIDs)) + " 下面的所属业务组");

                int userId = BLL.Util.GetLoginUserID();
                string[] groupRightArry = GroupRightStr.Split(';');
                foreach (string groupRight in groupRightArry)
                {
                    string[] arry = groupRight.Split('|');
                    Entities.UserGroupDataRigth groupDataRigthModel = new Entities.UserGroupDataRigth();
                    groupDataRigthModel.CreateTime = DateTime.Now;
                    groupDataRigthModel.CreateUserID = userId;
                    groupDataRigthModel.RightType = int.Parse(arry[1]);
                    groupDataRigthModel.UserID = int.Parse(UserIDs);
                    groupDataRigthModel.BGID = int.Parse(arry[0]);
                    int returnRecID = BLL.UserGroupDataRigth.Instance.Insert(groupDataRigthModel);
                    groupDataRigthModel.RecID = returnRecID;

                    userGroupDataRigthInsertLog(null, groupDataRigthModel);
                }
            }
            #endregion
        }

        //坐席表 插日志
        private void employeeAgentInsertLog(Entities.EmployeeAgent oldModel, Entities.EmployeeAgent newModel)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("UserID", "员工姓名");
            ht_FieldName.Add("AgentNum", "工号");
            ht_FieldName.Add("CreateTime", "编辑时间");
            ht_FieldName.Add("CreateUserID", "编辑人");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            ht_FieldType.Add("UserID", "UserID");
            ht_FieldType.Add("CreateUserID", "UserID");

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            if (oldModel == null)//为空，则是新增
            {
                //插入日志
                BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

                logStr = "坐席表新增：" + userLogStr;
            }
            else //不为空，则是编辑
            {
                //插入日志 
                BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

                logStr = "坐席表编辑：" + userLogStr;
            }

            if (userLogStr != string.Empty)
            {
                BLL.Util.InsertUserLog(logStr);
            }
        }

        //数据权限 插日志
        private void userDataRigthInsertLog(Entities.UserDataRigth oldModel, Entities.UserDataRigth newModel)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("UserID", "员工姓名");
            ht_FieldName.Add("RightType", "数据权限");
            ht_FieldName.Add("CreateTime", "编辑时间");
            ht_FieldName.Add("CreateUserID", "编辑人");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            Hashtable ht_RightType = new Hashtable();
            ht_FieldName.Add("1", "非全部");
            ht_FieldName.Add("2", "全部");

            ht_FieldType.Add("UserID", "UserID");
            ht_FieldType.Add("RightType", ht_FieldName);
            ht_FieldType.Add("CreateUserID", "UserID");

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            if (oldModel == null)//为空，则是新增
            {
                //插入日志
                BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

                logStr = "数据权限表新增：" + userLogStr;
            }
            else //不为空，则是编辑
            {
                //插入日志 
                BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

                logStr = "数据权限表编辑：" + userLogStr;
            }

            if (userLogStr != string.Empty)
            {
                BLL.Util.InsertUserLog(logStr);
            }
        }

        //分组权限 插日志
        private void userGroupDataRigthInsertLog(Entities.UserGroupDataRigth oldModel, Entities.UserGroupDataRigth newModel)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("UserID", "员工姓名");
            ht_FieldName.Add("RightType", "数据权限");
            ht_FieldName.Add("CreateTime", "编辑时间");
            ht_FieldName.Add("CreateUserID", "编辑人");
            ht_FieldName.Add("BGID", "所属分组");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            Hashtable ht_RightType = new Hashtable();
            ht_FieldName.Add("1", "本人");
            ht_FieldName.Add("2", "本组");

            ht_FieldType.Add("UserID", "UserID");
            ht_FieldType.Add("RightType", ht_FieldName);
            ht_FieldType.Add("CreateUserID", "UserID");
            ht_FieldType.Add("BGID", GetGroup());

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            if (oldModel == null && newModel != null)//为空，则是新增
            {
                //插入日志
                BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

                logStr = "分组权限表新增：" + userLogStr;
            }
            else if (oldModel != null && newModel != null) //不为空，则是编辑
            {
                //插入日志 
                BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

                logStr = "分组权限表编辑：" + userLogStr;
            }
            else if (oldModel != null && newModel == null)
            {
                //插入日志
                BLL.GetLogDesc.getDeleteLogInfo(oldModel, out userLogStr);

                logStr = "分组权限表信息删除：" + userLogStr;

                BLL.Util.InsertUserLog(logStr);
            }

            if (userLogStr != string.Empty)
            {
                BLL.Util.InsertUserLog(logStr);
            }
        }

        //业务组的ID和名称
        private Hashtable GetGroup()
        {
            Hashtable ht_msg = new Hashtable();

            DataTable dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ht_msg.Add(dr["BGID"].ToString(), dr["Name"].ToString());
            }

            return ht_msg;
        }

        private string SaveRolseSetting(string errMsg)
        {
            try
            {
                string[] userArr = UserIDs.Split(',');
                foreach (string userItem in userArr)
                {
                    bool isOk = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.InsertUserRole
                        (Convert.ToInt32(userItem), ConfigurationUtil.GetAppSettingValue("ThisSysID"), RoleIDs);
                }
            }
            catch (Exception ex)
            {
                errMsg = "modifyErr";
            }
            return errMsg;
        }

        /// <summary>
        /// 保存对坐席所属分组的修改
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private string SaveBusinessGroupSetting(string errMsg)
        {
            try
            {
                string[] userArr = UserIDs.Split(',');
                string[] bgidArr = BGIDS.Split(',');

                if (bgidArr.Length > 0 && !string.IsNullOrEmpty(bgidArr[0]))
                {
                    EmployeeAgent model;
                    for (int i = 0; i < userArr.Length; i++)
                    {

                        model = BitAuto.ISDC.CC2012.BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(Convert.ToInt32(userArr[i]));
                        if (model == null)
                        {
                            model = new EmployeeAgent();
                        }
                        model.UserID = Convert.ToInt32(userArr[i]);
                        model.BGID = Convert.ToInt32(bgidArr[0]);
                        model.RegionID = ReginId;
                        model.CreateUserID = BLL.Util.GetLoginUserID();

                        BitAuto.ISDC.CC2012.BLL.EmployeeAgent.Instance.Update(model);
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = "modifyErr";
            }
            return errMsg;
        }




        private void CheckedPar(out string errMsg)
        {
            errMsg = "";

            #region 检测用户ID

            if (UserIDs == string.Empty || UserIDs.Split(',').Length == 0)
            {
                errMsg = "noUserPar";
                return;
            }

            #endregion

            #region 检测角色ID

            string[] UserIDsList = UserIDs.Split(',');
            int intVal = 0;
            foreach (string item in UserIDsList)
            {
                if (!int.TryParse(item, out intVal))
                {
                    errMsg = "useridFormatErr"; //用户ID格式错误
                    break;
                }
            }
            if (errMsg != "")
            {
                return;
            }
            #endregion

            #region 是单个修改时，判断

            if (IsModfiySingle == "true")
            {
                #region 判断数据权限

                if (int.TryParse(RightType, out intVal))
                {
                    if (intVal != 1 && intVal != 2)
                    {
                        errMsg = "rightTypeErr";
                        return;
                    }
                }
                else
                {
                    errMsg = "rightTypeErr";
                    return;
                }

                #endregion

                #region 判断用户名

                if (UserIDs.Split(',').Length > 1)
                {
                    errMsg = "noUserPar";
                    return;
                }

                #endregion

                //#region 判断工号是否重复,如果IsContinute= "1"则不验证
                if (IsContinute != "1")
                {
                    //判断是否重复                  
                    Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
                    query.AgentNum = AgentNum;

                    int total = 0;
                    DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, "", 1, 10, out total);
                    if (total != 0)
                    {
                        if (dt.Rows[0]["UserID"].ToString() != UserIDs)
                        {
                            //与别人的工号有重复
                            errMsg = "repeated";
                            return;
                        }
                    }
                }

                //#endregion
            }

            #endregion

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