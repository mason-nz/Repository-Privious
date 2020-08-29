using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.Utils.Config;
using XYAuto.Utils;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace XYAuto.BUOC.ChiTuData2017.Dal
{
    public class DataBase
    {
        protected static string CONNECTIONSTRINGS = GetConnectionStrings("ConnectionStrings_DataSystem2017");

        protected static string SqlFilter(string str)
        {
            if (str != null)
            {
                return StringHelper.SqlFilter(str);
            }
            else
            {
                return null;
            }
        }

        private static string GetConnectionStrings(string key)
        {
            try
            {
                if (ConfigurationManager.AppSettings[key] != null)
                    return ConfigurationManager.AppSettings[key];
                else return "";
            }
            catch
            {
                return "";
            }
        }

        protected static string ConnectBaseData2017
        {
            get { return GetConnectionStrings("ConnectionStrings_BaseData2017"); }
        }

        protected static string ConnectChitunion2017
        {
            get { return GetConnectionStrings("ConnectionStrings_ITSC"); }
        }

        protected static string ConnectChitunionOp2017
        {
            get { return GetConnectionStrings("ConnectionStrings_Chitunion_OP2017"); }
        }

        protected static string Chitunion_DataSystem2017
        {
            get { return GetConnectionStrings("ConnectionStrings_DataSystem2017"); }
        }

        /// <summary>
        /// DataTable转list，auth：lixiong
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        protected List<T> DataTableToList<T>(DataTable table) //where T : EntityBase, new()
        {
            List<T> list = new List<T>();
            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                for (var i = 0; i < table.Rows.Count; i++)
                {
                    //创建泛型对象
                    T entity = Activator.CreateInstance<T>();
                    //属性和名称相同时则赋值
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        var property = entity.GetType().GetProperty(table.Columns[j].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (property != null && table.Rows[i][j] != DBNull.Value)
                        {
                            property.SetValue(entity, table.Rows[i][j], null);
                        }
                    }
                    list.Add(entity);
                }
            }
            return list;
        }

        /// <summary>
        /// DataTable转实体，auth：lixiong
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        protected T DataTableToEntity<T>(DataTable table) //where T : EntityBase, new()
        {
            var entity = Activator.CreateInstance<T>();
            if (table.Rows.Count == 0)
                return default(T);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                //var property = entity.GetType().GetProperty(table.Columns[i].ColumnName);
                var property = entity.GetType().GetProperty(table.Columns[i].ColumnName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

                if (property != null && table.Rows[0][i] != DBNull.Value)
                {
                    property.SetValue(entity, table.Rows[0][i], null);
                }
            }
            return entity;
        }

        /// <summary>
        /// SqlBulkCopy批量插入数据
        /// </summary>
        /// <param name="connectionStr">链接字符串</param>
        /// <param name="dataTableName">表名</param>
        /// <param name="sourceDataTable">数据源</param>
        /// <param name="batchSize">一次事务插入的行数</param>
        public void SqlBulkCopyByDataTable(string connectionStr, string dataTableName, DataTable sourceDataTable, int batchSize = 100000)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionStr, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlBulkCopy.DestinationTableName = dataTableName;
                        sqlBulkCopy.BatchSize = batchSize;
                        for (int i = 0; i < sourceDataTable.Columns.Count; i++)
                        {
                            sqlBulkCopy.ColumnMappings.Add(sourceDataTable.Columns[i].ColumnName, sourceDataTable.Columns[i].ColumnName);
                        }
                        sqlBulkCopy.WriteToServer(sourceDataTable);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connectionString">目标连接字符</param>
        /// <param name="TableName">目标表</param>
        /// <param name="dt">源数据</param>
        public void SqlBulkCopyByDatatable(string connectionString, string TableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void SqlBulkCopyByDataTable(SqlConnection conn, SqlTransaction trans, string dataTableName, DataTable sourceDataTable, int batchSize = 10000)
        {
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans))
            {
                sqlBulkCopy.DestinationTableName = dataTableName;
                sqlBulkCopy.BatchSize = batchSize;
                for (int i = 0; i < sourceDataTable.Columns.Count; i++)
                {
                    sqlBulkCopy.ColumnMappings.Add(sourceDataTable.Columns[i].ColumnName, sourceDataTable.Columns[i].ColumnName);
                }
                sqlBulkCopy.WriteToServer(sourceDataTable);
            }
        }
    }
}