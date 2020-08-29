using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WechatWithdrawals
{
    public class WechatWithdrawals : DataBase
    {
        public readonly static WechatWithdrawals Instance = new WechatWithdrawals();
        public DataTable GetWithdrawalsInfo(int WithdrawalsId, int UserID)
        {
            string strSql = $@"
                    SELECT  WD.RecID ,
                            WD.WithdrawalsPrice ,
                            WD.IndividualTaxPeice ,
                            WD.PracticalPrice ,
                            WD.PayeeAccount ,
                            WD.Status PayStatus,
                            WD.ApplicationDate ,
                            WD.PayDate ,
                            WD.PayeeID ,
                            WD.Reason ,
                            WD.CreateTime,
		                    UI.SysName AS TrueName ,
		                    UI.UserName,
		                    DC.DictName AS UserTypeName ,
                            DC1.DictName AS PayStatusName,
                            DP.DisbursementNo AS OrderNum,
                            AI.RejectMsg
                    FROM    dbo.LE_WithdrawalsDetail  AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.v_UserInfo  AS UI WITH ( NOLOCK ) ON UI.UserID = WD.PayeeID
		                    LEFT JOIN DBO.DictInfo AS DC WITH ( NOLOCK )  ON DC.DictId = UI.Type
                            LEFT JOIN DBO.DictInfo AS DC1 WITH ( NOLOCK )  ON DC1.DictId = WD.Status
                            LEFT JOIN LE_DisbursementPay DP WITH ( NOLOCK )  ON WD.RecID=DP.WithdrawalsId
                            LEFT JOIN dbo.AuditInfo AS AI WITH ( NOLOCK ) ON WD.RecID=AI.RelationId AND AI.RelationType = {(int)AuditTypeEnum.提现审核}
                    WHERE   WD.RecID = {WithdrawalsId} and WD.PayeeID={UserID} ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 添加用户金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="incomPrice"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public int AddWithdrawals(int userId, decimal incomPrice, SqlTransaction trans = null)
        {
            string strSql = $@"DECLARE @StatisticsCount INT;
                            SELECT @StatisticsCount = COUNT(1)
                            FROM LE_WithdrawalsStatistics
                            WHERE UserID = {userId};
                                        IF(@StatisticsCount > 0)
                                BEGIN
                                    UPDATE  LE_WithdrawalsStatistics
                                    SET     AccumulatedIncome += {incomPrice} ,
                                            RemainingAmount = ISNULL(RemainingAmount,
                                                                     AccumulatedIncome - HaveWithdrawals)
                                            + {incomPrice}
                                    WHERE UserID = {userId};
                                        END;
                                        ELSE
                                            BEGIN
                                    INSERT INTO dbo.LE_WithdrawalsStatistics
                                           (AccumulatedIncome,
                                             HaveWithdrawals,
                                             WithdrawalsProcess,
                                             UserID,
                                             CreateTime,
                                             Status,
                                             RemainingAmount
                                           )
                                    VALUES( {incomPrice}, 
                                              0, 
                                              0, 
                                              {userId}, 
                                              GETDATE(), 
                                              0, 
                                              {incomPrice}
                                            );
                                        END; ";
            int rowcount = 0;
            if (trans == null)
                rowcount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            else
                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, strSql);
            return rowcount;
        }
    }
}
