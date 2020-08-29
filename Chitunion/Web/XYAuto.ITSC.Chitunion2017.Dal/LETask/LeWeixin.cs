using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.WeixinOAuth;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //LE_Weixin
    public partial class LeWeixin : DataBase
    {


        public static readonly LeWeixin Instance = new LeWeixin();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeWeixin entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_Weixin(");
            strSql.Append("AppID,AccessToken,RefreshAccessToken,GetTokenTime,WxNumber,OriginalID,NickName,ServiceType,IsVerify,VerifyType,HeadImg,QrCodeUrl,FansCount,Biz,Status,OAuthStatus,RegTime,CreateTime,ModifyTime,SourceType,Summary,FullName,CreditCode,BusinessScope,EnterpriseType,EnterpriseCreateDate,EnterpriseBusinessTerm,EnterpriseVerifyDate,Location,LevelType,ProvinceID,CityID,Sign,IsAreaMedia,ReadNum,CategoryID,IsOriginal,TimestampSign,CreateUserID");
            strSql.Append(",ManFansRatio,WomanFansRatio");

            strSql.Append(") values (");
            strSql.Append("@AppID,@AccessToken,@RefreshAccessToken,@GetTokenTime,@WxNumber,@OriginalID,@NickName,@ServiceType,@IsVerify,@VerifyType,@HeadImg,@QrCodeUrl,@FansCount,@Biz,@Status,@OAuthStatus,@RegTime,@CreateTime,@ModifyTime,@SourceType,@Summary,@FullName,@CreditCode,@BusinessScope,@EnterpriseType,@EnterpriseCreateDate,@EnterpriseBusinessTerm,@EnterpriseVerifyDate,@Location,@LevelType,@ProvinceID,@CityID,@Sign,@IsAreaMedia,@ReadNum,@CategoryID,@IsOriginal,@TimestampSign,@CreateUserID");
            strSql.Append(",@ManFansRatio,@WomanFansRatio");

            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AppID",entity.AppID),
                        new SqlParameter("@AccessToken",entity.AccessToken),
                        new SqlParameter("@RefreshAccessToken",entity.RefreshAccessToken),
                        new SqlParameter("@GetTokenTime",entity.GetTokenTime),
                        new SqlParameter("@WxNumber",entity.WxNumber),
                        new SqlParameter("@OriginalID",entity.OriginalID),
                        new SqlParameter("@NickName",entity.NickName),
                        new SqlParameter("@ServiceType",entity.ServiceType),
                        new SqlParameter("@IsVerify",entity.IsVerify),
                        new SqlParameter("@VerifyType",entity.VerifyType),
                        new SqlParameter("@HeadImg",entity.HeadImg),
                        new SqlParameter("@QrCodeUrl",entity.QrCodeUrl),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@Biz",entity.Biz),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@OAuthStatus",entity.OAuthStatus),
                        new SqlParameter("@RegTime",entity.RegTime),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@ModifyTime",entity.ModifyTime),
                        new SqlParameter("@SourceType",entity.SourceType),
                        new SqlParameter("@Summary",entity.Summary),
                        new SqlParameter("@FullName",entity.FullName),
                        new SqlParameter("@CreditCode",entity.CreditCode),
                        new SqlParameter("@BusinessScope",entity.BusinessScope),
                        new SqlParameter("@EnterpriseType",entity.EnterpriseType),
                        new SqlParameter("@EnterpriseCreateDate",entity.EnterpriseCreateDate),
                        new SqlParameter("@EnterpriseBusinessTerm",entity.EnterpriseBusinessTerm),
                        new SqlParameter("@EnterpriseVerifyDate",entity.EnterpriseVerifyDate),
                        new SqlParameter("@Location",entity.Location),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@Sign",entity.Sign),
                        new SqlParameter("@IsAreaMedia",entity.IsAreaMedia),
                        new SqlParameter("@ReadNum",entity.ReadNum),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@IsOriginal",entity.IsOriginal),
                        new SqlParameter("@TimestampSign",entity.TimestampSign),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@ManFansRatio",entity.ManFansRatio),
                        new SqlParameter("@WomanFansRatio",entity.WomanFansRatio),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public Entities.LETask.LeWeixin GetInfoAndPrice(int mediaId)
        {
            var sql = $@"

                    
SELECT  WX.RecID AS MediaId,
        WX.WxNumber ,
        WX.OriginalID ,
        WX.NickName ,
        WX.HeadImg ,
        WX.QrCodeUrl ,
        WX.FansCount ,
        WX.CategoryID ,
        WX.ManFansRatio,
        WX.WomanFansRatio,
        MM.ProvinceID AS MpProvinceId,
		MM.CityID AS MpCityId,
        PricesInfo = ( SELECT   STUFF(( SELECT  '|'
                                                                + ISNULL(CAST(PD.ADPosition1 AS VARCHAR(100)), '') + ','
                                                                + ISNULL(CAST(PD.ADPosition2 AS VARCHAR(100)), '') + ','
                                                                + ISNULL(CAST(PD.Price AS VARCHAR(100)), '')
                                                        FROM    dbo.LE_PublishDetailInfo AS PD WITH ( NOLOCK )
                                                                LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = PD.ADPosition1
                                                                LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON DC2.DictId = PD.ADPosition2
                                                        WHERE   PD.MediaType = 14001
                                                                AND PD.MediaID = WX.RecID
                                                      FOR XML PATH('')
                                                      ), 1, 1, '')
                     )
FROM    dbo.LE_Weixin AS WX WITH ( NOLOCK )
        LEFT JOIN DBO.LE_MediaArea_Mapping AS MM WITH(NOLOCK) ON MM.MediaID = WX.RecID AND MM.MediaType = 14001
WHERE   WX.RecID = {mediaId}
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWeixin>(obj.Tables[0]);
        }


        /// <summary>
        /// 现在修改字段比较少，
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="fansCount"></param>
        /// <param name="categoryId"></param>
        /// <param name="manFansRatio"></param>
        /// <param name="womanFansRatio"></param>
        /// <returns></returns>
        public int UpdateOffer(int mediaId, int fansCount, int categoryId, decimal manFansRatio, decimal womanFansRatio)
        {
            var sql = @"

                UPDATE DBO.LE_Weixin SET FansCount = @FansCount,ManFansRatio = @ManFansRatio
                                        ,WomanFansRatio = @WomanFansRatio,CategoryID = @CategoryID
                WHERE RecID = @RecID
                ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("@RecID",mediaId),
                new SqlParameter("@FansCount",fansCount),
                new SqlParameter("@CategoryID",categoryId),
                new SqlParameter("@ManFansRatio",manFansRatio),
                        new SqlParameter("@WomanFansRatio",womanFansRatio),
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public Entities.LETask.LeWeixin GetInfo(int mediaId)
        {
            var sql = $@"

                    SELECT  WX.*
                    FROM    dbo.LE_Weixin AS WX WITH ( NOLOCK )
                    WHERE   WX.RecID = {mediaId};
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWeixin>(obj.Tables[0]);
        }

        #region 授权相关

        /// <summary>
        /// 更新微信信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateWeixinInfo(WeixinInfo info)
        {
            string sql = @"Update LE_Weixin set
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
            IsAreaMedia = @IsAreaMedia ,
            CreateUserID = @CreateUserID
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
                  new SqlParameter("@CreateUserID", info.UserId),
                new SqlParameter("@IsAreaMedia", info.IsAreaMedia)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 添加微信信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int AddWeixinInfo(WeixinInfo info)
        {
            string sql = @"insert into LE_Weixin(AppID,AccessToken,RefreshAccessToken,GetTokenTime,
            WxNumber,OriginalID,NickName,ServiceType,IsVerify,VerifyType,HeadImg,QrCodeUrl,FansCount,
            Biz,Status,OAuthStatus,RegTime,CreateTime,ModifyTime,SourceType,Summary,FullName,CreditCode,BusinessScope,
            EnterpriseType,EnterpriseCreateDate,EnterpriseBusinessTerm,EnterpriseVerifyDate,Location,LevelType,
            ProvinceID,CityID,Sign,IsAreaMedia,CreateUserID)
            values(
            @AppID,@AccessToken,@RefreshAccessToken,@GetTokenTime,@WxNumber,@OriginalID,@NickName,
            @ServiceType,@IsVerify,@VerifyType,@HeadImg,@QrCodeUrl,@FansCount,@Biz,@Status,@OAuthStatus,@RegTime,
            @CreateTime,@ModifyTime,@SourceType,@Summary,@FullName,@CreditCode,@BusinessScope,
            @EnterpriseType,@EnterpriseCreateDate,@EnterpriseBusinessTerm,@EnterpriseVerifyDate,@Location,
            @LevelType,@ProvinceID,@CityID,@Sign,@IsAreaMedia,@CreateUserID
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
                 new SqlParameter("@CreateUserID", info.UserId),
                new SqlParameter("@IsAreaMedia", info.IsAreaMedia),
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public WeixinInfo GetWeixinInfoByAppId(string appId)
        {
            string sql = "select top 1 LE_Weixin.* from LE_Weixin where AppID = '" + appId + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count.Equals(0))
                return null;
            return DataTableToEntity<WeixinInfo>(dt);
        }
        

        #endregion
    }
}

