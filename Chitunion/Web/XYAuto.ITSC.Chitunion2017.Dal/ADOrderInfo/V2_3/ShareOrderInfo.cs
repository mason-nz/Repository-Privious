/********************************************************
*创建人：hant
*创建时间：2018/1/17 19:14:46 
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

namespace XYAuto.ITSC.Chitunion2017.Dal.ADOrderInfo.V2_3
{
    public class ShareOrderInfo:DataBase
    {
        public static readonly ShareOrderInfo Instance = new ShareOrderInfo();

        /// <summary>
        /// 获取订单分享信息
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public Entities.DTO.V2_3.ShareOrderInfo GetInfoByOrderId(int OrderID)
        {
            var sql = @"SELECT [TotalAmount]
                          ,[Status]
                          ,[OrderUrl]
                          ,[UserID]
                      FROM [Chitunion2017].[dbo].[LE_ADOrderInfo]
                      WHERE RecID=@OrderID";
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.Int,4)
            };
            parameters[0].Value = OrderID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return DataTableToEntity<Entities.DTO.V2_3.ShareOrderInfo>(ds.Tables[0]);
        }
    }
}
