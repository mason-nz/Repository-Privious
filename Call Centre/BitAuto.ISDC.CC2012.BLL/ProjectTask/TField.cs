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
    /// 业务逻辑类TField 的摘要说明。
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
    public class TField
    {
        #region Instance
        public static readonly TField Instance = new TField();
        #endregion

        #region Contructor
        protected TField()
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
        public DataTable GetTField(QueryTField query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.TField.Instance.GetTField(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.TField.Instance.GetTField(new QueryTField(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TField GetTField(int RecID)
        {

            return Dal.TField.Instance.GetTField(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryTField query = new QueryTField();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTField(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.TField model)
        {
            return Dal.TField.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TField model)
        {
            return Dal.TField.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.TField model)
        {
            return Dal.TField.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TField model)
        {
            return Dal.TField.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.TField.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.TField.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public int GetMaxID()
        {
            return Dal.TField.Instance.GetMaxID();
        }
        public List<Entities.TField> GetTFieldListByTTCode(string ttcode)
        {
            return Dal.TField.Instance.GetTFieldListByTTCode(ttcode);
        }
        public DataTable GetTFieldTableByTTCode(string ttcode)
        {
            return Dal.TField.Instance.GetTFieldTableByTTCode(ttcode);
        }
        /// <summary>
        /// 根据ttCode得到表名，再找到该表已“Tempf”开头的列名
        /// </summary>
        /// <param name="tableName">ttCode</param>
        /// <returns></returns>
        public DataTable GetTemptColumnNameByTableName(string ttCode)
        {
            return Dal.TField.Instance.GetTemptColumnNameByTableName(ttCode);
        }
        /// <summary>
        /// 根据TFName获取字段名和是否导出的字段
        /// </summary>
        /// <param name="TFName"></param>
        /// <returns></returns>
        public DataTable GetTFieldTableByTFName(string TFName)
        {
            return Dal.TField.Instance.GetTFieldTableByTFName(TFName);
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public int AddColumn(string tableName, string fieldName, string typeID)
        {
            return Dal.TField.Instance.AddColumn(tableName, fieldName, typeID);
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int DelColumn(string tableName, string fieldName)
        {
            return Dal.TField.Instance.DelColumn(tableName, fieldName);
        }
    }
}

