using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils.Data;

namespace BitAuto.DSC.APPReport2016.Dal
{
    public class BrandAd
    {
        public static readonly BrandAd Instance = new BrandAd();

        /// 品牌广告合作文字信息（合作概况 + 合作排名）
        /// <summary>
        /// 品牌广告合作文字信息（合作概况 + 合作排名）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataSet GetBrandOverViewData()
        {
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetBrandOverViewData");
        }
        /// 获取品牌柱状图数据
        /// <summary>
        /// 获取品牌柱状图数据
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetBrandBarData()
        {
            return SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "p_GetBrandBarData").Tables[0];
        }
        /// 获取品牌排名数据
        /// <summary>
        /// 获取品牌排名数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBrandRankData(string order, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = @"SELECT b.Name,a.Amount,a.[Percent],a.YearBasis
                YanFaFROM dbo.BrandAdItem AS a 
                INNER JOIN dbo.v_CarBrand AS b ON a.BrandId = b.BrandId
                WHERE Year =(
                SELECT TOP 1
                CASE WHEN CAST(YearMonth / 100 AS INT) > YEAR(GETDATE())
                THEN YEAR(GETDATE())
                ELSE CAST(YearMonth / 100 AS INT)
                END
                FROM dbo.BrandAd
                ORDER BY YearMonth DESC)";
            SqlParameter[] parameters = {				
                    new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
                    new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = sql;
            parameters[1].Value = order;
            parameters[2].Value = pageIndex;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = SqlHelper.ExecuteDataset(DataBase.ConnectionStrings, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)parameters[4].Value;
            return ds.Tables[0];
        }
    }
}
