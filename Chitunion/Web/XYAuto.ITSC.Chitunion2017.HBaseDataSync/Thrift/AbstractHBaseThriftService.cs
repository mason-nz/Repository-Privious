using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thrift.Transport;
using Thrift.Protocol;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.Thrift
{
    /// <summary>
    /// HBaseThrift客户端抽象服务
    /// </summary>
    public abstract class AbstractHBaseThriftService
    {
        protected static readonly string CHARSET = "UTF-8";
        private string host = "localhost";
        private int port = 9090;
        private readonly TTransport transport;
        protected readonly Hbase.Client client;

        public AbstractHBaseThriftService() :
            this("localhost", 9090)
        {

        }

        public AbstractHBaseThriftService(string host, int port)
        {
            this.host = host;
            this.port = port;
            transport = new TSocket(host, port);
            TProtocol protocol = new TBinaryProtocol(transport, true, true);
            client = new Hbase.Client(protocol);
        }

        /// <summary>
        /// 打开通讯通道
        /// </summary>
        public void Open()
        {
            if (transport != null)
            {
                transport.Open();
            }
        }

        /// <summary>
        /// 关闭通讯通道
        /// </summary>
        public void Close()
        {
            if (transport != null)
            {
                transport.Close();
            }
        }

        /// <summary>
        /// 获取HBase数据所有用户表
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetTables();

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowKey"></param>
        /// <param name="writeToWal"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="attributes"></param>
        public abstract void Update(
            string table,
            string rowKey,
            bool writeToWal,
            string fieldName,
            string fieldValue,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowKey"></param>
        /// <param name="writeToWal"></param>
        /// <param name="fieldNameValues"></param>
        /// <param name="attributes"></param>
        public abstract void Update(
            string table,
            string rowKey,
            bool writeToWal,
            Dictionary<string, string> fieldNameValues,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 删除表中单元格
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="rowKey">行健</param>
        /// <param name="writeToWal"></param>
        /// <param name="column">列族</param>
        /// <param name="attributes">属性</param>
        public abstract void DeleteCell(
            string table,
            string rowKey,
            bool writeToWal,
            string column,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 删除表中指定单元格
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="rowKey">行健</param>
        /// <param name="writeToWal"></param>
        /// <param name="columns">列族</param>
        /// <param name="attributes">属性</param>
        public abstract void DeleteCells(
            string table,
            string rowKey,
            bool writeToWal,
            List<string> columns,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="rowKey">行健</param>
        /// <param name="attributes">属性</param>
        public abstract void DeleteRow(
            string table,
            string rowKey,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="startRow"></param>
        /// <param name="columns"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract int ScannerOpen(
            string table,
            string startRow,
            List<string> columns,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="startRow"></param>
        /// <param name="stopRow"></param>
        /// <param name="columns"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract int ScannerOpen(
            string table,
            string startRow,
            string stopRow,
            List<string> columns,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="startAndPrefix"></param>
        /// <param name="columns"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract int ScannerOpenWithPrefix(
            string table,
            string startAndPrefix,
            List<string> columns,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="startRow"></param>
        /// <param name="columns"></param>
        /// <param name="timestamp"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract int ScannerOpenTs(
            string table,
            string startRow,
            List<string> columns,
            long timestamp,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="startRow"></param>
        /// <param name="stopRow"></param>
        /// <param name="columns"></param>
        /// <param name="timestamp"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract int ScannerOpenTs(
            string table,
            string startRow,
            string stopRow,
            List<string> columns,
            long timestamp,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 扫描器获取行列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nbRows"></param>
        /// <returns></returns>
        public abstract List<TRowResult> ScannerGetList(int id, int nbRows);

        /// <summary>
        /// 扫描器获取行数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract List<TRowResult> ScannerGet(int id);

        /// <summary>
        /// 获取指定行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract List<TRowResult> GetRow(
            string table,
            string row,
            Dictionary<string, string> attributes);


        /// <summary>
        /// 批量获取列族
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rows"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract List<TRowResult> GetRows(
            string table,
            List<string> rows,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 批量获取指定列族的行
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract List<TRowResult> GetRowsWithColumns(
            string table,
            List<string> rows,
            List<string> columns,
            Dictionary<string, string> attributes);

        /// <summary>
        /// 关闭扫描器
        /// </summary>
        /// <param name="id"></param>
        public abstract void ScannerClose(int id);

        /// <summary>
        /// 迭代结果
        /// </summary>
        /// <param name="result"></param>
        public abstract void IterateResults(TRowResult result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filterString"></param>
        /// <param name="columns"></param>
        public abstract List<TRowResult> ScannerOpenWithScan(string table,
            string filterString,
            List<string> columns);

    }
}
