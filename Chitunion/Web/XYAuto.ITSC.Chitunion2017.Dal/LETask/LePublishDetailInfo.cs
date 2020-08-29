using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //广告位
    public partial class LePublishDetailInfo : DataBase
    {


        public static readonly LePublishDetailInfo Instance = new LePublishDetailInfo();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LePublishDetailInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_PublishDetailInfo(");
            strSql.Append("MediaType,MediaID,ADPosition1,ADPosition2,ADPosition3,Price,PublishStatus,CreateTime,CreateUserID");
            strSql.Append(") values (");
            strSql.Append("@MediaType,@MediaID,@ADPosition1,@ADPosition2,@ADPosition3,@Price,@PublishStatus,@CreateTime,@CreateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@ADPosition1",entity.ADPosition1),
                        new SqlParameter("@ADPosition2",entity.ADPosition2),
                        new SqlParameter("@ADPosition3",entity.ADPosition3),
                        new SqlParameter("@Price",entity.Price),
                        new SqlParameter("@PublishStatus",entity.PublishStatus),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int Insert(List<Entities.LETask.LePublishDetailInfo> list)
        {
            var strSql = new StringBuilder();

            strSql.Append($@" DELETE FROM dbo.LE_PublishDetailInfo WHERE MediaType = {list[0].MediaType} AND MediaID = {list[0].MediaID}");
            strSql.Append("insert into LE_PublishDetailInfo(");
            strSql.Append("MediaType,MediaID,ADPosition1,ADPosition2,ADPosition3,Price,PublishStatus,CreateTime,CreateUserID");
            strSql.Append(") values ");

            list.ForEach(s =>
            {
                strSql.AppendLine();
                strSql.Append($"({s.MediaType},{s.MediaID},{s.ADPosition1},{s.ADPosition2},{s.ADPosition3},{s.Price},{s.PublishStatus},getdate(),{s.CreateUserID}),");
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString().TrimEnd(','));

        }

    }
}

