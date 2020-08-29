/********************************************************
*创建人：lixiong
*创建时间：2017/12/5 14:49:48
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation
{
    public class ExportCsvHelper
    {
        #region Fields

        private string _fileName;
        private DataTable _dataSource;//数据源
        private string[] _titles = null;//列标题
        private string[] _fields = null;//字段名

        private List<string> listTitles = new List<string>();
        private List<string> listFields = new List<string>();
        private Dictionary<string, string> _keyValueTitles = new Dictionary<string, string>();

        #endregion Fields

        #region .ctor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">数据源</param>
        public ExportCsvHelper()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="titles">要输出到 Excel 的列标题的数组</param>
        /// <param name="fields">要输出到 Excel 的字段名称数组</param>
        /// <param name="dataSource">数据源</param>
        public ExportCsvHelper(string[] titles, string[] fields, DataTable dataSource)
         : this(titles, dataSource)
        {
            if (fields == null || fields.Length == 0)
                throw new ArgumentNullException("fields");
            if (titles.Length != fields.Length)
                throw new ArgumentException("titles.Length != fields.Length", "fields");
            _fields = fields;
        }

        public ExportCsvHelper(List<string> titles, List<string> fields, DataTable dataSource)
         : this(titles, dataSource)
        {
            if (fields == null || fields.Count == 0)
                throw new ArgumentNullException("fields");
            if (titles.Count != fields.Count)
                throw new ArgumentException("titles.Count != fields.Count", "fields");
            listFields = fields;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="titles">要输出到 Excel 的列标题的数组</param>
        /// <param name="dataSource">数据源</param>
        public ExportCsvHelper(string[] titles, DataTable dataSource)
         : this(dataSource)
        {
            if (titles == null || titles.Length == 0)
                throw new ArgumentNullException("titles");
            _titles = titles;
        }

        public ExportCsvHelper(List<string> titles, DataTable dataSource)
         : this(dataSource)
        {
            if (titles == null || titles.Count == 0)
                throw new ArgumentNullException("titles");
            listTitles = titles;
        }

        public ExportCsvHelper(Dictionary<string, string> keyValueTitles, DataTable dataSource)
         : this(dataSource)
        {
            if (keyValueTitles.Count == 0)
                throw new ArgumentNullException("titles");
            _keyValueTitles = keyValueTitles;
        }

        /// <_keyValueTitles>
        /// 构造函数
        /// </summary>
        /// <param name="dataSource">数据源</param>
        public ExportCsvHelper(DataTable dataSource)
        {
            if (dataSource == null)
                throw new ArgumentNullException("dataSource");
            // maybe more checks needed here (IEnumerable, IList, IListSource, ) ???
            // 很难判断，先简单的使用 DataTable
            _dataSource = dataSource;
        }

        #endregion .ctor

        #region public Methods

        #region 导出到CSV文件并且提示下载

        /// <summary>
        /// 导出到CSV文件并且提示下载
        /// </summary>
        /// <param name="fileName"></param>
        public byte[] DataToCSV(string fileName)
        {
            string data = listTitles.Count > 0 ? ExportCSVForActiveTitle() : ExportCSV();
            Console.WriteLine(data);

            return System.Text.Encoding.Default.GetBytes(data);
            /*HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.csv", System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)));
            HttpContext.Current.Response.ContentType = "text/h323;charset=gbk";
            HttpContext.Current.Response.Write(data);
            HttpContext.Current.Response.End();*/
        }

        public byte[] DataToCsvByDicTitle()
        {
            var data = ExportCsvForDicTitle();
            Console.WriteLine(data);
            return System.Text.Encoding.Default.GetBytes(data);
        }

        #endregion 导出到CSV文件并且提示下载

        /// <summary>
        /// 获取CSV导入的数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名称(.csv不用加)</param>
        /// <returns></returns>
        public DataTable GetCsvData(string filePath, string fileName)
        {
            string path = Path.Combine(filePath, fileName + ".csv");
            string connString = @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + filePath + ";Extensions=asc,csv,tab,txt;";
            try
            {
                using (OdbcConnection odbcConn = new OdbcConnection(connString))
                {
                    odbcConn.Open();
                    OdbcCommand oleComm = new OdbcCommand();
                    oleComm.Connection = odbcConn;
                    oleComm.CommandText = "select * from [" + fileName + "#csv]";
                    OdbcDataAdapter adapter = new OdbcDataAdapter(oleComm);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, fileName);
                    return ds.Tables[0];
                    odbcConn.Close();
                }
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                throw ex;
            }
        }

        #endregion public Methods

        #region 返回写入CSV的字符串

        /// <summary>
        /// 返回写入CSV的字符串
        /// </summary>
        /// <returns></returns>
        private string ExportCSV()
        {
            if (_dataSource == null)
                throw new ArgumentNullException("dataSource");
            StringBuilder strbData = new StringBuilder();
            if (_titles == null)
            {
                //添加列名
                foreach (DataColumn column in _dataSource.Columns)
                {
                    strbData.Append(column.ColumnName + ",");
                }

                strbData.Append("\n");
                foreach (DataRow dr in _dataSource.Rows)
                {
                    for (int i = 0; i < _dataSource.Columns.Count; i++)
                    {
                        strbData.Append(dr[i].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                    }
                    strbData.Append("\n");
                }
                return strbData.Replace(",\n", "\n").ToString();
            }
            else
            {
                foreach (string columnName in _titles)
                {
                    strbData.Append(columnName + ",");
                }
                strbData.Append("\n");
                if (_fields == null)
                {
                    foreach (DataRow dr in _dataSource.Rows)
                    {
                        for (int i = 0; i < _dataSource.Columns.Count; i++)
                        {
                            strbData.Append(dr[i].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                        }
                        strbData.Append("\n");
                    }
                    return strbData.Replace(",\n", "\n").ToString();
                }
                else
                {
                    foreach (DataRow dr in _dataSource.Rows)
                    {
                        for (int i = 0; i < _fields.Length; i++)
                        {
                            strbData.Append(dr[_fields[i]].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                        }
                        strbData.Append("\n");
                    }
                    return strbData.Replace(",\n", "\n").ToString();
                }
            }
        }

        private string ExportCsvForDicTitle()
        {
            if (_dataSource == null)
                throw new ArgumentNullException("dataSource");
            var strbData = new StringBuilder();
            foreach (DataColumn column in _dataSource.Columns)
            {
                var values = _keyValueTitles.FirstOrDefault(s => s.Key.Equals(column.ColumnName, StringComparison.OrdinalIgnoreCase));

                strbData.Append((values.Value ?? column.ColumnName) + ",");
            }

            strbData.Append("\n");

            foreach (DataRow dr in _dataSource.Rows)
            {
                for (var i = 0; i < _dataSource.Columns.Count; i++)
                {
                    strbData.Append(dr[i].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                }
                strbData.Append("\n");
            }
            return strbData.Replace(",\n", "\n").ToString();
        }

        private string ExportCSVForActiveTitle()
        {
            if (_dataSource == null)
                throw new ArgumentNullException("dataSource");
            StringBuilder strbData = new StringBuilder();
            if (listTitles.Count == 0)
            {
                //添加列名
                foreach (DataColumn column in _dataSource.Columns)
                {
                    strbData.Append(column.ColumnName + ",");
                }
                strbData.Append("\n");
                foreach (DataRow dr in _dataSource.Rows)
                {
                    for (int i = 0; i < _dataSource.Columns.Count; i++)
                    {
                        strbData.Append(dr[i].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                    }
                    strbData.Append("\n");
                }
                return strbData.Replace(",\n", "\n").ToString();
            }
            else
            {
                foreach (string columnName in listTitles)
                {
                    strbData.Append(columnName + ",");
                }
                strbData.Append("\n");
                if (listFields.Count == 0)
                {
                    foreach (DataRow dr in _dataSource.Rows)
                    {
                        for (int i = 0; i < _dataSource.Columns.Count; i++)
                        {
                            strbData.Append(dr[i].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                        }
                        strbData.Append("\n");
                    }
                    return strbData.Replace(",\n", "\n").ToString();
                }
                else
                {
                    foreach (DataRow dr in _dataSource.Rows)
                    {
                        for (int i = 0; i < listFields.Count; i++)
                        {
                            strbData.Append(dr[listFields[i]].ToString().Replace(",", "&#44").Replace("，", "&#44") + ",");
                        }
                        strbData.Append("\n");
                    }
                    return strbData.Replace(",\n", "\n").ToString();
                }
            }
        }

        #endregion 返回写入CSV的字符串

        #region 得到一个随意的文件名

        /// <summary>
        /// 得到一个随意的文件名
        /// </summary>
        /// <returns></returns>
        private string GetRandomFileName()
        {
            Random rnd = new Random((int)(DateTime.Now.Ticks));
            string s = rnd.Next(Int32.MaxValue).ToString();
            return DateTime.Now.ToShortDateString() + "_" + s + ".csv";
        }

        #endregion 得到一个随意的文件名
    }
}