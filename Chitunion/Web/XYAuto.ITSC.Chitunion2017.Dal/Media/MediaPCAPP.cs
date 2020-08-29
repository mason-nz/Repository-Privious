using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using System.Linq;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    //媒体-媒体信息
    public class MediaPCAPP : DataBase
    {
        #region Instance

        public static readonly MediaPCAPP Instance = new MediaPCAPP();

        #endregion Instance

        public Entities.Media.MediaPcApp GetEntity(int mediaId)
        {
            const string sql = @"SELECT [MediaID],[BaseMediaID]
                          ,[Name]
                          ,[HeadIconURL]
                          ,[CategoryID]
                          ,[ProvinceID]
                          ,[CityID]
                          ,[Terminal]
                          ,[DailyLive]
                          ,[DailyIP]
                          ,[WebSite]
                          ,[Remark]
                            ,[Source],[AuditStatus]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                      FROM [dbo].[Media_PCAPP] WITH(NOLOCK) WHERE Status = 0 AND MediaID = @MediaID";
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaPcApp>(data.Tables[0]);
        }

        public Entities.Media.MediaPcApp GetInfoByRoleAe(string name, string roleId)
        {
            var sql = @"
                        SELECT  MP.*
                        FROM    dbo.Media_PCAPP AS MP WITH ( NOLOCK )
                        WHERE   MP.Name = @Name
                                AND Status = 0
                                AND EXISTS ( SELECT UserID
                                             FROM   dbo.UserRole
                                             WHERE  RoleID = @RoleID
                                                    AND Status = 0
                                                    AND MP.CreateUserID = UserID ) ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name", name),
                 new SqlParameter("@RoleID", roleId)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaPcApp>(data.Tables[0]);
        }

        public Entities.Media.MediaPcApp GetEntity(string name, int filterMediaId = 0, int userId = 0)
        {
            string sql = @"SELECT TOP 1  [MediaID],[BaseMediaID]
                          ,[Name]
                          ,[HeadIconURL]
                          ,[CategoryID]
                          ,[ProvinceID]
                          ,[CityID]
                          ,[Terminal]
                          ,[DailyLive]
                          ,[DailyIP]
                          ,[WebSite]
                          ,[Remark]             ,[Source],[AuditStatus]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                      FROM [dbo].[Media_PCAPP] WITH(NOLOCK)
                        WHERE Status = 0 AND Name = @Name";
            var paras = new List<SqlParameter>() { new SqlParameter("@Name", name) };
            if (filterMediaId > 0)
            {
                sql += " AND MediaID != @MediaID";
                paras.Add(new SqlParameter("@MediaID", filterMediaId));
            }
            if (userId > 0)
            {
                sql += " AND CreateUserID = @CreateUserID";
                paras.Add(new SqlParameter("@CreateUserID", userId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaPcApp>(data.Tables[0]);
        }

        public PublishStatisticsCount GetStatisticsCount(PublishSearchAutoQuery<PublishStatisticsCount> query)
        {
            var sql = @"
                        ;
                        WITH    CTE_Statistics
                                AS ( SELECT   COUNT(*) AS AppendAuditCount ,
                                            0 AS RejectNotPassCount ,
                                            0 AS AuditPassCount
                                    FROM     dbo.Media_PCAPP AS MP WITH ( NOLOCK )
                                    WHERE    MP.AuditStatus = {0} --待审核
                                            AND MP.CreateUserID = {3}
					                        AND MP.Status = 0
                                    UNION
                                    SELECT   0 AS AppendAuditCount ,
                                            0 AS RejectNotPassCount ,
                                            COUNT(*) AS AuditPassCount
                                    FROM     dbo.Media_PCAPP AS MP WITH ( NOLOCK )
                                    WHERE    MP.AuditStatus = {1} --已通过
                                            AND MP.CreateUserID = {3}
					                        AND MP.Status = 0
                                    UNION
                                    SELECT   0 AS AppendAuditCount ,
                                            COUNT(*) AS RejectNotPassCount ,
                                            0 AS AuditPassCount
                                    FROM     dbo.Media_PCAPP AS MP WITH ( NOLOCK )
                                    WHERE    MP.AuditStatus = {2} --已驳回
                                            AND MP.CreateUserID = {3}
					                        AND MP.Status = 0
                                    )
                        SELECT  MAX(AppendAuditCount) AS AppendAuditCount ,
                                MAX(RejectNotPassCount) AS RejectNotPassCount ,
                                MAX(AuditPassCount) AS AuditPassCount
                        FROM    CTE_Statistics
                        ";
            var paras = new List<SqlParameter>();

            var whereSql = new StringBuilder();

            sql = string.Format(sql, (int)MediaAuditStatusEnum.PendingAudit, (int)MediaAuditStatusEnum.AlreadyPassed,
                (int)MediaAuditStatusEnum.RejectNotPass, query.CreateUserId);

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<PublishStatisticsCount>(data.Tables[0]);
        }

        /// <summary>
        /// app获取详情
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public Entities.Media.MediaPcApp GetInfo(int mediaId)
        {
            #region sql

            var sql = @"
                    CREATE TABLE #AreaTemp(AreaID VARCHAR(100),AreaName VARCHAR(100))
                    INSERT INTO #AreaTemp
                            ( AreaID, AreaName )
                    SELECT AreaID,AreaName FROM DBO.AreaInfo WITH(NOLOCK)

                    --DROP TABLE #AreaTemp
                    SELECT TOP 1 * ,AI1.AreaName AS ProvinceName,AI2.AreaName AS CityName
                                   , CommonlyClassStr = (STUFF(( SELECT  '|' + ( ISNULL(( CAST(DIF.DictId AS VARCHAR(15)) + ','
                                                                     + DIF.DictName + ','
                                                                     + CAST(MCC.SortNumber AS VARCHAR(15)) ),
                                                                   '') )
							                        FROM    dbo.Media_CommonlyClass AS MCC WITH ( NOLOCK )
									                        LEFT JOIN dbo.DictInfo AS DIF WITH ( NOLOCK ) ON MCC.CategoryID = DIF.DictId
							                        WHERE   MCC.MediaType = {0}
									                        AND MCC.MediaID = MP.MediaID
							                        ORDER BY MCC.SortNumber DESC
						                          FOR
							                        XML PATH('')
						                          ), 1, 1, '')
                              )
		                    ,AreaMapping=( SELECT  STUFF(( SELECT  '|' + RTRIM(ISNULL(( AI1.AreaID + ',' + AI1.AreaName ),
                                                               '') + '@=' + ISNULL(( AI2.AreaID
                                                                                  + ','
                                                                                  + ISNULL(AI2.AreaName,
                                                                                  '') ), ''))
                                            FROM    dbo.Media_Area_Mapping AS MAP WITH ( NOLOCK )
                                                    LEFT JOIN #AreaTemp AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MAP.ProvinceID
                                                    LEFT JOIN #AreaTemp AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MAP.CityID
                                            WHERE   MAP.MediaType = {0}
                                                    AND MAP.MediaID = MP.MediaID
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
                                          )
		                    ,OrderRemarkStr=(
                                SELECT  STUFF(( SELECT  '|'
                                                        + RTRIM(( CAST(PRK.RemarkID AS VARCHAR(15))
                                                                  + ',' + ISNULL(DI.DictName, '')
                                                                  + ',' + ISNULL(PRK.OtherContent,
                                                                                 '') ))
                                                FROM    dbo.Publish_Remark AS PRK WITH ( NOLOCK )
                                                        LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON PRK.RemarkID = DI.DictId
                                                WHERE   RelationID = MP.MediaID
                                                        AND PRK.EnumType = {1}
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '')

		                    )
                            --,AdTemplateId = ISNULL((
			                --    SELECT TOP 1 ADDT.RecID FROM DBO.App_AdTemplate AS ADDT WITH(NOLOCK)
			                --    WHERE ADDT.BaseMediaID = MP.RecID
				            --        --AND ADDT.AuditStatus IN ('已通过','待审核')
				            --        AND ADDT.Status = 0
				            --        ORDER BY ADDT.RecID ASC
		                    --),0)
                    FROM    dbo.Media_PCAPP AS MP WITH ( NOLOCK )
		                         LEFT JOIN #AreaTemp AS AI1 WITH ( NOLOCK ) ON AI1.AreaID = MP.ProvinceID
                                 LEFT JOIN #AreaTemp AS AI2 WITH ( NOLOCK ) ON AI2.AreaID = MP.CityID
                    WHERE   MP.MediaID = @MediaID
                            AND MP.Status = 0
                    ";

            #endregion sql

            sql = string.Format(sql, (int)MediaType.APP, (int)MediaRemarkTypeEnum.APP备注);
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Media.MediaPcApp>(data.Tables[0]);
        }

        public int Insert(Entities.Media.MediaPcApp entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_PCAPP(");
            strSql.Append("BaseMediaID,Name,HeadIconURL,CategoryID,ProvinceID,CityID,Terminal,DailyLive,DailyIP,WebSite,Remark,Source,AuditStatus,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
            strSql.Append(") values (");
            strSql.Append("@BaseMediaID,@Name,@HeadIconURL,@CategoryID,@ProvinceID,@CityID,@Terminal,@DailyLive,@DailyIP,@WebSite,@Remark,@Source,@AuditStatus,getdate(),@CreateUserID,getdate(),@LastUpdateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@BaseMediaID",entity.BaseMediaID),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@Terminal",entity.Terminal),
                        new SqlParameter("@DailyLive",entity.DailyLive),
                        new SqlParameter("@DailyIP",entity.DailyIP),
                        new SqlParameter("@WebSite",entity.WebSite),
                        new SqlParameter("@Remark",entity.Remark),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Media.MediaPcApp entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_PCAPP]");
            strSql.Append(@"SET     --[Name] = @Name,
                                  [HeadIconURL] = @HeadIconURL
                                  ,[CategoryID] = @CategoryID
                                  ,[ProvinceID] = @ProvinceID
                                  ,[CityID] = @CityID
                                  ,[Terminal] = @Terminal
                                  ,[DailyLive] = @DailyLive
                                  ,[DailyIP] = @DailyIP
                                  ,[WebSite] = @WebSite
                                  ,[Remark] = @Remark
                                  ,[AuditStatus] = @AuditStatus
                              --,[CreateTime] = @CreateTime
                              --,[CreateUserID] = @CreateUserID
                              ,[LastUpdateTime] = @LastUpdateTime
                              ,[LastUpdateUserID] = @LastUpdateUserID
                            WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                    new SqlParameter("@MediaID",entity.MediaID),
						//new SqlParameter("@Name",entity.Name),
            			new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@Terminal",entity.Terminal),
                        new SqlParameter("@DailyLive",entity.DailyLive),
                        new SqlParameter("@DailyIP",entity.DailyIP),
                        new SqlParameter("@WebSite",entity.WebSite),
                        new SqlParameter("@Remark",entity.Remark),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public int UpdateBaseMediaId(int mediaId, int baseMediaId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Media_PCAPP]");
            strSql.Append(@"SET BaseMediaID = @BaseMediaID
                            WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                    new SqlParameter("@MediaID",mediaId),
                        new SqlParameter("@BaseMediaID",baseMediaId)
                        };

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 存在就编辑，否则就添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Operate(Entities.Media.MediaPcApp entity, int userId)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || userId <= 0)
            {
                throw new Exception("媒体-app-请输入Name,创建人id");
            }
            var info = GetEntity(entity.Name);
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
        /// 获取平面媒体列表 ls
        /// </summary>
        /// <param name="name">媒体名称</param>
        /// <param name="source">来源</param>
        /// <param name="createUser">录入人</param>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">也大小</param>
        /// <param name="totalCount">记录总数</param>
        /// <returns></returns>
        public List<Entities.Media.MediaPcApp> GetList(string name, int source, string createUser, string beginDate, string endDate, string rightSql,
int categoryId, int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;
            SqlParameter[] parameters = {
                new SqlParameter("@Name", SqlDbType.VarChar),
                new SqlParameter("@Source", SqlDbType.Int),
                new SqlParameter("@CreateUser", SqlDbType.VarChar),
                new SqlParameter("@BeginDate", SqlDbType.VarChar),
                new SqlParameter("@EndDate", SqlDbType.VarChar),
                new SqlParameter("@RightSql",SqlDbType.VarChar),
                new SqlParameter("@PageIndex", SqlDbType.Int),
                new SqlParameter("@PageSize", SqlDbType.Int),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[0].Value = SqlFilter(name);
            parameters[1].Value = source;
            parameters[2].Value = SqlFilter(createUser);
            parameters[3].Value = SqlFilter(beginDate);
            parameters[4].Value = SqlFilter(endDate);
            parameters[5].Value = rightSql;
            parameters[6].Value = pageIndex;
            parameters[7].Value = pageSize;
            parameters[8].Direction = ParameterDirection.Output;

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_PCAPP_Select", parameters).Tables[0];
            List<Entities.Media.MediaPcApp> list = new List<Entities.Media.MediaPcApp>();

            #region 填充

            totalCount = Convert.ToInt32(parameters[8].Value);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new Entities.Media.MediaPcApp()
                {
                    MediaID = Convert.ToInt32(dr["MediaID"]),
                    Name = dr["Name"].ToString(),
                    CreateTime = dr["CreateTime"] == DBNull.Value ? new DateTime(1900, 01, 01) : Convert.ToDateTime(dr["CreateTime"]),
                    TrueName = dr["TrueName"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    SourceName = dr["SourceName"].ToString(),
                    AreaName = dr["AreaName"].ToString(),
                    OverlayName = dr["Overlay"].ToString(),
                    CategoryName = dr["CategoryName"].ToString(),
                    ADCount = dr["ADCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ADCount"]),
                    DailyLive = dr["DailyLive"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DailyLive"]),
                    PubCount = Convert.ToInt32(dr["PubCount"]),
                    Status = Convert.ToInt32(dr["AuditStatus"]),
                    PubID = dr["PubID"].ToString()
                });
            }

            #endregion 填充

            return list;
        }

        public MediaPCAPPDTO GetDetail(int mediaID, string rightSql)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@MediaID", SqlDbType.Int),
                new SqlParameter("@RightSql",SqlDbType.VarChar)
            };
            parameters[0].Value = mediaID;
            parameters[1].Value = rightSql;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_PCAPP_Detail", parameters);
            DataTable dt = ds.Tables[0];
            string orderRemarkName = string.Empty;
            MediaPCAPPDTO dto = new MediaPCAPPDTO();
            dto.MediaInfo = DataTableToEntity<Entities.Media.MediaPcApp>(dt);
            if (dto.MediaInfo != null)
            {
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

        public Entities.Publish.PublishBasicInfo GetBaseEntity(int pubId)
        {
            var sql = @"
                        SELECT * FROM DBO.Publish_BasicInfo
                        WHERE MediaType = {0}
                        AND PubID = {1}
                        AND Wx_Status = {2} --49004";
            var paras = new List<SqlParameter>();
            sql = string.Format(sql, (int)MediaType.APP, pubId, (int)AppPublishStatus.已上架);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Publish.PublishBasicInfo>(data.Tables[0]);
        }

        #region V1.1.4

        /// <summary>
        /// 添加下单备注
        /// </summary>
        /// <param name="MediaTableName">要添加的表</param>
        /// <param name="EnumType">枚举类型 45001：刊例，45002：微信，45003：App备注</param>
        /// <param name="RelationID">关联ID</param>
        /// <param name="ListRemarkID">下单备注ID集合</param>
        /// <param name="OtherContent">其他内容（为空则不添加）</param>
        /// <param name="CreateUserID">创建人ID</param>
        /// <param name="CreateTime">创建时间</param>
        /// <returns>大于0插入成功</returns>
        public int InsertMediaRemark(string MediaTableName, int EnumType, int RelationID, List<int> ListRemarkID, string OtherContent, int CreateUserID, DateTime CreateTime)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from " + MediaTableName + " where RelationID=" + RelationID + " and EnumType=" + EnumType);
            for (int i = 0; i < ListRemarkID.Count; i++)
            {
                sb.Append(";insert into " + MediaTableName + "   (RelationID,RemarkID,EnumType,CreateTime,CreateUserID) values (@RelationID," + ListRemarkID[i] + ",@EnumType,@CreateTime,@CreateUserID)");
            }
            SqlParameter[] parameters = {
                new SqlParameter("@EnumType", SqlDbType.Int),
                new SqlParameter("@RelationID", SqlDbType.Int),
                new SqlParameter("@CreateTime", SqlDbType.DateTime),
                new SqlParameter("@CreateUserID", SqlDbType.Int)
            };
            if (OtherContent != "")
            {
                sb.AppendFormat(";insert into {1}  (RelationID,RemarkID,EnumType,CreateTime,CreateUserID,OtherContent) values (@RelationID," + (int)MediaRemarkEnum.其他 + ",@EnumType,@CreateTime,@CreateUserID,'{0}')", SqlFilter(OtherContent), MediaTableName);
            }
            parameters[0].Value = EnumType;
            parameters[1].Value = RelationID;
            parameters[2].Value = CreateTime;
            parameters[3].Value = CreateUserID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString(), parameters);
        }

        public GetADListBResDTO GetAppADListB(int mediaID, string mediaName, string adName,
            string beginDate, string endDate, int pubStatus, string rightSql,
            string orderBy, int pageIndex, int pageSize)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID",mediaID),
                new SqlParameter("@MediaName",mediaName),
                new SqlParameter("@ADName",adName),
                new SqlParameter("@BeginDate",beginDate),
                new SqlParameter("@EndDate",endDate),
                new SqlParameter("@PubStatus",pubStatus),
                new SqlParameter("@RightSql",rightSql),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@Orderby",orderBy),
                new SqlParameter("@TotalCount",SqlDbType.Int),
            };
            parameters[10].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetAppADListB", parameters);
            int totalCount = Convert.ToInt32(parameters[10].Value);
            List<ADItemB> list = DataTableToList<ADItemB>(ds.Tables[0]);
            foreach (var item in list)
            {
                //PubID | BeginDate | EndDate | MinPrice | MaxPrice | PriceCount | AuditStatus | AuditStatusName
                if (!string.IsNullOrEmpty(item.CombinStr))
                {
                    item.PublishList = new List<PublishItem>();
                    foreach (string line in item.CombinStr.Split(','))
                    {
                        item.PublishList.Add(new PublishItem()
                        {
                            PubID = int.Parse(line.Split('|')[0].Trim()),
                            BeginDate = DateTime.Parse(line.Split('|')[1].Trim()),
                            EndDate = DateTime.Parse(line.Split('|')[2].Trim()),
                            MinPrice = int.Parse(line.Split('|')[3].Trim()),
                            MaxPrice = int.Parse(line.Split('|')[4].Trim()),
                            PriceCount = int.Parse(line.Split('|')[5].Trim()),
                            PubStatus = int.Parse(line.Split('|')[6].Trim()),
                            PubStatusName = line.Split('|')[7].Trim(),
                        });
                    }
                    item.CombinStr = string.Empty;
                }
            }
            return new GetADListBResDTO() { List = list, Total = totalCount };
        }

        public GetADListFResDTO GetAppADListF(int mediaID, string key, int categoryID,
            int saleType, int cityID, string minPrice, string maxPrice, int userID,
            string rightSql, string orderBy, int pageIndex, int pageSize)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID",mediaID),
                new SqlParameter("@Key", key),
                new SqlParameter("@CategoryID",categoryID),
                new SqlParameter("@SaleType",saleType),
                new SqlParameter("@CityID",cityID),
                new SqlParameter("@MinPrice",minPrice),
                new SqlParameter("@MaxPrice",maxPrice),
                new SqlParameter("@UserID",userID),
                new SqlParameter("@RightSql",rightSql),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@Orderby",orderBy),
                new SqlParameter("@TotalCount",SqlDbType.Int),
            };
            parameters[12].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetAppADListF", parameters);
            int totalCount = Convert.ToInt32(parameters[12].Value);
            List<ADItemF> list = DataTableToList<ADItemF>(ds.Tables[0]);
            return new GetADListFResDTO() { List = list, Total = totalCount };
        }

        public GetADDetailResDTO GetAppADItem(int mediaType, int pubID, int mediaID, int templateID, int createUserID)
        {
            GetADDetailResDTO res = new GetADDetailResDTO();
            string fileName = string.Empty;
            int index = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PubID", pubID),
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@TemplateID", templateID),
                new SqlParameter("@CreateUserID", createUserID),
                new SqlParameter("@BaseMediaID",DbType.Int32)
            };
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetAppADItem", parameters);
            res.BaseMediaID = Convert.ToInt32(parameters[4].Value == DBNull.Value ? 0 : parameters[4].Value);
            List<TemplateItem> templateList = DataTableToList<TemplateItem>(ds.Tables[0]);
            foreach (var template in templateList)
            {
                #region 拼拼拼拼拼

                if (!string.IsNullOrEmpty(template.SaleAreaStr))
                {//售卖区域
                    List<KeyValueItem> list = new List<KeyValueItem>();
                    foreach (string cp in template.SaleAreaStr.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(cp))
                            continue;
                        list.Add(new KeyValueItem() { Id = int.Parse(cp.Split('|')[0]), Name = cp.Split('|')[1] });
                    }
                    template.AdGroupList = list;
                    template.SaleAreaStr = string.Empty;
                }
                if (!string.IsNullOrEmpty(template.SellingModeStr))
                {//售卖类型
                    List<KeyValueItem> list = new List<KeyValueItem>();
                    foreach (string cp in template.SellingModeStr.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(cp))
                            continue;
                        list.Add(new KeyValueItem() { Id = int.Parse(cp.Split('|')[0]), Name = cp.Split('|')[1] });
                    }
                    template.SellingModeList = list;
                    template.SellingModeStr = string.Empty;
                }
                if (!string.IsNullOrEmpty(template.SellingPlatformStr))
                {//售卖平台
                    List<KeyValueItem> list = new List<KeyValueItem>();
                    foreach (string cp in template.SellingPlatformStr.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(cp))
                            continue;
                        list.Add(new KeyValueItem() { Id = int.Parse(cp.Split('|')[0]), Name = cp.Split('|')[1] });
                    }
                    template.SellingPlatformList = list;
                    template.SellingPlatformStr = string.Empty;
                }
                if (!string.IsNullOrEmpty(template.TemplateStyleStr))
                {//广告样式
                    List<KeyValueItem> list = new List<KeyValueItem>();
                    foreach (string cp in template.TemplateStyleStr.Split(','))
                    {
                        if (string.IsNullOrWhiteSpace(cp))
                            continue;
                        list.Add(new KeyValueItem() { Id = int.Parse(cp.Split('|')[0]), Name = cp.Split('|')[1] });
                    }
                    template.AdStyleList = list;
                    template.TemplateStyleStr = string.Empty;
                }

                #endregion 拼拼拼拼拼
            }
            res.TemplateList = templateList;
            if (!pubID.Equals(0))
            {
                res.PublishBasicInfo = DataTableToEntity<PublishBasicItem>(ds.Tables[1]);
                if (res.PublishBasicInfo != null && !string.IsNullOrWhiteSpace(res.PublishBasicInfo.PubFileUrl))
                {
                    fileName = res.PublishBasicInfo.PubFileUrl.Split('/')[res.PublishBasicInfo.PubFileUrl.Split('/').Length - 1];
                    if (fileName.Contains("$"))
                    {
                        index = fileName.LastIndexOf('$');
                        fileName = fileName.Substring(0, index);
                        res.PublishBasicInfo.PubFileName = fileName;
                    }
                }
                res.PubAuditInfo = DataTableToEntity<PubAuditItem>(ds.Tables[2]);
                res.PriceList = DataTableToList<PriceItem>(ds.Tables[3]);
                if (res.TemplateList.Count > 0)
                    res.TemplateList[0].PubDateList = DataTableToList<PubDateItem>(ds.Tables[4]);
            }
            else
            {
                if (res.TemplateList.Count > 0)
                {
                    var list = DataTableToList<PubDateItem>(ds.Tables[1]);
                    foreach (var item in list)
                    {
                        if (!string.IsNullOrEmpty(item.PubFileUrl))
                        {
                            fileName = item.PubFileUrl.Split('/')[item.PubFileUrl.Split('/').Length - 1];
                            if (fileName.Contains("$"))
                            {
                                index = fileName.LastIndexOf('$');
                                fileName = fileName.Substring(0, index);
                                item.FileName = fileName;
                            }
                        }
                    }
                    res.TemplateList[0].PubDateList = list;
                }
            }
            return res;
        }

        public List<PublishPriceItem> GetAuditAppADList(int pubID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PubID", pubID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetAuditAppADList", parameters);
            return DataTableToList<PublishPriceItem>(ds.Tables[0]);
        }

        public List<GetRecommendADResDTO> GetSimilarAD(int mediaID, int templateID, int userID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID",mediaID),
                new SqlParameter("@TemplateID",templateID),
                new SqlParameter("@UserID",userID),
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetSimilarAD", parameters);
            DataTable dt = ds.Tables[ds.Tables.Count - 1];
            return DataTableToList<GetRecommendADResDTO>(dt);
        }

        #endregion V1.1.4

        public void DelereOrderRemark(int mediaId, int enumType, MediaRelationType mediaRelationType)
        {
            var sql = new StringBuilder();
            if (mediaRelationType == MediaRelationType.BaseTable)
            {
                sql.AppendFormat(@"DELETE FROM DBO.Media_Remark_Basic WHERE RelationID = {0} AND EnumType = {1}",
                    mediaId, mediaId);
            }
            else
            {
                sql.AppendFormat(@"DELETE FROM DBO.Publish_Remark WHERE RelationID = {0} AND EnumType = {1}",
                      mediaId, mediaId);
            }
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString());
        }
    }
}