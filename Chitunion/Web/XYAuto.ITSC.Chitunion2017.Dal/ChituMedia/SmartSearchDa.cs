using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class SmartSearchDa : DataBase
    {
        #region 单例
        private SmartSearchDa() { }

        static SmartSearchDa instance = null;
        static readonly object padlock = new object();

        public static SmartSearchDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new SmartSearchDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 根据推广ID获取购物车中 微信媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<PublishlModel> GetSmartSearchWeiXin(int SmartSearchID,int UserID)
        {
            string SQL = @"
                        SELECT  W.RecID ,
                                W.WxNumber ,
                                W.NickName ,
                                W.Summary ,
                                D.DictName AS CategoryName ,
                                W.HeadImg ,
                                W.FansCount ,
                                W.FullName ,
                                W.Sign ,
                                W.QrCodeUrl,
                                W.ReadNum ,
                                W.TotalScores,
                                Cast(W.IsOriginal as int) IsOriginal,
                                P.ADPosition1 ,
                                P.ADPosition2 ,
                                P.Price ,
                                P.MediaID
                        FROM    dbo.LE_Weixin_Repea AS W
                                LEFT JOIN dbo.LE_PublishDetailInfo_Repea AS P ON P.MediaID = W.RecID
                                                                            AND P.MediaType = 14001
                                LEFT JOIN dbo.DictInfo AS D ON D.DictId = W.CategoryID
                        WHERE   W.SmartSearchID = @SmartSearchID AND W.CreateUserID=@UserID AND W.Status>=0 ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@SmartSearchID",SmartSearchID),
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<PublishlModel>(data.Tables[0]);
        }

        /// <summary>
        /// 根据推广ID获取购物车 微博媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<WeiBoModel> GetSmartSearchWeiBo(int SmartSearchID, int UserID)
        {
            string SQL = @"
                        SELECT  W.Number ,
                        W.Name ,
                        W.Sign,
                        D.DictName AS CategoryName ,
                        W.HeadIconURL ,
                        W.FansCount ,
                        W.Sex ,
						W.AuthType,
                        CAST(W.IsReserve AS INT) AS IsReserve,
                        W.ForwardAvg ,
                        W.CommentAvg ,
                        W.LikeAvg ,
                        W.DirectPrice ,
                        W.ForwardPrice,
                        W.TotalScores
                FROM    dbo.LE_Weibo_Repea AS W
                        LEFT JOIN dbo.DictInfo AS D ON D.DictId = W.CategoryID
                WHERE W.SmartSearchID = @SmartSearchID  AND W.CreateUserID=@UserID  AND W.Status>=0  ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@SmartSearchID",SmartSearchID),
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<WeiBoModel>(data.Tables[0]);
        }
        /// <summary>
        /// 根据推广ID获取购物车 APP媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<AppModel> GetSmartSearchApp(int SmartSearchID, int UserID)
        {
            string SQL = @"
                        SELECT  Name ,
                        HeadIconURL ,
                        DailyLive ,
                        Remark ,
                        CAST(IsMonitor AS INT) AS IsMonitor ,
                        CAST(IsLocate AS INT) AS IsLocate ,
                        TotalUser ,
                        D.DictName AS CategoryName 
                FROM    dbo.LE_APP_Repea AS A
                        LEFT JOIN dbo.DictInfo AS D ON D.DictId = A.CategoryID
                WHERE A.SmartSearchID = @SmartSearchID  AND A.CreateUserID=@UserID AND A.Status>=0 ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@SmartSearchID",SmartSearchID),
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<AppModel>(data.Tables[0]);
        }
        /// <summary>
        /// 根据ID获取智能搜索详情
        /// </summary>
        /// <param name="RecID"></param>
        /// <returns></returns>
        public SmartSearchListModel GetSmartSearchDetailInfo(int RecID,int UserID)
        {
            string SQL = $@"
                        SELECT  S.RecID ,
                                S.Name ,
                                S.Demand ,
                                S.Purposes ,
                                S.BudgetPrice ,
                                S.BeginTime ,
                                S.EndTime ,
                                S.MaterialUrl ,
                                S.MaterialUrlName,
                                D.DictName AS StatusName
                        FROM    dbo.LE_SmartSearch AS S
                                INNER JOIN dbo.DictInfo AS D ON S.Status = D.DictId
                        WHERE   S.RecID =@RecID AND  S.UserID=@UserID  AND S.Status>=0 ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@RecID",RecID),
                new SqlParameter("@UserID",UserID)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return DataTableToEntity<SmartSearchListModel>(data.Tables[0]);
        }
        /// <summary>
        /// 获取智能搜索列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, List<SmartSearchListModel>> GetSmartSearchList(QueryArgs query, int UserID)
        {
            StringBuilder SQL = new StringBuilder($@"
                            SELECT  *
                            YanFaFROM    ( 
                            SELECT  S.RecID,
                            S.Name ,
                            S.Demand ,
                            S.Purposes ,
                            S.BudgetPrice ,
                            S.BeginTime ,
							S.CreateTime,
                            S.EndTime ,
                            MaterialUrl ,
                            D.DictName AS StatusName ,
                            ( ISNULL(WB.Num,0) + ISNULL(WX.Num,0) + ISNULL(App.Num,0) ) AS MediaCount
                    FROM    dbo.LE_SmartSearch AS S
                            INNER JOIN dbo.DictInfo AS D ON S.Status = D.DictId
                            LEFT JOIN ( SELECT  COUNT(1) AS Num ,
                                                SmartSearchID
                                        FROM    dbo.LE_Weibo_Repea WHERE Status>=0
                                        GROUP BY SmartSearchID
                                      ) AS WB ON WB.SmartSearchID = S.RecID
                            LEFT JOIN ( SELECT  COUNT(1) AS Num ,
                                                SmartSearchID
                                        FROM    dbo.LE_Weixin_Repea WHERE Status>=0
                                        GROUP BY SmartSearchID
                                      ) AS WX ON WX.SmartSearchID = S.RecID
                            LEFT JOIN ( SELECT  COUNT(1) AS Num ,
                                                SmartSearchID
                                        FROM    dbo.LE_APP_Repea WHERE Status>=0
                                        GROUP BY SmartSearchID
                                      ) AS App ON App.SmartSearchID = S.RecID
                    WHERE S.Status>=0 AND S.UserID={UserID} ");
            if (query.Status > 0)
            {
                SQL.Append($" AND S.Status={query.Status}");
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                SQL.Append($" AND S.Name LIKE '%{query.Name}%' ");
            }
            SQL.Append(" ) AS A ");
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order","  RecID DESC ")
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<SmartSearchListModel>>(totalCount, DataTableToList<SmartSearchListModel>(data.Tables[0]));
        }
        public int AddSmartSearchInfo(SmartSearchModel SearchModel)
        {
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@Name",SearchModel.Name),
                new SqlParameter("@Demand",SearchModel.Demand),
                new SqlParameter("@Purposes",SearchModel.Purposes),
                new SqlParameter("@BudgetPrice",SearchModel.BudgetPrice),
                new SqlParameter("@BeginTime",SearchModel.BeginTime),
                new SqlParameter("@MaterialUrlName",SearchModel.MaterialUrlName),
                new SqlParameter("@EndTime",SearchModel.EndTime),
                new SqlParameter("@MaterialUrl",SearchModel.MaterialUrl),
                new SqlParameter("@UserID",SearchModel.UserID)
            };

            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Insert_LE_SmartSearch", sqlParams);

        }
        /// <summary>
        /// 获取推广计划总数
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int GetSmartSearchCount(int UserID)
        {
            string SQL = $@"
                            SELECT  ( SELECT    COUNT(1) AS CountNum
                                      FROM      dbo.LE_SmartSearch
                                      WHERE     UserID = @UserID AND Status>=0 
                                    ) + ( SELECT    COUNT(1) AS CountNum
                                          FROM      dbo.LE_ContentDistribute
                                          WHERE     UserID = @UserID AND Status>=0
                                        ) + ( SELECT    COUNT(1) AS CountNum
                                              FROM      dbo.LE_MediaPromotion
                                              WHERE     UserID = @UserID AND Status>=0 
                                            );";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };
            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
        }
    }
}
