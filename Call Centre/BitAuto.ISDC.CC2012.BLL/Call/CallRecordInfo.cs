using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CallRecordInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecordInfo
    {
        public static readonly CallRecordInfo Instance = new CallRecordInfo();

        protected CallRecordInfo()
        { }

        #region 旧版插入和更新作废，请用新版CallRecordInfoInfo类实现
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.CallRecordInfo model)
        {
            return Dal.CallRecordInfo.Instance.Insert(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CallRecordInfo model)
        {
            return Dal.CallRecordInfo.Instance.Update(model);
        }
        #endregion

        #region 分月查询
        /// 查询来去电数据
        /// <summary>
        /// 查询来去电数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="userid">当前用户ID</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns></returns>
        public DataTable GetCallRecordInfo(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfo(query, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// 根据任务id查询话务信息
        /// <summary>
        /// 根据任务id查询话务信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable GetCallRecordByTaskID(string TaskID, string tableEndName)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordByTaskID(TaskID, tableEndName);
        }
        /// 获取实体类
        /// <summary>
        /// 获取实体类
        /// </summary>
        /// <param name="callID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByCallID(long callID, string tableEndName)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoByCallID(callID, tableEndName);
        }
        #endregion

        #region 废弃功能
        /// 回访列表-功能废弃
        /// <summary>
        /// 回访列表-功能废弃
        /// </summary>
        /// <param name="queryCC_CallRecords"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCC_CallRecordsByRV(QueryCallRecordInfo queryCC_CallRecords, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CallRecordInfo.Instance.GetCC_CallRecordsByRV(queryCC_CallRecords, currentPage, pageSize, out totalCount);
        }
        /// 功能废弃-RVID无值
        /// <summary>
        /// 功能废弃-RVID无值
        /// </summary>
        /// <param name="RVID"></param>
        /// <returns></returns>
        public DataTable GetCallRecordByRVID(string RVID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordByRVID(RVID);
        }
        /// 根据任务ID查询实体类-只能查询现在表-功能废弃
        /// <summary>
        /// 根据任务ID查询实体类-只能查询现在表-功能废弃
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByTaskID(string taskID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoByTaskID(taskID);
        }
        /// 根据客户ID查询-只能查询现在表-删除客户检验时用-功能废弃
        /// <summary>
        /// 根据客户ID查询-只能查询现在表-删除客户检验时用-功能废弃
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public bool HavCarRecordInfoByCustID(string custID)
        {
            return Dal.CallRecordInfo.Instance.HavCarRecordInfoByCustID(custID);
        }
        /// 会员在CRM创建成功后，回写到表CC_CallRecords中
        /// <summary>
        /// 会员在CRM创建成功后，回写到表CC_CallRecords中
        /// </summary>
        /// <param name="cc_DMSMemberID">CC系统中会员ID</param>
        /// <param name="memberID">CRM系统中会员ID</param>
        public void UpdateOriginalDMSMemberIDByID(int cc_DMSMemberID, string memberID)
        {
            if (Dal.CallRecordInfo.Instance.UpdateOriginalDMSMemberIDByID(cc_DMSMemberID, memberID) > 0)
            {
                string content = "回写会员ID，在表CC_CallRecords中，将CCMemberID字段为:" + cc_DMSMemberID + "的值，改为NULL，DMSMemberID改为:" + memberID;
                //插入日志
                BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("BindRecordLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
            }
        }
        /// 客户在CRM创建成功后，回写到表CC_CallRecords中
        /// <summary>
        /// 客户在CRM创建成功后，回写到表CC_CallRecords中
        /// </summary>
        /// <param name="cc_DMSMemberID">CC系统中Excel导入客户信息ID</param>
        /// <param name="memberID">CRM系统中客户ID</param>
        public void UpdateCRMCustIDByID(string cccustid, string custid)
        {
            if (Dal.CallRecordInfo.Instance.UpdateCRMCustIDByID(cccustid, custid) > 0)
            {
                string content = "回写客户ID，在表CC_CallRecords中，将CCCustID字段为:" + cccustid + "的值，改为NULL，CRMCustID改为:" + custid;
                //插入日志
                BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("BindRecordLogModuleID"),
                    (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
            }
        }
        #endregion

        #region 其他功能
        /// 根据条件，获取查询类
        /// <summary>
        /// 根据条件，获取查询类
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ANI"></param>
        /// <param name="Agent">坐席名称</param>
        /// <param name="TaskID"></param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="AgentNum"></param>
        /// <param name="PhoneNum"></param>
        /// <param name="TaskCategory"></param>
        /// <param name="SpanTime1"></param>
        /// <param name="SpanTime2"></param>
        /// <param name="AgentGroup"></param>
        /// <returns></returns>
        public QueryCallRecordInfo GetQueryModel(string Name, string ANI, string Agent, string TaskID, string CallID,
                     string BeginTime, string EndTime, string AgentNum, string PhoneNum, string TaskCategory,
                     string SpanTime1, string SpanTime2, string AgentGroup, string CallStatus, int loginID,
                     string ownGroup, string oneSelf, string Category, string ivrScore, string icomingSource, string selBusinessType, string SelSolve)
        {

            DateTime tmpDt = new DateTime();
            int tmpInt = 0;
            Int64 tmpInt64 = 0;

            QueryCallRecordInfo query = new QueryCallRecordInfo();

            if (icomingSource != "" && int.TryParse(icomingSource, out tmpInt))
            {
                query.IncomingSource = tmpInt;
            }
            if (ivrScore != "" && ivrScore != "-1" && int.TryParse(ivrScore, out tmpInt))
            {
                query.IVRScore = tmpInt;
            }
            if (Name != "")
            {
                query.CustName = Name;
            }
            if (ANI != "")
            {
                query.ANI = ANI;
            }
            if (Agent != "")
            {
                query.Agent = Agent;
            }
            if (TaskID != "")
            {
                query.TaskID = TaskID;
            }
            if (CallID != "" && Int64.TryParse(CallID, out tmpInt64))
            {
                query.CallID = tmpInt64;
            }
            if (BeginTime != "" && DateTime.TryParse(BeginTime, out tmpDt))
            {
                query.BeginTime = tmpDt;
            }
            if (EndTime != "" && DateTime.TryParse(EndTime, out tmpDt))
            {
                query.EndTime = tmpDt;
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                query.AgentNum = AgentNum;
            }

            if (PhoneNum != "")
            {
                query.PhoneNum = PhoneNum;
            }
            if (TaskCategory != "" && int.TryParse(TaskCategory, out tmpInt))
            {
                query.TaskTypeID = tmpInt;
            }

            if (SpanTime1 != "" && int.TryParse(SpanTime1, out tmpInt))
            {
                query.SpanTime1 = tmpInt;
            }
            if (SpanTime2 != "" && int.TryParse(SpanTime2, out tmpInt))
            {
                query.SpanTime2 = tmpInt;
            }
            if (AgentGroup != "" && int.TryParse(AgentGroup, out tmpInt) && AgentGroup != "-1")
            {
                query.BGID = tmpInt;
            }
            if (Category != "" && int.TryParse(Category, out tmpInt) && Category != "-1")
            {
                query.SCID = tmpInt;
            }
            if (CallStatus != "" && int.TryParse(CallStatus, out tmpInt))
            {
                query.CallStatus = tmpInt;
            }
            if (selBusinessType != "" && selBusinessType != "-1")
            {
                query.selBusinessType = selBusinessType;
            }
            if (SelSolve != "" && SelSolve != "-1")
            {
                query.SelSolve = CommonFunction.ObjectToInteger(SelSolve);
            }

            #region 分组权限
            if (loginID != Constant.INT_INVALID_VALUE)
            {
                query.LoginID = loginID;
            }
            if (ownGroup != string.Empty)
            {
                query.OwnGroup = ownGroup;
            }
            if (oneSelf != string.Empty)
            {
                query.OneSelf = oneSelf;
            }
            #endregion

            return query;
        }
        public string GetViewUrl(string TaskTypeID, string TaskID, string carType, string source)
        {
            string url = "";

            if (TaskTypeID == "6")
            {
                //其他分类
                url = "/CRMStopCust/Edit.aspx?TaskID=" + TaskID;
            }

            if (TaskTypeID == "5")
            {
                //其他分类
                url = "/OtherTask/OtherTaskDealView.aspx?OtherTaskID=" + TaskID;
            }

            if (TaskTypeID == "4")
            {
                //客户回访
                url = "/ReturnVisit/ReturnVisitRecordView.aspx?RVID=" + TaskID;
            }

            if (TaskTypeID == "3")
            {
                //个人业务
                url = "/TaskManager/TaskDetail.aspx?TaskID=" + TaskID;
            }
            else if (TaskTypeID == "2")
            {
                //无主订单
                url = "/TaskManager/NoDealerOrder/NoDealerOrderView.aspx?TaskID=" + TaskID;
            }
            else if (TaskTypeID == "1")
            {
                //数据清洗

                if (source == "2")
                {
                    //CRM
                    if (carType == "2")
                    {
                        url = "/CustCheck/CrmCustCheck/SecondCarView.aspx?TID=" + TaskID + "&Action=view";
                    }
                    else
                    {
                        url = "/CustCheck/CrmCustCheck/View.aspx?TID=" + TaskID + "&Action=view";
                    }
                }
                else
                {
                    //Excel
                    if (carType == "2")
                    {
                        url = "/CustCheck/NewCustCheck/SecondCarView.aspx?TID=" + TaskID + "&Action=view";
                    }
                    else
                    {
                        url = "/CustCheck/NewCustCheck/View.aspx?TID=" + TaskID + "&Action=view";
                    }
                }
            }


            return url;
        }

        /// 其他系统-接口
        /// <summary>
        /// 其他系统-接口
        /// </summary>
        /// <param name="model"></param>
        /// <param name="model_ORIG"></param>
        /// <param name="recID"></param>
        /// <returns></returns>
        public bool InsertCallRecordInfoToHuiMaiChe(CallRecordInfoInfo model, Entities.CallRecord_ORIG model_ORIG, out long recID)
        {
            recID = 0;
            string logDesc = string.Empty;
            if (model_ORIG.EstablishedTime == null)
            {
                logDesc = "该通电话未接通";
                BLL.Loger.Log4Net.Info("【其他系统接口话务调用】【来电去电表插入成功】【失败】" + logDesc);
                return false;
            }

            CallRecordInfoInfo model_RecordInfo = BLL.CallRecordInfo.Instance.GetCallRecordInfoInfo(model.CallID.Value);
            if (model_RecordInfo != null)
            {
                recID = model_RecordInfo.RecID_Value;
                logDesc = " 该CallID：" + model.CallID + "记录在CallRecordInfo表中已存在，不能再次插入CallRecordInfo表，返回主键：" + model_RecordInfo.RecID + "！";
                BLL.Loger.Log4Net.Info("【其他系统接口话务调用】【来电去电表插入成功】" + logDesc);
                return true;
            }
            try
            {
                model.SessionID = model_ORIG.SessionID;
                model.ExtensionNum = model_ORIG.ExtensionNum;
                model.PhoneNum = BLL.Util.HaoMaProcess(model_ORIG.ANI);
                model.ANI = model_ORIG.PhoneNum;
                model.CallStatus = model_ORIG.CallStatus;
                model.BeginTime = model_ORIG.EstablishedTime;
                model.EndTime = model_ORIG.CustomerReleaseTime == null ? model_ORIG.AgentReleaseTime : model_ORIG.CustomerReleaseTime;
                model.TallTime = model_ORIG.TallTime;
                model.AudioURL = model_ORIG.AudioURL;
                model.SkillGroup = model_ORIG.SkillGroup;
                //recordInfo.CallID = long.Parse(info.CallID);// 外面赋值
                //recordInfo.SCID = int.Parse(info.SCID);// 外面赋值
                //recordInfo.TaskID = info.BusinessID;// 外面赋值
                //recordInfo.TaskTypeID = (int)ProjectSource.None;// 外面赋值
                //recordInfo.BGID = int.Parse(info.BGID);// 外面赋值
                //recordInfo.CustID = custId;// 外面赋值
                //recordInfo.CustName = info.CustName;// 外面赋值
                //recordInfo.Contact = info.CustName;// 外面赋值
                model.CreateTime = DateTime.Now;
                model.CreateUserID = model_ORIG.CreateUserID;
                CommonBll.Instance.InsertComAdoInfo(model);
                recID = model.RecID_Value;
                BLL.Loger.Log4Net.Info("【其他系统接口话务调用】【来电去电表插入成功】返回主键：" + recID);
                return true;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("【其他系统接口话务调用】【来电去电表插入成功】", ex);
                return false;
            }
        }

        /// 得到一个对象实体--只查询现在表
        /// <summary>
        /// 得到一个对象实体--只查询现在表
        /// </summary>
        public Entities.CallRecordInfo GetCallRecordInfo(long RecID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfo(RecID);
        }
        /// 根据callid获取实体类-只查询现在表
        /// <summary>
        /// 根据callid获取实体类-只查询现在表
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByCallID(long CallID)
        {
            //查询现在表
            return GetCallRecordInfoByCallID(CallID, "");
        }
        /// 根据SessionID获取实体类-只查询现在表
        /// <summary>
        /// 根据SessionID获取实体类-只查询现在表
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoBySessionID(string SessionID)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoBySessionID(SessionID);
        }
        /// 获取未成功原因
        /// <summary>
        /// 获取未成功原因
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public string GetNotSuccessReason(long ProjectId)
        {
            return Dal.CallRecordInfo.Instance.GetNotSuccessReason(ProjectId);
        }
        /// 根据CallID获取录音AudioURL
        /// <summary>
        /// 根据CallID获取录音AudioURL
        /// </summary>
        /// <param name="callID"></param>
        /// <returns></returns>
        public string GetAudioURLByCallID(string callID)
        {
            return Dal.CallRecordInfo.Instance.GetAudioURLByCallID(callID);
        }
        #endregion

        #region 新版CallRecordInfoInfo
        /// 查询实体类型
        /// <summary>
        /// 查询实体类型
        /// </summary>
        /// <param name="callid"></param>
        /// <returns></returns>
        public CallRecordInfoInfo GetCallRecordInfoInfo(long callid)
        {
            return Dal.CallRecordInfo.Instance.GetCallRecordInfoInfo(callid);
        }
        #endregion
    }
}

