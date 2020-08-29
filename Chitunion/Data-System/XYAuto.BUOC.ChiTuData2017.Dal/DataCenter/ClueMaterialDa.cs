using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.DataCenter
{
    public class ClueMaterialDa:DataBase
    {
        #region 单例
        private ClueMaterialDa() { }

        static ClueMaterialDa instance = null;
        static readonly object padlock = new object();

        public static ClueMaterialDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ClueMaterialDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 物料线索—表头统计-饼图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueStatistics_Pie(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ISNULL(ChannelID, 0) AS ChannelID ,
                                    ISNULL(ClueTypeID, 0) AS ClueTypeID ,
                                    ChannelName ,
                                    ClueTypeName ,
                                    ISNULL(ClueCount, 0) AS ClueCount
                          FROM      Stat_ClueSummary
                          WHERE     Status = 0
                                    AND SceneId = 0
                                    AND AAScoreType = 0
                                    AND MaterielTypeID = 0
                                    AND ( ChannelID >= 0
                                          OR ClueTypeID > 0
                                        )
                                    AND SceneName IS NULL
                                    AND MaterialName IS NULL
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY ClueCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;

        }
        
        /// <summary>
        /// 物料线索—表头统计-柱状图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueStatistics_Bar(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ClueTypeName ,
                                    ChannelName,
                                    Date ,
                                    ISNULL(ClueCount,0) as  ClueCount ,
                                    ISNULL(ChannelID,0) as  ChannelID
                          FROM      Stat_ClueStatistics
                          WHERE     Status = 0
                                    AND ClueTypeID > 0
                                    AND ChannelID >=0
                                    AND Date BETWEEN @BeginTime AND @EndTime ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }

        /// <summary>
        /// 数据概况—线索-柱状图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueStatisticsIndex_Bar(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ClueTypeName ,
                                    ChannelName,
                                    Date ,
                                    ISNULL(ClueCount,0) as  ClueCount ,
                                    ISNULL(ChannelID,0) as  ChannelID
                          FROM      Stat_ClueStatistics
                          WHERE     Status = 0
                                    AND ClueTypeID = 0
                                    AND ChannelID >=0
                                    AND Date BETWEEN @BeginTime AND @EndTime ORDER BY ClueCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }

        /// <summary>
        /// 物料线索—物料类型分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueMateriel(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    MaterialName ,
                                    ISNULL(ClueCount,0) as  ClueCount,
                                    ISNULL(ClueTypeID,0) as  ClueTypeID,
                                    ClueTypeName
                          FROM      Stat_ClueSummary
                          WHERE     Status = 0
                                    AND ClueTypeID>=0
                                    AND SceneId=0
									AND MaterielTypeID>0
                                    AND AAScoreType = 0
									AND ChannelID=0
                                    AND MaterialName is not null
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY ClueCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料线索—场景分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueScene(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    SceneName ,
                                    ISNULL(ClueCount,0) as  ClueCount,
                                    ISNULL(ClueTypeID,0) as  ClueTypeID,
                                    ClueTypeName
                          FROM      Stat_ClueSummary
                          WHERE     Status = 0
                                    AND ClueTypeID>=0
                                    AND SceneId>0
									AND MaterielTypeID=0
                                    AND AAScoreType = 0
									AND ChannelID=0
                                    AND SceneName IS NOT NULL
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY ClueCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料线索发—账号分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueAccount(BasicQueryArgs query)
        {
            string SQL = @"
                         SELECT D.DictName ,
                                ISNULL(S.ClueCount,0) as  ClueCount,
                                ISNULL(ClueTypeID,0) as  ClueTypeID,
                                ClueTypeName,D.OrderNum
                         FROM   Stat_ClueSummary AS S
                                INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 76
                                           ) AS D ON D.DictId = S.AAScoreType
                         WHERE  S.Status = 0
                                AND SceneId = 0
                                AND ClueTypeID>=0
                                AND MaterielTypeID = 0
                                AND AAScoreType > 0
                                AND ChannelID = 0
                                AND BeginTime = @BeginTime
                                AND EndTime = @EndTime  ORDER BY ClueCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料线索—头部文章分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetClueHeadEssay(BasicQueryArgs query)
        {
            string SQL = @"
                         SELECT D.DictName ,
                                ISNULL(S.ClueCount,0) as  ClueCount,
                                ISNULL(ClueTypeID,0) as  ClueTypeID,
                                ClueTypeName,D.OrderNum
                         FROM   Stat_ClueSummary AS S
                                INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 75
                                           ) AS D ON D.DictId = S.AAScoreType
                         WHERE  S.Status = 0
                                AND ClueTypeID>=0
                                AND SceneId = 0
                                AND MaterielTypeID = 0
                                AND AAScoreType > 0
                                AND ChannelID = 0
                                AND BeginTime = @BeginTime
                                AND EndTime = @EndTime  ORDER BY ClueCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }

        /// <summary>
        /// 物料线索-日汇总列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetClueStatisticsList(ListQueryArgs query)
        {
            string ProcedureName = "p_ClueStatisticsList";

            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@ChannelID",query.ChannelID),
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime),
                new SqlParameter("@PageSize",query.PageSize),
                new SqlParameter("@PageIndex",query.PageIndex)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, ProcedureName, sqlParams);
            int totalCount = (int)(sqlParams[0].Value);
            return new Tuple<int, DataSet>(totalCount, data);

        }
        /// <summary>
        /// 物料线索-明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetClueDetailList(ListQueryArgs query)
        {
            string strSmallScore = string.Empty;
            string strBigScore = string.Empty;
            StringBuilder SQL = new StringBuilder(@"
            SELECT  ClueDate,
					MaterialID ,
                    Title ,
                    Url ,
                    MaterialName ,
                    SceneName ,
                    AccountScore ,
                    ArticleScore ,
					ChannelName,
					BrandNmae,
					SerialName,
					InqueryCount,
					SessionCount,
					TelConnectCount
            YanFaFROM    Stat_ClueData
            WHERE   Status=0");

            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                SQL.Append($" AND ClueDate>='{query.BeginTime}' ");
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                SQL.Append($" AND ClueDate<='{query.EndTime}' ");
            }

            if (query.MaterielTypeID > 0)
            {
                SQL.Append($" AND MaterialType ={query.MaterielTypeID } ");
            }
            if (query.ChannelID > 0)
            {
                SQL.Append($" AND ChannelID ={query.ChannelID } ");
            }

            if (!string.IsNullOrEmpty(query.AccountName))
            {
                strSmallScore = query.AccountName.Split('-')[0];
                strBigScore = query.AccountName.Split('-')[1];
                SQL.Append($" AND AccountScore >{query.AccountName.Split('-')[0]} AND  AccountScore <={query.AccountName.Split('-')[1]}");

            }
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order"," ClueDate DESC ")
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);
            return new Tuple<int, DataSet>(totalCount, data);
        }
    }
}
