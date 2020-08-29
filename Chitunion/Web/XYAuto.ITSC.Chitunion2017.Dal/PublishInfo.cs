using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.Utils.Data;


namespace XYAuto.ITSC.Chitunion2017.Dal
{
    /// <summary>
    /// ls
    /// </summary>
    public class PublishInfo : DataBase
    {
        public static readonly PublishInfo Instance = new PublishInfo();

        /// <summary>
        /// 添加刊例基础信息
        /// ls
        /// </summary>
        /// <param name="model">值对象</param>
        /// <returns>成功返回新增记录ID</returns>
        public int AddPublishBasicInfo(Entities.Publish.PublishBasicInfo model, List<Entities.Publish.PublishDetailInfo> detailList, string rightSql)
        {
            #region parameters
            SqlParameter[] parameters = {
		        new SqlParameter("@PubID", SqlDbType.Int),
		        new SqlParameter("@MediaType", SqlDbType.Int),
		        new SqlParameter("@MediaID", SqlDbType.Int),
		        new SqlParameter("@PubCode", SqlDbType.VarChar, 20),
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
                new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = (int)model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = SqlFilter(model.PubCode);
            parameters[4].Value = model.BeginTime;
            parameters[5].Value = model.EndTime;
            parameters[6].Value = model.PurchaseDiscount;
            parameters[7].Value = model.SaleDiscount;
            parameters[8].Value = (int)model.Status;
            parameters[9].Value = (int)model.PublishStatus;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.CreateUserID;
            parameters[12].Value = model.LastUpdateTime;
            parameters[13].Value = model.LastUpdateUserID;
            parameters[14].Value = rightSql;
            #endregion
        
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_BasicInfo_Add", parameters);
                        int pubID = Convert.ToInt32(parameters[0].Value);
                        if (pubID > 0)
                        {
                            if (detailList != null && detailList.Count > 0)
                            {
                                foreach (var detail in detailList)
                                {
                                    #region parameters
                                    parameters = new SqlParameter[]
                                    {
					                    new SqlParameter("@RecID", SqlDbType.Int),
					                    new SqlParameter("@PubID", SqlDbType.Int),
					                    new SqlParameter("@MediaType", SqlDbType.Int),
					                    new SqlParameter("@ADPosition1", SqlDbType.Int),
					                    new SqlParameter("@ADPosition2", SqlDbType.Int),
                                        new SqlParameter("@ADPosition3", SqlDbType.Int),
					                    new SqlParameter("@Price", SqlDbType.Decimal),
					                    new SqlParameter("@IsCarousel", SqlDbType.Bit),
					                    new SqlParameter("@BeginPlayDays", SqlDbType.Int),
                                        new SqlParameter("@PublishStatus", SqlDbType.Int),
					                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
					                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                                        new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
                                    };
                                    parameters[0].Direction = ParameterDirection.Output;
                                    parameters[1].Value = pubID;
                                    parameters[2].Value = (int)model.MediaType;
                                    parameters[3].Value = detail.ADPosition1;
                                    parameters[4].Value = detail.ADPosition2;
                                    parameters[5].Value = detail.ADPosition3;
                                    parameters[6].Value = detail.Price;
                                    parameters[7].Value = detail.IsCarousel;
                                    parameters[8].Value = detail.BeginPlayDays;
                                    parameters[9].Value = detail.PublishStatus;
                                    parameters[10].Value = model.CreateTime;
                                    parameters[11].Value = model.CreateUserID;
                                    parameters[12].Value = rightSql;
                                    #endregion
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Add", parameters);
                                }
                            }
                            trans.Commit();
                        }
                        return pubID;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 更新刊例基础信息
        /// ls
        /// </summary>
        /// <returns>成功>0</returns>
        public int UpdataPublishBasicInfo(Entities.Publish.PublishBasicInfo model, List<Entities.Publish.PublishDetailInfo> detailList, string rightSql)
        {
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
					new SqlParameter("@PubID", SqlDbType.Int),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@PurchaseDiscount", SqlDbType.Decimal),
					new SqlParameter("@SaleDiscount", SqlDbType.Decimal),
                    new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
                    new SqlParameter("@RightSql", SqlDbType.VarChar)
            };
            parameters[0].Value = model.PubID;
            parameters[1].Value = model.BeginTime;
            parameters[2].Value = model.EndTime;
            parameters[3].Value = model.PurchaseDiscount;
            parameters[4].Value = model.SaleDiscount;
            parameters[5].Value = model.LastUpdateTime;
            parameters[6].Value = model.LastUpdateUserID;
            parameters[7].Value = rightSql;
            #endregion

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //修改存储过程中已经删除了Detail数据
                        int count = SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_BasicInfo_Update", parameters);
                        if (count > 0)
                        {
                            foreach (var detail in detailList)
                            {
                                #region parameters
                                parameters = new SqlParameter[]
                                {
					                new SqlParameter("@RecID", SqlDbType.Int),
					                new SqlParameter("@PubID", SqlDbType.Int),
                                    new SqlParameter("@MediaType", SqlDbType.Int),
					                new SqlParameter("@ADPosition1", SqlDbType.Int),
					                new SqlParameter("@ADPosition2", SqlDbType.Int),
					                new SqlParameter("@ADPosition3", SqlDbType.Int),
					                new SqlParameter("@Price", SqlDbType.Decimal),
					                new SqlParameter("@IsCarousel", SqlDbType.Bit),
					                new SqlParameter("@BeginPlayDays", SqlDbType.Int),
					                new SqlParameter("@CreateTime", SqlDbType.DateTime),
					                new SqlParameter("@CreateUserID", SqlDbType.Int),
                                    new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
                                };
                                parameters[0].Direction = ParameterDirection.Output;
                                parameters[1].Value = model.PubID;
                                parameters[2].Value = model.MediaType;
                                parameters[3].Value = detail.ADPosition1;
                                parameters[4].Value = detail.ADPosition2;
                                parameters[5].Value = detail.ADPosition3;
                                parameters[6].Value = detail.Price;
                                parameters[7].Value = detail.IsCarousel;
                                parameters[8].Value = detail.BeginPlayDays;
                                parameters[9].Value = model.LastUpdateTime;
                                parameters[10].Value = model.LastUpdateUserID;
                                parameters[11].Value = rightSql;
                                #endregion
                                SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Add", parameters);
                            }
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }

        }

        /// <summary>
        /// 添加刊例扩展信息（APP广告位）
        /// ls
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int AddPublishExtendInfo(ADPositionDTO dto, string rightSql) {
            //先添加详细信息  再添加扩展信息
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
			    new SqlParameter("@RecID", SqlDbType.Int),
			    new SqlParameter("@PubID", SqlDbType.Int),
                new SqlParameter("@MediaType", SqlDbType.Int),
			    new SqlParameter("@ADPosition1", SqlDbType.Int),
			    new SqlParameter("@ADPosition2", SqlDbType.Int),
			    new SqlParameter("@ADPosition3", SqlDbType.Int),
			    new SqlParameter("@Price", SqlDbType.Decimal),
			    new SqlParameter("@IsCarousel", SqlDbType.Bit),
			    new SqlParameter("@BeginPlayDays", SqlDbType.Int),
                new SqlParameter("@PublishStatus", SqlDbType.Int),
			    new SqlParameter("@CreateTime", SqlDbType.DateTime),
			    new SqlParameter("@CreateUserID", SqlDbType.Int),
                new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = dto.PubID;
            parameters[2].Value = 14002;
            parameters[3].Value = dto.SaleType;
            parameters[4].Value = -1;
            parameters[5].Value = -1;
            parameters[6].Value = dto.Price;
            parameters[7].Value = dto.IsCarousel;
            parameters[8].Value = dto.BeginPlayDays;
            parameters[9].Value = dto.PublishStatus;
            parameters[10].Value = dto.CreateTime;
            parameters[11].Value = dto.CreateUserID;
            parameters[12].Value = rightSql;
            #endregion
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Add", parameters);
                        int adDetailID = Convert.ToInt32(parameters[0].Value);
                        if (adDetailID > 0)
                        {
                            #region parameters
                            parameters = new SqlParameter[] 
                            {    
					            new SqlParameter("@ADDetailID", SqlDbType.Int),
					            new SqlParameter("@AdLegendURL", SqlDbType.NVarChar, 200),
					            new SqlParameter("@AdPosition", SqlDbType.NVarChar, 100),
					            new SqlParameter("@AdForm", SqlDbType.NVarChar, 100),
					            new SqlParameter("@DisplayLength", SqlDbType.Int),
                                new SqlParameter("@CanClick", SqlDbType.Bit),
					            new SqlParameter("@CarouselCount", SqlDbType.Int),
					            new SqlParameter("@PlayPosition", SqlDbType.NVarChar, 100),
					            new SqlParameter("@DailyExposureCount", SqlDbType.Int),
					            new SqlParameter("@CPM", SqlDbType.Bit),
                                new SqlParameter("@CarouselPlay", SqlDbType.Bit),
					            new SqlParameter("@DailyClickCount", SqlDbType.Int),
					            new SqlParameter("@CPM2", SqlDbType.Bit),
                                new SqlParameter("@CarouselPlay2", SqlDbType.Bit),
					            new SqlParameter("@ThrMonitor", SqlDbType.VarChar, 20),
					            new SqlParameter("@SysPlatform", SqlDbType.VarChar, 20),
					            new SqlParameter("@Style", SqlDbType.NVarChar, 100),
					            new SqlParameter("@IsDispatching", SqlDbType.Bit),
					            new SqlParameter("@ADShow", SqlDbType.VarChar, 500),
					            new SqlParameter("@ADRemark", SqlDbType.VarChar, 500),
					            new SqlParameter("@AcceptBusinessIDs", SqlDbType.VarChar, 200),
					            new SqlParameter("@NotAcceptBusinessIDs", SqlDbType.VarChar, 200),
					            new SqlParameter("@CreateTime", SqlDbType.DateTime),
					            new SqlParameter("@CreateUserID", SqlDbType.Int),
					            new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					            new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
                                new SqlParameter("@RightSql",rightSql)
                            };
                            parameters[0].Value = adDetailID;
                            parameters[1].Value = SqlFilter(dto.AdLegendURL);
                            parameters[2].Value = SqlFilter(dto.AdPosition);
                            parameters[3].Value = SqlFilter(dto.AdForm);
                            parameters[4].Value = dto.DisplayLength;
                            parameters[5].Value = dto.CanClick;
                            parameters[6].Value = dto.CarouselCount;
                            parameters[7].Value = SqlFilter(dto.PlayPosition);
                            parameters[8].Value = dto.DailyExposureCount;
                            parameters[9].Value = dto.CPM;
                            parameters[10].Value = dto.CarouselPlay;
                            parameters[11].Value = dto.DailyClickCount;
                            parameters[12].Value = dto.CPM2;
                            parameters[13].Value = dto.CarouselPlay2;
                            parameters[14].Value = dto.ThrMonitor == null ? null:string.Join(",", dto.ThrMonitor);
                            parameters[15].Value = dto.SysPlatform == null ? null:string.Join(",", dto.SysPlatform);
                            parameters[16].Value = SqlFilter(dto.Style);
                            parameters[17].Value = dto.IsDispatching;
                            parameters[18].Value = SqlFilter(dto.ADShow);
                            parameters[19].Value = SqlFilter(dto.ADRemark);
                            parameters[20].Value = dto.AcceptBusinessIDs == null ? null : string.Join(",", dto.AcceptBusinessIDs);
                            parameters[21].Value = dto.NotAcceptBusinessIDs == null ? null : string.Join(",", dto.NotAcceptBusinessIDs);
                            parameters[22].Value = dto.CreateTime;
                            parameters[23].Value = dto.CreateUserID;
                            parameters[24].Value = dto.LastUpdateTime;
                            parameters[25].Value = dto.LastUpdateUserID;
                            parameters[26].Value = rightSql;
                            #endregion
                            SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_ExtendInfoPCAPP_Add", parameters);
                        }
                        
                        trans.Commit();
                        return adDetailID;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 更新刊例扩展信息（APP广告位）
        /// ls
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdatePublishExtendInfo(ADPositionDTO dto, string rightSql)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                #region parameters
                SqlParameter[] parameters = new SqlParameter[]
                {
				    new SqlParameter("@RecID", SqlDbType.Int),
				    new SqlParameter("@ADPosition1", SqlDbType.Int),
				    new SqlParameter("@ADPosition2", SqlDbType.Int),
				    new SqlParameter("@ADPosition3", SqlDbType.Int),
				    new SqlParameter("@Price", SqlDbType.Decimal),
				    new SqlParameter("@IsCarousel", SqlDbType.Bit),
				    new SqlParameter("@BeginPlayDays", SqlDbType.Int),
                    new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
                    new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
                };
                parameters[0].Value = dto.ADDetailID;
                parameters[1].Value = dto.SaleType;
                parameters[2].Value = -1;
                parameters[3].Value = -1;
                parameters[4].Value = dto.Price;
                parameters[5].Value = dto.IsCarousel;
                parameters[6].Value = dto.BeginPlayDays;
                parameters[7].Value = dto.LastUpdateUserID;
                parameters[8].Value = rightSql;
                #endregion
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int count = SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Update", parameters);
                        if (count > 0)
                        {
                            #region parameters
                            parameters = new SqlParameter[]
                            {
					            new SqlParameter("@ADDetailID", SqlDbType.Int),
					            new SqlParameter("@AdLegendURL", SqlDbType.NVarChar, 200),
					            new SqlParameter("@AdPosition", SqlDbType.NVarChar, 100),
					            new SqlParameter("@AdForm", SqlDbType.NVarChar, 100),
					            new SqlParameter("@DisplayLength", SqlDbType.Int),
					            new SqlParameter("@CanClick", SqlDbType.Bit),
					            new SqlParameter("@CarouselCount", SqlDbType.Int),
					            new SqlParameter("@PlayPosition", SqlDbType.NVarChar, 100),
					            new SqlParameter("@DailyExposureCount", SqlDbType.Int),
					            new SqlParameter("@CPM", SqlDbType.Bit),
                                new SqlParameter("@CarouselPlay", SqlDbType.Bit),
					            new SqlParameter("@DailyClickCount", SqlDbType.Int),
					            new SqlParameter("@CPM2", SqlDbType.Bit),
                                new SqlParameter("@CarouselPlay2", SqlDbType.Bit),
					            new SqlParameter("@ThrMonitor", SqlDbType.VarChar, 20),
					            new SqlParameter("@SysPlatform", SqlDbType.VarChar, 20),
					            new SqlParameter("@Style", SqlDbType.NVarChar, 100),
					            new SqlParameter("@IsDispatching", SqlDbType.Bit),
					            new SqlParameter("@ADShow", SqlDbType.VarChar, 500),
					            new SqlParameter("@ADRemark", SqlDbType.VarChar, 500),
					            new SqlParameter("@AcceptBusinessIDs", SqlDbType.VarChar, 200),
					            new SqlParameter("@NotAcceptBusinessIDs", SqlDbType.VarChar, 200),
					            new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					            new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
                                new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
                            };
                            parameters[0].Value = dto.ADDetailID;
                            parameters[1].Value = SqlFilter(dto.AdLegendURL);
                            parameters[2].Value = SqlFilter(dto.AdPosition);
                            parameters[3].Value = SqlFilter(dto.AdForm);
                            parameters[4].Value = dto.DisplayLength;
                            parameters[5].Value = dto.CanClick;
                            parameters[6].Value = dto.CarouselCount;
                            parameters[7].Value = SqlFilter(dto.PlayPosition);
                            parameters[8].Value = dto.DailyExposureCount;
                            parameters[9].Value = dto.CPM;
                            parameters[10].Value = dto.CarouselPlay;
                            parameters[11].Value = dto.DailyClickCount;
                            parameters[12].Value = dto.CPM2;
                            parameters[13].Value = dto.CarouselPlay2;
                            parameters[14].Value = dto.ThrMonitor == null ? null : string.Join(",", dto.ThrMonitor);
                            parameters[15].Value = dto.SysPlatform == null ? null : string.Join(",", dto.SysPlatform);
                            parameters[16].Value = SqlFilter(dto.Style);
                            parameters[17].Value = dto.IsDispatching;
                            parameters[18].Value = SqlFilter(dto.ADShow);
                            parameters[19].Value = SqlFilter(dto.ADRemark);
                            parameters[20].Value = dto.AcceptBusinessIDs == null ? null : string.Join(",", dto.AcceptBusinessIDs);
                            parameters[21].Value = dto.NotAcceptBusinessIDs == null ? null : string.Join(",", dto.NotAcceptBusinessIDs);
                            parameters[22].Value = dto.LastUpdateTime;
                            parameters[23].Value = dto.LastUpdateUserID;
                            parameters[24].Value = rightSql;
                            #endregion
                            count = SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_ExtendInfoPCAPP_Update", parameters);
                        }
                        trans.Commit();
                        return count;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }
        }

