using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.DAL
{
    public class WXArticleInfo : XYAuto.ITSC.Chitunion2017.Dal.DataBase
    {
        public static readonly WXArticleInfo Instance = new WXArticleInfo();

        public void SyncData(DataTable sourceTable, string tableName, string con, out string errorMsg, int batchSize = 10000)
        {
            errorMsg = string.Empty;
            using (SqlConnection conn = new SqlConnection(con))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlBulkCopyByDataTable(conn, trans, tableName, sourceTable, batchSize);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        //trans.Rollback();
                        errorMsg = ex.Message;
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


//        public void SyncData(Entities.ArticleInfo model, string tableName, string con, out string errorMsg)
//        {
//            errorMsg = string.Empty;
//            using (SqlConnection conn = new SqlConnection(con))
//            {
//                conn.Open();
//                using (SqlTransaction trans = conn.BeginTransaction())
//                {
//                    try
//                    {
//                        InsertArticleInfo(trans, tableName, model);
//                        trans.Commit();
//                    }
//                    catch (Exception ex)
//                    {
//                        //trans.Rollback();
//                        errorMsg = ex.Message;
//                    }
//                }
//            }
//        }


//        public int InsertArticleInfo(SqlTransaction trans, string dataTableName, Entities.ArticleInfo model)
//        {
//            string sql = @"INSERT dbo.ArticleInfo
//        ( XyAttr ,
//          Url ,
//          Title ,
//          HeadImg ,
//          ReadNum ,
//          LikeNum ,
//          ComNum ,
//          Content ,
//          JsonContent ,
//          Abstract ,
//          CopyrightState ,
//          CarSerial ,
//          Resource ,
//          Category ,
//          Tag ,
//          DataId ,
//          DataName ,
//          RowKey ,
//          PublishTime ,
//          CreateTime ,
//          LastUpdateTime ,
//          IsIndex ,
//          Score ,
//          CategoryNew ,
//          HeadImgNew ,
//          Author ,
//          CategoryID
//        )
//VALUES  ( @XyAttr , -- XyAttr - int
//          @Url , -- Url - varchar(512)
//          @Title , -- Title - nvarchar(512)
//          @HeadImg , -- HeadImg - varchar(512)
//          @ReadNum , -- ReadNum - int
//          @LikeNum , -- LikeNum - int
//          @ComNum , -- ComNum - int
//          @Content , -- Content - nvarchar(max)
//          @JsonContent , -- JsonContent - text
//          @Abstract , -- Abstract - varchar(1024)
//          @CopyrightState , -- CopyrightState - int
//          @CarSerial , -- CarSerial - varchar(64)
//          @Resource , -- Resource - int
//          @Category , -- Category - varchar(32)
//          @Tag , -- Tag - varchar(64)
//          @DataId , -- DataId - varchar(64)
//          @DataName , -- DataName - varchar(64)
//          @RowKey , -- RowKey - varchar(64)
//          @PublishTime , -- PublishTime - datetime
//          GETDATE() , -- CreateTime - datetime
//          @LastUpdateTime , -- LastUpdateTime - datetime
//          @IsIndex , -- IsIndex - int
//          @Score , -- Score - decimal(10, 2)
//          @CategoryNew , -- CategoryNew - varchar(32)
//          @HeadImgNew , -- HeadImgNew - varchar(512)
//          @Author , -- Author - varchar(64)
//          @CategoryID  -- CategoryID - int
//        );select SCOPE_IDENTITY();";
//            SqlParameter[] parameters = {
//                new SqlParameter("@WxNum",model.XyAttr),
//                new SqlParameter("@Url",model.Url),
//                new SqlParameter("@Title",model.Title),
//                new SqlParameter("@HeadImg",model.HeadImg),
//                new SqlParameter("@ReadNum",model.ReadNum),
//                new SqlParameter("@LikeNum",model.LikeNum),
//                new SqlParameter("@ComNum",model.ComNum),
//                new SqlParameter("@Content",model.Content),
//                new SqlParameter("@JsonContent",model.JsonContent),
//                new SqlParameter("@Abstract",model.Abstract),
//                new SqlParameter("@CopyrightState",model.CopyrightState),
//                new SqlParameter("@CarSerial",model.CarSerial),
//                new SqlParameter("@Resource",model.Resource),
//                new SqlParameter("@Category",model.Category),
//                new SqlParameter("@Tag",model.Tag),
//                new SqlParameter("@DataId",model.DataId),
//                new SqlParameter("@DataName",model.DataName),
//                new SqlParameter("@RowKey",model.RowKey),
//                new SqlParameter("@PublishTime",model.PublishTime),
//                new SqlParameter("@LastUpdateTime",model.LastUpdateTime),
//                new SqlParameter("@IsIndex",model.IsIndex),
//                new SqlParameter("@Score",model.Score),
//                new SqlParameter("@CategoryNew",model.CategoryNew),
//                new SqlParameter("@HeadImgNew",model.HeadImgNew),
//                new SqlParameter("@Author",model.Author),
//                new SqlParameter("@CategoryID",model.CategoryID)
//        };


//            return XYAuto.Utils.Data.SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
//        }


    }
}
