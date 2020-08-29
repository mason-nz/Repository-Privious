using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync.DAL
{
    public class ArticleInfo : XYAuto.ITSC.Chitunion2017.Dal.DataBase
    {
        public static readonly ArticleInfo Instance = new ArticleInfo();
        private string Conn_BaseData = ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];
        private string Conn_OPData = ConfigurationManager.AppSettings["ConnectionStrings_OPData"];

        internal bool IsExistByTitle(string title)
        {
            string sql = string.Format(@"
SELECT COUNT(*) FROM dbo.ArticleInfo
WHERE Title='{0}'", StringHelper.SqlFilter(title));
            DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0
                    && int.Parse(ds.Tables[0].Rows[0][0].ToString()) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        internal DataTable GetTestData(int recid)
        {
            string sql = $@"SELECT TOP 500
    RecID,HeadImg,Content
FROM dbo.ArticleInfo
WHERE CreateTime> DATEADD(DAY, -10, GETDATE())
{(recid > 0 ? " AND RecID > " + recid + " " : " ")}
ORDER BY RecID";
            DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        internal decimal? ComputeArticleValueByNumber(string number, int resource)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Number", SqlDbType.VarChar),
                new SqlParameter("@Resource", SqlDbType.Int)
            };
            parameters[0].Value = number;
            parameters[1].Value = resource;
            DataSet ds = SqlHelper.ExecuteDataset(Conn_OPData, CommandType.StoredProcedure, "p_ComputeAccountArticleValueByNumber", parameters);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return decimal.Parse(ds.Tables[0].Rows[0]["ArticleScore"].ToString());
                }
            }
            return null;
        }

        /// <summary>
        /// 获取未领用过的文章数据
        /// </summary>
        /// <param name="recid">文章主键ID</param>
        /// <param name="topNum">获取数量</param>
        /// <returns></returns>
        internal DataTable GetKeyWordsData(int recid, int topNum)
        {
            string sql = $@"SELECT  TOP {topNum}
ai.RecID,
ai.Title,
ai.Content,
ai.XyAttr
FROM BaseData2017.dbo.ArticleInfo AI
WHERE AI.RecID NOT IN
      (
          SELECT AA.ArticleID
          FROM Chitunion_OP2017.dbo.AccountArticle AA
      )
	  AND ai.RecID>{recid}
ORDER BY ai.RecID";
            DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }


        internal DataTable GetHistoryHeadImgData(int recid, int topNum)
        {
            string sql = $@"SELECT TOP {topNum} RecID,HeadImg FROM BaseData2017.dbo.ArticleInfo
WHERE Resource=1 --AND CreateTime<='2018-01-15 14:20:05'
AND CreateTime>'2017-12-01'
AND HeadImg!='' AND HeadImgNew='' 
" + (recid > 0 ? $" AND RecID<{recid} " : "") + " ORDER BY RecID DESC";
            DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        internal int UpdateHeadImgNewByRecID(int recid, string headimgNew)
        {
            string sql = $@"UPDATE BaseData2017.dbo.ArticleInfo
SET HeadImgNew = '" + StringHelper.SqlFilter(headimgNew) + $@"'WHERE RecID={recid}";
            return SqlHelper.ExecuteNonQuery(Conn_BaseData, CommandType.Text, sql);
        }
    }
}
