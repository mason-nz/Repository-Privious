using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.UploadFileInfo
{
    public class UploadFileInfo : DataBase
    {
        #region Instance

        public static readonly UploadFileInfo Instance = new UploadFileInfo();

        #endregion Instance

        public List<Entities.UploadFileInfo.UploadFileInfo> GetList(UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo> query)
        {
            var sql = new StringBuilder();

            sql.AppendFormat(@"SELECT {0}  RecID ,
                                            [Type] ,
                                            RelationTableName ,
                                            RelationID ,
                                            FilePah ,
                                            [FileName] ,
                                            ExtendName ,
                                            FileSize ,
                                            CreateTime ,
                                            CreaetUserID
                                    FROM    [dbo].[UploadFileInfo] WITH ( NOLOCK ) WHERE 1=1 ", query.PageSize);
            var paras = new List<SqlParameter>();
            if (query.RelationID > 0)
            {
                sql.Append(" AND RelationID = @RelationID ");
                paras.Add(new SqlParameter("@RelationID", query.RelationID));
            }
            if (query.Type > 0)
            {
                sql.Append(" AND Type = @Type ");
                paras.Add(new SqlParameter("@Type", query.Type));
            }
            if (query.CreaetUserID > 0)
            {
                sql.Append(" AND CreaetUserID = @CreaetUserID ");
                paras.Add(new SqlParameter("@CreaetUserID", query.CreaetUserID));
            }
            if (!string.IsNullOrWhiteSpace(query.FilePah))
            {
                sql.Append(" AND FilePah = @FilePah ");
                paras.Add(new SqlParameter("@FilePah", query.FilePah));
            }
            if (!string.IsNullOrWhiteSpace(query.AEUserId) && !string.IsNullOrWhiteSpace(query.AEUserId.Trim(',')))
            {
                sql.AppendFormat(" AND CreaetUserID IN ({0}) ", query.AEUserId.Trim(','));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), paras.ToArray());
            return DataTableToList<Entities.UploadFileInfo.UploadFileInfo>(data.Tables[0]);
        }

        public int Insert(Entities.UploadFileInfo.UploadFileInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into UploadFileInfo(");
            strSql.Append("Type,RelationTableName,RelationID,FilePah,FileName,ExtendName,FileSize,CreateTime,CreaetUserID");
            strSql.Append(") values (");
            strSql.Append("@Type,@RelationTableName,@RelationID,@FilePah,@FileName,@ExtendName,@FileSize,@CreateTime,@CreaetUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Type",entity.Type),
                        new SqlParameter("@RelationTableName",entity.RelationTableName),
                        new SqlParameter("@RelationID",entity.RelationID),
                        new SqlParameter("@FilePah",entity.FilePah),
                        new SqlParameter("@FileName",entity.FileName),
                        new SqlParameter("@ExtendName",entity.ExtendName),
                        new SqlParameter("@FileSize",entity.FileSize),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreaetUserID",entity.CreaetUserID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public int Update(Entities.UploadFileInfo.UploadFileInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_Weixin]");
            strSql.Append(@" SET [Type] = @Type
                              ,[RelationTableName] = @RelationTableName
                              ,[RelationID] = @RelationID
                              ,[FilePah] = @FilePah
                              ,[FileName] = @FileName
                              ,[ExtendName] = @ExtendName
                              ,[FileSize] = @FileSize
                              --,[CreateTime] = <CreateTime, datetime,>
                              --,[CreaetUserID] = @CreaetUserID
                            WHERE RecID = @RecID");
            var parameters = new SqlParameter[]{
                 new SqlParameter("@RecID",entity.RecID),
                        new SqlParameter("@Type",entity.Type),
                        new SqlParameter("@RelationTableName",entity.RelationTableName),
                        new SqlParameter("@RelationID",entity.RelationID),
                        new SqlParameter("@FilePah",entity.FilePah),
                        new SqlParameter("@FileName",entity.FileName),
                        new SqlParameter("@ExtendName",entity.ExtendName),
                        new SqlParameter("@FileSize",entity.FileSize),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreaetUserID",entity.CreaetUserID),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public int Delete(int recId)
        {
            var sql = @"DELETE FROM DBO.[UploadFileInfo] WHERE RecID = @RecID ";
            var parameters = new SqlParameter[] { new SqlParameter("@RecID", recId) };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// CreaetUserID 可以不传，RelationID，Type 必须传
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(Entities.UploadFileInfo.UploadFileInfo entity)
        {
            var sql = @"DELETE FROM DBO.[UploadFileInfo] WHERE RelationID = @RelationID AND RelationTableName = @RelationTableName AND Type = @Type ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Type",entity.Type),
                new SqlParameter("@RelationTableName",entity.RelationTableName),
                new SqlParameter("@RelationID",entity.RelationID)
            };
            if (entity.CreaetUserID > 0)
            {
                sql += " AND CreaetUserID=@CreaetUserID ";
                parameters.Add(new SqlParameter("@CreaetUserID", entity.CreaetUserID));
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
        }

        /// <summary>
        /// 2017-03-24 张立彬 删除反馈数据对应的图片信息
        /// </summary>
        /// <param name="TalbeID">反馈表ID</param>
        /// <param name="TableName">反馈表名称</param>
        /// <param name="UploadType">上传类型</param>
        /// <returns></returns>
        public int Delete(int TalbeID, string TableName, int UploadType)
        {
            string strSql = "delete from UploadFileInfo where RelationID=@RelationID and RelationTableName=@RelationTableName and Type=@Type";
            SqlParameter[] param = new SqlParameter[]
            {
                   new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@RelationTableName",SqlDbType.VarChar,50),
                new SqlParameter("@RelationID",SqlDbType.Int)
            };
            param[0].Value = UploadType;
            param[1].Value = TableName;
            param[2].Value = TalbeID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, param);
        }
    }
}