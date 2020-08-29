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
    public class ForwardMaterialDa : DataBase
    {
        #region 单例
        private ForwardMaterialDa() { }

        static ForwardMaterialDa instance = null;
        static readonly object padlock = new object();

        public static ForwardMaterialDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ForwardMaterialDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 物料转发—表头统计-饼图
        /// </summary>
        /// <returns></returns>
        public DataSet GetForwardStatistics_Pie(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ChannelName ,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(MaterialForwardCount,0) as MaterialForwardCount
                          FROM      dbo.Stat_ForwardSummary
                          WHERE     Status = 0
                                    AND ChannelId >= 0
                                    AND SceneId = 0
                                    AND MaterielTypeID = 0
                                    AND AAScoreType = 0
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime  ORDER BY MaterialForwardCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;

        }

        /// <summary>
        /// 物料转发—表头统计-柱状图
        /// </summary>
        /// <returns></returns>
        public DataSet GetForwardStatistics_Bar(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ChannelName ,
                                    Date ,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(MaterialForwardCount,0) as MaterialForwardCount
                          FROM      Stat_ForwardStatistics
                          WHERE     Status = 0
                                    AND ChannelId >=0
                                    AND Date BETWEEN @BeginTime AND @EndTime";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料转发—物料类型分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetForwardMateriel(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    MaterialName ,
                                    ChannelName,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(MaterialForwardCount,0) as MaterialForwardCount
                          FROM      dbo.Stat_ForwardSummary
                          WHERE     Status = 0
                                    AND ChannelId>=0
									AND MaterielTypeID>0
                                    AND AAScoreType = 0
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY MaterialForwardCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料转发—场景分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetForwardScene(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    SceneName ,
                                    ChannelName,
                                    ISNULL(SceneId,0) as SceneId,
                                    ISNULL(ChannelId,0) as ChannelId,
                                    ISNULL(MaterialForwardCount,0) as MaterialForwardCount
                          FROM      dbo.Stat_ForwardSummary
                          WHERE     Status = 0
                                    AND SceneId>0
                                    AND ChannelId>=0
									AND MaterielTypeID=0
                                    AND AAScoreType = 0
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY MaterialForwardCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料转发—账号分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetForwardAccount(BasicQueryArgs query)
        {
            string SQL = @"
                         SELECT D.DictName ,
                                ChannelName,
                                ISNULL(ChannelId,0) as ChannelId,
                                ISNULL(MaterialForwardCount,0) as MaterialForwardCount,D.OrderNum
                         FROM   Stat_ForwardSummary AS S
                                INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 76
                                           ) AS D ON D.DictId = S.AAScoreType
                         WHERE  S.Status = 0
                                AND ChannelId>=0
                                AND MaterielTypeID = 0
                                AND AAScoreType > 0
                                AND BeginTime = @BeginTime
                                AND EndTime = @EndTime  ORDER BY MaterialForwardCount desc  ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料转发—头部文章分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetForwardHeadEssay(BasicQueryArgs query)
        {
            string SQL = @"
                         SELECT D.DictName ,
                                ChannelName,
                                ISNULL(ChannelId,0) as ChannelId,
                                ISNULL(MaterialForwardCount,0) as MaterialForwardCount,D.OrderNum 
                         FROM   Stat_ForwardSummary AS S
                                INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 75
                                           ) AS D ON D.DictId = S.AAScoreType
                         WHERE  S.Status = 0
                                AND ChannelId>=0
                                AND MaterielTypeID = 0
                                AND AAScoreType > 0
                                AND BeginTime = @BeginTime
                                AND EndTime = @EndTime  ORDER BY MaterialForwardCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }

        /// <summary>
        /// 物料转发-日汇总列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetForwardStatisticsList(ListQueryArgs query)
        {
            string ProcedureName = "p_ForwardStatisticsList";

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
        /// 物料转发-明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetForwardDetailList(ListQueryArgs query)
        {
            string strSmallScore = string.Empty;
            string strBigScore = string.Empty;
            StringBuilder SQL = new StringBuilder(@"
            SELECT  MaterialID ,
                    Title ,
                    Url ,
                    MaterialTypeName ,
					EncapsulateTime,
					DistributeTime,
                    ForwardStatisticsTime,
					ChannelName,
                    SceneName ,
                    AccountScore ,
                    ArticleScore ,
					BrandNmae,
					SerialName,
					MaterialForwardCount
            YanFaFROM    Stat_ForwardData
            WHERE   Status=0");

            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                SQL.Append($" AND ForwardStatisticsTime>='{query.BeginTime}' ");
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                SQL.Append($" AND ForwardStatisticsTime<='{query.EndTime}' ");
            }
            if (query.MaterielTypeID > 0)
            {
                SQL.Append($" AND MaterialType ={query.MaterielTypeID } ");
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
                new SqlParameter("@Order"," EncapsulateTime DESC ")
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page", sqlParams);
            int totalCount = (int)(sqlParams[0].Value);
            return new Tuple<int, DataSet>(totalCount, data);
        }
    }
}
