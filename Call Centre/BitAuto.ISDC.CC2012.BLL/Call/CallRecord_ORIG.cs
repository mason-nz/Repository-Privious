using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CallRecord_ORIG 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-16 04:11:45 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG
    {
        public static readonly CallRecord_ORIG Instance = new CallRecord_ORIG();

        protected CallRecord_ORIG()
        { }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG model)
        {
            return Dal.CallRecord_ORIG.Instance.Insert(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CallRecord_ORIG model)
        {
            return Dal.CallRecord_ORIG.Instance.Update(model);
        }

        #region 不区分时间方法
        /// 通过合力数据更新CC数据
        /// <summary>
        /// 通过合力数据更新CC数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int UpdateCallRecordORIGByHolly(DataTable dt, int worktype, Action<string, string> log)
        {
            BulkCopyToDB(dt, log);

            if (worktype == 1)
            {
                //批量更新-返回更新多少条
                string msg = Dal.CallRecord_ORIG.Instance.UpdateCallRecordORIGByHolly(dt);
                log("info", "批量更新：" + msg);
            }

            if (worktype == 2)
            {
                //未接来电数据批量插入留言表（来源：1 留言 2未接来电）
                int num2 = Dal.CustomerVoiceMsg.Instance.BCPCustomerVoiceMsgHolly();
                log("info", "未接来电数据批量插入留言表（来源：1 留言 2未接来电）" + num2);
            }
            return dt.Rows.Count;
        }
        /// 同步数据
        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="dt"></param>
        public void BulkCopyToDB(DataTable dt, Action<string, string> log)
        {
            //清空临时表数据
            Dal.CallRecord_ORIG.Instance.ClearHollyDataTemp();
            //映射关系
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("SessionID", "SessionID"));
            list.Add(new SqlBulkCopyColumnMapping("DEVICEDN", "DEVICEDN"));
            list.Add(new SqlBulkCopyColumnMapping("ORIANI", "ORIANI"));
            list.Add(new SqlBulkCopyColumnMapping("ORIDNIS", "ORIDNIS"));
            list.Add(new SqlBulkCopyColumnMapping("CALLDIRECTION", "CALLDIRECTION"));
            list.Add(new SqlBulkCopyColumnMapping("SKILLID", "SKILLID"));
            list.Add(new SqlBulkCopyColumnMapping("InitiatedTime", "InitiatedTime"));
            list.Add(new SqlBulkCopyColumnMapping("RingingTime", "RingingTime"));
            list.Add(new SqlBulkCopyColumnMapping("EstablishedTime", "EstablishedTime"));
            list.Add(new SqlBulkCopyColumnMapping("AgentReleaseTime", "AgentReleaseTime"));
            list.Add(new SqlBulkCopyColumnMapping("CustomerReleaseTime", "CustomerReleaseTime"));
            list.Add(new SqlBulkCopyColumnMapping("AfterWorkBeginTime", "AfterWorkBeginTime"));
            list.Add(new SqlBulkCopyColumnMapping("EndTime", "EndTime"));
            list.Add(new SqlBulkCopyColumnMapping("TallTime", "TallTime"));
            list.Add(new SqlBulkCopyColumnMapping("AudioURL", "AudioURL"));
            list.Add(new SqlBulkCopyColumnMapping("VARAGENTIDZ", "VARAGENTIDZ"));

            list.Add(new SqlBulkCopyColumnMapping("istransfer", "istransfer"));
            list.Add(new SqlBulkCopyColumnMapping("isconsult", "isconsult"));
            list.Add(new SqlBulkCopyColumnMapping("isconference", "isconference"));
            //批量新增
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.CallRecord_ORIG.Instance.Conn, "HollyDataTemp", 10000, list, out msg);
            log("info", "BulkCopyToDB：" + msg);
            //清除重复数据
            int del = Dal.CallRecord_ORIG.Instance.DeleteSameData();
            log("info", "清除重复数据：" + del);
        }
        /// 取客户核实任务状态描述
        /// <summary>
        /// 取客户核实任务状态描述
        /// </summary>
        /// <param name="stopstatus"></param>
        /// <param name="taskStatus"></param>
        /// <param name="applytype"></param>
        /// <returns></returns>
        public string GetStatusNameForCRMStop(string stopstatus, string taskStatus, string applytype)
        {
            string result = string.Empty;
            int _status = CommonFunction.ObjectToInteger(stopstatus);
            result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustStopStatus), _status);
            //停用类型
            if (applytype == "1")
            {
                if (_status == 2)
                {
                    result = "待停用";
                }
                else if (_status == 3)
                {
                    result = "已停用";
                }
            }
            //启用类型
            else if (applytype == "2")
            {
                if (_status == 2)
                {
                    result = "待启用";
                }
                else if (_status == 3)
                {
                    result = "已启用";
                }
            }
            //待审核
            if (_status == 1)
            {
                result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustTaskStatus), CommonFunction.ObjectToInteger(taskStatus));
            }
            return result;
        }
        #endregion

        #region 分月查询
        /// 话务总表数据查询
        /// <summary>
        /// 话务总表数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIGByList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGByList(query, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// 取满意度汇总列表
        /// <summary>
        /// 取满意度汇总列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlaceID">1是西安，2是北京</param>
        /// <param name="DateType">1是日，2是周，3是月</param>
        /// <returns></returns>
        public DataTable GetSatisfactionList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, out int totalCount, int PlaceID, DateTime b, DateTime e, int DateType, string tableEndName)
        {
            return Dal.CallRecord_ORIG.Instance.GetSatisfactionList(query, order, currentPage, pageSize, out totalCount, PlaceID, b, e, DateType, tableEndName);
        }
        /// 取满意度汇总列表（总汇总）
        /// <summary>
        /// 取满意度汇总列表（总汇总）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlaceID">1是西安，2是北京</param>
        /// <returns></returns>
        public DataTable GetSatisfactionList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, out int totalCount, int PlaceID, DateTime b, DateTime e, string TableEndName)
        {
            return Dal.CallRecord_ORIG.Instance.GetSatisfactionList(query, order, currentPage, pageSize, out totalCount, PlaceID, b, e, TableEndName);
        }
        /// 分月查询实体类
        /// <summary>
        /// 分月查询实体类
        /// </summary>
        /// <param name="CallID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGByCallID(long CallID, string tableEndName)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(CallID, tableEndName);
        }
        #endregion

        #region 接口调用，只查询现表
        /// 惠买车根据业务分组，分类获取一段时间范围内的话务总表数据
        /// <summary>
        /// 惠买车根据业务分组，分类获取一段时间范围内的话务总表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallDataHuiMC(string starttime, string endtime, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallDataHuiMC(starttime, endtime, order, currentPage, pageSize, out totalCount);
        }
        /// 惠买车获取Inbound话务总表数据
        /// <summary>
        /// 惠买车获取Inbound话务总表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetInboundDataHuiMC(string starttime, string endtime, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CallRecord_ORIG.Instance.GetInboundDataHuiMC(starttime, endtime, order, currentPage, pageSize, out totalCount);
        }
        /// 根据业务分组，分类获取一段时间范围内的话务总表数据
        /// <summary>
        /// 根据业务分组，分类获取一段时间范围内的话务总表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="bgid"></param>
        /// <param name="scid"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallData(string starttime, string endtime, int bgid, int scid, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallData(starttime, endtime, bgid, scid, order, currentPage, pageSize, out totalCount);
        }
        /// 获取话务数据接口（易集客调用）
        /// <summary>
        /// 获取话务数据接口（易集客调用）
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIGByYiJiKe(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGByYiJiKe(where, order, currentPage, pageSize, out totalCount);
        }
        /// 青牛接口调用 废弃
        /// <summary>
        /// 青牛接口调用 废弃
        /// </summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGBySessionID(string SessionID)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGBySessionID(SessionID);
        }
        /// 查询当前表中的实体
        /// <summary>
        /// 查询当前表中的实体
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGByCallID(long CallID)
        {
            //查询现在表
            return GetCallRecord_ORIGByCallID(CallID, "");
        }
        /// IM根据客户号获取业务记录
        /// <summary>
        /// IM根据客户号获取业务记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBaseInfo_ServiceRecord_IM(int userid, QueryCustHistoryInfo query, int currentPage, int pageSize, out int totalCount)
        {
            DataTable dt = new DataTable();
            dt.TableName = "CustHistoryInfo";
            totalCount = 0;
            try
            {
                string phones = "SELECT Tel FROM dbo.CustTel WHERE CustID='" + query.CustID + "'";
                DataTable mydt = Dal.CallRecord_ORIG.Instance.GetCustBaseInfo_ServiceRecord(phones, "ModifyTime desc", currentPage, pageSize, out totalCount);

                dt.Columns.Add("AssignUserID", typeof(string));//提交人
                dt.Columns.Add("Status", typeof(string));//状态
                dt.Columns.Add("TaskID", typeof(string));//任务ID              
                dt.Columns.Add("TaskUrl", typeof(string));//查看页地址
                dt.Columns.Add("Content", typeof(string));//联系记录
                dt.Columns.Add("RecordType", typeof(string));//记录类型
                dt.Columns.Add("LastOperTime", typeof(DateTime));//提交时间
                string url_head = ConfigurationUtil.GetAppSettingValue("IMTaskMiddleDomain") + "/IMTaskMiddle.aspx?" + "UserID=" + System.Web.HttpUtility.UrlEncode(userid.ToString());

                foreach (DataRow row in mydt.Rows)
                {
                    string url = "";
                    //任务状态
                    string taskstatus = CommonFunction.ObjectToString(row["TaskStatus"]);
                    string stopstatus = CommonFunction.ObjectToString(row["StopStatus"]);
                    //类型
                    string bussinessType = CommonFunction.ObjectToString(row["BusinessType"]);
                    //客户核实类型
                    string applytype = CommonFunction.ObjectToString(row["ApplyType"]);
                    //任务ID
                    string taskid = CommonFunction.ObjectToString(row["TaskID"]);
                    //来源
                    string tasksource = CommonFunction.ObjectToString(row["TaskSource"]);
                    //创建人
                    string createuserid = CommonFunction.ObjectToString(row["CreateUserID"]);

                    DataRow dr = dt.NewRow();
                    dr["AssignUserID"] = createuserid;
                    dr["Status"] = GetStatusText(CommonFunction.ObjectToInteger(bussinessType), taskstatus, stopstatus, applytype);
                    dr["TaskID"] = taskid;
                    dr["RecordType"] = tasksource;
                    url += "&BussinessType=" + System.Web.HttpUtility.UrlEncode(bussinessType);
                    url += "&TaskID=" + System.Web.HttpUtility.UrlEncode(taskid);
                    url += "&BGID=" + System.Web.HttpUtility.UrlEncode(Convert.ToString(row["BGID"]));
                    url += "&SCID=" + System.Web.HttpUtility.UrlEncode(Convert.ToString(row["SCID"]));
                    dr["TaskUrl"] = url_head + url;
                    dr["Content"] = Convert.ToString(row["Content"]);
                    dr["LastOperTime"] = CommonFunction.ObjectToString(row["LastOperTime"]);
                    dt.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[CustHistoryInf_BLL]GetCustHistoryInfoForWork_IM操作出错!errorMessage:" + ex.Message);
                BLL.Loger.Log4Net.Info("[CustHistoryInf_BLL]GetCustHistoryInfoForWork_IM操作出错!errorStackTrace:" + ex.StackTrace);
            }
            return dt;
        }
        /// 业务记录
        /// <summary>
        /// 业务记录
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBaseInfo_ServiceRecord(string phonenums, string order, int currentPage, int pageSize, out int totalCount)
        {
            phonenums = Dal.Util.SqlFilterByInCondition(phonenums);
            return Dal.CallRecord_ORIG.Instance.GetCustBaseInfo_ServiceRecord(phonenums, order, currentPage, pageSize, out totalCount);
        }
        /// 话务记录
        /// <summary>
        /// 话务记录
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBaseInfo_TrafficRecord(string phonenums, string order, int currentPage, int pageSize, out int totalCount)
        {
            phonenums = Dal.Util.SqlFilterByInCondition(phonenums);
            return Dal.CallRecord_ORIG.Instance.GetCustBaseInfo_TrafficRecord(phonenums, order, currentPage, pageSize, out totalCount);
        }
        /// 根据主键ID，获取从主键ID之后的最新数据，最大获取十万条
        /// <summary>
        /// 根据主键ID，获取从主键ID之后的最新数据，最大获取十万条
        /// </summary>
        /// <param name="maxID">最大主键ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetCallRecord_ORIGByMaxID(int maxID)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGByMaxID(maxID);
        }
        ///根据手机号码，查询在CC系统中，最近一次外呼的（InitiatedTime）初始化时间
        /// <summary>
        ///根据手机号码，查询在CC系统中，最近一次外呼的（InitiatedTime）初始化时间
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetPhoneLastestInitiatedTime(string phoneNumber)
        {
            return Dal.CallRecord_ORIG.Instance.GetPhoneLastestInitiatedTime(phoneNumber);
        }
        #endregion

        /// 取工单状态
        /// <summary>
        /// 取工单状态
        /// </summary>
        /// <param name="workorderstatus"></param>
        /// <returns></returns>
        public string GetStatusText(int bussinessType, string taskstatus, string crmstop_stopstatus, string crmstop_applytype)
        {
            if (string.IsNullOrEmpty(taskstatus))
            {
                return "";
            }
            switch (bussinessType)
            {
                case 1:
                    return BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.WorkOrderStatus), Convert.ToInt32(taskstatus));
                case 3:
                    return GetStatusNameForCRMStop(crmstop_stopstatus, taskstatus, crmstop_applytype);
                case 4:
                    return BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.OtheTaskStatus), Convert.ToInt32(taskstatus));
                case 5:
                case 6:
                    return BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.LeadsTaskStatus), Convert.ToInt32(taskstatus));
                case 7:
                    return BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.YTGActivityTaskStatus), Convert.ToInt32(taskstatus));
                default:
                    return "";
            }
        }

        #region 同步服务
        /// 拆分话务数据，14年数据放到old表中
        /// <summary>
        /// 拆分话务数据，14年数据放到old表中
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> SplitCallDataForOld(Action<string> logFunc)
        {
            return Dal.CallRecord_ORIG.Instance.SplitCallDataForOld(logFunc);
        }
        /// 拆分话务相关的表
        /// <summary>
        /// 拆分话务相关的表
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Dictionary<string, int> SplitCallDataForService(DateTime date, Action<string> logFunc)
        {
            return Dal.CallRecord_ORIG.Instance.SplitCallDataForService(date, logFunc);
        }

        /// 按月查询话务数据
        /// <summary>
        /// 按月查询话务数据
        /// </summary>
        /// <param name="endtablename"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void GetAllCallRecordORIGForHB(DateTime month, out int count, Action<string> callback)
        {
            string endtablename = BLL.Util.CalcTableNameByMonth(3, month);
            Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB(endtablename, month, out count, callback, new Func<string, string>(Util.HaoMaProcess));
        }
        /// 按时间段查询现在话务数据
        /// <summary>
        /// 按时间段查询现在话务数据
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public void GetAllCallRecordORIGForHB(DateTime st, DateTime et, out int count, Action<string> callback)
        {
            Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB(st, et, out count, callback, new Func<string, string>(Util.HaoMaProcess));
        }
        /// 查询Old表，取全部的数据
        /// <summary>
        /// 查询Old表，取全部的数据
        /// </summary>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public void GetAllCallRecordORIGForHB(out int count, Action<string> callback)
        {
            string endtablename = "_old";
            Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB(endtablename, out count, callback, new Func<string, string>(Util.HaoMaProcess));
        }
        /// 获取查询列
        /// <summary>
        /// 获取查询列
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCallRecordORIGForHB_SelectCol()
        {
            return Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB_SelectCol();
        }

        /// 获取话务总表存在问题的数据
        /// <summary>
        /// 获取话务总表存在问题的数据
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public List<Entities.CallRecord_ORIG> GetCallRecord_ORIGForError(DateTime st)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGForError(st);
        }
        #endregion
    }
}

