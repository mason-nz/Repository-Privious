using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    public class LeDisbursementPay : DataBase
    {


        public static readonly LeDisbursementPay Instance = new LeDisbursementPay();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeDisbursementPay entity,string syncResult)
        {
            var strSql = new StringBuilder();
            //添加同步接口返回接口
            strSql.Append($@" UPDATE DBO.LE_WithdrawalsDetail SET SyncResult = '{syncResult}'  WHERE RecID = {entity.WithdrawalsId}");
            //为了避免重复发起支付，产生多条数据，将之前的数据更改为不可用的状态，不能删除，为了记录
            strSql.Append($@" UPDATE [dbo].[LE_DisbursementPay] SET Status = -1 WHERE WithdrawalsId = {entity.WithdrawalsId}");
            strSql.Append(@"
                    INSERT INTO [dbo].[LE_DisbursementPay]
                               ([WithdrawalsId]
                               ,[DisbursementNo]
                               ,[BizDisbursementNo]
                               ,[BizNo]
                               ,[ContractNo]
                               ,[Remark]
                               ,[CreateTime]
                               ,[Status])
                         VALUES
                               (@WithdrawalsId
                               ,@DisbursementNo
                               ,@BizDisbursementNo
                               ,@BizNo
                               ,@ContractNo
                               ,@Remark
                               ,GETDATE()
                               ,@Status)");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@WithdrawalsId",entity.WithdrawalsId),
                        new SqlParameter("@DisbursementNo",entity.DisbursementNo),
                        new SqlParameter("@BizDisbursementNo",entity.BizDisbursementNo),
                        new SqlParameter("@BizNo",entity.BizNo),
                        new SqlParameter("@ContractNo",entity.ContractNo),
                        new SqlParameter("@Remark",entity.Remark),
                         new SqlParameter("@Status",entity.Status)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public List<Entities.LETask.LeDisbursementPay> GetInfo(string disbursementNo)
        {
            var sql = $@"
                        SELECT  DP.RecID ,
                                DP.WithdrawalsId ,
                                DP.DisbursementNo ,
                                DP.BizDisbursementNo ,
                                DP.BizNo ,
                                DP.ContractNo ,
                                DP.Remark ,
                                DP.CreateTime ,
                                WD.Status,
		                        WD.WithdrawalsPrice,
                                WD.PayeeID ,
                                WS.AccumulatedIncome ,
                                WS.HaveWithdrawals ,
                                WS.WithdrawalsProcess ,
                                WS.RemainingAmount,
                                WD.Status AS PayStatus
                        FROM    dbo.LE_DisbursementPay AS DP WITH ( NOLOCK )
                                INNER JOIN dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK ) ON WD.RecID = DP.WithdrawalsId AND WD.IsActive = 1
								INNER JOIN DBO.LE_WithdrawalsStatistics AS WS WITH(NOLOCK) ON WS.UserID = WD.PayeeID AND WS.Status = 0
                        WHERE   DP.DisbursementNo = '{disbursementNo}' AND DP.Status = 0
                        ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.LETask.LeDisbursementPay>(obj.Tables[0]);
        }
    }
}
