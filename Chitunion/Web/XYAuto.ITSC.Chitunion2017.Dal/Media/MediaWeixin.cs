using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaWeixin : DataBase
    {
        #region Instance

        public static readonly MediaWeixin Instance = new MediaWeixin();

        #endregion Instance

        public Entities.Media.MediaWeixin GetEntity(int mediaId)
        {
            string sql = @"SELECT [MediaID]
                          ,[Number]
                          ,[Name]
                          ,[HeadIconURL]
                          ,[TwoCodeURL]
                          ,[FansCount]
                          ,[FansCountURL]
                          ,[FansMalePer]
                          ,[FansFemalePer]
                          ,[CategoryID]
                          ,[ProvinceID]
                          ,[CityID]
                          ,[Sign]
                          ,[AreaID]
                          ,[LevelType]
                          ,[IsAuth]
                          ,[OrderRemark]
                          ,[IsReserve],[IsAreaMedia]
                          ,[Status],[Source],[PublishStatus],[AuditStatus],[AuthType]
                            ,[FansSexScaleUrl],[FansAreaShotUrl]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                            ,[ADName],[WxID]
                            ,OrderRemarkStr=(
                                SELECT  STUFF(( SELECT  '|'
                                                        + RTRIM(( CAST(PRK.RemarkID AS VARCHAR(15))
                                                                  + ',' + ISNULL(DI.DictName, '')
                                                                  + ',' + ISNULL(PRK.OtherContent,
                                                                                 '') ))
                                                FROM    dbo.Publish_Remark AS PRK WITH ( NOLOCK )
                                                        LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON PRK.RemarkID = DI.DictId
                                                WHERE   RelationID = Media_Weixin.MediaID
                                                        AND PRK.EnumType = {0}
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '')

		                    )
                            ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                               '') + '@=' + ISNULL(( AI2.AreaID
                                                                                  + ','
                                                                                  + ISNULL(AI2.AreaName,
                                                                                  '') ), ''))
                                    FROM    dbo.Media_Area_Mapping AS MAM WITH ( NOLOCK )
                                            LEFT JOIN dbo.AreaInfo AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAM.ProvinceID
                                            LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAM.CityID
                                    WHERE   MAM.MediaType = 14001
                                            AND MAM.MediaID = [Media_Weixin].MediaID
											AND MAM.RelateType = {1}
                                  FOR
                                    XML PATH('')
                                  ), 1, 1, '')
                                          )
                      FROM [dbo].[Media_Weixin] WITH(NOLOCK)
                                WHERE Status = 0 AND MediaID = @MediaID";

            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        public Entities.Media.MediaWeixin GetNormalEntityByWxID(int wxID, int userID, bool IsAE)
        {
            string sql = string.Empty;
            if (!IsAE)
                sql = "select top 1 * from Media_Weixin where Status = 0 and WxID = " + wxID + " and CreateUserID = " + userID;
            else
                sql = "select top 1 * from Media_Weixin where Status = 0 and WxID = " + wxID + " and CreateUserID in (select UserID from UserRole Where RoleID = 'SYS001RL00005')";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToEntity<Entities.Media.MediaWeixin>(dt);
        }

        public List<Entities.Media.MediaWeixin> GetList(MediaQuery<Entities.Media.MediaWeixin> query)
        {
            var sql = @"SELECT TOP ({0}) [MediaID]
                          ,[Number]
                          ,[Name]
                          ,[HeadIconURL]
                          ,[TwoCodeURL]
                          ,[FansCount]
                          ,[FansCountURL]
                          ,[FansMalePer]
                          ,[FansFemalePer]
                          ,[CategoryID]
                          ,[ProvinceID]
                          ,[CityID]
                          ,[Sign]
                          ,[AreaID]
                          ,[LevelType]
                          ,[IsAuth]
                          ,[OrderRemark]
                          ,[IsReserve],[IsAreaMedia]
                          ,[Status],[Source],[PublishStatus],[AuditStatus],[AuthType]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                      FROM [dbo].[Media_Weixin] WITH(NOLOCK) WHERE Status = 0 ";
            sql = string.Format(sql, query.PageSize);
            var paras = new List<SqlParameter>();
            var orderBy = " MediaID DESC ";
            if (query.MediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MediaId = @MediaId";
                paras.Add(new SqlParameter("@MediaId", query.MediaId));
            }
            if (!string.IsNullOrWhiteSpace(query.Number))
            {
                sql += " AND Number = @Number";
                paras.Add(new SqlParameter("@Number", Utils.StringHelper.SqlFilter(query.Number)));
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                sql += " AND Name = @Name";
                paras.Add(new SqlParameter("@Name", Utils.StringHelper.SqlFilter(query.Name)));
            }
            if (query.CreateUserId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND CreateUserId = @CreateUserId";
                paras.Add(new SqlParameter("@CreateUserId", query.CreateUserId));
            }
            if (query.AuditStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND AuditStatus = @AuditStatus";
                paras.Add(new SqlParameter("@AuditStatus", query.AuditStatus));
            }
            if (query.PublishStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND PublishStatus = @PublishStatus";
                paras.Add(new SqlParameter("@PublishStatus", query.PublishStatus));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                orderBy = query.OrderBy;
            }

            sql += " ORDER BY " + orderBy;

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        public int VerifyMediaCountByRole(string wxNumber, string roleId)
        {
            var sql = @"SELECT  TOP 1 MW.MediaID
                        FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )
                        WHERE   Number = @Number AND Status = 0
                                AND EXISTS ( SELECT UserID
                                             FROM   dbo.UserRole
                                             WHERE  RoleID = @RoleID
                                                    AND Status = 0
                                                    AND MW.CreateUserID = UserID ) ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@Number", wxNumber),
                 new SqlParameter("@RoleID", roleId)
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray()));
        }

        public Entities.Media.MediaWeixin GetEntity(string number, string name = null, int filterMediaId = 0)
        {
            var sql = @"SELECT TOP 1 [MediaID]
                          ,[Number]
                          ,[Name]
                          ,[HeadIconURL]
                          ,[TwoCodeURL]
                          ,[FansCount]
                          ,[FansCountURL]
                          ,[FansMalePer]
                          ,[FansFemalePer]
                          ,[CategoryID]
                          ,[ProvinceID]
                          ,[CityID]
                          ,[Sign]
                          ,[AreaID]
                          ,[LevelType]
                          ,[IsAuth]
                          ,[OrderRemark]
                          ,[IsReserve],[IsAreaMedia]
              ,[Status],[Source],[PublishStatus],[AuditStatus],[AuthType]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                      FROM [dbo].[Media_Weixin] WITH(NOLOCK) WHERE Status = 0 ";
            var paras = new List<SqlParameter>();
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
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        /// <summary>
        /// 为了验证编辑媒体信息，是否存在重复的帐号或者名称
        /// </summary>
        /// <param name="number">帐号</param>
        /// <param name="name">名称</param>
        /// <param name="roleIds">角色Id</param>
        /// <param name="mediaTableName">媒体表名称</param>
        /// <param name="filterMediaId">需要过滤的媒体Id</param>
        /// <returns></returns>
        public int GetRepeatCount(string number, string name, List<string> roleIds,
            string mediaTableName = "Media_Weixin", int filterMediaId = 0)
        {
            var sql = string.Format(@"SELECT  COUNT(1)
                        FROM    dbo.{0} WITH ( NOLOCK )
                        WHERE   Status = 0 ", mediaTableName);
            var paras = new List<SqlParameter>();
            if (filterMediaId > 0)
            {
                sql += " AND MediaID != @MediaID";
                paras.Add(new SqlParameter("@MediaID", filterMediaId));
            }
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
            if (roleIds.Count > 0)
            {
                var userIdSql = new StringBuilder(" AND CreateUserID IN (");
                userIdSql.AppendFormat(@"SELECT DISTINCT
                                        ( UserID )
                              FROM      UserRole
                              WHERE     RoleID IN ('{0}'))", string.Join("','", roleIds));

                sql += userIdSql.ToString();
            }

            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray()));
        }

        public int Insert(Entities.Media.MediaWeixin entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_Weixin(");
            strSql.Append("Number,Name,HeadIconURL,TwoCodeURL,FansCount,FansCountURL,FansMalePer,FansFemalePer,CategoryID,ProvinceID,CityID,Sign,AreaID,LevelType,IsAuth,OrderRemark,IsReserve,IsAreaMedia,Status,Source," +
                          "PublishStatus,AuditStatus,FansSexScaleUrl,FansAreaShotUrl,AuthType,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,WxID,ADName");
            strSql.Append(") values (");
            strSql.Append("@Number,@Name,@HeadIconURL,@TwoCodeURL,@FansCount,@FansCountURL,@FansMalePer,@FansFemalePer,@CategoryID,@ProvinceID,@CityID,@Sign,@AreaID,@LevelType,@IsAuth,@OrderRemark,@IsReserve,@IsAreaMedia,@Status,@Source," +
                          "@PublishStatus,@AuditStatus,@FansSexScaleUrl,@FansAreaShotUrl,@AuthType,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID,@WxID,@ADName");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Number",entity.Number),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@TwoCodeURL",entity.TwoCodeURL),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@FansMalePer",entity.FansMalePer),
                        new SqlParameter("@FansFemalePer",entity.FansFemalePer),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@Sign",entity.Sign),
                        new SqlParameter("@AreaID",entity.AreaID),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@IsAuth",entity.IsAuth),
                        new SqlParameter("@OrderRemark",entity.OrderRemark),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@IsAreaMedia",entity.IsAreaMedia),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@PublishStatus",entity.PublishStatus),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        new SqlParameter("@FansSexScaleUrl",entity.FansSexScaleUrl),
                        new SqlParameter("@FansAreaShotUrl",entity.FansAreaShotUrl),
                        new SqlParameter("@AuthType",entity.AuthType),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        new SqlParameter("@WxID",entity.WxID),
                        new SqlParameter("@ADName",entity.ADName),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int UpdateWxId(int mediaId, int wxId)
        {
            var sql = @" UPDATE [dbo].[Media_Weixin] SET WxID = @WxID  WHERE MediaID = @MediaID";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("@MediaID",mediaId),
                  new SqlParameter("@WxID",wxId),
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int Update(Entities.Media.MediaWeixin entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_Weixin]");
            strSql.Append(@"SET
                        --[Number] = @Number,
                        [Name] = @Name
                      ,[HeadIconURL] = @HeadIconURL
                      ,[TwoCodeURL] = @TwoCodeURL
                      ,[FansCount] = @FansCount
                      ,[FansCountURL] = @FansCountURL
                      ,[FansMalePer] = @FansMalePer
                      ,[FansFemalePer] = @FansFemalePer
                      ,[CategoryID] = @CategoryID
                      ,[ProvinceID] = @ProvinceID
                      ,[CityID] = @CityID
                      ,[Sign] = @Sign
                      ,[AreaID] = @AreaID
                      ,[LevelType] = @LevelType
                      ,[IsAuth] =@IsAuth
                      ,[OrderRemark] = @OrderRemark
                      ,[IsReserve] = @IsReserve
                      ,[IsAreaMedia] = @IsAreaMedia
                      ,[FansSexScaleUrl] = @FansSexScaleUrl
                        ,[FansAreaShotUrl] = @FansAreaShotUrl
                        ,[AuthType] = @AuthType
                      ,[Status] = @Status
                        ,[PublishStatus] = @PublishStatus
                        ,[AuditStatus] = @AuditStatus
                      --,[CreateTime] = @CreateTime
                      --,[CreateUserID] = @CreateUserID
                      ,[LastUpdateTime] = @LastUpdateTime
                      ,[LastUpdateUserID] = @LastUpdateUserID
                      ,[ADName] = @ADName
                      WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                 new SqlParameter("@MediaID",entity.MediaID),
						//new SqlParameter("@Number",entity.Number),
            			new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@TwoCodeURL",entity.TwoCodeURL),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@FansMalePer",entity.FansMalePer),
                        new SqlParameter("@FansFemalePer",entity.FansFemalePer),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@Sign",entity.Sign),
                        new SqlParameter("@AreaID",entity.AreaID),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@IsAuth",entity.IsAuth),
                        new SqlParameter("@OrderRemark",entity.OrderRemark),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@IsAreaMedia",entity.IsAreaMedia),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@PublishStatus",entity.PublishStatus),
                         new SqlParameter("@AuditStatus",entity.AuditStatus),
                          new SqlParameter("@FansSexScaleUrl",entity.FansSexScaleUrl),
                        new SqlParameter("@FansAreaShotUrl",entity.FansAreaShotUrl),
                        new SqlParameter("@AuthType",entity.AuthType),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        new SqlParameter("@ADName",entity.ADName),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 存在就编辑，否则就添加( 当前用户如果已经存在添加媒体信息，则要修改)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int Operate(Entities.Media.MediaWeixin entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || userId <= 0)
            {
                throw new Exception("媒体-weixin-请输入Name,创建人id");
            }
            var info = GetEntity(entity.Number);
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

        /// <summary>
        /// 获取微信列表
        /// ls
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="source"></param>
        /// <param name="createUser"></param>
        /// <param name="createTime"></param>
        /// <param name="orderby"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="categoryId"></param>
        /// <param name="evelType"></param>
        /// <param name="isAuth"></param>
        /// <returns></returns>
        public List<Entities.Media.MediaWeixin> GetList(string name, string number, int source, string createUser, string beginDate, string endDate, string rightSql,
            int categoryId, int evelType, int isAuth, string orderby,
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
            parameters[11].Value = evelType;
            parameters[12].Value = isAuth;
            parameters[13].Value = orderby;

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Weixin_Select", parameters).Tables[0];
            List<Entities.Media.MediaWeixin> list = new List<Entities.Media.MediaWeixin>();

            #region 填充

            totalCount = Convert.ToInt32(parameters[9].Value);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Entities.Media.MediaWeixin()
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

        public MediaWeixinDTO GetDetail(int mediaID, string rightSql)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MediaID", SqlDbType.Int),
                new SqlParameter("@RightSql",SqlDbType.VarChar)
            };
            parameters[0].Value = mediaID;
            parameters[1].Value = rightSql;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Weixin_Detail", parameters);
            DataTable dt = ds.Tables[0];
            MediaWeixinDTO dto = new MediaWeixinDTO();
            dto.MediaInfo = DataTableToEntity<Entities.Media.MediaWeixin>(dt);
            if (dto.MediaInfo != null)
            {
                dto.InteractionInfo = DataTableToEntity<Entities.Interaction.InteractionWeixin>(dt);
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

        public Entities.Media.MediaWeixin GetBaseEntity(int mediaId)
        {
            var sql = @"SELECT TOP 1
                                MW.*
                        FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )
                                INNER JOIN dbo.Publish_BasicInfo AS A WITH ( NOLOCK ) ON MW.MediaID = A.MediaID
                                INNER JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = A.PubID
                        WHERE   A.MediaID = {0}
                                AND A.MediaType = {1}
                                AND PD.PublishStatus = {2}
                                AND GETDATE() BETWEEN A.BeginTime AND A.EndTime";
            var paras = new List<SqlParameter>();
            sql = string.Format(sql, mediaId, (int)MediaType.WeiXin, (int)EnumPublishStatus.OnSold);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        public Entities.Media.MediaWeixin GetInnerJoinEntity(int mediaId)
        {
            var sql = @"
                    SELECT  WO.RecID AS WxID,WO.WxNumber AS Number ,
                            WO.NickName AS Name ,
                            MW.FansMalePer ,
                            MW.FansFemalePer ,
                            WO.HeadImg AS [HeadIconURL] ,
                            WO.QrCodeUrl AS [TwoCodeURL] ,
                            WO.[FansCount] ,
                            MW.MediaID,
                            MW.[FansCountURL] ,
                            [FansMalePer] ,
                            [FansFemalePer] ,
                            [CategoryID] ,
                            MW.[ProvinceID] ,
                            MW.[CityID] ,
                            MW.[Sign] ,
                            MW.[AreaID] ,
                            MW.[LevelType] ,
                            MW.[IsAuth] ,
                            MW.[OrderRemark] ,
                            MW.[IsReserve] ,
                            MW.[Status] ,
                            MW.[Source] ,
                            MW.[PublishStatus] ,
                            MW.[AuditStatus] ,
                            MW.[AuthType] ,
                            [FansSexScaleUrl] ,
                            [FansAreaShotUrl]
                            ,WO.IsAreaMedia
                            ,OrderRemarkStr=(
                                        SELECT STUFF((SELECT  '|'
                                                                + RTRIM((CAST(PRKB.RemarkID AS VARCHAR(15))
                                                                          + ',' + ISNULL(DI.DictName, '')
                                                                          + ',' + ISNULL(PRKB.OtherContent,
                                                                                         '')))
                                                        FROM    dbo.Media_Remark_Basic AS PRKB WITH(NOLOCK)
                                                                LEFT JOIN dbo.DictInfo AS DI WITH(NOLOCK) ON PRKB.RemarkID = DI.DictId
                                                        WHERE   RelationID = WO.RecID
                                                                AND PRKB.EnumType = {0}
                                                        FOR
                                                          XML PATH('')
                                                      ), 1, 1, '')

		                    )
                            ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                                       '') + '@=' + ISNULL(( AI2.AreaID
                                                                                          + ','
                                                                                          + ISNULL(AI2.AreaName,
                                                                                          '') ), ''))
                                            FROM    dbo.Media_Area_Mapping_Basic AS MAPB WITH ( NOLOCK )
                                                    LEFT JOIN dbo.AreaInfo AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAPB.ProvinceID
                                                    LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAPB.CityID
                                            WHERE   MAPB.MediaType = 14001
                                                    AND MAPB.BaseMediaID = WO.RecID
                                                    AND MAPB.RelateType = {1}
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
                                                  )
                    FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )
                            INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID
                                                             --AND WO.Status = 0
                            WHERE MW.MediaID = @MediaID AND MW.Status = 0";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@MediaID",mediaId)
            };
            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        public RespWeiXinItemDto GetItem(int mediaId, int userID)
        {
            var sql = @"SELECT TOP 1 MW.MediaID ,WO.RecID AS WxId,WO.FansCount,
                                WO.WxNumber AS Number ,
                                WO.NickName AS Name ,
                                WO.HeadImg AS HeadIconURL,
		                        MW.FansMalePer,
		                        MW.FansFemalePer,MW.ADName,
                                WO.Summary ,--简介
                                WO.FullName ,--全称\主体
                                WO.EnterpriseType ,--企业类型
		                        WO.Location,--Location
		                        WO.RegTime,--注册时间
		                        WO.CreateTime,--创建时间
                                CommonlyClass = ( SELECT DC1.DictName + ','

                                                     FROM   dbo.MediaCategory AS MC WITH ( NOLOCK )
                                                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MC.CategoryID
                                                     WHERE  MC.WxID = WO.RecID
                                                            AND MC.MediaType = @MediaType
                                                            ORDER BY SortNumber DESC
                                                            FOR XML PATH('')
                                                   )
                                ,DC1.DictName AS ServiceTypeName --公众号类型
                                ,IsAddBackList=(
		                            SELECT  CASE WHEN COUNT(1) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END FROM DBO.Media_CollectionBlacklist AS MCBL1 WITH(NOLOCK)
		                                WHERE MCBL1.MediaID = MW.MediaID
					                            AND MCBL1.Status = 0
					                            AND MCBL1.RelationType = {0}
                                                AND MCBL1.CreateUserID = {2}
	                            )
	                            ,IsCollectionList=(
		                            SELECT CASE WHEN COUNT(1) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END FROM DBO.Media_CollectionBlacklist AS MCBL1 WITH(NOLOCK)
		                                WHERE MCBL1.MediaID = MW.MediaID
					                            AND MCBL1.Status = 0
					                            AND MCBL1.RelationType = {1}
                                                AND MCBL1.CreateUserID = {2}
	                            )
                                ,WO.IsAreaMedia
                                ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                                       '') + '@=' + ISNULL(( AI2.AreaID
                                                                                          + ','
                                                                                          + ISNULL(AI2.AreaName,
                                                                                          '') ), ''))
                                            FROM    dbo.Media_Area_Mapping_Basic AS MAPB WITH ( NOLOCK )
                                                    LEFT JOIN dbo.AreaInfo AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAPB.ProvinceID
                                                    LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAPB.CityID
                                            WHERE   MAPB.MediaType = 14001
                                                    AND MAPB.BaseMediaID = WO.RecID
                                                    AND MAPB.RelateType = {3}
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
                                                  )
                                ,SWML.MaLiIndex
                        FROM    dbo.Weixin_OAuth AS WO WITH ( NOLOCK )
                                INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON WO.RecID = MW.WxID
                                LEFT JOIN NLP2017.dbo.Stat_Weixin_MLIndex AS SWML WITH ( NOLOCK ) ON WO.WxNumber = SWML.WxNum AND LEN(SWML.WxNum) > 0
                                LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON WO.ServiceType = DC1.DictId
                        WHERE   MW.MediaID = @MediaID ";

            sql = string.Format(sql, (int)CollectPullBackTypeEnum.PullBack, (int)CollectPullBackTypeEnum.Collection, userID,
                (int)MediaAreaMappingType.AreaMedia);
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@MediaID",mediaId),
                new SqlParameter("@MediaType",(int)MediaType.WeiXin)
            };
            //CollectPullBackTypeEnum
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<RespWeiXinItemDto>(data.Tables[0]);
        }

        /// <summary>
        /// 后台查询媒体详情
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public Entities.Media.MediaWeixin GetItemForBack(int mediaId)
        {
            var sql = @"
                        SELECT  MW.*
                                ,CommonlyClassStr = ( SELECT DC1.DictName + ','
                                                 FROM   dbo.Media_CommonlyClass AS MCC WITH ( NOLOCK )
                                                        LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MCC.CategoryID
                                                 WHERE  MCC.MediaID = MW.MediaID
                                                        AND MCC.MediaType = @MediaType
								                        ORDER BY SortNumber DESC
								                        FOR XML PATH('')
                                               ),
					                           AI1.AreaName AS ProvinceName,
					                           AI2.AreaName AS CityName,
                                                DC1.DictName AS LevelTypeName
                                                ,OrderRemarkStr=(
                                                    SELECT  STUFF(( SELECT  '|'
                                                                            + RTRIM(( CAST(PRK.RemarkID AS VARCHAR(15))
                                                                                      + ',' + ISNULL(DI.DictName, '')
                                                                                      + ',' + ISNULL(PRK.OtherContent,
                                                                                                     '') ))
                                                                    FROM    dbo.Publish_Remark AS PRK WITH ( NOLOCK )
                                                                            LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON PRK.RemarkID = DI.DictId
                                                                    WHERE   RelationID = MW.MediaID
                                                                            AND PRK.EnumType = {0}
                                                                  FOR
                                                                    XML PATH('')
                                                                  ), 1, 1, '')

		                                        )
                                ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                               '') + '@=' + ISNULL(( AI2.AreaID
                                                                                  + ','
                                                                                  + ISNULL(AI2.AreaName,
                                                                                  '') ), ''))
                                    FROM    dbo.Media_Area_Mapping AS MAM WITH ( NOLOCK )
                                            LEFT JOIN dbo.AreaInfo AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAM.ProvinceID
                                            LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAM.CityID
                                    WHERE   MAM.MediaType = 14001
                                            AND MAM.MediaID = MW.MediaID
											AND MAM.RelateType = {1}
                                  FOR
                                    XML PATH('')
                                  ), 1, 1, '')
                                          )
                        FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )
                        LEFT JOIN DBO.AreaInfo AS AI1 WITH(NOLOCK) ON AI1.AreaID = MW.ProvinceID
                        LEFT JOIN DBO.AreaInfo AS AI2 WITH(NOLOCK) ON AI2.AreaID = MW.CityID
                        LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = MW.LevelType
                        WHERE   MW.MediaID = @MediaID ";
            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@MediaID",mediaId),
                new SqlParameter("@MediaType",(int)MediaType.WeiXin)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        /// <summary>
        /// 后台查询媒体详情（返回主表信息字段）
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public Entities.Media.MediaWeixin GetItemForBackBase(int mediaId)
        {
            var sql = @"
                        SELECT   MW.MediaID ,
                                WO.WxNumber AS Number ,
                                WO.NickName AS Name ,
                                WO.HeadImg AS HeadIconURL ,
                                WO.QrCodeUrl AS TwoCodeURL ,
                                WO.FansCount ,
                                MW.FansCountURL ,
                                MW.FansMalePer ,
                                MW.FansFemalePer ,
                                MW.CategoryID ,
                                MW.ProvinceID ,
                                MW.CityID ,
                                MW.Sign ,
                                MW.AreaID ,
                                MW.LevelType ,
                                MW.IsAuth ,
                                MW.OrderRemark ,
                                MW.IsReserve ,
                                MW.Source ,
                                MW.Status ,
                                MW.PublishStatus ,
                                MW.AuditStatus ,
                                MW.FansSexScaleUrl ,
                                MW.FansAreaShotUrl ,
                                MW.AuthType ,
                                MW.CreateTime ,
                                MW.CreateUserID ,
                                MW.LastUpdateTime ,
                                MW.LastUpdateUserID ,
                                MW.WxID ,
                                MW.ADName,
                                WO.IsAreaMedia
                                ,CommonlyClassStr = ( SELECT DC1.DictName + ','
                                                        FROM   dbo.MediaCategory AS MCC WITH ( NOLOCK )
                                                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MCC.CategoryID
                                                        WHERE  MCC.WxID = WO.RecID
                                                            AND MCC.MediaType = @MediaType
								                            ORDER BY SortNumber DESC
								                            FOR XML PATH('')
                                                ),
					                           AI1.AreaName AS ProvinceName,
					                           AI2.AreaName AS CityName,
                                                DC1.DictName AS LevelTypeName
                                ,OrderRemarkStr=(
                                        SELECT STUFF((SELECT  '|'
                                                                + RTRIM((CAST(PRKB.RemarkID AS VARCHAR(15))
                                                                          + ',' + ISNULL(DI.DictName, '')
                                                                          + ',' + ISNULL(PRKB.OtherContent,
                                                                                         '')))
                                                        FROM    dbo.Media_Remark_Basic AS PRKB WITH(NOLOCK)
                                                                LEFT JOIN dbo.DictInfo AS DI WITH(NOLOCK) ON PRKB.RemarkID = DI.DictId
                                                        WHERE   RelationID = WO.RecID
                                                                AND PRKB.EnumType = {0}
                                                        FOR
                                                          XML PATH('')
                                                      ), 1, 1, '')

		                        )
                                ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                                       '') + '@=' + ISNULL(( AI2.AreaID
                                                                                          + ','
                                                                                          + ISNULL(AI2.AreaName,
                                                                                          '') ), ''))
                                            FROM    dbo.Media_Area_Mapping_Basic AS MAPB WITH ( NOLOCK )
                                                    LEFT JOIN dbo.AreaInfo AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAPB.ProvinceID
                                                    LEFT JOIN dbo.AreaInfo AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAPB.CityID
                                            WHERE   MAPB.MediaType = 14001
                                                    AND MAPB.BaseMediaID = WO.RecID
                                                    AND MAPB.RelateType = {1}
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
                                                  )
                        FROM  DBO.Media_Weixin AS MW WITH(NOLOCK)
                        INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK )  ON MW.WxID = WO.RecID --AND WO.Status = 0
                        LEFT JOIN DBO.AreaInfo AS AI1 WITH(NOLOCK) ON AI1.AreaID = MW.ProvinceID
                        LEFT JOIN DBO.AreaInfo AS AI2 WITH(NOLOCK) ON AI2.AreaID = MW.CityID
                        LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = MW.LevelType
                        WHERE   MW.MediaID = @MediaID ";
            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@MediaID",mediaId),
                new SqlParameter("@MediaType",(int)MediaType.WeiXin)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        /// <summary>
        /// 获取媒体关联的媒体主信息
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public Entities.Media.MediaWeixin GetItemForMediaRole(int mediaId)
        {
            var sql = @"
                        SELECT TOP 1
                                    MW.* ,
                                    UI.UserName ,
                                    UI.Mobile ,
                                    UDI.TrueName,
		                            DC1.DictName AS SourceName
                             FROM   dbo.Media_Weixin AS MW WITH ( NOLOCK )

                                    INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = MW.WxID
                                    LEFT JOIN dbo.UserInfo AS UI WITH ( NOLOCK ) ON MW.CreateUserID = UI.UserID
                                    LEFT JOIN dbo.UserDetailInfo AS UDI WITH ( NOLOCK ) ON UDI.UserID = UI.UserID
		                            LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = UI.Source
                        WHERE   MW.MediaID = @MediaID ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@MediaID",mediaId),
                //new SqlParameter("@MediaType",(int)MediaType.WeiXin)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaWeixin>(data.Tables[0]);
        }

        public List<SearchTitleResponse> GetSearchTitle(PublishSearchAutoQuery<SearchTitleResponse> query)
        {
            var sql = @"
                    SELECT  TOP ( {0} )
                            WX.MediaID ,
                            WO.WxNumber AS Number ,
                            WO.NickName AS Name
                    FROM    dbo.Media_Weixin AS WX WITH ( NOLOCK )
                            INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = WX.WxID --AND WO.Status = 0
                    WHERE   1 = 1 AND WX.Status = 0
                            {2}
                            AND ( WO.WxNumber LIKE '{4}%'
                                  OR WO.NickName LIKE '{4}%'
                                )
                            AND EXISTS ( SELECT 1
                                         FROM   dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                         WHERE  PB.MediaType =  {1}
                                                AND PB.MediaID = WX.MediaID
                                                AND PB.IsDel = 0
                                                --AND PB.Wx_Status = --已通过
                                       {3}
                                        )

                    ORDER BY WO.WxNumber ASC
                    ";

            var whereSql = string.Empty;

            if (query.CreateUserId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                whereSql += " AND WX.CreateUserID = " + query.CreateUserId;
            }

            var paras = new List<SqlParameter>();

            sql = string.Format(sql, query.PageSize, (int)MediaType.WeiXin
                , whereSql, string.Empty, XYAuto.Utils.StringHelper.SqlFilter(query.KeyWord));

            //sql += " ORDER BY  WX.Number ASC ";

            //paras.Add(new SqlParameter("@KeyWord", query.KeyWord));

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<SearchTitleResponse>(data.Tables[0]);
        }

        /// <summary>
        /// 主分类推荐
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<GetRecommendClassListDto> GetRecommendClassMain(PublishSearchAutoQuery<GetRecommendClassListDto> query)
        {
            var sql = @"
                    SELECT TOP ({0}) WO.RecID AS WxId,
		                    WO.WxNumber AS Number ,
                            WO.NickName AS Name ,
                            WO.FansCount ,
                            WO.HeadImg AS HeadIconURL ,
                            MW.MediaID ,
                            MW.PublishStatus --上下架状态
                            ,
                            Price = ISNULL(( SELECT TOP 1
                                                MIN(PD.Price)
                                        FROM      dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                LEFT JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = PB.PubID
                                        WHERE     PB.MediaID = MW.MediaID
                                                AND PB.MediaType = {1}
                                                AND PB.Wx_Status ={2} --注：改为 42011“上架”状态的刊例(广告)
                                                AND GETDATE() BETWEEN PB.BeginTime AND PB.EndTime
                                        GROUP BY  PD.Price ,
                                                PB.BeginTime
                                        ORDER BY  PD.Price ASC ,
                                                PB.BeginTime ASC
                                    ),0)
                    FROM    dbo.Weixin_OAuth AS WO WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON MW.Number = WO.WxNumber
                    WHERE   MW.Status = 0
		                    AND EXISTS(
			                    SELECT * FROM dbo.Publish_BasicInfo AS PB WITH(NOLOCK)
			                    WHERE MW.MediaID = PB.MediaID
				                    AND PB.MediaType = {1}
				                    AND PB.Wx_Status ={2}  --注：改为 42011“上架”状态的刊例(广告)
		                    )
                            AND MW.AuditStatus = {3} --注：改为 审核通过状态 43002
                            AND WO.RecID IN (
                                                    SELECT  MC1.WxID
								                    FROM    dbo.MediaCategory AS MC1 WITH ( NOLOCK )
								                    WHERE   MC1.CategoryID IN ( SELECT  MC.CategoryID
															                    FROM    dbo.MediaCategory AS MC WITH ( NOLOCK )
															                    WHERE   MC.WxID IN
																						(SELECT WxID FROM DBO.Media_Weixin WITH(NOLOCK) WHERE MediaID = {4})
																	                    AND MC.MediaType = {1}
                                                                                        --AND MC.SortNumber=1 --注：SortNumber=1主分类
                                                                               )
										                    AND MC1.MediaType = {1}
								                    GROUP BY MC1.WxID
								                    )
                            ORDER BY WO.FansCount DESC ";

            var paras = new List<SqlParameter>();
            sql = string.Format(sql, query.PageSize, (int)MediaType.WeiXin, (int)PublishBasicStatusEnum.上架
                , (int)MediaAuditStatusEnum.AlreadyPassed, query.MediaId);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<GetRecommendClassListDto>(data.Tables[0]);
        }

        /// <summary>
        /// 常见分类推荐
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<GetRecommendClassListDto> GetRecommendClass(PublishSearchAutoQuery<GetRecommendClassListDto> query)
        {
            var sql = @"
                    SELECT TOP ({0}) WO.RecID AS WxId,
		                    WO.WxNumber AS Number ,
                            WO.NickName AS Name ,
                            WO.FansCount ,
                            WO.HeadImg AS HeadIconURL ,
                            MW.MediaID ,
                            MW.PublishStatus --上下架状态
                            ,
                            Price = ISNULL(( SELECT TOP 1

                                                MIN(PD.SalePrice)
                                        FROM      dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                LEFT JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = PB.PubID
                                        WHERE     PB.MediaID = MW.MediaID
                                                AND PB.MediaType = {1}
                                                AND PB.Wx_Status ={2} --且媒体下有“启用”状态刊例的媒体  注：改为 42011“上架”状态的刊例(广告)
                                                AND GETDATE() BETWEEN PB.BeginTime AND PB.EndTime
                                        GROUP BY  PD.SalePrice ,
                                                PB.BeginTime

                                        ORDER BY  PD.SalePrice ASC ,
                                                PB.BeginTime ASC
                                    ),0)
                    FROM    dbo.Weixin_OAuth AS WO WITH ( NOLOCK )

                            INNER JOIN dbo.Media_Weixin AS MW WITH ( NOLOCK ) ON  WO.RecID = MW.WxID
                    WHERE   MW.Status = 0
		                    AND EXISTS(
			                    SELECT * FROM dbo.Publish_BasicInfo AS PB WITH(NOLOCK)
			                    WHERE MW.MediaID = PB.MediaID
				                    AND PB.MediaType = {1}

				                    AND PB.Wx_Status ={2}  --且媒体下有“启用”状态刊例的媒体  注：改为 42011“上架”状态的刊例(广告)
		                    )

                            AND MW.AuditStatus = {3} --44001媒体已上架 注：改为 审核通过状态 43002
                            AND WO.RecID IN (
                                                  SELECT  MC1.WxID
								                    FROM    dbo.MediaCategory AS MC1 WITH ( NOLOCK )
								                    WHERE   MC1.CategoryID IN ( SELECT  MC.CategoryID
															                    FROM    dbo.MediaCategory AS MC WITH ( NOLOCK )
															                   WHERE   MC.WxID IN
																						(SELECT WxID FROM DBO.Media_Weixin WITH(NOLOCK) WHERE MediaID = {4})

																	                    AND MC.MediaType = {1}
                                                                                        AND (MC.SortNumber<>1 OR MC.SortNumber IS NULL)--一般分类
                                                                               )
										                    AND MC1.MediaType = {1}
								                    GROUP BY MC1.WxID
								                    )
                            ORDER BY WO.FansCount DESC ";

            var paras = new List<SqlParameter>();

            sql = string.Format(sql, query.PageSize, (int)MediaType.WeiXin, (int)PublishBasicStatusEnum.上架
                , (int)MediaAuditStatusEnum.AlreadyPassed, query.MediaId);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<GetRecommendClassListDto>(data.Tables[0]);
        }

        /// <summary>
        /// 若推荐结果媒体数小于1，，则取全局微信类媒体粉丝数前五的做推荐
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<GetRecommendClassListDto> GetRecommendClassOther(PublishSearchAutoQuery<GetRecommendClassListDto> query)
        {
            var sql = @"
                        SELECT TOP ({0})  WO.RecID AS WxId,WO.WxNumber AS Number ,
                               WO.NickName AS Name ,
                                WO.FansCount ,
                                WO.HeadImg AS HeadIconURL ,
                                MW.MediaID ,
                                Price = ISNULL(( SELECT TOP 1

                                                    MIN(PD.SalePrice)
                                          FROM      dbo.Publish_BasicInfo AS PB WITH ( NOLOCK )
                                                    LEFT JOIN dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) ON PD.PubID = PB.PubID
                                          WHERE     PB.MediaID = MW.MediaID
                                                    AND PB.MediaType = {1}
                                                    --AND PB.PublishStatus = 15005
                                                    --AND GETDATE() BETWEEN PB.BeginTime AND PB.EndTime

                                          GROUP BY  PD.SalePrice
                                          ORDER BY  PD.SalePrice ASC
                                        ),0)
                        FROM    dbo.Media_Weixin AS MW WITH ( NOLOCK )

                                INNER JOIN  dbo.Weixin_OAuth AS WO WITH ( NOLOCK )  ON WO.RecID = MW.WxID
                        WHERE   MW.Status = 0
                                AND EXISTS(
			                    SELECT * FROM dbo.Publish_BasicInfo AS PB WITH(NOLOCK)
			                    WHERE MW.MediaID = PB.MediaID
				                    AND PB.MediaType = {1}

				                    AND PB.Wx_Status ={2}  --且媒体下有“启用”状态刊例的媒体 注：改为 42011“上架”状态的刊例(广告)
		                        )

                                AND MW.AuditStatus = {3} --44001媒体已上架 注：改为 审核通过状态 43002

                        ORDER BY WO.FansCount DESC ";
            var paras = new List<SqlParameter>();

            sql = string.Format(sql, query.PageSize, (int)MediaType.WeiXin, (int)PublishBasicStatusEnum.上架
                    , (int)MediaAuditStatusEnum.AlreadyPassed);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<GetRecommendClassListDto>(data.Tables[0]);
        }

        #region V1.1.1微信

        public GetMediaListBResDTO GetMediaListB(int auditStatus, string key, int categoryID, int levelType,
            int source, int publishStatus, int oauthType, int oauthStatus, string startDate, string endDate, string submitUserName,
            string submitStartDate, string submitEndDate, string auditStartDate, string auditEndDate,
            int isAreaMedia, int areaProvniceId, int areaCityId, string rightSql, string orderBy, int pageIndex, int pageSize)
        {
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            SqlParameter[] parameters = new SqlParameter[]
            {
                outParam,
                new SqlParameter("@AuditStatus", auditStatus),
                new SqlParameter("@Key", SqlFilter(key)),
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@LevelType", levelType),
                new SqlParameter("@PublishStatus", publishStatus),
                new SqlParameter("@OAuthType", oauthType),
                new SqlParameter("@OAuthStatus", oauthStatus),
                new SqlParameter("@SubmitUserName", SqlFilter(submitUserName)),
                new SqlParameter("@SubmitStartDate", SqlFilter(submitStartDate)),
                new SqlParameter("@SubmitEndDate", SqlFilter(submitEndDate)),
                new SqlParameter("@AuditStartDate", SqlFilter(auditStartDate)),
                new SqlParameter("@AuditEndDate", SqlFilter(auditEndDate)),
                new SqlParameter("@StartDate", SqlFilter(startDate)),
                new SqlParameter("@EndDate", SqlFilter(endDate)),
                new SqlParameter("@IsAreaMedia", isAreaMedia),
                new SqlParameter("@AreaProvniceId", areaProvniceId),
                new SqlParameter("@AreaCityId", areaCityId),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@Orderby", orderBy),
                new SqlParameter("@Source", source),
                new SqlParameter("@RightSql", rightSql),
            };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Weixin_SelectFromBack1_1_8", parameters).Tables[0];
            List<MediaItemBDTO> list = new List<MediaItemBDTO>();
            int totalCount = Convert.ToInt32(parameters[0].Value);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new MediaItemBDTO()
                {
                    MediaID = Convert.ToInt32(dr["MediaID"]),
                    Name = dr["Name"].ToString(),
                    Number = dr["Number"].ToString(),
                    HeadImg = dr["HeadImg"].ToString(),
                    CategoryNames = dr["CategoryNames"].ToString(),
                    FansCount = Convert.ToInt32(dr["FansCount"] == DBNull.Value ? 0 : dr["FansCount"]),
                    OAuthType = Convert.ToInt32(dr["AuthType"] == DBNull.Value ? -2 : dr["AuthType"]),
                    OAuthTypeName = dr["AuthTypeName"].ToString(),
                    PublishStatus = Convert.ToInt32(dr["PublishStatus"] == DBNull.Value ? -2 : dr["PublishStatus"]),
                    PublishStatusName = dr["PublishStatusName"].ToString(),
                    AuditStatus = Convert.ToInt32(dr["AuditStatus"] == DBNull.Value ? -2 : dr["AuditStatus"]),
                    LevelTypeName = dr["LevelTypeName"].ToString(),
                    CreateTime = dr["CreateTime"] == DBNull.Value ? "1900-01-01" : Convert.ToDateTime(dr["CreateTime"]).ToString("yyyy-MM-dd"),
                    LastUpdateTime = dr["LastUpdateTime"] == DBNull.Value ? "1900-01-01" : Convert.ToDateTime(dr["LastUpdateTime"]).ToString("yyyy-MM-dd"),
                    AuditTime = dr["AuditTime"] == DBNull.Value ? "1900-01-01" : Convert.ToDateTime(dr["AuditTime"]).ToString("yyyy-MM-dd"),
                    RejectMsg = dr["RejectMsg"].ToString(),
                    CanAddToCocommend = Convert.ToInt32(dr["CanAddToRecommend"] == DBNull.Value ? 0 : dr["CanAddToRecommend"]).Equals(1),
                    IsRange = Convert.ToInt32(dr["IsRange"]).Equals(1),
                    SubmitUserName = dr["TrueName"].ToString(),
                    WxID = Convert.ToInt32(dr["WxID"]),
                    hasOnPub = Convert.ToInt32(dr["OnPubCount"]) > 0,
                    IsAreaMedia = Convert.ToBoolean(dr["IsAreaMedia"]),
                    AreaMapping = dr["AreaMapping"].ToString()
                });
            }
            return new GetMediaListBResDTO() { List = list, Total = totalCount };
        }

        public GetMediaListFResDTO GetMediaListF(string key, int categoryID, string fansCountMax, string fansCountMin, string priceMax, string priceMin, int isVerify, bool canReceive, int userId,
            string rightSql, string orderBy, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Key",SqlFilter(key)),
                new SqlParameter("@CategoryID",categoryID),
                new SqlParameter("@FansCountMax",fansCountMax),
                new SqlParameter("@FansCountMin",fansCountMin),
                new SqlParameter("@PriceMax",priceMax),
                new SqlParameter("@PriceMin",priceMin),
                new SqlParameter("@IsVerify",isVerify),
                new SqlParameter("@CanReceive",canReceive),
                new SqlParameter("@UserID",userId),
                new SqlParameter("@RightSql",rightSql),
                new SqlParameter("@Orderby",orderBy),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@TotalCount",totalCount),
            };
            parameters[13].Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS_ReadOnly, CommandType.StoredProcedure, "p_Media_Weixin_SelectFromFrontV1_1_7", parameters).Tables[0];
            totalCount = Convert.ToInt32(parameters[13].Value);
            List<MediaItemFDTO> list = new List<MediaItemFDTO>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new MediaItemFDTO()
                    {
                        MediaID = Convert.ToInt32(dr["MediaID"] == DBNull.Value ? 0 : dr["MediaID"]),
                        PubID = Convert.ToInt32(dr["PubID"] == DBNull.Value ? 0 : dr["PubID"]),
                        ADDetailID = Convert.ToInt32(dr["ADDetailID"] == DBNull.Value ? 0 : dr["ADDetailID"]),
                        HeadImg = dr["HeadImg"] == DBNull.Value ? string.Empty : dr["HeadImg"].ToString(),
                        AverageReading = Convert.ToInt32(dr["AverageReading"] == DBNull.Value ? 0 : dr["AverageReading"]),
                        MaxReading = Convert.ToInt32(dr["MaxReading"] == DBNull.Value ? 0 : dr["MaxReading"]),
                        CategoryNames = dr["CategoryNames"] == DBNull.Value ? string.Empty : dr["CategoryNames"].ToString(),
                        FansCount = Convert.ToInt32(dr["FansCount"] == DBNull.Value ? 0 : dr["FansCount"]),
                        IsVerify = dr["IsVerify"] == DBNull.Value ? false : dr["IsVerify"].ToString().Equals("1"),
                        Name = dr["Name"] == DBNull.Value ? string.Empty : dr["Name"].ToString(),
                        Number = dr["Number"] == DBNull.Value ? string.Empty : dr["Number"].ToString(),
                        OwnerName = dr["OwnerName"] == DBNull.Value ? string.Empty : dr["OwnerName"].ToString(),
                        Price = Convert.ToInt32(dr["ADDetailID"]) == 0 ? "--" : dr["Price"].ToString(),
                        MaLiIndex = Convert.ToDecimal(dr["MaLiIndex"] == DBNull.Value ? 0 : dr["MaLiIndex"]),
                        RoleId = dr["RoleID"].ToString()
                    });
                }
            }
            return new GetMediaListFResDTO() { List = list, Total = totalCount };
        }

        public GetMediaAuditStatusCountResDTO GetAuditCount(int createUserID, bool isYY)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@CreateUserID",createUserID),
                new SqlParameter("@IsYY",isYY)
            };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_Weixin_SelectAuditCount1_1", parameters).Tables[0];
            return DataTableToEntity<GetMediaAuditStatusCountResDTO>(dt);
        }

        public GetMediaListBResDTO GetBasicList(string key, int categoryID, int levelType,
            int oauthType, int oauthStatus, int isAreaMedia, int areaProvniceId, int areaCityId, string rightSql, string orderBy, int pageIndex,
            int pageSize)
        {
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            SqlParameter[] parameters = new SqlParameter[]
            {
                outParam,
                new SqlParameter("@Key", key),
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@LevelType", levelType),
                new SqlParameter("@OAuthType", oauthType),
                new SqlParameter("@OAuthStatus", oauthStatus),
                new SqlParameter("@IsAreaMedia", isAreaMedia),
                new SqlParameter("@AreaProvniceId", areaProvniceId),
                new SqlParameter("@AreaCityId", areaCityId),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@Orderby", orderBy),
                new SqlParameter("@RightSql", rightSql)
            };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Weixin_OAuth_Select1_1_8", parameters).Tables[0];
            List<MediaItemBDTO> list = new List<MediaItemBDTO>();
            int totalCount = Convert.ToInt32(parameters[0].Value);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new MediaItemBDTO()
                {
                    MediaID = Convert.ToInt32(dr["MediaID"]),
                    Name = dr["Name"] == DBNull.Value ? string.Empty : dr["Name"].ToString(),
                    Number = dr["Number"] == DBNull.Value ? string.Empty : dr["Number"].ToString(),
                    HeadImg = dr["HeadImg"] == DBNull.Value ? string.Empty : dr["HeadImg"].ToString(),
                    CategoryNames = dr["CategoryNames"] == DBNull.Value ? string.Empty : dr["CategoryNames"].ToString(),
                    LevelTypeName = dr["LevelTypeName"] == DBNull.Value ? string.Empty : dr["LevelTypeName"].ToString(),
                    FansCount = Convert.ToInt32(dr["FansCount"] == DBNull.Value ? 0 : dr["FansCount"]),
                    AuditStatus = 43002,
                    OAuthType = Convert.ToInt32(dr["AuthType"] == DBNull.Value ? -2 : dr["AuthType"]),
                    OAuthTypeName = dr["AuthTypeName"] == DBNull.Value ? string.Empty : dr["AuthTypeName"].ToString(),
                    OAuthStatusName = dr["OAuthStatusName"] == DBNull.Value ? string.Empty : dr["OAuthStatusName"].ToString(),
                    CreateTime = dr["CreateTime"] == DBNull.Value ? "1900-01-01" : Convert.ToDateTime(dr["CreateTime"]).ToString("yyyy-MM-dd"),
                    LastUpdateTime = dr["CreateTime"] == DBNull.Value ? "1900-01-01" : Convert.ToDateTime(dr["CreateTime"]).ToString("yyyy-MM-dd"),
                    CanAddToCocommend = Convert.ToInt32(dr["CanAddToRecommend"] == DBNull.Value ? 0 : dr["CanAddToRecommend"]).Equals(1),
                    IsRange = Convert.ToInt32(dr["IsRange"]).Equals(1),
                    IsAreaMedia = Convert.ToBoolean(dr["IsAreaMedia"]),
                    AreaMapping = dr["AreaMapping"].ToString()
                });
            }
            return new GetMediaListBResDTO() { List = list, Total = totalCount };
        }

        public int GetWeixinMediaIDByMediaName(string mediaName, string rightSql)
        {
            string sql = "select top 1 MediaID from Media_Weixin where Name = @MediaName " + rightSql;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@MediaName",mediaName),
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 检查媒体主用户下或AE角色是否可以添加WxID
        /// </summary>
        /// <param name="wxID">微信主表ID</param>
        /// <param name="userID">用户ID 0表示AE角色</param>
        /// <returns>true\false</returns>
        public bool CheckCanAdd(int wxID, int userID)
        {
            string sql = "select count(1) from Media_Weixin where Status = 0 and WxID = " + wxID;
            if (userID.Equals(0))
            {
                sql += " and CreateUserID in ( select UserID from UserRole where RoleID = 'SYS001RL00005' )";
            }
            else
            {
                sql += " and CreateUserID =  " + userID;
            }
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return Convert.ToInt32(obj).Equals(0);
        }

        public List<MediaDictDTO> GetWeixin_OAuthDict(string key)
        {
            string sql = @"select top 10 WxNumber as Number,NickName as Name from Weixin_OAuth
            where Status = 0 and WxNumber like '%" + SqlFilter(key) + "%' or NickName like '%" + SqlFilter(key) + "%' order by RecID desc";
            return DataTableToList<MediaDictDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
        }

        #endregion V1.1.1微信

        public int UpdateSqlTest()
        {
            var sql = @"
                    UPDATE DBO.Media_Weixin SET LastUpdateTime = GETDATE() WHERE MediaID = 1
                    UPDATE DBO.Media_Weixin SET LastUpdateTime = GETDATE() WHERE MediaID = 2
                    UPDATE DBO.Media_Weixin SET LastUpdateTime = GETDATE() WHERE MediaID = 3
                    DELETE FROM DBO.Media_Weixin WHERE Status = -1 AND MediaID = 7
                    ";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}