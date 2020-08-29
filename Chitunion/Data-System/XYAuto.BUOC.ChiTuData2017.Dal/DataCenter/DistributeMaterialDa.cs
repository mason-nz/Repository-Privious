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
    public class DistributeMaterialDa: DataBase
    {
        #region 单例
        private DistributeMaterialDa() { }

        static DistributeMaterialDa instance = null;
        static readonly object padlock = new object();

        public static DistributeMaterialDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DistributeMaterialDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 物料分发—表头统计-柱状图
        /// </summary>
        /// <returns></returns>
        public DataSet GetDistributeStatistics_Bar(BasicQueryArgs query)
        {
            string SQL = @"
                        SELECT  ChannelName ,
                                ISNULL(ChannelId,0) as ChannelId,
                                ISNULL(DistributeCount,0) as DistributeCount,
                                Date
                        FROM    Stat_DistributeStatistics
                        WHERE   Status = 0
                                AND ChannelId >= 0
                                AND SceneId = 0 AND Date BETWEEN @BeginTime AND @EndTime  ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }


        /// <summary>
        /// 物料分发—物料类型分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetDistributeMateriel(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    MaterialName ,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(DistributeCount,0) as DistributeCount,
                                    ChannelName
                          FROM      dbo.Stat_DistributeSummary
                          WHERE     Status = 0
                                    AND MaterielTypeID>0
									AND ChannelId>=0
                                    AND SceneId=0
                                    AND AAScoreType = 0 
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY DistributeCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料分发—场景分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetDistributeScene(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    SceneName ,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(DistributeCount,0) as DistributeCount,
                                    ChannelName
                          FROM      dbo.Stat_DistributeSummary
                          WHERE     Status = 0
                                    AND SceneId>0
									AND ChannelId>=0
                                    AND MaterielTypeID=0 
                                    AND AAScoreType = 0 
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY DistributeCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料分发—账号分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetDistributeAccount(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    D.DictName ,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(DistributeCount,0) as DistributeCount,
                                    ChannelName,D.OrderNum
                          FROM      dbo.Stat_DistributeSummary AS S
                                    INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 76
                                           ) AS D ON D.DictId = S.AAScoreType
                          WHERE     S.Status = 0
                                    AND SceneId=0
									AND ChannelId>=0
                                    AND MaterielTypeID=0 
                                    AND AAScoreType > 0 
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY DistributeCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料分发—头部文章分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetDistributeHeadEssay(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    D.DictName ,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(DistributeCount,0) as DistributeCount,
                                    ChannelName,D.OrderNum
                          FROM      dbo.Stat_DistributeSummary AS S
                                    INNER JOIN ( SELECT *
                                                 FROM   DictInfo
                                                 WHERE  DictType = 75
                                               ) AS D ON D.DictId = S.AAScoreType
                          WHERE     S.Status = 0
                                    AND SceneId=0
									AND ChannelId>=0
                                    AND MaterielTypeID=0 
                                    AND AAScoreType > 0 
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY DistributeCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }

        /// <summary>
        /// 物料分发-日汇总列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetDistributeStatisticsList(ListQueryArgs query)
        {
            string ProcedureName = "p_DistributeStatisticsList";

            var outParam = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
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
        /// 物料分发-明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetDistributeDetailList(ListQueryArgs query)
        {
            string strSmallScore = string.Empty;
            string strBigScore = string.Empty;
            StringBuilder SQL = new StringBuilder(@"
            SELECT  MaterialID ,
                    Title ,
                    Url ,
                    MaterialTypeName ,
                    SceneName ,
                    AccountScore ,
                    ArticleScore ,
					BrandNmae,
					SerialName,
					EncapsulateTime,
					DistributeTime,
					ChannelName
            YanFaFROM    dbo.Stat_DistributeData
            WHERE   Status=0 ");

            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                SQL.Append($" AND DistributeTime>='{query.BeginTime}' ");
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                SQL.Append($" AND DistributeTime<'{Convert.ToDateTime(query.EndTime).AddDays(1).ToString("yyyy-MM-dd")}' ");
            }
            if (query.MaterielTypeID > 0)
            {
                SQL.Append($" AND MaterialTypeID ={query.MaterielTypeID } ");
            }
           
            if (query.ChannelID > 0)
            {
                SQL.Append($" AND ChannelID ={query.ChannelID} ");
            }
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL+string.Empty),
                new SqlParameter("@PageRows",query.PageSize),
                new SqlParameter("@CurPage",query.PageIndex),
                new SqlParameter("@Order"," DistributeTime DESC ")
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);
            return new Tuple<int, DataSet>(totalCount, data);
        }
    }
}
