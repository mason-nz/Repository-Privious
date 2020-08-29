using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.Interaction
{
    //媒体-互动参数信息表-微信，PC、APP没有互动参数设置
    public class InteractionWeixin : DataBase
    {
        #region Instance
        public static readonly InteractionWeixin Instance = new InteractionWeixin();
        #endregion

        public Entities.Interaction.InteractionWeixin GetEntity(int mediaId)
        {
            const string sql = @"SELECT [RecID]
                          ,[MeidaType]
                          ,[MediaID]
                          ,[ReferReadCount]
                          ,[AveragePointCount]
                          ,[MoreReadCount]
                          ,[OrigArticleCount]
                          ,[UpdateCount]
                          ,[MaxinumReading]
                          ,[ScreenShotURL]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                      FROM [dbo].[Interaction_Weixin] WITH(NOLOCK) WHERE MediaID = @MediaID";
            var paras = new List<SqlParameter>() { new SqlParameter("@MediaID", mediaId) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Interaction.InteractionWeixin>(data.Tables[0]);
        }

        public Entities.Interaction.InteractionWeixin GetEntityByWxID(int wxID)
        {
            const string sql = @"SELECT [RecID]
                          ,[MeidaType]
                          ,[MediaID]
                          ,[ReferReadCount]
                          ,[AveragePointCount]
                          ,[MoreReadCount]
                          ,[OrigArticleCount]
                          ,[UpdateCount]
                          ,[MaxinumReading]
                          ,[ScreenShotURL]
                          ,[CreateTime]
                          ,[CreateUserID]
                          ,[LastUpdateTime]
                          ,[LastUpdateUserID]
                          ,[WxID]
                      FROM [dbo].[Interaction_Weixin] WITH(NOLOCK) WHERE WxID = @WxID";
            var paras = new List<SqlParameter>() { new SqlParameter("@WxID", wxID) };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToEntity<Entities.Interaction.InteractionWeixin>(data.Tables[0]);
        }

        public int Insert(Entities.Interaction.InteractionWeixin entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Interaction_Weixin(");
            strSql.Append("MeidaType,MediaID,ReferReadCount,AveragePointCount,MoreReadCount,OrigArticleCount,UpdateCount,MaxinumReading,ScreenShotURL,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID,WxID");
            strSql.Append(") values (");
            strSql.Append("@MeidaType,@MediaID,@ReferReadCount,@AveragePointCount,@MoreReadCount,@OrigArticleCount,@UpdateCount,@MaxinumReading,@ScreenShotURL,@CreateTime,@CreateUserID,@LastUpdateTime,@LastUpdateUserID,@WxID");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
						new SqlParameter("@MeidaType",entity.MeidaType),
            			new SqlParameter("@MediaID",entity.MediaID),
            			new SqlParameter("@ReferReadCount",entity.ReferReadCount),
            			new SqlParameter("@AveragePointCount",entity.AveragePointCount),
            			new SqlParameter("@MoreReadCount",entity.MoreReadCount),
            			new SqlParameter("@OrigArticleCount",entity.OrigArticleCount),
            			new SqlParameter("@UpdateCount",entity.UpdateCount),
            			new SqlParameter("@MaxinumReading",entity.MaxinumReading),
            			new SqlParameter("@ScreenShotURL",entity.ScreenShotURL),
            			new SqlParameter("@CreateTime",entity.CreateTime),
            			new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        new SqlParameter("@WxID",entity.WxID),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int Update(Entities.Interaction.InteractionWeixin entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Interaction_Weixin]");
            strSql.Append(@"SET [MeidaType] = @MeidaType
                                  ,[MediaID] =@MediaID
                                  ,[ReferReadCount] = @ReferReadCount
                                  ,[AveragePointCount] =@AveragePointCount
                                  ,[MoreReadCount] = @MoreReadCount
                                  ,[OrigArticleCount] = @OrigArticleCount
                                  ,[UpdateCount] = @UpdateCount
                                  ,[MaxinumReading] = @MaxinumReading
                                  ,[ScreenShotURL] = @ScreenShotURL
                              --,[CreateTime] = @CreateTime
                              --,[CreateUserID] = @CreateUserID
                              ,[LastUpdateTime] = @LastUpdateTime
                              ,[LastUpdateUserID] = @LastUpdateUserID  
                            WHERE MediaID = @MediaID");
            var parameters = new SqlParameter[]{
                new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@MeidaType",entity.MeidaType),
                        new SqlParameter("@ReferReadCount",entity.ReferReadCount),
                        new SqlParameter("@AveragePointCount",entity.AveragePointCount),
                        new SqlParameter("@MoreReadCount",entity.MoreReadCount),
                        new SqlParameter("@OrigArticleCount",entity.OrigArticleCount),
                        new SqlParameter("@UpdateCount",entity.UpdateCount),
                        new SqlParameter("@MaxinumReading",entity.MaxinumReading),
                        new SqlParameter("@ScreenShotURL",entity.ScreenShotURL),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }
        public int UpdateByWxID(Entities.Interaction.InteractionWeixin entity)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[Interaction_Weixin]");
            strSql.Append(@"SET [MeidaType] = @MeidaType
                                  ,[MediaID] =@MediaID
                                  ,[ReferReadCount] = @ReferReadCount
                                  ,[AveragePointCount] =@AveragePointCount
                                  ,[MoreReadCount] = @MoreReadCount
                                  ,[OrigArticleCount] = @OrigArticleCount
                                  ,[UpdateCount] = @UpdateCount
                                  ,[MaxinumReading] = @MaxinumReading
                                  ,[ScreenShotURL] = @ScreenShotURL
                              --,[CreateTime] = @CreateTime
                              --,[CreateUserID] = @CreateUserID
                              ,[LastUpdateTime] = @LastUpdateTime
                              ,[LastUpdateUserID] = @LastUpdateUserID  
                            WHERE WxID = @WxID");
            var parameters = new SqlParameter[]{
                new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@MeidaType",entity.MeidaType),
                        new SqlParameter("@ReferReadCount",entity.ReferReadCount),
                        new SqlParameter("@AveragePointCount",entity.AveragePointCount),
                        new SqlParameter("@MoreReadCount",entity.MoreReadCount),
                        new SqlParameter("@OrigArticleCount",entity.OrigArticleCount),
                        new SqlParameter("@UpdateCount",entity.UpdateCount),
                        new SqlParameter("@MaxinumReading",entity.MaxinumReading),
                        new SqlParameter("@ScreenShotURL",entity.ScreenShotURL),
                        //new SqlParameter("@CreateTime",entity.CreateTime),
                        //new SqlParameter("@CreateUserID",entity.CreateUserID),
            			new SqlParameter("@LastUpdateTime",entity.LastUpdateTime),
                        new SqlParameter("@LastUpdateUserID",entity.LastUpdateUserID),
                        new SqlParameter("@WxID",entity.WxID),
                        };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 存在就编辑，否则就添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Operate(Entities.Interaction.InteractionWeixin entity, int userId)
        {
            if (entity.MediaID <= 0 || userId <= 0)
            {
                throw new Exception("互动参数-weixin-MediaID不能为0,创建用户Id不能为0");
            }
            entity.MeidaType = (int)MediaTypeEnum.微信;
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

