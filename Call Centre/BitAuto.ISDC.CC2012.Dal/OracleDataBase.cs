using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Dal
{
    /// <summary>
    /// Oracle 访问
    /// qiangfei
    /// 2014-7-9
    /// </summary>
    public class OracleDataBase : IDisposable
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        private DbConnection connection;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return this.connection;
            }
        }

        /// 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="conString"></param>
        public OracleDataBase()
        {
            try
            {
                string conString = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC_Oracle");
                this.connection = new OracleConnection(conString);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// 释放连接
        /// <summary>
        /// 释放连接
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.IsOpen())
            {
                this.connection.Close();
            }
            this.connection.Dispose();
        }

        #region 辅助
        /// 连接是否打开
        /// <summary>
        /// 连接是否打开
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return (((OracleConnection)connection).State == ConnectionState.Open);
        }
        /// 创建命令
        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommand CreateSqlCommand(string sql, OracleParameter[] paras)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException("sql语句不能为空", "sql");
            DbCommand command = connection.CreateCommand();
            if (!this.IsOpen())
                connection.Open();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            if (paras != null)
            {
                command.Parameters.AddRange(paras);
            }
            return command;
        }
        /// 创建适配器
        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            return new OracleDataAdapter((OracleCommand)cmd);
        }
        /// 返回DataReader
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            DbDataReader reader;
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return reader;
        }
        #endregion

        #region 读取数据
        /// 查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql)
        {
            return ExecuteDataTable(sql, null);
        }
        /// 查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, OracleParameter[] paras)
        {
            if (string.IsNullOrEmpty(sql))
                return new DataTable();
            DataSet ds = new DataSet();
            try
            {
                DbCommand command = CreateSqlCommand(sql, paras);
                using (DbDataAdapter adapter = CreateDataAdapter(command))
                {
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else return new DataTable();
        }
        /// 分页查询数据
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns>自动增加row_id列在第一列位置</returns>
        public DataTable ExecuteDataTable(string sql, int pageIndex, int pageSize, out int total)
        {
            return ExecuteDataTable(sql, null, pageIndex, pageSize, out total);
        }
        /// 分页查询数据
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns>自动增加row_id列在第一列位置</returns>
        public DataTable ExecuteDataTable(string sql, OracleParameter[] paras, int pageIndex, int pageSize, out int total)
        {
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            string sqltotal = "select count(*) from (" + sql + ") tmp";
            string sqlpage = "select * from (select rownum as row_id,tmp1.* from (" + sql + ") tmp1) tmp2 where row_id>=" + start + " and row_id<=" + end;
            DataSet ds = new DataSet();
            total = 0;
            try
            {
                //查询总数
                DbCommand cmdtotal = CreateSqlCommand(sqltotal, paras);
                using (DbDataReader reader = ExecuteReader(cmdtotal))
                {
                    while (reader.Read())
                    {
                        total = reader.GetInt32(0);
                    }
                }
                //查询分页
                DbCommand cmdpage = CreateSqlCommand(sqlpage, paras);
                using (DbDataAdapter adapter = CreateDataAdapter(cmdpage))
                {
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else return new DataTable();
        }
        /// 查询总数
        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExecuteDataTableCount(string sql, OracleParameter[] paras)
        {
            int total = 0;
            string sqltotal = "select count(*) from (" + sql + ") tmp";
            try
            {
                //查询总数
                DbCommand cmdtotal = CreateSqlCommand(sqltotal, paras);
                using (DbDataReader reader = ExecuteReader(cmdtotal))
                {
                    while (reader.Read())
                    {
                        total = reader.GetInt32(0);
                    }
                }
                return total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
