using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.BatchMediaAudit
{
    public class BatchMediaAudit : DataBase
    {
        public static readonly BatchMediaAudit Instance = new BatchMediaAudit();
        public int Insert(Entities.BatchMediaAudit.BatchMediaAudit entity)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"INSERT dbo.BatchMediaAudit
                                    ( TaskType ,
                                      MediaType ,
                                      MediaName ,
                                      MediaNumber ,
                                      HeadImg ,
                                      IsSelfDo ,
                                      HomeUrl ,
                                      BrandID ,
                                      SerialID ,
                                      Status ,
                                      SubmitTime ,
                                      CreateTime ,
                                      CreateUserID
                                    )
                            VALUES  ( @TaskType , -- TaskType - int
                                      @MediaType , -- MediaType - int
                                      @MediaName , -- MediaName - varchar(200)
                                      @MediaNumber , -- MediaNumber - varchar(100)
                                      @HeadImg , -- HeadImg - varchar(200)
                                      @IsSelfDo , -- IsSelfDo - bit
                                      @HomeUrl , -- HomeUrl - varchar(200)
                                      @BrandID , -- BrandID - int
                                      @SerialID , -- SerialID - int
                                      @Status , -- Status - int
                                      GETDATE() , -- SubmitTime - datetime
                                      GETDATE() , -- CreateTime - datetime
                                      @CreateUserID  -- CreateUserID - int
                                    )");
            sbSql.Append(";SELECT SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@TaskType",entity.TaskType),
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaName",entity.MediaName),
                        new SqlParameter("@MediaNumber",entity.MediaNumber),
                        new SqlParameter("@HeadImg",entity.HeadImg),
                        new SqlParameter("@IsSelfDo",entity.IsSelfDo),
                        new SqlParameter("@HomeUrl",entity.HomeUrl),
                        new SqlParameter("@BrandID",entity.BrandID),
                        new SqlParameter("@SerialID",entity.SerialID),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateUserID",entity.CreateUserID)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        #region 根据任务类型、媒体类型
        public Entities.BatchMediaAudit.BatchMediaAudit GetModelPending(int taskType,int mediaType,string number,string name)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"SELECT TOP 1
                                    *
                            FROM    dbo.BatchMediaAudit BMA
                            WHERE   BMA.TaskType = {taskType}
                                    AND BMA.MediaType = {mediaType}                                    
                                    AND BMA.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审}");

            if (mediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信)
                sbSql.Append($@"AND BMA.MediaNumber = '{number}'");
            else
                sbSql.Append($@"AND BMA.MediaName = '{name}'");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToEntity<Entities.BatchMediaAudit.BatchMediaAudit>(data.Tables[0]);
        }
        #endregion
        #region 根据车型
        public Entities.BatchMediaAudit.BatchMediaAudit GetModelPending(int brandID, int serialID)
        {
            var sbSql = new StringBuilder();
            if (serialID == -2)
                sbSql.Append($@"SELECT TOP 1
                                    *
                            FROM    dbo.BatchMediaAudit BMA
                            WHERE   BMA.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌}
                                    AND BMA.BrandID = {brandID}                                    
                                    AND BMA.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审}");
            else
                sbSql.Append($@"SELECT TOP 1
                                    *
                            FROM    dbo.BatchMediaAudit BMA
                            WHERE   BMA.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型}
                                    AND BMA.BrandID = {brandID} 
                                    AND BMA.SerialID = {serialID}                                   
                                    AND BMA.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审}");


            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToEntity<Entities.BatchMediaAudit.BatchMediaAudit>(data.Tables[0]);
        }
        #endregion
    }
}
