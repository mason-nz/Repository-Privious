using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.QingNiao
{
    //渠道统计
    public partial class ChituChannelStat : DataBase
    {
        public static readonly ChituChannelStat Instance = new ChituChannelStat();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.QingNiao.ChituChannelStat entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into chitu_channel_stat(");
            strSql.Append("dt,channel,pv,uv,avg_dur,orders,order_phones");
            strSql.Append(") values (");
            strSql.Append("@dt,@channel,@pv,@uv,@avg_dur,@orders,@order_phones");
            strSql.Append(") ");

            var parameters = new SqlParameter[]{
                        new SqlParameter("@dt",entity.Dt),
                        new SqlParameter("@channel",entity.Channel),
                        new SqlParameter("@pv",entity.Pv),
                        new SqlParameter("@uv",entity.Uv),
                        new SqlParameter("@avg_dur",entity.Avg_dur),
                        new SqlParameter("@orders",entity.Orders),
                        new SqlParameter("@order_phones",entity.Order_Phones),
                        };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }
    }
}