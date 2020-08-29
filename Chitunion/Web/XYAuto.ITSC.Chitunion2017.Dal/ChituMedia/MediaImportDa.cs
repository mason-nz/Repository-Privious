using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class MediaImportDa:DataBase
    {
        #region 初始化
        private MediaImportDa() { }

        public static MediaImportDa instance = null;
        public static readonly object padlock = new object();

        public static MediaImportDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MediaImportDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 插入APP媒体信息
        /// </summary>
        /// <param name="sourceValueModel"></param>
        /// <returns></returns>
        public bool InsertAppMedai(DataTable sourceValueModel)
        {
            bool result = false;
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRINGS))
            {
                connection.Open();
                //开启事务
                SqlTransaction trans = connection.BeginTransaction();
                //使用批处理插入数据库 临时表
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, trans))
                {
                    try
                    {
                        if (sourceValueModel != null && sourceValueModel.Rows.Count > 0)
                        {

                            sqlBulkCopy.DestinationTableName = "LE_APP_Temp";//插入表名

                            sqlBulkCopy.BatchSize = 5000;//分批次插入（每批次插入的条数）

                            sqlBulkCopy.BulkCopyTimeout = 120;//超时时间（秒数）

                            //循环对Table列赋值
                            for (int i = 0; i < sourceValueModel.Columns.Count; i++)
                            {
                                sqlBulkCopy.ColumnMappings.Add(sourceValueModel.Columns[i].ColumnName, sourceValueModel.Columns[i].ColumnName);
                            }

                            sqlBulkCopy.WriteToServer(sourceValueModel);//将对象Table复制到SqlBulkCopy中

                            sourceValueModel.Dispose();//释放Table资源

                        }

                        string SQL = @"MERGE INTO LE_APP AS W
                                    USING
                                        ( SELECT    LT.* ,
                                                    D.DictId
                                          FROM      dbo.LE_APP_Temp AS LT
                                                    LEFT JOIN ( SELECT Name ,
                                                                        MAX(RecID) AS RecID
                                                                 FROM   dbo.LE_APP
                                                                 GROUP BY Name
                                                               ) AS A ON A.RecID = LT.RecID
                                                    LEFT JOIN dbo.DictInfo AS D ON D.DictName = LT.CategoryName
                                                                                   AND D.DictType = 52
                                        ) AS T
                                    ON T.Name = W.Name
                                    WHEN MATCHED THEN
                                        UPDATE SET W.Name = T.Name ,
                                                   W.HeadIconURL = T.HeadIconURL ,
                                                   W.DailyLive = T.DailyLive ,
                                                   W.CategoryID = T.DictId ,
                                                   W.IsMonitor = T.IsMonitor ,
                                                   W.IsLocate = T.IsLocate ,
                                                   W.Remark = T.Remark ,
                                                   W.TotalUser = T.TotalUser
                                    WHEN NOT MATCHED THEN
                                        INSERT
                                        VALUES (		 T.Name ,
		                                                 T.HeadIconURL ,
		                                                 T.ProvinceID ,
		                                                 T.CityID ,
		                                                 T.DailyLive ,
		                                                 T.Remark ,
		                                                 T.Status ,
		                                                 T.DictId ,
		                                                 T.IsMonitor ,
		                                                 T.IsLocate ,
		                                                 T.CreateTime ,
					                                     NULL,
		                                                 T.TotalUser ,
		                                                 T.CreateUserID
                                               );   ";
                        //插入生产表
                        int returnNum = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, SQL, null);

                        trans.Commit();//事务提交
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();

                        throw ex;

                    }
                    finally
                    {

                        connection.Close();
                    }
                }
            }

            return result;
        }
    }
}
