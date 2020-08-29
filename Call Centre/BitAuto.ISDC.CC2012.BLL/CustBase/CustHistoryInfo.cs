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
    /// ҵ���߼���CustHistoryInfo ��ժҪ˵����
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

        #region ��ѯ-���ܷ���
        /// ��������ID��ѯ��ʷ��Ϣ-ת������
        /// <summary>
        /// ��������ID��ѯ��ʷ��Ϣ-ת������
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Entities.CustHistoryInfo GetCustHistoryInfo(string taskId)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(taskId);
        }

        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetCustHistoryInfo(QueryCustHistoryInfo query, string order, int currentPage, int pageSize, string fields, out int totalCount)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(query, order, currentPage, pageSize, fields, out totalCount);
        }
        public DataTable GetCustHistoryInfoForExport(QueryCustHistoryInfo query, string fields)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfoForExport(query, fields);
        }

        /// <summary>
        /// �õ�һ������ʵ��
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
        /// ɾ��һ������
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

        /// ����TaskID��custID��BusinessType������ϵ��¼
        /// <summary>
        /// ����TaskID��custID��BusinessType������ϵ��¼
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="businessType">ҵ�����ͣ�1������2�Ź�������3�ͻ���ʵ��4��������</param>
        /// <returns></returns>
        public Entities.CustHistoryInfo GetCustHistoryInfo(string taskID, string custID, int businessType)
        {
            return Dal.CustHistoryInfo.Instance.GetCustHistoryInfo(taskID, custID, businessType);
        }
        /// ��ѯ�ͻ����Ƿ�����ʷ��¼��ɾ�������û�
        /// <summary>
        /// ��ѯ�ͻ����Ƿ�����ʷ��¼��ɾ�������û�
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public bool HavCustHistoryInfoByCustID(string custID)
        {
            return Dal.CustHistoryInfo.Instance.HavCustHistoryInfoByCustID(custID);
        }

        /// ����CustHistoryInfo�� ����Ҷϣ��������ͻ���ʵ���������񣬽ӿڣ����ͣ�����
        /// <summary>
        /// ����CustHistoryInfo�� ����Ҷϣ��������ͻ���ʵ���������񣬽ӿڣ����ͣ�����
        /// </summary>
        /// <param name="custhistoryinfo">CustID,TaskID��CallRecordID��RecordType��CreateUserID</param>
        /// <param name="msg">���ص��ý����Ϣ</param>
        public void InsertOrUpdateCustHistoryInfo(Entities.CustHistoryInfo custhistoryinfo, out string msg)
        {
            msg = "'result':'false'";
            try
            {
                int bType = custhistoryinfo.BusinessType.HasValue ? custhistoryinfo.BusinessType.Value : -1;
                //У��
                if (string.IsNullOrEmpty(custhistoryinfo.TaskID) || string.IsNullOrEmpty(custhistoryinfo.CustID) || bType <= 0)
                {
                    Loger.Log4Net.Info("[ά��CustHistoryInfo] ��������ʧ�ܡ��ͻ��š�" + custhistoryinfo.CustID + "������ID��" + custhistoryinfo.TaskID + "������RecID��" + custhistoryinfo.CallRecordID + "�����͡�" + bType + "�������ˡ�" + custhistoryinfo.CreateUserID + "������ʱ�䡿" + DateTime.Now);
                    return;
                }
                string logDesc = string.Empty;

                //�жϸ�TaskID�Ƿ��й���¼���м�¼���£��޼�¼�Ͳ���һ��.
                Entities.CustHistoryInfo model = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(custhistoryinfo.TaskID, custhistoryinfo.CustID, bType);
                if (model == null)
                {
                    //model=null ��ʾ��������ϵ��¼��Ҫ����
                    model = custhistoryinfo;
                    BLL.CustHistoryInfo.Instance.Insert(model);
                    logDesc = "[����CustHistoryInfo] ������¼����TaskID��" + custhistoryinfo.TaskID + "��CallRecordID��" + custhistoryinfo.CallRecordID + "��CustID��" + custhistoryinfo.CustID + "��RecordType��" + bType + "��CreateTime��" + model.CreateTime + "��CreateUserID��" + custhistoryinfo.CreateUserID + "��RecordType��" + custhistoryinfo.RecordType;
                }
                else
                {
                    //���ڼ�¼�������
                    model.CallRecordID = custhistoryinfo.CallRecordID;
                    model.RecordType = custhistoryinfo.RecordType;
                    BLL.CustHistoryInfo.Instance.Update(model);
                    logDesc = "[�޸�CustHistoryInfo] ����¼�¼����RecID��" + model.RecID + "��CallRecordID��" + custhistoryinfo.CallRecordID + "��RecordType��" + custhistoryinfo.RecordType;
                }

                try
                {
                    Loger.Log4Net.Info(logDesc);
                    BLL.Util.InsertUserLogNoUser(logDesc);
                }
                catch (Exception ex)
                {
                    Loger.Log4Net.Error("[ά��CustHistoryInfo] ��־ʧ�ܣ�ԭ��", ex);
                }
                msg = "'result':'true'";
            }
            catch (Exception)
            {
                msg = "'result':'false'";
                Loger.Log4Net.Info("[ά��CustHistoryInfo] ʧ�ܡ��ͻ��š�" + custhistoryinfo.CustID + "������ID��" + custhistoryinfo.TaskID + "�������ˡ�" + custhistoryinfo.CreateUserID + "������ʱ�䡿" + DateTime.Now);
            }
        }

        /// ����һ������
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.CustHistoryInfo model)
        {
            return Dal.CustHistoryInfo.Instance.Insert(model);
        }
        /// ����һ������
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CustHistoryInfo model)
        {
            return Dal.CustHistoryInfo.Instance.Update(model);
        }
    }
}

