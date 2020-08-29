/********************************************************
*创建人：lixiong
*创建时间：2017/8/24 14:20:04
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Entities.Query.GDT;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.BOP2017.Dal.GDT
{
    public class GdtHourlyRrportForZhy : DataBase
    {
        public static readonly GdtHourlyRrportForZhy Instance = new GdtHourlyRrportForZhy();

        public int InsertByGdtServer(List<Entities.GDT.GdtHourlyRrportForZhy> list,
            Entities.GDT.GdtHourlyRrportForZhy entityWhere, int pageIndex)
        {
            var sql = new StringBuilder();
            if (list.Count == 0)
                return 1;
            if (pageIndex == 1)
            {
                sql.AppendFormat(@"DELETE FROM DBO.GDT_HourlyRrportForZHY
                                            WHERE DemandBillNo = {0}
                                            AND Level = {1}
                                            AND [Date] = '{2}' ", entityWhere.DemandBillNo, entityWhere.Level,
                                            entityWhere.Date.ToString("yyyy-MM-dd"));
            }

            sql.AppendFormat(@"
                    INSERT dbo.GDT_HourlyRrportForZHY
                            ( AccountId ,
                              Level ,
                              Hour ,
                              CampaignId ,
                              AdgroupId ,
                              Impression ,
                              Click ,
                              Cost ,
                              Download ,
                              Conversion,
                              Date ,
                              PullTime,
                              OrderQuantity,
                              BillOfQuantities ,DemandBillNo
                            )
                    VALUES
                    ");

            list.ForEach(item =>
            {
                sql.AppendFormat(@"({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},'{10}',GETDATE(),-2,-2,{11}),"
                , entityWhere.AccountId, entityWhere.Level, item.Hour, item.CampaignId, item.AdgroupId, item.Impression, item.Click, item.Cost, item.Download,
                item.Conversion, entityWhere.Date.Date, entityWhere.DemandBillNo
                );
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString().Trim(','));
        }

        public List<Entities.GDT.GdtHourlyRrportForZhy> GetHourlyRrportForZhy(ReportQuery<Entities.GDT.GdtHourlyRrport> query)
        {
            var sql = @"
                        SELECT  SUM(Impression) AS Impression ,
                                SUM(Click) AS Click ,
                                SUM(Cost) AS Cost ,
                                SUM(Download) AS Download ,
                                SUM(Conversion) AS Conversion ,
                                [Hour] ,
                                [Date] ,
                                DemandBillNo
                        FROM    dbo.GDT_HourlyRrportForZHY WITH(NOLOCK)
                        WHERE  1 = 1  {0}
                        GROUP BY [HOUR] ,DemandBillNo,[Date]
                    ";

            var sqlFilter = string.Empty;
            var parameters = new List<SqlParameter>()
            {
            };
            if (query.AccountId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sqlFilter += " AND AccountId = @AccountId";
                parameters.Add(new SqlParameter("@AccountId", query.AccountId));
            }
            if (query.Level != ReportLevelEnum.None)
            {
                sqlFilter += " AND Level = @Level";
                parameters.Add(new SqlParameter("@Level", (int)query.Level));
            }
            if (query.DemandBillNo != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sqlFilter += " AND DemandBillNo = @DemandBillNo";
                parameters.Add(new SqlParameter("@DemandBillNo", query.DemandBillNo));
            }
            if (!string.IsNullOrWhiteSpace(query.Date))
            {
                sqlFilter += $" AND DATEDIFF(dd,[Date],'{query.Date}') = 0";
            }
            sql = string.Format(sql, sqlFilter);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.GDT.GdtHourlyRrportForZhy>(data.Tables[0]);
        }
    }
}