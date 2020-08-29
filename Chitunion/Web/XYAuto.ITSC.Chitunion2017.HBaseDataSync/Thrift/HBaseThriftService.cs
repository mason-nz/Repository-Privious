using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.Thrift
{
    /// <summary>
    /// HBaseThrift服务
    /// </summary>
    public class HBaseThriftService : AbstractHBaseThriftService
    {
        private readonly int HBaseGetRowCount = ConfigurationManager.AppSettings["HBaseGetRowCount"] == "" ? 100000 : Convert.ToInt32(ConfigurationManager.AppSettings["HBaseGetRowCount"]);
        public HBaseThriftService()
            : this("localhost", 9090)
        {

        }

        public HBaseThriftService(string host, int port)
            : base(host, port)
        {

        }

        /// <inheriated-doc />
        public override List<string> GetTables()
        {
            List<byte[]> tables = client.getTableNames();
            List<String> list = new List<String>();
            foreach (byte[] table in tables)
            {
                list.Add(Decode(table));
            }
            return list;
        }

        /// <inheriated-doc />
        public override void Update(string table, string rowKey, bool writeToWal, string fieldName, string fieldValue, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] row = Encode(rowKey);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            List<Mutation> mutations = new List<Mutation>();
            Mutation mutation = new Mutation();
            mutation.IsDelete = false;
            mutation.WriteToWAL = writeToWal;
            mutation.Column = Encode(fieldName);
            mutation.Value = Encode(fieldValue);
            mutations.Add(mutation);
            client.mutateRow(tableName, row, mutations, encodedAttributes);
        }

        /// <inheriated-doc />
        public override void Update(string table, string rowKey, bool writeToWal, Dictionary<string, string> fieldNameValues, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] row = Encode(rowKey);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            List<Mutation> mutations = new List<Mutation>();
            foreach (KeyValuePair<String, String> pair in fieldNameValues)
            {
                Mutation mutation = new Mutation();
                mutation.IsDelete = false;
                mutation.WriteToWAL = writeToWal;
                mutation.Column = Encode(pair.Key);
                mutation.Value = Encode(pair.Value);
                mutations.Add(mutation);
            }
            client.mutateRow(tableName, row, mutations, encodedAttributes);
        }

        /// <inheriated-doc />
        public override void DeleteCell(string table, string rowKey, bool writeToWal, string column, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] row = Encode(rowKey);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            List<Mutation> mutations = new List<Mutation>();
            Mutation mutation = new Mutation();
            mutation.IsDelete = true;
            mutation.WriteToWAL = writeToWal;
            mutation.Column = Encode(column);
            mutations.Add(mutation);
            client.mutateRow(tableName, row, mutations, encodedAttributes);
        }

        /// <inheriated-doc />
        public override void DeleteCells(string table, string rowKey, bool writeToWal, List<string> columns, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] row = Encode(rowKey);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            List<Mutation> mutations = new List<Mutation>();
            foreach (string column in columns)
            {
                Mutation mutation = new Mutation();
                mutation.IsDelete = true;
                mutation.WriteToWAL = writeToWal;
                mutation.Column = Encode(column);
                mutations.Add(mutation);
            }
            client.mutateRow(tableName, row, mutations, encodedAttributes);
        }

        /// <inheriated-doc />
        public override void DeleteRow(string table, string rowKey, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] row = Encode(rowKey);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            client.deleteAllRow(tableName, row, encodedAttributes);
        }

        /// <inheriated-doc />
        public override int ScannerOpen(string table, string startRow, List<string> columns, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] start = Encode(startRow);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.scannerOpen(tableName, start, encodedColumns, encodedAttributes);
        }

        /// <inheriated-doc />
        public override int ScannerOpen(string table, string startRow, string stopRow, List<string> columns, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] start = Encode(startRow);
            byte[] stop = Encode(stopRow);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.scannerOpenWithStop(tableName, start, stop, encodedColumns, encodedAttributes);
        }

        /// <inheriated-doc />
        public override int ScannerOpenWithPrefix(string table, string startAndPrefix, List<string> columns, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] prefix = Encode(startAndPrefix);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.scannerOpenWithPrefix(tableName, prefix, encodedColumns, encodedAttributes);
        }

        /// <inheriated-doc />
        public override int ScannerOpenTs(string table, string startRow, List<string> columns, long timestamp, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] start = Encode(startRow);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.scannerOpenTs(tableName, start, encodedColumns, timestamp, encodedAttributes);
        }

        /// <inheriated-doc />
        public override int ScannerOpenTs(string table, string startRow, string stopRow, List<string> columns, long timestamp, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] start = Encode(startRow);
            byte[] stop = Encode(stopRow);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.scannerOpenWithStopTs(tableName, start, stop, encodedColumns, timestamp, encodedAttributes);
        }

        /// <inheriated-doc />
        public override List<TRowResult> ScannerGetList(int id, int nbRows)
        {
            return client.scannerGetList(id, nbRows);
        }

        /// <inheriated-doc />
        public override List<TRowResult> ScannerGet(int id)
        {
            return client.scannerGet(id);
        }

        /// <inheriated-doc />
        public override List<TRowResult> GetRow(string table, string row, Dictionary<String, String> attributes)
        {
            byte[] tableName = Encode(table);
            byte[] startRow = Encode(row);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.getRow(tableName, startRow, encodedAttributes);

        }

        /// <inheriated-doc />
        public override List<TRowResult> GetRows(string table, List<string> rows, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            List<byte[]> encodedRows = EncodeStringList(rows);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.getRows(tableName, encodedRows, encodedAttributes);
        }

        /// <inheriated-doc />
        public override List<TRowResult> GetRowsWithColumns(string table, List<string> rows, List<string> columns, Dictionary<string, string> attributes)
        {
            byte[] tableName = Encode(table);
            List<byte[]> encodedRows = EncodeStringList(rows);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            Dictionary<byte[], byte[]> encodedAttributes = EncodeAttributes(attributes);
            return client.getRowsWithColumns(tableName, encodedRows, encodedColumns, encodedAttributes);
        }

        /// <inheriated-doc />
        public override void ScannerClose(int id)
        {
            client.scannerClose(id);
        }

        /// <inheriated-doc />
        public override void IterateResults(TRowResult result)
        {
            foreach (KeyValuePair<byte[], TCell> pair in result.Columns)
            {
                Console.WriteLine("\tCol=" + Decode(pair.Key) + ", Value=" + Decode(pair.Value.Value));
            }
        }

        /// <inheriated-doc />
        private String Decode(byte[] bs)
        {
            return UTF8Encoding.Default.GetString(bs);
        }

        /// <inheriated-doc />
        private byte[] Encode(String str)
        {
            return UTF8Encoding.Default.GetBytes(str);
        }

        /// <inheriated-doc />
        private Dictionary<byte[], byte[]> EncodeAttributes(Dictionary<String, String> attributes)
        {
            Dictionary<byte[], byte[]> encodedAttributes = new Dictionary<byte[], byte[]>();
            foreach (KeyValuePair<String, String> pair in attributes)
            {
                encodedAttributes.Add(Encode(pair.Key), Encode(pair.Value));
            }
            return encodedAttributes;
        }

        /// <inheriated-doc />
        private List<byte[]> EncodeStringList(List<String> strings)
        {
            List<byte[]> list = new List<byte[]>();
            if (strings != null)
            {
                foreach (String str in strings)
                {
                    list.Add(Encode(str));
                }
            }
            return list;
        }

        public override List<TRowResult> ScannerOpenWithScan(string table,
            string filterString,
            List<string> columns)
        {
            byte[] tableName = Encode(table);
            byte[] encodedFilterString = Encoding.UTF8.GetBytes(filterString);
            List<byte[]> encodedColumns = EncodeStringList(columns);
            //return client.getRowsWithColumns(tableName, encodedRows, encodedColumns, encodedAttributes);

            TScan _scan = new TScan();
            //SingleColumnValueFilter('i', 'Data', =, '2')  
            _scan.FilterString = encodedFilterString;
            _scan.Columns = encodedColumns;
            //transport.Open();
            int ScannerID = client.scannerOpenWithScan(tableName, _scan, null);
            //scannerID = ScannerID;
            List<TRowResult> reslut = client.scannerGetList(ScannerID, HBaseGetRowCount);
            return reslut;
        }
    }
}
