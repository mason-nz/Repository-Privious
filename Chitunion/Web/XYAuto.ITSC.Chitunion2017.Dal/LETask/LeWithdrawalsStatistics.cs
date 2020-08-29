using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    public class LeWithdrawalsStatistics : DataBase
    {
        public static readonly LeWithdrawalsStatistics Instance = new LeWithdrawalsStatistics();


        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.LETask.LeWithdrawalsStatistics entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_WithdrawalsStatistics(");
            strSql.Append("AccumulatedIncome,HaveWithdrawals,WithdrawalsProcess,UserID,CreateTime,Status");
            strSql.Append(") values (");
            strSql.Append("@AccumulatedIncome,@HaveWithdrawals,@WithdrawalsProcess,@UserID,@CreateTime,@Status");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AccumulatedIncome",entity.AccumulatedIncome),
                        new SqlParameter("@HaveWithdrawals",entity.HaveWithdrawals),
                        new SqlParameter("@WithdrawalsProcess",entity.WithdrawalsProcess),
                        new SqlParameter("@UserID",entity.UserID),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@Status",entity.Status),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public Entities.LETask.LeWithdrawalsStatistics GetInfo(int userId)
        {
            var sql = $@"

                   
                SELECT  WS.RecID ,
                        WS.AccumulatedIncome ,
                        WS.HaveWithdrawals ,
                        WS.WithdrawalsProcess ,
                        WS.UserID ,
                        WS.CreateTime ,
                        WS.Status ,
                        WS.RemainingAmount
                FROM    dbo.LE_WithdrawalsStatistics AS WS WITH ( NOLOCK )
                WHERE   WS.UserID = {userId};
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWithdrawalsStatistics>(obj.Tables[0]);
        }


        /// <summary>
        /// 提现操作-校验规则
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="withdrawalsMobile">提现手机号</param>
        /// <param name="withdrawalsMoney">提现金额</param>
        /// <returns></returns>
        public VerifyResultCode VerifyWithdrawals(int userId, string withdrawalsMobile, decimal withdrawalsMoney)
        {
            var sql = $@"

                    DECLARE @UserId INT = {userId},
		                    @WithdrawalsMobile VARCHAR(20) = '{withdrawalsMobile}',
		                    @WithdrawalsMoney DECIMAL(18,2) = {withdrawalsMoney} ,
		                    @Mobile VARCHAR(20) = '' ,
		                    @AuditStatus INT = 0
                    SELECT  UD.*,UI.Mobile
                    INTO    #Temp_UserDetailInfo
                    FROM    
                    dbo.UserInfo AS UI WITH(NOLOCK)
                    LEFT JOIN dbo.UserDetailInfo AS UD WITH ( NOLOCK ) ON UD.UserID = UI.UserID
                    WHERE   UI.UserID = @UserId

                    IF(NOT EXISTS(SELECT 1 FROM #Temp_UserDetailInfo))
                    BEGIN
	                    SELECT  1011 AS ResultCode;--用户没有补全信息
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    SELECT  @AuditStatus = Status,@Mobile = Mobile FROM  #Temp_UserDetailInfo
                    IF(@AuditStatus != 2)
                    BEGIN
	                    SELECT  1012 AS ResultCode;--用户资质没有审核通过
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    IF(@Mobile = '' OR @Mobile IS NULL)
					BEGIN
						SELECT  1018 AS ResultCode;--未绑定手机号
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
					END

                    IF(NOT EXISTS(SELECT 1 FROM dbo.LE_UserBankAccount AS UB WITH(NOLOCK)
                    WHERE UB.UserID = @UserId))
                    BEGIN
	                    SELECT  1013 AS ResultCode;--用户还未添加提现账户
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    IF ( EXISTS ( SELECT    1
                                  FROM      dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                                  WHERE     WD.PayeeID = @UserId
                                            AND WD.IsActive = 1
                                            AND DATEDIFF(DAY,WD.ApplicationDate,'{DateTime.Now}') = 0 ) )
                        BEGIN
                            SELECT  1016 AS ResultCode;--一天只能提现一次
                            SELECT  * FROM    #Temp_UserDetailInfo;
                            RETURN;
                        END;

                    IF ( EXISTS ( SELECT    1
                                  FROM      dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                                  WHERE     WD.PayeeID = @UserId
                                            AND WD.IsActive = 1
                                            AND WD.Status = {(int)WithdrawalsStatusEnum.支付中} ) )
                        BEGIN
                            SELECT  1017 AS ResultCode;--有正在提现中的申请，不能多次申请同时进行
                            SELECT  * FROM    #Temp_UserDetailInfo;
                            RETURN;
                        END

                    --IF(@WithdrawalsMobie != @Mobile)
                    --BEGIN
	                    --SELECT  1014 AS ResultCode;--用户提现手机号与注册手机号不一致
                        --SELECT  * FROM  #Temp_UserDetailInfo;
                        --RETURN;
                    --END

                    DECLARE @RemainingAmount DECIMAL(18,2) = 0
                    SELECT TOP 1
                            @RemainingAmount = ISNULL(WS.RemainingAmount,0)
                    FROM    dbo.LE_WithdrawalsStatistics AS WS WITH ( NOLOCK )
                    WHERE WS.UserID = @UserId

                    IF(@RemainingAmount < @WithdrawalsMoney)
                    BEGIN
	                    SELECT  1015 AS ResultCode;--可提现金额不足，无法提现
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    SELECT  0 AS ResultCode;
                    SELECT  * FROM  #Temp_UserDetailInfo;
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.VerifyResultCode>(obj.Tables[0]);
        }

        /// <summary>
        /// 提现操作-校验规则-点击提现按钮校验
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public VerifyResultCode VerifyWithdrawalsClick(int userId)
        {
            var sql = $@"

                    DECLARE @UserId INT = {userId},
		                    @AuditStatus INT = 0 ,
                            @Mobile VARCHAR(20) = ''
                    SELECT  UD.*,UI.Mobile
                    INTO    #Temp_UserDetailInfo
                    FROM    
                    dbo.UserInfo AS UI WITH(NOLOCK)
                    LEFT JOIN dbo.UserDetailInfo AS UD WITH ( NOLOCK ) ON UD.UserID = UI.UserID
                    WHERE   UI.UserID = @UserId

                    IF(NOT EXISTS(SELECT 1 FROM #Temp_UserDetailInfo))
                    BEGIN
	                    SELECT  1011 AS ResultCode;--用户没有补全信息
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    SELECT  @AuditStatus = Status,@Mobile = Mobile FROM  #Temp_UserDetailInfo
                    IF(@AuditStatus != 2)
                    BEGIN
	                    SELECT  1012 AS ResultCode;--用户资质没有审核通过
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    IF(@Mobile = '' OR @Mobile IS NULL)
					BEGIN
						SELECT  1018 AS ResultCode;--未绑定手机号
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
					END

                    IF(NOT EXISTS(SELECT 1 FROM dbo.LE_UserBankAccount AS UB WITH(NOLOCK)
                    WHERE UB.UserID = @UserId))
                    BEGIN
	                    SELECT  1013 AS ResultCode;--用户还未添加提现账户
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    IF ( EXISTS ( SELECT    1
                                  FROM      dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                                  WHERE     WD.PayeeID = @UserId
                                            AND WD.IsActive = 1
                                            AND DATEDIFF(DAY,WD.ApplicationDate,'{DateTime.Now}') = 0 ) )
                        BEGIN
                            SELECT  1016 AS ResultCode;--一天只能提现一次
                            SELECT  * FROM    #Temp_UserDetailInfo;
                            RETURN;
                        END;

                    IF ( EXISTS ( SELECT    1
                                  FROM      dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                                  WHERE     WD.PayeeID = @UserId 
                                            AND WD.IsActive = 1
                                            AND WD.Status = {(int)WithdrawalsStatusEnum.支付中} ) )
                        BEGIN
                            SELECT  1017 AS ResultCode;--有正在提现中的申请，不能多次申请同时进行
                            SELECT  * FROM    #Temp_UserDetailInfo;
                            RETURN;
                        END

                    SELECT  0 AS ResultCode;--可提现金额不足，无法提现
                    SELECT  * FROM  #Temp_UserDetailInfo;
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.VerifyResultCode>(obj.Tables[0]);
        }

        /// <summary>
        /// 提现操作-审核-校验规则
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="withdrawalsMoney">提现金额</param>
        /// <returns></returns>
        public Tuple<VerifyResultCode, Entities.LETask.UserDetailInfo> VerifyWithdrawalsAudit(int userId, decimal withdrawalsMoney)
        {
            var sql = $@"

                    DECLARE @UserId INT = {userId},
		                    @WithdrawalsMobile VARCHAR(20) = '',
		                    @WithdrawalsMoney DECIMAL(18,2) = {withdrawalsMoney} ,
		                    @Mobile VARCHAR(20) = '' ,
		                    @AuditStatus INT = 0
                    SELECT  UD.*,UI.Mobile,UI.Type
                    INTO    #Temp_UserDetailInfo
                    FROM    
                    dbo.UserInfo AS UI WITH(NOLOCK)
                    LEFT JOIN dbo.UserDetailInfo AS UD WITH ( NOLOCK ) ON UD.UserID = UI.UserID
                    WHERE   UI.UserID = @UserId

                    IF(NOT EXISTS(SELECT 1 FROM #Temp_UserDetailInfo))
                    BEGIN
	                    SELECT  1011 AS ResultCode;--用户没有补全信息
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    SELECT  @AuditStatus = Status,@Mobile = Mobile FROM  #Temp_UserDetailInfo
                    IF(@AuditStatus != 2)
                    BEGIN
	                    SELECT  1012 AS ResultCode;--用户资质没有审核通过
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END


                    IF(NOT EXISTS(SELECT 1 FROM dbo.LE_UserBankAccount AS UB WITH(NOLOCK)
                    WHERE UB.UserID = @UserId))
                    BEGIN
	                    SELECT  1013 AS ResultCode;--用户还未添加提现账户
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END

                    --判断 当前审核的提现申请金额，与提现统计当中 提现中的金额对比，如果统计中的体现中的金额小于了 当前申请的提现金额，那就是不对
                    DECLARE @WithdrawalsProcess DECIMAL(18,2) = 0
                    SELECT TOP 1
		                    @WithdrawalsProcess = ISNULL(WS.WithdrawalsProcess,0) 
                    FROM    dbo.LE_WithdrawalsStatistics AS WS WITH ( NOLOCK )
                    WHERE WS.UserID = @UserId

                    IF(@WithdrawalsProcess < @WithdrawalsMoney)
                    BEGIN
	                    SELECT  1015 AS ResultCode;--可提现金额不足，无法提现(对账错误)
                        SELECT  * FROM  #Temp_UserDetailInfo;
                        RETURN;
                    END
                    SELECT  0 AS ResultCode;
                    SELECT  * FROM  #Temp_UserDetailInfo;
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<VerifyResultCode, Entities.LETask.UserDetailInfo>(
                DataTableToEntity<Entities.LETask.VerifyResultCode>(obj.Tables[0]),
                DataTableToEntity<Entities.LETask.UserDetailInfo>(obj.Tables[1]));
        }


        /// <summary>
        /// 提现操作之后，修改剩余金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public int UpdateWithdrawalsStatistics(int userId, decimal money)
        {
            var sql = $@"
                    UPDATE  dbo.LE_WithdrawalsStatistics
                    SET     WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) + {money}
                            , RemainingAmount = ISNULL(RemainingAmount,0) - {money}
                    WHERE   UserID = {userId};
                    ";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// <summary>
        /// 提现审核通过
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <param name="withdrawalsId"></param>
        /// <param name="reason"></param>
        /// <param name="asynResult"></param>
        /// <param name="status"></param>
        /// <param name="payDateTime"></param>
        /// <returns></returns>
        public int UpdateWithdrawalsStatisticsSuccess(int userId, decimal money,
            int withdrawalsId, string reason, string asynResult, WithdrawalsStatusEnum status, DateTime payDateTime)
        {
            var sqlWithdrawals = "";
            if (status == WithdrawalsStatusEnum.已支付)
            {
                //支付成功：已提现+金额   体现中-金额
                sqlWithdrawals = $@"
                        UPDATE  dbo.LE_WithdrawalsStatistics
                        SET     HaveWithdrawals = ISNULL(HaveWithdrawals, 0) + {money}
                                ,WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) - {money}
                        WHERE   UserID = {userId} AND Status = 0 ;
                        ";
            }
            else if (status == WithdrawalsStatusEnum.支付失败)
            {
                //支付失败： 体现中-金额      余额+金额
                sqlWithdrawals = $@"
                        UPDATE  dbo.LE_WithdrawalsStatistics
                        SET     WithdrawalsProcess = ISNULL(WithdrawalsProcess, 0) - {money}
                                ,RemainingAmount = ISNULL(RemainingAmount,0) + {money}
                        WHERE   UserID = {userId} AND Status = 0 ;
                        ";
            }


            var sql = $@"

                    BEGIN TRANSACTION tran_bank;
                    DECLARE @tran_error INT;
                    SET @tran_error = 0;
                    BEGIN TRY
                        {sqlWithdrawals}
                        SET @tran_error = @tran_error + @@error;

                        UPDATE  dbo.LE_WithdrawalsDetail
                        SET     Status = {(int)status} ,
                                Reason = '{reason}',
                                AsynResult = '{asynResult}',
                                PayDate = '{payDateTime}'
                        WHERE   RecID = {withdrawalsId}
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
        /// <summary>
        /// 获取用户今日收益
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetTodayIncome(int userId)
        {
            string strSql = $@"SELECT SUM(IncomePrice) FROM dbo.LE_IncomeDetail WHERE CONVERT(VARCHAR(10), IncomeTime, 23) = CONVERT(VARCHAR(10), DATEADD(DAY,-1,GETDATE()), 23)  AND UserID = {userId}  ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToDecimal(obj.ToString() == "" ? 0 : obj);
        }
    }
}
