using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //投放订单信息
    public partial class LeAdOrderInfo : DataBase
    {


        public static readonly LeAdOrderInfo Instance = new LeAdOrderInfo();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeAdOrderInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_ADOrderInfo(");
            strSql.Append("BeginTime,EndTime,TotalAmount,Status,OrderType,OrderName,OrderUrl,PasterUrl,UserID,TaskID,CreateTime,BillingRuleName,OrderCoding,MediaType,MediaID,ChannelID,UserIdentity,CPCUnitPrice,CPLUnitPrice,StatisticsStatus");
            strSql.Append(",IP,PromotionChannelID");
            strSql.Append(") values (");
            strSql.Append("@BeginTime,@EndTime,@TotalAmount,@Status,@OrderType,@OrderName,@OrderUrl,@PasterUrl,@UserID,@TaskID,@CreateTime,@BillingRuleName,@OrderCoding,@MediaType,@MediaID,@ChannelID,@UserIdentity,@CPCUnitPrice,@CPLUnitPrice,@StatisticsStatus");
            strSql.Append(",@IP,@PromotionChannelID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@BeginTime",entity.BeginTime),
                        new SqlParameter("@EndTime",entity.EndTime),
                        new SqlParameter("@TotalAmount",entity.TotalAmount),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@OrderType",entity.OrderType),
                        new SqlParameter("@OrderName",entity.OrderName),
                        new SqlParameter("@OrderUrl",entity.OrderUrl),
                        new SqlParameter("@PasterUrl",entity.PasterUrl),
                        new SqlParameter("@UserID",entity.UserID),
                        new SqlParameter("@TaskID",entity.TaskID),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@BillingRuleName",entity.BillingRuleName),
                        new SqlParameter("@OrderCoding",entity.OrderCoding),
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@ChannelID",entity.ChannelID),
                        new SqlParameter("@UserIdentity",entity.UserIdentity),
                        new SqlParameter("@CPCUnitPrice",entity.CPCUnitPrice),
                        new SqlParameter("@CPLUnitPrice",entity.CPLUnitPrice),
                        new SqlParameter("@StatisticsStatus",entity.StatisticsStatus),
                        new SqlParameter("@IP",entity.IP),
                        new SqlParameter("@PromotionChannelID",entity.PromotionChannelID),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public Entities.LETask.LeAdOrderInfo GetAdOrderInfo(int orderId, int userId)
        {
            var sql = $@"

                    SELECT  AO.RecID,
                            AO.BeginTime ,
                            AO.EndTime ,
                            AO.TotalAmount ,
                            AO.Status ,
                            AO.OrderType ,
                            AO.OrderName ,
                            CASE WHEN AO.PromotionChannelID > 0
                                 THEN AO.OrderUrl + '&channel='
                                      + CAST(AO.PromotionChannelID AS VARCHAR(20))
                                 ELSE AO.OrderUrl
                            END OrderUrl ,
                            AO.PasterUrl ,
                            AO.UserID ,
                            AO.TaskID ,
                            AO.CreateTime ,
                            AO.BillingRuleName ,
                            AO.OrderCoding ,
                            AO.MediaType ,
                            AO.MediaID ,
                            AO.ChannelID ,
                            AO.UserIdentity  ,
                            DC.DictName AS OrderStatus ,
                            T.CPCPrice,
							T.CPLPrice,
                            AO.CPCUnitPrice ,
                            AO.CPLUnitPrice  ,
                            AO.StatisticsStatus
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )  
                            LEFT JOIN DBO.LE_TaskInfo AS T WITH(NOLOCK) ON T.RecID = AO.TaskID
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AO.Status
                    WHERE   AO.RecID = {orderId} AND AO.UserID = {userId}
                ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeAdOrderInfo>(obj.Tables[0]);
        }

        public Entities.LETask.LeAdOrderInfo GetAdOrderInfoByTaskId(int taskId, int userId)
        {
            var sql = $@"

                    SELECT TOP 1 AO.RecID,
                            AO.BeginTime ,
                            AO.EndTime ,
                            AO.TotalAmount ,
                            AO.Status ,
                            AO.OrderType ,
                            AO.OrderName ,
                            AO.OrderUrl ,
                            AO.PasterUrl ,
                            AO.UserID ,
                            AO.TaskID ,
                            AO.CreateTime ,
                            AO.BillingRuleName ,
                            AO.OrderCoding ,
                            AO.MediaType ,
                            AO.MediaID ,
                            AO.ChannelID ,
                            AO.UserIdentity  ,
                            DC.DictName AS OrderStatus ,
                            T.CPCPrice,
							T.CPLPrice,
                            AO.CPCUnitPrice ,
                            AO.CPLUnitPrice  ,
                            AO.StatisticsStatus
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )  
                            LEFT JOIN DBO.LE_TaskInfo AS T WITH(NOLOCK) ON T.RecID = AO.TaskID
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AO.Status
                    WHERE   AO.TaskID = {taskId} AND AO.UserID = {userId}
                ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeAdOrderInfo>(obj.Tables[0]);
        }

        public decimal GetTotalAmount(string sqlWhere)
        {
            var sql = $@"

                    SELECT  SUM(ISNULL(AO.TotalAmount,0)) AS TotalAmount
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )
                    WHERE   1 = 1
                    {sqlWhere}
                    ";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }

        public decimal GetTotalAmountByMediaOwn(string sqlWhere)
        {
            var sql = $@"
                SELECT  SUM(ISNULL(AO.TotalAmount,0)) AS TotalAmount
                FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )
                        LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = AO.UserID
                WHERE   1 = 1
                    {sqlWhere}
                ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }

        public int GetUserOrderCount(int userId)
        {
            var sql = $@"

		            DECLARE @StartTime date = GETDATE(),@EndTime DATE
					SET @EndTime = DATEADD(DAY,1,@StartTime)

					SELECT COUNT(*) FROM dbo.LE_ADOrderInfo AS AO WITH(NOLOCK)
					WHERE AO.UserID = {userId} AND AO.CreateTime >= @StartTime AND AO.CreateTime < @EndTime
                ";
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int GetOrderCodingCount(string orderCoding)
        {
            var sql = $@"
		            SELECT COUNT(*) FROM dbo.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    WHERE AO.OrderCoding = @OrderCoding
                ";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@OrderCoding", orderCoding)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

    }
}

