using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Data;
using System.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CallAfterTaskProcess : DataBase
    {
        public static CallAfterTaskProcess Instance = new CallAfterTaskProcess();

        /// 补充数据
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="callid"></param>
        /// <param name="custid"></param>
        /// <param name="custname"></param>
        /// <param name="tasktypeid"></param>
        /// <param name="taskid"></param>
        public void UpdateCallRecordInfoAfterNewTask(string callids, string custid, string custname, ProjectSource tasktypeid, string taskid)
        {
            string sql = @"UPDATE CallRecordInfo 
                                    SET CustID='" + SqlFilter(custid) + "',CustName='" + SqlFilter(custname) + "',TaskTypeID=" + (int)tasktypeid + ",TaskID='" + SqlFilter(taskid) + @"'
                                    WHERE CallID IN (" + Dal.Util.SqlFilterByInCondition(callids) + ")";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        /// 补充数据
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="callid"></param>
        /// <param name="businessid"></param>
        public void UpdateCallRecord_ORIG_BusinessAfterNewTask(string callids, string businessid)
        {
            string sql = @"UPDATE CallRecord_ORIG_Business
                                    SET BusinessID='" + SqlFilter(businessid) + @"'
                                    WHERE CallID IN (" + Dal.Util.SqlFilterByInCondition(callids) + ")";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            //不用触发CallRecord_ORIG_Task表的更新
            //CallRecord_ORIG_Task记录的项目任务数据，所以在电话中就已维护，不会走新建任务以后维护的逻辑
        }
        /// 补充数据
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="custid"></param>
        /// <param name="crmcustid"></param>
        /// <param name="tasktype"></param>
        /// <param name="taskid"></param>
        public void UpdateSMSSendHistoryAfterNewTask(string smsids, string custid, string crmcustid, ProjectSource tasktype, string taskid)
        {
            string sql = @"UPDATE SMSSendHistory
                                    SET CustID='" + SqlFilter(custid) + "', CRMCustID='" + SqlFilter(crmcustid) + "',TaskType=" + (int)tasktype + ",TaskID='" + SqlFilter(taskid) + @"'
                                    WHERE RecID IN (" + Dal.Util.SqlFilterByInCondition(smsids) + ")";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}
