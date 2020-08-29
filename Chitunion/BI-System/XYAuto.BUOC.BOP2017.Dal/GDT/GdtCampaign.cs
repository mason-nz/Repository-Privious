using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    //广点通推广计划表
    public partial class GdtCampaign : DataBase
    {
        public static readonly GdtCampaign Instance = new GdtCampaign();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GDT.GdtCampaign entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_Campaign(");
            strSql.Append("CampaignId,CampaignName,AccountId,CreatedTime,LastModifiedTime,PullTime");
            strSql.Append(") values (");
            strSql.Append("@CampaignId,@CampaignName,@AccountId,@CreatedTime,@LastModifiedTime,@PullTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@CampaignId",entity.CampaignId),
                        new SqlParameter("@CampaignName",entity.CampaignName),
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@CreatedTime",entity.CreatedTime),
                        new SqlParameter("@LastModifiedTime",entity.LastModifiedTime),
                        new SqlParameter("@PullTime",entity.PullTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtCampaign> list,
           Entities.GDT.GdtCampaign entityWhere, int pageIndex)
        {
            var sql = new StringBuilder();
            if (list.Count == 0)
                return 1;
            if (pageIndex == 1)
            {
                sql.AppendFormat(@"DELETE FROM DBO.GDT_Campaign
                                WHERE AccountId = {0} ", entityWhere.AccountId);
            }
            sql.AppendFormat(@"
                        INSERT INTO [dbo].[GDT_Campaign]
                               ([CampaignId]
                               ,[CampaignName]
                               ,[AccountId]
                               ,[ConfiguredStatus]
                               ,[CampaignType]
                               ,[DailyBudget]
                               ,[BudgetReachDate]
                               ,[CreatedTime]
                               ,[LastModifiedTime]
                               ,[PullTime])
                         VALUES
                        ");
            list.ForEach(item =>
            {
                sql.AppendFormat(@"( {0},'{1}',{2},{3},{4},{5},{6},{7},{8},getdate() ),",
                   item.CampaignId, item.CampaignName, entityWhere.AccountId, item.ConfiguredStatus, item.CampaignType, item.DailyBudget, item.BudgetReachDate
                   , item.CreatedTime, item.LastModifiedTime);
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString().Trim(','));
        }
    }
}