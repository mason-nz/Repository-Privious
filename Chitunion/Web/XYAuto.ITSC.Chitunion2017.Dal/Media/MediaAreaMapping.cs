using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaAreaMapping : DataBase
    {
        #region Instance

        public static readonly MediaAreaMapping Instance = new MediaAreaMapping();

        #endregion Instance

        public int Insert(Entities.Media.MediaAreaMapping entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Media_Area_Mapping(");
            strSql.Append("MediaType,MediaID,ProvinceID,CityID,RelateType,CreateTime,CreateUserID");
            strSql.Append(") values (");
            strSql.Append("@MediaType,@MediaID,@ProvinceID,@CityID,@RelateType,@CreateTime,@CreateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@ProvinceID",entity.ProvinceID),
                        new SqlParameter("@CityID",entity.CityID),
                        new SqlParameter("@RelateType",entity.RelateType),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        };
            if (parameters[3].Value.Equals(0))
                parameters[3].Value = null;
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int InsertBasic(Entities.Media.MediaAreaMappingBase entity)
        {
            string sql = @"INSERT INTO dbo.Media_Area_Mapping_Basic(MediaType, BaseMediaID, ProvinceID, CityID, RelateType, CreateTime, CreateUserID) 
                                    VALUES (@MediaType, @BaseMediaID, @ProvinceID, @CityID, @RelateType, @CreateTime, @CreateUserID);
                                    SELECT SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@MediaType", entity.MediaType),
                new SqlParameter("@BaseMediaID", entity.BaseMediaID),
                new SqlParameter("@ProvinceID", entity.ProvinceID),
                new SqlParameter("@CityID", entity.CityID),
                new SqlParameter("@RelateType", entity.RelateType),
                new SqlParameter("@CreateTime", entity.CreateTime),
                new SqlParameter("@CreateUserID", entity.CreateUserID)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Delete(int mediaId, int mediaType, int relateType = -2)
        {
            var strSql = new StringBuilder();
            strSql.Append("DELETE FROM DBO.Media_Area_Mapping WHERE MediaType = @MediaType AND MediaID = @MediaID");
            if (!relateType.Equals(-2))
            {
                strSql.Append(" AND  RelateType =" + relateType);
            }
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",mediaType),
                        new SqlParameter("@MediaID",mediaId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public int DeleteBase(int mediaId, int mediaType, int relateType = -2)
        {
            var strSql = new StringBuilder();
            strSql.Append("DELETE FROM DBO.Media_Area_Mapping_Basic WHERE MediaType = @MediaType AND BaseMediaID = @BaseMediaID");
            if (!relateType.Equals(-2))
            {
                strSql.Append(" AND  RelateType =" + relateType);
            }
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MediaType",mediaType),
                        new SqlParameter("@BaseMediaID",mediaId),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        public void Insert_BulkCopyToDB(DataTable dt, MediaRelationType mediaRelationType)
        {
            if (dt.Rows.Count <= 0) return;
            if (mediaRelationType == MediaRelationType.Attached)
            {
                Delete(Convert.ToInt32(dt.Rows[0]["MediaID"]), Convert.ToInt32(dt.Rows[0]["MediaType"]));
                SqlBulkCopyByDatatable(CONNECTIONSTRINGS, "Media_Area_Mapping", dt);
            }
            else
            {
                DeleteBase(Convert.ToInt32(dt.Rows[0]["BaseMediaID"]), Convert.ToInt32(dt.Rows[0]["MediaType"]));
                Insert_Base_BulkCopyToDB(dt);
            }
        }

        private void Insert_Base_BulkCopyToDB(DataTable dt)
        {
            SqlBulkCopyByDatatable(CONNECTIONSTRINGS, "Media_Area_Mapping_Basic", dt);
        }

        /// <summary>
        /// 将基表的相关数据复制到附表
        /// </summary>
        /// <param name="entityAttach"></param>
        /// <param name="orderRemarkType">枚举类型 45001：刊例，45002：微信，45003：App备注</param>
        public void CopyBaseMediaToAttachTable(CopyBaseMediaToAttach entityAttach, int orderRemarkType)
        {
            var sqlCommonlyClass = string.Format(@"INSERT INTO DBO.Media_CommonlyClass
                                            (MediaID,
                                              MediaType,
                                              CategoryID,
                                              SortNumber,
                                              CreateTime,
                                              CreateUserID
                                            )
                                    SELECT {1},MC.MediaType,MC.CategoryID,MC.SortNumber,GETDATE(),{3}
                                    FROM DBO.MediaCategory AS MC WITH(NOLOCK)
                                    WHERE MC.MediaType = {2}
                                    AND MC.WxID = {0}
                                    ", entityAttach.BaseMediaId, entityAttach.MediaId, (int)entityAttach.MediaType, entityAttach.CreateUserId);
            var sqlAreaMapping = string.Format(@"INSERT INTO DBO.Media_Area_Mapping
                                        ( MediaType ,
                                          MediaID ,
                                          ProvinceID ,
                                          CityID ,
                                          RelateType,
                                          CreateTime ,
                                          CreateUserID
                                        )
                                SELECT MAMB.MediaType,{1},MAMB.ProvinceID,MAMB.CityID,RelateType,GETDATE(),{3}
                                FROM DBO.Media_Area_Mapping_Basic AS MAMB WITH(NOLOCK)
                                WHERE MAMB.MediaType ={2}
                                AND MAMB.BaseMediaID = {0}",
                                entityAttach.BaseMediaId, entityAttach.MediaId, (int)entityAttach.MediaType, entityAttach.CreateUserId);
            var sqlOrderRemark = string.Format(@"INSERT INTO DBO.Publish_Remark
                                        ( RelationID ,
                                          RemarkID ,
                                          OtherContent ,
                                          EnumType ,
                                          CreateTime ,
                                          CreateUserID
                                        )
                                SELECT {1},PRKB.RemarkID,PRKB.OtherContent,PRKB.EnumType,GETDATE(),{3}
                                FROM dbo.Media_Remark_Basic  AS PRKB WITH ( NOLOCK )
                                WHERE PRKB.EnumType = {2}
                                AND PRKB.RelationID = {0}",
                    entityAttach.BaseMediaId, entityAttach.MediaId, orderRemarkType, entityAttach.CreateUserId);

            var sql = string.Format("{0}{1}{0}{2}{3}", System.Environment.NewLine, sqlCommonlyClass, sqlAreaMapping, sqlOrderRemark);
            var parameters = new SqlParameter[] { };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
    }

    public class CopyBaseMediaToAttach
    {
        public MediaType MediaType { get; set; }

        public int BaseMediaId { get; set; }
        public int MediaId { get; set; }
        public int CreateUserId { get; set; }
    }
}