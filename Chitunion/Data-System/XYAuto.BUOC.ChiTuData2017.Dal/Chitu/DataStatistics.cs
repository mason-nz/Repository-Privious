/********************************************************
*创建人：hant
*创建时间：2017/12/21 14:04:49 
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
    public class DataStatistics :DataBase
    {
        public static readonly DataStatistics Instance = new DataStatistics();

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="swhere">查询条件</param>
        /// <returns></returns>
        public List<Entities.Chitu.DataStatistics> GetDataByChannelAndDate(string swhere)
        {
            var sql = @"
            SELECT [ProfitOfYesterday]
                  ,[ProfitOfThisMonth]
                  ,[ProfitLastMonth]
                  ,[Accumulated]
                  ,[SettledAmount]
                  ,[ChannelID]
              FROM [dbo].[DataStatistics] 
              WHERE [Status]=0 " + swhere;
           
            var data = SqlHelper.ExecuteDataset(Chitunion_DataSystem2017, CommandType.Text, sql);
            return DataTableToList<Entities.Chitu.DataStatistics>(data.Tables[0]);
        }
    }
}
