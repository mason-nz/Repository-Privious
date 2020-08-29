using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���SMSSendHistory ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-12-23 06:16:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SMSSendHistory
    {
        public static readonly SMSSendHistory Instance = new SMSSendHistory();

        protected SMSSendHistory()
        { }

        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetSMSSendHistory(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSSendHistory.Instance.GetSMSSendHistory(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetSMSHistroyStatistics(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSSendHistory.Instance.GetSMSHistroyStatistics(query, order, currentPage, pageSize, out totalCount);
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
        public DataTable GetSMSSendHistoryForExport(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SMSSendHistory.Instance.GetSMSSendHistoryForExport(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SMSSendHistory.Instance.GetSMSSendHistory(new QuerySMSSendHistory(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(Entities.SMSSendHistory model)
        {
            Dal.SMSSendHistory.Instance.Insert(model);
        }

        public int AddCustIdInfo(string SentSMSHistoryTel, string AddedCustID)
        {
            return Dal.SMSSendHistory.Instance.AddCustIdInfo(SentSMSHistoryTel, AddedCustID);
        }

        /// �󶨶��ż�¼������id
        /// <summary>
        /// �󶨶��ż�¼������id
        /// </summary>
        /// <param name="recids"></param>
        /// <param name="taskid"></param>
        public void BindSMSSendHistoryForTask(string recids, string taskid)
        {
            Dal.SMSSendHistory.Instance.BindSMSSendHistoryForTask(recids, taskid);
        }
    }
}

