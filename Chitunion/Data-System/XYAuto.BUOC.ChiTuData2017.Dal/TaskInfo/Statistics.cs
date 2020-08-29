/********************************************************
*创建人：hant
*创建时间：2017/12/20 9:55:25 
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

namespace XYAuto.BUOC.ChiTuData2017.Dal.TaskInfo
{
    public class Statistics:DataBase
    {
        public static readonly Statistics Instance = new Statistics();

        /// <summary>
        /// 获取分析数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Entities.Task.TaskStatistics> GetStatisticsByCode(string code,string begindate,string enddate, int PageIndex, int PageSize, out int totalCount)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT TaskID AS TaskId,[CPCCount] AS UV,[CPLCount] AS Clue,[PVCount] AS PV, CONVERT(nvarchar(10),[StatisticsTime]) AS Date,OrderUrl YanFaFrom [Chitunion2017].[dbo].[LE_AccountBalance] AB INNER JOIN [Chitunion2017].[dbo].[LE_ADOrderInfo] OI ON AB.OrderID = OI.RecID WHERE OI.OrderCoding='{0}' AND StatisticsTime BETWEEN '{1}' AND '{2}'", code, begindate, enddate);
            string strOrder = " StatisticsTime DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL.ToString()),
                new SqlParameter("@CurPage",PageIndex),
                new SqlParameter("@PageRows",PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var ds = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.StoredProcedure, "p_Page", sqlParams);
            totalCount = (int)(sqlParams[0].Value);
            return DataTableToList<Entities.Task.TaskStatistics>(ds.Tables[0]);
        }
    }
}
