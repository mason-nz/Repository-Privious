using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //广点通资金账户日结明细
    public partial class GdtStatementDaily : DataBase
    {
        public static readonly GdtStatementDaily Instance = new GdtStatementDaily();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtStatementDaily entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_StatementDaily(");
            strSql.Append("AccountId,FundType,TradeType,Time,Amount,Description,PullTime");
            strSql.Append(") values (");
            strSql.Append("@AccountId,@FundType,@TradeType,@Time,@Amount,@Description,@PullTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@FundType",entity.FundType),
                        new SqlParameter("@TradeType",entity.TradeType),
                        new SqlParameter("@Time",entity.Time),
                        new SqlParameter("@Amount",entity.Amount),
                        new SqlParameter("@Description",entity.Description),
                        new SqlParameter("@PullTime",entity.PullTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtStatementDaily> list,
            Entities.GDT.GdtStatementDaily entityWhere, int page)
        {
            if (list.Count == 0)
                return 1;
            var sbSql = new StringBuilder();
            if (page == 1)
            {
                //删除今天的日期
                sbSql.AppendFormat(@"DELETE FROM dbo.GDT_StatementDaily WHERE  AccountId = {0} AND FundType = {2}
                                                AND [Date] = '{1}'", entityWhere.AccountId,
                                                entityWhere.Date.ToString("yyyy-MM-dd"), entityWhere.FundType);
            }
            sbSql.AppendFormat(@"
                        INSERT INTO [dbo].[GDT_StatementDaily]
                                   ([AccountId]
                                   ,[FundType]
                                   ,[TradeType]
                                   ,[Time]
                                   ,[Amount]
                                   ,[Description]
                                   ,[PullTime]
                                   ,[Date])
                             VALUES
                        ");
            list.ForEach(item =>
            {
                sbSql.AppendFormat("( {0},{1},{2},{3},{4},'{5}',getdate(),'{6}' ),", entityWhere.AccountId, entityWhere.FundType, item.TradeType,
                    item.Time, item.Amount, item.Description, entityWhere.Date.Date);
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString().Trim(','));
        }
    }
}