        public int CopyPublishExtendInfo(int oldID, int userID, string rightSql) {
            //先复制详细信息  再复制扩展信息
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
			    new SqlParameter("@ADDetailID", SqlDbType.Int),
			    new SqlParameter("@CreateTime", SqlDbType.DateTime),
			    new SqlParameter("@CreateUserID", SqlDbType.Int),
			    new SqlParameter("@RecID", SqlDbType.Int),
                new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
            };
            parameters[0].Value = oldID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = userID;
            parameters[3].Direction = ParameterDirection.Output;
            parameters[4].Value = rightSql;
            #endregion
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Copy", parameters);
                        int newID = Convert.ToInt32(parameters[3].Value);
                        if (newID > 0)
                        {
                            #region parameters
                            parameters = new SqlParameter[] 
                            {
					            new SqlParameter("@OldID", SqlDbType.Int),
					            new SqlParameter("@NewID", SqlDbType.Int),
					            new SqlParameter("@CreateTime", SqlDbType.DateTime),
					            new SqlParameter("@CreateUserID", SqlDbType.Int),
					            new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
                            };
                            parameters[0].Value = oldID;
                            parameters[1].Value = newID;
                            parameters[2].Value = DateTime.Now;
                            parameters[3].Value = userID;
                            parameters[4].Value = rightSql;
                            #endregion
                            SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_ExtendInfoPCAPP_Copy", parameters);
                        }

