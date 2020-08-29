using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaVideo : DataBase
    {
        #region Instance

        public static readonly MediaVideo Instance = new MediaVideo();

        #endregion Instance

        public Entities.Media.MediaVideo GetEntity(int mediaId)
        {
            const string sql = @"SELECT [MediaID]
                              ,[Platform]
                              ,[Number]
                              ,[Name]
                              ,[HeadIconURL]
                              ,[Sex]
                              ,[FansCount]
                              ,[FansCountURL]
                              ,[CategoryID]
                              ,[Profession]
                              ,[AuthType]
                              ,[LevelType]
                              ,[ProvinceID]
                              ,[CityID]
                              ,[IsReserve]
                              ,[Status],[Source]
                              ,[CreateTime]
                              ,[CreateUserID]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserID]
                          FROM [dbo].[Media_Video] WITH(NOLOCK) WHERE Status = 0 AND MediaID = @MediaID";
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaVideo>(data.Tables[0]);
        }

        public Entities.Media.MediaVideo GetEntity(string number, int platform, int filterMediaId = 0)
        {
            string sql = @"SELECT TOP 1  [MediaID]
                              ,[Platform]
                              ,[Number]
                              ,[Name]
                              ,[HeadIconURL]
                              ,[Sex]
                              ,[FansCount]
                              ,[FansCountURL]
                              ,[CategoryID]
                              ,[Profession]
                              ,[AuthType]
                              ,[LevelType]
                              ,[ProvinceID]
                              ,[CityID]
                              ,[IsReserve]
                               ,[Status],[Source]
                              ,[CreateTime]
                              ,[CreateUserID]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserID]
                          FROM [dbo].[Media_Video] WITH(NOLOCK) WHERE Status = 0 AND Platform = @Platform AND Number = @Number";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@Platform", platform),
                new SqlParameter("@Number", number)
            };
            if (filterMediaId > 0)
            {
                sql += " AND MediaID != @MediaID";
                paras.Add(new SqlParameter("@MediaID", filterMediaId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaVideo>(data.Tables[0]);
        }

        public int Insert(Entities.Media.MediaVideo entity)
        {
            var strSql = @"
            INSERT INTO [Media_Video]
                ([Platform],[Number],[Name],[HeadIconURL],[Sex],[FansCount],[FansCountURL],[CategoryID],[Profession],[AuthType]
              ,[LevelType],[ProvinceID] ,[CityID],[IsReserve],[Status],[Source],[CreateTime],[CreateUserID],[LastUpdateTime],[LastUpdateUserID])
            VALUES(@Platform, @Number, @Name, @HeadIconURL, @Sex, @FansCount,@FansCountURL,@CategoryID,@Profession,@AuthType,
                @LevelType,@ProvinceID,@CityID,@IsReserve,@Status,@Source,GETDATE(),@CreateUserID,GETDATE(),@LastUpdateUserID);
            SELECT @@IDENTITY
            ";

            var parameters = new SqlParameter[]{
                        new SqlParameter("@Platform",entity.Platform),
                        new SqlParameter("@Number",entity.Number),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@Sex",entity.Sex),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@Profession",entity.Profession),
                        new SqlParameter("@AuthType",entity.AuthType),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Media.MediaVideo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_Video]");
            strSql.Append(@"SET [Platform] = @Platform
                              --,[Number] = @Number
                              ,[Name] = @Name
                              ,[HeadIconURL] = @HeadIconURL
                              ,[Sex] = @Sex
                              ,[FansCount] = @FansCount
                              ,[FansCountURL] = @FansCountURL
                              ,[CategoryID] = @CategoryID
                              ,[Profession] = @Profession
                              ,[AuthType] = @AuthType
                              ,[LevelType] = @LevelType
                              ,[ProvinceID] = @ProvinceID
                              ,[CityID] =@CityID
                              ,[IsReserve] =@IsReserve
                              ,[Status] = @Status
                              --,[CreateTime] = @CreateTime
                              --,[CreateUserID] = @CreateUserID
                              ,[LastUpdateTime] = @LastUpdateTime
                              ,[LastUpdateUserID] = @LastUpdateUserID
                        WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                 new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@Platform",entity.Platform),
            			//new SqlParameter("@Number",entity.Number),
            			new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@Sex",entity.Sex),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@Profession",entity.Profession),
                        new SqlParameter("@AuthType",entity.AuthType),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@Status",entity.Status),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 存在就编辑，否则就添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Operate(Entities.Media.MediaVideo entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Platform < 1 || userId <= 0)
            {
                throw new Exception("媒体-视频-请输入Name，Platform,创建人id");
            }
            var info = GetEntity(entity.Number, entity.Platform);
            if (info == null)
            {
                return Insert(entity);
            }
            if (entity.CreateUserID == userId)
            {
                entity.MediaID = info.MediaID;
                return Update(entity) > 0 ? entity.MediaID : 0;
            }
            return Insert(entity);
        }

        public List<Entities.Media.MediaVideo> GetList(int platform, string name, string number, int source, string createUser, string beginDate, string endDate, string rightSql,
 int categoryId, string orderby, int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            SqlParameter[] parameters = {
                new SqlParameter("@Platform", SqlDbType.Int),
                new SqlParameter("@Name", SqlDbType.VarChar),
                new SqlParameter("@Number", SqlDbType.VarChar),
                new SqlParameter("@Source", SqlDbType.Int),
                new SqlParameter("@CreateUser", SqlDbType.VarChar),
                new SqlParameter("@BeginDate", SqlDbType.VarChar),
                new SqlParameter("@EndDate", SqlDbType.VarChar),
                new SqlParameter("@RightSql",SqlDbType.VarChar),
                new SqlParameter("@PageIndex", SqlDbType.Int),
                new SqlParameter("@PageSize", SqlDbType.Int),
                new SqlParameter("@TotalCount", SqlDbType.Int),
                new SqlParameter("@Orderby", SqlDbType.NVarChar),
                new SqlParameter("@CategoryId", SqlDbType.Int),
            };
            parameters[0].Value = platform;
            parameters[1].Value = SqlFilter(name);
            parameters[2].Value = SqlFilter(number);
            parameters[3].Value = source;
            parameters[4].Value = SqlFilter(createUser);
            parameters[5].Value = SqlFilter(beginDate);
            parameters[6].Value = SqlFilter(endDate);
            parameters[7].Value = rightSql;
            parameters[8].Value = pageIndex;
            parameters[9].Value = pageSize;
            parameters[10].Direction = ParameterDirection.Output;
            parameters[11].Value = orderby;
            parameters[12].Value = categoryId;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Video_Select", parameters).Tables[0];
            List<Entities.Media.MediaVideo> list = new List<Entities.Media.MediaVideo>();

            #region 填充

            totalCount = Convert.ToInt32(parameters[10].Value);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Entities.Media.MediaVideo()
                {
                    MediaID = Convert.ToInt32(dr["MediaID"]),
                    Name = dr["Name"].ToString(),
                    Number = dr["Number"].ToString(),
                    HeadIconURL = dr["HeadIconURL"].ToString(),
                    FansCount = dr["FansCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FansCount"]),
                    CityID = dr["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CityID"]),
                    LevelType = dr["LevelType"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LevelType"]),
                    CreateTime = dr["CreateTime"] == DBNull.Value ? new DateTime(1900, 01, 01) : Convert.ToDateTime(dr["CreateTime"]),
                    LevelTypeName = dr["LevelTypeName"].ToString(),
                    TrueName = dr["TrueName"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    SourceName = dr["SourceName"].ToString(),
                    PlatformName = dr["PlatformName"].ToString(),
                    PubCount = Convert.ToInt32(dr["PubCount"]),
                    Status = Convert.ToInt32(dr["AuditStatus"]),
                    PubID = dr["PubID"].ToString(),
                    CategoryName = dr["Category"].ToString(),
                    CanAddToRecommend = Convert.ToBoolean(dr["CanAddToRecommend"]),//
                    IsRange = Convert.ToBoolean(dr["IsRange"])
                });
            }

            #endregion 填充

            return list;
        }

        public MediaVideoDTO GetDetail(int mediaID, string rightSql)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MediaID", SqlDbType.Int),
                new SqlParameter("@RightSql",SqlDbType.VarChar)
            };
            parameters[0].Value = mediaID;
            parameters[1].Value = rightSql;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Video_Detail", parameters);
            DataTable dt = ds.Tables[0];
            string orderRemarkName = string.Empty;
            MediaVideoDTO dto = new MediaVideoDTO();
            dto.MediaInfo = DataTableToEntity<Entities.Media.MediaVideo>(dt);
            if (dto.MediaInfo != null)
            {
                dto.InteractionInfo = DataTableToEntity<Entities.Interaction.InteractionVideo>(dt);
                dto.UserInfo = DataTableToEntity<Entities.DTO.UserInfoDTO>(dt);
                //覆盖区域
                dto.OverlayIDs = new List<ProvinceCityDTO>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    dto.OverlayIDs.Add(new ProvinceCityDTO()
                    {
                        ProvinceID = dr["ProvinceID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProvinceID"]),
                        CityID = dr["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CityID"]),
                        ProvinceName = dr["ProvinceName"].ToString(),
                        CityName = dr["CityName"].ToString()
                    });
                }
            }
            return dto;
        }

        public Entities.Media.MediaVideo GetBaseEntity(int mediaId)
        {
            var sql = @"SELECT TOP 1
                                MV.*
                        FROM    dbo.Media_Video AS MV WITH ( NOLOCK )
                                INNER JOIN dbo.Publish_BasicInfo AS A WITH ( NOLOCK ) ON MV.MediaID = A.MediaID
                                INNER JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = A.PubID
                        WHERE   MV.MediaID = {0}
                                AND A.MediaType = {1}
                                AND PD.PublishStatus = {2}
                                AND GETDATE() BETWEEN A.BeginTime AND A.EndTime";
            var paras = new List<SqlParameter>();
            sql = string.Format(sql, mediaId, (int)MediaType.Video, (int)EnumPublishStatus.OnSold);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaVideo>(data.Tables[0]);
        }
    }
}