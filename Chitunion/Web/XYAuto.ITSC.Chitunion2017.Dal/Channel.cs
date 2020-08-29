using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Channel;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class Channel : DataBase
    {
        public static readonly Channel Instance = new Channel();

        /// <summary>
        /// 新增编辑渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool ModifyChannel(ModifyChannelReqDTO dto, ref int channelID)
        {
            string sql = string.Empty;
            int rowcount = 0;
            int defaultValue = 0;
            SqlParameter[] parameters = null;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (dto.ChannelID.Equals(0))
                        {
                            #region 新建渠道

                            sql = @"INSERT INTO dbo.ChannelInfo( ChannelName ,IncludingTax ,CooperateBeginDate ,CooperateEndDate ,Remark ,Status ,CreateUserID ,CreateTime, LastUpdateTime)
                                          VALUES( @ChannelName ,@IncludingTax ,@CooperateBeginDate ,@CooperateEndDate ,@Remark ,@Status ,@CreateUserID ,@CreateTime, @LastUpdateTime);
                                         SELECT @@IDENTITY";
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@ChannelName", dto.ChannelName),
                                new SqlParameter("@IncludingTax", dto.IncludingTax),
                                new SqlParameter("@CooperateBeginDate", dto.CooperateBeginDate.Date),
                                new SqlParameter("@CooperateEndDate", dto.CooperateEndDate.Date),
                                new SqlParameter("@Remark", dto.Remark),
                                new SqlParameter("@CreateUserID", dto.CreateUserID),
                                new SqlParameter("@CreateTime", dto.CreateTime),
                                new SqlParameter("@LastUpdateTime", dto.LastUpdateTime),
                                new SqlParameter("@Status", defaultValue),
                            };
                            dto.ChannelID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));

                            #endregion 新建渠道
                        }
                        else
                        {
                            #region 更新渠道

                            sql = @"UPDATE dbo.ChannelInfo SET ChannelName = @ChannelName, IncludingTax = @IncludingTax, CooperateBeginDate = @CooperateBeginDate, CooperateEndDate = @CooperateEndDate, Remark = @Remark, LastUpdateTime = @LastUpdateTime
                                          WHERE ChannelID = @ChannelID";
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@ChannelName", dto.ChannelName),
                                new SqlParameter("@IncludingTax", dto.IncludingTax),
                                new SqlParameter("@CooperateBeginDate", dto.CooperateBeginDate.Date),
                                new SqlParameter("@CooperateEndDate", dto.CooperateEndDate.Date),
                                new SqlParameter("@Remark", dto.Remark),
                                new SqlParameter("@LastUpdateTime", dto.LastUpdateTime),
                                new SqlParameter("@ChannelID", dto.ChannelID),
                            };
                            rowcount = Convert.ToInt32(SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters));
                            if (rowcount.Equals(0))
                            {
                                trans.Rollback();
                                return false;
                            }

                            #endregion 更新渠道

                            #region 逻辑删除政策

                            string range = string.Empty;
                            dto.PolicyList.Where(a => !a.PolicyID.Equals(0)).Select(a => a.PolicyID).ToList().ForEach(a => range += (a + ","));
                            if (range.Length > 0)
                                range = range.Substring(0, range.Length - 1);
                            else
                                range = "0";
                            sql = "UPDATE dbo.ChannelPolicy SET Status = -1 WHERE ChannelID = " + dto.ChannelID + " AND PolicyID NOT IN (" + range + ")";
                            rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);

                            #endregion 逻辑删除政策
                        }

                        foreach (var policy in dto.PolicyList)
                        {
                            if (policy.PolicyID.Equals(0))
                            {
                                #region 新增政策 Status = 0

                                sql = @"INSERT INTO dbo.ChannelPolicy( ChannelID ,Quota ,QuotaIncludingEqual ,SingleAccountSum ,SingleAccountSumType ,PurchaseDiscount ,RebateType1 ,RebateType2 ,RebateValue ,RebateDateType ,Status ,CreateUserID ,CreateTime, LastUpdateTime)
                                              VALUES( @ChannelID ,@Quota ,@QuotaIncludingEqual ,@SingleAccountSum ,@SingleAccountSumType ,@PurchaseDiscount ,@RebateType1 ,@RebateType2 ,@RebateValue ,@RebateDateType ,@Status ,@CreateUserID ,@CreateTime, @LastUpdateTime);
                                              SELECT @@IDENTITY";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@ChannelID", dto.ChannelID),
                                    new SqlParameter("@Quota", policy.Quota),
                                    new SqlParameter("@QuotaIncludingEqual", policy.QuotaIncludingEqual),
                                    new SqlParameter("@SingleAccountSum",policy.SingleAccountSum),
                                    new SqlParameter("@SingleAccountSumType", policy.SingleAccountSumType),
                                    new SqlParameter("@PurchaseDiscount", policy.PurchaseDiscount),
                                    new SqlParameter("@RebateType1", policy.RebateType1),
                                    new SqlParameter("@RebateType2", policy.RebateType2),
                                    new SqlParameter("@RebateValue", policy.RebateValue),
                                    new SqlParameter("@RebateDateType", policy.RebateDateType),
                                    new SqlParameter("@Status", defaultValue),
                                    new SqlParameter("@CreateUserID", dto.CreateUserID),
                                    new SqlParameter("@CreateTime", dto.CreateTime),
                                    new SqlParameter("@LastUpdateTime", dto.LastUpdateTime)
                                };
                                policy.PolicyID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));

                                #endregion 新增政策 Status = 0
                            }
                            else
                            {
                                #region 更新政策

                                sql = @"UPDATE dbo.ChannelPolicy SET Quota = @Quota, QuotaIncludingEqual = @QuotaIncludingEqual, SingleAccountSum = @SingleAccountSum, SingleAccountSumType = @SingleAccountSumType,
                                          PurchaseDiscount = @PurchaseDiscount, RebateType1 = @RebateType1, RebateType2 = @RebateType2, RebateValue = @RebateValue, RebateDateType = @RebateDateType, LastUpdateTime = @LastUpdateTime
                                          WHERE PolicyID = @PolicyID AND ChannelID = @ChannelID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@Quota", policy.Quota),
                                    new SqlParameter("@QuotaIncludingEqual", policy.QuotaIncludingEqual),
                                    new SqlParameter("@SingleAccountSum",policy.SingleAccountSum),
                                    new SqlParameter("@SingleAccountSumType", policy.SingleAccountSumType),
                                    new SqlParameter("@PurchaseDiscount", policy.PurchaseDiscount),
                                    new SqlParameter("@RebateType1", policy.RebateType1),
                                    new SqlParameter("@RebateType2", policy.RebateType2),
                                    new SqlParameter("@RebateValue", policy.RebateValue),
                                    new SqlParameter("@RebateDateType", policy.RebateDateType),
                                    new SqlParameter("@LastUpdateTime", dto.LastUpdateTime),
                                    new SqlParameter("@PolicyID", policy.PolicyID),
                                    new SqlParameter("@ChannelID", dto.ChannelID)
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                if (rowcount.Equals(0))
                                {
                                    trans.Rollback();
                                    return false;
                                }

                                #endregion 更新政策
                            }
                        }
                        channelID = dto.ChannelID;
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否重复
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool CheckChannelIsRepeat(ModifyChannelReqDTO dto)
        {
            string sql = @"SELECT  COUNT(1)
                                      FROM dbo.ChannelInfo
                                      WHERE Status = 0
                                      AND ChannelName = @ChannelName
                                      AND ChannelID <> @ChannelID
                                      AND CONVERT(VARCHAR(10), CooperateBeginDate, 23) <= CONVERT(VARCHAR(10), @CooperateEndDate, 23)
		                              AND CONVERT(VARCHAR(10), CooperateEndDate, 23) >= CONVERT(VARCHAR(10), @CooperateBeginDate, 23); ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                            new SqlParameter("@ChannelName", dto.ChannelName),
                            new SqlParameter("@ChannelID", dto.ChannelID),
                            new SqlParameter("@CooperateBeginDate", dto.CooperateBeginDate),
                            new SqlParameter("@CooperateEndDate", dto.CooperateEndDate)
            };
            int rowcount = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            return rowcount > 0;
        }

        public int SelectMediaCountOfChannel(int channelID)
        {
            string sql = "SELECT COUNT(1) FROM dbo.ChannelCost WHERE Status = 0 AND ChannelID = " + channelID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// <summary>
        /// 删除渠道
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public bool DeleteChannel(int channelID)
        {
            string sql = @"UPDATE dbo.ChannelInfo SET Status = -1 WHERE ChannelID = @ChannelID;
                                    UPDATE dbo.ChannelPolicy SET Status = -1 WHERE ChannelID = @ChannelID;";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@ChannelID", channelID) };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters) > 0;
        }

        /// <summary>
        /// 获取渠道列表
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="status">0正常 1过期 -2全部(不包括删除)</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetChannelListResDTO GetChannelList(string channelName, int status, int pageIndex, int pageSize)
        {
            GetChannelListResDTO res = new GetChannelListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ChannelName", channelName),
                new SqlParameter("@Status", status),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[4].Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetChannelList", parameters).Tables[0];
            res.TotalCount = Convert.ToInt32(parameters[4].Value);
            res.List = DataTableToList<ChannelItem>(dt);
            return res;
        }

        /// <summary>
        /// 获取渠道详情
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public GetChannelInfoResDTO GetChannelDetail(int channelID)
        {
            GetChannelInfoResDTO res = new GetChannelInfoResDTO();
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@ChannelID", channelID) };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetChannelDetail", parameters);
            DataTable channelDt = ds.Tables[0];
            DataTable policyDt = ds.Tables[1];
            res = DataTableToEntity<GetChannelInfoResDTO>(channelDt);
            res.PolicyList = DataTableToList<PolicyInfo>(policyDt);
            return res;
        }

        public Dictionary<int, string> GetDictNames(int dictType)
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();
            string sql = "SELECT DictId,DictName FROM dbo.DictInfo WHERE DictType = " + dictType;
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int id = Convert.ToInt32(dr["DictId"]);
                    string name = dr["DictName"].ToString();
                    if (!dict.Keys.Contains(id))
                        dict.Add(id, name);
                }
            }
            return dict;
        }

        public Tuple<int, int> GetAreaNames(string provinceName, string cityName)
        {
            string sql = "SELECT TOP 1 AreaID FROM dbo.AreaInfo WHERE Level = 1 AND AreaName = '" + provinceName + "'";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {//先查省
                int provinceID = Convert.ToInt32(dt.Rows[0]["AreaID"]);
                if (string.IsNullOrWhiteSpace(cityName))
                    return new Tuple<int, int>(provinceID, -2);
                else
                {
                    sql = "SELECT TOP 1 AreaID FROM dbo.AreaInfo WHERE Level = 2 AND AreaName = '" + cityName + "'";
                    dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int cityID = Convert.ToInt32(dt.Rows[0]["AreaID"]);
                        return new Tuple<int, int>(provinceID, cityID);
                    }
                    else
                    {
                        return new Tuple<int, int>(provinceID, -99);
                    }
                }
            }
            else
                return new Tuple<int, int>(-99, -99);
        }

        public GetCostMediaListResDTO GetCostMediaList(string mediaName)
        {
            GetCostMediaListResDTO res = new GetCostMediaListResDTO();
            string sql = @"SELECT DISTINCT dbo.Media_Weixin.MediaID,dbo.Media_Weixin.Name AS MediaName FROM dbo.ChannelCost
                                    INNER JOIN dbo.Media_Weixin ON Media_Weixin.MediaID = ChannelCost.MediaID
                                    WHERE dbo.ChannelCost.Status = 0 AND dbo.Media_Weixin.Name LIKE '%" + SqlFilter(mediaName) + "%'";
            res.List = DataTableToList<GetCostMediaItem>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            return res;
        }

        public GetCostChannelListResDTO GetCostChannelList()
        {
            GetCostChannelListResDTO res = new GetCostChannelListResDTO();
            string sql = @"SELECT DISTINCT dbo.ChannelInfo.ChannelID,dbo.ChannelInfo.ChannelName FROM dbo.ChannelCost
                                    INNER JOIN dbo.ChannelInfo ON ChannelInfo.ChannelID = ChannelCost.ChannelID
                                    WHERE dbo.ChannelCost.Status = 0";
            res.List = DataTableToList<GetCostChannelItem>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            return res;
        }

        public bool DeleteCost(int costID)
        {
            string sql = @"UPDATE dbo.ChannelCostDetail SET Status = -1 WHERE CostID = @CostID
                                    UPDATE dbo.ChannelCost SET Status = -1 WHERE CostID = @CostID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CostID" ,costID)
            };
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return rowcount > 0;
        }

        public bool BatchCostOperate(List<int> costIDList, int opType)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int costID in costIDList)
            {
                sb.Append(costID + ",");
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
            }
            string sql = "UPDATE dbo.ChannelCost SET SaleStatus = " + opType + " WHERE CostID IN (" + sb.ToString() + ") AND SaleStatus <> " + opType;
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            return rowcount > 0;
        }

        /// <summary>
        /// 获取成本列表
        /// </summary>
        /// <param name="mediaID"></param>
        /// <param name="channelID"></param>
        /// <param name="saleStatus"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetCostListResDTO GetCostList(int mediaID, int channelID, int saleStatus, int pageIndex, int pageSize)
        {
            GetCostListResDTO res = new GetCostListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID", mediaID),
                new SqlParameter("@ChannelID", channelID),
                new SqlParameter("@SaleStatus", saleStatus),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[5].Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetChannelCostList", parameters).Tables[0];
            res.TotalCount = Convert.ToInt32(parameters[5].Value);
            res.List = DataTableToList<CostListItem>(dt);
            return res;
        }

        public List<int> GetNeedComputeMediaIDList(int channelID)
        {
            string sql = "SELECT DISTINCT MediaID FROM dbo.ChannelCost WHERE ChannelID = " + channelID + " AND Status = 0 AND SaleStatus = " + (int)Entities.Enum.MediaPublishStatusEnum.UpOnshelf;
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            List<int> list = new List<int>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(Convert.ToInt32(dr["MediaID"]));
                }
            }
            return list;
        }

        public GetCostDetailResDTO GetCostDetail(int costID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CostID", costID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetChannelCostDetail", parameters);
            var res = DataTableToEntity<GetCostDetailResDTO>(ds.Tables[0]);
            if (res != null)
            {
                res.CostDetailList = DataTableToList<CostItem>(ds.Tables[1]);
            }
            return res;
        }

        public int AddCost(ChannelCost cost)
        {
            string sql = @"INSERT INTO dbo.ChannelCost( MediaID ,ChannelID ,SaleStatus ,OriginalPrice ,Status ,CreateUserID ,CreateTime ,LastUpdateTime)
                                     VALUES(@MediaID ,@ChannelID ,@SaleStatus ,@OriginalPrice ,@Status ,@CreateUserID ,@CreateTime ,@LastUpdateTime);
                                     SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[]
            {
                            new SqlParameter("@MediaID", cost.MediaID),
                            new SqlParameter("@ChannelID", cost.ChannelID),
                            new SqlParameter("@SaleStatus", cost.SaleStatus),
                            new SqlParameter("@OriginalPrice", cost.OriginalPrice),
                            new SqlParameter("@Status", cost.Status),
                            new SqlParameter("@CreateUserID", cost.CreateUserID),
                            new SqlParameter("@CreateTime", cost.CreateTime),
                            new SqlParameter("@LastUpdateTime", cost.LastUpdateTime)
            };
            int costID = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            return costID;
        }

        public bool UpdateCost(ChannelCost cost)
        {
            string sql = @"UPDATE dbo.ChannelCost set SaleStatus = @SaleStatus ,OriginalPrice = @OriginalPrice ,Status = @Status ,LastUpdateTime = @LastUpdateTime
                                    WHERE CostID = @CostID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                            new SqlParameter("@SaleStatus", cost.SaleStatus),
                            new SqlParameter("@OriginalPrice", cost.OriginalPrice),
                            new SqlParameter("@Status", cost.Status),
                            new SqlParameter("@LastUpdateTime", cost.LastUpdateTime),
                            new SqlParameter("@CostID", cost.CostID)
            };
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return rowcount > 0;
        }

        public int AddCostDetail(ChannelCostDetail detail)
        {
            string sql = @"INSERT INTO dbo.ChannelCostDetail( CostID ,ChannelID ,ADPosition1 ,ADPosition2 ,ADPosition3 ,CostPrice ,SalePrice ,Status ,CreateUserID ,CreateTime)
                                    VALUES(@CostID ,@ChannelID ,@ADPosition1 ,@ADPosition2 ,@ADPosition3 ,@CostPrice ,@SalePrice ,@Status ,@CreateUserID ,@CreateTime);
                                    SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[]
            {
                            new SqlParameter("@CostID", detail.CostID),
                            new SqlParameter("@ChannelID", detail.ChannelID),
                            new SqlParameter("@ADPosition1", detail.ADPosition1),
                            new SqlParameter("@ADPosition2", detail.ADPosition2),
                            new SqlParameter("@ADPosition3", detail.ADPosition3),
                            new SqlParameter("@CostPrice", detail.CostPrice),
                            new SqlParameter("@SalePrice", detail.SalePrice),
                            new SqlParameter("@Status", detail.Status),
                            new SqlParameter("@CreateUserID", detail.CreateUserID),
                            new SqlParameter("@CreateTime", detail.CreateTime),
            };
            int detailID = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            return detailID;
        }

        public bool UpdateCostDetail(ChannelCostDetail detail)
        {
            string sql = @"UPDATE dbo.ChannelCostDetail set CostPrice = @CostPrice ,SalePrice = @SalePrice ,Status = @Status
                                    WHERE DetailID = @DetailID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                            new SqlParameter("@CostPrice", detail.CostPrice),
                            new SqlParameter("@SalePrice", detail.SalePrice),
                            new SqlParameter("@Status", detail.Status),
                            new SqlParameter("@DetailID", detail.DetailID),
            };
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return rowcount > 0;
        }

        public int DeleteCostDetail(List<int> detailIDs)
        {
            string range = string.Join(",", detailIDs);
            string sql = string.Format("UPDATE dbo.ChannelCostDetail SET Status = -1 WHERE DetailID IN ({0})", range);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public ChannelCost GetCost(int costID)
        {
            string sql = "SELECT * FROM dbo.ChannelCost WHERE CostID = " + costID;
            return DataTableToEntity<ChannelCost>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
        }

        /// <summary>
        /// 获取AE角色下的 媒体信息、分类、备注、分布区域、渠道对应下的成本和广告位
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public OldDataDTO GetAEMediaInfo(int channelID)
        {
            string sql = @"SELECT  mw.MediaID ,mw.Number ,mw.Name ,ISNULL(ai1.AreaName,'') AS ProvinceName,ISNULL(ai2.AreaName,'') AS CityName ,mw.FansCount ,mw.LevelType ,ISNULL(d1.DictName,'') AS LevelTypeName ,mw.WxID ,mw.CreateTime ,mw.CreateUserID
                                    FROM    dbo.Media_Weixin mw
									LEFT JOIN dbo.Media_Area_Mapping mam ON mw.MediaID = mam.MediaID AND mam.MediaType = 14001 AND mam.RelateType = 59002
									LEFT JOIN dbo.AreaInfo ai1 ON mam.ProvinceID = ai1.AreaID
									LEFT JOIN dbo.AreaInfo ai2 ON mam.CityID = ai2.AreaID
                                    LEFT JOIN dbo.DictInfo d1 ON mw.LevelType = d1.DictId
                                    WHERE   mw.Status = 0
                                                   AND EXISTS ( SELECT 1
                                                                          FROM   dbo.UserRole
                                                                          WHERE  RoleID IN ( 'SYS001RL00005' )
                                                                          AND dbo.UserRole.UserID = mw.CreateUserID );

                                    SELECT  mcc.MediaID,mcc.CategoryID ,ISNULL(d1.DictName,'') AS CategoryName FROM dbo.Media_CommonlyClass mcc
                                    LEFT JOIN dbo.DictInfo d1 ON mcc.CategoryID = d1.DictId
                                    WHERE   MediaType = 14001
                                            AND EXISTS ( SELECT *
                                                         FROM   dbo.Media_Weixin mw
                                                         WHERE  Status = 0
                                                                AND EXISTS ( SELECT 1
                                                                             FROM   dbo.UserRole
                                                                             WHERE  RoleID IN ( 'SYS001RL00005' )
                                                                                    AND dbo.UserRole.UserID = mw.CreateUserID )
                                                                AND mcc.MediaID = mw.MediaID );

                                    SELECT  RecID ,RelationID ,RemarkID,ISNULL(d1.DictName,'') AS RemarkName FROM dbo.Publish_Remark pr
                                    LEFT JOIN dbo.DictInfo d1 ON pr.RemarkID = d1.DictId
                                    WHERE   EnumType = 45002
                                            AND EXISTS ( SELECT *
                                                         FROM   dbo.Media_Weixin mw
                                                         WHERE  Status = 0
                                                                AND EXISTS ( SELECT 1
                                                                             FROM   dbo.UserRole
                                                                             WHERE  RoleID IN ( 'SYS001RL00005' )
                                                                                    AND dbo.UserRole.UserID = mw.CreateUserID )
                                                                AND pr.RelationID = mw.MediaID );

                                    SELECT  map.MediaID ,map.ProvinceID ,map.CityID,ISNULL(a1.AreaName,'') as ProvinceName,ISNULL(a2.AreaName,'') as CityName FROM dbo.Media_Area_Mapping map
                                    LEFT JOIN dbo.AreaInfo a1 ON map.ProvinceID = a1.AreaID and a1.Level = 1
                                    LEFT JOIN dbo.AreaInfo a2 ON map.CityID = a2.AreaID and a2.Level = 2
                                    WHERE   MediaType = 14001
                                            AND RelateType = 59002
                                            AND EXISTS ( SELECT *
                                                         FROM   dbo.Media_Weixin mw
                                                         WHERE  Status = 0
                                                                AND EXISTS ( SELECT 1
                                                                             FROM   dbo.UserRole
                                                                             WHERE  RoleID IN ( 'SYS001RL00005' )
                                                                                    AND dbo.UserRole.UserID = mw.CreateUserID )
                                                                AND map.MediaID = mw.MediaID );

                                    SELECT  cc.CostID,cc.MediaID ,mw.Name AS WxName ,mw.Number AS WxNumber ,cc.OriginalPrice
                                    FROM    dbo.ChannelCost cc
                                            INNER JOIN dbo.Media_Weixin mw ON cc.MediaID = mw.MediaID
                                    WHERE   cc.ChannelID = @ChannelID;

                                    SELECT  ccd.DetailID ,cc.MediaID ,mw.Name AS WxName ,mw.Number AS WxNumber ,( d1.DictName + d2.DictName ) AS ADPosition ,ccd.SalePrice ,ccd.CostPrice,
                                    ccd.ADPosition1,ccd.ADPosition2,ccd.ADPosition3
                                    FROM    dbo.ChannelCostDetail ccd
                                    INNER JOIN dbo.ChannelCost cc ON ccd.CostID = cc.CostID
                                    INNER JOIN dbo.Media_Weixin mw ON cc.MediaID = mw.MediaID
                                    INNER JOIN dbo.DictInfo d1 ON ccd.ADPosition1 = d1.DictId
                                    INNER JOIN dbo.DictInfo d2 ON ccd.ADPosition3 = d2.DictId
                                    WHERE ccd.ChannelID = @ChannelID AND ccd.Status = 0;";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@ChannelID", channelID) };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            var mediaList = DataTableToList<MediaWeixin>(ds.Tables[0]);
            var categoryList = DataTableToList<MediaCommonlyClass>(ds.Tables[1]);
            var remarkList = DataTableToList<PublishRemark>(ds.Tables[2]);
            var areaList = DataTableToList<MediaAreaMapping>(ds.Tables[3]);
            var costList = DataTableToList<ChannelCost>(ds.Tables[4]);
            var priceList = DataTableToList<ChannelCostDetail>(ds.Tables[5]);
            return new OldDataDTO { MediaList = mediaList, CategoryList = categoryList, RemarkList = remarkList, AreaList = areaList, CostList = costList, PriceList = priceList };
        }

        /// <summary>
        /// 获取媒体范围内、有效期交叉的、广告位范围内的广告位信息
        /// </summary>
        /// <param name="channelID">排除渠道 没有传0</param>
        /// <param name="mediaRange">涉及媒体ID</param>
        /// <param name="beginDate">交叉日期-开始日期</param>
        /// <param name="endDate">交叉日期-结束日期</param>
        /// <param name="adPositionRange">涉及广告位范围</param>
        /// <returns></returns>
        public List<ChannelCostDetail> PreviewGetCostDetailList(int channelID, string mediaRange, DateTime beginDate, DateTime endDate, string adPositionRange)
        {
            string sql = string.Format(
                               @"SELECT  wx.MediaID ,
                                    wx.Name AS WxName ,wx.Number AS WxNumber ,
                                    ci.ChannelID ,ci.ChannelName ,ci.CooperateBeginDate ,ci.CooperateEndDate ,
                                    cc.CostID ,
                                    cc.OriginalPrice,
                                    ccd.DetailID ,ccd.ADPosition1 ,ccd.ADPosition2 ,ccd.ADPosition3 ,
		                            (d1.DictName+d3.DictName) AS ADPosition,
                                    ccd.SalePrice ,ccd.CostPrice
                            FROM    dbo.ChannelCostDetail ccd
                                    INNER JOIN dbo.ChannelCost cc ON ccd.CostID = cc.CostID
                                    INNER JOIN dbo.ChannelInfo ci ON cc.ChannelID = ci.ChannelID
                                    INNER JOIN dbo.Media_Weixin wx ON cc.MediaID = wx.MediaID
		                            INNER JOIN dbo.DictInfo d1 ON ccd.ADPosition1 = d1.DictId
		                            INNER JOIN dbo.DictInfo d2 ON ccd.ADPosition2 = d2.DictId
		                            INNER JOIN dbo.DictInfo d3 ON ccd.ADPosition3 = d3.DictId
                            WHERE   ci.ChannelID <> @ChannelID
                                    AND cc.MediaID IN ({0})
                                    AND cc.SaleStatus = {1}
                                    AND cc.Status = 0
                                    AND CONVERT(VARCHAR(10), ci.CooperateBeginDate, 23) <= CONVERT(VARCHAR(10), @CooperateEndDate, 23)
                                    AND CONVERT(VARCHAR(10), ci.CooperateEndDate, 23) >= CONVERT(VARCHAR(10), @CooperateBeginDate, 23)
                                    AND ccd.Status = 0"
                            , mediaRange, (int)Entities.Enum.MediaPublishStatusEnum.UpOnshelf);
            if (!string.IsNullOrWhiteSpace(adPositionRange))
                sql += (" AND ( CAST(ccd.ADPosition1 AS VARCHAR) + CAST(ccd.ADPosition2 AS VARCHAR) + CAST(ccd.ADPosition3 AS VARCHAR) ) IN (" +adPositionRange+ ");");
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ChannelID", channelID),
                new SqlParameter("@CooperateBeginDate", beginDate),
                new SqlParameter("@CooperateEndDate", endDate)
            };
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            return DataTableToList<ChannelCostDetail>(dt);
        }

        public List<Entities.Channel.PolicyInfo> GetList(int channelId)
        {
            var sql = @"
                    SELECT * FROM DBO.ChannelPolicy WITH ( NOLOCK ) WHERE Status = 0 AND  ChannelID =@ChannelID
                    ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@ChannelID",channelId)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<PolicyInfo>(data.Tables[0]);
        }

        public List<ChannelInfo> GetCrossChannel(DateTime beginTime, DateTime endTime)
        {
            string sql = @"SELECT ChannelID,
                                    (CASE WHEN CooperateBeginDate > @BeginTime THEN CooperateBeginDate ELSE @BeginTime END) AS CooperateBeginDate,
                                    (CASE WHEN CooperateEndDate < @EndTime THEN CooperateEndDate ELSE @EndTime END) AS CooperateEndDate,
                                    CreateUserID
                                    FROM dbo.ChannelInfo 
                                    WHERE Status = 0 
                                    AND dbo.ChannelInfo.CooperateBeginDate <= @EndTime
                                    AND dbo.ChannelInfo.CooperateEndDate >= @BeginTime";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@BeginTime", beginTime),
                new SqlParameter("@EndTime", endTime)
            };
            var crossChannelList = DataTableToList<ChannelInfo>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0]);
            return crossChannelList;
        }
    }
}