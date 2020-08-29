using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //提现明细
    public partial class LeWithdrawalsDetail : DataBase
    {


        public static readonly LeWithdrawalsDetail Instance = new LeWithdrawalsDetail();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeWithdrawalsDetail entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_WithdrawalsDetail(");
            strSql.Append("WithdrawalsPrice,IndividualTaxPeice,PracticalPrice,PayeeAccount,Status,ApplicationDate,PayDate,OrderID,PayeeID,Reason,CreateTime");

            strSql.Append(",AuditStatus,ApplySource,SyncPayStatus");
            strSql.Append(") values (");
            strSql.Append("@WithdrawalsPrice,@IndividualTaxPeice,@PracticalPrice,@PayeeAccount,@Status,@ApplicationDate,@PayDate,@OrderID,@PayeeID,@Reason,@CreateTime");
            strSql.Append(",@AuditStatus,@ApplySource,@SyncPayStatus");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@WithdrawalsPrice",entity.WithdrawalsPrice),
                        new SqlParameter("@IndividualTaxPeice",entity.IndividualTaxPeice),
                        new SqlParameter("@PracticalPrice",entity.PracticalPrice),
                        new SqlParameter("@PayeeAccount",entity.PayeeAccount),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@ApplicationDate",entity.ApplicationDate),
                        new SqlParameter("@PayDate",null),
                        new SqlParameter("@OrderID",entity.OrderID),
                        new SqlParameter("@PayeeID",entity.PayeeID),
                        new SqlParameter("@Reason",entity.Reason),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@AuditStatus",entity.AuditStatus),
                        new SqlParameter("@ApplySource",entity.ApplySource),

                        new SqlParameter("@SyncPayStatus",entity.SyncPayStatus),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);


        }

        public bool IsExist(int userId)
        {
            var sql = $@"
				  SELECT COUNT(*) FROM dbo.LE_WithdrawalsDetail
				  WHERE  IsActive = 1 AND PayeeID = {userId} AND DATEDIFF(DAY,ApplicationDate,'{DateTime.Now}') = 0
                ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj != null && Convert.ToInt32(obj) > 0;
        }

        public decimal GetTotalAmount(string sqlWhere)
        {
            var sql = $@"

                   SELECT  SUM(WD.WithdrawalsPrice) AS WithdrawalsPrice
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                    WHERE   IsActive = 1
                    {sqlWhere}
                    ";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }
        public decimal GetTotalAmountByAdmin(string sqlWhere)
        {
            var sql = $@"

                   SELECT  SUM(WD.WithdrawalsPrice) AS WithdrawalsPrice
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = WD.PayeeID
                    WHERE   IsActive = 1
                    {sqlWhere}
                    ";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }


        /// <summary>
        /// 获取当前用户再当月的累计提现收入（支付中，已支付）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public Entities.LETask.LeWithdrawalsDetail GetMonthOfWithdrawalsMoney(int userId, string startDay, string endDay)
        {
            var sql = $@"
                    SELECT  SUM(WD.WithdrawalsPrice) AS WithdrawalsPrice ,
                            SUM(WD.IndividualTaxPeice) AS IndividualTaxPeice
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                    WHERE   WD.IsActive = 1 AND WD.PayeeID = {userId}
                            AND WD.ApplicationDate >= '{startDay}'
                            AND WD.ApplicationDate < '{endDay}'
                            AND WD.Status IN ( {(int)WithdrawalsStatusEnum.支付中}, {(int)WithdrawalsStatusEnum.已支付} )
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWithdrawalsDetail>(obj.Tables[0]);
        }


        public Entities.LETask.LeWithdrawalsDetail GetInfo(int recId)
        {
            var sql = $@"
                    SELECT  WD.RecID ,
                            WD.WithdrawalsPrice ,
                            WD.IndividualTaxPeice ,
                            WD.PracticalPrice ,
                            WD.PayeeAccount ,
                            WD.Status ,
                            WD.ApplicationDate ,
                            WD.PayDate ,
                            WD.OrderID ,
                            WD.PayeeID ,
                            WD.Reason ,
                            WD.IsLock ,
                            ISNULL(AuditStatus,0) AS AuditStatus ,
                            WD.CreateTime,
		                    UI.SysName AS TrueName ,
		                    UI.UserName,
		                    DC.DictName AS UserTypeName ,
                            DC1.DictName AS PayStatusName
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = WD.PayeeID
		                    LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = UI.Type
                            LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = UI.Status
                    WHERE   WD.RecID = {recId}
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWithdrawalsDetail>(obj.Tables[0]);
        }


        public Entities.LETask.LeWithdrawalsDetail GetAuditDetail(int withdrawalsId)
        {
            var sql = $@"

                     SELECT WD.RecID ,
                            WD.WithdrawalsPrice ,
                            WD.IndividualTaxPeice ,
                            WD.PracticalPrice ,
                            WD.PayeeAccount ,
                            WD.Status ,
                            WD.ApplicationDate ,
                            WD.PayDate ,
                            WD.OrderID ,
                            WD.PayeeID ,
                            WD.Reason ,
                            WD.AuditStatus ,
                            WD.CreateTime ,
                            WD.IsLock ,
                            UI.SysName AS TrueName ,
                            UI.UserName ,
                            DC.DictName AS UserTypeName ,
                            WS.AccumulatedIncome ,
                            WS.HaveWithdrawals
                     FROM   dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.LE_WithdrawalsStatistics AS WS WITH ( NOLOCK ) ON WS.UserID = WD.PayeeID
                            LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = WD.PayeeID
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = UI.Type
                     WHERE  WD.RecID = { withdrawalsId };
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWithdrawalsDetail>(obj.Tables[0]);
        }

        public Tuple<Entities.LETask.LeWithdrawalsDetail, Entities.LETask.AuditInfo> GetInfoByAuditInfo(int recId)
        {
            var sql = $@"
                    SELECT  WD.RecID ,
                            WD.WithdrawalsPrice ,
                            WD.IndividualTaxPeice ,
                            WD.PracticalPrice ,
                            WD.PayeeAccount ,
                            WD.Status ,
                            WD.ApplicationDate ,
                            WD.PayDate ,
                            WD.OrderID ,
                            WD.PayeeID ,
                            WD.Reason ,
                            WD.AuditStatus ,
                            WD.IsLock,
                            WD.CreateTime,
		                    UI.SysName AS TrueName ,
		                    UI.UserName,
		                    DC.DictName AS UserTypeName ,
                            DC1.DictName AS PayStatusName
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = WD.PayeeID
		                    LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = UI.Type
                            LEFT JOIN DBO.DictInfo AS DC1 WITH(NOLOCK) ON DC1.DictId = WD.Status
                    WHERE   WD.RecID = {recId}
                    ";

            string sqlAudit = $@"
                    SELECT  AI.Id ,
                            AI.RelationType ,
                            AI.RelationId ,
                            AI.AuditStatus ,
                            AI.RejectMsg ,
                            AI.CreateTime ,
                            AI.CreateUserId ,
							UI.SysName AS CreateUserName ,
							DC.DictName AS AuditStatusName
                    FROM    dbo.AuditInfo AS AI WITH ( NOLOCK )
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AI.AuditStatus
							LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = AI.CreateUserId
                    WHERE   AI.RelationType = {(int)AuditTypeEnum.提现审核}
                            AND AI.RelationId = {recId}
";
            sql += sqlAudit;
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<Entities.LETask.LeWithdrawalsDetail, Entities.LETask.AuditInfo>(
                DataTableToEntity<Entities.LETask.LeWithdrawalsDetail>(obj.Tables[0]),
                DataTableToEntity<Entities.LETask.AuditInfo>(obj.Tables[1]));
        }

        /// <summary>
        /// 修改提现状态
        /// </summary>
        /// <param name="recId"></param>
        /// <param name="reason"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateWithdrawalsStatus(int recId, string reason, WithdrawalsStatusEnum status)
        {
            var sql = $@"
                    UPDATE  dbo.LE_WithdrawalsDetail
                    SET     Status = @Status ,
                            Reason = @Reason
                    WHERE   RecID = @RecID
                    ";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Status",(int)status),
                        new SqlParameter("@RecID",recId),
                        new SqlParameter("@Reason",reason),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int UpdateSyncResult(int withdrawalsId, WithdrawalsAuditStatusEnum withfrawalsAuditStatus)
        {
            var sql = $@"
                    UPDATE DBO.LE_WithdrawalsDetail SET AuditStatus = @AuditStatus,SyncPayStatus = @SyncPayStatus  WHERE RecID = @RecID
                    ";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AuditStatus",(int)withfrawalsAuditStatus),
                            new SqlParameter("@SyncPayStatus",(int)WithdrawalsStatusEnum.支付中),
                        new SqlParameter("@RecID",withdrawalsId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        public int UpdateSyncResult(string syncResult, int withdrawalsId)
        {
            var sql = $@"
                    UPDATE DBO.LE_WithdrawalsDetail SET SyncResult = @SyncResult  WHERE RecID = @RecID
                    ";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@SyncResult",syncResult),
                        new SqlParameter("@RecID",withdrawalsId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int UpdateAsynResult(string asynResult, int withdrawalsId, WithdrawalsStatusEnum withdrawalsStatus)
        {
            var sql = $@"
                    UPDATE DBO.LE_WithdrawalsDetail SET AsynResult = @AsynResult,Status = @Status WHERE RecID = @RecID
                    ";
            var parameters = new SqlParameter[]{
                 new SqlParameter("@Status",(int)withdrawalsStatus),
                        new SqlParameter("@AsynResult",asynResult),
                        new SqlParameter("@RecID",withdrawalsId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 再次提交审核
        /// </summary>
        /// <param name="withdrawalsId">提现申请id</param>
        /// <param name="money"></param>
        /// <param name="userId">提现申请人</param>
        /// <returns></returns>
        public int UpdateApplyAgain(int withdrawalsId, decimal money, int userId)
        {
            var sqlWithdrawals = $@"
                UPDATE  dbo.LE_WithdrawalsDetail
                SET     Status = {(int)WithdrawalsStatusEnum.支付中}
                WHERE   RecID = {withdrawalsId}
                        AND PayeeID = {userId}
                    ";

            sqlWithdrawals += $@"
                    UPDATE  dbo.LE_WithdrawalsStatistics
                    SET     WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) + {money}
                            , RemainingAmount = ISNULL(RemainingAmount,0) - {money}
                    WHERE   UserID = {userId};
                    ";
            var sql = $@"

                    BEGIN TRANSACTION tran_bank;
                    DECLARE @tran_error INT;
                    SET @tran_error = 0;
                    BEGIN TRY

                        {sqlWithdrawals}
                        SET @tran_error = @tran_error + @@error;
                    
                    END TRY
                    BEGIN CATCH
                        PRINT '出现异常，错误编号：' + CONVERT(VARCHAR, ERROR_NUMBER())
                            + '， 错误消息：' + ERROR_MESSAGE();
                        SET @tran_error = @tran_error + 1;
                    END CATCH;
                    IF ( @tran_error > 0 )
                        BEGIN
                        --执行出错，回滚事务
                            ROLLBACK TRAN;
                            SELECT  0;
                            PRINT '失败';
                        END;
                    ELSE
                        BEGIN
                        --没有异常，提交事务
                            COMMIT TRAN;
                            SELECT  1;
                            PRINT '成功';
                        END;
                    ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int UpdateLock(int withdrawalsId, bool lockStatus)
        {
            var sql = $@"
                    UPDATE DBO.LE_WithdrawalsDetail SET IsLock = @IsLock  WHERE RecID = @RecID
                    ";
            var parameters = new SqlParameter[]{
                 new SqlParameter("@IsLock",lockStatus),
                        new SqlParameter("@RecID",withdrawalsId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int UpdateSyncPayStatus(int withdrawalsId, WithdrawalsStatusEnum withdrawalsStatus, string syncResult = "")
        {
            var sql = $@"
                    UPDATE DBO.LE_WithdrawalsDetail SET SyncPayStatus = @SyncPayStatus 
                    ";
            if (!string.IsNullOrWhiteSpace(syncResult))
            {
                sql += $",SyncResult = '{syncResult}'";
            }
            sql += " WHERE RecID = @RecID";
            var parameters = new SqlParameter[]{
                 new SqlParameter("@SyncPayStatus",(int)withdrawalsStatus),
                        new SqlParameter("@RecID",withdrawalsId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }


        public int UpdateNotPass(int withdrawalsId, string reason, decimal money, int userId)
        {
            var sqlWithdrawals = $@"
                    UPDATE DBO.LE_WithdrawalsDetail 
                        SET AuditStatus = {(int)WithdrawalsAuditStatusEnum.驳回}
                            ,Status = {(int)WithdrawalsStatusEnum.支付失败}
                            ,SyncPayStatus = {(int)WithdrawalsStatusEnum.支付失败}
                            ,Reason = '{reason}'
                        WHERE RecID = {withdrawalsId}
                    ";
            //支付失败： 体现中-金额      余额+金额
            sqlWithdrawals += $@"
                        UPDATE  dbo.LE_WithdrawalsStatistics
                        SET     WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) - {money}
                                ,RemainingAmount = ISNULL(RemainingAmount,0) + {money}
                        WHERE   UserID = {userId};
                        ";

            var sql = $@"

                    BEGIN TRANSACTION tran_bank;
                    DECLARE @tran_error INT;
                    SET @tran_error = 0;
                    BEGIN TRY

                        {sqlWithdrawals}
                        SET @tran_error = @tran_error + @@error;
                    
                    END TRY
                    BEGIN CATCH
                        PRINT '出现异常，错误编号：' + CONVERT(VARCHAR, ERROR_NUMBER())
                            + '， 错误消息：' + ERROR_MESSAGE();
                        SET @tran_error = @tran_error + 1;
                    END CATCH;
                    IF ( @tran_error > 0 )
                        BEGIN
                        --执行出错，回滚事务
                            ROLLBACK TRAN;
                            SELECT  0;
                            PRINT '失败';
                        END;
                    ELSE
                        BEGIN
                        --没有异常，提交事务
                            COMMIT TRAN;
                            SELECT  1;
                            PRINT '成功';
                        END;
                    ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}

