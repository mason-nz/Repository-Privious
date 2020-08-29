using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.WxEditor;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WxEditor
{

    /// <summary>
    /// 2017-06-22 zlb
    /// 图文组DAL
    /// </summary>
    public class WxArticleGroup : DataBase
    {
        public static readonly WxArticleGroup Instance = new WxArticleGroup();
        /// <summary>
        /// zlb 2017-06-22 
        /// 分页查询用户的图文组ID集合
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="TitleOrAbstract">标题或摘要</param>
        /// <param name="Pagesize">页数大小</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="Status">0全部 1同步失败的</param>
        /// <returns></returns>
        public DataTable SelectGroupIDListByUserid(int UserID, string TitleOrAbstract, int Pagesize, int PageIndex, int Status)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@TitleOrAbstract", SqlDbType.VarChar,500),
                    new SqlParameter("@Pagesize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int),
                    new SqlParameter("@Status", SqlDbType.Int)
                    };
            parameters[0].Value = UserID;
            parameters[1].Value = TitleOrAbstract;
            parameters[2].Value = Pagesize;
            parameters[3].Value = PageIndex;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = Status;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectArticleGroupHistory", parameters);
            int totalCount = (int)(parameters[4].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// zlb 2017-06-22
        /// 根据图文组ID集合查询图文组文章信息
        /// </summary>
        /// <param name="WxgrIDList">图文组批次主键ID集合</param>
        /// <returns></returns>
        public DataSet SelectArticleGroupListByIDList(string WxgrIDList)
        {
            string strSql = "SELECT WSH.WxGRID,Orderby,WSH.Title,WSH.CoverPicUrl FROM  WxStaticHitory WSH  WHERE WSH.WxGRID IN (" + WxgrIDList + ") ";
            //{
            //strSql += " and WSH.WxGRID in (SELECT WxGRID FROM WxHistoryStatus  WHERE Status=" + (int)ArticleSyncStatus.同步失败 + ") ";
            //}
            strSql += " SELECT WHS.WxGRID,WO.NickName,WO.HeadImg,WHS.Status, WHS.CompleteTime FROM WxHistoryStatus WHS INNER JOIN Weixin_OAuth WO ON WHS.WxID=WO.RecID WHERE WHS.WxGRID IN (" + WxgrIDList + ") ";
            //if (Status == 1)
            //{
            //    strSql += " and WHS.Status=" + (int)ArticleSyncStatus.同步失败 + ") ";
            //}
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds;
        }

        /// <summary>
        /// 获取所属微信 图文组表存在的
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<OwnWeixinItemDTO> GetOwnWeixinList(int userID)
        {
            string sql = @"
            SELECT  FromWxID as WxID ,FromWxNumber as WxNumber ,FromWxName as WxName
            FROM    dbo.WxMaterial_ArticleGroup
            WHERE   CreateUserID = @CreateUserID AND Status = 0
            GROUP BY FromWxID ,FromWxNumber ,FromWxName
            ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CreateUserID", userID)
            };
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters).Tables[0];
            return DataTableToList<OwnWeixinItemDTO>(dt);
        }

        /// <summary>
        /// 获取图文组列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="key"></param>
        /// <param name="wxName"></param>
        /// <param name="wxNumber"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetWeixinArticleGroupListResDTO GetWeixinArticleGroupList(int userID, string key, string wxName, string wxNumber,
            string beginDate, string endDate, int pageIndex, int pageSize)
        {
            GetWeixinArticleGroupListResDTO res = new GetWeixinArticleGroupListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@Key", key),
                new SqlParameter("@WxName", wxName),
                new SqlParameter("@WxNumber", wxNumber),
                new SqlParameter("@BeginDate", beginDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[8].Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetWxArticleGroupList", parameters).Tables[0];
            res.TotalCount = Convert.ToInt32(parameters[8].Value);
            res.List = DataTableToList<ArticleGroupItem>(dt);
            return res;
        }

        /// <summary>
        /// 获取图文列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="key"></param>
        /// <param name="wxName"></param>
        /// <param name="wxNumber"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetWeixinArticleListResDTO GetWeixinArticleList(int userID, string key, string wxName, string wxNumber,
            string beginDate, string endDate, int pageIndex, int pageSize)
        {
            GetWeixinArticleListResDTO res = new GetWeixinArticleListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@Key", key),
                new SqlParameter("@WxName", wxName),
                new SqlParameter("@WxNumber", wxNumber),
                new SqlParameter("@BeginDate", beginDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[8].Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetWxArticleList", parameters).Tables[0];
            res.TotalCount = Convert.ToInt32(parameters[8].Value);
            res.List = DataTableToList<ArticleItem>(dt);
            return res;
        }

        /// <summary>
        /// 获取好文推荐图文列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetWeixinArticleListResDTO GetWeixinGoodArticleTJList(string key, int pageIndex, int pageSize)
        {
            GetWeixinArticleListResDTO res = new GetWeixinArticleListResDTO();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Key", key),
                new SqlParameter("@PageIndex", pageIndex),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@TotalCount", SqlDbType.Int)
            };
            parameters[3].Direction = ParameterDirection.Output;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Media_GetWxGoodArticleTJList", parameters).Tables[0];
            res.TotalCount = Convert.ToInt32(parameters[3].Value);
            res.List = DataTableToList<ArticleItem>(dt);
            return res;
        }

        /// <summary>
        /// 根据图文ID获取图文信息
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public ArticleInfo GetArticleInfoByID(int articleID)
        {
            string sql = "SELECT TOP 1 * FROM dbo.WxMaterial_Article WHERE ArticleID = " + articleID;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToEntity<ArticleInfo>(dt);
        }

        /// <summary>
        /// 按groupID获取下面的正常图文
        /// </summary>
        /// <param name="gourpID"></param>
        /// <returns></returns>
        public List<ArticleInfo> GetArticleListByGroupID(int gourpID)
        {
            string sql = "SELECT * FROM dbo.WxMaterial_Article WHERE Status = 0 AND GroupID = " + gourpID + " ORDER BY Orderby asc";
            var dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            return DataTableToList<ArticleInfo>(dt);
        }

        /// <summary>
        /// 创建上传记录 批次-历史-静态历史
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="wxIDs"></param>
        /// <param name="userID"></param>
        /// <param name="msg"></param>
        /// <param name="ranking"></param>
        /// <returns></returns>
        public bool CreateUploadRecord(int groupID, List<int> wxIDs, int userID, ref string msg, ref int ranking)
        {
            string sql = string.Empty;
            int rowcount = 0;
            SqlParameter[] parameters = null;
            var articleList = this.GetArticleListByGroupID(groupID);
            ranking = this.GetGroupCurrentRanking(groupID);
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region 批次
                        sql = @"INSERT INTO dbo.WxGroupRanking (GroupID,Ranking,CreateUserID,CreateTime)
                                        VALUES(@GroupID,@Ranking,@CreateUserID,@CreateTime);
                                        SELECT @@IDENTITY";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@GroupID", groupID),
                            new SqlParameter("@Ranking", ranking),
                            new SqlParameter("@CreateUserID", userID),
                            new SqlParameter("@CreateTime", DateTime.Now)
                        };
                        int rankID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
                        #endregion
                        foreach (int wxID in wxIDs)
                        {
                            #region 同步历史
                            sql = @"INSERT INTO dbo.WxHistoryStatus(WxGRID ,GroupID ,WxID ,Status ,CreateUserID ,CreateTime ,CompleteTime ,Ranking)
                                        VALUES  (@WxGRID ,@GroupID ,@WxID ,@Status ,@CreateUserID ,@CreateTime ,@CompleteTime ,@Ranking);
                                        SELECT @@IDENTITY";
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@WxGRID", rankID),
                                new SqlParameter("@GroupID", groupID),
                                new SqlParameter("@WxID", wxID),
                                new SqlParameter("@Status", (int)ArticleSyncStatus.待同步),
                                new SqlParameter("@CreateUserID", userID),
                                new SqlParameter("@CreateTime", DateTime.Now),
                                new SqlParameter("@CompleteTime", null),
                                new SqlParameter("@Ranking", ranking),
                            };
                            int hisID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
                            #endregion
                        }
                        foreach (var article in articleList)
                        {
                            #region 图文静态历史
                            sql = @"INSERT dbo.WxStaticHitory(WxGRID ,GroupID ,ArticleID ,Title ,CoverPicUrl ,Abstract ,CreateTime ,Orderby ,StaticUrl ,CreateUserID) 
                                          VALUES(@WxGRID ,@GroupID ,@ArticleID ,@Title ,@CoverPicUrl ,@Abstract ,@CreateTime ,@Orderby ,@StaticUrl ,@CreateUserID)";
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@WxGRID", rankID),
                                new SqlParameter("@GroupID", groupID),
                                new SqlParameter("@ArticleID", article.ArticleID),
                                new SqlParameter("@Title", article.Title),
                                new SqlParameter("@CoverPicUrl", article.CoverPicUrl),
                                new SqlParameter("@Abstract", article.Abstract),
                                new SqlParameter("@CreateTime", DateTime.Now),
                                new SqlParameter("@Orderby", article.Orderby),
                                new SqlParameter("@StaticUrl", string.Empty),
                                new SqlParameter("@CreateUserID", userID),
                            };
                            rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                            #endregion
                        }
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        msg = "系统异常";
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 上传结果同步
        /// </summary>
        /// <param name="gourpID"></param>
        /// <param name="wxID"></param>
        /// <param name="ranking"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UploadSyncHisStatus(int gourpID, int wxID, int ranking, ArticleSyncStatus status)
        {
            string sql = @"UPDATE dbo.WxHistoryStatus 
                                    SET Status = @Status,CompleteTime = @CompleteTime 
                                    WHERE GroupID = @GroupID AND Ranking = @Ranking AND WxID = @WxID";
            DateTime CompleteTime = Entities.Constants.Constant.DATE_INVALID_VALUE;
            if (status == ArticleSyncStatus.同步成功 || status == ArticleSyncStatus.同步失败)
                CompleteTime = DateTime.Now;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Status", (int)status),
                new SqlParameter("@CompleteTime", CompleteTime),
                new SqlParameter("@GroupID", gourpID),
                new SqlParameter("@Ranking", ranking),
                new SqlParameter("@WxID", wxID)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters) > 0;
        }

        public bool UpdateWeixinMaterialID(int groupID, string materialID)
        {
            string sql = string.Format("UPDATE dbo.WxMaterial_ArticleGroup SET WxMaterialID = '{1}' WHERE GroupID = {0}", groupID, materialID);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// 获取当前批次
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int GetGroupCurrentRanking(int groupID)
        {
            string sql = "SELECT ISNULL(MAX(Ranking),0)+1 FROM dbo.WxGroupRanking WHERE GroupID = " + groupID;
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }

        /// <summary>
        /// 移动图文
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="optType"></param>
        /// <returns></returns>
        public bool MoveArticle(int articleID, int optType)
        {
            int totalCount = 0;
            int rowCount = 0;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sql = "SELECT TOP 1 * FROM dbo.WxMaterial_Article WHERE ArticleID = " + articleID;
                        DataTable dt = SqlHelper.ExecuteDataset(trans, CommandType.Text, sql).Tables[0];
                        var one = DataTableToEntity<ArticleInfo>(dt);
                        sql = string.Format("SELECT COUNT(1) FROM dbo.WxMaterial_Article WHERE GroupID = {0} AND Status = 0", one.GroupID);
                        totalCount = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql));
                        if (optType.Equals(1))
                        {//上移
                            if (one.Orderby < 2)
                            {
                                trans.Rollback();
                                return false;
                            }
                            sql = string.Format("UPDATE dbo.WxMaterial_Article SET Orderby = {0} WHERE Orderby = {1}", one.Orderby, one.Orderby - 1);
                            rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                            if (rowCount.Equals(0))
                            {
                                trans.Rollback();
                                return false;
                            }
                            sql = string.Format("UPDATE dbo.WxMaterial_Article SET Orderby = {0} WHERE ArticleID = {1}", one.Orderby - 1, one.ArticleID);
                            rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                            if (rowCount.Equals(0))
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                        else
                        {//下移
                            if (one.Orderby >= totalCount)
                            {
                                trans.Rollback();
                                return false;
                            }
                            sql = string.Format("UPDATE dbo.WxMaterial_Article SET Orderby = {0} WHERE Orderby = {1}", one.Orderby, one.Orderby + 1);
                            rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                            if (rowCount.Equals(0))
                            {
                                trans.Rollback();
                                return false;
                            }
                            sql = string.Format("UPDATE dbo.WxMaterial_Article SET Orderby = {0} WHERE ArticleID = {1}", one.Orderby + 1, one.ArticleID);
                            rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                            if (rowCount.Equals(0))
                            {
                                trans.Rollback();
                                return false;
                            }
                        }
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 新增编辑图文
        /// </summary>
        /// <param name="req"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ModifyArticle(ModifyArticleReqDTO dto, ref int groupID)
        {
            string sql = string.Empty;
            int rowcount = 0;
            int defaultValue = 0;
            SqlParameter[] parameters = null;
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (dto.GroupID.Equals(0))
                        {

                            #region 新建组

                            sql = @"INSERT INTO dbo.WxMaterial_ArticleGroup
                                         (WxMaterialID, ArticleCount, SourceType, FromUrl, FromWxID, FromWxNumber, FromWxName, CreateUserID, CreateTime, LastUpdateTime, Status)
                                         VALUES 
                                         (@WxMaterialID,@ArticleCount,@SourceType,@FromUrl,@FromWxID,@FromWxNumber,@FromWxName,@CreateUserID,@CreateTime,@LastUpdateTime,@Status);
                                         SELECT @@IDENTITY";
                            parameters = new SqlParameter[]
                            {
                                new SqlParameter("@WxMaterialID", string.Empty),
                                new SqlParameter("@ArticleCount", defaultValue),
                                new SqlParameter("@SourceType", (int)dto.ImportType),
                                new SqlParameter("@FromUrl", dto.ImportType.Equals(ArticleImportTypeEnum.手工添加) ? string.Empty : dto.fromUrl),
                                new SqlParameter("@FromWxID", dto.ImportType.Equals(ArticleImportTypeEnum.手工添加) ? defaultValue : dto.fromWxID),
                                new SqlParameter("@FromWxNumber", dto.ImportType.Equals(ArticleImportTypeEnum.手工添加) ? string.Empty : dto.fromWxNumber),
                                new SqlParameter("@FromWxName", dto.ImportType.Equals(ArticleImportTypeEnum.手工添加)? "赤兔联盟" : dto.fromWxName),
                                new SqlParameter("@CreateUserID", dto.UpdateUserID),
                                new SqlParameter("@CreateTime", dto.UpdateTime),
                                new SqlParameter("@LastUpdateTime", dto.UpdateTime),
                                new SqlParameter("@Status", Convert.ToInt32(0)),
                            };
                            dto.GroupID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));

                            #endregion
                        }
                        else
                        {
                            sql = "SELECT COUNT(1) FROM dbo.WxMaterial_ArticleGroup WHERE GroupID = @GroupID";
                            parameters = new SqlParameter[] { new SqlParameter("@GroupID", dto.GroupID) };
                            rowcount = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
                            if (rowcount.Equals(0))
                            {
                                trans.Rollback();
                                return false;
                            }
                            #region 逻辑删除
                            string range = string.Empty;
                            dto.ArticleList.Where(a => !a.ArticleID.Equals(0)).Select(a => a.ArticleID).ToList().ForEach(a => range += (a + ","));
                            if (range.Length > 0)
                                range = range.Substring(0, range.Length - 1);
                            else
                                range = "0";
                            sql = "UPDATE dbo.WxMaterial_Article SET Status = -1 WHERE GroupID = " + dto.GroupID + " AND ArticleID NOT IN (" + range + ")";
                            rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql);
                            #endregion
                        }

                        foreach (var article in dto.ArticleList)
                        {
                            if (article.ArticleID.Equals(0))
                            {
                                #region 新增图文 Status = 0
                                sql = @"INSERT INTO dbo.WxMaterial_Article 
                                         (GroupID , Orderby , Title , CoverPicUrl , Author , Abstract , Content , OriginalUrl , Status , CreateUserID , CreateTime , LastUpdateTime , PCViewUrl , MobileViewUrl )
                                          VALUES
                                         (@GroupID ,@Orderby ,@Title ,@CoverPicUrl ,@Author ,@Abstract ,@Content ,@OriginalUrl ,@Status ,@CreateUserID ,@CreateTime ,@LastUpdateTime ,@PCViewUrl ,@MobileViewUrl );
                                          SELECT @@IDENTITY";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@GroupID", dto.GroupID),
                                    new SqlParameter("@Orderby", article.Orderby),
                                    new SqlParameter("@Title", article.Title),
                                    new SqlParameter("@CoverPicUrl",article.CoverPicUrl),
                                    new SqlParameter("@Author", article.Author),
                                    new SqlParameter("@Abstract", article.Abstract),
                                    new SqlParameter("@Content", article.Content),
                                    new SqlParameter("@OriginalUrl", article.OriginalUrl),
                                    new SqlParameter("@Status", defaultValue),
                                    new SqlParameter("@CreateUserID", dto.UpdateUserID),
                                    new SqlParameter("@CreateTime", dto.UpdateTime),
                                    new SqlParameter("@LastUpdateTime", dto.UpdateTime),
                                    new SqlParameter("@PCViewUrl", string.Empty),
                                    new SqlParameter("@MobileViewUrl", string.Empty),
                                };
                                article.ArticleID = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
                                #endregion
                            }
                            else
                            {
                                #region 更新图文
                                sql = @"UPDATE dbo.WxMaterial_Article SET Title = @Title, Orderby = @Orderby, CoverPicUrl = @CoverPicUrl, Author = @Author,
                                          Abstract = @Abstract, Content = @Content, OriginalUrl = @OriginalUrl, LastUpdateTime = @LastUpdateTime
                                          WHERE ArticleID = @ArticleID";
                                parameters = new SqlParameter[]
                                {
                                    new SqlParameter("@Title", article.Title),
                                    new SqlParameter("@Orderby", article.Orderby),
                                    new SqlParameter("@CoverPicUrl", article.CoverPicUrl),
                                    new SqlParameter("@Author", article.Author),
                                    new SqlParameter("@Abstract", article.Abstract),
                                    new SqlParameter("@Content", article.Content),
                                    new SqlParameter("@OriginalUrl", article.OriginalUrl),
                                    new SqlParameter("@LastUpdateTime", dto.UpdateTime),
                                    new SqlParameter("@ArticleID", article.ArticleID)
                                };
                                rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                                if (rowcount.Equals(0))
                                {
                                    trans.Rollback();
                                    return false;
                                }
                                #endregion
                            }
                        }
                        #region 更新图文组图文数量
                        //sql = "UPDATE dbo.WxMaterial_ArticleGroup SET STATUS = 0 WHERE GroupID = @GroupID";
                        //parameters = new SqlParameter[] { new SqlParameter("@GroupID", dto.GroupID) };
                        //rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                        //if (rowcount.Equals(0))
                        //{
                        //    trans.Rollback();
                        //    return false;
                        //}
                        sql = @"UPDATE  dbo.WxMaterial_ArticleGroup  SET  
                                      ArticleCount = ( SELECT COUNT(1)
                                                                 FROM   dbo.WxMaterial_Article
                                                                 WHERE  Status = 0
                                                                 AND dbo.WxMaterial_Article.GroupID = dbo.WxMaterial_ArticleGroup.GroupID
                                                               )
                                      WHERE   WxMaterial_ArticleGroup.GroupID = @GroupID";
                        parameters = new SqlParameter[] { new SqlParameter("@GroupID", dto.GroupID) };
                        rowcount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                        if (rowcount.Equals(0))
                        {
                            trans.Rollback();
                            return false;
                        }
                        #endregion
                        groupID = dto.GroupID;
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 回填静态页地址
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="optType">1：PC 2：Mobile</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool UpdateArticleViewUrl(int articleID, int optType, string url)
        {
            string sql = "";
            SqlParameter[] parameters = null;
            if (optType.Equals(1))
            {
                sql = "UPDATE dbo.WxMaterial_Article SET PCViewUrl = @PCViewUrl  WHERE ArticleID = @ArticleID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@PCViewUrl", url),
                    new SqlParameter("@ArticleID", articleID),
                };
            }
            else
            {
                sql = "UPDATE dbo.WxMaterial_Article SET MobileViewUrl = @MobileViewUrl WHERE ArticleID = @ArticleID";
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@MobileViewUrl", url),
                    new SqlParameter("@ArticleID", articleID),
                };
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters) > 0;
        }

        /// <summary>
        /// 删除图文
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public bool DeleteArticle(int articleID, ref string msg)
        {
            var one = this.GetArticleInfoByID(articleID);
            string sql = string.Empty;
            int rowCount = 0;
            SqlParameter[] parameters = null;
            //验证
            sql = "SELECT COUNT(1) FROM dbo.WxHistoryStatus WHERE GroupID = @GroupID AND Status in (54001,54002)";
            parameters = new SqlParameter[]
            {
                            new SqlParameter("@GroupID", one.GroupID),
            };
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            if (count > 0)
            {
                msg = "未同步完";
                return false;
            }
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //WxMaterial_Article Status = -1
                        sql = "UPDATE dbo.WxMaterial_Article SET Status = -1 WHERE ArticleID = @ArticleID";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ArticleID", one.ArticleID)
                        };
                        rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                        if (rowCount.Equals(0))
                        {
                            trans.Rollback();
                            return false;
                        }
                        //WxMaterial_Article Orderby = Orderby -1
                        sql = "UPDATE dbo.WxMaterial_Article SET Orderby = Orderby -1 WHERE GroupID = @GroupID and Orderby > @Orderby";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@GroupID", one.GroupID),
                            new SqlParameter("@Orderby", one.Orderby),
                        };
                        rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                        //WxMaterial_Article Select Count
                        sql = "SELECT COUNT(1) FROM dbo.WxMaterial_Article WHERE GroupID = @GroupID AND Status = 0";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@GroupID", one.GroupID),
                        };
                        rowCount = Convert.ToInt32(SqlHelper.ExecuteScalar(trans, CommandType.Text, sql, parameters));
                        //WxMaterial_ArticleGroup ArticleCount LastUpdateTime Status
                        sql = string.Format("UPDATE dbo.WxMaterial_ArticleGroup SET ArticleCount = @ArticleCount,LastUpdateTime = @LastUpdateTime{0} WHERE GroupID = @GroupID", rowCount.Equals(0) ? ",Status = -1" : string.Empty);
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@ArticleCount", rowCount),
                            new SqlParameter("@LastUpdateTime", DateTime.Now),
                            new SqlParameter("@GroupID", one.GroupID)
                        };
                        rowCount = SqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, parameters);
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        msg = "系统异常";
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 删除图文组
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteArticleGroup(int groupID, ref string msg)
        {
            string sql = string.Empty;
            int rowCount = 0;
            SqlParameter[] parameters = null;
            //验证
            sql = "SELECT COUNT(1) FROM dbo.WxHistoryStatus WHERE GroupID = @GroupID AND Status in (54001,54002)";
            parameters = new SqlParameter[]
            {
                            new SqlParameter("@GroupID", groupID),
            };
            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters));
            if (count > 0)
            {
                msg = "未同步完";
                return false;
            }
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        sql = @"UPDATE dbo.WxMaterial_Article SET Status = -1 WHERE GroupID = @GroupID;
                                      UPDATE dbo.WxMaterial_ArticleGroup SET Status = -1 WHERE GroupID = @GroupID";
                        parameters = new SqlParameter[]
                        {
                            new SqlParameter("@GroupID", groupID)
                        };
                        rowCount = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        msg = "系统异常";
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// zlb 2017-06-27
        /// 查询图文组同步微信状态
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectWxStatusInfoByGroupID(int GroupID, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT WO.RecID AS WxID,WO.WxNumber,WHS.Ranking,WO.NickName AS WxName,WO.HeadImg,(CASE WHEN WHS.Status IS NULL THEN A.HasRight ELSE WHS.Status END) Status
                FROM (SELECT  h1.WxID, CASE WHEN d1.RecID IS NULL THEN 0 ELSE 1 END AS HasRight FROM   dbo.OAuth_History h1
                LEFT JOIN dbo.OAuth_Detail d1 ON h1.RecID = d1.HisID AND d1.OAuthID = 37011
                WHERE  EXISTS(SELECT 1 FROM(SELECT    MAX(RecID) AS RecID,WxID FROM  dbo.OAuth_History GROUP BY  WxID) h2 WHERE  h1.RecID = h2.RecID)   AND h1.Status IN(39001, 39002))  A
                LEFT JOIN Weixin_OAuth WO ON  A.WxID=WO.RecID LEFT JOIN WxHistoryStatus WHS  ON A.WxID=WHS.WxID AND  WHS.Ranking = (SELECT MAX(Ranking) FROM WxGroupRanking WHERE CreateUserID = {0} AND GroupID ={1}) 
				 AND WHS.CreateUserID = {0} AND WHS.GroupID = {1}  INNER JOIN Media_Weixin MW ON MW.WxID=WO.RecID  WHERE  MW.Status=0 AND MW.CreateUserID={0} ", UserID, GroupID);
            sb.Append(" ORDER BY WO.CreateTime");
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString()).Tables[0];
        }
        /// <summary>
        /// zlb 2017-06-27
        /// 查询用户下所有授权账号
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectWxOAuthInfo(int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT WO.RecID AS WxID,WO.WxNumber,WO.NickName AS WxName,WO.HeadImg,A.HasRight AS Status FROM Weixin_OAuth WO INNER JOIN (SELECT  h1.WxID, CASE WHEN d1.RecID IS NULL THEN 0 ELSE 1 END AS HasRight FROM   dbo.OAuth_History h1
                LEFT JOIN dbo.OAuth_Detail d1 ON h1.RecID = d1.HisID AND d1.OAuthID = 37011
                WHERE  EXISTS(SELECT 1 FROM(SELECT    MAX(RecID) AS RecID,WxID FROM  dbo.OAuth_History GROUP BY  WxID) h2 WHERE  h1.RecID = h2.RecID)
                AND h1.Status IN(39001, 39002)) A  ON WO.RecID=A.WxID INNER JOIN Media_Weixin MW ON MW.WxID=WO.RecID WHERE  MW.Status=0 AND MW.CreateUserID={0}", UserID);
            sb.Append(" ORDER BY WO.CreateTime");
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString()).Tables[0];
        }
        /// <summary>
        /// zlb 2017-06-27
        /// 查询该用户下GroupID对应的图文组正在同步或待同步的微信号的数量
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int SelectInSyncGroupCount(int GroupID, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT COUNT(1) FROM dbo.WxHistoryStatus WHERE GroupID={0} AND CreateUserID={1} AND (Status={2} OR Status={3})", GroupID, UserID, (int)ArticleSyncStatus.同步中, (int)ArticleSyncStatus.待同步);
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        /// <summary>
        /// zlb 2017-06-27
        /// 查询图文组信息
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectGroupInfoByGroupID(int GroupID, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT WMA.ArticleID,WMA.Title,WMA.CoverPicUrl,WMA.Orderby FROM WxMaterial_Article WMA INNER JOIN WxMaterial_ArticleGroup WMG ON WMA.GroupID=WMG.GroupID   WHERE WMA.Status!=-1 AND WMG.Status!=-1 AND WMG.CreateUserID={0} AND WMG.GroupID={1} ORDER BY WMA.Orderby", UserID, GroupID);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString()).Tables[0];
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 根据图文ID和用户ID查询图文信息
        /// </summary>
        /// <param name="ArticleID">图文ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectArticleInfoByArticleID(int ArticleID, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT top 1 WMA.Title,WMA.Author,WMA.Abstract,WMA.Content,WMA.OriginalUrl FROM WxMaterial_Article WMA INNER JOIN WxMaterial_ArticleGroup WMG ON WMA.GroupID=WMG.GroupID  WHERE WMA.Status!=-1 AND WMG.Status!=-1 AND WMA.ArticleID={0} AND WMG.CreateUserID={1}", ArticleID, UserID);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString()).Tables[0];
        }
        /// <summary>
        /// zlb 2017-07-01
        /// 查询图文组文章信息列表
        /// </summary>
        /// <param name="GroupID">图文组ID</param>
        /// <param name="UserID">用户ID</param>
        /// 
        /// <returns></returns>
        public DataTable SelectGroupArticlesByGroupID(int GroupID, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT WA.ArticleID,WA.Title,WA.CoverPicUrl,WA.Author,WA.Abstract,WA.Content,WA.OriginalUrl,WA.PCViewUrl,WA.MobileViewUrl,WA.Orderby FROM  WxMaterial_ArticleGroup WG INNER JOIN  dbo.WxMaterial_Article WA ON WG.GroupID=WA.GroupID WHERE WG.Status=0  AND WA.Status=0  AND  WG.CreateUserID={0} AND  WG.GroupID={1}", UserID, GroupID);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString()).Tables[0];
        }
        /// <summary>
        ///zlb 2017-07-01 根据导入网址查询对应的文章ID和zuID
        /// </summary>
        /// <param name="ImportUrl">导入文章URL</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectArticleIdByUrl(string ImportUrl, int userID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT TOP 1 A.ArticleID,A.GroupID FROM WxMaterial_ArticleGroup G INNER JOIN WxMaterial_Article A ON G.GroupID=A.GroupID WHERE A.Status=0  AND G.Status=0  AND G.FromUrl = '{0}'  AND G.SourceType ={1} AND G.CreateUserID={2}", StringHelper.SqlFilter(ImportUrl), (int)ArticleImportTypeEnum.Url, userID);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sb.ToString()).Tables[0];

        }
        /// <summary>
        ///zlb 2017-07-01  修改图文信息
        /// </summary>
        /// <param name="ArticleID"></param>
        /// <param name="GroupID"></param>
        /// <param name="MAR"></param>
        /// <returns></returns>
        public bool UpdateGroupArticleByID(int ArticleID, int GroupID, ModifyArticleReqDTO MAR)
        {
            SqlParameter[] parameters = {

                    new SqlParameter("@FromWxNumber", SqlDbType.VarChar,200),
                    new SqlParameter("@FromWxName", SqlDbType.VarChar,200),
                    new SqlParameter("@Title", SqlDbType.VarChar,200),
                    new SqlParameter("@Author", SqlDbType.VarChar,100),
                    new SqlParameter("@Abstract", SqlDbType.VarChar,500),
                    new SqlParameter("@Content", SqlDbType.Text),
                    new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Status", SqlDbType.Int),
                    new SqlParameter("@GroupID", SqlDbType.Int),
                    new SqlParameter("@ArticleID", SqlDbType.Int)
                    };
            parameters[0].Value = MAR.fromWxNumber;
            parameters[1].Value = MAR.fromWxName;
            parameters[2].Value = MAR.ArticleList[0].Title;
            parameters[3].Value = MAR.ArticleList[0].Author;
            parameters[4].Value = MAR.ArticleList[0].Abstract;
            parameters[5].Value = MAR.ArticleList[0].Content;
            parameters[6].Value = MAR.UpdateTime;
            parameters[7].Value = 0;
            parameters[8].Value = GroupID;
            parameters[9].Value = ArticleID;
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE  dbo.WxMaterial_ArticleGroup SET  FromWxNumber=@FromWxNumber,FromWxName=@FromWxName,LastUpdateTime=@LastUpdateTime,Status=@Status where GroupID=@GroupID;");

            sb.Append("UPDATE WxMaterial_Article SET Title=@Title,Author=@Author,Abstract=@Abstract,Content=@Content,LastUpdateTime=@LastUpdateTime,Status=@Status where ArticleID=@ArticleID");

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString(), parameters) > 0 ? true : false;
        }

    }
}
