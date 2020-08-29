using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    //账户余额
    public partial class GdtAccountBalance : DataBase
    {
        #region Instance

        public static readonly GdtAccountBalance Instance = new GdtAccountBalance();

        #endregion Instance

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(XYAuto.ITSC.Chitunion2017.Entities.GDT.GdtAccountBalance entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_AccountBalance(");
            strSql.Append("AccountId,FundType,Balance,FundStatus,RealtimeCost,PullTime");
            strSql.Append(") values (");
            strSql.Append("@AccountId,@FundType,@Balance,@FundStatus,@RealtimeCost,@PullTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@FundType",entity.FundType),
                        new SqlParameter("@Balance",entity.Balance),
                        new SqlParameter("@FundStatus",entity.FundStatus),
                        new SqlParameter("@RealtimeCost",entity.RealtimeCost),
                        new SqlParameter("@PullTime",entity.PullTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtAccountBalance> list, Entities.GDT.GdtAccountBalance entityWhere)
        {
            var sql = new StringBuilder();
            if (list.Count == 0)
                return 1;
            sql.AppendFormat($"  UPDATE [dbo].[GDT_AccountBalance] SET Status = -1 WHERE AccountId = {entityWhere.AccountId}");
            sql.AppendFormat(@"  INSERT INTO [dbo].[GDT_AccountBalance]
                                               ([AccountId]
                                               ,[FundType]
                                               ,[Balance]
                                               ,[FundStatus]
                                               ,[RealtimeCost]
                                               ,[PullTime]
                                               ,[Status])");
            sql.AppendFormat(@" VALUES ");

            list.ForEach(item =>
            {
                sql.AppendFormat($" ( {entityWhere.AccountId},{item.FundType},{item.Balance},{item.FundStatus},{item.RealtimeCost},getdate(),0),");
            });
            var lastSql = sql.ToString().Trim(',');

            lastSql += $"{System.Environment.NewLine} DELETE FROM [dbo].[GDT_AccountBalance] WHERE Status = -1";
            var obj = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, lastSql);
            return Convert.ToInt32(obj);
        }
    }
}