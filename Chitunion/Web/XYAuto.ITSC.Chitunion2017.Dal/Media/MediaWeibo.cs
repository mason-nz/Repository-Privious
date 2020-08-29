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
    public class MediaWeibo : DataBase
    {
        #region Instance

        public static readonly MediaWeibo Instance = new MediaWeibo();

        #endregion Instance

        public Entities.Media.MediaWeibo GetEntity(int mediaId)
        {
            const string sql = @"SELECT [MediaID]
                              ,[Number]
                              ,[Name]
                              ,[Sex]
                              ,[HeadIconURL]
                              ,[FansCount]
                              ,[FansCountURL]
                              ,[FansSex]
                              ,[CategoryID]
                              ,[AreaID]
                              ,[Profession]
                              ,[ProvinceID]
                              ,[CityID]
                              ,[LevelType]
                              ,[AuthType]
                              ,[Sign]
                              ,[OrderRemark]
                              ,[IsReserve]
                              ,[Status]
                              ,[Source]
                              ,[CreateTime]
                              ,[CreateUserID]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserID]
                          FROM [dbo].[Media_Weibo] WITH(NOLOCK) WHERE Status = 0 AND MediaID = @MediaID";
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeibo>(data.Tables[0]);
        }

        public Entities.Media.MediaWeibo GetEntity(string number, string name, int filterMediaId = 0)
        {
            string sql = @"SELECT TOP 1 [MediaID]
                              ,[Number]
                              ,[Name]
                              ,[Sex]
                              ,[HeadIconURL]
                              ,[FansCount]
                              ,[FansCountURL]
                              ,[FansSex]
                              ,[CategoryID]
                              ,[AreaID]
                              ,[Profession]
                              ,[ProvinceID]
                              ,[CityID]
                              ,[LevelType]
                              ,[AuthType]
                              ,[Sign]
                              ,[OrderRemark]
                              ,[IsReserve]
                              ,[Status]
                              ,[Source]
                              ,[CreateTime]
                              ,[CreateUserID]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserID]
                          FROM [dbo].[Media_Weibo] WITH(NOLOCK) WHERE Status = 0 ";
            var paras = new List<SqlParameter>() { };
            if (!string.IsNullOrWhiteSpace(number))
            {
                sql += " AND Number = @Number";
                paras.Add(new SqlParameter("@Number", number));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " AND Name = @Name";
                paras.Add(new SqlParameter("@Name", name));
            }
            if (filterMediaId > 0)
            {
                sql += " AND MediaID != @MediaID";
                paras.Add(new SqlParameter("@MediaID", filterMediaId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeibo>(data.Tables[0]);
        }

        public int Insert(Entities.Media.MediaWeibo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into dbo.Media_Weibo(");
            strSql.Append("Number,Name,Sex,HeadIconURL,FansCount,FansCountURL,FansSex,CategoryID,AreaID,Profession,ProvinceID,CityID,LevelType,AuthType,Sign,OrderRemark,IsReserve,Status,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
            strSql.Append(") values (");
            strSql.Append("@Number,@Name,@Sex,@HeadIconURL,@FansCount,@FansCountURL,@FansSex,@CategoryID,@AreaID,@Profession,@ProvinceID,@CityID,@LevelType,@AuthType,@Sign,@OrderRemark,@IsReserve,@Status,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Number",entity.Number),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@Sex",entity.Sex),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@FansSex",entity.FansSex),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@AreaID",entity.AreaID),
                        new SqlParameter("@Profession",entity.Profession),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@AuthType",entity.AuthType),
                        new SqlParameter("@Sign",entity.Sign),
                        new SqlParameter("@OrderRemark",entity.OrderRemark),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Media.MediaWeibo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_Weibo]");
            strSql.Append(@"SET
                            --[Number] = @Number,
                          [Name] = @Name
                          ,[Sex] = @Sex
                          ,[HeadIconURL] = @HeadIconURL
                          ,[FansCount] = @FansCount
                          ,[FansCountURL] = @FansCountURL
                          ,[FansSex] = @FansSex
                          ,[CategoryID] = @CategoryID
                          ,[AreaID] =@AreaID
                          ,[Profession] = @Profession
                          ,[ProvinceID] = @ProvinceID
                          ,[CityID] = @CityID
                          ,[LevelType] =@LevelType
                          ,[AuthType] = @AuthType
                          ,[Sign] = @Sign
                          ,[OrderRemark] = @OrderRemark
                          ,[IsReserve] = @IsReserve
                          ,[Status] = @Status
                          ,[Source] = @Source
                      --,[CreateTime] = @CreateTime
                      --,[CreateUserID] = @CreateUserID
                      ,[LastUpdateTime] = @LastUpdateTime
                      ,[LastUpdateUserID] = @LastUpdateUserID  WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                 new SqlParameter("@MediaID",entity.MediaID),
						//new SqlParameter("@Number",entity.Number),
            			new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@Sex",entity.Sex),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@FansSex",entity.FansSex),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@AreaID",entity.AreaID),
                        new SqlParameter("@Profession",entity.Profession),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@AuthType",entity.AuthType),
                        new SqlParameter("@Sign",entity.Sign),
                        new SqlParameter("@OrderRemark",entity.OrderRemark),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Source",entity.Source),
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
        public int Operate(Entities.Media.MediaWeibo entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || userId <= 0)
            {
                throw new Exception("媒体-weibo-请输入Name创建人id");
            }
            var info = GetEntity(entity.Number, string.Empty);
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

        public List<Entities.Media.MediaWeibo> GetList(string name, string number, int source, string createUser, string beginDate, string endDate, string rightSql,
               int categoryId, int levelType, int isAuth, string orderby,
            int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            SqlParameter[] parameters = {
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
                new SqlParameter("@CategoryId", SqlDbType.Int),
                new SqlParameter("@LevelType", SqlDbType.Int),
                new SqlParameter("@IsAuth", SqlDbType.Int),
                new SqlParameter("@Orderby", SqlDbType.NVarChar)
            };
            parameters[0].Value = SqlFilter(name);
            parameters[1].Value = SqlFilter(number);
            parameters[2].Value = source;
            parameters[3].Value = SqlFilter(createUser);
            parameters[4].Value = SqlFilter(beginDate);
            parameters[5].Value = SqlFilter(endDate);
            parameters[6].Value = rightSql;
            parameters[7].Value = pageIndex;
            parameters[8].Value = pageSize;
            parameters[9].Direction = ParameterDirection.Output;
            parameters[10].Value = categoryId;
            parameters[11].Value = levelType;
            parameters[12].Value = isAuth;
            parameters[13].Value = orderby;

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Weibo_Select", parameters).Tables[0];
            List<Entities.Media.MediaWeibo> list = new List<Entities.Media.MediaWeibo>();

            #region 填充

            totalCount = Convert.ToInt32(parameters[9].Value);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Entities.Media.MediaWeibo()
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
                    PubCount = Convert.ToInt32(dr["PubCount"]),
                    Status = Convert.ToInt32(dr["AuditStatus"]),
                    PubID = dr["PubID"].ToString(),
                    CategoryName = dr["Category"].ToString(),
                    CanAddToRecommend = Convert.ToBoolean(dr["CanAddToRecommend"]),
                    IsRange = Convert.ToBoolean(dr["IsRange"])
                });
            }

            #endregion 填充

            return list;
        }

        public MediaWeiboDTO GetDetail(int mediaID, string rightSql)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MediaID", SqlDbType.Int),
                new SqlParameter("@RightSql",SqlDbType.VarChar)
            };
            parameters[0].Value = mediaID;
            parameters[1].Value = rightSql;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Weibo_Detail", parameters);
            DataTable dt = ds.Tables[0];
            MediaWeiboDTO dto = new MediaWeiboDTO();
            dto.MediaInfo = DataTableToEntity<Entities.Media.MediaWeibo>(dt);
            if (dto.MediaInfo != null)
            {
                dto.InteractionInfo = DataTableToEntity<Entities.Interaction.InteractionWeibo>(dt);
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
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    dto.MediaInfo.OrderRemarkName = ds.Tables[2].Rows[0]["OrderRemarkName"] == DBNull.Value ? string.Empty : ds.Tables[2].Rows[0]["OrderRemarkName"].ToString();
            }
            return dto;
        }

        public Entities.Media.MediaWeibo GetBaseEntity(int mediaId)
        {
            var sql = @"SELECT TOP 1
                                MW.*
                        FROM    dbo.Media_Weibo AS MW WITH ( NOLOCK )
                                INNER JOIN dbo.Publish_BasicInfo AS A WITH ( NOLOCK ) ON MW.MediaID = A.MediaID
                                INNER JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = A.PubID
                        WHERE   MW.MediaID = {0}
                                AND A.MediaType = {1}
                                AND PD.PublishStatus = {2}
                                AND GETDATE() BETWEEN A.BeginTime AND A.EndTime";
            var paras = new List<SqlParameter>();
            sql = string.Format(sql, mediaId, (int)MediaType.WeiBo, (int)EnumPublishStatus.OnSold);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeibo>(data.Tables[0]);
        }
    }
}