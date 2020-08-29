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
    /// ҵ���߼���CallRecord_ORIG ��ժҪ˵����
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
        /// ����һ������
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG model)
        {
            return Dal.CallRecord_ORIG.Instance.Insert(model);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CallRecord_ORIG model)
        {
            return Dal.CallRecord_ORIG.Instance.Update(model);
        }

        #region ������ʱ�䷽��
        /// ͨ���������ݸ���CC����
        /// <summary>
        /// ͨ���������ݸ���CC����
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int UpdateCallRecordORIGByHolly(DataTable dt, int worktype, Action<string, string> log)
        {
            BulkCopyToDB(dt, log);

            if (worktype == 1)
            {
                //��������-���ظ��¶�����
                string msg = Dal.CallRecord_ORIG.Instance.UpdateCallRecordORIGByHolly(dt);
                log("info", "�������£�" + msg);
            }

            if (worktype == 2)
            {
                //δ���������������������Ա���Դ��1 ���� 2δ�����磩
                int num2 = Dal.CustomerVoiceMsg.Instance.BCPCustomerVoiceMsgHolly();
                log("info", "δ���������������������Ա���Դ��1 ���� 2δ�����磩" + num2);
            }
            return dt.Rows.Count;
        }
        /// ͬ������
        /// <summary>
        /// ͬ������
        /// </summary>
        /// <param name="dt"></param>
        public void BulkCopyToDB(DataTable dt, Action<string, string> log)
        {
            //�����ʱ������
            Dal.CallRecord_ORIG.Instance.ClearHollyDataTemp();
            //ӳ���ϵ
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
            //��������
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.CallRecord_ORIG.Instance.Conn, "HollyDataTemp", 10000, list, out msg);
            log("info", "BulkCopyToDB��" + msg);
            //����ظ�����
            int del = Dal.CallRecord_ORIG.Instance.DeleteSameData();
            log("info", "����ظ����ݣ�" + del);
        }
        /// ȡ�ͻ���ʵ����״̬����
        /// <summary>
        /// ȡ�ͻ���ʵ����״̬����
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
            //ͣ������
            if (applytype == "1")
            {
                if (_status == 2)
                {
                    result = "��ͣ��";
                }
                else if (_status == 3)
                {
                    result = "��ͣ��";
                }
            }
            //��������
            else if (applytype == "2")
            {
                if (_status == 2)
                {
                    result = "������";
                }
                else if (_status == 3)
                {
                    result = "������";
                }
            }
            //�����
            if (_status == 1)
            {
                result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustTaskStatus), CommonFunction.ObjectToInteger(taskStatus));
            }
            return result;
        }
        #endregion

        #region ���²�ѯ
        /// �����ܱ����ݲ�ѯ
        /// <summary>
        /// �����ܱ����ݲ�ѯ
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
        /// ȡ����Ȼ����б�
        /// <summary>
        /// ȡ����Ȼ����б�
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlaceID">1��������2�Ǳ���</param>
        /// <param name="DateType">1���գ�2���ܣ�3����</param>
        /// <returns></returns>
        public DataTable GetSatisfactionList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, out int totalCount, int PlaceID, DateTime b, DateTime e, int DateType, string tableEndName)
        {
            return Dal.CallRecord_ORIG.Instance.GetSatisfactionList(query, order, currentPage, pageSize, out totalCount, PlaceID, b, e, DateType, tableEndName);
        }
        /// ȡ����Ȼ����б��ܻ��ܣ�
        /// <summary>
        /// ȡ����Ȼ����б��ܻ��ܣ�
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlaceID">1��������2�Ǳ���</param>
        /// <returns></returns>
        public DataTable GetSatisfactionList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, out int totalCount, int PlaceID, DateTime b, DateTime e, string TableEndName)
        {
            return Dal.CallRecord_ORIG.Instance.GetSatisfactionList(query, order, currentPage, pageSize, out totalCount, PlaceID, b, e, TableEndName);
        }
        /// ���²�ѯʵ����
        /// <summary>
        /// ���²�ѯʵ����
        /// </summary>
        /// <param name="CallID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGByCallID(long CallID, string tableEndName)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(CallID, tableEndName);
        }
        #endregion

        #region �ӿڵ��ã�ֻ��ѯ�ֱ�
        /// ���򳵸���ҵ����飬�����ȡһ��ʱ�䷶Χ�ڵĻ����ܱ�����
        /// <summary>
        /// ���򳵸���ҵ����飬�����ȡһ��ʱ�䷶Χ�ڵĻ����ܱ�����
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
        /// ���򳵻�ȡInbound�����ܱ�����
        /// <summary>
        /// ���򳵻�ȡInbound�����ܱ�����
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
        /// ����ҵ����飬�����ȡһ��ʱ�䷶Χ�ڵĻ����ܱ�����
        /// <summary>
        /// ����ҵ����飬�����ȡһ��ʱ�䷶Χ�ڵĻ����ܱ�����
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
        /// ��ȡ�������ݽӿڣ��׼��͵��ã�
        /// <summary>
        /// ��ȡ�������ݽӿڣ��׼��͵��ã�
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
        /// ��ţ�ӿڵ��� ����
        /// <summary>
        /// ��ţ�ӿڵ��� ����
        /// </summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGBySessionID(string SessionID)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGBySessionID(SessionID);
        }
        /// ��ѯ��ǰ���е�ʵ��
        /// <summary>
        /// ��ѯ��ǰ���е�ʵ��
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGByCallID(long CallID)
        {
            //��ѯ���ڱ�
            return GetCallRecord_ORIGByCallID(CallID, "");
        }
        /// IM���ݿͻ��Ż�ȡҵ���¼
        /// <summary>
        /// IM���ݿͻ��Ż�ȡҵ���¼
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

                dt.Columns.Add("AssignUserID", typeof(string));//�ύ��
                dt.Columns.Add("Status", typeof(string));//״̬
                dt.Columns.Add("TaskID", typeof(string));//����ID              
                dt.Columns.Add("TaskUrl", typeof(string));//�鿴ҳ��ַ
                dt.Columns.Add("Content", typeof(string));//��ϵ��¼
                dt.Columns.Add("RecordType", typeof(string));//��¼����
                dt.Columns.Add("LastOperTime", typeof(DateTime));//�ύʱ��
                string url_head = ConfigurationUtil.GetAppSettingValue("IMTaskMiddleDomain") + "/IMTaskMiddle.aspx?" + "UserID=" + System.Web.HttpUtility.UrlEncode(userid.ToString());

                foreach (DataRow row in mydt.Rows)
                {
                    string url = "";
                    //����״̬
                    string taskstatus = CommonFunction.ObjectToString(row["TaskStatus"]);
                    string stopstatus = CommonFunction.ObjectToString(row["StopStatus"]);
                    //����
                    string bussinessType = CommonFunction.ObjectToString(row["BusinessType"]);
                    //�ͻ���ʵ����
                    string applytype = CommonFunction.ObjectToString(row["ApplyType"]);
                    //����ID
                    string taskid = CommonFunction.ObjectToString(row["TaskID"]);
                    //��Դ
                    string tasksource = CommonFunction.ObjectToString(row["TaskSource"]);
                    //������
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
                BLL.Loger.Log4Net.Info("[CustHistoryInf_BLL]GetCustHistoryInfoForWork_IM��������!errorMessage:" + ex.Message);
                BLL.Loger.Log4Net.Info("[CustHistoryInf_BLL]GetCustHistoryInfoForWork_IM��������!errorStackTrace:" + ex.StackTrace);
            }
            return dt;
        }
        /// ҵ���¼
        /// <summary>
        /// ҵ���¼
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
        /// �����¼
        /// <summary>
        /// �����¼
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
        /// ��������ID����ȡ������ID֮����������ݣ�����ȡʮ����
        /// <summary>
        /// ��������ID����ȡ������ID֮����������ݣ�����ȡʮ����
        /// </summary>
        /// <param name="maxID">�������ID</param>
        /// <returns>����DataTable</returns>
        public DataTable GetCallRecord_ORIGByMaxID(int maxID)
        {
            return Dal.CallRecord_ORIG.Instance.GetCallRecord_ORIGByMaxID(maxID);
        }
        ///�����ֻ����룬��ѯ��CCϵͳ�У����һ������ģ�InitiatedTime����ʼ��ʱ��
        /// <summary>
        ///�����ֻ����룬��ѯ��CCϵͳ�У����һ������ģ�InitiatedTime����ʼ��ʱ��
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetPhoneLastestInitiatedTime(string phoneNumber)
        {
            return Dal.CallRecord_ORIG.Instance.GetPhoneLastestInitiatedTime(phoneNumber);
        }
        #endregion

        /// ȡ����״̬
        /// <summary>
        /// ȡ����״̬
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

        #region ͬ������
        /// ��ֻ������ݣ�14�����ݷŵ�old����
        /// <summary>
        /// ��ֻ������ݣ�14�����ݷŵ�old����
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> SplitCallDataForOld(Action<string> logFunc)
        {
            return Dal.CallRecord_ORIG.Instance.SplitCallDataForOld(logFunc);
        }
        /// ��ֻ�����صı�
        /// <summary>
        /// ��ֻ�����صı�
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Dictionary<string, int> SplitCallDataForService(DateTime date, Action<string> logFunc)
        {
            return Dal.CallRecord_ORIG.Instance.SplitCallDataForService(date, logFunc);
        }

        /// ���²�ѯ��������
        /// <summary>
        /// ���²�ѯ��������
        /// </summary>
        /// <param name="endtablename"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void GetAllCallRecordORIGForHB(DateTime month, out int count, Action<string> callback)
        {
            string endtablename = BLL.Util.CalcTableNameByMonth(3, month);
            Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB(endtablename, month, out count, callback, new Func<string, string>(Util.HaoMaProcess));
        }
        /// ��ʱ��β�ѯ���ڻ�������
        /// <summary>
        /// ��ʱ��β�ѯ���ڻ�������
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public void GetAllCallRecordORIGForHB(DateTime st, DateTime et, out int count, Action<string> callback)
        {
            Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB(st, et, out count, callback, new Func<string, string>(Util.HaoMaProcess));
        }
        /// ��ѯOld��ȡȫ��������
        /// <summary>
        /// ��ѯOld��ȡȫ��������
        /// </summary>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public void GetAllCallRecordORIGForHB(out int count, Action<string> callback)
        {
            string endtablename = "_old";
            Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB(endtablename, out count, callback, new Func<string, string>(Util.HaoMaProcess));
        }
        /// ��ȡ��ѯ��
        /// <summary>
        /// ��ȡ��ѯ��
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCallRecordORIGForHB_SelectCol()
        {
            return Dal.CallRecord_ORIG.Instance.GetAllCallRecordORIGForHB_SelectCol();
        }

        /// ��ȡ�����ܱ�������������
        /// <summary>
        /// ��ȡ�����ܱ�������������
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

