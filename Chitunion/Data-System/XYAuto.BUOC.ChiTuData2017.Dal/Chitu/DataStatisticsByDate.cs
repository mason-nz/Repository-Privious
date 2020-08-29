/********************************************************
*创建人：hant
*创建时间：2017/12/21 15:33:33 
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
    public class DataStatisticsByDate :DataBase
    {
        public static readonly DataStatisticsByDate Instance = new DataStatisticsByDate();

        public List<Entities.Chitu.DataStatisticsByDate> GetDataByWhere(string swhere, int PageIndex, int PageSize, out int totalCount)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("SELECT [Profit],[OrderNumber],[StateOfSettlement],[TimeOfSettlement],[ChannelID],DictName AS ChannelName,[Date],DS.[CreateTime] YanFaFrom [Chitunion_DataSystem2017].[dbo].[DataStatisticsByDate] DS");
            SQL.Append(" INNER JOIN [Chitunion2017].[dbo].[DictInfo] DI ON DS.ChannelID = DI.DictId");
            SQL.Append("  WHERE DS.[Status] = 0 and (DS.Profit>0 OR DS.OrderNumber>0) ");
            SQL.Append(swhere);
            string strOrder = " DS.Date DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL.ToString()),
                new SqlParameter("@CurPage",PageIndex),
                new SqlParameter("@PageRows",PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017, CommandType.StoredProcedure, "p_Page", sqlParams);
            totalCount = Convert.IsDBNull(sqlParams[0].Value)?0:Convert.ToInt32(sqlParams[0].Value);
            return DataTableToList<Entities.Chitu.DataStatisticsByDate>(data.Tables[0]);
        }
    }
}
