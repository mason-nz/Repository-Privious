/********************************************************
*创建人：hant
*创建时间：2018/1/17 19:24:48 
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

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class ShareOrderInfo: DataBase
    {
        public static readonly ShareOrderInfo Instance = new ShareOrderInfo();

        public Entities.DTO.V2_3.ShareOrderInfo GetInfoByOrderId(int OrderID)
        {
            var sql = @"SELECT [TotalAmount] AS Price
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

        public bool IsOrder(int TaskId, int UserId)
        {
            var sql = @"SELECT COUNT(0)
                  FROM [Chitunion2017].[dbo].[LE_ADOrderInfo]
                  WHERE TaskID=@TaskID AND UserID = @UserID";
            SqlParameter[] parameters = {
                    new SqlParameter("@TaskID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
            };
            parameters[0].Value = TaskId;
            parameters[1].Value = UserId;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return Convert.ToInt32(obj) >0;
        }

        public DataSet GetOrderByTaskIdAndUserId(int TaskId, int UserId)
        {
            var sql = @"SELECT OrderUrl
                  FROM [Chitunion2017].[dbo].[LE_ADOrderInfo]
                  WHERE TaskID=@TaskID AND UserID = @UserID";
            SqlParameter[] parameters = {
                    new SqlParameter("@TaskID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
            };
            parameters[0].Value = TaskId;
            parameters[1].Value = UserId;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return ds;
        }
    }
}
