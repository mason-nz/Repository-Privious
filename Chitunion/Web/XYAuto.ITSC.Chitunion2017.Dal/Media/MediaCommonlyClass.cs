using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Media
{
    public class MediaCommonlyClass : DataBase
    {
        #region Instance

        public static readonly MediaCommonlyClass Instance = new MediaCommonlyClass();

        #endregion Instance

        public int InsertByBulk(Entities.Media.MediaCommonlyClass entity, int weightId)
        {
            var sql = @"
                                DELETE FROM DBO.Media_CommonlyClass
                                WHERE MediaID = {0} AND MediaType = {1}

                                    INSERT  INTO dbo.Media_CommonlyClass
                                    ( MediaID ,
                                      MediaType ,
                                      CategoryID ,
                                      SortNumber,
                                      CreateTime ,
                                      CreateUserID
                                    )
                                    SELECT  {0} ,
                                            {1} ,
                                            DictId ,
                                            ( CASE WHEN DictId = {4} THEN 1
                                                   ELSE 0
                                              END ) AS SortNumber,
                                            GETDATE() ,
                                            {2}
                                    FROM    dbo.DictInfo WITH ( NOLOCK )
                                    WHERE   DictId IN ( {3} )
                                            AND Status = 0 ";

            sql = string.Format(sql, entity.MediaID, entity.MediaType, entity.CreateUserID,
                string.Join(",", entity.CategoryIDs), weightId);
            var parameters = new SqlParameter();
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public int Insert(Entities.Media.MediaCommonlyClass entity)
        {
            string sql = @"INSERT dbo.Media_CommonlyClass( MediaID ,MediaType ,CategoryID ,SortNumber ,CreateTime ,CreateUserID)
                                    VALUES(@MediaID ,@MediaType ,@CategoryID ,@SortNumber ,@CreateTime ,@CreateUserID);
                                    SELECT SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaID", entity.MediaID),
                new SqlParameter("@MediaType", entity.MediaType),
                new SqlParameter("@CategoryID", entity.CategoryID),
                new SqlParameter("@SortNumber", entity.SortNumber),
                new SqlParameter("@CreateTime", entity.CreateTime),
                new SqlParameter("@CreateUserID", entity.CreateUserID)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int InsertBasic(Entities.Media.MediaCommonlyClass entity)
        {
            string sql = @"INSERT INTO dbo.MediaCategory( MediaType ,WxID ,CategoryID ,SortNumber)
                                    VALUES (@MediaType ,@WxID ,@CategoryID ,@SortNumber);
                                    SELECT SCOPE_IDENTITY()";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MediaType", entity.MediaType),
                new SqlParameter("@WxID", entity.MediaID),
                new SqlParameter("@CategoryID", entity.CategoryID),
                new SqlParameter("@SortNumber", entity.SortNumber),
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int DeleteBasic(int mediaType, int mediaID)
        {
            string sql = "DELETE FROM dbo.MediaCategory WHERE MediaType = "+mediaType+" AND WxID = " + mediaID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public int Delete(int mediaType, int mediaID)
        {
            string sql = "DELETE FROM dbo.Media_CommonlyClass WHERE MediaType = " + mediaType + " AND MediaID = " + mediaID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// <summary>
        /// 微信授权表主表 - 常见分类
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="weightId">权重Id,重要的分类</param>
        /// <returns></returns>
        public int InsertMediaCategoryByBulk(Entities.Media.MediaCommonlyClass entity, int weightId)
        {
            var sql = @"
                                DELETE FROM DBO.MediaCategory
                                WHERE WxID = {0} AND MediaType = {1}

                                    INSERT  INTO dbo.MediaCategory
                                    ( WxID,MediaType, CategoryID,SortNumber)
                                    SELECT  {0} ,
                                            {1} ,
                                            DictId,
                                            ( CASE WHEN DictId = {3} THEN 1
                                                   ELSE 0
                                              END ) AS SortNumber
                                    FROM    dbo.DictInfo WITH ( NOLOCK )
                                    WHERE   DictId IN ( {2} )
                                            AND Status = 0 ";

            sql = string.Format(sql, entity.MediaID, entity.MediaType,
                string.Join(",", entity.CategoryIDs), weightId);
            var parameters = new SqlParameter();
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }

        public List<Entities.Media.MediaCommonlyClass> GetList(MediaQuery<Entities.Media.MediaCommonlyClass> query)
        {
            var sql = @"SELECT  RecID ,
                                MediaID ,
                                MediaType ,SortNumber,
                                CategoryID ,
                                CreateTime ,
                                CreateUserID
                        FROM    dbo.Media_CommonlyClass WITH ( NOLOCK )
                        WHERE   1= 1 ";
            var paras = new List<SqlParameter>();

            if (query.MediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MediaId = @MediaId";
                paras.Add(new SqlParameter("@MediaId", query.MediaId));
            }

            if (query.MediaType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MediaType = @MediaType";
                paras.Add(new SqlParameter("@MediaType", query.MediaType));
            }

            sql += " ORDER BY SortNumber DESC";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Media.MediaCommonlyClass>(data.Tables[0]);
        }

        public List<Entities.Media.MediaCommonlyClass> GetListByMediaCategory(MediaQuery<Entities.Media.MediaCommonlyClass> query)
        {
            var sql = @"SELECT  MC.RecID ,
                                MC.MediaType ,
                                MC.WxID ,
                                MC.SortNumber,
                                MC.CategoryID
                        FROM    dbo.MediaCategory AS MC WITH ( NOLOCK )
                        WHERE   1 = 1
                        ";
            var paras = new List<SqlParameter>();

            if (query.WxId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MC.WxID = @WxID ";
                paras.Add(new SqlParameter("@WxID", query.WxId));
            }

            if (query.MediaType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND MC.MediaType = @MediaType";
                paras.Add(new SqlParameter("@MediaType", query.MediaType));
            }
            sql += " ORDER BY SortNumber DESC";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.Media.MediaCommonlyClass>(data.Tables[0]);
        }
    }
}