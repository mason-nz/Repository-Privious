/********************************************************
*创建人：lixiong
*创建时间：2017/9/13 10:44:22
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Distribute
{
    public class MaterielInfo : DataBase
    {
        public static readonly MaterielInfo Instance = new MaterielInfo();

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Insert(Entities.Distribute.MaterielExtend entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into MaterielExtend(");
            strSql.Append("MaterielType,Resource,SceneID,Status,Memo,Title,Content,ImageUrl,ThirdID,ArticleFrom,HeadContentType,BodyContentType,ContractNumber,CreateUserID,CreateTime,LastUpdateTime,Name,ArticleID,HeadContentURL,FootContentURL,SerialID,Tag,Category,MaterielUrl");
            strSql.Append(") values (");
            strSql.Append("@MaterielType,@Resource,@SceneID,@Status,@Memo,@Title,@Content,@ImageUrl,@ThirdID,@ArticleFrom,@HeadContentType,@BodyContentType,@ContractNumber,@CreateUserID,@CreateTime,@LastUpdateTime,@Name,@ArticleID,@HeadContentURL,@FootContentURL,@SerialID,@Tag,@Category,@MaterielUrl");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MaterielType",entity.MaterielType),
                        new SqlParameter("@Resource",entity.Resource),
                        new SqlParameter("@SceneID",entity.SceneId),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@Memo",entity.Memo),
                        new SqlParameter("@Title",entity.Title),
                        new SqlParameter("@Content",entity.Content),
                        new SqlParameter("@ImageUrl",entity.ImageUrl),
                        new SqlParameter("@ThirdID",entity.ThirdId),
                        new SqlParameter("@ArticleFrom",entity.ArticleFrom),
                        new SqlParameter("@HeadContentType",entity.HeadContentType),
                        new SqlParameter("@BodyContentType",entity.BodyContentType),
                        new SqlParameter("@ContractNumber",entity.ContractNumber),
                        new SqlParameter("@CreateUserID",entity.CreateUserId),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@Name",entity.Name),
                        new SqlParameter("@ArticleID",entity.ArticleID),
                        new SqlParameter("@HeadContentURL",entity.HeadContentURL),
                        new SqlParameter("@FootContentURL",entity.FootContentURL),
                        new SqlParameter("@SerialID",entity.SerialId),
                        new SqlParameter("@Tag",entity.Tag),
                        new SqlParameter("@Category",entity.Category),
                        new SqlParameter("@MaterielUrl",entity.MaterielUrl),
                        };

            var obj = SqlHelper.ExecuteScalar(ConnectChitunionOp2017, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public Entities.Distribute.MaterielExtend GetMaterielInfo(int materielId)
        {
            var sql = @"

                    SELECT ME.MaterielID ,
                           ME.MaterielType ,
                           ME.SceneID ,
                           ME.Status ,
                           ME.Memo ,
                           ME.Title ,
                           ME.Content ,
                           ME.ImageUrl ,
                           ME.ThirdID ,
                           ME.ArticleFrom ,
                           ME.HeadContentType ,
                           ME.BodyContentType ,
                           ME.ContractNumber ,
                           ME.CreateUserID ,
                           ME.CreateTime ,
                           ME.LastUpdateTime ,
                           ME.Name ,
                           ME.ArticleID ,
                           ME.HeadContentURL ,
                           ME.FootContentURL ,
                           ME.SerialID ,
                           ME.Tag ,
                           ME.Category
                    FROM Chitunion_OP2017.DBO.MaterielExtend AS ME WITH(NOLOCK)
                    WHERE ME.MaterielID = @MaterielID
                    ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@MaterielID",materielId)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunionOp2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Distribute.MaterielExtend>(data.Tables[0]);
        }

        public Entities.Distribute.MaterielExtend GetMaterielInfoByBodyArticleId(int bodyArticleId)
        {
            var sql = @"

                    SELECT  ME.*
                    FROM    Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK)
                    WHERE   1 = 1
                            AND ME.MaterielType = 7004
                            AND ME.ArticleID = @ArticleID
                    ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ArticleID",bodyArticleId)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunionOp2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Distribute.MaterielExtend>(data.Tables[0]);
        }

        public Tuple<Entities.Distribute.MaterielTemp, Entities.Distribute.MaterielTemp>
            GetMaterielTemp(int headArticleId, int bodyArticleId)
        {
            var sql = @"
                    --获取车型id(腰部文章id)
                    SELECT  AC.CSID
                    FROM    NLP2017.dbo.TR_ArticleCarMapping AS AC WITH ( NOLOCK )
                    WHERE   AC.ArticleID = @ArticleID
                    --获取场景（头部文章id）
                    SELECT TOP 1
                            CA.SceneID ,
                            AI.Title,
                            AI.Resource,
                            AI.Content
                    FROM    BaseData2017.dbo.ArticleInfo AS AI WITH ( NOLOCK )
                            LEFT JOIN Chitunion_OP2017.dbo.ChannelAccount AS CA WITH ( NOLOCK ) ON CA.Number = AI.DataId
                    WHERE   RecID = @RecID
                    ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ArticleID",bodyArticleId),
                new SqlParameter("@RecID",headArticleId)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunionOp2017, CommandType.Text, sql, parameters.ToArray());

            return new Tuple<MaterielTemp, MaterielTemp>(DataTableToEntity<Entities.Distribute.MaterielTemp>(data.Tables[0]),
                DataTableToEntity<Entities.Distribute.MaterielTemp>(data.Tables[1]));
        }

        public List<Entities.Distribute.MaterielTemp> GetArticleInfo(List<int> ids)
        {
            var sql = @"
                        SELECT ai.RecID AS ArticleId,
                               ai.XyAttr ,
                               ai.Title ,
                               ai.ReadNum ,
                               ai.LikeNum
                        FROM BaseData2017.dbo.ArticleInfo AS ai WITH ( NOLOCK )
                        WHERE 1 =1
                        ";
            if (ids.Any())
            {
                sql += $"  AND ai.RecID IN ({string.Join(",", ids)})";
            }
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);

            return DataTableToList<Entities.Distribute.MaterielTemp>(data.Tables[0]);
        }

        public List<Entities.Distribute.MaterielTemp> GetArticleInfo(XyAttrTypeEnum xyAttrType, string date,
            int topPageSize = 100, int startId = 0)
        {
            var sql = $@"

                    SELECT  TOP {topPageSize}
                            ai.RecID AS ArticleId
                    FROM    NLP2017.dbo.TR_GroupInfo AS g WITH ( NOLOCK )
                            JOIN NLP2017.dbo.TR_ArticleInfo AS a WITH ( NOLOCK ) ON g.GroupID = a.GroupID
                            JOIN BaseData2017.dbo.ArticleInfo AS ai WITH ( NOLOCK ) ON ai.RecID = a.ArticleID
                            JOIN NLP2017.dbo.SubscribeTaskInfo AS sti WITH ( NOLOCK ) ON sti.TaskID = g.TaskID
                                                                         AND sti.TypeID = 1
                                                                         AND sti.Status = 1
                    WHERE   ai.XyAttr = {(int)xyAttrType}
                            AND sti.BeginTime = '{date}'
                    ";
            if (startId > 0)
            {
                sql += $" AND AI.RecID > {startId}";
            }
            var data = SqlHelper.ExecuteDataset(ConnectBaseData2017, CommandType.Text, sql);

            return DataTableToList<Entities.Distribute.MaterielTemp>(data.Tables[0]);
        }

        public List<Entities.Distribute.MaterielTemp> GetArticleInfo(DistributeQuery<Entities.Distribute.MaterielTemp> query)
        {
            const string storedProcedure = "p_SelectHeadArticle";
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@PageIndex",query.PageIndex),
                new SqlParameter("@PageSize",query.PageSize),
                new SqlParameter("@Date",query.Date),
            };

            var data = SqlHelper.ExecuteDataset(ConnectChitunionOp2017, CommandType.StoredProcedure, storedProcedure, sqlParams);
            query.Total = (int)(sqlParams[0].Value);
            return query.DataList = DataTableToList<Entities.Distribute.MaterielTemp>(data.Tables[0]);
        }
    }
}