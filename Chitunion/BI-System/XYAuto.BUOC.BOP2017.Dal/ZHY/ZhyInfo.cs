using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Dto;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.ZHY
{
    public class ZhyInfo : DataBase
    {
        public static readonly ZhyInfo Instance = new ZhyInfo();

        /// <summary>
        /// zlb 2017-08-18
        /// 根查询广告主/子客列表
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="UserId">运营ID或0（0则查全部）</param>
        /// <returns></returns>
        public DataTable SelectZhyAdvertiserList(string Mobile, int PageIndex, int PageSize, int UserId)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Mobile", SqlDbType.VarChar,20),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = Mobile;
            parameters[1].Value = PageIndex;
            parameters[2].Value = PageSize;
            parameters[3].Value = UserId;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectZhyAdvertiserList", parameters);
            int totalCount = (int)(parameters[4].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 统计广告主数据
        /// </summary>
        /// <param name="Mobile">手机号</param>
        /// <param name="UserId">运营ID或0（0则查全部）</param>
        /// <returns></returns>
        public DataTable ZhyAdvertiserStatistics(string Mobile, int UserId)
        {
            string strSql = @"SELECT SUM(AB1.Balance) AS TotalCashBalance,SUM(AB2.Balance) AS TotalVirtualBalance,SUM(AB3.Balance) AS TotalDividedBalance,SUM(AB4.Balance) AS TotalSilverCBalance,
                            SUM(ABTotal.RealtimeCost)  AS TotalTodaySpend, COUNT(D.CreateUserId)  AS TotalNumber
                            FROM  Chitunion2017.dbo.UserInfo U  LEFT JOIN  GDT_AccountRelation  AR ON U.UserID = AR.UserId
                            LEFT JOIN GDT_AccountInfo AI ON AR.AccountId = AI.AccountId
                            LEFT JOIN GDT_AccountBalance AB1  ON AR.AccountId = AB1.AccountId AND AB1.FundType = 81001
                            LEFT JOIN GDT_AccountBalance AB2  ON AR.AccountId = AB2.AccountId AND AB2.FundType = 81002
                            LEFT JOIN GDT_AccountBalance AB3  ON AR.AccountId = AB3.AccountId AND AB3.FundType = 81003
                            LEFT JOIN GDT_AccountBalance AB4  ON AR.AccountId = AB4.AccountId AND AB4.FundType = 81004
							LEFT JOIN GDT_AccountBalance ABTotal  ON AR.AccountId = ABTotal.AccountId
                            LEFT JOIN GDT_StatementDaily SD ON SD.AccountId = AR.AccountId and SD.TradeType = 84002 AND CONVERT(varchar(100), SD.Date, 23) = CONVERT(varchar(100), GETDATE(), 23)
                            LEFT JOIN GDT_Demand D ON  D.CreateUserId = U.UserID  AND AuditStatus > 89002
                            LEFT JOIN GDT_RoleUser RU ON RU.UserId=U.UserID
                            WHERE U.Source = 3004";
            if (!string.IsNullOrEmpty(Mobile))
            {
                strSql += " and U.Mobile like '%" + SqlFilter(Mobile) + "%'";
            }
            if (UserId > 0)
            {
                strSql += " and RU.AuthToUserId=" + UserId;
            }
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询广告运营角色列表
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Mobile">手机号</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectZhyOperaterList(string UserName, string Mobile, int PageIndex, int PageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Mobile",SqlDbType.VarChar,20),
                    new SqlParameter("@PageIndex",SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = Mobile;
            parameters[1].Value = PageIndex;
            parameters[2].Value = PageSize;
            parameters[3].Value = UserName;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectZhyOperaterList", parameters);
            int totalCount = (int)(parameters[4].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询未绑定子客广告运营用户
        /// </summary>
        /// <returns></returns>
        public DataSet SelectGdtUserAndOperaterList()
        {
            string strSql = @"SELECT AccountId,CorporationName FROM GDT_AccountInfo WHERE SystemStatus=80001 AND AccountId NOT IN (SELECT AccountId FROM GDT_AccountRelation) ";
            strSql += " SELECT U.UserID,U.UserName,U.Mobile FROM Chitunion2017.dbo.UserInfo U INNER JOIN  Chitunion2017.dbo.UserRole R ON U.UserID=R.UserID WHERE R.RoleID='SYS005RL00021'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询未绑定运营的广告主信息集合
        /// </summary>
        /// <returns></returns>
        public DataTable SelectFreeAdvertiserList()
        {
            string strSql = @"SELECT U.UserID,U.UserName,U.Mobile FROM Chitunion2017.dbo.UserInfo U WHERE Source=3004 AND  U.UserID NOT IN (SELECT UserId FROM  GDT_RoleUser)";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// zlb 2017-08-22
        ///查询广告运营所管理的广告主
        /// </summary>
        /// <param name="OperaterId">广告运营ID</param>
        /// <returns></returns>
        public DataTable SelectOptAdvertiserList(int OperaterId)
        {
            string strSql = $"SELECT U.UserID,U.UserName,U.Mobile FROM Chitunion2017.dbo.UserInfo U WHERE Source = 3004 AND U.UserID IN (SELECT UserId FROM  GDT_RoleUser WHERE AuthToUserId = {OperaterId})";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// zlb 2017-08-18
        //子客和广告主绑定
        /// </summary>
        /// <param name="ReqDto"></param>
        /// <param name="CreateUserId">创建人ID</param>
        /// <returns></returns>
        public int BingdingOptIdAndAccIdToAdvId(BindingIdReqDTO ReqDto, int CreateUserId)
        {
            string strSql = $" DELETE FROM  GDT_AccountRelation WHERE UserId={ReqDto.AdvertiserId}";
            strSql += $" INSERT INTO dbo.GDT_AccountRelation(UserId,AccountId,CreateUserId,CreateTime) VALUES ({ReqDto.AdvertiserId},{ReqDto.AccountId},{CreateUserId},GETDATE())";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        ///// <summary>
        ///// zlb 2017-08-18
        ///// 子客和广告主绑定
        ///// </summary>
        ///// <param name="ReqDto"></param>
        ///// <param name="CreateUserId">创建人ID</param>
        ///// <returns></returns>
        //public int BingdingAccountIdToAdvId(BindingIdReqDTO ReqDto, int CreateUserId)
        //{
        //    string strSql = $"INSERT INTO dbo.GDT_AccountRelation(UserId,AccountId,CreateUserId,CreateTime) VALUES ({ReqDto.AdvertiserId},{ReqDto.AccountId},{CreateUserId},GETDATE())";
        //    return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        //}
        /// <summary>
        /// zlb 2017-08-18
        /// 查询广告主是否被绑定
        /// </summary>
        /// <param name="AdvertiserId">广告ID</param>
        /// <returns></returns>
        public int SelectBingdingAdvCount(int AdvertiserId)
        {
            string strSql = "SELECT COUNT(1) FROM GDT_AccountRelation WHERE UserId=" + AdvertiserId;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询子客是否被绑定
        /// </summary>
        /// <param name="AccountId">子客ID</param>
        /// <returns></returns>
        public int SelectBingdingAccountCount(int AccountId)
        {
            string strSql = "SELECT COUNT(1) FROM GDT_AccountRelation WHERE AccountId=" + AccountId;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 查询广告主是否被绑定
        /// </summary>
        /// <param name="OperaterId">运营ID</param>
        /// <param name="AdvertiserIds">广告ID集合</param>
        /// <returns></returns>
        public int SelectBingdingAdvCount(int OperaterId, string AdvertiserIds)
        {
            string strSql = $"SELECT COUNT(1) FROM GDT_RoleUser WHERE AuthToUserId!={OperaterId} and UserId in (" + AdvertiserIds + ")";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 运营批量绑定广告主
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <param name="CreateUserId">创建人ID</param>
        /// <returns></returns>
        public int BingdingOptIdToAdvsId(AdvIdToOptIdReqDTO ReqDTO, int CreateUserId, string AdvertiserIds)
        {
            StringBuilder sb = new StringBuilder();
            if (ReqDTO.OperateType == 1)
            {
                sb.Append($" DELETE FROM GDT_RoleUser WHERE UserId in ({ AdvertiserIds})");
            }
            else
            {
                sb.Append($" DELETE FROM GDT_RoleUser WHERE AuthToUserId={ReqDTO.OperaterId}");
            }
            string strSql = sb.ToString();
            if (AdvertiserIds != "")
            {
                sb.Append(" INSERT INTO dbo.GDT_RoleUser(UserId,AuthToUserId,CreateUserId,CreateTime) VALUES ");
                DateTime dtNow = DateTime.Now;
                foreach (var item in ReqDTO.AdvertiserIds)
                {
                    sb.AppendFormat("({0},{1},{2},'{3}'),", item, ReqDTO.OperaterId, CreateUserId, dtNow);
                }
                strSql = sb.ToString();
                strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// <summary>
        /// zlb 2017-08-18
        /// 解除广告主与第三方广告主的绑定
        /// </summary>
        /// <param name="AdvertiserId">第三方广告主ID (广点通广告主ID或者今日头条广告主ID)</param>
        /// <returns></returns>
        public int DeleteAdvsiterByAdvId(int AdvertiserId)
        {
            string strSql = "DELETE FROM GDT_AccountRelation WHERE AccountId=" + AdvertiserId;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询流水明细
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="GdtNum"></param>
        /// <param name="AccountType"></param>
        /// <param name="TradeType"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectGdtFlowDetail(string UserName, string GdtNum, int AccountType, int TradeType, string StarDate, string EndDate, int UserId, int PageIndex, int PageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@GdtNum", SqlDbType.VarChar,100),
                    new SqlParameter("@AccountType", SqlDbType.Int),
                    new SqlParameter("@TradeType", SqlDbType.Int),
                    new SqlParameter("@StarDate", SqlDbType.VarChar,20),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,20),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@UserId", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = UserName;
            parameters[1].Value = GdtNum;
            parameters[2].Value = AccountType;
            parameters[3].Value = TradeType;
            parameters[4].Value = StarDate;
            parameters[5].Value = EndDate;
            parameters[6].Value = PageIndex;
            parameters[7].Value = PageSize;
            parameters[8].Value = UserId;
            parameters[9].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectGdtFlowDetail", parameters);
            int totalCount = (int)(parameters[9].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询日结汇总
        /// </summary>
        /// <param name="AccountType"></param>
        /// <param name="TradeType"></param>
        /// <param name="StarDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="UserId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectGdtDateSummaryInfo(int AccountType, int TradeType, string StarDate, string EndDate, int UserId, int PageIndex, int PageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@AccountType", SqlDbType.Int),
                    new SqlParameter("@TradeType", SqlDbType.Int),
                    new SqlParameter("@StarDate", SqlDbType.VarChar,20),
                    new SqlParameter("@EndDate", SqlDbType.VarChar,20),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@UserId", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = AccountType;
            parameters[1].Value = TradeType;
            parameters[2].Value = StarDate;
            parameters[3].Value = EndDate;
            parameters[4].Value = PageIndex;
            parameters[5].Value = PageSize;
            parameters[6].Value = UserId;
            parameters[7].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectGdtDateSummaryInfo", parameters);
            int totalCount = (int)(parameters[7].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询充值列表
        /// </summary>
        /// <param name="RechargeNumber"></param>
        /// <param name="DemandBillNo"></param>
        /// <param name="ExternalBillNo"></param>
        /// <param name="UserName"></param>
        /// <param name="RechargeStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectGdtRechargeInfo(string RechargeNumber, string DemandBillNo, string ExternalBillNo, string UserName, int RechargeStatus, int PageIndex, int PageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RechargeNumber", SqlDbType.VarChar,200),
                    new SqlParameter("@DemandBillNo", SqlDbType.VarChar,100),
                    new SqlParameter("@ExternalBillNo", SqlDbType.VarChar,200),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@RechargeStatus", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = RechargeNumber;
            parameters[1].Value = DemandBillNo;
            parameters[2].Value = ExternalBillNo;
            parameters[3].Value = UserName;
            parameters[4].Value = RechargeStatus;
            parameters[5].Value = PageIndex;
            parameters[6].Value = PageSize;
            parameters[7].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectGdtRechargeInfo", parameters);
            int totalCount = (int)(parameters[7].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询未关联的GDT流水单列表
        /// </summary>
        /// <param name="TradeType">84001:充值，84003回划 </param>
        /// <returns></returns>
        public DataTable SelectGdtInfo(int TradeType, int DemandBillNo)
        {
            string strSql = $@"SELECT ExternalBillNo AS GdtNum,DictName as AccountType,Amount,Date AS OperateTime  FROM GDT_StatementsDetailed SD LEFT JOIN dbo.DictInfo ON DictId=SD.FundType
                              WHERE SD.AccountId IN (SELECT A.AccountId FROM GDT_Demand D INNER JOIN GDT_AccountRelation A ON D.CreateUserId=A.UserId WHERE D.DemandBillNo={DemandBillNo}) AND TradeType={TradeType}  AND ExternalBillNo NOT IN (SELECT ExternalBillNo FROM GDT_RechargeStatementRelation WHERE AccountType={TradeType})";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询充值对账或资金回划是否已被绑定
        /// </summary>
        /// <param name="ExternalBillNos">流水单号</param>
        /// <returns></returns>
        public int SelectBingdingTradeCount(string ExternalBillNos)
        {
            string strSql = $"SELECT COUNT(1) FROM GDT_RechargeStatementRelation WHERE  ExternalBillNo in (" + ExternalBillNos + ")";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        ///  zlb 2017-08-21
        /// 查询充值对账或资金回划是否已被绑定
        /// </summary>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="AccountType">账号类型</param>
        /// <returns></returns>
        public int SelectBingdingTradeCount(int DemandBillNo, int AccountType)
        {
            string strSql = $"SELECT COUNT(1) FROM GDT_RechargeStatementRelation WHERE AccountType={AccountType} AND DemandBillNo={DemandBillNo}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        ///  zlb 2017-08-21
        ///  充值对账或资金回划绑定GDT流水单
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <param name="CreateUserId">创建人id</param>
        /// <returns></returns>
        public int BingdingTradeRelation(GdtTradeReqDTO ReqDTO, int CreateUserId)
        {
            StringBuilder sb = new StringBuilder();
            if (ReqDTO.TradeType == 84001)
            {
                sb.Append(" UPDATE GDT_RechargeRelation SET RechargeStatus=90002 WHERE DemandBillNo=" + ReqDTO.DemandBillNo);
            }
            sb.Append(" INSERT INTO dbo.GDT_RechargeStatementRelation(DemandBillNo,AccountType,ExternalBillNo,CreateUserId,CreateTime) VALUES ");
            DateTime dtNow = DateTime.Now;
            foreach (var item in ReqDTO.GdtNumList)
            {
                sb.AppendFormat("({0},{1},'{2}',{3},'{4}'),", ReqDTO.DemandBillNo, ReqDTO.TradeType, item, CreateUserId, dtNow);
            }
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        public decimal SelectZhyRechargeAmount(int DemandBillNo)
        {
            string strSql = $"SELECT top 1 Amount FROM GDT_RechargeRelation WHERE RechargeStatus=90002 and  DemandBillNo={DemandBillNo}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }

        public decimal SelectZhyConsumeAmount(int DemandBillNo)
        {
            string strSql = $"SELECT SUM(Cost)FROM  GDT_DemandRelation  DGR INNER JOIN GDT_HourlyRrport GA ON  DGR.AdgroupId = GA.AdgroupId WHERE DGR.DemandBillNo ={ DemandBillNo}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj == null || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(obj);
            }
        }

        public decimal SelectGdtRechargeAmount(string ExternalBillNos)
        {
            string strSql = $"SELECT SUM(Amount) FROM GDT_StatementsDetailed WHERE  ExternalBillNo in (" + ExternalBillNos + ")";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }

        /// <summary>
        /// zlb 2017-08-21
        /// 查询资金回划列表
        /// </summary>
        /// <param name="RechargeNumber"></param>
        /// <param name="DemandBillNo"></param>
        /// <param name="ExternalBillNo"></param>
        /// <param name="UserName"></param>
        /// <param name="HandleStatus"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectBackAmountInfo(string RechargeNumber, string DemandBillNo, string ExternalBillNo, string UserName, int HandleStatus, int PageIndex, int PageSize)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RechargeNumber", SqlDbType.VarChar,200),
                    new SqlParameter("@DemandBillNo", SqlDbType.VarChar,100),
                    new SqlParameter("@ExternalBillNo", SqlDbType.VarChar,200),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@HandleStatus", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = RechargeNumber;
            parameters[1].Value = DemandBillNo;
            parameters[2].Value = ExternalBillNo;
            parameters[3].Value = UserName;
            parameters[4].Value = HandleStatus;
            parameters[5].Value = PageIndex;
            parameters[6].Value = PageSize;
            parameters[7].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectBackAmountInfo", parameters);
            int totalCount = (int)(parameters[7].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// zlb 2017-08-24
        /// 查询需求个数
        /// </summary>
        /// <param name="AdvertiserId">第三方广告主ID (广点通广告主ID或者今日头条广告主ID)</param>
        /// <returns></returns>
        public int SelectDemandCount(int AdvertiserId)
        {
            string strSql = $"SELECT COUNT(1) FROM GDT_Demand WHERE Status=0 and (AuditStatus=89006 or AuditStatus=89004) and CreateUserId =(SELECT  TOP 1 UserId FROM dbo.GDT_AccountRelation WHERE AccountId={AdvertiserId})";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-24
        /// 查询需求个数
        /// </summary>
        /// <param name="AdvertiserId">智慧云的广告主ID</param>
        /// <returns></returns>
        public int SelectDemandCountByZhyId(int AdvertiserId)
        {
            string strSql = $"SELECT COUNT(1) FROM GDT_Demand WHERE Status=0 and (AuditStatus=89006 or AuditStatus=89004) and CreateUserId ={AdvertiserId}";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-08-24
        /// 查询智慧云会员ID
        /// </summary>
        /// <param name="DemandNo"></param>
        /// <returns></returns>
        public int SelectOrganizeId(int DemandBillNo)
        {
            string strSql = "SELECT TOP 1 OrganizeId FROM GDT_Demand D INNER JOIN  GDT_UserOrganize  U ON D.CreateUserId=U.UserId WHERE D.DemandBillNo=" + DemandBillNo;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// zlb 2017-08-24
        /// 查询广点通流水单金额
        /// </summary>
        /// <param name="ExternalBillNos"></param>
        /// <returns></returns>
        public DataTable SelectZhyTradeAmount(string ExternalBillNos)
        {
            string strSql = $"SELECT ExternalBillNo,Amount FROM GDT_StatementsDetailed WHERE ExternalBillNo IN({ExternalBillNos})";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-09-30
        /// 查询广告主客户详情
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataTable SelectZhyAdvertiserInfo(int UserId)
        {
            string strSql = $@"SELECT U.UserID AS UserId,U.UserName,U.Mobile,AR.AccountId,AI.CorporationName AS CustomerName,AB1.Balance AS CashBalance,AB2.Balance AS VirtualBalance,AB3.Balance AS DividedBalance,AB4.Balance AS SilverCardBalance,
                              (SELECT SUM(RealtimeCost) FROM GDT_AccountBalance WHERE AccountId = AR.AccountId  and CONVERT(VARCHAR(10),  PullTime, 120)=CONVERT(VARCHAR(10),  GETDATE(), 120)) AS TodaySpend,
                              (SELECT COUNT(1) FROM GDT_Demand D WHERE D.CreateUserId = U.UserID  AND AuditStatus > 89002) AS Number, U1.UserName AS Operater,UD.TrueName AS CompanyName
                              FROM  Chitunion2017.dbo.UserInfo U  LEFT JOIN  GDT_AccountRelation AR ON U.UserID = AR.UserId
                              LEFT JOIN GDT_AccountInfo AI ON AR.AccountId = AI.AccountId
                              LEFT JOIN GDT_AccountBalance AB1  ON AR.AccountId = AB1.AccountId AND AB1.FundType = 81001
                              LEFT JOIN GDT_AccountBalance AB2  ON AR.AccountId = AB2.AccountId AND AB2.FundType = 81002
                              LEFT JOIN GDT_AccountBalance AB3  ON AR.AccountId = AB3.AccountId AND AB3.FundType = 81003
                              LEFT JOIN GDT_AccountBalance AB4  ON AR.AccountId = AB4.AccountId AND AB4.FundType = 81004
                              LEFT JOIN GDT_RoleUser RU ON RU.UserId = U.UserID
                              LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON U1.UserID = RU.AuthToUserId
                              LEFT JOIN Chitunion2017.dbo.UserDetailInfo UD ON U.UserID = UD.UserID WHERE U.Source = 3004 AND U.UserID={UserId}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 查询需求充值列表
        /// </summary>
        /// <param name="RechargeNumber">充值单号</param>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="StartTime">充值请求开始时间</param>
        /// <param name="EndTime">充值请求结束时间</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectDemandBillRecharge(string RechargeNumber, string DemandBillNo, string UserName, string StartTime, string EndTime, int PageIndex, int PageSize,int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RechargeNumber", SqlDbType.VarChar,200),
                    new SqlParameter("@DemandBillNo", SqlDbType.VarChar,100),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@StartTime", SqlDbType.VarChar,18),
                    new SqlParameter("@EndTime", SqlDbType.VarChar,18),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = RechargeNumber;
            parameters[1].Value = DemandBillNo;
            parameters[2].Value = UserName;
            parameters[3].Value = StartTime;
            parameters[4].Value = EndTime;
            parameters[5].Value = PageIndex;
            parameters[6].Value = PageSize;
            parameters[7].Value = UserID;
            parameters[8].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectDemandBillRecharge", parameters);
            int totalCount = (int)(parameters[8].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-09-30
        /// 查询异常数据列表
        /// </summary>
        /// <param name="RechargeNumber">充值单号</param>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="DemandBillName">需求名称</param>
        /// <param name="ExceptionType">异常类型（0全部 1充值异常 2回划异常）</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectExceptionDataList(string RechargeNumber, string DemandBillNo, string DemandBillName, int ExceptionType, int PageIndex, int PageSize)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@RechargeNumber", SqlDbType.VarChar,200),
                    new SqlParameter("@DemandBillNo", SqlDbType.VarChar,100),
                    new SqlParameter("@DemandBillName", SqlDbType.VarChar,500),
                    new SqlParameter("@ExceptionType", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = RechargeNumber;
            parameters[1].Value = DemandBillNo;
            parameters[2].Value = DemandBillName;
            parameters[3].Value = ExceptionType;
            parameters[4].Value = PageIndex;
            parameters[5].Value = PageSize;
            parameters[6].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectExceptionDataList", parameters);
            int totalCount = (int)(parameters[6].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-09-30
        /// 查询需求花费记录
        /// </summary>
        /// <param name="DemandBillNo">需求单号</param>
        /// <param name="DemandBillName">需求名称</param>
        /// <param name="StartTime">需求创建开始时间</param>
        /// <param name="EndTime">需求结束开始时间</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable SelectDemandSpendList(string DemandBillNo, string DemandBillName, string StartTime, string EndTime, int PageIndex, int PageSize,int UserId)
        {

            SqlParameter[] parameters = {
                    new SqlParameter("@DemandBillNo", SqlDbType.VarChar,100),
                    new SqlParameter("@DemandBillName", SqlDbType.VarChar,500),
                    new SqlParameter("@StartTime", SqlDbType.VarChar,18),
                    new SqlParameter("@EndTime", SqlDbType.VarChar,18),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = DemandBillNo;
            parameters[1].Value = DemandBillName;
            parameters[2].Value = StartTime;
            parameters[3].Value = EndTime;
            parameters[4].Value = PageIndex;
            parameters[5].Value = PageSize;
            parameters[6].Value = UserId;
            parameters[7].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectDemandSpendList", parameters);
            int totalCount = (int)(parameters[7].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


    }
}