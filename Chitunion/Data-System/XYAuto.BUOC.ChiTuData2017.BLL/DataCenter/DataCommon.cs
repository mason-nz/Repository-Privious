using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter
{
    public class DataCommon
    {
        public static List<T> DataTableToList<T>(DataTable table) //where T : EntityBase, new()
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
        public static int GetAppSettingInt32Value(string key, bool throwException = false)
        {
            try
            {
                string appSettingValue = ConfigurationManager.AppSettings[key];

                return Convert.ToInt32(appSettingValue);
            }
            catch
            {
                if (throwException)
                {
                    throw new Exception("没有在配置文件里找到名为'" + key + "'的配置信息。");
                }
                else
                {
                    return 10000;
                }
            }
        }
    }
}