                        trans.Commit();
                        return newID;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// 修改刊例上下架状态 (自媒体)
        /// ls
        /// </summary>
        /// <param name="PubID"></param>
        /// <param name="Status"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ModifyPublishStatus(int pubID, int status, int userID, string rightSql)
        {

            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@PubID", SqlDbType.Int),
                new SqlParameter("@Status", SqlDbType.Int),
                new SqlParameter("@UserID", SqlDbType.Int),
                new SqlParameter("@RowCount", SqlDbType.Int),
                new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
             };
            parameters[0].Value = pubID;
            parameters[1].Value = status;
            parameters[2].Value = userID;
            parameters[3].Direction = ParameterDirection.Output;
            parameters[4].Value = rightSql;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_ModifyState", parameters);
            return Convert.ToInt32(parameters[3].Value) > 0;
        }

        /// <summary>
        /// 修改广告位上下架状态
        /// ls
        /// </summary>
        /// <param name="RecID"></param>
        /// <param name="PubID"></param>
        /// <param name="Status"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool ModifyADPositionStatus(string recID, int pubID, int status, int userID, string rightSql) {

            SqlParameter[] parameters = new SqlParameter[] 
            {
				new SqlParameter("@RecID", SqlDbType.VarChar),
				new SqlParameter("@PubID", SqlDbType.Int),
				new SqlParameter("@Status", SqlDbType.Int),
				new SqlParameter("@UserID", SqlDbType.Int),
                new SqlParameter("@RightSql", SqlDbType.VarChar, 200)
             };
            parameters[0].Value = recID;
            parameters[1].Value = pubID;
            parameters[2].Value = status;
            parameters[3].Value = userID;
            parameters[4].Value = rightSql;
            int count = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_ExtendInfoPCAPP_ModifyState", parameters);
            return count > 0;
        }

        /// <summary>
        /// 检查是否存在同名或同账号的媒体
        /// ls
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="name">名称</param>
        /// <param name="number">账号</param>
        /// <returns></returns>
        public MediaExistsDTO MediaIsExists(MediaTypeEnum mediaType, int mediaID, string name, string number, bool needCount, string rightSql)
        {
            MediaExistsDTO dto = new MediaExistsDTO();
            SqlParameter[] parameters = new SqlParameter[] 
            {
				new SqlParameter("@MediaType", SqlDbType.Int),
				new SqlParameter("@Name", SqlDbType.VarChar),
				new SqlParameter("@Number", SqlDbType.VarChar),
                new SqlParameter("@RightSql", SqlDbType.VarChar),
                new SqlParameter("@NeedCount", SqlDbType.Bit),
				new SqlParameter("@PubID", SqlDbType.Int),
				new SqlParameter("@ADCount", SqlDbType.Int),
				new SqlParameter("@MediaID", SqlDbType.Int)
             };
            parameters[0].Value = (int)mediaType;
            parameters[1].Value = name;
            parameters[2].Value = number;
            parameters[3].Value = rightSql;
            parameters[4].Value = needCount;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[6].Direction = ParameterDirection.Output;
            parameters[7].Direction = ParameterDirection.InputOutput;
            parameters[7].Value = mediaID;
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_IsExists", parameters));
            dto.IsExists = count > 0;
            dto.PubID = Convert.ToInt32(parameters[5].Value);
            dto.ADCount = Convert.ToInt32(parameters[6].Value);
            dto.MediaID = Convert.ToInt32(parameters[7].Value);
            return dto;
        }

        /// <summary>
        /// 获取媒体字典
        /// ls
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<MediaDictDTO> GetMediaDict(MediaTypeEnum mediaType, string name, string number, string rightSql)
        {

            SqlParameter[] parameters = new SqlParameter[]
            {
				new SqlParameter("@MediaType", SqlDbType.Int),
				new SqlParameter("@Name", SqlDbType.VarChar),
				new SqlParameter("@Number", SqlDbType.VarChar),
                new SqlParameter("@RightSql", SqlDbType.VarChar)
             };
            parameters[0].Value = (int)mediaType;
            parameters[1].Value = name;
            parameters[2].Value = number;
            parameters[3].Value = rightSql;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetDict", parameters);
            List<MediaDictDTO> list = null;
            if (ds.Tables.Count == 0)
                return list;
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                list = new List<MediaDictDTO>();
                foreach (DataRow dr in dt.Rows)
                {
                    MediaDictDTO item = new MediaDictDTO()
                    {
                        MediaID = Convert.ToInt32(dr["MediaID"]),
                        Name = dr["Name"].ToString(),
                        Number = dr["Number"].ToString()
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        public int GetMediaTypeByPubID(int pubID) {
            try
            {
                string sql = "select top 1 MediaType from Publish_BasicInfo where PubID = " + pubID;
                int mediaType = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
                return mediaType;
            }
            catch {
                return 0;
            }
        }

        public int GetMediaCreateUserIDByPubID(int pubID) {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@PubID", pubID),
                    new SqlParameter("@MediaCreateUserID", DbType.Int32)
                };
                parameters[1].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetMediaCreateUserID", parameters);
                return Convert.ToInt32(parameters[1].Value);
            }
            catch {
                return -1;
            }
        }

        /// <summary>
        /// 获取模板可用状态、审核状态 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IsTemplateID">是否模板ID 否则刊例ID</param>
        /// <param name="createUserID">创建者ID 0表示AE</param>
        /// <returns>为0为不可用</returns>
        public int GetTemplateStatus(int id, bool IsTemplateID, int createUserID) {
            string sql = string.Empty;
            if (IsTemplateID)
            {
                sql = string.Format(@"
                select AuditStatus from App_AdTemplate 
                where Status = 0 
                and RecID = {0} 
                and ( AuditStatus = {1} or (AuditStatus <> {1} and {2}) )",
                id, (int)AppTemplateEnum.已通过, createUserID.Equals(0) ? "CreateUserID in (select UserID from UserRole where RoleID = 'SYS001RL00005')" : "CreateUserID = " + createUserID);
            }
            else
            {
                sql = "select AuditStatus from App_AdTemplate where exists(select 1 from Publish_BasicInfo where PubID = " + id + " and App_AdTemplate.RecID = Publish_BasicInfo.TemplateID)";
            }
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return Convert.ToInt32(obj == null ? 0 : obj);
        }

        /// <summary>
        /// 根据媒体ID 获取媒体的创建人ID
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaID"></param>
        /// <returns></returns>
        public int GetMediaCreateUserIDByMediaID(MediaTypeEnum mediaType, int mediaID) {
            try
            {
                string sql = string.Empty;
                switch (mediaType)
                {
                    case MediaTypeEnum.微信:
                        sql = "select CreateUserID from Media_Weixin where MediaID = @MediaID";
                        break;
                    case MediaTypeEnum.APP:
                        sql = "select CreateUserID from Media_PCAPP where MediaID = @MediaID";
                        break;
                    case MediaTypeEnum.微博:
                        sql = "select CreateUserID from Media_Weibo where MediaID = @MediaID";
                        break;
                    case MediaTypeEnum.视频:
                        sql = "select CreateUserID from Media_Video where MediaID = @MediaID";
                        break;
                    case MediaTypeEnum.直播:
                        sql = "select CreateUserID from Media_Broadcast where MediaID = @MediaID";
                        break;
                }
                SqlParameter[] parameters = new SqlParameter[]
                { 
                    new SqlParameter("@MediaID",mediaID),
                };
                return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            }
            catch {
                return -1;
            }
        }

        public int AuditPublish(int pubID, int status, int auditUserID, string rightSql,out int nextPubID) {

            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@PubID", pubID),
                new SqlParameter("@Status", status),
                new SqlParameter("@UserID", auditUserID),
                new SqlParameter("@RightSql", rightSql),
                new SqlParameter("@RowCount", SqlDbType.Int),
                new SqlParameter("@NextPubID", SqlDbType.Int),
            };
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_ModifyStateV1_1", parameters);
            nextPubID = Convert.ToInt32(parameters[5].Value);
            int rowcount = Convert.ToInt32(parameters[4].Value);
            return rowcount;
        }

        public OldPublishDTO GetWeixinOldPublish(int pubID)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@PubID", pubID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetWeixinOld", parameters);
            DataTable dt1 = ds.Tables[0];
            if (dt1 == null || dt1.Rows.Count.Equals(0))
            {
                return null;
            }
            else
            {
                OldPublishDTO publish = new OldPublishDTO()
                {
                    BeginTime = Convert.ToDateTime(dt1.Rows[0]["BeginTime"]),
                    EndTime = Convert.ToDateTime(dt1.Rows[0]["EndTime"]),
                    IsAppointment = dt1.Rows[0]["IsAppointment"].ToString().Equals("1"),
                    PurchaseDiscount = Convert.ToDecimal(dt1.Rows[0]["PurchaseDiscount"]),
                    SaleDiscount = Convert.ToDecimal(dt1.Rows[0]["SaleDiscount"])
                };
                DataTable dt2 = ds.Tables[1];
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    List<OldADDetailDTO> list = new List<OldADDetailDTO>();
                    foreach (DataRow dr in dt2.Rows)
                    {
                        list.Add(new OldADDetailDTO()
                        {
                            ADPosition = dr["ADPosition"].ToString(),
                            ADPositionName = dr["ADPositionName"].ToString(),
                            Price = Convert.ToDecimal(dr["Price"]),
                            SalePrice = Convert.ToDecimal(dr["SalePrice"])
                        });
                    }
                    publish.Details = list;
                }
                DataTable dt3 = ds.Tables[2];
                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    string str = string.Empty;
                    foreach (DataRow dr in dt3.Rows)
                    {
                        str += dr["RemarkName"].ToString();
                    }
                    publish.OrderRemarkName = str;
                }
                return publish;
            }
            
        }

        public OldPublishDTO GetAppOldPublish(int pubID)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@PubID", pubID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetAppOld", parameters);
            DataTable dt1 = ds.Tables[0];
            if (dt1 == null || dt1.Rows.Count.Equals(0))
            {
                return null;
            }
            else
            {
                OldPublishDTO publish = new OldPublishDTO()
                {
                    BeginTime = Convert.ToDateTime(dt1.Rows[0]["BeginTime"]),
                    EndTime = Convert.ToDateTime(dt1.Rows[0]["EndTime"]),
                    IsAppointment = dt1.Rows[0]["IsAppointment"].ToString().Equals("1"),
                    PurchaseDiscount = Convert.ToDecimal(dt1.Rows[0]["PurchaseDiscount"]),
                    SaleDiscount = Convert.ToDecimal(dt1.Rows[0]["SaleDiscount"])
                };
                DataTable dt2 = ds.Tables[1];
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    publish.Prices = DataTableToList<AppPriceInfo>(dt2);
                }
                return publish;
            }

        }

        /// <summary>
        /// 添加刊例基础信息
        /// ls
        /// </summary>
        /// <param name="model">值对象</param>
        /// <returns>成功返回新增记录ID</returns>
        public int AddPublishBasicInfo_V1_1(ModifyPublish pub, string rightSql, List<ModifyPublishDetail> detailList, List<ADPrice> appPriceList)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region Publish_BasicInfo
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@PubID", SqlDbType.Int),
                            new SqlParameter("@MediaType", SqlDbType.Int),
                            new SqlParameter("@MediaID", SqlDbType.Int),
                            new SqlParameter("@PubCode", SqlDbType.VarChar, 20),
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
                            new SqlParameter("@RightSql", SqlDbType.VarChar, 200),
                            new SqlParameter("@Wx_Status", SqlDbType.VarChar, 200),
                            new SqlParameter("@PubName", SqlDbType.VarChar, 50),
                            new SqlParameter("@IsAppointment", SqlDbType.Bit),
                            new SqlParameter("@TemplateID", SqlDbType.Int),
                            new SqlParameter("@HasHoliday", SqlDbType.Bit)
                        };
                        parameters[0].Direction = ParameterDirection.Output;
                        parameters[1].Value = (int)pub.MediaType;
                        parameters[2].Value = pub.MediaID;
                        parameters[3].Value = pub.PubCode;
                        parameters[4].Value = pub.BeginTime.Date;
                        parameters[5].Value = pub.EndTime.Date;
                        parameters[6].Value = pub.PurchaseDiscount;
                        parameters[7].Value = pub.SaleDiscount;
                        parameters[8].Value = Entities.Constants.Constant.INT_INVALID_VALUE;
                        parameters[9].Value = Entities.Constants.Constant.INT_INVALID_VALUE;
                        parameters[10].Value = pub.CreateTime;
                        parameters[11].Value = pub.CreateUserID;
                        parameters[12].Value = pub.LastUpdateTime;
                        parameters[13].Value = pub.LastUpdateUserID;
                        parameters[14].Value = rightSql;
                        parameters[15].Value = pub.Wx_Status;
                        parameters[16].Value = pub.PubName;
                        parameters[17].Value = pub.IsAppointment;
                        parameters[18].Value = pub.TemplateID;
                        parameters[19].Value = pub.HasHoliday;
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_BasicInfo_Add_V1_1", parameters);
                        #endregion
                        int pubID = Convert.ToInt32(parameters[0].Value);
                        if (pubID > 0)
                        {
                            string sql = string.Empty;

                            #region V1.1.1广告名称
                            if (pub.MediaType.Equals((int)MediaTypeEnum.微信) && !string.IsNullOrEmpty(pub.ADName) && pub.MediaID > 0) {
                                sql = "update Media_Weixin set ADName = @ADName where MediaID = @MediaID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@ADName",SqlFilter(pub.ADName)),
                                    new SqlParameter("@MediaID",pub.MediaID)
                                };
                                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                            }
                            #endregion

                            if (!string.IsNullOrEmpty(pub.ImgUrl))
                            {
                                #region  刊例附件
                                sql = "insert into Publish_FileInfo(PubID,FileUrl,CreateTime,CreateUserID) values(@PubID,@FileUrl,@CreateTime,@CreateUserID)";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@PubID", pubID),
                                    new SqlParameter("@FileUrl", pub.ImgUrl),
                                    new SqlParameter("@CreateTime", DateTime.Now),
                                    new SqlParameter("@CreateUserID", pub.CreateUserID)
                                };
                                int rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);

                                if (pub.MediaType.Equals((int)MediaTypeEnum.APP) && rowcount > 0)
                                {
                                    #region V1.1.4 更新有效期一样的刊例附件
                                    sql = @"
                                    UPDATE  Publish_FileInfo SET FileUrl = @FileUrl 
                                    WHERE EXISTS (
                                        SELECT 1 FROM   Publish_BasicInfo 
                                        WHERE  dbo.Publish_BasicInfo.PubID = dbo.Publish_FileInfo.PubID
                                        AND MediaType = 14002
                                        AND CONVERT(VARCHAR(10), BeginTime, 23) = @BeginTime
                                        AND CONVERT(VARCHAR(10), EndTime, 23) = @EndTime 
                                    )";
                                    parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("@FileUrl", pub.ImgUrl),
                                        new SqlParameter("@BeginTime", pub.BeginTime.ToString("yyyy-MM-dd")),
                                        new SqlParameter("@EndTime", pub.EndTime.ToString("yyyy-MM-dd"))
                                    };
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                    #endregion
                                }
                                #endregion
                            }
                            if (pub.MediaType.Equals((int)MediaTypeEnum.微信) && detailList != null && detailList.Count > 0)
                            {
                                #region  V1.1.1 微信 广告位及附件
                                foreach (var detail in detailList)
                                {
                                    parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("@RecID", SqlDbType.Int),
                                        new SqlParameter("@PubID", SqlDbType.Int),
                                        new SqlParameter("@MediaType", SqlDbType.Int),
                                        new SqlParameter("@ADPosition1", SqlDbType.Int),
                                        new SqlParameter("@ADPosition2", SqlDbType.Int),
                                        new SqlParameter("@ADPosition3", SqlDbType.Int),
                                        new SqlParameter("@Price", SqlDbType.Decimal),
                                        new SqlParameter("@IsCarousel", SqlDbType.Bit),
                                        new SqlParameter("@BeginPlayDays", SqlDbType.Int),
                                        new SqlParameter("@PublishStatus", SqlDbType.Int),
                                        new SqlParameter("@CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@CreateUserID", SqlDbType.Int),
                                        new SqlParameter("@RightSql", SqlDbType.VarChar, 200),
                                        new SqlParameter("@SalePrice", SqlDbType.Decimal),
                                    };
                                    parameters[0].Direction = ParameterDirection.Output;
                                    parameters[1].Value = pubID;
                                    parameters[2].Value = pub.MediaType;
                                    parameters[3].Value = detail.ADPosition1;
                                    parameters[4].Value = detail.ADPosition2;
                                    parameters[5].Value = detail.ADPosition3;
                                    parameters[6].Value = detail.Price;
                                    parameters[7].Value = Entities.Constants.Constant.INT_INVALID_VALUE;
                                    parameters[8].Value = 0;
                                    parameters[9].Value = Entities.Constants.Constant.INT_INVALID_VALUE;
                                    parameters[10].Value = pub.CreateTime;
                                    parameters[11].Value = pub.CreateUserID;
                                    parameters[12].Value = rightSql;
                                    parameters[13].Value = detail.SalePrice;
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Add_V1_1", parameters);
                                    int adDetailID = Convert.ToInt32(parameters[0].Value);
                                    if (adDetailID > 0 && detail.ImgUrls != null && detail.ImgUrls.Count > 0)
                                    {
                                        int ps = 1;
                                        foreach (string url in detail.ImgUrls)
                                        {
                                            sql = "insert into PubDeatil_ImgInfo(ADDetailID,ImageUrl,ImagePosition,CreateTime,CreateUserID) values (@ADDetailID,@ImageUrl,@ImagePosition,@CreateTime,@CreateUserID)";
                                            parameters = new SqlParameter[] 
                                            {
                                                new SqlParameter("@ADDetailID", adDetailID),
                                                new SqlParameter("@ImageUrl", url),
                                                new SqlParameter("@ImagePosition", ps),
                                                new SqlParameter("@CreateTime", pub.CreateTime),
                                                new SqlParameter("@CreateUserID", pub.CreateUserID),
                                            };
                                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                            ps++;
                                        }
                                    }
                                }
                                #endregion
                            }
                            if (pub.MediaType.Equals((int)MediaTypeEnum.APP) && appPriceList != null && appPriceList.Count > 0) {
                                #region V1.1.4 APP 广告价格
                                foreach (var now in appPriceList)
                                {
                                    parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("@PubID", pubID),
                                        new SqlParameter("@TemplateID", pub.TemplateID),
                                        new SqlParameter("@MediaID", pub.MediaID),
                                        new SqlParameter("@ADStyle", now.ADStyle),
                                        new SqlParameter("@CarouselNumber",now.CarouselNumber),
                                        new SqlParameter("@SalePlatform", now.SalePlatform),
                                        new SqlParameter("@SaleType", now.SaleType),
                                        new SqlParameter("@SaleArea", now.SaleArea),
                                        new SqlParameter("@ClickCount", now.ClickCount),
                                        new SqlParameter("@ExposureCount", now.ExposureCount),
                                        new SqlParameter("@PubPrice_Holiday", now.PubPrice_Holiday),
                                        new SqlParameter("@SalePrice_Holiday", now.SalePrice_Holiday),
                                        new SqlParameter("@PubPrice", now.PubPrice),
                                        new SqlParameter("@SalePrice", now.SalePrice),
                                        new SqlParameter("@Status", now.Status),
                                        new SqlParameter("@CreateUserID", pub.CreateUserID),
                                        new SqlParameter("@CreateTime", pub.CreateTime),
                                        new SqlParameter("@RightSql", rightSql),
                                    };
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_AppPriceInfo_Add", parameters);
                                }
                                #endregion
                            }
                            trans.Commit();
                        }
                        return pubID;
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// 更新刊例基础信息
        /// ls
        /// </summary>
        /// <returns>成功>0</returns>
        public int UpdataPublishBasicInfo_V1_1(ModifyPublish pub, string rightSql, List<ModifyPublishDetail> detailList, List<ADPrice> appPriceList)
        {

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region Publish_BasicInfo
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("@PubID", SqlDbType.Int),
                            new SqlParameter("@BeginTime", SqlDbType.DateTime),
                            new SqlParameter("@EndTime", SqlDbType.DateTime),
                            new SqlParameter("@PurchaseDiscount", SqlDbType.Decimal),
                            new SqlParameter("@SaleDiscount", SqlDbType.Decimal),
                            new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
                            new SqlParameter("@LastUpdateUserID", SqlDbType.Int),
                            new SqlParameter("@RightSql", SqlDbType.VarChar),
                            new SqlParameter("@IsAppointment", SqlDbType.Bit),
                            new SqlParameter("@Wx_Status", SqlDbType.Int),
                            new SqlParameter("@PubName", SqlDbType.VarChar),
                            new SqlParameter("@HasHoliday",SqlDbType.Bit),
                            new SqlParameter("@RowCount",SqlDbType.Int)
                        };
                        parameters[0].Value = pub.PubID;
                        parameters[1].Value = pub.BeginTime.Date;
                        parameters[2].Value = pub.EndTime.Date;
                        parameters[3].Value = pub.PurchaseDiscount;
                        parameters[4].Value = pub.SaleDiscount;
                        parameters[5].Value = pub.LastUpdateTime;
                        parameters[6].Value = pub.LastUpdateUserID;
                        parameters[7].Value = rightSql;
                        parameters[8].Value = pub.IsAppointment;
                        parameters[9].Value = pub.Wx_Status;
                        parameters[10].Value = pub.PubName;
                        parameters[11].Value = pub.HasHoliday;
                        parameters[12].Direction = ParameterDirection.Output;
                        //修改存储过程中已经删除了Detail数据
                        #endregion
                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_BasicInfo_Update_V1_1", parameters);
                        int count = Convert.ToInt32(parameters[12].Value);
                        if (count > 0)
                        {
                            string sql = string.Empty;
                            #region V1.1.1广告名称
                            if(pub.MediaType == (int)MediaTypeEnum.微信 && !string.IsNullOrEmpty(pub.ADName) && pub.MediaID > 0)
                            {
                                sql = "update Media_Weixin set ADName = @ADName where MediaID = @MediaID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@ADName",SqlFilter(pub.ADName)),
                                    new SqlParameter("@MediaID",pub.MediaID)
                                };
                                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                            }
                            #endregion
                            if (!string.IsNullOrEmpty(pub.ImgUrl))
                            {
                                #region  刊例附件
                                sql = "delete from Publish_FileInfo where PubID = @PubID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@PubID", pub.PubID)
                                };
                                SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);

                                sql = "insert into Publish_FileInfo(PubID,FileUrl,CreateTime,CreateUserID) values(@PubID,@FileUrl,@CreateTime,@CreateUserID)";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@PubID", pub.PubID),
                                    new SqlParameter("@FileUrl", pub.ImgUrl),
                                    new SqlParameter("@CreateTime", DateTime.Now),
                                    new SqlParameter("@CreateUserID", pub.CreateUserID)
                                };
                                int rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);

                                if (pub.MediaType.Equals((int)MediaTypeEnum.APP) && rowcount > 0)
                                {
                                    #region V1.1.4 更新有效期一样的刊例附件
                                    sql = @"
                                    UPDATE  Publish_FileInfo SET FileUrl = @FileUrl 
                                    WHERE EXISTS (
                                        SELECT 1 FROM   Publish_BasicInfo 
                                        WHERE  dbo.Publish_BasicInfo.PubID = dbo.Publish_FileInfo.PubID
                                        AND MediaType = 14002
                                        AND CONVERT(VARCHAR(10), BeginTime, 23) = @BeginTime
                                        AND CONVERT(VARCHAR(10), EndTime, 23) = @EndTime 
                                    )";
                                    parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("@FileUrl", pub.ImgUrl),
                                        new SqlParameter("@BeginTime", pub.BeginTime.ToString("yyyy-MM-dd")),
                                        new SqlParameter("@EndTime", pub.EndTime.ToString("yyyy-MM-dd"))
                                    };
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                    #endregion
                                }

                                #endregion
                            }
                            if (pub.MediaType.Equals((int)MediaTypeEnum.微信))
                            {
                                #region  V1.1.1 微信 广告位及附件
                                foreach (var detail in detailList)
                                {
                                    parameters = new SqlParameter[]
                                    {
                                        new SqlParameter("@RecID", SqlDbType.Int),
                                        new SqlParameter("@PubID", SqlDbType.Int),
                                        new SqlParameter("@MediaType", SqlDbType.Int),
                                        new SqlParameter("@ADPosition1", SqlDbType.Int),
                                        new SqlParameter("@ADPosition2", SqlDbType.Int),
                                        new SqlParameter("@ADPosition3", SqlDbType.Int),
                                        new SqlParameter("@Price", SqlDbType.Decimal),
                                        new SqlParameter("@IsCarousel", SqlDbType.Bit),
                                        new SqlParameter("@BeginPlayDays", SqlDbType.Int),
                                        new SqlParameter("@PublishStatus", SqlDbType.Int),
                                        new SqlParameter("@CreateTime", SqlDbType.DateTime),
                                        new SqlParameter("@CreateUserID", SqlDbType.Int),
                                        new SqlParameter("@RightSql", SqlDbType.VarChar, 200),
                                        new SqlParameter("@SalePrice", SqlDbType.Decimal),
                                    };
                                    parameters[0].Direction = ParameterDirection.Output;
                                    parameters[1].Value = pub.PubID;
                                    parameters[2].Value = pub.MediaType;
                                    parameters[3].Value = detail.ADPosition1;
                                    parameters[4].Value = detail.ADPosition2;
                                    parameters[5].Value = detail.ADPosition3;
                                    parameters[6].Value = detail.Price;
                                    parameters[7].Value = Entities.Constants.Constant.INT_INVALID_VALUE;
                                    parameters[8].Value = 0;
                                    parameters[9].Value = Entities.Constants.Constant.INT_INVALID_VALUE;
                                    parameters[10].Value = pub.LastUpdateTime;
                                    parameters[11].Value = pub.LastUpdateUserID;
                                    parameters[12].Value = rightSql;
                                    parameters[13].Value = detail.SalePrice;
                                    SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_Publish_DetailInfo_Add_V1_1", parameters);
                                    int adDetailID = Convert.ToInt32(parameters[0].Value);
                                    if (adDetailID > 0 && detail.ImgUrls != null && detail.ImgUrls.Count > 0)
                                    {
                                        int ps = 1;
                                        foreach (string url in detail.ImgUrls)
                                        {
                                            sql = "insert into PubDeatil_ImgInfo(ADDetailID,ImageUrl,ImagePosition,CreateTime,CreateUserID) values (@ADDetailID,@ImageUrl,@ImagePosition,@CreateTime,@CreateUserID)";
                                            parameters = new SqlParameter[]
                                            {
                                            new SqlParameter("@ADDetailID", adDetailID),
                                            new SqlParameter("@ImageUrl", url),
                                            new SqlParameter("@ImagePosition", ps),
                                            new SqlParameter("@CreateTime", DateTime.Now),
                                            new SqlParameter("@CreateUserID", pub.LastUpdateUserID),
                                            };
                                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                            ps++;
                                        }
                                    }
                                }
                                #endregion
                            }
                            if (pub.MediaType.Equals((int)MediaTypeEnum.APP) && appPriceList != null && appPriceList.Count > 0)
                            {
                                #region V1.1.4 APP 广告价格
                                sql = "SELECT * FROM dbo.Publish_BasicInfo WHERE PubID = " + pub.PubID;
                                var oldPub = DataTableToEntity<PublishBasicInfo>(SqlHelper.ExecuteDataset(trans, CommandType.Text, sql).Tables[0]);
                                sql = "SELECT * FROM AppPriceInfo WHERE Status = 0 AND PubID = " + pub.PubID;
                                List<AppPriceInfo> hasList = DataTableToList<AppPriceInfo>(SqlHelper.ExecuteDataset(trans, CommandType.Text, sql).Tables[0]);
                                appPriceList.ForEach(now => {
                                    if (!now.RecID.Equals(0) && hasList.Exists(old => old.RecID.Equals(now.RecID)))
                                    {
                                        #region 修改
                                        parameters = new SqlParameter[]
                                        {
                                            new SqlParameter("@RecID", now.RecID),
                                            new SqlParameter("@ADStyle", now.ADStyle),
                                            new SqlParameter("@CarouselNumber",now.CarouselNumber),
                                            new SqlParameter("@SalePlatform", now.SalePlatform),
                                            new SqlParameter("@SaleType", now.SaleType),
                                            new SqlParameter("@SaleArea", now.SaleArea),
                                            new SqlParameter("@ClickCount", now.ClickCount),
                                            new SqlParameter("@ExposureCount", now.ExposureCount),
                                            new SqlParameter("@PubPrice_Holiday", now.PubPrice_Holiday),
                                            new SqlParameter("@SalePrice_Holiday", now.SalePrice_Holiday),
                                            new SqlParameter("@PubPrice", now.PubPrice),
                                            new SqlParameter("@SalePrice", now.SalePrice),
                                            new SqlParameter("@RightSql", rightSql),
                                        };
                                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_AppPriceInfo_Update", parameters);
                                        #endregion
                                    }
                                    else if(now.RecID.Equals(0) && !hasList.Exists(old => old.RecID.Equals(now.RecID))){
                                        #region 新增
                                        parameters = new SqlParameter[]
                                        {
                                            new SqlParameter("@PubID", pub.PubID),
                                            new SqlParameter("@TemplateID", oldPub.TemplateID),
                                            new SqlParameter("@MediaID", oldPub.MediaID),
                                            new SqlParameter("@ADStyle", now.ADStyle),
                                            new SqlParameter("@CarouselNumber",now.CarouselNumber),
                                            new SqlParameter("@SalePlatform", now.SalePlatform),
                                            new SqlParameter("@SaleType", now.SaleType),
                                            new SqlParameter("@SaleArea", now.SaleArea),
                                            new SqlParameter("@ClickCount", now.ClickCount),
                                            new SqlParameter("@ExposureCount", now.ExposureCount),
                                            new SqlParameter("@PubPrice_Holiday", now.PubPrice_Holiday),
                                            new SqlParameter("@SalePrice_Holiday", now.SalePrice_Holiday),
                                            new SqlParameter("@PubPrice", now.PubPrice),
                                            new SqlParameter("@SalePrice", now.SalePrice),
                                            new SqlParameter("@Status", Convert.ToInt32(0)),
                                            new SqlParameter("@CreateUserID", pub.LastUpdateUserID),
                                            new SqlParameter("@CreateTime", pub.LastUpdateTime),
                                            new SqlParameter("@RightSql", rightSql),
                                        };
                                        SqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "p_AppPriceInfo_Add", parameters);
                                        #endregion
                                    }
                                });

                                #region 删除
                                hasList.ForEach(old => {
                                    sql = string.Empty;
                                    if (!appPriceList.Exists(now => now.RecID.Equals(old.RecID))) {
                                        sql += "update AppPriceInfo set Status = -1 where RecID = " + old.RecID + ";";
                                    }
                                    if (!string.IsNullOrWhiteSpace(sql))
                                        SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                                });
                                #endregion

                                #endregion
                            }
                        }
                        trans.Commit();
                        return count;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }

        }

        public int DeletePublishBasicInfo(int pubID) {

            string sql = "Update Publish_BasicInfo set IsDel = -1 where IsDel = 0 and ((MediaType = 14001 and Wx_Status <> 42011) or (MediaType = 14002 and Wx_Status <> 49004)) and PubID = " + pubID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// <summary>
        /// 检查排期冲突
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="mediaID">媒体ID（新增）</param>
        /// <param name="templateID">模板ID（新增 APP）</param>
        /// <param name="pubID">刊例ID（编辑）</param>
        /// <returns></returns>
        public bool CheckIsConflict(int mediaType, string beginDate, string endDate, int mediaID,  int templateID, int pubID)
        {
            string sql = string.Empty;
            SqlParameter[] parameters = null;
            if (mediaType.Equals((int)MediaTypeEnum.微信))
            {
                sql = @"select count(1) from Publish_BasicInfo 
                        where (CONVERT(varchar(10), BeginTime, 23) <= @EndDate 
                        and CONVERT(varchar(10), EndTime, 23) >= @BeginDate) 
                        and MediaType =@MediaType
                        and IsDel = 0 
                        and PubID <> @PubID
                        and MediaID = @MediaID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@BeginDate",beginDate),
                    new SqlParameter("@EndDate",endDate),
                    new SqlParameter("@MediaType",mediaType),
                    new SqlParameter("@PubID",pubID),
                    new SqlParameter("@MediaID",mediaID)
                };
                var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                return Convert.ToInt32(obj) > 0;
            }
            else if (mediaType.Equals((int)MediaTypeEnum.APP))
            {
                sql = @"
                        select count(1) from Publish_BasicInfo 
                        where (CONVERT(varchar(10), BeginTime, 23) <= @EndDate 
                        and CONVERT(varchar(10), EndTime, 23) >= @BeginDate) 
                        and IsDel = 0 
                        and MediaType =@MediaType
                        and PubID <> @PubID
                        and TemplateID = @TemplateID
                        and MediaID = @MediaID
                        and Wx_Status in (49001,49002,49004,49006)";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@BeginDate",beginDate),
                    new SqlParameter("@EndDate",endDate),
                    new SqlParameter("@MediaType",mediaType),
                    new SqlParameter("@PubID",pubID),
                    new SqlParameter("@TemplateID",templateID),
                    new SqlParameter("@MediaID",mediaID)
                };
                var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                return Convert.ToInt32(obj) > 0;
            }
            else
                return false;
        }

        public Entities.Publish.PublishBasicInfo GetDetail(int pubID) {

            string sql = "select * from Publish_BasicInfo where PubID = "+pubID;
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.Publish.PublishBasicInfo>(ds.Tables[0]);
        }

        public int GetAppPublishStatus(int templateID, int createUserID) {

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Wx_Status",DbType.Int32),
                new SqlParameter("@TemplateID",DbType.Int32),
                new SqlParameter("@CreateUserID",DbType.Int32),
            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = templateID;
            parameters[2].Value = createUserID;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetAppPublishStatus", parameters);
            return Convert.ToInt32(parameters[0].Value);
        }

        public List<Entities.AdTemplate.AppAdTemplate> GetAppPriceList(int pubID) {

            string sql = "select * from AppPriceInfo where PubID = " + pubID;
            List<Entities.AdTemplate.AppAdTemplate> list = DataTableToList<Entities.AdTemplate.AppAdTemplate>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            return list;
        }

        #region V1.1.4
        public GetPublishListBResDTO GetAppPublishList(int mediaID, string mediaName, string adName,
    string userName, string beginDate, string endDate, int pubStatus, string rightSql,
    string orderBy, int pageIndex, int pageSize)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID",mediaID),
                new SqlParameter("@MediaName",mediaName),
                new SqlParameter("@ADName",adName),
                new SqlParameter("@UserName",userName),
                new SqlParameter("@BeginDate",beginDate),
                new SqlParameter("@EndDate",endDate),
                new SqlParameter("@PubStatus",pubStatus),
                new SqlParameter("@RightSql",rightSql),
                new SqlParameter("@PageIndex",pageIndex),
                new SqlParameter("@PageSize",pageSize),
                new SqlParameter("@Orderby",orderBy),
                new SqlParameter("@TotalCount",SqlDbType.Int),
            };
            parameters[11].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetAppADList", parameters);
            int totalCount = Convert.ToInt32(parameters[11].Value);
            List<ADPublishItemB> list = DataTableToList<ADPublishItemB>(ds.Tables[0]);
            return new GetPublishListBResDTO() { List = list, Total = totalCount };
        }

        public List<PubDateItem> GetPubDateList(int mediaID, int templateID)
        {
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@TemplateID", templateID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Publish_GetPubDateList", parameters);
            var list = DataTableToList<PubDateItem>(ds.Tables[0]);
            string fileName = string.Empty;
            int index = 0;
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
            return list;
        }

        #endregion

        #region V1.1.8

        public int AddPublishBasicInfo(PublishBasicInfo model)
        {
            string sql = @"INSERT INTO dbo.Publish_BasicInfo( MediaType ,MediaID ,PubCode ,BeginTime ,EndTime ,PurchaseDiscount ,SaleDiscount ,Status ,PublishStatus ,CreateTime ,CreateUserID ,LastUpdateTime ,LastUpdateUserID ,IsAppointment ,Wx_Status ,PubName ,IsDel ,TemplateID ,HasHoliday,OriginalReferencePrice)
                                    VALUES ( @MediaType ,@MediaID ,@PubCode ,@BeginTime ,@EndTime ,@PurchaseDiscount ,@SaleDiscount ,@Status ,@PublishStatus ,@CreateTime ,@CreateUserID ,@LastUpdateTime ,@LastUpdateUserID ,@IsAppointment ,@Wx_Status ,@PubName ,@IsDel ,@TemplateID ,@HasHoliday,@OriginalReferencePrice);
                                    SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@MediaType", (int)model.MediaType),
                new SqlParameter("@MediaID", model.MediaID),
                new SqlParameter("@PubCode", model.PubCode),
                new SqlParameter("@BeginTime", model.BeginTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@PurchaseDiscount", model.PurchaseDiscount),
                new SqlParameter("@SaleDiscount", model.SaleDiscount),
                new SqlParameter("@Status", (int)model.Status),
                new SqlParameter("@PublishStatus", (int)model.PublishStatus),
                new SqlParameter("@CreateTime", model.CreateTime),
                new SqlParameter("@CreateUserID", model.CreateUserID),
                new SqlParameter("@LastUpdateTime", model.LastUpdateTime),
                new SqlParameter("@LastUpdateUserID", model.LastUpdateUserID),
                new SqlParameter("@IsAppointment", model.IsAppointment),
                new SqlParameter("@Wx_Status", (int)model.Wx_Status),
                new SqlParameter("@PubName", model.PubName),
                new SqlParameter("@IsDel", model.IsDel),
                new SqlParameter("@TemplateID", model.TemplateID),
                new SqlParameter("@HasHoliday", model.HasHoliday),
                new SqlParameter("@OriginalReferencePrice", model.OriginalReferencePrice)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int UpdatePublishBasicInfo(PublishBasicInfo model)
        {
            string sql = @"UPDATE dbo.Publish_BasicInfo SET 
                                    PubCode = @PubCode,
                                    BeginTime = @BeginTime,
                                    EndTime = @EndTime,
                                    PurchaseDiscount=@PurchaseDiscount,
                                    SaleDiscount = @SaleDiscount, 
                                    Status = @Status,
                                    PublishStatus = @PublishStatus, 
                                    LastUpdateTime = @LastUpdateTime,
                                    LastUpdateUserID = @LastUpdateUserID,
                                    IsAppointment = @IsAppointment,
                                    Wx_Status = @Wx_Status,
                                    PubName = @PubName,
                                    TemplateID = @TemplateID,
                                    HasHoliday = @HasHoliday,
                                    OriginalReferencePrice = @OriginalReferencePrice
                                    WHERE PubID = @PubID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PubCode", model.PubCode),
                new SqlParameter("@BeginTime", model.BeginTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@PurchaseDiscount", model.PurchaseDiscount),
                new SqlParameter("@SaleDiscount", model.SaleDiscount),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@PublishStatus", model.PublishStatus),
                new SqlParameter("@LastUpdateTime", model.LastUpdateTime),
                new SqlParameter("@LastUpdateUserID", model.LastUpdateUserID),
                new SqlParameter("@IsAppointment", model.IsAppointment),
                new SqlParameter("@Wx_Status", model.Wx_Status),
                new SqlParameter("@PubName", model.PubName),
                new SqlParameter("@TemplateID", model.TemplateID),
                new SqlParameter("@HasHoliday", model.HasHoliday),
                new SqlParameter("@OriginalReferencePrice", model.OriginalReferencePrice)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int DeleteCrossDatePublish(int mediaID, DateTime beginTime, DateTime endTime)
        {
            int rowcount = 0;

            #region 处理 左交叉、右交叉、包含
            string sql = @"UPDATE  dbo.Publish_BasicInfo SET EndTime = @NewEndTime
                            WHERE   dbo.Publish_BasicInfo.MediaID = @MediaID
                            AND IsDel = 0
                            AND dbo.Publish_BasicInfo.MediaType = 14001
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.BeginTime, 23) < CONVERT(VARCHAR(10), @BeginDate, 23)
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.EndTime, 23) >= CONVERT(VARCHAR(10), @BeginDate, 23)
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.EndTime, 23) <= CONVERT(VARCHAR(10), @EndDate, 23);

                            UPDATE  dbo.Publish_BasicInfo SET BeginTime = @NewBeginTime
                            WHERE   dbo.Publish_BasicInfo.MediaID = @MediaID
                            AND IsDel = 0
                            AND dbo.Publish_BasicInfo.MediaType = 14001
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.EndTime, 23) > CONVERT(VARCHAR(10), @EndDate, 23)
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.BeginTime, 23) >= CONVERT(VARCHAR(10), @BeginDate, 23)
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.BeginTime, 23) <= CONVERT(VARCHAR(10), @EndDate, 23);

                            UPDATE  dbo.Publish_BasicInfo SET IsDel = -1,Wx_Status = 42012
                            WHERE   dbo.Publish_BasicInfo.MediaID = @MediaID
                            AND dbo.Publish_BasicInfo.MediaType = 14001
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.BeginTime, 23) >= CONVERT(VARCHAR(10), @BeginDate, 23)
                            AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.EndTime, 23) <= CONVERT(VARCHAR(10), @EndDate, 23);";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@BeginDate", beginTime),
                new SqlParameter("@EndDate", endTime),
                new SqlParameter("@NewBeginTime", endTime.AddDays(1)),
                new SqlParameter("@NewEndTime", beginTime.AddDays(-1)),
            };
            rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            #endregion

            #region 处理被包含
            sql = @"SELECT PubID,BeginTime,EndTime FROM dbo.Publish_BasicInfo 
                        WHERE  dbo.Publish_BasicInfo.MediaID = @MediaID 
                        AND dbo.Publish_BasicInfo.MediaType = 14001
                        AND IsDel = 0
                        AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.BeginTime, 23) < CONVERT(VARCHAR(10), @BeginDate, 23)  
                        AND CONVERT(VARCHAR(10), dbo.Publish_BasicInfo.EndTime, 23) > CONVERT(VARCHAR(10), @EndDate, 23)";
            parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@BeginDate", beginTime),
                new SqlParameter("@EndDate", endTime)
            };
            var crossPubList = DataTableToList<PublishBasicInfo>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0]);
            foreach (var pub in crossPubList)
            {
                //复制刊例
                sql = @"INSERT INTO dbo.Publish_BasicInfo( MediaType ,MediaID ,PubCode ,BeginTime ,EndTime ,PurchaseDiscount ,SaleDiscount ,Status ,PublishStatus ,CreateTime ,CreateUserID ,LastUpdateTime ,LastUpdateUserID ,IsAppointment ,Wx_Status ,PubName ,IsDel ,TemplateID ,HasHoliday ,OriginalReferencePrice)
                            SELECT MediaType ,MediaID ,PubCode ,@NewBeginTime ,EndTime ,PurchaseDiscount ,SaleDiscount ,Status ,PublishStatus ,CreateTime ,CreateUserID ,LastUpdateTime ,LastUpdateUserID ,IsAppointment ,Wx_Status ,PubName ,IsDel ,TemplateID ,HasHoliday ,OriginalReferencePrice FROM dbo.Publish_BasicInfo WHERE PubID = @PubID;
                            SELECT @@IDENTITY";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@NewBeginTime", endTime.AddDays(1)),
                    new SqlParameter("@PubID", pub.PubID)
                };
                var newPubID = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                rowcount++;
                //复制广告位
                sql = @"INSERT INTO dbo.Publish_DetailInfo( PubID ,MediaType ,MediaID ,ADPosition1 ,ADPosition2 ,ADPosition3 ,Price ,IsCarousel ,BeginPlayDays ,PublishStatus ,CreateTime ,CreateUserID ,SalePrice ,CostReferencePrice ,CostDetailID)
		                    SELECT @NewPubID ,MediaType ,MediaID ,ADPosition1 ,ADPosition2 ,ADPosition3 ,Price ,IsCarousel ,BeginPlayDays ,PublishStatus ,CreateTime ,CreateUserID ,SalePrice ,CostReferencePrice ,CostDetailID FROM dbo.Publish_DetailInfo WHERE PubID = @PubID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@NewPubID", newPubID),
                    new SqlParameter("@PubID", pub.PubID)
                };
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                //更新原刊例
                sql = @"UPDATE  dbo.Publish_BasicInfo SET EndTime = @NewEndTime
                            WHERE   dbo.Publish_BasicInfo.PubID = @PubID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@NewEndTime", beginTime.AddDays(-1)),
                    new SqlParameter("@PubID", pub.PubID)
                };
                rowcount += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            }
            #endregion

            return rowcount;
        }

        public int AddPublishDetail(PublishDetailInfo model)
        {
            string sql = @"INSERT INTO dbo.Publish_DetailInfo( PubID ,MediaType ,MediaID ,ADPosition1 ,ADPosition2 ,ADPosition3 ,Price ,IsCarousel ,BeginPlayDays ,PublishStatus ,CreateTime ,CreateUserID ,SalePrice ,CostReferencePrice ,CostDetailID)
                                    VALUES ( @PubID ,@MediaType ,@MediaID ,@ADPosition1 ,@ADPosition2 ,@ADPosition3 ,@Price ,@IsCarousel ,@BeginPlayDays ,@PublishStatus ,@CreateTime ,@CreateUserID ,@SalePrice ,@CostReferencePrice ,@CostDetailID);
                                    SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@PubID", model.PubID),
                new SqlParameter("@MediaType", (int)model.MediaType),
                new SqlParameter("@MediaID", model.MediaID),
                new SqlParameter("@ADPosition1", model.ADPosition1),
                new SqlParameter("@ADPosition2", model.ADPosition2),
                new SqlParameter("@ADPosition3", model.ADPosition3),
                new SqlParameter("@Price", model.Price),
                new SqlParameter("@IsCarousel", model.IsCarousel),
                new SqlParameter("@BeginPlayDays", model.BeginPlayDays),
                new SqlParameter("@PublishStatus", (int)model.PublishStatus),
                new SqlParameter("@CreateTime", model.CreateTime),
                new SqlParameter("@CreateUserID", model.CreateUserID),
                new SqlParameter("@SalePrice", model.SalePrice),
                new SqlParameter("@CostReferencePrice", model.CostReferencePrice),
                new SqlParameter("@CostDetailID", model.CostDetailID)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public List<PublishDetailInfo> GetPublishDetailListByPubID(int pubID)
        {
            string sql = "SELECT * FROM dbo.Publish_DetailInfo WHERE PubID = " + pubID;
            return DataTableToList<PublishDetailInfo>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
        }
        #endregion

    }
}