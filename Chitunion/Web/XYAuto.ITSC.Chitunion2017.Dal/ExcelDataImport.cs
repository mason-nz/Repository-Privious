using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class ExcelDataImport:DataBase
    {


        public int InsertOne(ImportDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int mediaID = AddMediaInfo(dto.MediaType, dto.MediaInfo, trans);
                        if (mediaID.Equals(0))
                            throw new Exception("插入媒体失败");
                        if(dto.MappingList != null && dto.MappingList.Count >0)
                            AddAreaMapping(dto.MediaType, mediaID, dto.MappingList, trans,false);
                        if(dto.MediaType != MediaTypeEnum.APP)
                            AddInteraction(dto.MediaType, mediaID, dto.Interaction, trans);
                        int pubID = AddPubBasic(dto.MediaType, mediaID, dto.PubBasicInfo, trans);
                        if(pubID.Equals(0))
                            throw new Exception("插入刊例基本信息失败");
                        if(AddAuditInfo(dto.MediaType,pubID,dto.PubBasicInfo.CreateUserID).Equals(0))
                            throw new Exception("插入刊例审核信息失败");
                        List<int> detailIDs = AddPubDetail(dto.MediaType, mediaID, pubID, dto.PubDetailList, trans);
                        //if (detailIDs == null || detailIDs.Count.Equals(0))
                           // throw new Exception("插入刊例详细信息失败");
                        if (dto.MediaType.Equals(MediaTypeEnum.APP))
                        {
                            AddPubExtend(mediaID, pubID, detailIDs.First(), dto.PubExtend, trans);
                        }
                        trans.Commit();
                        return mediaID;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                    }
                }
            }
        }

        public int UpdateOne(ImportDTO dto) {

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //5月24日 APP导入 重复名称的不更新主体信息
                        int count = 0;
                        //int count = UpdateMediaInfo(dto.MediaType, dto.MediaInfo, trans);
                        //if (count.Equals(0))
                            //throw new Exception("更新媒体信息失败");
                        //覆盖区域删除旧的
                        //if (dto.MappingList != null && dto.MappingList.Count > 0)
                           // AddAreaMapping(dto.MediaType, dto.MediaInfo.MediaID, dto.MappingList, trans);
                        //互动信息
                        //if (!dto.MediaType.Equals(MediaTypeEnum.APP))
                        //{
                            //int recID = GetExistsInteraction(dto.MediaType, dto.MediaInfo.MediaID);
                            //if (recID.Equals(0))
                                //AddInteraction(dto.MediaType, dto.MediaInfo.MediaID, dto.Interaction, trans);
                            //else
                                //UpdateInteraction(dto.MediaType, recID, dto.Interaction, trans);
                        //}
                        //刊例基本信息
                        int pubID = GetExistsPublish(dto.MediaType, dto.MediaInfo.MediaID);
                        if (pubID.Equals(0))
                        {
                            pubID = AddPubBasic(dto.MediaType, dto.MediaInfo.MediaID, dto.PubBasicInfo, trans);
                            if (pubID.Equals(0))
                                throw new Exception("插入刊例基本信息失败");
                        }
                        else
                        {
                            count = UpdatePubBasic(dto.MediaType, pubID, dto.PubBasicInfo, trans);
                            if (count.Equals(0))
                                throw new Exception("更新刊例基本信息失败");
                        }
                        List<int> detailIDs = AddPubDetail(dto.MediaType, dto.MediaInfo.MediaID, pubID, dto.PubDetailList, trans);
                        //if (detailIDs == null || detailIDs.Count.Equals(0))
                           // throw new Exception("插入刊例详细信息失败");
                        if (dto.MediaType.Equals(MediaTypeEnum.APP))
                        {
                            AddPubExtend(dto.MediaInfo.MediaID, pubID, detailIDs.First(), dto.PubExtend, trans);
                        }
                        trans.Commit();
                        return dto.MediaInfo.MediaID;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                    }
                }
            }
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
            if (string.IsNullOrWhiteSpace(name))
                return -1;
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 DictID from DictInfo ");
            sql.Append("where DictType = @DictType and DictName = @DictName");
            SqlParameter[] parameters = new SqlParameter[]{
			    new SqlParameter("@DictType", SqlDbType.Int),
			    new SqlParameter("@DictName", SqlDbType.VarChar)
            };
            parameters[0].Value = type;
            parameters[1].Value = name;
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? -1 : Convert.ToInt32(obj);
        }
        
        /// <summary>
        /// 根据所在地 省名-市名 获取省市对应ID 没有为-1
        /// </summary>
        /// <param name="location">所在地</param>
        /// <returns>省ID,市ID</returns>
        public Tuple<int, int> GetArea(string location) {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                    return new Tuple<int, int>(-1,-1);
                string[] area = location.Split('-');
                string pname = area[0];
                string sql = "select top 1 AreaID from AreaInfo where Level = 1 and AreaName = @AreaName";
                SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@AreaName",pname),
                };
                int pid = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
                pid = pid.Equals(0) ? -1 : pid;
                if (!pid.Equals(0) && area.Length > 1)//排除全国
                {
                    string cname = area[1];
                    sql = "select top 1 AreaID from AreaInfo where Level = 2 and AreaName = @AreaName and PID = @PID";
                        parameters = new SqlParameter[] { 
                        new SqlParameter("@AreaName",cname),
                        new SqlParameter("@PID",pid),
                     };
                    int cid = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
                    cid = cid.Equals(0) ? -1 : cid;
                    return new Tuple<int, int>(pid,cid);
                }
                return new Tuple<int, int>(pid, -1);
            }
            catch {
                return new Tuple<int, int>(-1,-1);
            }
        }

        /// <summary>
        /// 获取存在媒体ID
        /// </summary>
        /// <param name="type">媒体类型</param>
        /// <param name="userid">用户ID</param>
        /// <param name="key">关键字,名字或账号</param>
        /// <param name="platform">所属平台,可选</param>
        /// <returns></returns>
        public int GetExistsMedia(MediaTypeEnum mediaType, int userid, string key, int platform = 0)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (mediaType)
            {
                case MediaTypeEnum.微信:
                    sql.Append("select top 1 MediaID from Media_Weixin ");
                    sql.Append("where CreateUserID = @CreateUserID and Number = @Number");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Number", SqlDbType.VarChar)
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    break;
                case MediaTypeEnum.微博:
                    sql.Append("select top 1 MediaID from Media_Weibo ");
                    sql.Append("where CreateUserID = @CreateUserID and Name = @Name");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@CreateUserID", SqlDbType.Int),
			            new SqlParameter("@Name", SqlDbType.VarChar)
                    };
                    parameters[0].Value = userid;
                    parameters[1].Value = key;
                    break;
                case MediaTypeEnum.视频:
                    sql.Append("select top 1 MediaID from Media_Video ");
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
                    sql.Append("select top 1 MediaID from Media_Broadcast ");
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
                    sql.Append("select top 1 MediaID from Media_PCAPP ");
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

        /// <summary>
        /// 获取存在的刊例基本信息ID
        /// </summary>
        /// <param name="type">媒体类型</param>
        /// <param name="mediaID">媒体ID</param>
        /// <returns></returns>
        private int GetExistsPublish(MediaTypeEnum mediaType, int mediaID) {
            StringBuilder sql = new StringBuilder();
            sql.Append("select top 1 PubID from Publish_BasicInfo ");
            sql.Append("where MediaType = @MediaType and MediaID = @MediaID");
            SqlParameter[] parameters = new SqlParameter[]{
			    new SqlParameter("@MediaType", SqlDbType.Int),
			    new SqlParameter("@MediaID", SqlDbType.Int),
            };
            parameters[0].Value = (int)mediaType;
            parameters[1].Value = mediaID;
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 获取存在的互动信息ID
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaID"></param>
        /// <returns></returns>
        private int GetExistsInteraction(MediaTypeEnum mediaType, int mediaID) { 
            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (mediaType)
            {
                case MediaTypeEnum.微信:
                    sql.Append("select top 1 RecID from  Interaction_Weixin ");
                    sql.Append("where MediaID = @MediaID");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@MediaID", SqlDbType.Int),
                    };
                    parameters[0].Value = mediaID;
                    break;
                case MediaTypeEnum.微博:
                    sql.Append("select top 1 RecID from  Interaction_Weibo ");
                    sql.Append("where MediaID = @MediaID");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@MediaID", SqlDbType.Int),
                    };
                    parameters[0].Value = mediaID;
                    break;
                case MediaTypeEnum.视频:
                    sql.Append("select top 1 RecID from  Interaction_Video ");
                    sql.Append("where MediaID = @MediaID");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@MediaID", SqlDbType.Int),
                    };
                    parameters[0].Value = mediaID;
                    break;
                case MediaTypeEnum.直播:
                    sql.Append("select top 1 RecID from  Interaction_Broadcast ");
                    sql.Append("where MediaID = @MediaID");
                    parameters = new SqlParameter[]{
			            new SqlParameter("@MediaID", SqlDbType.Int),
                    };
                    parameters[0].Value = mediaID;
                    break;
            }
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 插入审核记录
        /// </summary>
        /// <returns></returns>
        private int AddAuditInfo(MediaTypeEnum mediaType,int pubID,int userID)
        {
            string sql = @"insert into PublishAuditInfo(MediaType,PublishID,OptType,PubStatus,RejectMsg,CreateUserID,CreateTime) 
                                    values(@MediaType,@PublishID,@OptType,@PubStatus,@RejectMsg,@CreateUserID,@CreateTime)";
            SqlParameter[] parameters = new SqlParameter[] { 
            
                new SqlParameter("@MediaType",(int)mediaType),
                new SqlParameter("@PublishID",pubID),
                new SqlParameter("@OptType",27001),
                new SqlParameter("@PubStatus",15003),
                new SqlParameter("@RejectMsg",null),
                new SqlParameter("@CreateUserID",userID),
                new SqlParameter("@CreateTime",DateTime.Now)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        #region 刊例
        public void DeleteAppDetail(List<string> appNameList,int userID) {
            if (appNameList == null || appNameList.Count.Equals(0))
                return;
            string names = "(";
            foreach (string appName in appNameList) {
                if (!appNameList.IndexOf(appName).Equals(0))
                    names += ",";
                names += "'"+appName+"'";
            }
            names += ")";
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from Publish_ExtendInfoPCAPP where MediaID in (select MediaID from Media_PCAPP where CreateUserID = " + userID+" and Name in "+names+");");
            sql.Append("delete from Publish_DetailInfo where MediaType = 14002 and MediaID in (select MediaID from Media_PCAPP where CreateUserID = " + userID + " and Name in " + names + ")");
            int res = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString());
            return;
        }


        /// <summary>
        /// 增加刊例基本信息 返回增加的刊例ID
        /// </summary>
        /// <param name="mediaType">媒体类型枚举</param>
        /// <param name="mediaID">媒体ID</param>
        /// <param name="model">刊例对象</param>
        /// <param name="trans">事务对象</param>
        /// <param name="deleteOld">是否删除已经存在的</param>
        /// <returns>返回增加的刊例ID</returns>
        private int AddPubBasic(MediaTypeEnum mediaType, int mediaID, PublishBasicInfo model, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into Publish_BasicInfo(");
            sql.Append("MediaType,MediaID,PubCode,BeginTime,EndTime,PurchaseDiscount,SaleDiscount,Status,PublishStatus,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,Wx_Status,IsDel");
            sql.Append(") values (");
            sql.Append("@MediaType,@MediaID,@PubCode,@BeginTime,@EndTime,@PurchaseDiscount,@SaleDiscount,@Status,@PublishStatus,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID,@Wx_Status,@IsDel");
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
					    new SqlParameter("@PublishStatus", SqlDbType.Int),
					    new SqlParameter("@CreateTime", SqlDbType.DateTime),
					    new SqlParameter("@CreateUserID", SqlDbType.Int),
                        new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					    new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
                        new SqlParameter("@Wx_Status", SqlDbType.Int),
                        new SqlParameter("@IsDel", SqlDbType.Int),
            };
            parameters[0].Value = (int)mediaType;
            parameters[1].Value = mediaID;
            parameters[2].Value = "AP" + (int)mediaType + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(10000, 99999);
            parameters[3].Value = model.BeginTime;
            parameters[4].Value = model.EndTime;
            parameters[5].Value = model.PurchaseDiscount;
            parameters[6].Value = model.SaleDiscount;
            parameters[7].Value = (int)model.Status;
            parameters[8].Value = (int)model.PublishStatus;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.LastUpdateTime;
            parameters[12].Value = model.LastUpdateUserID;
            parameters[13].Value = model.Wx_Status;
            parameters[14].Value = model.IsDel;
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
        private List<int> AddPubDetail(MediaTypeEnum mediaType, int mediaID, int pubID, List<PublishDetailInfo> list, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into Publish_DetailInfo(");
            sql.Append("PubID,MediaType,MediaID,ADPosition1,ADPosition2,ADPosition3,Price,IsCarousel,BeginPlayDays,PublishStatus,CreateTime,CreateUserID");
            sql.Append(") values (");
            sql.Append("@PubID,@MediaType,@MediaID,@ADPosition1,@ADPosition2,@ADPosition3,@Price,@IsCarousel,@BeginPlayDays,@PublishStatus,@CreateTime,@CreateUserID");
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
					new SqlParameter("@PublishStatus", SqlDbType.Int),
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
                parameters[9].Value = item.PublishStatus;
                parameters[10].Value = item.CreateTime;
                parameters[11].Value = item.CreateUserID;
                var obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
                res.Add(obj == null ? 0 : Convert.ToInt32(obj));
            }
            return res;
        }

        /// <summary>
        /// 增加刊例扩展信息 返回增加的刊例扩展信息ID
        /// </summary>
        /// <param name="extend">扩展信息对象</param>
        /// <param name="trans"></param>
        /// <param name="deleteOld"></param>
        /// <returns></returns>
        private int AddPubExtend(int mediaID, int pubID, int adDetailID, ADPositionDTO dto, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into Publish_ExtendInfoPCAPP(");
            sql.Append("ADDetailID,PubID,MediaType,MediaID,AdLegendURL,AdPosition,AdForm,DisplayLength,CanClick,CarouselCount,PlayPosition,DailyExposureCount,CPM,CarouselPlay,DailyClickCount,CPM2,CarouselPlay2,ThrMonitor,SysPlatform,Style,IsDispatching,ADShow,ADRemark,AcceptBusinessIDs,NotAcceptBusinessIDs,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,AcceptBusinessNames,NotAcceptBusinessNames");
            sql.Append(") values (");
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
            parameters[3].Value = SqlFilter(dto.AdLegendURL);
            parameters[4].Value = SqlFilter(dto.AdPosition);
            parameters[5].Value = SqlFilter(dto.AdForm);
            parameters[6].Value = dto.DisplayLength;
            parameters[7].Value = dto.CanClick;
            parameters[8].Value = dto.CarouselCount;
            parameters[9].Value = SqlFilter(dto.PlayPosition);
            parameters[10].Value = dto.DailyExposureCount;
            parameters[11].Value = dto.CPM;
            parameters[12].Value = dto.CarouselPlay;
            parameters[13].Value = dto.DailyClickCount;
            parameters[14].Value = dto.CPM2;
            parameters[15].Value = dto.CarouselPlay2;
            parameters[16].Value = dto.ThrMonitor == null ? null : string.Join(",", dto.ThrMonitor);
            parameters[17].Value = dto.SysPlatform == null ? null : string.Join(",", dto.SysPlatform);
            parameters[18].Value = SqlFilter(dto.Style);
            parameters[19].Value = dto.IsDispatching;
            parameters[20].Value = SqlFilter(dto.ADShow);
            parameters[21].Value = SqlFilter(dto.ADRemark);
            parameters[22].Value = dto.AcceptBusinessIDs == null ? null : string.Join(",", dto.AcceptBusinessIDs);
            parameters[23].Value = dto.NotAcceptBusinessIDs == null ? null : string.Join(",", dto.NotAcceptBusinessIDs);
            parameters[24].Value = dto.CreateTime;
            parameters[25].Value = dto.CreateUserID;
            parameters[26].Value = dto.LastUpdateTime;
            parameters[27].Value = dto.LastUpdateUserID;
            parameters[28].Value = null;
            parameters[29].Value = null;
            return SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString(), parameters);
        }

        private int UpdatePubBasic(MediaTypeEnum mediaType, int pubID, PublishBasicInfo model, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            //sql.Append("delete from Publish_ExtendInfoPCAPP where PubID = @PubID;");
            //sql.Append("delete from Publish_DetailInfo where PubID = @PubID;");
            sql.Append("update Publish_BasicInfo set ");
            sql.Append("BeginTime = @BeginTime,");
            sql.Append("EndTime = @EndTime,");
            sql.Append("Status = @Status,");
            sql.Append("PublishStatus = @PublishStatus,");
            sql.Append("PurchaseDiscount = @PurchaseDiscount,");
            sql.Append("SaleDiscount = @SaleDiscount,");
            sql.Append("LastUpdateTime = @LastUpdateTime,");
            sql.Append("LastUpdateUserID = @LastUpdateUserID ");
            sql.Append("where PubID = @PubID");
            SqlParameter[] parameters = new SqlParameter[]{
					    new SqlParameter("@PubID", SqlDbType.Int),
					    new SqlParameter("@BeginTime", SqlDbType.DateTime),
					    new SqlParameter("@EndTime", SqlDbType.DateTime),
					    new SqlParameter("@Status", SqlDbType.Int),
					    new SqlParameter("@PublishStatus", SqlDbType.Int),
					    new SqlParameter("@PurchaseDiscount", SqlDbType.Decimal),
					    new SqlParameter("@SaleDiscount", SqlDbType.Decimal),
                        new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					    new SqlParameter("@LastUpdateUserID", SqlDbType.Int)
            };
            parameters[0].Value = pubID;
            parameters[1].Value = model.BeginTime;
            parameters[2].Value = model.EndTime;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.PublishStatus;
            parameters[5].Value = model.PurchaseDiscount;
            parameters[6].Value = model.SaleDiscount;
            parameters[7].Value = model.LastUpdateTime;
            parameters[8].Value = model.LastUpdateUserID;
            return SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString(), parameters);
        }

        private int UpdatePubDetail() { 
            return 0;
        }

        #endregion

        #region 媒体、互动参数、覆盖区域

        /// <summary>
        /// 添加媒体基本信息
        /// </summary>
        /// <param name="mediaType">媒体类型枚举</param>
        /// <param name="media">媒体对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        private int AddMediaInfo(MediaTypeEnum mediaType, dynamic media, SqlTransaction trans)
        {
            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (mediaType) { 
                #region 分类拼Sql、参数
                case MediaTypeEnum.微信:
                    #region 
                    //基表
                    sql.Append("INSERT INTO dbo.Weixin_OAuth(");
                    sql.Append("WxNumber, NickName, ServiceType, IsVerify, VerifyType, HeadImg, QrCodeUrl, FansCount, Status, OAuthStatus, CreateTime, ModifyTime, SourceType, LevelType, ProvinceID, CityID, Sign");
                    sql.Append(")");
                    sql.Append("VALUES (");
                    sql.Append("@Number, @Name,-2,0,-2,@HeadIconURL,@TwoCodeURL,@FansCount,0,39003,GETDATE(),GETDATE(),38003,@LevelType,@ProvinceID,@CityID,@Sign");
                    sql.Append(")");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[] {
                        new SqlParameter("@Number",media.Number),
                        new SqlParameter("@Name",media.Name),
                        new SqlParameter("@HeadIconURL",media.HeadIconURL),
                        new SqlParameter("@TwoCodeURL",media.TwoCodeURL),
                        new SqlParameter("@FansCount",media.FansCount),
                        new SqlParameter("@LevelType",media.LevelType),
                        new SqlParameter("@ProvinceID",media.ProvinceID),
                        new SqlParameter("@CityID",media.CityID),
                        new SqlParameter("@Sign",media.Sign),
                    };
                    var res = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
                    if (res == null)
                        return 0;
                    sql = new StringBuilder();
                    sql.Append("insert into Media_Weixin(");
                    sql.Append("Number,Name,HeadIconURL,TwoCodeURL,FansCount,FansCountURL,FansMalePer,FansFemalePer,CategoryID,ProvinceID,CityID,Sign,AreaID,LevelType,IsAuth,OrderRemark,IsReserve,Status,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,AuditStatus,AuthType,WxID,ADName");
                    sql.Append(") values (");
                    sql.Append("@Number,@Name,@HeadIconURL,@TwoCodeURL,@FansCount,@FansCountURL,@FansMalePer,@FansFemalePer,@CategoryID,@ProvinceID,@CityID,@Sign,@AreaID,@LevelType,@IsAuth,@OrderRemark,@IsReserve,@Status,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID,@AuditStatus,@AuthType,@WxID,@ADName");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[] { 
						new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@TwoCodeURL",media.TwoCodeURL),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@FansMalePer",media.FansMalePer),
            			new SqlParameter("@FansFemalePer",media.FansFemalePer),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@Sign",media.Sign),
            			new SqlParameter("@AreaID",media.AreaID),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@IsAuth",media.IsAuth),
            			new SqlParameter("@OrderRemark",media.OrderRemark),
            			new SqlParameter("@IsReserve",media.IsReserve),
            			new SqlParameter("@Status",media.Status),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@CreateTime",media.CreateTime),
            			new SqlParameter("@CreateUserID",media.CreateUserID),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        new SqlParameter("@AuditStatus",43002),
                        new SqlParameter("@AuthType",38003),
                        new SqlParameter("@WxID",Convert.ToInt32(res)),
                        new SqlParameter("@ADName",media.Name),
                    };
                #endregion
                    break;
                case MediaTypeEnum.APP:
                    #region
                    sql.Append("insert into Media_PCAPP(");
                    sql.Append("Name,HeadIconURL,CategoryID,ProvinceID,CityID,Terminal,DailyLive,DailyIP,WebSite,Remark,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@Name,@HeadIconURL,@CategoryID,@ProvinceID,@CityID,@Terminal,@DailyLive,@DailyIP,@WebSite,@Remark,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[]{
						new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@Terminal",media.Terminal),
            			new SqlParameter("@DailyLive",media.DailyLive),
            			new SqlParameter("@DailyIP",media.DailyIP),
            			new SqlParameter("@WebSite",media.WebSite),
            			new SqlParameter("@Remark",media.Remark),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@CreateTime",media.CreateTime),
            			new SqlParameter("@CreateUserID",media.CreateUserID),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.微博:
                    #region
                    sql.Append("insert into dbo.Media_Weibo(");
                    sql.Append("Number,Name,Sex,HeadIconURL,FansCount,FansCountURL,FansSex,CategoryID,AreaID,Profession,ProvinceID,CityID,LevelType,AuthType,Sign,OrderRemark,IsReserve,Status,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@Number,@Name,@Sex,@HeadIconURL,@FansCount,@FansCountURL,@FansSex,@CategoryID,@AreaID,@Profession,@ProvinceID,@CityID,@LevelType,@AuthType,@Sign,@OrderRemark,@IsReserve,@Status,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[]{
						new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@Sex",media.Sex),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@FansSex",media.FansSex),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@AreaID",media.AreaID),
            			new SqlParameter("@Profession",media.Profession),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@AuthType",media.AuthType),
            			new SqlParameter("@Sign",media.Sign),
            			new SqlParameter("@OrderRemark",media.OrderRemark),
            			new SqlParameter("@IsReserve",media.IsReserve),
            			new SqlParameter("@Status",media.Status),
            			new SqlParameter("@Source",media.Source),
            			new SqlParameter("@CreateTime",media.CreateTime),
            			new SqlParameter("@CreateUserID",media.CreateUserID),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.视频:
                    #region
                    sql.Append("insert into dbo.Media_Video(");
                    sql.Append("Platform,Number,Name,HeadIconURL,Sex,FansCount,FansCountURL,CategoryID,Profession,AuthType,LevelType,ProvinceID ,CityID,IsReserve,Status,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@Platform,@Number,@Name,@HeadIconURL,@Sex,@FansCount,@FansCountURL,@CategoryID,@Profession,@AuthType,@LevelType,@ProvinceID ,@CityID,@IsReserve,@Status,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[]{
						new SqlParameter("@Platform",media.Platform),
            			new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@Sex",media.Sex),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@Profession",media.Profession),
            			new SqlParameter("@AuthType",media.AuthType),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@IsReserve",media.IsReserve),
            			new SqlParameter("@Status",media.Status),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@CreateTime",media.CreateTime),
            			new SqlParameter("@CreateUserID",media.CreateUserID),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.直播:
                    #region
                    sql.Append("insert into Media_Broadcast(");
                    sql.Append("Platform,RoomID,Number,Name,HeadIconURL,FansCountURL,Sex,FansCount,CategoryID,Profession,ProvinceID,CityID,IsAuth,LevelType,IsReserve,Status,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
                    sql.Append(") values (");
                    sql.Append("@Platform,@RoomID,@Number,@Name,@HeadIconURL,@FansCountURL,@Sex,@FansCount,@CategoryID,@Profession,@ProvinceID,@CityID,@IsAuth,@LevelType,@IsReserve,@Status,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
                    sql.Append(") ");
                    sql.Append(";select SCOPE_IDENTITY()");
                    parameters = new SqlParameter[]{
						new SqlParameter("@Platform",media.Platform),
            			new SqlParameter("@RoomID",media.RoomID),
            			new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
                        new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@Sex",media.Sex),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@Profession",media.Profession),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@IsAuth",media.IsAuth),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@IsReserve",media.IsReserve),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@Status",media.Status),
            			new SqlParameter("@CreateTime",media.CreateTime),
            			new SqlParameter("@CreateUserID",media.CreateUserID),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                default:
                    return 0;
                #endregion
            }
            var obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 增加媒体互动信息 返回增加的互动信息ID
        /// </summary>
        /// <param name="mediaType">媒体类型枚举</param>
        /// <param name="mediaID">媒体ID</param>
        /// <param name="interaction">互动信息对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        private int AddInteraction(MediaTypeEnum mediaType, int mediaID, dynamic interaction, SqlTransaction trans)
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
        /// 增加区域覆盖
        /// </summary>
        /// <param name="list">覆盖区域数组</param>
        /// <param name="trans">事务对象</param>
        /// <param name="deleteOld">是否删除旧的</param>
        /// <returns></returns>
        private List<int> AddAreaMapping(MediaTypeEnum mediaType, int mediaID, List<MediaAreaMapping> list, SqlTransaction trans, bool deleteOld = true)
        {
            var sql = new StringBuilder();
            if (deleteOld)
            {
                sql.Append("delete from Media_Area_Mapping ");
                sql.Append("where MediaType = @MediaType and MediaID = @MediaID;");
            }
            sql.Append("insert into Media_Area_Mapping(");
            sql.Append("MediaType,MediaID,ProvinceID,CityID,CreateTime,CreateUserID");
            sql.Append(") values (");
            sql.Append("@MediaType,@MediaID,@ProvinceID,@CityID,@CreateTime,@CreateUserID");
            sql.Append(") ");
            sql.Append(";select SCOPE_IDENTITY()");
            List<int> res = new List<int>();
            SqlParameter[] parameters = null;
            foreach (var item in list) { 
                parameters = new SqlParameter[]{
					new SqlParameter("@MediaType",mediaType),
            		new SqlParameter("@MediaID",mediaID),
            		new SqlParameter("@ProvinceID",item.ProvinceID),
            		new SqlParameter("@CityID",item.CityID),
            		new SqlParameter("@CreateTime",item.CreateTime),
            		new SqlParameter("@CreateUserID",item.CreateUserID),
                };
                if (parameters[3].Value.Equals(0))
                    parameters[3].Value = null;
                var obj = SqlHelper.ExecuteScalar(trans, CommandType.Text, sql.ToString(), parameters);
                res.Add(obj == null ? 0 : Convert.ToInt32(obj));
            }
            return res;
        }

        private int UpdateMediaInfo(MediaTypeEnum mediaType, dynamic media, SqlTransaction trans) {

            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (mediaType)
            {
                #region 分类拼Sql、参数
                case MediaTypeEnum.微信:
                    #region
                    sql.Append(@"UPDATE [dbo].[Media_Weixin] ");
                    sql.Append(@"SET [Number] = @Number,[Name] = @Name
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
                      ,[Source] = @Source
                      ,[Status] = @Status
                      ,[LastUpdateTime] = @LastUpdateTime
                      ,[LastUpdateUserID] = @LastUpdateUserID  WHERE MediaID = @MediaID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",media.MediaID),
						new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@TwoCodeURL",media.TwoCodeURL),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@FansMalePer",media.FansMalePer),
            			new SqlParameter("@FansFemalePer",media.FansFemalePer),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@Sign",media.Sign),
            			new SqlParameter("@AreaID",media.AreaID),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@IsAuth",media.IsAuth),
            			new SqlParameter("@OrderRemark",media.OrderRemark),
            			new SqlParameter("@IsReserve",media.IsReserve),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@Status",media.Status),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.APP:
                    #region
                    sql.Append(@"UPDATE [dbo].[Media_PCAPP] ");
                    sql.Append(@"SET [Name] = @Name
                                  ,[HeadIconURL] = @HeadIconURL
                                  ,[CategoryID] = @CategoryID
                                  ,[ProvinceID] = @ProvinceID
                                  ,[CityID] = @CityID
                                  ,[Terminal] = @Terminal
                                  ,[DailyLive] = @DailyLive
                                  ,[DailyIP] = @DailyIP
                                  ,[WebSite] = @WebSite
                                  ,[Remark] = @Remark
                                  ,[Source] = @Source
                              ,[LastUpdateTime] = @LastUpdateTime
                              ,[LastUpdateUserID] = @LastUpdateUserID
                            WHERE MediaID = @MediaID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",media.MediaID),
						new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@Terminal",media.Terminal),
            			new SqlParameter("@DailyLive",media.DailyLive),
            			new SqlParameter("@DailyIP",media.DailyIP),
            			new SqlParameter("@WebSite",media.WebSite),
            			new SqlParameter("@Remark",media.Remark),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.微博:
                    #region
                    sql.Append(@"UPDATE [dbo].[Media_Weibo] ");
                    sql.Append(@"SET [Number] = @Number
                        ,[Name] = @Name
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
                        ,[LastUpdateTime] = @LastUpdateTime
                        ,[LastUpdateUserID] = @LastUpdateUserID  WHERE MediaID = @MediaID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",media.MediaID),
						new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@Sex",media.Sex),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@FansSex",media.FansSex),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@AreaID",media.AreaID),
            			new SqlParameter("@Profession",media.Profession),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@AuthType",media.AuthType),
            			new SqlParameter("@Sign",media.Sign),
            			new SqlParameter("@OrderRemark",media.OrderRemark),
            			new SqlParameter("@IsReserve",media.IsReserve),
            			new SqlParameter("@Status",media.Status),
            			new SqlParameter("@Source",media.Source),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.视频:
                    #region
                    sql.Append(@"UPDATE [dbo].[Media_Video] ");
                    sql.Append(@"SET [Platform] = @Platform
                        ,[Number] = @Number
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
                        ,[Source] = @Source
                        ,[Status] = @Status
                        ,[LastUpdateTime] = @LastUpdateTime
                        ,[LastUpdateUserID] = @LastUpdateUserID       
                        WHERE MediaID = @MediaID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",media.MediaID),
						new SqlParameter("@Platform",media.Platform),
            			new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
            			new SqlParameter("@Sex",media.Sex),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@Profession",media.Profession),
            			new SqlParameter("@AuthType",media.AuthType),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@IsReserve",media.IsReserve),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@Status",media.Status),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.直播:
                    #region
                    sql.Append(@"UPDATE [dbo].[Media_Broadcast] ");
                    sql.Append(@"SET [Platform] = @Platform
                        ,[RoomID] = @RoomID
                        ,[Number] = @Number
                        ,[Name] = @Name
                        ,[HeadIconURL] = @HeadIconURL
                        ,[FansCountURL] = @FansCountURL
                        ,[Sex] = @Sex
                        ,[FansCount] = @FansCount
                        ,[CategoryID] =@CategoryID
                        ,[Profession] = @Profession
                        ,[ProvinceID] = @ProvinceID
                        ,[CityID] = @CityID
                        ,[IsAuth] = @IsAuth
                        ,[LevelType] = @LevelType
                        ,[IsReserve] = @IsReserve
                        ,[Source] = @Source
                        ,[Status] = @Status
                        ,[LastUpdateTime] = @LastUpdateTime
                        ,[LastUpdateUserID] = @LastUpdateUserID  
                        WHERE MediaID = @MediaID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@MediaID",media.MediaID),
						new SqlParameter("@Platform",media.Platform),
            			new SqlParameter("@RoomID",media.RoomID),
            			new SqlParameter("@Number",media.Number),
            			new SqlParameter("@Name",media.Name),
            			new SqlParameter("@HeadIconURL",media.HeadIconURL),
                        new SqlParameter("@FansCountURL",media.FansCountURL),
            			new SqlParameter("@Sex",media.Sex),
            			new SqlParameter("@FansCount",media.FansCount),
            			new SqlParameter("@CategoryID",media.CategoryID),
            			new SqlParameter("@Profession",media.Profession),
            			new SqlParameter("@ProvinceID",media.ProvinceID),
            			new SqlParameter("@CityID",media.CityID),
            			new SqlParameter("@IsAuth",media.IsAuth),
            			new SqlParameter("@LevelType",media.LevelType),
            			new SqlParameter("@IsReserve",media.IsReserve),
                        new SqlParameter("@Source",media.Source),
            			new SqlParameter("@Status",media.Status),
            			new SqlParameter("@LastUpdateTime",media.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",media.LastUpdateUserID),
                        };
                    #endregion
                    break;
                default:
                    return 0;
                #endregion
            }
            return SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString(), parameters);
        }

        private int UpdateInteraction(MediaTypeEnum mediaType, int recID, dynamic interaction, SqlTransaction trans) {

            StringBuilder sql = new StringBuilder();
            SqlParameter[] parameters = null;
            switch (mediaType)
            {
                #region 分类拼Sql、参数
                case MediaTypeEnum.微信:
                    #region
                    sql.Append(@"UPDATE [dbo].[Interaction_Weixin] ");
                    sql.Append(@"SET [ReferReadCount] = @ReferReadCount
                        ,[AveragePointCount] =@AveragePointCount
                        ,[MoreReadCount] = @MoreReadCount
                        ,[OrigArticleCount] = @OrigArticleCount
                        ,[UpdateCount] = @UpdateCount
                        ,[MaxinumReading] = @MaxinumReading
                        ,[ScreenShotURL] = @ScreenShotURL
                        ,[LastUpdateTime] = @LastUpdateTime
                       ,[LastUpdateUserID] = @LastUpdateUserID  
                       WHERE RecID = @RecID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",recID),
            			new SqlParameter("@ReferReadCount",interaction.ReferReadCount),
            			new SqlParameter("@AveragePointCount",interaction.AveragePointCount),
            			new SqlParameter("@MoreReadCount",interaction.MoreReadCount),
            			new SqlParameter("@OrigArticleCount",interaction.OrigArticleCount),
            			new SqlParameter("@UpdateCount",interaction.UpdateCount),
            			new SqlParameter("@MaxinumReading",interaction.MaxinumReading),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.微博:
                    #region
                    sql.Append(@"UPDATE [dbo].[Interaction_Weibo] ");
                    sql.Append(@"SET [AverageForwardCount] = @AverageForwardCount
                        ,[AverageCommentCount] = @AverageCommentCount
                        ,[AveragePointCount] = @AveragePointCount
                        ,[ScreenShotURL] =@ScreenShotURL
                        ,[LastUpdateTime] = @LastUpdateTime
                        ,[LastUpdateUserID] = @LastUpdateUserID  
                        WHERE RecID = @RecID");
                     parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",recID),
            			new SqlParameter("@AverageForwardCount",interaction.AverageForwardCount),
            			new SqlParameter("@AverageCommentCount",interaction.AverageCommentCount),
            			new SqlParameter("@AveragePointCount",interaction.AveragePointCount),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                        };
                    #endregion
                    break;
                case MediaTypeEnum.视频:
                    #region
                    sql.Append(@"UPDATE [dbo].[Interaction_Video] ");
                    sql.Append(@"SET [AveragePlayCount] = @AveragePlayCount
                    ,[AveragePointCount] = @AveragePointCount
                    ,[AverageCommentCount] =@AverageCommentCount
                    ,[AverageBarrageCount] = @AverageBarrageCount
                    ,[ScreenShotURL] = @ScreenShotURL
                    ,[LastUpdateTime] = @LastUpdateTime
                    ,[LastUpdateUserID] = @LastUpdateUserID  
                    WHERE RecID = @RecID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",recID),
            			new SqlParameter("@AveragePlayCount",interaction.AveragePlayCount),
            			new SqlParameter("@AveragePointCount",interaction.AveragePointCount),
            			new SqlParameter("@AverageCommentCount",interaction.AverageCommentCount),
            			new SqlParameter("@AverageBarrageCount",interaction.AverageBarrageCount),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID)
                        };
                    #endregion
                    break;
                case MediaTypeEnum.直播:
                    #region
                    sql.Append(@"UPDATE [dbo].[Interaction_Broadcast] ");
                    sql.Append(@"SET [AudienceCount] = @AudienceCount
                    ,[MaximumAudience] =@MaximumAudience
                    ,[AverageAudience] = @AverageAudience
                    ,[CumulateReward] = @CumulateReward
                    ,[CumulateIncome] = @CumulateIncome
                    ,[CumulatePoints] = @CumulatePoints
                    ,[CumulateSendCount] =@CumulateSendCount
                    ,[ScreenShotURL] = @ScreenShotURL
                    ,[LastUpdateTime] = @LastUpdateTime
                    ,[LastUpdateUserID] = @LastUpdateUserID  
                    WHERE RecID = @RecID");
                    parameters = new SqlParameter[]{
                        new SqlParameter("@RecID",recID),
            			new SqlParameter("@AudienceCount",interaction.AudienceCount),
            			new SqlParameter("@MaximumAudience",interaction.MaximumAudience),
            			new SqlParameter("@AverageAudience",interaction.AverageAudience),
            			new SqlParameter("@CumulateReward",interaction.CumulateReward),
            			new SqlParameter("@CumulateIncome",interaction.CumulateIncome),
            			new SqlParameter("@CumulatePoints",interaction.CumulatePoints),
            			new SqlParameter("@CumulateSendCount",interaction.CumulateSendCount),
            			new SqlParameter("@ScreenShotURL",interaction.ScreenShotURL),
            			new SqlParameter("@LastUpdateTime",interaction.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",interaction.LastUpdateUserID),
                        };
                    #endregion
                    break;
                default:
                    return 0;
                #endregion
            }
            return SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql.ToString(), parameters);
        }

        #endregion

    }
}
