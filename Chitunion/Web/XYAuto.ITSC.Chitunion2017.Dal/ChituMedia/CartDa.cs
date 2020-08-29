using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.ChituMedia
{
    public class CartDa : DataBase
    {
        #region 单例
        private CartDa() { }

        static CartDa instance = null;
        static readonly object padlock = new object();

        public static CartDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CartDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 根据用户ID获取购物车列表

        /// <summary>
        /// 根据用户ID获取购物车列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<CartModel> GetCartInfoList(int UserID)
        {
            string SQL = $@"
                        SELECT  *
                        FROM    ( SELECT    C.RecID ,
                                            W.NickName AS Name ,
                                            W.HeadImg AS LogUrl ,
                                            CAST(W.WxNumber AS VARCHAR(50) )  AS MediaText,
                                            C.MediaType ,
                                            C.MediaID,
                                            C.CreateTime
                                  FROM      dbo.LE_Weixin AS W
                                            INNER JOIN dbo.LE_CartInfo AS C ON C.MediaID = W.RecID
                                                                               AND C.MediaType = 14001
                                                                               AND C.UserID = @UserID
                                  UNION ALL
                                  SELECT    C.RecID ,
                                            W.Name AS Name ,
                                            W.HeadIconURL AS LogUrl ,
                                            CAST(W.FansCount AS VARCHAR(50) ) AS MediaText ,
                                            C.MediaType ,
                                            C.MediaID,
                                            C.CreateTime
                                  FROM      dbo.LE_Weibo AS W
                                            INNER JOIN dbo.LE_CartInfo AS C ON C.MediaID = W.RecID
                                                                               AND C.MediaType = 14003
                                                                               AND C.UserID = @UserID
                                  UNION ALL
                                  SELECT    C.RecID ,
                                            W.Name AS Name ,
                                            W.HeadIconURL AS LogUrl ,
                                            CAST(W.DailyLive AS VARCHAR(50) )  AS MediaText  ,
                                            C.MediaType ,
                                            C.MediaID,
                                            C.CreateTime
                                  FROM      dbo.LE_APP AS W
                                            INNER JOIN dbo.LE_CartInfo AS C ON C.MediaID = W.RecID
                                                                               AND C.MediaType = 14002
                                                                               AND C.UserID = @UserID
                                ) AS A
                        ORDER BY A.CreateTime DESC";

            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<CartModel>(data.Tables[0]);
        }

        #endregion

        #region 批量删除购物车

        /// <summary>
        /// 批量删除购物车
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int DeleteCartInfo(DeleteCartInfoDto deleteArgs, int UserID)
        {
            int returnResult = 0;
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                try
                {
                    string SQL = @"DELETE FROM dbo.LE_CartInfo WHERE MediaType=@MediaType AND MediaID=@MediaID AND UserID=@UserID ";
                    foreach (CartModel entity in deleteArgs.CartInfoList)
                    {
                        var sqlParams = new SqlParameter[]{
                            new SqlParameter("@MediaID",entity.MediaID),
                            new SqlParameter("@UserID",UserID),
                            new SqlParameter("@MediaType",entity.MediaType)
                        };

                        SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
                    }

                    ts.Complete();
                    returnResult = 1;
                }
                catch (Exception ex)
                {

                }
            }
            return returnResult;
        }
        #endregion

        #region 根据用户ID获取购物车总数量

        /// <summary>
        /// 根据用户ID获取购物车总数量
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int GetCartCount(int UserID)
        {
            string SQL = @"SELECT COUNT(*) FROM dbo.LE_CartInfo WHERE UserID=@UserID";

            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };

            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

        }
        #endregion

        #region 添加购物车

        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public int AddCartInfo(List<CartModel> list, int UserID)
        {
            int returnResult = -1;
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                try
                {
                    string SQL = @"
                        IF NOT EXISTS ( SELECT  *
                                        FROM    LE_CartInfo
                                        WHERE   UserID = @UserID
                                                AND MediaID = @MediaID
                                                AND MediaType = @MediaType )
                            BEGIN
                                INSERT  INTO dbo.LE_CartInfo
                                        ( MediaType ,
                                          MediaID ,
                                          UserID ,
                                          Status ,
                                          CreateTime
                                        )
                                VALUES  ( @MediaType ,
                                          @MediaID ,
                                          @UserID ,
                                          0 ,
                                          GETDATE()
                                        );
                            END;";

                    foreach (CartModel entity in list)
                    {
                        var sqlParams = new SqlParameter[]{
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@UserID",UserID),
                        };

                        SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
                    }

                    ts.Complete();
                    returnResult = 1;
                }
                catch (Exception ex)
                {

                }
            }
            return returnResult;
        }

        #endregion

        /// <summary>
        /// 根据用户ID获取购物车中 微信媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<PublishlModel> GetCartWeiXin(int UserID)
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
                                CAST(W.IsOriginal AS INT) AS IsOriginal,
                                P.ADPosition1 ,
                                P.ADPosition2 ,
                                P.Price ,
                                P.MediaID,
                                W.TotalScores
                        FROM    dbo.LE_Weixin AS W
                                INNER JOIN dbo.LE_CartInfo AS C ON W.RecID = C.MediaID
                                                                   AND C.MediaType = 14001
                                LEFT JOIN dbo.LE_PublishDetailInfo AS P ON P.MediaID = W.RecID
                                                                            AND P.MediaType = 14001
                                LEFT JOIN dbo.DictInfo AS D ON D.DictId = W.CategoryID
                        WHERE   C.UserID = @UserID ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<PublishlModel>(data.Tables[0]);
        }

        /// <summary>
        /// 根据用户ID获取购物车 微博媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<WeiBoModel> GetCartWeiBo(int UserID)
        {
            string SQL = @"
                        SELECT  W.Number ,W.RecID,
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
                FROM    dbo.LE_Weibo AS W
                        LEFT JOIN dbo.DictInfo AS D ON D.DictId = W.CategoryID
                        INNER JOIN dbo.LE_CartInfo AS C ON W.RecID = C.MediaID
                                                           AND C.MediaType = 14003
                WHERE C.UserID = @UserID";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<WeiBoModel>(data.Tables[0]);
        }
        /// <summary>
        /// 根据用户ID获取购物车 APP媒体信息列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<AppModel> GetCartApp(int UserID)
        {
            string SQL = @"
                        SELECT  Name ,A.RecID,
                        HeadIconURL ,
                        DailyLive ,
                        Remark ,
                        CAST(IsMonitor AS INT) AS IsMonitor ,
                        CAST(IsLocate AS INT) AS IsLocate ,
                        TotalUser ,
                        D.DictName AS CategoryName ,
                        CAST(TimestampSign AS DATETIME) AS TimestampSign
                FROM    dbo.LE_APP AS A
                        INNER JOIN dbo.LE_CartInfo AS C ON C.MediaID = A.RecID
                                                           AND C.MediaType = 14002
                        LEFT JOIN dbo.DictInfo AS D ON D.DictId = A.CategoryID
                WHERE C.UserID = @UserID";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",UserID)
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);

            return DataTableToList<AppModel>(data.Tables[0]);
        }
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        public Tuple<int, List<TaskListModel>> TaskListInfo(QueryTaskargs queryArgs)
        {
            StringBuilder SQL = new StringBuilder(@"
                                                    SELECT  *
                                                    YanFaFROM    ( SELECT    TaskName ,
                                                                        REPLACE(MaterialUrl,'newscdn','www') as MaterialUrl ,
                                                                        ImgUrl ,
                                                                        Synopsis ,
                                                                        CreateTime
                                                              FROM      dbo.LE_TaskInfo
                                                            ) AS A ");

            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",queryArgs.PageSize),
                new SqlParameter("@CurPage",queryArgs.PageIndex),
                new SqlParameter("@Order"," CreateTime DESC ")
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<TaskListModel>>(totalCount, DataTableToList<TaskListModel>(data.Tables[0]));


        }
    }
}
