/********************************************************
*创建人：hant
*创建时间：2017/12/22 10:14:02 
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
    public class DataStatisticsByMonth:DataBase
    {
        public static readonly DataStatisticsByMonth Instance = new DataStatisticsByMonth();

        public List<Entities.Chitu.DataStatisticsByMonth> GetMonthStatistics(string swhere, int PageIndex, int PageSize, out int totalCount)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append(" SELECT [RecID],[Date],DM.ChannelID,[AppName] AS ChannelName,[OrderNumber],[TotalAmount],[StateOfSettlement],[TimeOfSettlement],[OperaterId],[OperaterName],DM.[Status],DM.[CreateTime] YanFaFrom [Chitunion_DataSystem2017].[dbo].[DataStatisticsByMonth] DM INNER JOIN[Chitunion_DataSystem2017].[dbo].[AppInfo] AI ON DM.ChannelID = AI.ChannelID");
            SQL.Append(" WHERE DM.[Status]=0 AND (DM.OrderNumber >0 OR DM.TotalAmount>0) ");
            SQL.Append(swhere);
            string strOrder = " Date DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL.ToString()),
                new SqlParameter("@CurPage",PageIndex),
                new SqlParameter("@PageRows",PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017, CommandType.StoredProcedure, "p_Page", sqlParams);
            totalCount = (int)(sqlParams[0].Value);
            return DataTableToList<Entities.Chitu.DataStatisticsByMonth>(data.Tables[0]);
        }

        public DataSet GetMonthStatisticsExcel(string swhere)
        {
            var sql = @"
                        SELECT [RecID],CONVERT(nvarchar(7), [Date],20) AS [Date],[AppName] AS ChannelName,[OrderNumber],[TotalAmount],[StateOfSettlement],[TimeOfSettlement],[OperaterId],[OperaterName],DM.[Status],DM.[CreateTime] From [Chitunion_DataSystem2017].[dbo].[DataStatisticsByMonth] DM
                          INNER JOIN [Chitunion_DataSystem2017].[dbo].[AppInfo] AI ON DM.ChannelID = AI.ChannelID
                        WHERE DM.[Status]=0  " + swhere;
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017, CommandType.Text, sql);
            return data;
        }
    }
}
