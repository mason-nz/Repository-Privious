/********************************************************
*创建人：hant
*创建时间：2017/12/21 17:08:29 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Chitu
{
    public class Order:DataBase
    {
        public static readonly Order Instance = new Order();

        /// <summary>
        /// 订单列表分页
        /// </summary>
        /// <param name="swhere"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataSet GetOrder(string swhere, string abwhere ,int PageIndex, int PageSize, out int totalCount)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" SELECT OI.RecID AS OrderId,OI.TaskID,AI.AppName AS ChannelName");
            SQL.Append(" ,CASE WHEN (LEN(UserIdentity)>0)  THEN UserIdentity ELSE UI.UserName END AS UserIdentity");
            SQL.Append(" ,OI.CreateTime,OI.EndTime,TI.CPCPrice, CPCCount,TI.CPLPrice,CPLCount,TotalAmount");
            SQL.Append(" ,(ISNULL(CPCLimitPrice,0)+ ISNULL(CPLLimitPrice,0)) AS TotalProfit,DictName AS OrderType");
            SQL.Append(" YanFaFrom [dbo].[LE_ADOrderInfo] OI WITH(NOLOCK)");
            SQL.Append(" INNER JOIN [dbo].[LE_TaskInfo] TI WITH(NOLOCK) ON OI.TaskID = TI.RecID");
            SQL.Append(" INNER JOIN [dbo].[DictInfo] DI WITH(NOLOCK) ON OI.OrderType = DI.DictId ");
            SQL.Append(" INNER JOIN [Chitunion_DataSystem2017]..AppInfo AI ON OI.ChannelID = AI.ChannelID");
            SQL.Append("  LEFT JOIN Chitunion2017..UserInfo UI ON OI.UserID = UI.UserID");
            SQL.AppendFormat(" LEFT JOIN (SELECT OrderID,SUM(CPCCount) AS CPCCount,SUM(CPLCount) AS CPLCount FROM [dbo].[LE_AccountBalance] WITH(NOLOCK) WHERE Status =0 {0} GROUP BY OrderID) AB ON OI.RecID = AB.OrderID", abwhere);
            SQL.Append(" WHERE 1=1 ");
            SQL.Append( swhere );
            string strOrder = "OI.CreateTime DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL.ToString()),
                new SqlParameter("@CurPage",PageIndex),
                new SqlParameter("@PageRows",PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.StoredProcedure, "p_Page", sqlParams);
            totalCount = Convert.IsDBNull(sqlParams[0].Value) ? 0 : Convert.ToInt32(sqlParams[0].Value);
            return data;
        }

        public DataSet GetOrderExcel(string swhere,string abwhere)
        {
            var sql = @"
                        SELECT OI.RecID AS OrderId,OI.TaskID,AI.AppName AS ChannelName,CASE WHEN (LEN(UserIdentity)>0)  THEN UserIdentity ELSE UI.UserName END AS UserIdentity
                        ,OI.CreateTime,OI.EndTime,TI.CPCPrice,CPCCount,TI.CPLPrice,CPLCount,TotalAmount
                         ,(ISNULL(CPCLimitPrice,0)+ ISNULL(CPLLimitPrice,0)) AS TotalProfit,DictName AS OrderType
                        From [dbo].[LE_ADOrderInfo] OI WITH(NOLOCK)
                        INNER JOIN [dbo].[LE_TaskInfo] TI WITH(NOLOCK) ON OI.TaskID = TI.RecID
                        INNER JOIN [dbo].[DictInfo] DI WITH(NOLOCK) ON OI.OrderType = DI.DictId
                        INNER JOIN [Chitunion_DataSystem2017]..AppInfo AI ON OI.ChannelID = AI.ChannelID
                       LEFT JOIN Chitunion2017..UserInfo UI ON OI.UserID = UI.UserID
                       LEFT JOIN 
(SELECT OrderID,SUM(CPCCount) AS CPCCount,SUM(CPLCount) AS CPLCount FROM [dbo].[LE_AccountBalance] WITH(NOLOCK)
WHERE Status = 0 
GROUP BY OrderID) AB  ON OI.RecID = AB.OrderID 
                        WHERE 1=1  " + swhere + " ORDER BY OI.CreateTime DESC ";
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.Text, sql);
            return data;
        }

        /// <summary>
        /// 订单信息
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataSet GetOrderByOrderId(int orderid)
        {
            var sql = @"
                        SELECT OI.RecID AS OrderId,OI.TaskID,TI.TaskName,DIC.DictName AS ChannelName,DI.DictName AS OrderType
                        ,DIS.DictName AS Status,OI.CreateTime,OI.EndTime,CASE WHEN (LEN(UserIdentity)>0)  THEN UserIdentity ELSE UI.UserName END AS UserIdentity
                        ,CPCPrice,CPLPrice,(ISNULL(CPCLimitPrice,0)+ ISNULL(CPLLimitPrice,0)) AS TotalProfit,NULL AS StateOfSettlement,NULL AS TimeOfSettlement,CASE WHEN DI.DictId=192002 THEN ImgUrl ELSE MaterialUrl END AS MaterialUrl,
                           CASE WHEN DI.DictId=192002 THEN PasterUrl ELSE  OrderUrl END AS OrderUrl,ChannelID 
                        FROM [dbo].[LE_ADOrderInfo] OI WITH(NOLOCK)
                        INNER JOIN [dbo].[LE_TaskInfo] TI WITH(NOLOCK) ON OI.TaskID = TI.RecID
                        INNER JOIN [dbo].[DictInfo] DI WITH(NOLOCK) ON OI.OrderType = DI.DictId
                        INNER JOIN [dbo].[DictInfo] DIS WITH(NOLOCK) ON OI.Status = DIS.DictId
                        INNER JOIN [dbo].[DictInfo] DIC WITH(NOLOCK) ON OI.ChannelID = DIC.DictId
                        LEFT JOIN Chitunion2017..UserInfo UI ON OI.UserID = UI.UserID
                        WHERE OI.RecID = @OrderId";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@OrderId",orderid)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.Text, sql, parameters.ToArray());
            return data;
        }

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataSet GetOrderBalanceByOrderId(int orderid)
        {
            var sql = @"
                        SELECT StatisticsTime AS Date,CPCCount,CPCTotalPrice AS CPCProfit,CPLCount,CPLTotalPrice AS CPLProfit, TotalMoney AS  TotalProfit
                        FROM [dbo].[LE_AccountBalance]  AB WITH(NOLOCK)
                        INNER JOIN [dbo].[LE_ADOrderInfo] OI WITH(NOLOCK) ON AB.OrderID = OI.RecID
                        WHERE AB.StatisticsTime>=OI.BeginTime AND AB.StatisticsTime<=OI.EndTime
						AND  OrderID = @OrderId
                        ORDER BY StatisticsTime DESC";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@OrderId",orderid)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.Text, sql, parameters.ToArray());
            return data;
        }
    }
}
