/********************************************************
*创建人：lixiong
*创建时间：2017/8/21 13:52:08
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    public class GdtRealtimeCost : DataBase
    {
        public static readonly GdtRealtimeCost Instance = new GdtRealtimeCost();

        public int InsertByGdtServer(Entities.GDT.GdtRealtimeCost entity)
        {
            var sql = string.Format(@"
                        INSERT INTO [dbo].[GDT_RealtimeCost]
                                   ([AccountId]
                                   ,[Level]
                                   ,[Cost]
                                   ,[Date]
                                   ,[CampaignId]
                                   ,[AdgroupId]
                                   ,[PullTime])
                             VALUES
                                   ({0}
                                   ,{1}
                                   ,{2}
                                   ,'{3}'
                                   ,{4}
                                   ,{5}
                                   ,getdate());select SCOPE_IDENTITY()
                        ", entity.AccountId, entity.Level, entity.Cost, entity.Date, entity.CampaignId, entity.AdgroupId);

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        //public Entities.GDT.GdtRealtimeCost GetQueue()
        //{
        //    var sql = @"";
        //}
    }
}