using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    public class LeChannelStatMonthRelation : DataBase
    {
        public static readonly LeChannelStatMonthRelation Instance = new LeChannelStatMonthRelation();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeChannelStatMonthRelation entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [dbo].[LE_ChannelStatMonthRelation]
                                   ([StatisticsId]
                                   ,[PayStatus]
                                   ,[PayTime]
                                   ,[CreateUserId]
                                   ,[CreateTime]
                                   ,[Status]
                                   ,[Reason])
                             VALUES
                                   (@StatisticsId
                                   ,@PayStatus
                                   ,@PayTime
                                   ,@CreateUserId
                                   ,GETDATE()
                                   ,@Status
                                   ,@Reason)");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@StatisticsId",entity.StatisticsId),
                        new SqlParameter("@PayStatus",entity.PayStatus),
                        new SqlParameter("@PayTime",entity.PayTime),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Reason",entity.Reason)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public Entities.LETask.LeChannelStatMonthRelation GetInfoByStatId(int statisticsId)
        {
            var sql = $@"
                    SELECT  RecId ,
                            StatisticsId ,
                            PayStatus ,
                            PayTime ,
                            CreateUserId ,
                            CreateTime ,
                            Status ,
                            Reason
                    FROM    dbo.LE_ChannelStatMonthRelation
                    WHERE   Status = 0 AND StatisticsId = {statisticsId};
                    ";

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeChannelStatMonthRelation>(obj.Tables[0]);
        }

        public decimal GetToTalDecimal(string sqlWhere)
        {
            var sql = $@"

                   SELECT  SUM(DSB.TotalAmount) AS TotalAmount
                    FROM    Chitunion_DataSystem2017.dbo.DataStatisticsByMonth AS DSB WITH ( NOLOCK )
                            LEFT JOIN dbo.LE_ChannelStatMonthRelation AS CSM WITH ( NOLOCK ) ON CSM.StatisticsId = DSB.RecID
                    WHERE   1 = 1
                    {sqlWhere}
                    ";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }


        public List<ChannelStatMonth> GetChannelStatMonths()
        {
            var sql = @"
                    SELECT DISTINCT TOP 12
                            ( CONVERT(VARCHAR(7), DSB.Date, 120) ) AS Date
                    FROM    dbo.DataStatisticsByMonth AS DSB
                    WHERE   DSB.Status = 0
                    ORDER BY Date DESC;
                    ";

            var obj = SqlHelper.ExecuteDataset(ConnectDataSystem2017, CommandType.Text, sql);
            return DataTableToList<Entities.LETask.ChannelStatMonth>(obj.Tables[0]);
        }
    }
}
