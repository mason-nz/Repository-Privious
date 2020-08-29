using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.DAL
{
    public class Weixin_ArticleInfo : XYAuto.ITSC.Chitunion2017.Dal.DataBase
    {
        private int SQLCommandTimeout = (ConfigurationManager.AppSettings["ConnectionStrings_BaseData_SQLCommandTimeout"] == null ? 60 : int.Parse(ConfigurationManager.AppSettings["ConnectionStrings_BaseData_SQLCommandTimeout"].ToString()));//执行sql，超时时间60秒
        public static readonly Weixin_ArticleInfo Instance = new Weixin_ArticleInfo();
        private string Conn_BaseData = ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];

        public SqlCommand GetCommand(SqlConnection conn, SqlTransaction transaction, CommandType cmdType, string sql, SqlParameter[] parms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = cmdType;
            cmd.CommandText = sql;
            cmd.CommandTimeout = SQLCommandTimeout;
            if (transaction != null)
                cmd.Transaction = transaction;
            if (parms != null && parms.Length != 0)
                cmd.Parameters.AddRange(parms);
            return cmd;
        }
        /// <summary>
        /// 获取待清洗文章数据
        /// </summary>
        /// <param name="topNum">获取数量</param>
        /// <param name="Resource">（1-微信、3-头条、6-搜狐）</param>
        /// <returns></returns>
        public DataTable GetData(int topNum, int Resource)
        {
            lock (Instance)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                BLL.Loger.Log4Net.Info($"调用存储过过程p_QueryArticleData，参数topNum={topNum}，Resource={Resource}，开始");
                SqlParameter[] parameters = {
                    new SqlParameter("@TopNum", SqlDbType.Int,4),
                    new SqlParameter("@Resource", SqlDbType.Int,4)
                };
                parameters[0].Value = topNum;
                parameters[1].Value = Resource;
                //DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, "p_QueryArticleData", parameters);
                //BLL.Loger.Log4Net.Info($"调用存储过过程p_QueryArticleData，参数topNum={topNum}，Resource={Resource}，结束，结果返回条数为：{(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows.Count : 0)}条记录");
                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //    {
                //        return ds.Tables[0];
                //    }
                //}


                try
                {
                    using (SqlConnection conn = new SqlConnection(Conn_BaseData))
                    {
                        conn.Open();
                        using (SqlCommand cmd = GetCommand(conn, null, CommandType.StoredProcedure, "p_QueryArticleData", parameters))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                sw.Start();
                                BLL.Loger.Log4Net.Info($"调用存储过过程p_QueryArticleData，参数topNum={topNum}，Resource={Resource}，结束，调用耗时={sw.ElapsedMilliseconds}毫秒，结果返回条数为：{(dt != null && dt.Rows.Count > 0 ? dt.Rows.Count : 0)}条记录");
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        return dt;
                                    }
                                }
                                return null;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    BLL.Loger.Log4Net.Error($"调用存储过过程p_QueryArticleData，参数topNum ={ topNum}，Resource ={ Resource}，异常", ex);
                    return null;
                }
            }
        }

        public int UpdateStatusByIDs(string recids, int status)
        {
            BLL.Loger.Log4Net.Info($"更新这些微信Weixin_ArticleInfo表中的主键ID[{recids}],的状态为：{status}");
            string sql = string.Format(@"Update Weixin_ArticleInfo Set Status={0} WHERE Recid in ({1})", status, StringHelper.SqlFilter(recids));
            int r = SqlHelper.ExecuteNonQuery(Conn_BaseData, CommandType.Text, sql);
            return r;
        }


    }
}
