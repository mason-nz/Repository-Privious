using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.QingNiao
{
    //点击明细统计
    public partial class ChituClickStat : DataBase
    {
        public static readonly ChituClickStat Instance = new ChituClickStat();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.QingNiao.ChituClickStat entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into chitu_click_stat(");
            strSql.Append("dt,click_name,pv,uv,material_id,click_site,click_val");
            strSql.Append(") values (");
            strSql.Append("@dt,@click_name,@pv,@uv,@material_id,@click_site,@click_val");
            strSql.Append(") ");

            var parameters = new SqlParameter[]{
                        new SqlParameter("@dt",entity.Dt),
                        new SqlParameter("@click_name",entity.Click_Name),
                        new SqlParameter("@pv",entity.Pv),
                        new SqlParameter("@uv",entity.Uv),
                        new SqlParameter("@material_id",entity.Material_Id),
                        new SqlParameter("@click_site",entity.Click_Site),
                        new SqlParameter("@click_val",entity.Click_Val),
                        };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public List<Entities.QingNiao.ChituClickStat> GetList(DistributeQuery<Entities.QingNiao.ChituClickStat> query)
        {
            var sql = $@"
                        SELECT  CCS.dt ,
                                CCS.click_name ,
                                CCS.pv ,
                                CCS.uv ,
                                CCS.material_id ,
                                CCS.click_site ,
                                CCS.click_val  ,
                                AI.XyAttr,
		                        AI.ReadNum
                        FROM    dbo.chitu_click_stat AS CCS WITH ( NOLOCK )
                        LEFT JOIN BaseData2017.DBO.ArticleInfo AS AI WITH(NOLOCK) ON AI.RecID = CAST(CCS.click_val AS INT)
                        WHERE   CCS.material_id = @material_id

                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter(@"material_id",query.MaterielId)
            };
            if (!string.IsNullOrWhiteSpace(query.StartDate) || !string.IsNullOrWhiteSpace(query.EndDate))
            {
                var endDate = Convert.ToDateTime(query.EndDate).AddDays(1).ToString("yyyy-MM-dd");
                sql += $" AND CCS.dt >= @StartDate AND CCS.dt < @EndDate";
                parameters.Add(new SqlParameter("@StartDate", query.StartDate));
                parameters.Add(new SqlParameter("@EndDate", endDate));
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());

            return DataTableToList<Entities.QingNiao.ChituClickStat>(data.Tables[0]);
        }
    }
}