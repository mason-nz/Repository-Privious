using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.Utils.Config;
using System.Threading;
using System.IO;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Data;
using System.Reflection;
using System.Web.ModelBinding;

namespace XYAuto.ITSC.Chitunion2017.Common
{
    public class Util
    {

        //protected static string CONNECTIONSTRINGS = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_ITSC");
        private static Mutex m_mutex = new Mutex();
        //创建记录日志生成状态的日志
        public static void Log(string pathName, string status, string msg)
        {
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + pathName;
            m_mutex.WaitOne();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string logfile = path + "\\" + System.DateTime.Today.ToString("yyyy-MM-dd") + ".log";
            System.IO.StreamWriter sw = File.AppendText(logfile);
            sw.WriteLine("{0:-20}\t{1:10}\t{2}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), status, msg);
            sw.Close();
            m_mutex.ReleaseMutex();
        }

        /// <summary>
        /// 获取ModelState验证的错误信息
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>

        public static string GetErrorMsg(ModelStateDictionary dict)
        {

            StringBuilder sb = new StringBuilder();
            if (dict.IsValid)
                return string.Empty;
            else
            {
                foreach (var value in dict.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        sb.Append(error.ErrorMessage + "\r\n");
                    }
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// DataTable转list，auth：lixiong
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
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
        public static string GetChar(Random rnd)
        {
            //            0 - 9
            // A - Z  ASCII值  65 - 90
            // a - z  ASCII值  97 - 122
            int i = rnd.Next(0, 123);
            if (i < 10)
            {
                //                返回数字
                return i.ToString();
            }
            char c = (char)i;
            //            返回小写字母加数字
            // return char.IsLower(c) ? c.ToString() : GetChar(rnd);

            //            返回大写字母加数字
            // return char.IsUpper(c) ? c.ToString() : GetChar(rnd);

            //            返回大小写字母加数字
            return char.IsLetter(c) ? c.ToString() : GetChar(rnd);
        }

        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(GetChar(r));
            }
            return result.ToString();
        }
    }
}
