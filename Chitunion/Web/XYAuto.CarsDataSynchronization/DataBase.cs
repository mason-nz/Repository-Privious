using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace XYAuto.CarsDataSynchronization
{
    public class DataBase
    {
        protected static string CONNECTIONSTRINGS = GetConnectionStrings("ConnectionStrings_ITSC");
        protected static string HostUrl = GetConnectionStrings("HostUrl");
       
        static string strHour = GetConnectionStrings("OperateHour");
        protected static int OperateHour = strHour == "" ? 23:Convert.ToInt16(strHour) ; 

        static string strMinute = GetConnectionStrings("OperateMinute");
        protected static int OperateMinute = strMinute == "" ? 0 : Convert.ToInt16(strMinute);
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
    }
}
