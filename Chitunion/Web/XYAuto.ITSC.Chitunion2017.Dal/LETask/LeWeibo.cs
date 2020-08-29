using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //媒体-微博公众号信息
    public partial class LeWeibo : DataBase
    {


        public static readonly LeWeibo Instance = new LeWeibo();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeWeibo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_Weibo(");
            strSql.Append("Number,Name,Sex,HeadIconURL,FansCount,FansCountURL,FansSex,CategoryID,AreaID,Profession,ProvinceID,CityID,LevelType,AuthType,Sign,OrderRemark,IsReserve,Status,Source,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,ForwardAvg,CommentAvg,LikeAvg,DirectPrice,ForwardPrice,TimestampSign");
            strSql.Append(") values (");
            strSql.Append("@Number,@Name,@Sex,@HeadIconURL,@FansCount,@FansCountURL,@FansSex,@CategoryID,@AreaID,@Profession,@ProvinceID,@CityID,@LevelType,@AuthType,@Sign,@OrderRemark,@IsReserve,@Status,@Source,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID,@ForwardAvg,@CommentAvg,@LikeAvg,@DirectPrice,@ForwardPrice,@TimestampSign");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@Number",entity.Number),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@Sex",entity.Sex),
                        new SqlParameter("@HeadIconURL",entity.HeadIconURL),
                        new SqlParameter("@FansCount",entity.FansCount),
                        new SqlParameter("@FansCountURL",entity.FansCountURL),
                        new SqlParameter("@FansSex",entity.FansSex),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@AreaID",entity.AreaID),
                        new SqlParameter("@Profession",entity.Profession),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@LevelType",entity.LevelType),
                        new SqlParameter("@AuthType",entity.AuthType),
                        new SqlParameter("@Sign",entity.Sign),
                        new SqlParameter("@OrderRemark",entity.OrderRemark),
                        new SqlParameter("@IsReserve",entity.IsReserve),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        new SqlParameter("@ForwardAvg",entity.ForwardAvg),
                        new SqlParameter("@CommentAvg",entity.CommentAvg),
                        new SqlParameter("@LikeAvg",entity.LikeAvg),
                        new SqlParameter("@DirectPrice",entity.DirectPrice),
                        new SqlParameter("@ForwardPrice",entity.ForwardPrice),
                        new SqlParameter("@TimestampSign",entity.TimestampSign),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);


        }

        public int UpdateOffer(int mediaId, int categoryId)
        {
            var sql = @"

                UPDATE DBO.LE_Weibo SET CategoryID = @CategoryID
                WHERE RecID = @RecID
                ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("@RecID",mediaId),
                new SqlParameter("@CategoryID",categoryId)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public Entities.LETask.LeWeibo GetInfo(int mediaId)
        {
            var sql = $@"
                    SELECT  WB.*
                    FROM    dbo.LE_Weibo AS WB WITH ( NOLOCK )
                    WHERE   WB.RecID = {mediaId};
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeWeibo>(obj.Tables[0]);
        }
    }
}

