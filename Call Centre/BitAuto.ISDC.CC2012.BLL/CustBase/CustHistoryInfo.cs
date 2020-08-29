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

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CustHistoryInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustHistoryInfo
    {
        public static readonly CustHistoryInfo Instance = new CustHistoryInfo();

        protected CustHistoryInfo()
        { }

        #region 查询-功能废弃
        /// 根据任务ID查询历史信息-转让任务
        /// <summary>
        /// 根据任务ID查询历史信息-转让任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Entities.CustHistoryInfo GetCustHistoryInfo(string taskId)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(taskId);
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCustHistoryInfo(QueryCustHistoryInfo query, string order, int currentPage, int pageSize, string fields, out int totalCount)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(query, order, currentPage, pageSize, fields, out totalCount);
        }
        public DataTable GetCustHistoryInfoForExport(QueryCustHistoryInfo query, string fields)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfoForExport(query, fields);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustHistoryInfo GetCustHistoryInfo(long RecID)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(RecID);
        }

        public int Insert(SqlTransaction sqltran, Entities.CustHistoryInfo model)
        {
            return Dal.CustHistoryInfo.Instance.Insert(sqltran, model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {
            return Dal.CustHistoryInfo.Instance.Delete(RecID);
        }


        public QueryCustHistoryInfo GetQueryModel(string RequestTaskID, string RequestCustName, string RequestBeginTime, string RequestEndTime,
                              string RequestConsultID, string RequestQuestionType, string RequestQuestionQuality, string RequestIsComplaint,
                              string RequestProcessStatus, string RequestStatus, string RequestIsForwarding)
        {
            QueryCustHistoryInfo query = new QueryCustHistoryInfo();
            if (RequestTaskID != "")
            {
                query.TaskID = RequestTaskID;
            }
            if (RequestCustName != "")
            {
                query.CustName = System.Web.HttpUtility.UrlDecode(RequestCustName);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = RequestBeginTime;
            }
            if (RequestEndTime != "")
            {
                query.EndTime = RequestEndTime;
            }
            int consultID;
            if (int.TryParse(RequestConsultID, out consultID))
            {
                query.ConsultID = consultID;
            }
            if (RequestQuestionType != "")
            {
                query.QuestionType = RequestQuestionType;
            }

            if (RequestQuestionQuality != "")
            {
                query.QuestionQualityStr = RequestQuestionQuality;
            }
            if (RequestIsComplaint != "")
            {
                query.IsCompaintStr = RequestIsComplaint;
            }

            query.ProcessStatusStr = RequestProcessStatus;

            if (RequestStatus != "")
            {
                query.Status = RequestStatus;
            }
            if (RequestIsForwarding != "")
            {
                query.IsForwardingStr = RequestIsForwarding;
            }
            return query;
        }
        #endregion

        /// 根据TaskID、custID、BusinessType查找联系记录
        /// <summary>
        /// 根据TaskID、custID、BusinessType查找联系记录
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="businessType">业务类型：1工单，2团购订单，3客户核实，4其他任务</param>
        /// <returns></returns>
        public Entities.CustHistoryInfo GetCustHistoryInfo(string taskID, string custID, int businessType)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(taskID, custID, businessType);
        }
        /// 查询客户下是否有历史记录：删除个人用户
        /// <summary>
        /// 查询客户下是否有历史记录：删除个人用户
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public bool HavCustHistoryInfoByCustID(string custID)
        {
            return Dal.CustHistoryInfo.Instance.HavCustHistoryInfoByCustID(custID);
        }

        /// 更新CustHistoryInfo表 外呼挂断：工单，客户核实，其他任务，接口，集客，弹屏
        /// <summary>
        /// 更新CustHistoryInfo表 外呼挂断：工单，客户核实，其他任务，接口，集客，弹屏
        /// </summary>
        /// <param name="custhistoryinfo">CustID,TaskID，CallRecordID，RecordType，CreateUserID</param>
        /// <param name="msg">返回调用结果信息</param>
        public void InsertOrUpdateCustHistoryInfo(Entities.CustHistoryInfo custhistoryinfo, out string msg)
        {
            msg = "'result':'false'";
            try
            {
                int bType = custhistoryinfo.BusinessType.HasValue ? custhistoryinfo.BusinessType.Value : -1;
                //校验
                if (string.IsNullOrEmpty(custhistoryinfo.TaskID) || string.IsNullOrEmpty(custhistoryinfo.CustID) || bType <= 0)
                {
                    Loger.Log4Net.Info("[维护CustHistoryInfo] 参数错误失败【客户号】" + custhistoryinfo.CustID + "【任务ID】" + custhistoryinfo.TaskID + "【话务RecID】" + custhistoryinfo.CallRecordID + "【类型】" + bType + "【操作人】" + custhistoryinfo.CreateUserID + "【操作时间】" + DateTime.Now);
                    return;
                }
                string logDesc = string.Empty;

                //判断该TaskID是否有过记录，有记录更新，无记录就插入一条.
                Entities.CustHistoryInfo model = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(custhistoryinfo.TaskID, custhistoryinfo.CustID, bType);
                if (model == null)
                {
                    //model=null 表示不存在联系记录需要插入
                    model = custhistoryinfo;
                    BLL.CustHistoryInfo.Instance.Insert(model);
                    logDesc = "[新增CustHistoryInfo] 表插入记录：【TaskID】" + custhistoryinfo.TaskID + "【CallRecordID】" + custhistoryinfo.CallRecordID + "【CustID】" + custhistoryinfo.CustID + "【RecordType】" + bType + "【CreateTime】" + model.CreateTime + "【CreateUserID】" + custhistoryinfo.CreateUserID + "【RecordType】" + custhistoryinfo.RecordType;
                }
                else
                {
                    //存在记录，则更新
                    model.CallRecordID = custhistoryinfo.CallRecordID;
                    model.RecordType = custhistoryinfo.RecordType;
                    BLL.CustHistoryInfo.Instance.Update(model);
                    logDesc = "[修改CustHistoryInfo] 表更新记录：【RecID】" + model.RecID + "【CallRecordID】" + custhistoryinfo.CallRecordID + "【RecordType】" + custhistoryinfo.RecordType;
                }

                try
                {
                    Loger.Log4Net.Info(logDesc);
                    BLL.Util.InsertUserLogNoUser(logDesc);
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("[维护CustHistoryInfo] 日志失败：原因：", ex);
                }
                msg = "'result':'true'";
            }
            catch (Exception)
            {
                msg = "'result':'false'";
                Loger.Log4Net.Info("[维护CustHistoryInfo] 失败【客户号】" + custhistoryinfo.CustID + "【任务ID】" + custhistoryinfo.TaskID + "【操作人】" + custhistoryinfo.CreateUserID + "【操作时间】" + DateTime.Now);
            }
        }

        /// 增加一条数据
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.CustHistoryInfo model)
        {
            return Dal.CustHistoryInfo.Instance.Insert(model);
        }
        /// 更新一条数据
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CustHistoryInfo model)
        {
            return Dal.CustHistoryInfo.Instance.Update(model);
        }
    }
}

