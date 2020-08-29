using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.Entities.Query.GDT;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    //小时报表
    public partial class GdtHourlyRrport : DataBase
    {
        public static readonly GdtHourlyRrport Instance = new GdtHourlyRrport();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(XYAuto.ITSC.Chitunion2017.Entities.GDT.GdtHourlyRrport entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into GDT_HourlyRrport(");
            strSql.Append("AccountId,Level,Hour,CampaignId,AdgroupId,Impression,Click,Cost,Download,Conversion,Activation,AppPaymentCount,AppPaymentAmount,LikeOrComment,ImageClick,Follow,Share,Date,PullTime");
            strSql.Append(") values (");
            strSql.Append("@AccountId,@Level,@Hour,@CampaignId,@AdgroupId,@Impression,@Click,@Cost,@Download,@Conversion,@Activation,@AppPaymentCount,@AppPaymentAmount,@LikeOrComment,@ImageClick,@Follow,@Share,@Date,@PullTime");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@AccountId",entity.AccountId),
                        new SqlParameter("@Level",entity.Level),
                        new SqlParameter("@Hour",entity.Hour),
                        new SqlParameter("@CampaignId",entity.CampaignId),
                        new SqlParameter("@AdgroupId",entity.AdgroupId),
                        new SqlParameter("@Impression",entity.Impression),
                        new SqlParameter("@Click",entity.Click),
                        new SqlParameter("@Cost",entity.Cost),
                        new SqlParameter("@Download",entity.Download),
                        new SqlParameter("@Conversion",entity.Conversion),
                        new SqlParameter("@Activation",entity.Activation),
                        new SqlParameter("@AppPaymentCount",entity.AppPaymentCount),
                        new SqlParameter("@AppPaymentAmount",entity.AppPaymentAmount),
                        new SqlParameter("@LikeOrComment",entity.LikeOrComment),
                        new SqlParameter("@ImageClick",entity.ImageClick),
                        new SqlParameter("@Follow",entity.Follow),
                        new SqlParameter("@Share",entity.Share),
                        new SqlParameter("@Date",entity.Date),
                        new SqlParameter("@PullTime",entity.PullTime),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int InsertByGdtServer(List<Entities.GDT.GdtHourlyRrport> list,
       Entities.GDT.GdtHourlyRrport entityWhere, int pageIndex)
        {
            var sql = new StringBuilder();
            if (list.Count == 0)
                return 1;
            if (pageIndex == 1)
            {
                sql.AppendFormat(@"DELETE FROM DBO.GDT_HourlyRrport
                                            WHERE AccountId = {0}
                                            AND Level = {1}
                                            AND DATEDIFF(dd,[Date],'{2}')=0 ", entityWhere.AccountId, entityWhere.Level,
                                            entityWhere.Date.ToString("yyyy-MM-dd"));
            }

            sql.AppendFormat(@"
                    INSERT dbo.GDT_HourlyRrport
                            ( AccountId ,
                              Level ,
                              Hour ,
                              CampaignId ,
                              AdgroupId ,
                              Impression ,
                              Click ,
                              Cost ,
                              Download ,
                              Conversion ,
                              Activation ,
                              AppPaymentCount ,
                              AppPaymentAmount ,
                              LikeOrComment ,
                              ImageClick ,
                              Follow ,
                              Share ,
                              Date ,
                              PullTime,
                              OrderQuantity,
                              BillOfQuantities
                            )
                    VALUES
                    ");

            list.ForEach(item =>
            {
                sql.AppendFormat(@"({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},'{17}',GETDATE(),-2,-2),"
, entityWhere.AccountId, entityWhere.Level, item.Hour, item.CampaignId, item.AdgroupId, item.Impression, item.Click, item.Cost, item.Download,
item.Conversion, item.Activation, item.AppPaymentCount, item.AppPaymentAmount, item.LikeOrComment, item.ImageClick, item.Follow, item.Share,
entityWhere.Date.Date
);
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql.ToString().Trim(','));
        }
    }
}