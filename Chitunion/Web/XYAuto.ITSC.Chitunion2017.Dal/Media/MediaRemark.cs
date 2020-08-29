using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaRemark:DataBase
    {
        #region Instance
        public static readonly MediaRemark Instance = new MediaRemark();
        #endregion Instance

        public int Insert(Entities.Media.MediaRemarkBasic entity)
        {
            string sql = @"INSERT INTO dbo.Publish_Remark( RelationID ,RemarkID ,OtherContent ,EnumType ,CreateTime ,CreateUserID)
                                    VALUES (@RelationID ,@RemarkID ,@OtherContent ,@EnumType,@CreateTime,@CreateUserID);
                                    SELECT SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@RelationID", entity.RelationID),
                new SqlParameter("@RemarkID", entity.RemarkID),
                new SqlParameter("@OtherContent", entity.OtherContent),
                new SqlParameter("@EnumType", entity.EnumType),
                new SqlParameter("@CreateTime", entity.CreateTime),
                new SqlParameter("@CreateUserID", entity.CreateUserID),
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int InsertBasic(Entities.Media.MediaRemarkBasic entity)
        {
            string sql = @"INSERT INTO dbo.Media_Remark_Basic( RelationID ,RemarkID ,OtherContent ,EnumType ,CreateTime ,CreateUserID)
                                    VALUES (@RelationID ,@RemarkID ,@OtherContent ,@EnumType,@CreateTime,@CreateUserID);
                                    SELECT SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@RelationID", entity.RelationID),
                new SqlParameter("@RemarkID", entity.RemarkID),
                new SqlParameter("@OtherContent", entity.OtherContent),
                new SqlParameter("@EnumType", entity.EnumType),
                new SqlParameter("@CreateTime", entity.CreateTime),
                new SqlParameter("@CreateUserID", entity.CreateUserID),
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int DeleteBasic(int enumType, int relationID)
        {
            string sql = "DELETE FROM dbo.Media_Remark_Basic WHERE RelationID = " + relationID+" AND EnumType = "+enumType;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public int Delete(int enumType, int relationID)
        {
            string sql = "DELETE FROM dbo.Publish_Remark WHERE RelationID = "+relationID+" AND EnumType = " + enumType;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}
