using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Interaction
{
    //媒体-互动参数信息表-微博，PC、APP没有互动参数设置
    public class InteractionWeibo : DataBase
    {
        #region Instance
        public static readonly InteractionWeibo Instance = new InteractionWeibo();
        #endregion

        public Entities.Interaction.InteractionWeibo GetEntity(int mediaId)
        {
            const string sql = @"SELECT [RecID]
                              ,[MeidaType]
                              ,[MediaID]
                              ,[AverageForwardCount]
                              ,[AverageCommentCount]
                              ,[AveragePointCount]
                              ,[ScreenShotURL]
                              ,[CreateTime]
                              ,[CreateUserID]
                              ,[LastUpdateTime]
                              ,[LastUpdateUserID]
                          FROM [dbo].[Interaction_Weibo] WITH(NOLOCK) WHERE MediaID = @MediaID";
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Interaction.InteractionWeibo>(data.Tables[0]);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Interaction.InteractionWeibo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Interaction_Weibo(");
            strSql.Append("MeidaType,MediaID,AverageForwardCount,AverageCommentCount,AveragePointCount,ScreenShotURL,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID");
            strSql.Append(") values (");
            strSql.Append("@MeidaType,@MediaID,@AverageForwardCount,@AverageCommentCount,@AveragePointCount,@ScreenShotURL,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
						new SqlParameter("@MeidaType",entity.MeidaType),
            			new SqlParameter("@MediaID",entity.MediaID),
            			new SqlParameter("@AverageForwardCount",entity.AverageForwardCount),
            			new SqlParameter("@AverageCommentCount",entity.AverageCommentCount),
            			new SqlParameter("@AveragePointCount",entity.AveragePointCount),
            			new SqlParameter("@ScreenShotURL",entity.ScreenShotURL),
            			new SqlParameter("@CreateTime",entity.CreateTime),
            			new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Interaction.InteractionWeibo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Interaction_Weibo]");
            strSql.Append(@"SET [MeidaType] = @MeidaType
                              ,[MediaID] = @MediaID
                              ,[AverageForwardCount] = @AverageForwardCount
                              ,[AverageCommentCount] = @AverageCommentCount
                              ,[AveragePointCount] = @AveragePointCount
                              ,[ScreenShotURL] =@ScreenShotURL
                              --,[CreateTime] = @CreateTime
                              --,[CreateUserID] = @CreateUserID
                              ,[LastUpdateTime] = @LastUpdateTime
                              ,[LastUpdateUserID] = @LastUpdateUserID  
                            WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                new SqlParameter("@MediaID",entity.MediaID),
						new SqlParameter("@MeidaType",entity.MeidaType),
            			new SqlParameter("@AverageForwardCount",entity.AverageForwardCount),
            			new SqlParameter("@AverageCommentCount",entity.AverageCommentCount),
            			new SqlParameter("@AveragePointCount",entity.AveragePointCount),
            			new SqlParameter("@ScreenShotURL",entity.ScreenShotURL),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
            			new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 存在就编辑，否则就添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Operate(Entities.Interaction.InteractionWeibo entity, int userId)
        {
            if (entity.MediaID <= 0 || userId <= 0)
            {
                throw new Exception("互动参数-微博-MediaID不能为0,创建人id不能为0");
            }
            entity.MeidaType = (int)MediaTypeEnum.微博;
            var info = GetEntity(entity.MediaID);
            if (info == null)
            {
                return Insert(entity);
            }
            if (entity.CreateUserID == userId)
            {
                entity.MediaID = info.MediaID;
                return Update(entity) > 0 ? entity.MediaID : 0;
            }
            return Insert(entity);
        }
    }
}

