using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Recommend
{
    public class HomeMedia : DataBase
    {
        #region Instance

        public static readonly HomeMedia Instance = new HomeMedia();

        #endregion Instance

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Recommend.HomeMedia model)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Home_Media(");
            strSql.Append("CategoryID,ADDetailID,TemplateID,MediaID,MediaType,PublishState,SortNumber,ImageUrl,VideoUrl,CreateUserId,CreateTime");
            strSql.Append(") values (");
            strSql.Append("@CategoryID,@ADDetailID,@TemplateID,@MediaID,@MediaType,@PublishState,@SortNumber,@ImageUrl,@VideoUrl,@CreateUserId,getdate()");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@CategoryID",model.CategoryID),
                        new SqlParameter("@ADDetailID",model.ADDetailID),
                        new SqlParameter("@TemplateID",model.TemplateID),
                        new SqlParameter("@MediaID",model.MediaID),
                        new SqlParameter("@MediaType",model.MediaType),
                        new SqlParameter("@PublishState",model.PublishState),
                        new SqlParameter("@SortNumber",model.SortNumber),
                        new SqlParameter("@ImageUrl",model.ImageUrl),
                        new SqlParameter("@VideoUrl",model.VideoUrl),
                        new SqlParameter("@CreateUserId",model.CreateUserId)
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Recommend.HomeMedia model)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE  [dbo].[Home_Media]");
            strSql.Append(@"SET     SortNumber = @SortNumber ,
                                    ImageUrl = @ImageUrl ,
                                    VideoUrl = @VideoUrl
                            WHERE   RecID = @RecID");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",model.RecID),
                        new SqlParameter("@SortNumber",model.SortNumber),
                        new SqlParameter("@ImageUrl",model.ImageUrl),
                        new SqlParameter("@VideoUrl",model.VideoUrl)
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public int UpdatePublishState(int mediaType, HomePublishStateEnum state)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE  [dbo].[Home_Media]");
            strSql.Append(@"SET     PublishState = @PublishState
                            WHERE   MediaType = @MediaType");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",mediaType),
                        new SqlParameter("@PublishState",(int)state)
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public Entities.Recommend.HomeMedia GetEntityByApp(int adDetailId, int mediaType, int templateId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"    SELECT top 1  RecID ,
                                CategoryID ,
                                MediaID ,TemplateID,
                                MediaType ,
                                PublishState ,
                                SortNumber ,
                                ImageUrl ,
                                VideoUrl ,
                                CreateUserId ,
                                CreateTime
                        FROM    [dbo].[Home_Media] WITH(NOLOCK)
                        WHERE  MediaType =@MediaType
                            ");
            var parameters = new List<SqlParameter> { new SqlParameter("@MediaType", mediaType) };
            if (adDetailId > 0)
            {
                strSql.Append(" AND ADDetailID = @ADDetailID");
                parameters.Add(new SqlParameter("@ADDetailID", adDetailId));
            }
            if (templateId > 0)
            {
                strSql.Append(" AND TemplateID = @TemplateID");
                parameters.Add(new SqlParameter("@TemplateID", templateId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters.ToArray());
            return DataTableToEntity<Entities.Recommend.HomeMedia>(data.Tables[0]);
        }

        public List<Entities.HomeCategoryModle> GetEntityByMediaType(int mediaType)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"   SELECT  HC.*,HM.MediaID
                                FROM    dbo.Home_Category HC WITH ( NOLOCK )
                                        LEFT JOIN dbo.Home_Media AS HM WITH ( NOLOCK ) ON HM.MediaType = HC.MediaType
                                WHERE   HC.MediaType = @MediaType
                            ");
            var parameters = new SqlParameter[]{
                            new SqlParameter("@MediaType",mediaType)
                        };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters.ToArray());
            return DataTableToList<Entities.HomeCategoryModle>(data.Tables[0]);
        }

        public Entities.Recommend.HomeMedia GetEntity(int mediaId, int mediaType)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"    SELECT top 1  RecID ,
                                CategoryID ,
                                MediaID ,
                                MediaType ,
                                PublishState ,
                                SortNumber ,
                                ImageUrl ,
                                VideoUrl ,
                                CreateUserId ,
                                CreateTime
                        FROM    [dbo].[Home_Media] WITH(NOLOCK)
                        WHERE MediaID =@MediaID AND MediaType =@MediaType
                            ");
            var parameters = new SqlParameter[]{
                          new SqlParameter("@MediaID",mediaId),
                            new SqlParameter("@MediaType",mediaType)
                        };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters.ToArray());
            return DataTableToEntity<Entities.Recommend.HomeMedia>(data.Tables[0]);
        }

        public int GetSortNumber(int mediaType, int categoryId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  ISNULL(MAX(SortNumber) + 1,1)
                            FROM    dbo.Home_Media WITH ( NOLOCK )
                            WHERE   MediaType = @MediaType AND CategoryID = @CategoryID
                            ");
            var parameters = new SqlParameter[]{
                      new SqlParameter("@CategoryID",categoryId),
                        new SqlParameter("@MediaType",mediaType)
                        };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int GetSortNumberApp(int categoryId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  ISNULL(MAX(SortNumber) + 1,1)
                            FROM    dbo.Home_Media WITH ( NOLOCK )
                            WHERE   MediaType = @MediaType
                            ");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",(int)MediaType.APP)
                        };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int GetEntityByMediaTypeApp()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT COUNT(1) FROM DBO.Home_Media WITH(NOLOCK)
                            WHERE   MediaType = @MediaType
                            ");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",(int)MediaType.APP)
                        };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public Entities.Recommend.HomeMedia GetEntity(int recId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"    SELECT  RecID ,
                                CategoryID ,
                                MediaID ,
                                MediaType ,
                                PublishState ,
                                SortNumber ,
                                ImageUrl ,
                                VideoUrl ,
                                CreateUserId ,
                                CreateTime
                        FROM    [dbo].[Home_Media] WITH(NOLOCK)
                        WHERE RecID = @RecID
                            ");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",recId)
                        };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters.ToArray());
            return DataTableToEntity<Entities.Recommend.HomeMedia>(data.Tables[0]);
        }

        /// <summary>
        /// 校验媒体推荐列表分类中是否满足数量
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="filterCount"></param>
        /// <returns></returns>
        public List<Entities.DTO.HomeMediaDto> GetFilterCount(int mediaType, int filterCount)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"    SELECT  HC.CategoryID ,
                                        HC.CategoryName ,HC.RecID,
                                        COUNT(HM.CategoryID) AS TotleCount
                                FROM    dbo.Home_Category HC WITH ( NOLOCK )
                                        LEFT JOIN dbo.Home_Media AS HM WITH ( NOLOCK ) ON HM.CategoryID = HC.CategoryID
		                                AND HM.MediaType = HC.MediaType
                                WHERE   HC.MediaType = @MediaType
                                GROUP BY HC.CategoryID ,HC.RecID,
                                        HC.CategoryName
                                HAVING  COUNT(HC.CategoryID) < @filterCount
                                ORDER BY HC.RecID
                            ");
            var parameters = new SqlParameter[]{
                       new SqlParameter("@MediaType",mediaType),
                        new SqlParameter("@filterCount",filterCount)
                        };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters.ToArray());
            return DataTableToList<Entities.DTO.HomeMediaDto>(data.Tables[0]);
        }

        public int GetAppFilterCount(int filterCount)
        {
            var strSql = new StringBuilder();
            strSql.Append(@" SELECT COUNT(1) AS TotleCount FROM DBO.Home_Media WITH(NOLOCK)
                            WHERE MediaType = @MediaType
                            HAVING COUNT(1) < @filterCount");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",(int)MediaType.APP),
                            new SqlParameter("@filterCount",filterCount)
                        };
            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters.ToArray());
            return Convert.ToInt32(data);
        }

        public int Delete(int recId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"DELETE FROM [dbo].[Home_Media] WHERE RecID = @RecID");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",recId)
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }
    }
}