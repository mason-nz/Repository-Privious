using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{

    //媒体——覆盖区域信息
    public partial class LeMediaAreaMapping : DataBase
    {


        public static readonly LeMediaAreaMapping Instance = new LeMediaAreaMapping();


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeMediaAreaMapping entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_MediaArea_Mapping(");
            strSql.Append("MediaType,MediaID,ProvinceID,CityID,CreateTime,CreateUserID,RelateType");
            strSql.Append(") values (");
            strSql.Append("@MediaType,@MediaID,@ProvinceID,@CityID,@CreateTime,@CreateUserID,@RelateType");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@RelateType",entity.RelateType),
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);

        }

        public int Update(Entities.LETask.LeMediaAreaMapping entity)
        {
            var strSql = new StringBuilder();

            strSql.Append($@"
                DELETE FROM [dbo].[LE_MediaArea_Mapping]
                 WHERE MediaID = {entity.MediaID} AND MediaType = {entity.MediaType} AND [RelateType] = {entity.RelateType}
            ");
            strSql.Append("insert into LE_MediaArea_Mapping(");
            strSql.Append("MediaType,MediaID,ProvinceID,CityID,CreateTime,CreateUserID,RelateType");
            strSql.Append(") values (");
            strSql.Append("@MediaType,@MediaID,@ProvinceID,@CityID,@CreateTime,@CreateUserID,@RelateType");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@RelateType",entity.RelateType),
                        };


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }
    }
}

