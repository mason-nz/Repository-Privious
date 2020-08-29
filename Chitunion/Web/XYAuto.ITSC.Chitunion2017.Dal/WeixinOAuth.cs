using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    /// <summary>
    /// 微信授权相关
    /// </summary>
    public class WeixinOAuth : DataBase
    {
        public static readonly WeixinOAuth Instance = new WeixinOAuth();

        #region 权限

        /// <summary>
        /// 添加授权历史信息
        /// </summary>
        /// <param name="his">授权历史实体</param>
        /// <returns>新增ID</returns>
        public int AddOAuthHistory(OAuthHistory his)
        {
            string sql = "insert into OAuth_History(WxID,AppID,Status,CreateTime) values(@WxID,@AppID,@Status,@CreateTime);select SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxID", his.WxID),
                new SqlParameter("@AppID", his.AppID),
                new SqlParameter("@Status", his.Status),
                new SqlParameter("@CreateTime", his.CreateTime)
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
        }

        /// <summary>
        /// 添加授权详情
        /// </summary>
        /// <param name="list">详情List</param>
        /// <returns>受影响行数</returns>
        public int AddOAuthDetail(List<OAuthDetail> list)
        {
            string sql = string.Empty;
            SqlParameter[] parameters = null;
            int total = 0;
            if (list != null && list.Count > 0)
            {
                foreach (var detail in list)
                {
                    sql = "insert into OAuth_Detail(HisID,OAuthID) values(@HisID,@OAuthID)";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@HisID", detail.HisID),
                        new SqlParameter("@OAuthID", detail.OAuthID)
                    };
                    total += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                }
            }
            return total;
        }

        /// <summary>
        /// 根据WxID获取当前授权详情
        /// </summary>
        /// <param name="wxID">微信授权主键</param>
        /// <returns>权限详情List</returns>
        public List<OAuthDetail> GetOAuthDetail(int wxID)
        {
            string sql = "select max(RecID) from OAuth_History where WxID = " + wxID;
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (obj == null)
                return null;
            int hisID = Convert.ToInt32(obj);
            sql = "select * from OAuth_Detail where HisID = " + hisID;
            return DataTableToList<OAuthDetail>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
        }

        public List<string> GetWeixinAuthorityList(int wxID)
        {
            string sql = @"
            SELECT  di.DictName
            FROM    ( SELECT MAX(RecID) AS HisID
                      FROM      dbo.OAuth_History
                      WHERE     WxID = @WxID
                      GROUP BY  WxID
                    ) t
                    LEFT JOIN dbo.OAuth_Detail od ON t.HisID = od.HisID
                    LEFT JOIN dbo.DictInfo di ON od.OAuthID = di.DictId;
            ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxID", wxID),
            };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            List<string> list = new List<string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString());
                }
            }
            return list;
        }

        #endregion 权限

        #region 微信

        /// <summary>
        /// 根据ID获取微信实体
        /// </summary>
        /// <param name="recID">id</param>
        /// <returns>对应实体</returns>
        public WeixinInfo GetWeixinInfoByID(int recID)
        {
            string sql = @"select top 1 Weixin_OAuth.*
                         ,OrderRemarkStr=(
                                        SELECT STUFF((SELECT  '|'
                                                                + RTRIM((CAST(PRKB.RemarkID AS VARCHAR(15))
                                                                          + ',' + ISNULL(DI.DictName, '')
                                                                          + ',' + ISNULL(PRKB.OtherContent,
                                                                                         '')))
                                                        FROM    dbo.Media_Remark_Basic AS PRKB WITH(NOLOCK)
                                                                LEFT JOIN dbo.DictInfo AS DI WITH(NOLOCK) ON PRKB.RemarkID = DI.DictId
                                                        WHERE   RelationID = Weixin_OAuth.RecID
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
                                                    AND MAPB.BaseMediaID = Weixin_OAuth.RecID
                                                    AND MAPB.RelateType = {1}
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
                                                  )
                            from Weixin_OAuth where RecID = " + recID;
            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count.Equals(0))
                return null;
            return DataTableToEntity<WeixinInfo>(dt);
        }

        public WeixinInfo GetWeixinInfo(int wxID)
        {
            string sql = "select * from Weixin_OAuth where RecID = " + wxID;
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToEntity<WeixinInfo>(dt);
        }

        public WeixinInfo GetWeixinInfoByAudit(int recId)
        {
            string sql = @"

                        SELECT  WO.*
                                ,CommonlyClassStr = ( SELECT DC1.DictName + ','
                                                    FROM   dbo.MediaCategory AS MCC WITH ( NOLOCK )
                                                        LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = MCC.CategoryID
                                                    WHERE  MCC.WxID = WO.RecID
                                                        AND MCC.MediaType = 14001
								                        ORDER BY SortNumber DESC
								                        FOR XML PATH('')
                                                ),
					                            AI1.AreaName AS ProvinceName,
					                            AI2.AreaName AS CityName,
                                                DC1.DictName AS LevelTypeName
                                ,OrderRemarkStr=(
                                        SELECT  STUFF(( SELECT  '|'
                                                                + RTRIM(( CAST(PRKB.RemarkID AS VARCHAR(15))
                                                                          + ',' + ISNULL(DI.DictName, '')
                                                                          + ',' + ISNULL(PRKB.OtherContent,
                                                                                         '') ))
                                                        FROM    dbo.Media_Remark_Basic AS PRKB WITH ( NOLOCK )
                                                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON PRKB.RemarkID = DI.DictId
                                                        WHERE   RelationID =WO.RecID
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
                        FROM    dbo.Weixin_OAuth AS WO WITH ( NOLOCK )
                        LEFT JOIN DBO.AreaInfo AS AI1 WITH(NOLOCK) ON AI1.AreaID = WO.ProvinceID
                        LEFT JOIN DBO.AreaInfo AS AI2 WITH(NOLOCK) ON AI2.AreaID = WO.CityID
                        LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = WO.LevelType
                        WHERE WO.Status !=-1 AND WO.RecID =" + recId;

            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            var paras = new List<SqlParameter>()
            {
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.WeixinOAuth.WeixinInfo>(data.Tables[0]);
        }

        public WeixinInfo GetInfo(int recID, int userId)
        {
            string sql = @"
                        SELECT WO.* ,
                                IsExist = (SELECT  CASE WHEN COUNT(1) > 0 THEN 1
                                                         ELSE 0
                                                    END
                                            FROM    dbo.Media_Weixin AS MW WITH (NOLOCK)
                                            WHERE   WO.RecID = MW.WxID AND MW.Status = 0
                                                    AND MW.CreateUserID = {0}
                                          )
                                  ,OrderRemarkStr=(
                                        SELECT  STUFF(( SELECT  '|'
                                                                + RTRIM(( CAST(PRKB.RemarkID AS VARCHAR(15))
                                                                          + ',' + ISNULL(DI.DictName, '')
                                                                          + ',' + ISNULL(PRKB.OtherContent,
                                                                                         '') ))
                                                        FROM    dbo.Media_Remark_Basic AS PRKB WITH ( NOLOCK )
                                                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON PRKB.RemarkID = DI.DictId
                                                        WHERE   RelationID =WO.RecID
									                            AND PRKB.EnumType = {1}
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
                                                    AND MAPB.RelateType = {2}
                                          FOR
                                            XML PATH('')
                                          ), 1, 1, '')
                                                  )
                        FROM dbo.Weixin_OAuth AS WO WITH (NOLOCK)
                        WHERE WO.Status !=-1 AND WO.RecID =" + recID;
            sql = string.Format(sql, userId, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count.Equals(0))
                return null;
            return DataTableToEntity<WeixinInfo>(dt);
        }

        public List<RespSearchMediaDto> SearchMedia(MediaQuery<RespSearchMediaDto> query)
        {
            var sql = @"
                            SELECT TOP ( {0} )
                                    WO.RecID AS BaseMediaId,
                                    WO.WxNumber AS Number ,
                                    WO.NickName AS Name
                            FROM    dbo.Weixin_OAuth AS WO WITH ( NOLOCK )
                            WHERE   WO.Status !=-1 AND WO.WxNumber LIKE '%{1}%'
                        ";

            sql = string.Format(sql, query.PageSize, query.KeyWord);

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToList<RespSearchMediaDto>(dt);
        }

        public WeixinInfo GetEntity(string wxNumber, int filterRecId = 0)
        {
            var sql = @"select top 1 WO.*
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
                            from Weixin_OAuth AS WO WITH(NOLOCK)
                            where WO.Status != -1 AND WxNumber = @WxNumber";

            if (filterRecId > 0)
            {
                sql += " AND RecID != " + filterRecId;
            }
            sql = string.Format(sql, (int)MediaRemarkTypeEnum.微信备注, (int)MediaAreaMappingType.AreaMedia);
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@WxNumber", wxNumber),
           };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            return DataTableToEntity<WeixinInfo>(dt);
        }

        public WeixinInfo GetWeixinInfoByWxNumber(string wxNumber, int filterRecId = 0, int status = 0)
        {
            string sql = string.Empty;
            if (status.Equals(-1))
            {
                sql = "select top 1 Weixin_OAuth.* from Weixin_OAuth where WxNumber = '" + wxNumber + "'";
            }
            else
            {
                sql = "select top 1 Weixin_OAuth.* from Weixin_OAuth where status=" + status + " and WxNumber = '" + wxNumber + "'";
            }
            if (filterRecId > 0)
            {
                sql += " and RecID != " + filterRecId;
            }
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count.Equals(0))
                return null;
            return DataTableToEntity<WeixinInfo>(dt);
        }

        public WeixinInfo GetWeixinInfoByAppID(string appID)
        {
            string sql = "select top 1 Weixin_OAuth.* from Weixin_OAuth where AppID = '" + appID + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count.Equals(0))
                return null;
            return DataTableToEntity<WeixinInfo>(dt);
        }

        public string GetWxNumberByMediaID(int mediaID)
        {
            string sql = "select Number from Media_Weixin where Media_Weixin.MediaID = " + mediaID;
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? string.Empty : obj.ToString();
        }

        public int GetWxIDByWxNumber(string wxNumber)
        {
            string sql = "select top 1 RecID from Weixin_OAuth where WxNumber = @WxNumber";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxNumber",SqlFilter(wxNumber))
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int GetWxIDByBiz(string biz)
        {
            string sql = "select top 1 RecID from Weixin_OAuth where Biz = @Biz";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Biz",SqlFilter(biz))
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int CheckHasRight(int oAuthID, int mediaID, out int wxID)
        {
            wxID = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@OAuthID", oAuthID),
                new SqlParameter("@WxID" ,wxID),
            };
            parameters[2].Direction = ParameterDirection.Output;
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Weixin_OAuth_HasRight1_1", parameters);
            wxID = Convert.ToInt32(parameters[2].Value);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 删除过期数据 两天前
        /// </summary>
        /// <returns></returns>
        public int ClearOverDueDate()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            delete from OAuth_Detail where exists (
                select 1 from OAuth_History where OAuth_History.RecID = OAuth_Detail.HisID and exists(
                    select 1 from Weixin_OAuth where Status = 1 and Weixin_OAuth.RecID = OAuth_History.WxID
            ));");
            sb.Append(@"
            delete from OAuth_History where exists(
                select 1 from Weixin_OAuth where Status = 1 and Weixin_OAuth.RecID = OAuth_History.WxID
            );");
            sb.Append(@"
            delete from Article_Weixin where exists(
                select 1 from Weixin_OAuth where Status = 1 and Weixin_OAuth.RecID = Article_Weixin.WxID
            );");
            sb.Append(@"
            delete from ReadStatistic_Weixin where exists(
                select 1 from Weixin_OAuth where Status = 1 and Weixin_OAuth.RecID = ReadStatistic_Weixin.WxID
            );");
            sb.Append(@"
            delete from UpdateStatistic_Weixin where exists(
                select 1 from Weixin_OAuth where Status = 1 and Weixin_OAuth.RecID = UpdateStatistic_Weixin.WxID
            );");
            sb.Append(@"delete from Weixin_OAuth where
                Status = 1 and CreateTime < cast('" + DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd") + "' as datetime);");
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }

        /// <summary>
        /// 添加微信信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddWeixinInfo(WeixinInfo info)
        {
            string sql = @"insert into Weixin_OAuth(AppID,AccessToken,RefreshAccessToken,GetTokenTime,
            WxNumber,OriginalID,NickName,ServiceType,IsVerify,VerifyType,HeadImg,QrCodeUrl,FansCount,
            Biz,Status,OAuthStatus,RegTime,CreateTime,ModifyTime,SourceType,Summary,FullName,CreditCode,BusinessScope,
            EnterpriseType,EnterpriseCreateDate,EnterpriseBusinessTerm,EnterpriseVerifyDate,Location,LevelType,
            ProvinceID,CityID,Sign,IsAreaMedia)
            values(
            @AppID,@AccessToken,@RefreshAccessToken,@GetTokenTime,@WxNumber,@OriginalID,@NickName,
            @ServiceType,@IsVerify,@VerifyType,@HeadImg,@QrCodeUrl,@FansCount,@Biz,@Status,@OAuthStatus,@RegTime,
            @CreateTime,@ModifyTime,@SourceType,@Summary,@FullName,@CreditCode,@BusinessScope,
            @EnterpriseType,@EnterpriseCreateDate,@EnterpriseBusinessTerm,@EnterpriseVerifyDate,@Location,
            @LevelType,@ProvinceID,@CityID,@Sign,@IsAreaMedia
            );
            select SCOPE_IDENTITY() ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AppID", info.AppID),
                new SqlParameter("@AccessToken", info.AccessToken),
                new SqlParameter("@RefreshAccessToken", info.RefreshAccessToken),
                new SqlParameter("@GetTokenTime", info.GetTokenTime),
                new SqlParameter("@WxNumber", info.WxNumber),
                new SqlParameter("@OriginalID", info.OriginalID),
                new SqlParameter("@NickName", info.NickName),
                new SqlParameter("@ServiceType", info.ServiceType),
                new SqlParameter("@IsVerify", info.IsVerify),
                new SqlParameter("@VerifyType", info.VerifyType),
                new SqlParameter("@HeadImg", info.HeadImg),
                new SqlParameter("@QrCodeUrl", info.QrCodeUrl),
                new SqlParameter("@FansCount", info.FansCount),
                new SqlParameter("@Biz", info.Biz),
                new SqlParameter("@Status", info.Status),
                new SqlParameter("@OAuthStatus", info.OAuthStatus),
                new SqlParameter("@RegTime", info.RegTime),
                new SqlParameter("@CreateTime", info.CreateTime),
                new SqlParameter("@ModifyTime", info.ModifyTime),
                new SqlParameter("@SourceType", info.SourceType),
                new SqlParameter("@Summary", info.Summary),
                new SqlParameter("@FullName", info.FullName),
                new SqlParameter("@CreditCode", info.CreditCode),
                new SqlParameter("@BusinessScope", info.BusinessScope),
                new SqlParameter("@EnterpriseType", info.EnterpriseType),
                new SqlParameter("@EnterpriseCreateDate", info.EnterpriseCreateDate),
                new SqlParameter("@EnterpriseBusinessTerm", info.EnterpriseBusinessTerm),
                new SqlParameter("@EnterpriseVerifyDate", info.EnterpriseVerifyDate),
                new SqlParameter("@Location", info.Location),
                new SqlParameter("@LevelType", info.LevelType),
                new SqlParameter("@ProvinceID", info.ProvinceID),
                new SqlParameter("@CityID", info.CityID),
                new SqlParameter("@Sign", info.Sign),
                new SqlParameter("@IsAreaMedia", info.IsAreaMedia),
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 更新微信信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateWeixinInfo(WeixinInfo info)
        {
            string sql = @"Update Weixin_OAuth set
            AppID = @AppID,
            AccessToken = @AccessToken,
            RefreshAccessToken = @RefreshAccessToken,
            GetTokenTime = @GetTokenTime,
            WxNumber = @WxNumber,
            OriginalID = @OriginalID,
            NickName = @NickName,
            ServiceType = @ServiceType,
            IsVerify = @IsVerify,
            VerifyType = @VerifyType,
            HeadImg = @HeadImg,
            QrCodeUrl = @QrCodeUrl,
            FansCount = @FansCount,
            Biz = @Biz,
            Status = @Status,
            OAuthStatus = @OAuthStatus,
            RegTime = @RegTime,
            CreateTime = @CreateTime,
            ModifyTime = @ModifyTime,
            SourceType = @SourceType,
            Summary = @Summary,
            FullName = @FullName,
            CreditCode = @CreditCode,
            BusinessScope = @BusinessScope,
            EnterpriseType = @EnterpriseType,
            EnterpriseCreateDate = @EnterpriseCreateDate,
            EnterpriseBusinessTerm = @EnterpriseBusinessTerm,
            EnterpriseVerifyDate = @EnterpriseVerifyDate,
            Location = @Location,
            LevelType = @LevelType,
            ProvinceID = @ProvinceID,
            CityID = @CityID,
            Sign = @Sign,
            IsAreaMedia = @IsAreaMedia
            where RecID = @RecID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AppID", info.AppID),
                new SqlParameter("@AccessToken", info.AccessToken),
                new SqlParameter("@RefreshAccessToken", info.RefreshAccessToken),
                new SqlParameter("@GetTokenTime", info.GetTokenTime),
                new SqlParameter("@WxNumber", info.WxNumber),
                new SqlParameter("@OriginalID", info.OriginalID),
                new SqlParameter("@NickName", info.NickName),
                new SqlParameter("@ServiceType", info.ServiceType),
                new SqlParameter("@IsVerify", info.IsVerify),
                new SqlParameter("@VerifyType", info.VerifyType),
                new SqlParameter("@HeadImg", info.HeadImg),
                new SqlParameter("@QrCodeUrl", info.QrCodeUrl),
                new SqlParameter("@FansCount", info.FansCount),
                new SqlParameter("@Biz", info.Biz),
                new SqlParameter("@Status", info.Status),
                new SqlParameter("@OAuthStatus", info.OAuthStatus),
                new SqlParameter("@RegTime", info.RegTime),
                new SqlParameter("@CreateTime", info.CreateTime),
                new SqlParameter("@ModifyTime", info.ModifyTime),
                new SqlParameter("@SourceType", info.SourceType),
                new SqlParameter("@Summary", info.Summary),
                new SqlParameter("@FullName", info.FullName),
                new SqlParameter("@CreditCode", info.CreditCode),
                new SqlParameter("@BusinessScope", info.BusinessScope),
                new SqlParameter("@EnterpriseType", info.EnterpriseType),
                new SqlParameter("@EnterpriseCreateDate", info.EnterpriseCreateDate),
                new SqlParameter("@EnterpriseBusinessTerm", info.EnterpriseBusinessTerm),
                new SqlParameter("@EnterpriseVerifyDate", info.EnterpriseVerifyDate),
                new SqlParameter("@Location", info.Location),
                new SqlParameter("@LevelType", info.LevelType),
                new SqlParameter("@ProvinceID", info.ProvinceID),
                new SqlParameter("@CityID", info.CityID),
                new SqlParameter("@Sign", info.Sign),
                new SqlParameter("@RecID", info.RecID),
                new SqlParameter("@IsAreaMedia", info.IsAreaMedia)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int UpdateTokenInfo(string appID, string accessToken, string refreshAccessToken, DateTime getTokenTime)
        {
            string sql = @"Update Weixin_OAuth set AccessToken = @AccessToken,RefreshAccessToken = @RefreshAccessToken,
                               GetTokenTime = @GetTokenTime where AppID = @AppID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AccessToken", accessToken),
                new SqlParameter("@RefreshAccessToken", refreshAccessToken),
                new SqlParameter("@GetTokenTime", getTokenTime),
                new SqlParameter("@AppID", appID)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        //public List<WeixinInfo> GetWeixinUpdateList(int count) {
        //    string sql = string.Format(@"
        //    select {0} Weixin_OAuth.*,t.HisID from Weixin_OAuth inner join (
        //        select RecID as HisID,AppID,Status as OAuthStatus from OAuth_History
        //        where RecID in (
        //            select max(RecID) from OAuth_History group by AppID
        //        )
        //    )t on Weixin_OAuth.AppID = t.AppID
        //    where Weixin_OAuth.OAuthType = 1 and t.OAuthStatus = 0
        //    order by ModifyTime asc", count.Equals(-1)?string.Empty:("top " +count));
        //    DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
        //    return DataTableToList<WeixinInfo>(ds.Tables[0]);
        //}

        public List<WeixinInfo> GetWeixinList()
        {
            string sql = @"select * from Weixin_OAuth where Weixin_OAuth.Status = 0
                                order by RecID desc";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<WeixinInfo>(ds.Tables[0]);
        }

        #endregion 微信

        #region 图文

        /// <summary>
        /// 添加图文信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddArticle(Article info)
        {
            string sql = @"insert into Article_Weixin(
                                WxID,AppID,MsgID,PubDate,ArticleType,
                                IntReadUserCount,IntReadCount,OriReadUserCount,OriReadCount,ShareUserCount,
                                ShareCount,CreateTime
                                )
                                values(
                                @WxID,@AppID,@MsgID,@PubDate,@ArticleType,
                                @IntReadUserCount,@IntReadCount,@OriReadUserCount,@OriReadCount,@ShareUserCount,
                                @ShareCount,@CreateTime
                                );select SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxID", info.WxID),
                new SqlParameter("@AppID", info.AppID),
                new SqlParameter("@MsgID", info.MsgID),
                new SqlParameter("@PubDate", info.PubDate),
                new SqlParameter("@ArticleType", info.ArticleType),
                new SqlParameter("@IntReadUserCount", info.IntReadUserCount),
                new SqlParameter("@IntReadCount", info.IntReadCount),
                new SqlParameter("@OriReadUserCount", info.OriReadUserCount),
                new SqlParameter("@OriReadCount", info.OriReadCount),
                new SqlParameter("@ShareUserCount", info.ShareUserCount),
                new SqlParameter("@ShareCount", info.ShareCount),
                new SqlParameter("@CreateTime", info.CreateTime)
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
        }

        public int UpdateArticle(Article info)
        {
            string sql = @"update Article_Weixin set IntReadUserCount = @IntReadUserCount,IntReadCount = @IntReadCount,
                               OriReadUserCount = @OriReadUserCount,OriReadCount = @OriReadCount,
                               ShareUserCount = @ShareUserCount,ShareCount = @ShareCount where MsgID = @MsgID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IntReadUserCount", info.IntReadUserCount),
                new SqlParameter("@IntReadCount", info.IntReadCount),
                new SqlParameter("@OriReadUserCount", info.OriReadUserCount),
                new SqlParameter("@OriReadCount", info.OriReadCount),
                new SqlParameter("@ShareUserCount", info.ShareUserCount),
                new SqlParameter("@ShareCount", info.ShareCount),
                new SqlParameter("@MsgID", info.MsgID),
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 获取一个微信公众号指定间隔内已经统计到的日期
        /// </summary>
        /// <param name="wxID"></param>
        /// <param name="dayInterval"></param>
        /// <returns></returns>
        public List<Article> GetArticleListByDate(int wxID, int dayInterval)
        {
            string now = DateTime.Now.AddDays(dayInterval).ToString("yyyy-MM-dd");
            string sql = @"select * from Article_Weixin
                                where WxID = " + wxID + "  and CONVERT(varchar, PubDate, 23) >= '" + now + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToList<Article>(dt);
        }

        #endregion 图文

        #region Component

        public int UpdateVerifyTicket(string appID, string verifyTicket, DateTime ticketTime)
        {
            string sql = @"Update Weixin_Component set VerifyTicket = @VerifyTicket,TicketTime = @TicketTime
                                where AppID = @AppID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@VerifyTicket", verifyTicket),
                new SqlParameter("@TicketTime", ticketTime),
                new SqlParameter("@AppID", appID),
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int UpdateAccessToken(string appID, string accessToken, DateTime accessTokenTime)
        {
            string sql = @"Update Weixin_Component set AccessToken = @AccessToken,AccessTokenTime = @AccessTokenTime
                                where AppID = @AppID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AccessToken", accessToken),
                new SqlParameter("@AccessTokenTime", accessTokenTime),
                new SqlParameter("@AppID", appID),
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public Component GetComponentDetail(string appID)
        {
            string sql = "select top 1 * from Weixin_Component where AppID = @AppID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AppID", appID),
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return DataTableToEntity<Component>(ds.Tables[0]);
        }

        #endregion Component

        #region 统计

        public int AddReadStatistic(ReadStatistic_Weixin info)
        {
            string sql = @"insert into ReadStatistic_Weixin(WxID,ArticleType,
            AverageReading,MaxReading,FromDate,EndDate,CreateTime) values(@WxID,@ArticleType,
            @AverageReading,@MaxReading,@FromDate,@EndDate,@CreateTime);
            select SCOPE_IDENTITY() ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxID", info.WxID),
                new SqlParameter("@ArticleType", info.ArticleType),
                new SqlParameter("@AverageReading", info.AverageReading),
                new SqlParameter("@MaxReading", info.MaxReading),
                new SqlParameter("@FromDate", info.FromDate),
                new SqlParameter("@EndDate", info.EndDate),
                new SqlParameter("@CreateTime", info.CreateTime),
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
        }

        public int AddUpdateStatistic(UpdateStatistic_Weixin info)
        {
            string sql = @"insert into UpdateStatistic_Weixin(WxID,UpdateCount,
            StatisticDate,CreateTime) values(@WxID,@UpdateCount,
            @StatisticDate,@CreateTime);
            select SCOPE_IDENTITY() ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@WxID", info.WxID),
                new SqlParameter("@UpdateCount", info.UpdateCount),
                new SqlParameter("@StatisticDate", info.StatisticDate),
                new SqlParameter("@CreateTime", info.CreateTime)
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
        }

        public int ClearReadStatistic(int wxID)
        {
            string sql = "delete from ReadStatistic_Weixin where WxID = " + wxID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public List<ReadStatistic_Weixin> GetReadStatistic_Weixin(int wxID, string dateStr)
        {
            string sql = "select * from ReadStatistic_Weixin where CONVERT(varchar, CreateTime, 23) = '" + dateStr + "' and WxID = " + wxID;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<ReadStatistic_Weixin>(ds.Tables[0]);
        }

        #endregion 统计

        public string GetBizByWxNumber(string wxNumber)
        {
            string sql = "SELECT TOP 1 BIZ FROM ExportWXIDAndBiz WHERE WXID = '"+wxNumber+"'";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? string.Empty : obj.ToString();

        }
    }
}