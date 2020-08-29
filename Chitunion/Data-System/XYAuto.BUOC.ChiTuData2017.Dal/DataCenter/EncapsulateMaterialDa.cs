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
    public class EncapsulateMaterialDa : DataBase
    {
        #region 单例
        private EncapsulateMaterialDa() { }

        static EncapsulateMaterialDa instance = null;
        static readonly object padlock = new object();

        public static EncapsulateMaterialDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new EncapsulateMaterialDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 物料封装—表头统计-饼图
        /// </summary>
        /// <returns></returns>
        public DataSet GetEncapsulateStatistics_Pie(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ISNULL(MaterielTypeID,0) as MaterielTypeID,
                                    ISNULL(EncapsulateCount,0) as EncapsulateCount,
                                    MaterielTypeName
                          FROM      dbo.Stat_EncapsulateSummary
                          WHERE     Status = 0
                                    AND MaterielTypeID >= 0
                                    AND ConditionName IS NULL
                                    AND SceneId = 0
                                    AND ConditionId=0
                                    AND AAScoreType = 0
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY EncapsulateCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;

        }

        /// <summary>
        /// 物料封装—表头统计-柱状图
        /// </summary>
        /// <returns></returns>
        public DataSet GetEncapsulateStatistics_Bar(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    MaterielTypeName ,
                                    Date ,
                                    EncapsulateCount,
                                    ISNULL(MaterielTypeID,0) as MaterielTypeID,
                                    ISNULL(EncapsulateCount,0) as EncapsulateCount
                          FROM      Stat_EncapsulateStatistics
                          WHERE     Status = 0
                                    AND Date BETWEEN @BeginTime AND @EndTime  ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料封装—场景分布图
        /// </summary>
        /// <returns></returns>
        public DataSet GetEncapsulateScene(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    SceneName ,
                                    MaterielTypeName,
                                    ISNULL(MaterielTypeID,0) as MaterielTypeID,
                                    ISNULL(EncapsulateCount,0) as EncapsulateCount
                          FROM      dbo.Stat_EncapsulateSummary
                          WHERE     Status = 0
									AND MaterielTypeID >= 0
                                    AND SceneId>0
                                    AND ConditionId=0
                                    AND AAScoreType = 0
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime  ORDER BY EncapsulateCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料封装—账号分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetEncapsulateAccount(BasicQueryArgs query)
        {
            string SQL = @"
                         SELECT D.DictName ,
                                MaterielTypeName,
                                ISNULL(MaterielTypeID,0) as MaterielTypeID,
                                ISNULL(EncapsulateCount,0) as EncapsulateCount,D.OrderNum
                         FROM   Stat_EncapsulateSummary AS S
                                INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 76
                                           ) AS D ON D.DictId = S.AAScoreType
                         WHERE  S.Status = 0
								AND MaterielTypeID >= 0
                                AND SceneId = 0
                                AND ConditionId=0
                                AND AAScoreType > 0
                                AND BeginTime = @BeginTime
                                AND EndTime = @EndTime  ORDER BY EncapsulateCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料封装—头部文章分值图
        /// </summary>
        /// <returns></returns>
        public DataSet GetEncapsulateHeadEssay(BasicQueryArgs query)
        {
            string SQL = @"
                         SELECT D.DictName ,
                                MaterielTypeName,
                                ISNULL(MaterielTypeID,0) as MaterielTypeID,
                                ISNULL(EncapsulateCount,0) as EncapsulateCount,D.OrderNum
                         FROM   Stat_EncapsulateSummary AS S
                                INNER JOIN ( SELECT *
                                             FROM   DictInfo
                                             WHERE  DictType = 75
                                           ) AS D ON D.DictId = S.AAScoreType
                         WHERE  S.Status = 0
								AND MaterielTypeID >= 0
                                AND SceneId = 0
                                AND ConditionId=0
                                AND AAScoreType > 0
                                AND BeginTime = @BeginTime
                                AND EndTime = @EndTime ORDER BY EncapsulateCount desc ";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料封装-状态分布图
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet GetEncapsulateCondition(BasicQueryArgs query)
        {
            string SQL = @"
                          SELECT    ConditionName ,
                                    MaterielTypeName,
                                    ISNULL(MaterielTypeID,0) as MaterielTypeID,
                                    ISNULL(EncapsulateCount,0) as EncapsulateCount
                          FROM      dbo.Stat_EncapsulateSummary
                          WHERE     Status = 0
                                    AND ConditionName IS NOT NULL
									AND MaterielTypeID >= 0
                                    AND SceneId = 0
                                    AND AAScoreType = 0
                                    AND BeginTime = @BeginTime
                                    AND EndTime = @EndTime ORDER BY EncapsulateCount desc";
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",query.BeginTime),
                new SqlParameter("@EndTime",query.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL, sqlParams);
            return data;
        }
        /// <summary>
        /// 物料封装-日汇总列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetEncapsulateStatisticsList(ListQueryArgs query)
        {
            string ProcedureName = "p_EncapsulateStatisticsList";

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
        /// 物料封装-明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Tuple<int, DataSet> GetEncapsulateDetailList(ListQueryArgs query)
        {
            string strSmallScore = string.Empty;
            string strBigScore = string.Empty;
            StringBuilder SQL = new StringBuilder(@"
            SELECT  MaterialID ,
                    Title ,
                    Url ,
                    MaterialName ,
                    EncapsulateTime ,
                    ChannelName ,
                    SceneName,
                    AccountName ,
                    AccountScore ,
                    ArticleScore ,
                    BrandNmae,
                    SerialName ,
                    ConditionName ,
                    Reason
            YanFaFROM    dbo.Stat_EncapsulateData
            WHERE   Status=0 ");

            if (!string.IsNullOrEmpty(query.BeginTime))
            {
                SQL.Append($" AND EncapsulateTime>='{query.BeginTime}' ");
            }
            if (!string.IsNullOrEmpty(query.EndTime))
            {
                SQL.Append($" AND EncapsulateTime<='{Convert.ToDateTime(query.EndTime).AddDays(1).ToString("yyyy-MM-dd")}' ");
            }
            if (query.MaterielTypeID > 0)
            {
                SQL.Append($" AND MaterielTypeID ={query.MaterielTypeID } ");
            }
            if (query.SceneID > 0)
            {
                SQL.Append($" AND SceneId ={query.SceneID} ");
            }
            if (!string.IsNullOrEmpty(query.AccountName))
            {
                strSmallScore = query.AccountName.Split('-')[0];
                strBigScore = query.AccountName.Split('-')[1];
                SQL.Append($" AND AccountScore >{query.AccountName.Split('-')[0]} AND  AccountScore <={query.AccountName.Split('-')[1]}");
            }
            if (query.ChannelID > 0)
            {
                SQL.Append($" AND ChannelID ={query.ChannelID} ");
            }
            if (query.ConditionID > 0)
            {
                SQL.Append($" AND ConditionId ={query.ConditionID} ");
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
