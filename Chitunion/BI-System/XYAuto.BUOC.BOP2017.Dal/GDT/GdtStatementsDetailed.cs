using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //广点通资金账户流水
    public class GdtStatementsDetailed : DataBase
    {
        #region Instance

        public static readonly GdtStatementsDetailed Instance = new GdtStatementsDetailed();

        #endregion Instance

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtStatementsDetailed entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_StatementsDetailed(");
            strSql.Append("AccountId,TradeType,FundType,Amount,Date,ExternalBillNo,PullTime,Description");
            strSql.Append(") values (");
            strSql.Append("@AccountId,@TradeType,@FundType,@Amount,@Date,@ExternalBillNo,@PullTime,@Description");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@TradeType",entity.TradeType),
                        new SqlParameter("@FundType",entity.FundType),
                        new SqlParameter("@Amount",entity.Amount),
                        new SqlParameter("@Date",entity.Date),
                        new SqlParameter("@ExternalBillNo",entity.ExternalBillNo),
                        new SqlParameter("@PullTime",entity.PullTime),
                        new SqlParameter("@Description",entity.Description),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtStatementsDetailed> list,
            Entities.GDT.GdtStatementsDetailed entityWhere, int pageIndex)
        {
            var sql = new StringBuilder();
            if (list.Count == 0)
                return 1;
            if (pageIndex == 1)
            {
                sql.AppendFormat(@"DELETE FROM DBO.GDT_StatementsDetailed
                                WHERE AccountId = {0} AND FundType = {1}
                                AND [Date] = '{2}'", entityWhere.AccountId, entityWhere.FundType,
                                entityWhere.Date.ToString("yyyy-MM-dd"));
            }
            sql.AppendFormat(@"
                        INSERT INTO [dbo].[GDT_StatementsDetailed]
                                   ([AccountId]
                                   ,[TradeType]
                                   ,[FundType]
                                   ,[Amount]
                                   ,[Date]
                                   ,[ExternalBillNo]
                                   ,[Description]
                                   ,[PullTime])
                             VALUES
                        ");
            list.ForEach(item =>
            {
                sql.AppendFormat(@"( {0},{1},{2},{3},'{4}','{5}','{6}',getdate() ),",
                    entityWhere.AccountId, item.TradeType, entityWhere.FundType, item.Amount, entityWhere.Date, item.ExternalBillNo, item.Description);
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString().Trim(','));
        }
    }
}