/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 15:16:59
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
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Materiel
{
    public class TaskSchedulerUser : DataBase
    {
        public static readonly TaskSchedulerUser Instance = new TaskSchedulerUser();

        public int Insert(Entities.Materiel.TaskSchedulerUser entity)
        {
            var sql = @"
                    INSERT INTO [dbo].[TaskScheduler_User]
                               ([UserId]
                               ,[GroupId]
                               ,[TaskStatus]
                               ,[CreateTime]
                               ,[CreateUserId])
                         VALUES
                               (@UserId
                               ,@GroupId
                               ,@TaskStatus
                               ,getdate()
                               ,@CreateUserId)
                    ";
            var parameters = new SqlParameter[]{
                        new SqlParameter("@UserId",entity.UserId),
                        new SqlParameter("@GroupId",entity.GroupId),
                        new SqlParameter("@TaskStatus",(int)TaskStatusEnum.Already),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return obj == null ? 0 :
                Convert.ToInt32(obj);
        }

        public int Insert(List<string> groupIds, Entities.Materiel.TaskSchedulerUser entity)
        {
            if (!groupIds.Any())
                return 1;
            var sbSql = new StringBuilder();
            sbSql.AppendFormat(@"
                        INSERT INTO [dbo].[TaskScheduler_User]
                               ([UserId]
                               ,[GroupId]
                               ,[TaskStatus]
                               ,[CreateTime]
                               ,[CreateUserId])
                        VALUES
                            ");
            groupIds.ForEach(item =>
            {
                sbSql.AppendFormat(@"({0},{1},{2},getdate(),{3}),",
                    entity.UserId, item, (int)TaskStatusEnum.Already, entity.CreateUserId);
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString().Trim(','));
        }

        public int InsertGroupArticle(int groupId, int taskId, List<int> articleIds, List<int> deleteList, int xyAttr = 2)
        {
            if (!articleIds.Any())
                return 1;
            var sbSql = new StringBuilder();

            if (deleteList.Any())
            {
                sbSql.AppendFormat($@"
                        UPDATE NLP2017.dbo.TR_ArticleInfo SET Status = -1
                        WHERE XyAttr= 2 AND GroupID = {groupId} AND ArticleID IN ({string.Join(",", deleteList)}) AND Status = 0
                        ");
            }
            sbSql.AppendLine();
            sbSql.AppendFormat(@"
                    INSERT INTO NLP2017.DBO.TR_ArticleInfo
                            ( TaskID ,
                              GroupID ,
                              ArticleID ,
                              XyAttr ,
                              CreateTime ,
                              ArticleTagID
                            )
                    VALUES
                    ");
            articleIds.ForEach(item =>
            {
                sbSql.AppendFormat(@"({0},{1},{2},{3},getdate(),-2),",
                    taskId, groupId, item, xyAttr);
            });

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString().Trim(','));
        }

        public int UpdateStatusByRecovery(List<string> groupIds, TaskStatusEnum taskStatus, TaskStatusEnum whereTaskStatus)
        {
            var sql = string.Format(@"UPDATE TaskScheduler_User SET TaskStatus = {1}
                                        WHERE GroupId IN ({0}) AND TaskStatus = {2} AND Status = 0",
                                        string.Join(",", groupIds), (int)taskStatus, (int)whereTaskStatus);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public int UpdateStatusByRecovery(List<string> groupIds)
        {
            var sql = string.Format(@"DELETE FROM  TaskScheduler_User
                                        WHERE GroupId IN ({0}) AND TaskStatus = {1}",
                                        string.Join(",", groupIds), (int)TaskStatusEnum.Already);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public int UpdateTrArticleInfoStatus(int groupId)
        {
            var sql = string.Format(@"  UPDATE NLP2017.dbo.TR_ArticleInfo SET Status = -1
                                        WHERE GroupId ={0} AND Status = 0",
                                        groupId);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public Entities.Materiel.TrGroupInfo GeTrGroupInfo(int groupId)
        {
            var sql = @"
                        SELECT g.* FROM NLP2017.dbo.TR_GroupInfo AS g WHERE g.GroupID = @GroupID
                        ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@GroupID",groupId)
            };
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());

            return DataTableToEntity<Entities.Materiel.TrGroupInfo>(ds.Tables[0]);
        }

        public Entities.Materiel.TaskSchedulerUser GetNextGroupIdByTaskUser(int userId, int groupId)
        {
            var sql = $@"
                        SELECT  TSU.*
                        FROM    [dbo].[TaskScheduler_User] AS TSU WITH ( NOLOCK )
                        WHERE   TSU.Status = 0
                                AND TSU.UserId = {userId}
                                AND TSU.GroupId <> {groupId}
                                AND TSU.TaskStatus IN ({ (int)TaskStatusEnum.Already},{(int)TaskStatusEnum.Processing})
                        ORDER BY TSU.GroupId ASC
                        ";
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);

            return DataTableToEntity<Entities.Materiel.TaskSchedulerUser>(ds.Tables[0]);
        }

        public List<Entities.Materiel.TaskSchedulerUser> GetListByGroupId(int groupId, List<string> groupIds)
        {
            var sql = @"SELECT TSU.* FROM [dbo].[TaskScheduler_User] AS TSU WITH(NOLOCK)
                        WHERE TSU.Status = 0";
            var parameters = new List<SqlParameter>()
            {
            };

            if (groupId > 0)
            {
                sql += "  AND TSU.GroupId = @GroupId";
                parameters.Add(new SqlParameter("@GroupId", groupId));
            }
            if (groupIds.Any())
            {
                sql += $"  AND TSU.GroupId IN ({ string.Join(",", groupIds)})";
            }

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());

            return DataTableToList<Entities.Materiel.TaskSchedulerUser>(ds.Tables[0]);
        }

        public List<Entities.Materiel.TaskSchedulerDto> GetTaskListByGroupId(int groupId, bool isSee)
        {
            var sql = @"

                    SELECT  a.RecID ,
                            a.ArticleID ,
                            g.GroupID ,
                            g.CarBrandID ,
                            g.CSID AS SerialId ,
                            a.XyAttr ,
                            ai.Url ,
                            REPLACE(ai.Title, CHAR(10), '') AS Title ,
                            ai.HeadImg ,
                            ai.Content,
                            REPLACE(ai.Abstract, CHAR(10), '') AS Abstract ,
                            ai.CopyrightState ,
                            ai.Resource ,
                            ai.Category ,
                            CS.ShowName AS SerialName ,
                            CB.Name AS BrandName,
                            DC.DictId AS TaskStatus ,
                            DC.DictName AS TaskStatusName
                    FROM    NLP2017.dbo.TR_GroupInfo AS g
                            INNER JOIN NLP2017.dbo.TR_ArticleInfo AS a ON g.GroupID = a.GroupID
                            INNER JOIN BaseData2017.dbo.ArticleInfo AS ai ON ai.RecID = a.ArticleID
                            LEFT JOIN BaseData2017.dbo.CarSerial AS CS WITH ( NOLOCK ) ON CS.SerialID = g.CSID
                            LEFT JOIN BaseData2017.dbo.CarBrand AS CB WITH ( NOLOCK ) ON CB.BrandID = g.CarBrandID
                            LEFT JOIN dbo.TaskScheduler_User AS TSU WITH ( NOLOCK ) ON TSU.GroupId = a.GroupID
		                    LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = TSU.TaskStatus
                    WHERE   g.GroupID = @GroupId
                            AND a.Status = 0

                    ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@GroupId", groupId)
            };
            if (!isSee)
            {
                sql += $"{System.Environment.NewLine}";
                sql += @"--修改状态
                    UPDATE TaskScheduler_User SET TaskStatus = @TaskStatus WHERE GroupId = @GroupId1 AND Status = 0";
                parameters.Add(new SqlParameter("@GroupId1", groupId));
                parameters.Add(new SqlParameter("@TaskStatus", (int)TaskStatusEnum.Processing));
            }

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());

            return DataTableToList<Entities.Materiel.TaskSchedulerDto>(ds.Tables[0]);
        }

        public List<Entities.Materiel.TrGroupArticle> GetGroupArticleList(int groupId, List<int> deleteArticleId)
        {
            //删除腰部文章关联
            var sql = string.Empty;
            if (deleteArticleId.Any())
            {
                sql += $@"
                        UPDATE NLP2017.dbo.TR_ArticleInfo SET Status = -1
                        WHERE XyAttr= 2 AND GroupID = {groupId} AND ArticleID IN ({string.Join(",", deleteArticleId)}) AND Status = 0
                        ";
            }
            sql += $"{System.Environment.NewLine}";
            sql += $" SELECT a.GroupID,a.ArticleID,a.XyAttr FROM NLP2017.dbo.TR_ArticleInfo AS a WITH(NOLOCK)" +
                   $" WHERE a.GroupID = {groupId} AND a.Status = 0";
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);

            return DataTableToList<Entities.Materiel.TrGroupArticle>(ds.Tables[0]);
        }

        public int UpdateArticleInfo(Entities.Materiel.ArticleInfo entity)
        {
            var sql = @"
                        UPDATE  BaseData2017.dbo.ArticleInfo
                        SET     Title = @Title ,
                                Content = @Content ,
                                Abstract = @Abstract ,
                                LastUpdateTime = GETDATE()
                        WHERE   RecID = @RecID
                    ";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@RecID",entity.Abstract),
                    new SqlParameter("@Title",entity.Title),
                        new SqlParameter("@Content",entity.Content),
                            new SqlParameter("@Abstract",entity.Abstract)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
        }

        public int UpdateArticleInfo(List<Entities.Materiel.ArticleInfo> list, int groupId)
        {
            if (!list.Any())
                return 1;
            var sbSql = new StringBuilder();
            list.ForEach(item =>
            {
                sbSql.Append($@"
                        UPDATE  BaseData2017.dbo.ArticleInfo
                        SET     Title = '{item.Title}' ,
                                Content = '{item.Content}' ,
                                JsonContent = '{item.JsonContent}',
                                Abstract = '{item.Abstract}' ,
                                LastUpdateTime = GETDATE()
                        WHERE   RecID = {item.ArticleId}
                    ");
                sbSql.AppendLine();
            });

            sbSql.AppendFormat(@"
                    --修改状态:处理完成
                    UPDATE TaskScheduler_User SET TaskStatus = {0} WHERE GroupId = {1} AND Status = 0",
                        (int)TaskStatusEnum.AlreadyProcessed, groupId);

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
        }
    }
}