using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.BusinessLL
{
    public class MaterielStat
    {
        public static readonly MaterielStat Instance = new MaterielStat();
        private string Conn_OPData = ConfigurationManager.AppSettings["ConnectionStrings_OPData"];


        /// <summary>
        /// 根据统计时间（天），获取统计数据
        /// </summary>
        /// <param name="date">格式为：yyyy-MM-dd</param>
        /// <returns>返回多张列表</returns>
        public DataSet GetStatData(string date)
        {
            

            SqlParameter[] parameters = new SqlParameter[]
               {
                new SqlParameter("@Date",SqlDbType.VarChar,10)
               };
            parameters[0].Value = date;
            //DataSet ds = SqlHelper.ExecuteDataset(Conn_OPData, CommandType.StoredProcedure, "p_StatMaterielData", parameters);
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    return ds;
            //}
            //return null;


            using (SqlConnection conn = new SqlConnection(Conn_OPData))
            {
                conn.Open();
                using (SqlCommand cmd = DAL.Weixin_ArticleInfo.Instance.GetCommand(conn, null, CommandType.StoredProcedure, "p_StatMaterielData", parameters))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        //sw.Start();
                        //BLL.Loger.Log4Net.Info($"调用存储过过程p_QueryArticleData，参数topNum={topNum}，Resource={Resource}，结束，调用耗时={sw.ElapsedMilliseconds}毫秒，结果返回条数为：{(dt != null && dt.Rows.Count > 0 ? dt.Rows.Count : 0)}条记录");
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                return ds;
                            }
                        }
                        return null;
                    }
                }
            }
        }
    }
}
