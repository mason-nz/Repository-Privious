using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    /// 任务处理完成，再次处理话务类
    /// <summary>
    /// 任务处理完成，再次处理话务类
    /// 强斐
    /// 2016-7-30
    /// </summary>
    public class CallAfterTaskProcess
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
        public void UpdateCallRecordInfoAfterTask(string callids, string custid, string custname, ProjectSource tasktypeid, string taskid)
        {
            Dal.CallAfterTaskProcess.Instance.UpdateCallRecordInfoAfterNewTask(callids, custid, custname, tasktypeid, taskid);
        }
        /// 补充数据
        /// <summary>
        /// 补充数据
        /// </summary>
        /// <param name="callid"></param>
        /// <param name="businessid"></param>
        public void UpdateCallRecord_ORIG_BusinessAfterTask(string callids, string businessid)
        {
            Dal.CallAfterTaskProcess.Instance.UpdateCallRecord_ORIG_BusinessAfterNewTask(callids, businessid);
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
        public void UpdateSMSSendHistoryAfterTask(string smsids, string custid, string crmcustid, ProjectSource tasktype, string taskid)
        {
            Dal.CallAfterTaskProcess.Instance.UpdateSMSSendHistoryAfterNewTask(smsids, custid, crmcustid, tasktype, taskid);
        }
    }
}
