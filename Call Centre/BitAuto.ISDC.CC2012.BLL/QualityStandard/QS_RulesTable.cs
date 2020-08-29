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
    /// ҵ���߼���QS_RulesTable ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:37 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_RulesTable
    {
        #region Instance
        public static readonly QS_RulesTable Instance = new QS_RulesTable();
        #endregion

        #region Contructor
        protected QS_RulesTable()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetQS_RulesTable(QueryQS_RulesTable query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.QS_RulesTable.Instance.GetQS_RulesTable(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.QS_RulesTable.Instance.GetQS_RulesTable(new QueryQS_RulesTable(), string.Empty, 1, -1, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.QS_RulesTable GetQS_RulesTable(int QS_RTID)
        {

            return Dal.QS_RulesTable.Instance.GetQS_RulesTable(QS_RTID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByQS_RTID(int QS_RTID)
        {
            QueryQS_RulesTable query = new QueryQS_RulesTable();
            query.QS_RTID = QS_RTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_RulesTable(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_RulesTable model)
        {
            return Dal.QS_RulesTable.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int QS_RTID)
        {

            return Dal.QS_RulesTable.Instance.Delete(QS_RTID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_RTID)
        {

            return Dal.QS_RulesTable.Instance.Delete(sqltran, QS_RTID);
        }

        #endregion

        /// <summary>
        /// �������ֱ�IDȡ���ֱ���ϸ������Dataset�������DataTable �ֱ������ַ��࣬�ʼ���Ŀ���ʼ��׼���۷��������
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesTableDetailByQS_RTID(int QS_RTID)
        {
            return Dal.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(QS_RTID);
        }

        /// <summary>
        /// �������ֱ�ID,�ʼ�ɼ�IDȡ���ֱ���ϸ������Dataset�������DataTable �ֱ������ַ��࣬�ʼ���Ŀ���ʼ��׼���۷��������
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesTableDetailByQS_RTID(int QS_RTID, int QS_RID)
        {
            return Dal.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(QS_RTID, QS_RID);
        }

         /// <summary>
        /// ����RTID��ȡ���ֱ�����
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        public string GetScoreTypeByRTID(int rtid)
        {
            return Dal.QS_RulesTable.Instance.GetScoreTypeByRTID(rtid);
        }

        /// <summary>
        /// �ж�ָ�����ֱ�������Ƿ��Ѿ����ڣ�ָ�����ֱ����ͣ�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scoreType"></param>
        /// <returns></returns>
        public bool IsRuleTableNameExist(string name, int scoreType)
        {
            return Dal.QS_RulesTable.Instance.IsRuleTableNameExist(name, scoreType);
        }
    }
}

