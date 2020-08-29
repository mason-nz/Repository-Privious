/********************************************************
*创建人：hant
*创建时间：2018/1/25 15:53:07 
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

namespace XYAuto.ITSC.Chitunion2017.Dal.WeChat
{
    public class Order : DataBase
    {
        public static readonly Order Instance = new Order();

        public Entities.DTO.V2_3.OrderResDTO GetOrderByStatus(int userid, int status, int PageIndex, int PageSize, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT TaskID,[RecID] AS OrderId,[TotalAmount],[OrderName],[OrderUrl],[CreateTime],[CPCUnitPrice]  YanFaFROM [Chitunion2017].[dbo].[LE_ADOrderInfo]");
            sql.AppendFormat(" WHERE MediaType = 14001 AND [Status] ={0} AND UserID ={1}", status, userid);

            string strOrder = " CreateTime DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",sql.ToString()),
                new SqlParameter("@CurPage",PageIndex),
                new SqlParameter("@PageRows",PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            totalCount = Convert.IsDBNull(sqlParams[0].Value) ? 0 : Convert.ToInt32(sqlParams[0].Value);
            List<Entities.DTO.V2_3.Order> list = DataTableToList<Entities.DTO.V2_3.Order>(data.Tables[0]);
            return new Entities.DTO.V2_3.OrderResDTO() { TotalCount = totalCount, List = list };
        }
        /// <summary>
        /// 获取用户前一天订单数量
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="DayCount">前N天数据</param>
        /// <returns></returns>
        public int GetUserDayOrderCount(int UserId, int DayCount)
        {
            string SelectSQL = $@"SELECT COUNT(1) FROM dbo.LE_ADOrderInfo O WHERE O.UserID={UserId} ";
            if (DayCount > 0)
            {
                SelectSQL += $" AND DATEDIFF(DAY, O.CreateTime, GETDATE()) = {DayCount}";
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SelectSQL);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}
