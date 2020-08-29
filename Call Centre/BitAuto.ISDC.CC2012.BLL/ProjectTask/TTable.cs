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
    /// ҵ���߼���TTable ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-20 03:24:42 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TTable
    {
        #region Instance
        public static readonly TTable Instance = new TTable();
        #endregion

        #region Contructor
        protected TTable()
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
        public DataTable GetTTable(QueryTTable query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.TTable.Instance.GetTTable(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.TTable.Instance.GetTTable(new QueryTTable(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.TTable GetTTable(int RecID)
        {

            return Dal.TTable.Instance.GetTTable(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryTTable query = new QueryTTable();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTTable(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.TTable model)
        {
            return Dal.TTable.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TTable model)
        {
            return Dal.TTable.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.TTable model)
        {
            return Dal.TTable.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TTable model)
        {
            return Dal.TTable.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.TTable.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.TTable.Instance.Delete(sqltran, RecID);
        }

        #endregion


        public int GetMaxID()
        {
            return Dal.TTable.Instance.GetMaxID();
        }

        public Entities.TTable GetTTableByTTCode(string ttcode)
        {
            Entities.TTable ttableModel = new Entities.TTable();
            Entities.QueryTTable query = new QueryTTable();
            query.TTCode = ttcode;
            int totalCount = 0;
            DataTable dt = BLL.TTable.Instance.GetTTable(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                ttableModel = Dal.TTable.Instance.GetTTable(int.Parse(dt.Rows[0]["RecID"].ToString()));
            }
            return ttableModel;
        }

        /// <summary>
        /// �����������ݱ�
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="msg"></param>
        public void CreateTable(string sqlStr, out string msg)
        {
            Dal.TTable.Instance.CreateTable(sqlStr, out msg);
        }

        /// <summary>
        /// ��ȡ���ɵ��Զ��������RecId
        /// </summary>
        /// <param name="TTName"></param>
        /// <returns></returns>
        public int GetMaxRecIdByTTName(string TTName)
        {
            return Dal.TTable.Instance.GetMaxRecIdByTTName(TTName);
        }
        /// <summary>
        /// ����TTCode�õ��Զ���ı��������Ϣ
        /// </summary>
        /// <param name="TTCode">TTCode</param>
        /// <returns></returns>
        public DataTable GetTemptInfoByTTCode(string TTCode)
        {
            return Dal.TTable.Instance.GetTemptInfoByTTCode(TTCode);
        }


        /// <summary>
        ///  ����IDs��ȡ�Զ�����е�
        /// </summary>
        /// <param name="selectDataIDs"></param>
        /// <param name="TTName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable GetDataByIDs(string selectDataIDs, string TTName, string ttcode, string projectid, out string msg)
        {
            return Dal.TTable.GetDataByIDs(selectDataIDs, TTName, ttcode,projectid,out  msg);
        }

        /// <summary>
        /// ����IDs��ȡ�Զ�����е����ݣ�������Ӵ���ʱ�䣬�ύʱ����� add by qizq 2014-11-25
        /// </summary>
        /// <param name="selectDataIDs"></param>
        /// <param name="TTName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable GetDataByIDs(string selectDataIDs, string TTName, string ttcode, string projectid, string taskcreatestart, string taskcreateend, string tasksubstart, string tasksubend, out string msg)
        {
            return Dal.TTable.GetDataByIDs(selectDataIDs, TTName, ttcode, projectid,taskcreatestart,taskcreateend,tasksubstart,tasksubend, out  msg);
        }

        public  DataTable GetDataByRelationIDs(string selectDataIDs, string TTName, out string msg)
        {
            return Dal.TTable.GetDataByRelationIDs(selectDataIDs, TTName, out  msg);
        }
    }
}

