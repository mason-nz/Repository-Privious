using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaCollectionBlacklist : DataBase
    {
        #region Instance

        public static readonly MediaCollectionBlacklist Instance = new MediaCollectionBlacklist();

        #endregion Instance

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.Media.MediaCollectionBlacklist entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_CollectionBlacklist(");
            strSql.Append("MediaID,MediaType,RelationType,CreateTime,Status,CreateUserID");
            strSql.Append(") values (");
            strSql.Append("@MediaID,@MediaType,@RelationType,@CreateTime,@Status,@CreateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@RelationType",entity.RelationType),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public Entities.Media.MediaCollectionBlacklist GetEntity(int recId)
        {
            const string sql = @"SELECT RecID ,
                                       MediaID ,
                                       MediaType ,
                                       RelationType ,
	                                   [Status],
                                       CreateTime ,
                                       CreateUserID
	                                   FROM DBO.Media_CollectionBlacklist WITH(NOLOCK)
	                            WHERE Status = 0 AND RecID = @RecID
                              ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@RecID", recId)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaCollectionBlacklist>(data.Tables[0]);
        }

        public Entities.Media.MediaCollectionBlacklist GetEntity(int mediaId, int mediaType, int relationType, int createUserId)
        {
            const string sql = @"SELECT RecID ,
                                       MediaID ,
                                       MediaType ,
                                       RelationType ,
	                                   [Status],
                                       CreateTime ,
                                       CreateUserID
	                                   FROM DBO.Media_CollectionBlacklist WITH(NOLOCK)
	                            WHERE Status = 0 AND MediaID = @MediaID
                                AND MediaType = @MediaType
                                AND CreateUserID = @CreateUserID
                                AND RelationType = @RelationType";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@MediaID", mediaId) ,
                new SqlParameter("@MediaType", mediaType),
                 new SqlParameter("@CreateUserID", createUserId),
                new SqlParameter("@RelationType", relationType)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaCollectionBlacklist>(data.Tables[0]);
        }

        public List<Entities.Media.MediaCollectionBlacklist> GetList(MediaQuery<Entities.Media.MediaCollectionBlacklist> query)
        {
            var sql = @"SELECT RecID ,
                                       MediaID ,
                                       MediaType ,
                                       RelationType ,
	                                   [Status],
                                       CreateTime ,
                                       CreateUserID
	                                   FROM DBO.Media_CollectionBlacklist WITH(NOLOCK)
	                            WHERE Status = 0 ";
            var paras = new List<SqlParameter>();

            if (query.MediaType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MediaType = @MediaType";
                paras.Add(new SqlParameter("@MediaType", query.MediaType));
            }
            if (query.MediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MediaId = @MediaId";
                paras.Add(new SqlParameter("@MediaId", query.MediaId));
            }
            if (query.RelationType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND RelationType = @RelationType";
                paras.Add(new SqlParameter("@RelationType", query.RelationType));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Media.MediaCollectionBlacklist>(data.Tables[0]);
        }

        public int Delete(int mediaId, MediaType mediaType, CollectPullBackTypeEnum relationType, int userId)
        {
            const string sql = @"
                             DECLARE @RecId INT = 0

                             SELECT TOP 1
                                    @RecId = RecID
                             FROM   dbo.Media_CollectionBlacklist
                             WHERE  MediaID = @MediaID
                                    AND MediaType = @MediaType
                                    AND RelationType = @RelationType
                                    AND CreateUserID = @CreateUserID
                                    AND Status = 0

                             IF ( @RecId > 0 )
                                BEGIN
                                    UPDATE  dbo.Media_CollectionBlacklist
                                    SET     Status = @Status
                                    WHERE   RecID = @RecId
                                    SELECT  1
                                END
                             ELSE
                                BEGIN
                                    SELECT  0
                                END ";

            var paras = new List<SqlParameter>()
            {
                //new SqlParameter("@RecID", recId),
                 new SqlParameter("@MediaID", mediaId),
                 new SqlParameter("@MediaType",(int) mediaType),
                 new SqlParameter("@RelationType", (int)relationType),
                  new SqlParameter("@CreateUserID",userId),
                 new SqlParameter("@Status", (int)DataStatusEnum.Delete)
            };
            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());

            return Convert.ToInt32(data);
        }

        #region 1.1.4

        /// <summary>
        /// 2017-06-08 zlb
        /// 查询自己的拉黑或收藏媒体列表
        /// </summary>
        /// <param name="SelectType">1收藏 2拉黑</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public DataTable SelectCollectionPullBlack(int SelectType, int UserID, int PageIndex, int pageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@pageSize", SqlDbType.Int),
                    new SqlParameter("@operatetype", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int),
                    };
            parameters[0].Value = PageIndex;
            parameters[1].Value = pageSize;
            parameters[2].Value = SelectType;
            parameters[3].Value = UserID;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectCollectionPullBlack", parameters);
            int totalCount = (int)(parameters[4].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            return ds.Tables[0];
        }

        #endregion 1.1.4
    }
}