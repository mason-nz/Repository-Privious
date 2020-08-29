using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using System.Data.SqlClient;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using System.Data;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    /// <summary>
    /// 暂定的思路：
    /// 一个媒体分别写对应的方法，原因：1、因为涉及到的字段比较多，如果统一集中管理，则参数会暴增，管理不方便
    ///                         2、一个媒体对应 媒体表字段、互动参数字段、覆盖区域表字段、
    /// </summary>
    public class DataImport : DataBase
    {

        /// <summary>
        /// 媒体编辑、新增方法
        /// </summary>
        /// <param name="importDto"></param>
        /// <param name="mediaType">MediaTypeEnum 媒体类型</param>
        /// <returns></returns>
        public int MediaOperate(Import_PCAPPDTO importDto, MediaTypeEnum mediaType)
        {
            var dic = new Dictionary<int, Func<string, int>>()
            {
                {(int)MediaTypeEnum.微信, s=> new Dal.Media.MediaWeixin().Operate(importDto.MediaWeixin,importDto.MediaWeixin.CreateUserID)},
                 {(int)MediaTypeEnum.APP, s=> new Dal.Media.MediaPCAPP().Operate(importDto.MediaPcApp,importDto.MediaPcApp.CreateUserID)},
                  {(int)MediaTypeEnum.直播, s=> new Dal.Media.MediaBroadcast().Operate(importDto.MediaBroadcast,importDto.MediaBroadcast.CreateUserID)},
                   {(int)MediaTypeEnum.微博, s=> new Dal.Media.MediaWeibo().Operate(importDto.MediaWeibo,importDto.MediaWeibo.CreateUserID)},
                    {(int)MediaTypeEnum.视频, s=> new Dal.Media.MediaVideo().Operate(importDto.MediaVideo,importDto.MediaVideo.CreateUserID)}
            };
            var mediaKey = (int)mediaType;
            if (dic.ContainsKey(mediaKey))
            {
                return dic[mediaKey].Invoke(string.Empty);
            }
            throw new Exception("请传入正确的媒体类型枚举");
        }

        /// <summary>
        /// 互动参数编辑、新增方法
        /// </summary>
        /// <param name="importDto"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public int InteractionOperate(Import_PCAPPDTO importDto, MediaTypeEnum mediaType, int mediaId)
        {
            var dic = new Dictionary<int, Func<string, int>>()
            {
                {(int)MediaTypeEnum.微信, s =>
                {
                    importDto.InteractionWeixin.MediaID = mediaId;
                  return  new Dal.Interaction.InteractionWeixin().Operate(importDto.InteractionWeixin,importDto.InteractionWeixin.CreateUserID);
                }},
                  {(int)MediaTypeEnum.直播, s=> new Dal.Interaction.InteractionBroadcast().Operate(importDto.InteractionBroadcast,importDto.InteractionBroadcast.CreateUserID)},
                   {(int)MediaTypeEnum.微博, s=> new Dal.Interaction.InteractionWeibo().Operate(importDto.InteractionWeibo,importDto.InteractionWeibo.CreateUserID)},
                    {(int)MediaTypeEnum.视频, s=> new Dal.Interaction.InteractionVideo().Operate(importDto.InteractionVideo,importDto.InteractionVideo.CreateUserID)}
            };
            var mediaKey = (int)mediaType;
            if (dic.ContainsKey(mediaKey))
            {
                return dic[mediaKey].Invoke(string.Empty);
            }
            throw new Exception("请传入正确的媒体类型枚举");
        }

        /// <summary>
        /// 覆盖区域编辑（暂时没定具体的格式）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public int MediaAreaMappingOperate(Entities.Media.MediaAreaMapping entity, MediaTypeEnum mediaType)
        {
            var mediaAreaMapping = new Dal.Media.MediaAreaMapping();
            mediaAreaMapping.Delete(entity.MediaID, (int)mediaType);//删除

            return 0;
        }



        /// <summary>
        /// 根据用户名查ID
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public int GetUserID(string username)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 UserID from UserInfo ");
            sql.Append("where UserName = @UserName");
            SqlParameter[] parameters = new SqlParameter[]{
			    new SqlParameter("@UserName", SqlDbType.VarChar)
            };
            parameters[0].Value = SqlFilter(username);
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 根据类型 名称 获取字典ID
        /// </summary>
        /// <param name="type">字典类型</param>
        /// <param name="name">字典名称</param>
        /// <returns></returns>
        public int GetDictID(int type, string name)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 DictID from DictInfo ");
            sql.Append("where DictType = @DictType and DictName = @DictName");
            SqlParameter[] parameters = new SqlParameter[]{
			    new SqlParameter("@DictType", SqlDbType.Int),
			    new SqlParameter("@DictName", SqlDbType.VarChar)
            };
            parameters[0].Value = type;
            parameters[1].Value = SqlFilter(name);
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 获取存在媒体的数量
        /// </summary>
        /// <param name="type">媒体类型</param>
        /// <param name="userid">用户ID</param>
        /// <param name="key">关键字,名字或账号</param>
        /// <param name="platform">所属平台,可选</param>
        /// <returns></returns>
        public int GetExistsCount(MediaTypeEnum type, int userid, string key, int platform = 0)
        {

            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (type)
            {

                case MediaTypeEnum.微信:
                    sql.Append("select count(*) from Media_Weixin ");
                    sql.Append("where CreateUserID = @CreateUserID and Number = @Number");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Number", SqlDbType.VarChar)
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    break;
                case MediaTypeEnum.微博:
                    sql.Append("select count(*) from Media_Weibo ");
                    sql.Append("where CreateUserID = @CreateUserID and Number = @Number");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Number", SqlDbType.VarChar)
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    break;
                case MediaTypeEnum.视频:
                    sql.Append("select count(*) from Media_Video ");
                    sql.Append("where CreateUserID = @CreateUserID and Number = @Number and Platform = @Platform");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Number", SqlDbType.VarChar),
			            new SqlParameter("@Platform", SqlDbType.Int)
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    parameters[2].Value = platform;
                    break;
                case MediaTypeEnum.直播:
                    sql.Append("select count(*) from Media_Broadcast ");
                    sql.Append("where CreateUserID = @CreateUserID and Number = @Number and Platform = @Platform");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Number", SqlDbType.VarChar),
			            new SqlParameter("@Platform", SqlDbType.Int)
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    parameters[2].Value = platform;
                    break;
                case MediaTypeEnum.APP:
                    sql.Append("select count(*) from Media_PCAPP ");
                    sql.Append("where CreateUserID = @CreateUserID and Name = @Name");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Name", SqlDbType.VarChar),
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    break;
            }
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 获取字典名称
        /// </summary>
        /// <param name="ids">字典ID字符串 例:14002,14003</param>
        /// <returns></returns>
        public string GetDictNames(string ids)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Ids",SqlDbType.VarChar,500),
                new SqlParameter("@Names",SqlDbType.NVarChar,500)
            };
            parameters[0].Value = ids;
            parameters[1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConvertIDsToNames", parameters);
            return parameters[1].Value.ToString();
        }

        /// <summary>
        /// 获取字典ID
        /// </summary>
        /// <param name="type">DictType</param>
        /// <param name="names">字典名称串 例:'微信','微博'</param>
        /// <returns></returns>
        public string GetDictIds(int type, string names)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Ids",SqlDbType.VarChar,500),
                new SqlParameter("@Type",SqlDbType.Int),
                new SqlParameter("@Names",SqlDbType.NVarChar,500)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = type;
            parameters[2].Value = names;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConvertNamesToIDs", parameters);
            return parameters[0].Value.ToString();
        }


        #region 私有方法

        /// <summary>
        /// 增加刊例基本信息 返回增加的刊例ID
        /// </summary>
        /// <param name="mediaType">媒体类型枚举</param>
        /// <param name="mediaID">媒体ID</param>
        /// <param name="model">刊例对象</param>
        /// <param name="trans">事务对象</param>
        /// <param name="deleteOld">是否删除已经存在的</param>
        /// <returns>返回增加的刊例ID</returns>
        public int AddPubBasic(MediaTypeEnum mediaType, int mediaID, PublishBasicInfo model, SqlTransaction trans, bool deleteOld = false)
        {

            StringBuilder sql = new StringBuilder();
            if (deleteOld)
            {
                sql.Append("delete from Publish_BasicInfo ");
                sql.Append("where MediaType = @MediaType and MediaID = @MediaID;");
                sql.Append("delete from Publish_DetailInfo ");
                sql.Append("where MediaType = @MediaType and MediaID = @MediaID;");
            }
            sql.Append("insert into Publish_BasicInfo(MediaType,MediaID,PubCode,BeginTime,EndTime,PurchaseDiscount,SaleDiscount,Status,null,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID) values(");
            sql.Append("@MediaType,@MediaID,@PubCode,@BeginTime,@EndTime,@PurchaseDiscount,@SaleDiscount,@Status,null,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
            sql.Append(")");
            sql.Append(";select SCOPE_IDENTITY()");
            SqlParameter[] parameters = new SqlParameter[]{
					    new SqlParameter("@MediaType", SqlDbType.Int),
					    new SqlParameter("@MediaID", SqlDbType.Int),
					    new SqlParameter("@PubCode", SqlDbType.VarChar,20),
					    new SqlParameter("@BeginTime", SqlDbType.DateTime),
					    new SqlParameter("@EndTime", SqlDbType.DateTime),
					    new SqlParameter("@PurchaseDiscount", SqlDbType.Decimal),
					    new SqlParameter("@SaleDiscount", SqlDbType.Decimal),
					    new SqlParameter("@Status", SqlDbType.Int),
					    new SqlParameter("@CreateTime", SqlDbType.DateTime),
					    new SqlParameter("@CreateUserID", SqlDbType.Int),
                        new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					    new SqlParameter("@LastUpdateUserID", SqlDbType.Int)
            };
            parameters[0].Value = (int)mediaType;
            parameters[1].Value = mediaID;
            parameters[2].Value = "AP" + (int)mediaType + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(10000, 99999);
            parameters[3].Value = model.BeginTime;
            parameters[4].Value = model.EndTime;
            parameters[5].Value = model.PurchaseDiscount;
            parameters[6].Value = model.SaleDiscount;
            parameters[7].Value = (int)model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.LastUpdateTime;
            parameters[11].Value = model.LastUpdateUserID;
            var obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 增加刊例详细信息 返回增加的DetailID数组
        /// </summary>
        /// <param name="mediaType">媒体类型枚举</param>
        /// <param name="mediaID">媒体ID</param>
        /// <param name="pubid">刊例基本信息pubID</param>
        /// <param name="list">刊例详情list</param>
        /// <param name="trans">事务对象</param>
        /// <param name="deleteOld">是否删除已经存在的</param>
        /// <returns>返回受影响的条数</returns>
        public List<int> AddPubDetail(MediaTypeEnum mediaType, int mediaID, int pubID, List<PublishDetailInfo> list, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into Publish_DetailInfo(PubID,MediaType,MediaID,ADPosition1,ADPosition2,ADPosition3,Price,IsCarousel,BeginPlayDays,null,CreateTime,CreateUserID) values(");
            sql.Append("@PubID,@MediaType,@MediaID,@ADPosition1,@ADPosition2,@ADPosition3,@Price,@IsCarousel,@BeginPlayDays,null,@CreateTime,@CreateUserID");
            sql.Append(")");
            sql.Append(";select SCOPE_IDENTITY()");
            SqlParameter[] parameters = null;
            List<int> res = new List<int>();
            foreach (var item in list)
            {
                parameters = new SqlParameter[]{
					new SqlParameter("@PubID", SqlDbType.Int),
					new SqlParameter("@MediaType", SqlDbType.Int),
					new SqlParameter("@MediaID", SqlDbType.Int),
					new SqlParameter("@ADPosition1", SqlDbType.Int),
					new SqlParameter("@ADPosition2", SqlDbType.Int),
					new SqlParameter("@ADPosition3", SqlDbType.Int),
					new SqlParameter("@Price", SqlDbType.Decimal),
					new SqlParameter("@IsCarousel", SqlDbType.Bit),
					new SqlParameter("@BeginPlayDays", SqlDbType.Int),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int)
                };
                parameters[0].Value = pubID;
                parameters[1].Value = (int)mediaType;
                parameters[2].Value = mediaID;
                parameters[3].Value = item.ADPosition1;
                parameters[4].Value = item.ADPosition2;
                parameters[5].Value = item.ADPosition3;
                parameters[6].Value = item.Price;
                parameters[7].Value = item.IsCarousel;
                parameters[8].Value = item.BeginPlayDays;
                parameters[9].Value = item.CreateTime;
                parameters[10].Value = item.CreateUserID;
                var obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
                res.Add(obj == null ? 0 : Convert.ToInt32(obj));
            }
            return res;
        }

        /// <summary>
        /// 增加媒体互动信息 返回增加的互动信息ID
        /// </summary>
        /// <param name="mediaType">媒体类型枚举</param>
        /// <param name="mediaID">媒体ID</param>
        /// <param name="interaction">互动信息对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public int AddInteraction(MediaTypeEnum mediaType, int mediaID, dynamic interaction, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (mediaType)
            {
                #region 分类拼Sql、参数
                case MediaTypeEnum.微信:
                    sql.Append("insert into Interaction_Weixin(");
                    sql.Append("MeidaType,MediaID,ReferReadCount,AveragePointCount,MoreReadCount,OrigArticleCount,UpdateCount,MaxinumReading,ScreenShotURL,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@MeidaType,@MediaID,@ReferReadCount,@AveragePointCount,@MoreReadCount,@OrigArticleCount,@UpdateCount,@MaxinumReading,@ScreenShotURL,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[] { 
						new SqlParameter("@MeidaType",(int)mediaType),
            			new SqlParameter("@MediaID",mediaID),
            			new SqlParameter("@ReferReadCount",interaction.ReferReadCount),
            			new SqlParameter("@AveragePointCount",interaction.AveragePointCount),
            			new SqlParameter("@MoreReadCount",interaction.MoreReadCount),
            			new SqlParameter("@OrigArticleCount",interaction.OrigArticleCount),
            			new SqlParameter("@UpdateCount",interaction.UpdateCount),
            			new SqlParameter("@MaxinumReading",interaction.MaxinumReading),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@CreateTime",interaction.CreateTime),
            			new SqlParameter("@CreateUserID",interaction.CreateUserID),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                    };

                    break;
                case MediaTypeEnum.微博:
                    sql.Append("insert into Interaction_Weibo(");
                    sql.Append("MeidaType,MediaID,AverageForwardCount,AverageCommentCount,AveragePointCount,ScreenShotURL,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@MeidaType,@MediaID,@AverageForwardCount,@AverageCommentCount,@AveragePointCount,@ScreenShotURL,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[] { 
						new SqlParameter("@MeidaType",(int)mediaType),
            			new SqlParameter("@MediaID",mediaID),
            			new SqlParameter("@AverageForwardCount",interaction.AverageForwardCount),
            			new SqlParameter("@AverageCommentCount",interaction.AverageCommentCount),
            			new SqlParameter("@AveragePointCount",interaction.AveragePointCount),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@CreateTime",interaction.CreateTime),
            			new SqlParameter("@CreateUserID",interaction.CreateUserID),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                    };
                    break;
                case MediaTypeEnum.视频:
                    sql.Append("insert into Interaction_Video(");
                    sql.Append("MeidaType,MediaID,AveragePlayCount,AveragePointCount,AverageCommentCount,AverageBarrageCount,ScreenShotURL,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@MeidaType,@MediaID,@AveragePlayCount,@AveragePointCount,@AverageCommentCount,@AverageBarrageCount,@ScreenShotURL,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[]{
						new SqlParameter("@MeidaType",(int)mediaType),
            			new SqlParameter("@MediaID",mediaID),
            			new SqlParameter("@AveragePlayCount",interaction.AveragePlayCount),
            			new SqlParameter("@AveragePointCount",interaction.AveragePointCount),
            			new SqlParameter("@AverageCommentCount",interaction.AverageCommentCount),
            			new SqlParameter("@AverageBarrageCount",interaction.AverageBarrageCount),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@CreateTime",interaction.CreateTime),
            			new SqlParameter("@CreateUserID",interaction.CreateUserID),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                        };
                    break;
                case MediaTypeEnum.直播:
                    sql.Append("insert into Interaction_Broadcast(");
                    sql.Append("MeidaType,MediaID,AudienceCount,MaximumAudience,AverageAudience,CumulateReward,CumulateIncome,CumulatePoints,CumulateSendCount,ScreenShotURL,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@MeidaType,@MediaID,@AudienceCount,@MaximumAudience,@AverageAudience,@CumulateReward,@CumulateIncome,@CumulatePoints,@CumulateSendCount,@ScreenShotURL,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[] { 
						new SqlParameter("@MeidaType",(int)mediaType),
            			new SqlParameter("@MediaID",mediaID),
            			new SqlParameter("@AudienceCount",interaction.AudienceCount),
            			new SqlParameter("@MaximumAudience",interaction.MaximumAudience),
            			new SqlParameter("@AverageAudience",interaction.AverageAudience),
            			new SqlParameter("@CumulateReward",interaction.CumulateReward),
            			new SqlParameter("@CumulateIncome",interaction.CumulateIncome),
            			new SqlParameter("@CumulatePoints",interaction.CumulatePoints),
            			new SqlParameter("@CumulateSendCount",interaction.CumulateSendCount),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@CreateTime",interaction.CreateTime),
            			new SqlParameter("@CreateUserID",interaction.CreateUserID),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                    };
                    break;
                default:
                    return 0;
                #endregion
            }
            var obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 增加刊例扩展信息 返回增加的刊例扩展信息ID
        /// </summary>
        /// <param name="extend">扩展信息对象</param>
        /// <param name="trans"></param>
        /// <param name="deleteOld"></param>
        /// <returns></returns>
        public int AddPubExtend(int mediaID, int pubID, int adDetailID, PublishExtendInfo extend, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into Publish_ExtendInfoPCAPP(ADDetailID,PubID,MediaType,MediaID,AdLegendURL,AdPosition,AdForm,DisplayLength,CanClick,CarouselCount,PlayPosition,DailyExposureCount,CPM,CarouselPlay,DailyClickCount,CPM2,CarouselPlay2,ThrMonitor,SysPlatform,Style,IsDispatching,ADShow,ADRemark,AcceptBusinessIDs,NotAcceptBusinessIDs,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,AcceptBusinessNames,NotAcceptBusinessNames) values(");
            sql.Append("@ADDetailID,@PubID,14002,@MediaID,@AdLegendURL,@AdPosition,@AdForm,@DisplayLength,@CanClick,@CarouselCount,@PlayPosition,@DailyExposureCount,@CPM,@CarouselPlay,@DailyClickCount,@CPM2,@CarouselPlay2,@ThrMonitor,@SysPlatform,@Style,@IsDispatching,@ADShow,@ADRemark,@AcceptBusinessIDs,@NotAcceptBusinessIDs,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID,@AcceptBusinessNames,@NotAcceptBusinessNames");
            sql.Append(")");
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@MediaID", SqlDbType.Int),
                new SqlParameter("@PubID", SqlDbType.Int),
                new SqlParameter("@ADDetailID", SqlDbType.Int),
				new SqlParameter("@AdLegendURL", SqlDbType.NVarChar,200),
				new SqlParameter("@AdPosition", SqlDbType.NVarChar,100),
				new SqlParameter("@AdForm", SqlDbType.NVarChar,100),
				new SqlParameter("@DisplayLength", SqlDbType.Int),
				new SqlParameter("@CanClick", SqlDbType.Bit),
				new SqlParameter("@CarouselCount", SqlDbType.Int),
				new SqlParameter("@PlayPosition", SqlDbType.NVarChar,100),
				new SqlParameter("@DailyExposureCount", SqlDbType.Int),
				new SqlParameter("@CPM", SqlDbType.Bit),
                new SqlParameter("@CarouselPlay", SqlDbType.Bit),
				new SqlParameter("@DailyClickCount", SqlDbType.Int),
				new SqlParameter("@CPM2", SqlDbType.Bit),
                new SqlParameter("@CarouselPlay2", SqlDbType.Bit),
				new SqlParameter("@ThrMonitor", SqlDbType.VarChar,20),
				new SqlParameter("@SysPlatform", SqlDbType.VarChar,20),
				new SqlParameter("@Style", SqlDbType.NVarChar,100),
				new SqlParameter("@IsDispatching", SqlDbType.Bit),
				new SqlParameter("@ADShow", SqlDbType.VarChar,500),
				new SqlParameter("@ADRemark", SqlDbType.VarChar,500),
				new SqlParameter("@AcceptBusinessIDs", SqlDbType.VarChar,200),
				new SqlParameter("@NotAcceptBusinessIDs", SqlDbType.VarChar,200),
				new SqlParameter("@CreateTime", SqlDbType.DateTime),
				new SqlParameter("@CreateUserID", SqlDbType.Int),
				new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
				new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
				new SqlParameter("@AcceptBusinessNames", SqlDbType.VarChar,200),
				new SqlParameter("@NotAcceptBusinessNames", SqlDbType.VarChar,200)
            };
            parameters[0].Value = mediaID;
            parameters[1].Value = pubID;
            parameters[2].Value = adDetailID;
            parameters[3].Value = SqlFilter(extend.AdLegendURL);
            parameters[4].Value = SqlFilter(extend.AdPosition);
            parameters[5].Value = SqlFilter(extend.AdForm);
            parameters[6].Value = extend.DisplayLength;
            parameters[7].Value = extend.CanClick;
            parameters[8].Value = extend.CarouselCount;
            parameters[9].Value = SqlFilter(extend.PlayPosition);
            parameters[10].Value = extend.DailyExposureCount;
            parameters[11].Value = extend.CPM;
            parameters[12].Value = extend.CarouselPlay;
            parameters[13].Value = extend.DailyClickCount;
            parameters[14].Value = extend.CPM2;
            parameters[15].Value = extend.CarouselPlay2;
            parameters[16].Value = extend.ThrMonitor == null ? null : string.Join(",", extend.ThrMonitor);
            parameters[17].Value = extend.SysPlatform == null ? null : string.Join(",", extend.SysPlatform);
            parameters[18].Value = SqlFilter(extend.Style);
            parameters[19].Value = extend.IsDispatching;
            parameters[20].Value = SqlFilter(extend.ADShow);
            parameters[21].Value = SqlFilter(extend.ADRemark);
            parameters[22].Value = extend.AcceptBusinessIDs == null ? null : string.Join(",", extend.AcceptBusinessIDs);
            parameters[23].Value = extend.NotAcceptBusinessIDs == null ? null : string.Join(",", extend.NotAcceptBusinessIDs);
            parameters[24].Value = extend.CreateTime;
            parameters[25].Value = extend.CreateUserID;
            parameters[26].Value = extend.LastUpdateTime;
            parameters[27].Value = extend.LastUpdateUserID;
            parameters[28].Value = extend.LastUpdateUserID;
            parameters[29].Value = extend.LastUpdateUserID;
            return 0;
        }

        private int AddAreaMapping(List<MediaAreaMapping> list, SqlTransaction trans, bool deleteOld = true)
        {
            return 0;
        }

        #endregion
    }


}
