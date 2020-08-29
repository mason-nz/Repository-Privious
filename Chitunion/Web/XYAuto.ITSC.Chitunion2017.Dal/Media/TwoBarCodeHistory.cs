/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 13:37:48
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class TwoBarCodeHistory : DataBase
    {
        #region Instance

        public static readonly TwoBarCodeHistory Instance = new TwoBarCodeHistory();

        #endregion Instance

        public List<Entities.Media.TwoBarCodeHistory> GetList(TwoBarCodeHistoryQuery<Entities.Media.TwoBarCodeHistory> query)
        {
            var sql = @"SELECT * FROM DBO.TwoBarCodeHistory AS TBC WITH(NOLOCK) WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(query.RecIds))
            {
                sql += string.Format(" AND TBC.RecId IN ({0})", query.RecIds.Trim(','));
            }

            if (!string.IsNullOrWhiteSpace(query.OrderId))
            {
                sql += string.Format(" AND TBC.OrderID = {0}", query.OrderId);
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Media.TwoBarCodeHistory>(data.Tables[0]);
        }

        /// <summary>
        /// 此方法不要动
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<RespChannelListDto> GetChannelList(ChannelQuery<RespChannelListDto> query)
        {
            var sql = @"
                       SELECT  CI.ChannelID ,
                                CI.ChannelName ,
                                CI.CooperateBeginDate ,
                                CI.CooperateEndDate ,
                                AlreadyPayMoney = (
			                        SELECT SUM(SAD.CostTotal) FROM dbo.SubADInfo AS SAD WITH(NOLOCK)
			                        WHERE SAD.ChannelID = CI.ChannelID AND SAD.Status = @Status
		                        ),
                                CCD.CostPrice AS CostPriceReference ,
                                CCD.SalePrice AS SalePriceReference
                        FROM    dbo.ChannelInfo AS CI WITH ( NOLOCK )
                                INNER JOIN dbo.ChannelCost AS CC WITH ( NOLOCK ) ON CC.ChannelID = CI.ChannelID
                                INNER JOIN dbo.ChannelCostDetail AS CCD WITH ( NOLOCK ) ON CCD.CostID = CC.CostID
                        WHERE   CI.Status = 0
                                AND CC.MediaID = @MediaID
                                AND CCD.ADPosition1 = @ADPosition1
                                AND CCD.ADPosition2 = @ADPosition2
                                AND CCD.ADPosition3 = @ADPosition3
                                AND (CI.CooperateBeginDate <= @CooperateDate1 AND @CooperateDate2<= CI.CooperateEndDate)
                        ";

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@Status",(int)EnumInterfaceOrderStatus.OrderFinished),
                 new SqlParameter("@MediaID",query.MediaId),
                  new SqlParameter("@ADPosition1",query.AdPosition1),
                   new SqlParameter("@ADPosition2",query.AdPosition2),
                   new SqlParameter("@ADPosition3",query.AdPosition3),
                   new SqlParameter("@CooperateDate1",query.CooperateDate),
                new SqlParameter("@CooperateDate2",query.CooperateDate)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<RespChannelListDto>(data.Tables[0]);
        }

        public List<RespChannelListDto> GetChannelListForChannelId(ChannelQuery<RespChannelListDto> query)
        {
            var sql = @"
                       SELECT  CI.ChannelID ,
                                CI.ChannelName ,
                                CI.CooperateBeginDate ,
                                CI.CooperateEndDate ,
                                AlreadyPayMoney = (
			                        SELECT SUM(SAD.CostTotal) FROM dbo.SubADInfo AS SAD WITH(NOLOCK)
			                        WHERE SAD.ChannelID = CI.ChannelID AND SAD.Status = @Status
		                        ),
                                CCD.CostPrice AS CostPriceReference ,
                                CCD.SalePrice AS SalePriceReference
                        FROM    dbo.ChannelInfo AS CI WITH ( NOLOCK )
                                INNER JOIN dbo.ChannelCost AS CC WITH ( NOLOCK ) ON CC.ChannelID = CI.ChannelID
                                INNER JOIN dbo.ChannelCostDetail AS CCD WITH ( NOLOCK ) ON CCD.CostID = CC.CostID
                        WHERE   CI.Status = 0
                                AND CCD.ADPosition1 = @ADPosition1
                                AND CCD.ADPosition2 = @ADPosition2
                                AND CCD.ADPosition3 = @ADPosition3
                                AND (CI.CooperateBeginDate <= @CooperateDate1 AND @CooperateDate2<= CI.CooperateEndDate)
                        ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@Status",(int)EnumInterfaceOrderStatus.OrderFinished),
                  new SqlParameter("@ADPosition1",query.AdPosition1),
                   new SqlParameter("@ADPosition2",query.AdPosition2),
                   new SqlParameter("@ADPosition3",query.AdPosition3),
                   new SqlParameter("@CooperateDate1",query.CooperateDate),
                new SqlParameter("@CooperateDate2",query.CooperateDate)
            };
            if (query.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND CI.ChannelID = @ChannelID";
                paras.Add(new SqlParameter("@ChannelID", query.ChannelId));
            }
            if (query.MediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND CC.MediaID = @MediaID";
                paras.Add(new SqlParameter("@MediaID", query.MediaId));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<RespChannelListDto>(data.Tables[0]);
        }

        public void Insert(List<Entities.Media.TwoBarCodeHistory> list)
        {
            if (list.Count == 0) return;
            var sbCode = new StringBuilder();
            var firstEntity = list[0];
            sbCode.AppendFormat(@"DELETE FROM DBO.TwoBarCodeHistory WHERE MediaType = {0} AND OrderID = '{1}' AND MediaID IN ({2})",
                firstEntity.MediaType, firstEntity.OrderID, string.Join(",", list.Select(s => s.MediaID)));
            sbCode.AppendLine();
            sbCode.AppendFormat(@"INSERT INTO [dbo].[TwoBarCodeHistory]
                                           ([OrderID]
                                           ,[MediaType]
                                           ,[MediaID]
                                           ,[URL]
                                           ,[TwoBarUrl]
                                           ,[CreateTime]
                                           ,[CreateUserID])");
            sbCode.AppendFormat(@" VALUES ");
            list.ForEach(item =>
            {
                sbCode.AppendFormat(@"('{0}'
                                           ,{1}
                                           ,{2}
                                           ,'{3}'
                                           ,'{4}'
                                            ,GETDATE()
                                           ,{5}),", item.OrderID, item.MediaType, item.MediaID, item.URL, item.TwoBarUrl, item.CreateUserID);
            });

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbCode.ToString().Trim(','));
        }
    }
}