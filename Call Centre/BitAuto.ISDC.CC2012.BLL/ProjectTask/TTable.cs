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
    /// 业务逻辑类TTable 的摘要说明。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetTTable(QueryTTable query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.TTable.Instance.GetTTable(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.TTable.Instance.GetTTable(new QueryTTable(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TTable GetTTable(int RecID)
        {

            return Dal.TTable.Instance.GetTTable(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
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
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.TTable model)
        {
            return Dal.TTable.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TTable model)
        {
            return Dal.TTable.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.TTable model)
        {
            return Dal.TTable.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TTable model)
        {
            return Dal.TTable.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.TTable.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
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
        /// 生成物理数据表
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="msg"></param>
        public void CreateTable(string sqlStr, out string msg)
        {
            Dal.TTable.Instance.CreateTable(sqlStr, out msg);
        }

        /// <summary>
        /// 获取生成的自定义表的最大RecId
        /// </summary>
        /// <param name="TTName"></param>
        /// <returns></returns>
        public int GetMaxRecIdByTTName(string TTName)
        {
            return Dal.TTable.Instance.GetMaxRecIdByTTName(TTName);
        }
        /// <summary>
        /// 根据TTCode得到自定义的表的数据信息
        /// </summary>
        /// <param name="TTCode">TTCode</param>
        /// <returns></returns>
        public DataTable GetTemptInfoByTTCode(string TTCode)
        {
            return Dal.TTable.Instance.GetTemptInfoByTTCode(TTCode);
        }


        /// <summary>
        ///  根据IDs获取自定义表中的
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
        /// 根据IDs获取自定义表中的数据，给任务加创建时间，提交时间过滤 add by qizq 2014-11-25
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

