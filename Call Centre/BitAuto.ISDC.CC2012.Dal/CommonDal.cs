using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CommonDal : DataBase
    {
        private CommonDal() { }
        private static CommonDal instance = null;
        public static CommonDal Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommonDal();
                }
                return instance;
            }
        }

        public static string Default_Conn
        {
            get
            {
                return CONNECTIONSTRINGS;
            }
        }
        public static string CRM_Conn
        {
            get
            {
                return ConnectionStrings_CRM;
            }
        }
        public static string SYS_Conn
        {
            get
            {
                return ConnectionStrings_SYS;
            }
        }

        #region 通用实体类增删改查DAL（适用[ADO自动生成工具]生成的实体类）
        /// 新增 (自动生成)
        /// <summary>
        /// 新增 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InsertComAdoInfo<T>(T info)
        {
            return InsertComAdoInfo(info, Default_Conn);
        }
        /// 修改 (自动生成)
        /// <summary>
        /// 修改 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateComAdoInfo<T>(T info)
        {
            return UpdateComAdoInfo(info, Default_Conn);
        }
        /// 删除 (自动生成)
        /// <summary>
        /// 删除 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool DeleteComAdoInfo<T>(T info)
        {
            return DeleteComAdoInfo(info, Default_Conn);
        }

        /// 新增 (自动生成)
        /// <summary>
        /// 新增 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InsertComAdoInfo<T>(T info, SqlTransaction sqltran)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetInsertSqlParameter(info, out sql);

            if (parameters == null || string.IsNullOrEmpty(sql))
                return false;
            int num = SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sql, parameters);
            object o = SqlHelper.ExecuteScalar(sqltran, CommandType.Text, "SELECT @@IDENTITY");
            if (o != null)
            {
                AttributeHelper.SetIDENTITY(info, o.ToString());
            }
            return num == 1;
        }
        /// 修改 (自动生成)
        /// <summary>
        /// 修改 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateComAdoInfo<T>(T info, SqlTransaction sqltran)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetUpdateSqlParameter(info, out sql);
            if (parameters == null || string.IsNullOrEmpty(sql))
                return false;
            int num = SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sql, parameters);
            return num == 1;
        }
        /// 删除 (自动生成)
        /// <summary>
        /// 删除 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool DeleteComAdoInfo<T>(T info, SqlTransaction sqltran)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetDeleteSqlParameter(info, out sql);
            if (parameters == null || string.IsNullOrEmpty(sql))
                return false;
            int num = SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sql, parameters);
            return num == 1;
        }

        /// 新增 (自动生成)
        /// <summary>
        /// 新增 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool InsertComAdoInfo<T>(T info, string connstr)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetInsertSqlParameter(info, out sql);

            if (parameters == null || string.IsNullOrEmpty(sql))
                return false;
            int num = 0;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                num = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parameters);
                object o = SqlHelper.ExecuteScalar(conn, CommandType.Text, "SELECT @@IDENTITY");
                if (o != null)
                {
                    AttributeHelper.SetIDENTITY(info, o.ToString());
                }
            }
            return num == 1;
        }
        /// 修改 (自动生成)
        /// <summary>
        /// 修改 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateComAdoInfo<T>(T info, string connstr)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetUpdateSqlParameter(info, out sql);
            if (parameters == null || string.IsNullOrEmpty(sql))
                return false;
            int num = SqlHelper.ExecuteNonQuery(connstr, CommandType.Text, sql, parameters);
            return num == 1;
        }
        /// 删除 (自动生成)
        /// <summary>
        /// 删除 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool DeleteComAdoInfo<T>(T info, string connstr)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetDeleteSqlParameter(info, out sql);
            if (parameters == null || string.IsNullOrEmpty(sql))
                return false;
            int num = SqlHelper.ExecuteNonQuery(connstr, CommandType.Text, sql, parameters);
            return num == 1;
        }

        /// 查询 (自动生成)
        /// <summary>
        /// 查询 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public T GetComAdoInfo<T>(T info)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetSelectSqlParameter(info, out sql);
            if (parameters == null || string.IsNullOrEmpty(sql))
                return default(T);
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            if (dt != null && dt.Rows.Count != 0)
            {
                return (T)Activator.CreateInstance(typeof(T), new object[] { dt.Rows[0] });
            }
            else return default(T);
        }
        /// 查询 (自动生成)
        /// <summary>
        /// 查询 (自动生成)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">主键序列值（和实体类中构造方法中的主键参数顺序一致）</param>
        /// <returns></returns>
        public DataTable GetComAdoData<T>(T info)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            parameters = AttributeHelper.GetSelectSqlParameter(info, out sql);
            if (parameters == null || string.IsNullOrEmpty(sql))
                return new DataTable();
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            return dt;
        }

        /// 自定义查询 (自动生成)
        /// <summary>
        /// 自定义查询 (自动生成)
        /// </summary>
        public DataTable GetComAdoInfo(string tablename, string where)
        {
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "select * from " + tablename + " where " + where).Tables[0];
        }
        /// 自定义删除 (自动生成)
        /// <summary>
        /// 自定义删除 (自动生成)
        /// </summary>
        public bool DeleteComAdoInfo(string tablename, string where)
        {
            int num = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, "delete from " + tablename + " where " + where);
            return num == 1;
        }
        #endregion

        /// 公共分页查询方法
        /// <summary>
        /// 公共分页查询方法
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCommonPageData(string conn, string sql, string order, int currentPage, int pageSize, out int totalCount)
        {
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sql;
            parameters[1].Value = order;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 公共分页查询方法
        /// <summary>
        /// 公共分页查询方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCommonPageData(string sql, string order, int currentPage, int pageSize, out int totalCount)
        {
            return GetCommonPageData(CONNECTIONSTRINGS, sql, order, currentPage, pageSize, out  totalCount);
        }

        /// <summary>
        /// 批量插入DB数据方法
        /// </summary>
        /// <param name="dt">Datatable</param>
        /// <param name="conn">链接字符串</param>
        /// <param name="tableName">目标表名</param>
        /// <param name="batchSize">批次大小</param>
        /// <param name="list">映射列表</param>
        public void BulkCopyToDB(DataTable dt, string conn, string tableName, int batchSize, IList<SqlBulkCopyColumnMapping> list)
        {
            if (dt.Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default,
                                   transaction))
                        {
                            bulkCopy.BulkCopyTimeout = 1800;
                            if (list != null && list.Count > 0)
                            {
                                foreach (SqlBulkCopyColumnMapping sc in list)
                                {
                                    bulkCopy.ColumnMappings.Add(sc);
                                }
                            }
                            bulkCopy.BatchSize = batchSize;
                            bulkCopy.DestinationTableName = tableName;

                            try
                            {
                                bulkCopy.WriteToServer(dt);
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                            }
                            finally
                            {
                                bulkCopy.Close();
                            }
                        }
                    }
                }
            }
        }

        /// 校验表是否存在
        /// <summary>
        /// 校验表是否存在
        /// </summary>
        /// <param name="tablenames"></param>
        /// <returns></returns>
        public bool CheckTableExists(params string[] tablenames)
        {
            if (tablenames == null && tablenames.Length == 0)
            {
                return true;
            }

            string where = "";
            foreach (string key in tablenames)
            {
                where += "OBJECT_ID(N'[" + key + "]'),";
            }
            where = where.TrimEnd(',');

            string sql = @"SELECT  COUNT(*)
                                    FROM    dbo.SysObjects
                                    WHERE   ID IN ( " + where + @" )
                                            AND OBJECTPROPERTY(ID, 'IsTable') = 1";
            int count = (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return count == tablenames.Length;
        }
    }
}
