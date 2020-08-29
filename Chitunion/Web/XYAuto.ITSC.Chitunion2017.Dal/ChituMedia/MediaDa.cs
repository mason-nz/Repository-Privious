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
    public class MediaDa : DataBase
    {
        #region 单例
        private MediaDa() { }

        static MediaDa instance = null;
        static readonly object padlock = new object();

        public static MediaDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new MediaDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public int GetWeiXinCount(LuceneQuery query)
        {
            string SQL = "SELECT COUNT(*) FROM dbo.LE_WeiXin WHERE CAST(TimestampSign AS DATETIME)>@LastTime";


            var sqlParams = new SqlParameter[]{
                new SqlParameter("@LastTime",query.LastTime)
            };

            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);


        }

        /// <summary>
        /// 获取微信列表数据
        /// </summary>
        /// <returns></returns>
        public Tuple<int, List<WeiXinLuceneModel>> GetWeiXinLuceneList(LuceneQuery query)
        {
            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@PageIndex",query.PageIndex),
                new SqlParameter("@PageSize",query.PageSize),
                new SqlParameter("@LastTime",query.LastTime)
            };
            int totalCount = 0;
            //var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LE_LuceneWeiXin", sqlParams);

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(CONNECTIONSTRINGS))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("p_LE_LuceneWeiXin", con))
                {
                    cmd.Parameters.AddRange(sqlParams);
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        totalCount = (int)(sqlParams[0].Value);
                    }

                }
            }

            //var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LE_LuceneWeiXin", sqlParams);
            //int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<WeiXinLuceneModel>>(totalCount, DataTableToList<WeiXinLuceneModel>(dt));
        }


        /// <summary>
        /// 获取微博列表数据
        /// </summary>
        /// <returns></returns>
        public Tuple<int, List<WeiBoModel>> GetWeiBoLuceneList(LuceneQuery query)
        {
            string SQL = $@"
                            SELECT  *
                            YanFaFROM    ( SELECT    
                                                LE.RecID ,
                                                Number ,
                                                Name ,
                                                Sex ,
                                                HeadIconURL ,
                                                FansCount ,
                                                FansSex ,
                                                CategoryID ,
                                                Sign ,
                                                ForwardAvg ,
                                                CommentAvg ,
                                                LikeAvg ,
						                        LE.AuthType,
                                                CAST(LE.IsReserve AS INT) AS IsReserve,
                                                DirectPrice ,
                                                ForwardPrice ,
                                                AM.ProvinceID ,
                                                AM.CityID ,
                                                CategoryDic.DictName AS CategoryName ,
                                                CAST(TimestampSign AS DATETIME) AS TimestampSign,
                                                LE.TotalScores,
                                                LE.Summary,
                                                LE.TagText
                                      FROM      dbo.LE_Weibo AS LE WITH(NOLOCK)
                                                LEFT JOIN dbo.DictInfo AS CategoryDic WITH ( NOLOCK ) ON CategoryDic.DictId = LE.CategoryID
                                                LEFT JOIN dbo.LE_MediaArea_Mapping AS AM WITH ( NOLOCK ) ON AM.MediaType = 14003
                                                                                            AND LE.RecID = AM.MediaID
                                                                                            AND AM.RelateType = 59001
                                                WHERE LE.Status>=0 AND CAST(LE.TimestampSign AS DATETIME)>'{query.LastTime}'
                                    ) AS A ";

            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order","  TimestampSign DESC ")
            };
            int totalCount = 0;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(CONNECTIONSTRINGS))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("p_Page", con))
                {
                    cmd.Parameters.AddRange(sqlParams);
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        totalCount = (int)(sqlParams[0].Value);
                    }

                }
            }

            return new Tuple<int, List<WeiBoModel>>(totalCount, DataTableToList<WeiBoModel>(dt));

        }

        /// <summary>
        /// 获取APP列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, List<AppModel>> GetAppLuceneList(MediaQueryArgs query, List<TagVehicleInfoList> listKeyword)
        {
            StringBuilder SQL = new StringBuilder($@"
                            SELECT  *
                            YanFaFROM    ( SELECT    
                                                RecID,
                                                Name ,
                                                HeadIconURL ,
                                                DailyLive ,
                                                Remark ,
                                                CAST(IsMonitor AS INT) AS IsMonitor ,
                                                CAST(IsLocate AS INT) AS IsLocate ,
                                                TotalUser,
												D.DictName AS CategoryName ,
                                                CAST(TimestampSign AS DATETIME) AS TimestampSign,
                                                LE_APP.CreateTime,
                                                LE_APP.TagText
                                      FROM      dbo.LE_APP WITH ( NOLOCK )
									  LEFT JOIN dbo.DictInfo AS D WITH ( NOLOCK ) ON d.DictId=dbo.LE_APP.CategoryID
                                      WHERE     LE_APP.Status>=0 
                                     ");
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                SQL.Append($" AND (Name LIKE '%{query.Keyword}%' OR  Remark LIKE '%{query.Keyword}%' OR D.DictName LIKE '%{query.Keyword}%' ");
                if (listKeyword.Count > 0)
                {
                    foreach (TagVehicleInfoList item in listKeyword)
                    {
                        SQL.Append($" OR TagText Like '%{item.MediaTagName}%' ");
                    }
                }
                SQL.Append(" )");
            }

            if (query.CategoryID > 0)
            {
                SQL.Append($" AND CategoryID ={query.CategoryID} ");
            }
            SQL.Append($" ) AS A");

            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order","  TotalUser DESC ")
            };

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);

            return new Tuple<int, List<AppModel>>(totalCount, DataTableToList<AppModel>(data.Tables[0]));


        }
    }
}
