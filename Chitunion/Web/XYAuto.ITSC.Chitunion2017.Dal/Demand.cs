using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class Demand : DataBase
    {
        public static readonly Demand Instance = new Demand();

        public DemandDTO GetDemandEntityByBillNo(int demandBillNo)
        {
            string sql = $"SELECT * FROM dbo.GDT_Demand WHERE DemandBillNo = {demandBillNo}";
            return DataTableToEntity<DemandDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
        }

        public GetDemandListResDTO GetDemandList(string createUser, string belongYY, string demandName, int auditStatus, int yyUserID, int pageIndex, int pageSize)
        {
            GetDemandListResDTO res = new GetDemandListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CreateUser", createUser),
                new SqlParameter("@BelongYY", belongYY),
                new SqlParameter("@DemandName", demandName),
                new SqlParameter("@AuditStatus", auditStatus),
                new SqlParameter("@YYUserID", yyUserID),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters.Last().Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetDemandList", parameters).Tables[0];
            res.TotalCount = Convert.ToInt32(parameters.Last().Value);
            res.List = DataTableToList<DemandDTO>(dt);
            return res;
        }

        public GetDemandDetailResDTO GetDemandDetail(int demandBillNo, int yyUserID)
        {
            GetDemandDetailResDTO res = new GetDemandDetailResDTO();
            string sql = $@"SELECT  d.DemandId ,
			                                      d.DemandBillNo ,
			                                      d.Name ,
			                                      d1.DictName AS AuditStatusName ,
			                                      d.BrandSerialJson ,
			                                      d.ProvinceCityJson ,
			                                      d.DistributorJson ,
			                                      d.PromotionPolicy ,
			                                      d.TotalBudget ,
			                                      d.BeginDate ,
			                                      d.EndDate ,
			                                      d.ClueNumber ,
			                                      ui1.UserName AS CreateUserName,
                                                  ui2.UserName AS BelongYY,
			                                      AuditTime = (SELECT MAX(CreateTime) FROM dbo.AuditInfo WHERE RelationType = 91001 AND RelationId = d.DemandBillNo AND AuditStatus in (89002,89003)),
			                                      RejectReason = (SELECT TOP 1 RejectMsg FROM dbo.AuditInfo WHERE RelationType = 91001 AND RelationId = d.DemandBillNo AND AuditStatus in (89002,89003) ORDER BY CreateTime desc)
	                                FROM    dbo.GDT_Demand d
			                                      LEFT JOIN dbo.DictInfo d1 ON d.AuditStatus = d1.DictId
			                                      LEFT JOIN dbo.UserInfo ui1 ON d.CreateUserId = ui1.UserID
                                                  LEFT JOIN dbo.GDT_RoleUser ru on d.CreateUserId = ru.UserID
                                                  LEFT JOIN dbo.UserInfo ui2 ON ru.AuthToUserId = ui2.UserID
                                    WHERE  d.DemandBillNo = {demandBillNo} AND ({yyUserID} = 0 OR ru.AuthToUserId = {yyUserID})";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sql).Tables[0];
            res.Demand = DataTableToEntity<DemandDTO>(dt);

            sql = $@"SELECT  ag.AdgroupId,
                                          ag.AdgroupName ,
                                          ag.PullTime,
                                          d1.DictName AS SystemStatusName ,
                                          t.*
                            FROM    dbo.GDT_AdGroup ag
                                          LEFT JOIN dbo.GDT_AccountRelation ar ON ag.AccountId = ar.AccountId
                                          LEFT JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
		                                  INNER JOIN dbo.GDT_DemandRelation dr ON ag.AdgroupId = dr.AdgroupId
                                          LEFT JOIN dbo.DictInfo d1 ON ag.SystemStatus = d1.DictId
                                          LEFT JOIN ( SELECT  hr.AdgroupId ,
                                                             SUM(hr.Impression) AS TotalImpression ,
                                                             SUM(hr.Click) AS TotalClick ,
                                                             CAST(( SUM(hr.Click) / SUM(hr.Impression) ) AS DECIMAL(18,2)) AS AvgClickPercent ,
                                                             SUM(hr.Cost) AS TotalCost ,
                                                             SUM(hr.OrderQuantity) AS OrderQuantity ,
                                                             SUM(hr.BillOfQuantities) AS BillOfQuantities
                                                             FROM    dbo.GDT_HourlyRrport hr
                                                             GROUP BY hr.AdgroupId
                                                           ) t ON ag.AdgroupId = t.AdgroupId
                                    
                            WHERE   dr.DemandBillNo = {demandBillNo}
                            AND       ({yyUserID} = 0 OR ru.AuthToUserId = {yyUserID})
                            ORDER    BY    ag.CreatedTime DESC";
            res.ADGroupList = DataTableToList<ADGroupDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            return res;
        }

        public bool AuditDemand(int demandBillNo, int auditStatus, string rejectReason, int auditUserID)
        {
            string sql = string.Empty;
            int rowcount = 0;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (auditStatus == (int)Entities.Enum.GDT.DemandAuditStatusEnum.Terminated)
                        {
                            sql = $@"UPDATE dbo.GDT_Demand SET AuditStatus = {auditStatus} 
                                      WHERE AuditStatus in ({(int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn},{(int)Entities.Enum.GDT.DemandAuditStatusEnum.Puting})
                                      AND DemandBillNo = {demandBillNo}";
                        }
                        else
                        {
                            sql = $@"UPDATE dbo.GDT_Demand SET AuditStatus = {auditStatus} 
                                      WHERE AuditStatus = {(int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingAudit} 
                                      AND DemandBillNo = {demandBillNo}";
                        }
                        rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                        if (rowcount > 0)
                        {
                            sql = $@"INSERT INTO dbo.AuditInfo( RelationType ,RelationId ,AuditStatus ,RejectMsg ,CreateTime ,CreateUserId) 
                                          VALUES(91001 ,{demandBillNo} ,{auditStatus} ,'{SqlFilter(rejectReason)}' ,GETDATE() ,{auditUserID})";
                            SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                        }
                        trans.Commit();
                        return rowcount > 0;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }


        }

        public int SelectNextWaittingAuditDemand()
        {
            string sql = "SELECT TOP 1 DemandBillNo FROM dbo.GDT_Demand WHERE AuditStatus = 89001 ORDER BY UpdateTime DESC";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : (int)obj;
        }


        public List<ADGroupDTO> GetCanSelectADGroupList(int demandBillNo)
        {//有效的 系统状态
            string sql = $@"SELECT  ag.AdgroupId,
                                                    ag.AdgroupName,
                                                    ag.ConfiguredStatus,
                                                    c.CampaignName,
                                                    CAST((CASE WHEN dr.DemandBillNo IS NULL THEN 0 ELSE 1 END) AS BIT) AS IsChecked
                                       FROM    dbo.GDT_AdGroup ag
                                       LEFT JOIN dbo.GDT_DemandRelation dr ON ag.AdgroupId = dr.AdgroupId
                                       LEFT JOIN dbo.GDT_Campaign c ON ag.CampaignId = c.CampaignId
                                       INNER JOIN dbo.GDT_AccountRelation ar ON ar.AccountId = ag.AccountId
									   INNER JOIN dbo.GDT_Demand d ON ar.UserId = d.CreateUserId AND d.DemandBillNo = {demandBillNo}
                                       WHERE ag.SystemStatus = 87001
                                       AND (dr.DemandBillNo IS NULL OR dr.DemandBillNo = {demandBillNo});";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToList<ADGroupDTO>(dt);
        }

        public bool CheckDemandCanStop(int demandBillNo)
        {//有效的 系统状态
            string sql = $@"SELECT COUNT(1) FROM dbo.GDT_DemandRelation dr
		                              LEFT JOIN dbo.GDT_AdGroup ag ON dr.AdgroupId = ag.AdgroupId 
                                      WHERE dr.DemandBillNo = {demandBillNo} AND ag.ConfiguredStatus = 86001";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return Convert.ToInt32(obj) == 0;
        }

        public bool RelateToADGroupList(int demandBillNo, List<int> adGroupList, int userID)
        {
            string sql = $"DELETE FROM dbo.GDT_DemandRelation WHERE DemandBillNo = {demandBillNo}";
            int rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            adGroupList.ForEach(adGroup =>
            {
                sql = $@"INSERT INTO dbo.GDT_DemandRelation( DemandBillNo ,AdgroupId ,CreateUserId ,CreateTime)
                                VALUES({demandBillNo},{adGroup},{userID},GETDATE())";
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            });
            sql = $"UPDATE dbo.GDT_Demand SET AuditStatus = {(int)Entities.Enum.GDT.DemandAuditStatusEnum.Puting} WHERE DemandBillNo = {demandBillNo}";
            rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            return rowcount > 0;
        }


        public List<MapGetLeftItemDTO> GetDemandADGroupMenu(int userID)
        {
            string sql = @"SELECT d.DemandBillNo,d.Name,ag.AdgroupId,ag.AdgroupName FROM dbo.GDT_DemandRelation dr
		                            INNER JOIN dbo.GDT_Demand d ON dr.DemandBillNo = d.DemandBillNo
		                            INNER JOIN dbo.GDT_AdGroup ag ON dr.AdgroupId = ag.AdgroupId
                                    LEFT JOIN dbo.GDT_RoleUser ru ON d.CreateUserId = ru.UserId
                                    ";
            if (userID != 0)
                sql += $" WHERE ru.AuthToUserId = {userID}";
            List<ADGroupDTO> list = new List<ADGroupDTO>();
            List<MapGetLeftItemDTO> res = new List<MapGetLeftItemDTO>();
            list = DataTableToList<ADGroupDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            if (list != null && list.Count > 0)
            {
                list.GroupBy(i => new { i.DemandBillNo, i.Name }).ToList().ForEach(gb =>
                {
                    MapGetLeftItemDTO demand = new MapGetLeftItemDTO { DemandBillNo = gb.Key.DemandBillNo, Name = gb.Key.Name };
                    if (gb.Count() > 0)
                        gb.ToList().ForEach(a => { demand.ADGroupList.Add(new ADGroupDTO { AdgroupId = a.AdgroupId, AdgroupName = a.AdgroupName }); });
                    res.Add(demand);
                });
            }
            return res;
        }

        public MapGetRightOneResDTO GetAccountInfo(int userID)
        {
            #region 账户信息
            string sql = $@"SELECT ab.FundType,ISNULL(SUM(ab.Balance),0) AS 'Current',ISNULL(SUM(t.Amount),0) AS 'Cost' FROM dbo.GDT_AccountInfo ai
		                              INNER JOIN dbo.GDT_AccountBalance ab ON ai.AccountId = ab.AccountId AND ab.FundType IN (81001,81002,81003,81004) AND ab.FundStatus = 82001
		                              INNER JOIN dbo.GDT_AccountRelation ar ON ai.AccountId = ar.AccountId 
                                      LEFT JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
                                      LEFT JOIN (SELECT AccountId,FundType,SUM(Amount) AS Amount FROM dbo.GDT_StatementsDetailed WHERE TradeType = 84002 AND CONVERT(VARCHAR(10), Date, 23) = '{DateTime.Now.ToString("yyyy-MM-dd")}' GROUP BY AccountId,FundType)t ON ai.AccountId = t.AccountId AND ab.FundType = t.FundType
		                              WHERE ai.SystemStatus = 80001";
            if (userID != 0)
                sql += $" AND ru.AuthToUserId = {userID}";
            sql += $" GROUP BY ab.FundType";
            List<AccountItemDTO> accountList = DataTableToList<AccountItemDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            MapGetRightOneResDTO res = new MapGetRightOneResDTO();
            if (accountList != null && accountList.Count > 0)
            {
                foreach (var item in accountList)
                {
                    switch (item.FundType)
                    {
                        case (int)Entities.Enum.GDT.FundTypeEnum.现金账户:
                            res.XJAccount.Current = item.Current;
                            res.XJAccount.Cost = item.Cost;
                            break;
                        case (int)Entities.Enum.GDT.FundTypeEnum.虚拟金账户:
                            res.XNAccount.Current = item.Current;
                            res.XNAccount.Cost = item.Cost;
                            break;
                        case (int)Entities.Enum.GDT.FundTypeEnum.分成账户:
                            res.FCAccount.Current = item.Current;
                            res.FCAccount.Cost = item.Cost;
                            break;
                        case (int)Entities.Enum.GDT.FundTypeEnum.银证账户:
                            res.YZAccount.Current = item.Current;
                            res.YZAccount.Cost = item.Cost;
                            break;
                    }
                }
            }
            #endregion

            string whereCondition = string.Empty;
            if (userID != 0)
                whereCondition = $"AND ru.AuthToUserId = {userID}";
            #region 需求统计
            sql = $@"SELECT d.AuditStatus,COUNT(1) AS 'Count' FROM dbo.GDT_Demand d
                            LEFT JOIN dbo.GDT_RoleUser ru ON d.CreateUserId = ru.UserId
		                    WHERE d.Status = 0 
                            AND d.AuditStatus IN (89001,89003,89004,89006)
                            {whereCondition}
		                    GROUP BY d.AuditStatus";
            List<StatisticItemDTO> demandList = DataTableToList<StatisticItemDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            if (demandList != null && demandList.Count > 0)
            {
                foreach (var item in demandList)
                {
                    switch (item.AuditStatus)
                    {
                        case (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingAudit:
                            res.DemandStatistic.DSH = item.Count;
                            break;
                        case (int)Entities.Enum.GDT.DemandAuditStatusEnum.PendingPutIn:
                            res.DemandStatistic.DTF = item.Count;
                            break;
                        case (int)Entities.Enum.GDT.DemandAuditStatusEnum.Puting:
                            res.DemandStatistic.TFZ = item.Count;
                            break;
                        case (int)Entities.Enum.GDT.DemandAuditStatusEnum.IsOver:
                            res.DemandStatistic.YJS = item.Count;
                            break;
                    }
                }
            }
            #endregion

            #region 广告统计
            sql = $@"SELECT  ag.SystemStatus AS AuditStatus ,COUNT(1) AS 'Count'
                          FROM    dbo.GDT_AdGroup ag
                          INNER    JOIN dbo.GDT_AccountRelation ar on ag.AccountId = ar.AccountId
                          LEFT    JOIN dbo.GDT_RoleUser ru on ar.UserId = ru.UserId
                          WHERE  ag.SystemStatus IN (87001,87002,87004)
                          {whereCondition}
                          GROUP BY ag.SystemStatus";
            List<StatisticItemDTO> adGroupList = DataTableToList<StatisticItemDTO>(SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0]);
            if (adGroupList != null && adGroupList.Count > 0)
            {
                foreach (var item in adGroupList)
                {
                    switch (item.AuditStatus)
                    {
                        case (int)Entities.Enum.GDT.SystemStatusEnum.待审核:
                            res.ADGroupStatistic.DSH = item.Count;
                            break;
                        case (int)Entities.Enum.GDT.SystemStatusEnum.有效:
                            res.ADGroupStatistic.TFZ = item.Count;
                            break;
                        case (int)Entities.Enum.GDT.SystemStatusEnum.封停:
                            res.ADGroupStatistic.YZT = item.Count;
                            break;
                    }
                }
            }
            #endregion

            return res;
        }

        public MapGetRightTwoResDTO GetStatistic(int demandBillNo, int adGroupID, int userID)
        {
            MapGetRightTwoResDTO res = new MapGetRightTwoResDTO();
            //曝光 点击 平均点击 订单 话单
            string sql = $@"SELECT ISNULL(SUM(hr.Cost),0) AS TotalCost, 
                                      ISNULL(SUM(hr.Impression),0) AS TotalImpression,
                                      ISNULL(SUM(hr.Click),0) AS TotalClick,ISNULL(CAST((SUM(hr.Click)/SUM(hr.Impression)) AS DECIMAL(18,2)),0) AS AvgClickPercent,
                                      ISNULL(SUM(hr.OrderQuantity),0) AS OrderQuantity,
                                      ISNULL(SUM(hr.BillOfQuantities),0) AS BillOfQuantities FROM dbo.GDT_HourlyRrport hr
                                      INNER JOIN dbo.GDT_AccountRelation ar ON hr.AccountId = ar.AccountId
                                      INNER JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
                                      INNER JOIN dbo.GDT_DemandRelation dr ON hr.AdgroupId = dr.AdgroupId
                                      WHERE ({userID} = 0 OR ru.AuthToUserId = {userID})
                                      AND ({demandBillNo} = 0 OR dr.DemandBillNo = {demandBillNo}) 
                                      AND ({adGroupID} = 0 OR dr.AdgroupId = {adGroupID})";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                res.TotalCost = (int)dt.Rows[0]["TotalCost"];
                res.TotalImpression = (int)dt.Rows[0]["TotalImpression"];
                res.TotalClick = (int)dt.Rows[0]["TotalClick"];
                res.AvgClickPercent = (decimal)dt.Rows[0]["AvgClickPercent"];
                res.OrderQuantity = (int)dt.Rows[0]["OrderQuantity"];
                res.BillOfQuantities = (int)dt.Rows[0]["BillOfQuantities"];
            }
            if (adGroupID != 0)
            {//广告详情
                sql = $@"SELECT ag.AdgroupName,ui.UserName AS BelongYY,di.DictName AS AuditStatusName FROM dbo.GDT_AdGroup ag
                                INNER JOIN dbo.GDT_DemandRelation dr ON ag.AdgroupId = dr.AdgroupId
                                INNER JOIN dbo.GDT_AccountRelation ar ON ag.AccountId = ar.AccountId
                                LEFT JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
                                INNER JOIN dbo.UserInfo ui ON ru.AuthToUserId = ui.UserID
                                LEFT JOIN dbo.DictInfo di ON ag.SystemStatus = di.DictId
                                WHERE ag.AdgroupId = {adGroupID} AND ({userID} = 0 OR ru.AuthToUserId = {userID})";
                dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    res.AdgroupName = dt.Rows[0]["AdgroupName"].ToString();
                    res.BelongYY = dt.Rows[0]["BelongYY"].ToString();
                    res.AuditStatusName = dt.Rows[0]["AuditStatusName"].ToString();
                }
            }
            else if (demandBillNo != 0)
            {//广告数量
                sql = $@"SELECT COUNT(1) FROM dbo.GDT_DemandRelation dr
                                INNER JOIN dbo.GDT_AdGroup ag ON dr.AdgroupId = ag.AdgroupId
                                INNER JOIN dbo.GDT_AccountRelation ar ON ag.AccountId = ar.AccountId
                                LEFT JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
                                WHERE dr.DemandBillNo = {demandBillNo} AND ({userID} = 0 OR ru.AuthToUserId = {userID})";
                var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
                res.ADGroupCount = (int)obj;
            }
            return res;
        }

        public List<MapGetRightThreeItemDTO> GetChart(int demandBillNo, int adGroupID, DateTime startDt, DateTime endDt, int dataType, int userID)
        {
            List<MapGetRightThreeItemDTO> res = new List<MapGetRightThreeItemDTO>();
            string sql = $@"SELECT * FROM dbo.GDT_HourlyRrport hr
                                      INNER JOIN dbo.GDT_DemandRelation dr ON hr.AdgroupId = dr.AdgroupId
                                      INNER JOIN dbo.GDT_AccountRelation ar ON hr.AccountId = ar.AccountId
                                      LEFT JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
                                      WHERE ({userID} = 0 OR ru.AuthToUserId = {userID})
                                      AND ({demandBillNo} = 0 OR dr.DemandBillNo = {demandBillNo}) 
                                      AND ({adGroupID} = 0 OR dr.AdgroupId = {adGroupID})
                                      AND hr.Date BETWEEN @StartDate AND @EndDate";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StartDate", startDt),
                new SqlParameter("@EndDate", endDt)
            };
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            var list = DataTableToList<Entities.GDT.GdtHourlyRrport>(dt);
            if (list != null && list.Count > 0)
            {
                list.GroupBy(i => new { i.Date, i.Hour }).OrderBy(gb => gb.Key.Date).ThenBy(gb => gb.Key.Hour).ToList().ForEach(gb =>
                    {
                        var item = new MapGetRightThreeItemDTO { Date = gb.Key.Date, Key = gb.Key.Hour.ToString() };
                        switch (dataType)
                        {//曝光量:1 点击量:2 点击率:3 费用:4 订单量:5 话单量:6
                        case 1:
                                item.Value = gb.Sum(i => i.Impression);
                                break;
                            case 2:
                                item.Value = gb.Sum(i => i.Click);
                                break;
                            case 3:
                                item.Value = Math.Round((decimal)gb.Sum(i => i.Impression) / gb.Sum(i => i.Click), 2);
                                break;
                            case 4:
                                item.Value = gb.Sum(i => i.Cost);
                                break;
                            case 5:
                                item.Value = gb.Sum(i => i.OrderQuantity);
                                break;
                            case 6:
                                item.Value = gb.Sum(i => i.BillOfQuantities);
                                break;
                        }
                        res.Add(item);
                    });
            }
            return res;
        }

        public MapGetRightADListResDTO GetADGroupRankList(int demandBillNo, int userID, string orderByStr, int pageIndex, int pageSize)
        {
            MapGetRightADListResDTO res = new MapGetRightADListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DemandBillNo", demandBillNo),
                new SqlParameter("@OrderByStr", orderByStr),
                new SqlParameter("@YYUserID", userID),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters.Last().Direction = ParameterDirection.Output;
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetADGroupRankList", parameters).Tables[0];
            res.List = DataTableToList<ADGroupDTO>(dt);
            res.TotalCount = (int)parameters.Last().Value;
            return res;
        }

        public MapGetRightADListResDTO GetADGroupDetailList(int demandBillNo, int adGroupID, string startDt, string endDt, int userID, int pageIndex, int pageSize)
        {
            MapGetRightADListResDTO res = new MapGetRightADListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DemandBillNo", demandBillNo),
                new SqlParameter("@ADGroupID", adGroupID),
                new SqlParameter("@StartDate", startDt),
                new SqlParameter("@EndDate", endDt),
                new SqlParameter("@YYUserID", userID),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters.Last().Direction = ParameterDirection.Output;
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetADGroupHourList", parameters).Tables[0];
            res.List = DataTableToList<ADGroupDTO>(dt);
            res.TotalCount = (int)parameters.Last().Value;
            if (res.List != null && res.List.Count > 0)
            {
                res.List.ForEach(item => { item.AdgroupDate = item.Date.ToString("yyyy-MM-dd") + $" {item.Hour-1}-{item.Hour}时"; });
            }
            return res;
        }

        public List<ADGroupDTO> GetAllADGroupDetailList(int demandBillNo, int adGroupID, string startDt, string endDt, int userID)
        {
            string sql = $@"SELECT hr.Date ,
		                                           hr.Hour ,
		                                           SUM(hr.Impression) AS TotalImpression ,
		                                           SUM(hr.Click) AS TotalClick ,
		                                           CAST(( SUM(hr.Click) / SUM(hr.Impression) ) AS DECIMAL(18, 2)) AS AvgClickPercent ,
		                                           SUM(hr.Cost) AS TotalCost ,
		                                           SUM(hr.OrderQuantity) AS OrderQuantity ,
		                                           SUM(hr.BillOfQuantities) AS BillOfQuantities
	                                  FROM   dbo.GDT_HourlyRrport hr
	                                  INNER JOIN dbo.GDT_DemandRelation dr ON hr.AdgroupId = dr.AdgroupId
	                                  INNER JOIN dbo.GDT_AccountRelation ar ON hr.AccountId = ar.AccountId
	                                  LEFT JOIN dbo.GDT_RoleUser ru ON ar.UserId = ru.UserId
	                                  WHERE ({userID} = 0 OR ru.AuthToUserId = {userID})
                                      AND     ({demandBillNo} = 0 OR dr.DemandBillNo = {demandBillNo})
                                      AND     ({adGroupID} = 0 OR dr.AdgroupId = {adGroupID})";
            if (!string.IsNullOrEmpty(startDt) || !string.IsNullOrEmpty(endDt))
                sql += $" AND hr.Date BETWEEN '{startDt}' AND '{endDt}'";
            sql += " GROUP BY hr.Date ,hr.Hour ORDER BY hr.Date DESC, hr.Hour DESC";
            List<ADGroupDTO> list = new List<ADGroupDTO>();
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            list = DataTableToList<ADGroupDTO>(dt);
            if (list != null && list.Count > 0)
            {
                list.ForEach(item => { item.AdgroupDate = item.Date.ToString("yyyy-MM-dd") + $" {item.Hour - 1}-{item.Hour}时"; });
            }
            return list;
        }

        public int GetOrganizeldByDemandBillNo(int demandBillNo)
        {
            string sql = $@"SELECT uo.OrganizeId FROM dbo.GDT_Demand d INNER JOIN dbo.GDT_UserOrganize uo ON d.CreateUserId = uo.UserId 
                                      WHERE d.DemandBillNo = {demandBillNo}";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : (int)obj;
        }

        public bool CheckDemandHasReCharge(int demandBillNo)
        {
            string sql = $"SELECT COUNT(1) FROM dbo.GDT_RechargeRelation WHERE DemandBillNo = {demandBillNo} AND  RechargeStatus = 90002";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            int res = obj == null ? 0 : (int)obj;
            return res > 0;
        }
    }
}
 