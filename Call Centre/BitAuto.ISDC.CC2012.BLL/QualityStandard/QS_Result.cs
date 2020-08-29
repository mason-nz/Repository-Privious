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
    /// ҵ���߼���QS_Result ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:36 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_Result
    {
        public static readonly QS_Result Instance = new QS_Result();
        protected QS_Result()
        { }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.QS_Result GetQS_Result(int QS_RID)
        {
            return Dal.QS_Result.Instance.GetQS_Result(QS_RID);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.QS_Result model)
        {
            return Dal.QS_Result.Instance.Update(model);
        }
        /// <summary>
        /// �ϸ���-�����ʼ��׼��Ϣ
        /// </summary> 
        /// <param name="query">query</param>
        /// <returns>����</returns>
        public DataTable GetStandardByQualifiedType(QueryQS_Result query)
        {
            return Dal.QS_Result.Instance.GetStandardByQualifiedType(query);
        }
        public DataTable GetAnswerByQualifiedType_IM(string where)
        {
            return Dal.QS_Result.Instance.GetAnswerByQualifiedType_IM(where);
        }
        public bool HasScored(string CallID)
        {
            return Dal.QS_Result.Instance.HasScored(CallID);
        }

        #region ���²�ѯ
        /// ����һ������
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.QS_Result model, string tableEndName)
        {
            //У��
            var old = Dal.QS_Result.Instance.GetQS_Result(model.CallReCordID);
            if (old != null)
            {
                // "�˻��������ֹ����ˣ������ٴ����֣�";
                return -9999;
            }
            else
            {
                return Dal.QS_Result.Instance.Insert(model, tableEndName);
            }
        }

        /// ��ѯ¼���ʼ�����
        /// <summary>
        /// ��ѯ¼���ʼ�����
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetQS_ResultList(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.QS_Result.Instance.GetQS_ResultList(query, order, currentPage, pageSize, tableEndName, out totalCount, BLL.Loger.Log4Net);
        }
        /// �ɼ�ͳ�Ʋ�ѯ
        /// <summary>
        /// �ɼ�ͳ�Ʋ�ѯ
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultScoreStat(QueryQS_ScoreStat query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            string where = GetQS_ResultScoreStat_Where(query);
            return Dal.QS_Result.Instance.GetQS_ResultScoreStat(where, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// ������ͳ�Ʋ�ѯ
        /// <summary>
        /// ������ͳ�Ʋ�ѯ
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultWorkloadStat(string where, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.QS_Result.Instance.GetQS_ResultWorkloadStat(where, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// �ϸ���-��������Ϣ
        /// <summary>
        /// �ϸ���-��������Ϣ
        /// </summary>
        /// <param name="query">��ѯ����</param> 
        /// <returns></returns>
        public DataTable GetAnswerByQualifiedType(QueryQS_ResultDetail query, string tableEndName)
        {
            return Dal.QS_Result.Instance.GetAnswerByQualifiedType(query, tableEndName);
        }
        /// �ϸ���-����������Ϣ���ʼ��׼��Ϣ
        /// <summary>
        /// �ϸ���-����������Ϣ���ʼ��׼��Ϣ
        /// </summary>
        /// <param name="query">��ѯ����</param> 
        /// <returns> </returns>
        public DataTable GetBaseInfoByQualifiedType(QueryCallRecordInfo query, string tableEndName)
        {
            return Dal.QS_Result.Instance.GetBaseInfoByQualifiedType(query, tableEndName);
        }
        /// ������-�ɼ�������
        /// <summary>
        /// ������-�ɼ�������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable getResultByExport(Entities.QueryCallRecordInfo query, string tableEndName)
        {
            return Dal.QS_Result.Instance.getResultByExport(query, tableEndName);
        }
        /// ���߽��ͳ��
        /// <summary>
        /// ���߽��ͳ��
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultAppealStat(string where, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.QS_Result.Instance.GetQS_ResultAppealStat(where, order, currentPage, pageSize, tableEndName, out totalCount);
        }
        /// <summary>
        ///   ���ųɼ���ϸ��������
        /// </summary>
        /// <param name="whereBGID"></param>
        /// <param name="whereTime1"></param>
        /// <param name="whereTime2"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchTable"></param>
        /// <param name="totalCount"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultGradeStatisticsHotLine(string whereBGID, string whereTime1, string whereTime2, int currentPage, int pageSize, string searchTable, out int totalCount,SqlConnection conn)
        {
            return Dal.QS_Result.Instance.GetQS_ResultGradeStatisticsHotLine(whereBGID, whereTime1, whereTime2, currentPage, pageSize, searchTable, out totalCount, conn);
        }
           /// <summary>
        /// ���ųɼ�������ϸ
       /// </summary>
       /// <param name="whereOut"></param>
       /// <param name="order"></param>
       /// <param name="currentPage"></param>
       /// <param name="pageSize"></param>
       /// <param name="tableEndName"></param>
       /// <param name="totalCount"></param>
       /// <param name="userid"></param>
       /// <returns></returns>
        public DataTable GetQS_ResultGradeStatisticsOnLine(string whereOut,  string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.QS_Result.Instance.GetQS_ResultGradeStatisticsOnLine(whereOut, order, currentPage, pageSize, tableEndName, out totalCount, userid);
        }
        /// ���Ƶ��ͳ�� 
        /// <summary>
        /// ���Ƶ��ͳ�� 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetQS_ResultFrequenyStatistics(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            return Dal.QS_Result.Instance.GetQS_ResultFrequenyStatistics(query, order, currentPage, pageSize, tableEndName, out totalCount);
        }

        private string GetQS_ResultScoreStat_Where(QueryQS_ScoreStat query)
        {
            string where = "";
            int userId = 0;
            if (int.TryParse(query.RequestUserID, out userId) && userId > 0)
            {
                where += " And cri.CreateUserID=" + userId;
            }
            int groupId = 0;
            if (int.TryParse(query.RequestGroupID, out groupId) && groupId > 0)
            {
                where += " AND cri.BGID=" + groupId;
            }
           
            DateTime recordBeginTime;
            DateTime recordEndTime;
            if (DateTime.TryParse(query.RequestRecordBeginTime, out recordBeginTime))
            {
                where += " And cri.CreateTime>='" + recordBeginTime.ToString() + "'";
            }
            if (DateTime.TryParse(query.RequestRecordEndTime, out recordEndTime))
            {
                where += " And cri.CreateTime<='" + recordEndTime.Add(new TimeSpan(23, 59, 59)).ToString() + "'";
            }

            // ����Ȩ���ж�
            int loginUserId = BLL.Util.GetLoginUserID();
            string whereDataRight = "";
            whereDataRight = BLL.UserGroupDataRigth.Instance.GetSqlRightstr("cri", "BGID", "CreateUserID", loginUserId);
            where += whereDataRight;
            return where;
        }
        #endregion

        /// <summary>
        /// �������ֱ�������ȡ���ֱ�����
        /// </summary>
        /// <param name="qs_Rid"></param>
        /// <returns></returns>
        public string GetScoreTypeByRID(string qs_Rid)
        {
            return Dal.QS_Result.Instance.GetScoreTypeByRID(qs_Rid);
        }

        /// <summary>
        /// ����RTID��CallId��ȡRTID
        /// </summary>
        /// <param name="qsRtid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetRidByCallidAndRtid(int qsRtid, Int64 callid)
        {
            return Dal.QS_Result.Instance.GetRidByCallidAndRtid(qsRtid, callid);
        }
    }
}

