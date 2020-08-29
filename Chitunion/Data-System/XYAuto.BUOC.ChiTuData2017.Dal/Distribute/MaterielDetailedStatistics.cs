using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Distribute
{
    //物料详情明细表（分发明细表 下面的详情，以天为单位）
    public partial class MaterielDetailedStatistics : DataBase
    {
        public static readonly MaterielDetailedStatistics Instance = new MaterielDetailedStatistics();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Distribute.MaterielDetailedStatistics entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Materiel_DetailedStatistics(");
            strSql.Append("MaterielId,DistributeId,ArticleId,ConentType,ArticleType,ReadNumber,CreateUserId,PV,UV,Status,CreateTime,LikeNumber,ForwardNumber,ClickNumber,Url,ClickPV,ClickUV,CustomType,CustomLocation");
            strSql.Append(") values (");
            strSql.Append("@MaterielId,@DistributeId,@ArticleId,@ConentType,@ArticleType,@ReadNumber,@CreateUserId,@PV,@UV,@Status,@CreateTime,@LikeNumber,@ForwardNumber,@ClickNumber,@Url,@ClickPV,@ClickUV,@CustomType,@CustomLocation");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MaterielId",entity.MaterielId),
                        new SqlParameter("@DistributeId",entity.DistributeId),
                        new SqlParameter("@ArticleId",entity.ArticleId),
                        new SqlParameter("@ConentType",entity.ConentType),
                        new SqlParameter("@ArticleType",entity.ArticleType),
                        new SqlParameter("@ReadNumber",entity.ReadNumber),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@PV",entity.PV),
                        new SqlParameter("@UV",entity.UV),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@LikeNumber",entity.LikeNumber),
                        new SqlParameter("@ForwardNumber",entity.ForwardNumber),
                        new SqlParameter("@ClickNumber",entity.ClickNumber),
                        new SqlParameter("@Url",entity.Url),
                        new SqlParameter("@ClickPV",entity.ClickPV),
                        new SqlParameter("@ClickUV",entity.ClickUV),
                        new SqlParameter("@CustomType",entity.CustomType),
                        new SqlParameter("@CustomLocation",entity.CustomLocation),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int Insert(List<Entities.Distribute.MaterielDetailedStatistics> list, int distributeId, DateTime crDateTime)
        {
            if (!list.Any())
            {
                return 1;
            }

            var sqlCode = new StringBuilder();

            sqlCode.AppendFormat(@"

                    INSERT INTO dbo.Materiel_DetailedStatistics
                            ( MaterielId ,
                              DistributeId ,
                              ArticleId ,
                              ConentType ,
                              ArticleType ,
                              ReadNumber ,
                              CreateUserId ,
                              PV ,
                              UV ,
                              Status ,
                              CreateTime ,
                              LikeNumber ,
                              ForwardNumber ,
                              ClickNumber ,
                              Url ,
                              ClickPV ,
                              ClickUV ,
                              CustomType ,
                              CustomLocation
                            )
                    VALUES
");
            list.ForEach(s =>
            {
                var readNum = s.ReadNumber;//s.ConentType == (int)MaterielConentTypeEnum.Foot ? s.ReadNumber : -1;
                sqlCode.AppendFormat($@"({s.MaterielId},{distributeId},{s.ArticleId},{s.ConentType},{s.ArticleType},{readNum},
                                        {s.CreateUserId},{s.PV},{s.UV},{s.Status},'{DateTime.Now}',{s.LikeNumber},{s.ForwardNumber},{s.ClickNumber},
                                        '{s.Url}',{s.ClickPV},{s.UV},{s.CustomType},{s.CustomLocation}),");
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlCode.ToString().Trim(','));
        }

        public List<Entities.Distribute.MaterielDetailedStatistics> GetList(int distributeId, List<int> distributeIds)
        {
            var sql = @"
                        SELECT  DS.* ,
                                ISNULL(AI.Title,'') as Title,
                                DI.DictName AS ConentTypeName ,
                                DIOP.DictName AS ArticleTypeName
                        FROM    dbo.Materiel_DetailedStatistics AS DS WITH ( NOLOCK )
                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON DI.DictId = DS.ConentType
                                LEFT JOIN dbo.DictInfo AS DIOP WITH ( NOLOCK ) ON DIOP.DictId = DS.ArticleType
                                LEFT JOIN BaseData2017.dbo.ArticleInfo AS AI WITH ( NOLOCK ) ON AI.RecID = DS.ArticleId
                        WHERE   DS.Status = 0
                        ";
            var parameters = new List<SqlParameter>();
            if (distributeId > 0)
            {
                sql += $" AND DS.DistributeId = @DistributeId";
                parameters.Add(new SqlParameter("@DistributeId", distributeId));
            }

            if (distributeIds.Any())
            {
                sql += $" AND DS.DistributeId IN ({string.Join(",", distributeIds)})";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Distribute.MaterielDetailedStatistics>(data.Tables[0]);
        }

        public List<Entities.Distribute.MaterielDetailedStatistics> GetList(int distributeId, string sqldistributeIds)
        {
            var sql = sqldistributeIds.Replace("T.* YanFa", "T.DistributeId AS StatisticsId ,T.MaterielId,T.DistributeId, B.ArticleId, B.ConentType, B.ArticleType, B.ReadNumber, B.CreateUserId,B.PV, B.UV, B.Status, B.CreateTime, B.LikeNumber, B.ForwardNumber, B.ClickNumber, B.Url,B.ClickPV, B.ClickUV, B.CustomType, B.CustomLocation, B.Title, B.ConentTypeName, B.ArticleTypeName ");
             sql += @"
                        LEFT JOIN (SELECT  DS.* ,
                                ISNULL(AI.Title,'') as Title,
                                DI.DictName AS ConentTypeName ,
                                DIOP.DictName AS ArticleTypeName
                        FROM    dbo.Materiel_DetailedStatistics AS DS WITH ( NOLOCK )
                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON DI.DictId = DS.ConentType
                                LEFT JOIN dbo.DictInfo AS DIOP WITH ( NOLOCK ) ON DIOP.DictId = DS.ArticleType
                                LEFT JOIN BaseData2017.dbo.ArticleInfo AS AI WITH ( NOLOCK ) ON AI.RecID = DS.ArticleId
                        WHERE   DS.Status = 0
                        ";
            var parameters = new List<SqlParameter>();
            if (distributeId > 0)
            {
                sql += $" AND DS.DistributeId = @DistributeId";
                parameters.Add(new SqlParameter("@DistributeId", distributeId));
            }
            sql += " )  B ON T.DistributeId = B.DistributeId ORDER BY DistributeId";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Distribute.MaterielDetailedStatistics>(data.Tables[0]);
        }

        public List<Entities.Distribute.MaterielDetailedStatistics> GetStatisticsDetailedsList(DistributeQuery<Entities.Distribute.MaterielDetailedStatistics> query)
        {
            var sql = @"
                        SELECT  DD.Date ,
                                DS.*,
                                AI.Title ,
                                DI.DictName AS ConentTypeName ,
                                DIOP.DictName AS ArticleTypeName
                        FROM    dbo.Materiel_DistributeDetailed AS DD WITH ( NOLOCK )
                                LEFT JOIN dbo.Materiel_DetailedStatistics AS DS WITH ( NOLOCK ) ON DS.DistributeId = DD.DistributeId
                                LEFT JOIN dbo.DictInfo AS DI WITH ( NOLOCK ) ON DI.DictId = DS.ConentType
                                LEFT JOIN dbo.DictInfo AS DIOP WITH ( NOLOCK ) ON DIOP.DictId = DS.ArticleType
                                LEFT JOIN BaseData2017.dbo.ArticleInfo AS AI WITH ( NOLOCK ) ON AI.RecID = DS.ArticleId
                        WHERE   DD.Status = 0 AND DS.StatisticsId > 0
                        ";
            var parameters = new List<SqlParameter>();
            if (query.MaterielId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.MaterielId = @MaterielId";
                parameters.Add(new SqlParameter("@MaterielId", query.MaterielId));
            }
            if (query.DistributeId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.DistributeId = @DistributeId";
                parameters.Add(new SqlParameter("@DistributeId", query.DistributeId));
            }
            if (!string.IsNullOrWhiteSpace(query.StartDate))
            {
                sql += $" AND DD.Date >= @StartDate";
                parameters.Add(new SqlParameter("@StartDate", query.StartDate));
            }
            if (!string.IsNullOrWhiteSpace(query.EndDate))
            {
                sql += $" AND DD.Date <= @EndDate";
                parameters.Add(new SqlParameter("@EndDate", query.EndDate));
            }
            if (query.Source != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.Source <= @Source";
                parameters.Add(new SqlParameter("@Source", query.Source));
            }

            sql += " ORDER BY DD.Date ASC";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Distribute.MaterielDetailedStatistics>(data.Tables[0]);
        }
    }
}