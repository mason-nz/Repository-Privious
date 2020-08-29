/********************************************************
*创建人：lixiong
*创建时间：2017/8/21 16:30:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    public class GdtAdGroup : DataBase
    {
        #region Instance

        public static readonly GdtAdGroup Instance = new GdtAdGroup();

        #endregion Instance

        public List<Entities.GDT.GdtAdGroup> GetList(int configuredStatus)
        {
            var sql = @"
                        SELECT ADS.* FROM DBO.GDT_AdGroup AS ADS WITH(NOLOCK) WHERE 1 =1
                        ";
            if (configuredStatus > 0)
            {
                sql += $" AND ADS.ConfiguredStatus = {configuredStatus}";
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.GDT.GdtAdGroup>(data.Tables[0]);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtAdGroup> list,
            Entities.GDT.GdtAdGroup entityWhere, int pageIndex)
        {
            var sql = new StringBuilder();
            if (list.Count == 0)
                return 1;
            if (pageIndex == 1)
            {
                sql.AppendFormat(@"DELETE FROM DBO.GDT_AdGroup
                                WHERE AccountId = {0} ", entityWhere.AccountId);
            }
            sql.AppendFormat(@"
                        INSERT INTO [dbo].[GDT_AdGroup]
                                   ([AccountId]
                                   ,[CampaignId]
                                   ,[AdgroupId]
                                   ,[AdgroupName]
                                   ,[ConfiguredStatus]
                                   ,[SystemStatus]
                                   ,[RejectMessage]
                                   ,[SiteSet]
                                   ,[OptimizationGoal]
                                   ,[BillingEvent]
                                   ,[BidAmount]
                                   ,[DailyBudget]
                                   ,[BeginDate]
                                   ,[EndDate]
                                   ,[TimeSeries]
                                   ,[CustomizedCategory]
                                   ,[CreatedTime]
                                   ,[LastModifiedTime]
                                   ,[PullTime])
                             VALUES
                        ");
            list.ForEach(item =>
            {
                sql.AppendFormat(@"( {0},{1},{2},'{3}',{4},{5},'{6}','{7}',{8},{9},{10},{11},'{12}','{13}','{14}','{15}',{16},{17},getdate() ),",
                    entityWhere.AccountId, item.CampaignId, item.AdgroupId, item.AdgroupName,
                    item.ConfiguredStatus, item.SystemStatus, item.RejectMessage, item.SiteSet, item.OptimizationGoal, item.BillingEvent, item.BidAmount
                    , item.DailyBudget, item.BeginDate, item.EndDate, item.TimeSeries, item.CustomizedCategory, item.CreatedTime,
                    item.LastModifiedTime);
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString().Trim(','));
        }
    }
}