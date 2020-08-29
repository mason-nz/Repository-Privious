using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.Thrift
{
    public class HBaseThriftHelper
    {
        private readonly AbstractHBaseThriftService thriftService;
        private static string HBaseIP = ConfigurationManager.AppSettings["HBaseIP"];
        private static int HBasePort = ConfigurationManager.AppSettings["HBasePort"] == "" ? 9090 : Convert.ToInt32(ConfigurationManager.AppSettings["HBasePort"]);        

        public HBaseThriftHelper(String host, int port)
        {
            thriftService = new HBaseThriftService(host, port);
            thriftService.Open();
        }

        public HBaseThriftHelper() : this(HBaseIP, HBasePort)
        {

        }


        public void Close()
        {
            thriftService.Close();
        }

        /// <summary>
        /// 返回HBase，当前表名List
        /// </summary>
        /// <returns>表名List集合</returns>
        public List<string> GetTables()
        {
            return thriftService.GetTables();
            //foreach (var existedTableName in tableNameList)
            //{
            //    Console.WriteLine("Table:{0}", existedTableName);
            //}
        }


        #region 注释掉的逻辑
        //public void CaseForCreateTable(
        //    string tableName,
        //    IList<string> columnNameList)
        //{
        //    thriftService.CreateTable(tableName, columnNameList);
        //}

        //public void CaseForUpdate()
        //{
        //    bool writeToWal = false;
        //    Dictionary<String, String> attributes = new Dictionary<String, String>(0);
        //    string table = SetTable();
        //    // put kv pairs

        //    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        //    watch.Start();
        //    for (long i = 0; i < 50000; i++)
        //    {
        //        string rowKey = i.ToString().PadLeft(4, '0');
        //        Dictionary<String, String> fieldNameValues = new Dictionary<String, String>();
        //        fieldNameValues.Add("info:name", "NameHBaseThriftHelper");
        //        thriftService.Update(table, rowKey, writeToWal, fieldNameValues, attributes);

        //        if (i % 10000 == 0)
        //        {
        //            Console.WriteLine(
        //                "Insert [{0}] Rows Time Eclipsed:{1}",
        //                i,
        //                watch.ElapsedMilliseconds);
        //        }
        //    }
        //    watch.Stop();

        //    Console.WriteLine(
        //                "Insert [{0}] Rows Time Eclipsed:{1}",
        //                50000,
        //                watch.ElapsedMilliseconds);
        //}

        //public void CaseForDeleteCells()
        //{
        //    bool writeToWal = false;
        //    Dictionary<String, String> attributes = new Dictionary<String, String>(0);
        //    String table = SetTable();
        //    // put kv pairs
        //    for (long i = 5; i < 10; i++)
        //    {
        //        String rowKey = i.ToString().PadLeft(4, '0');
        //        List<String> columns = new List<String>(0);
        //        columns.Add("info:birthday");
        //        thriftService.DeleteCells(table, rowKey, writeToWal, columns, attributes);
        //    }
        //}

        //public void CaseForDeleteRow()
        //{
        //    Dictionary<String, String> attributes = new Dictionary<String, String>(0);
        //    String table = SetTable();
        //    // delete rows
        //    for (long i = 5; i < 10; i++)
        //    {
        //        String rowKey = i.ToString().PadLeft(4, '0');
        //        thriftService.DeleteRow(table, rowKey, attributes);
        //    }
        //}

        //public void CaseForScan()
        //{
        //    Dictionary<String, String> attributes = new Dictionary<String, String>(0);
        //    String table = "";// SetTable();
        //    String startRow = "0001";
        //    String stopRow = "0015";
        //    List<String> columns = new List<String>(0);
        //    columns.Add("info:name");
        //    int id = thriftService.ScannerOpen(table, startRow, stopRow, columns, attributes);
        //    int nbRows = 2;
        //    List<TRowResult> results = thriftService.ScannerGetList(id, nbRows);
        //    while (results?.Count > 0)
        //    {
        //        foreach (TRowResult result in results)
        //        {
        //            thriftService.IterateResults(result);
        //        }
        //        results = thriftService.ScannerGetList(id, nbRows);
        //    }
        //    thriftService.ScannerClose(id);
        //}
        #endregion

        /// <summary>
        /// 根据表名及RowKey，获取指定的行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="rowKey">RowKey</param>
        /// <returns>返回指定的行数据（若Rowkey重复，会过滤掉）</returns>
        public Dictionary<string, Dictionary<string, string>> GetRow(string tableName, string rowKey)
        {
            Dictionary<String, String> attributes = new Dictionary<String, String>(0);
            Dictionary<string, Dictionary<string, string>> dicResult = new Dictionary<string, Dictionary<string, string>>(0);
            List<TRowResult> reslut = thriftService.GetRow(tableName, rowKey, attributes);

            BindData(dicResult, reslut);
            return dicResult;
        }


        /// <summary>
        /// 根据表名及RowKey，获取指定的行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="rowKey">RowKey字符串列表</param>
        /// <returns>返回指定的行数据（若Rowkey重复，会过滤掉）</returns>
        public Dictionary<string, Dictionary<string, string>> GetRows(string tableName, List<string> rowKey)
        {
            Dictionary<String, String> attributes = new Dictionary<String, String>(0);
            Dictionary<string, Dictionary<string, string>> dicResult = new Dictionary<string, Dictionary<string, string>>(0);
            List<TRowResult> reslut = thriftService.GetRows(tableName, rowKey, attributes);
            BindData(dicResult, reslut);
            return dicResult;
        }

        private void BindData(Dictionary<string, Dictionary<string, string>> dicResult, List<TRowResult> reslut)
        {
            foreach (var key in reslut)
            {
                Dictionary<string, string> data = new Dictionary<string, string>(0);
                foreach (var k in key.Columns)
                {
                    data.Add(Encoding.UTF8.GetString(k.Key), Encoding.UTF8.GetString(k.Value.Value));
                }
                if (key.Columns.Count > 0 && !dicResult.ContainsKey(Encoding.UTF8.GetString(key.Row)))
                {
                    dicResult.Add(Encoding.UTF8.GetString(key.Row), data);
                }
            }
        }

        /// <summary>
        /// 根据表名、RowKey及指定列名，获取指定的行数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="rowKeys">RowKey列表</param>
        /// <param name="columns">指定列名数组</param>
        /// <returns>返回指定的行数据（若Rowkey重复，会过滤掉）</returns>
        public Dictionary<string, Dictionary<string, string>> GetRowsWithColumns(string tableName, List<string> rowKeys, List<string> columns)
        {
            Dictionary<String, String> attributes = new Dictionary<String, String>(0);
            Dictionary<string, Dictionary<string, string>> dicResult = new Dictionary<string, Dictionary<string, string>>(0);
            List<TRowResult> results = thriftService.GetRowsWithColumns(tableName, rowKeys, columns, attributes);
            BindData(dicResult, results);
            return dicResult;
        }


        public Dictionary<string, Dictionary<string, string>> ScannerOpenWithScan(string tableName, string filterString, List<string> columns)
        {
            //Dictionary<String, String> attributes = new Dictionary<String, String>(0);
            Dictionary<string, Dictionary<string, string>> dicResult = new Dictionary<string, Dictionary<string, string>>(0);
            List<TRowResult> results = thriftService.ScannerOpenWithScan(tableName, filterString, columns);
            BindData(dicResult, results);
            return dicResult;
        }



    }
}
