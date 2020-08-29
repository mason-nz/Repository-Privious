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
    public class FunnelMaterialDa : DataBase
    {
        #region 初始化
        private FunnelMaterialDa() { }

        static FunnelMaterialDa instance = null;
        static readonly object padlock = new object();

        public static FunnelMaterialDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new FunnelMaterialDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 漏斗分析—头部列表

        public DataSet GetFunnel_Head_List(BasicQueryArgs queryArgs)
        {
            StringBuilder SQL = new StringBuilder(@"
              SELECT    SceneName ,
                        ChannelName ,
                        S.DictName AS AAScoreTypeName ,
                        SpiderArticleCount ,
                        SpiderAccountCount ,
                        AutoArticleCount ,
                        AutoAccountCount ,
                        PrimaryArticleCount ,
                        PrimaryAccountCount ,
                        ArtificialArticleCount ,
                        ArtificialAccountCount ,
                        EncapsulateArticleCount ,
                        EncapsulateAccountCount
              FROM      dbo.Stat_Funnel_Head AS F
                        LEFT JOIN ( SELECT  *
                                    FROM    DictInfo
                                    WHERE   DictType = 75
                                  ) AS S ON F.AAScoreType = S.DictId
              WHERE     F.Status = 0
                        AND F.BeginTime = @BeginTime
                        AND F.EndTime = @EndTime ");
            string OrderFile = " SpiderArticleCount DESC";
            if (queryArgs.Operator == 1) //场景
            {
                SQL.Append(" AND F.ChannelID=0 AND F.AAScoreType=0 AND F.SceneId>0 ");
            }
            if (queryArgs.Operator == 2) //渠道
            {
                SQL.Append(" AND F.ChannelID>0 AND F.AAScoreType=0 AND F.SceneId=0 ");
            }
            if (queryArgs.Operator == 3) //文章分值
            {
                OrderFile = " AutoArticleCount DESC";
                SQL.Append(" AND F.ChannelID=0 AND F.AAScoreType>0 AND F.SceneId=0 ");
            }
            SQL.Append($" ORDER BY {OrderFile} ");
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",queryArgs.BeginTime),
                new SqlParameter("@EndTime",queryArgs.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL.ToString(), sqlParams);
            return data;
        }
        #endregion

        #region 漏斗分析—腰部列表

        public DataSet GetFunnel_Waist_List(BasicQueryArgs queryArgs)
        {
            StringBuilder SQL = new StringBuilder(@"
				 SELECT Category ,
				        ChannelName ,
				        SpiderCount ,
				        AutoCleanCount ,
				        MatchedCount ,
				        ArtificialCount ,
				        EncapsulateCount
				 FROM   Stat_Funnel_Waist
				 WHERE  Status = 0
				        AND BeginTime = @BeginTime
				        AND EndTime = @EndTime ");
            if (queryArgs.Operator == 1) //文章类别
            {
                SQL.Append(" AND ChannelID=0 AND  Category IS NOT NULL");
            }
            if (queryArgs.Operator == 2) //渠道
            {
                 SQL.Append("  AND ChannelID>0 AND  Category IS NULL  ");
            }
            SQL.Append(" ORDER BY SpiderCount DESC "); 
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",queryArgs.BeginTime),
                new SqlParameter("@EndTime",queryArgs.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL.ToString(), sqlParams);
            return data;
        }
        #endregion


        #region 漏斗分析—物料列表

        public DataSet GetFunnel_Material_List(BasicQueryArgs queryArgs)
        {
            StringBuilder SQL = new StringBuilder(@"
                 SELECT SceneName ,
                        ChannelName ,
                        S.DictName AS AAScoreTypeName ,
                        Encapsulate ,
                        Distribute ,
                        Clue ,
                        Forward
                 FROM   Stat_Funnel_Material AS F
                        LEFT JOIN ( SELECT  *
                                    FROM    DictInfo
                                    WHERE   DictType = 75
                                  ) AS S ON F.AAScoreType = S.DictId
                 WHERE  F.Status = 0
                        AND F.BeginTime = @BeginTime
                        AND F.EndTime = @EndTime ");
            if (queryArgs.Operator == 1) //场景
            {
                SQL.Append(" AND F.ChannelID=0 AND F.AAScoreType=0 AND F.SceneId>0 ");
            }
            if (queryArgs.Operator == 2)//渠道
            {
                SQL.Append(" AND F.ChannelID>0 AND F.AAScoreType=0 AND F.SceneId=0 ");
            }
            if (queryArgs.Operator == 3)//文章分值
            {
                SQL.Append(" AND F.ChannelID=0 AND F.AAScoreType>0 AND F.SceneId=0 ");
            }
            SQL.Append(" ORDER BY Encapsulate DESC ");
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@BeginTime",queryArgs.BeginTime),
                new SqlParameter("@EndTime",queryArgs.EndTime)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL.ToString(), sqlParams);
            return data;
        }
        #endregion
    }
}
