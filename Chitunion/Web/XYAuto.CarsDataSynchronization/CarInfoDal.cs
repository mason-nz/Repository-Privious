using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using XYAuto.Utils.Data;

namespace XYAuto.CarsDataSynchronization
{
    /// <summary>
    /// 2017-03-03张立彬
    /// 汽车信息Dal
    /// </summary>
    public class CarInfoDal : DataBase
    {
        public static readonly CarInfoDal Instance = new CarInfoDal();

        #region 同步汽车品牌信息
        /// <summary>
        /// 2017-03-03张立彬
        /// 清空汽车品牌信息
        /// </summary>
        public void ClearCarBrandInfo()
        {
            string strSql = "TRUNCATE TABLE Car_Brand";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        ///  2017-03-03张立彬
        /// 批量添加汽车品牌信息
        /// </summary>
        /// <param name="lsitCarBrand"></param>
        /// <returns></returns>
        public long InsertCarBrandInfo(List<CarBrandModel> lsitCarBrand)
        {
            long sqlBulkCopyInsertRunTime = SqlBulkCopyInsertCarBrand(lsitCarBrand);
            return sqlBulkCopyInsertRunTime;
        }
        /// <summary>
        /// 使用SqlBulkCopy方式插入数据
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static long SqlBulkCopyInsertCarBrand(List<CarBrandModel> lsitCarBrand)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DataTable dataTable = GetTableSchemaCardBrand();
            string passportKey;
            for (int i = 0; i < lsitCarBrand.Count; i++)
            {
                passportKey = Guid.NewGuid().ToString();
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = lsitCarBrand[i].MasterBrandID;
                dataRow[1] = lsitCarBrand[i].BrandID;
                dataRow[2] = lsitCarBrand[i].Name;
                dataRow[3] = lsitCarBrand[i].SEOName;
                dataRow[4] = lsitCarBrand[i].AllSpell;
                dataRow[5] = lsitCarBrand[i].Spell;
                dataRow[6] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dataRow[7] = lsitCarBrand[i].Country;
                dataRow[8] = lsitCarBrand[i].CountrySeries;
                dataTable.Rows.Add(dataRow);
            }

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(CONNECTIONSTRINGS);
            sqlBulkCopy.DestinationTableName = "Car_Brand";
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            SqlConnection sqlConnection = new SqlConnection(CONNECTIONSTRINGS);
            sqlConnection.Open();
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                sqlBulkCopy.WriteToServer(dataTable);
            }
            sqlBulkCopy.Close();
            sqlConnection.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        private static DataTable GetTableSchemaCardBrand()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("MasterBrandID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("BrandID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("Name") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("SEOName") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("AllSpell") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("Spell") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CreateTime") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("Country") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CountrySeries") });
            return dataTable;
        }
        #endregion

        /// <summary>
        /// 2017-03-03张立彬
        /// 清空汽车车系信息
        /// </summary>
        public void ClearCarSerialInfo(string CarBrandID)
        {
            string strSql = "delete from Car_Serial where BrandID=" + CarBrandID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);

        }
        /// <summary>
        ///  2017-03-03张立彬
        /// 批量添加汽车品牌信息
        /// </summary>
        /// <param name="lsitCarBrand"></param>
        /// <returns></returns>
        public long InsertCarSerialInfo(List<CardSerialModel> lsitSerial)
        {
            long sqlBulkCopyInsertRunTime = SqlBulkCopyInsertCarSerial(lsitSerial);
            return sqlBulkCopyInsertRunTime;
        }
        /// <summary>
        /// 使用SqlBulkCopy方式插入数据
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static long SqlBulkCopyInsertCarSerial(List<CardSerialModel> lsitSerial)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DataTable dataTable = GetTableSchemaCarSearil();
            string passportKey;
            for (int i = 0; i < lsitSerial.Count; i++)
            {
                passportKey = Guid.NewGuid().ToString();
                DataRow dataRow = dataTable.NewRow();
                dataRow[0] = lsitSerial[i].MasterBrandID;
                dataRow[1] = lsitSerial[i].BrandID;
                dataRow[2] = lsitSerial[i].SerialID;
                dataRow[3] = lsitSerial[i].Name;
                dataRow[4] = lsitSerial[i].SEOName;
                dataRow[5] = lsitSerial[i].AllSpell;
                dataRow[6] = lsitSerial[i].Spell;
                dataRow[7] = lsitSerial[i].CsSaleState;
                dataRow[8] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dataRow[9] = lsitSerial[i].CsLevel;
                dataTable.Rows.Add(dataRow);
            }

            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(CONNECTIONSTRINGS);
            sqlBulkCopy.DestinationTableName = "Car_Serial";
            sqlBulkCopy.BatchSize = dataTable.Rows.Count;
            SqlConnection sqlConnection = new SqlConnection(CONNECTIONSTRINGS);
            sqlConnection.Open();
            if (dataTable != null && dataTable.Rows.Count != 0)
            {
                sqlBulkCopy.WriteToServer(dataTable);
            }
            sqlBulkCopy.Close();
            sqlConnection.Close();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        private static DataTable GetTableSchemaCarSearil()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("MasterBrandID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("BrandID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("SerialID") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("Name") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("SEOName") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("AllSpell") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("Spell") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CsSaleState") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CreateTime") });
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("CsLevel") });
   
            return dataTable;
        }
    }
}
