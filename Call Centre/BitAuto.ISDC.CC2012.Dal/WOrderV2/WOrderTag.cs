using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WOrderTag : DataBase
    {
        public static WOrderTag Instance = new WOrderTag();

        public DataTable GetAllData(Entities.QueryWOrderTag query)
        {
            string sql = "SELECT  RecID ,BusiTypeID,TagName ,PID ,SortNum,Status FROM WOrderTag where status<>'-1'";
            List<SqlParameter> listParam = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(query.PID))
            {
                sql += " and PID=@PID";
                listParam.Add(new SqlParameter("@PID", SqlDbType.Int, 4) { SqlValue = query.PID });
            }

            if (!string.IsNullOrEmpty(query.RecID))
            {
                sql += " and RecID=@RecID";
                listParam.Add(new SqlParameter("@RecID", SqlDbType.Int, 4) { SqlValue = query.RecID });
            }
            if (!string.IsNullOrEmpty(query.NoRecID))
            {
                sql += " and RecID<>@NoRecID";
                listParam.Add(new SqlParameter("@NoRecID", SqlDbType.Int, 4) { SqlValue = query.NoRecID });
            }
            if (!string.IsNullOrEmpty(query.TagName))
            {
                sql += " and TagName=@TagName";
                listParam.Add(new SqlParameter("@TagName", SqlDbType.NVarChar, 50) { SqlValue = query.TagName });
            }
            if (!string.IsNullOrEmpty(query.BusiTypeID))
            {
                sql += " and BusiTypeID=@BusiTypeID";
                listParam.Add(new SqlParameter("@BusiTypeID", SqlDbType.Int, 4) { SqlValue = query.BusiTypeID });
            }
            if (!string.IsNullOrEmpty(query.Status))
            {
                //sql += " and Status in (select * from dbo.f_split(@status,','))";
                //listParam.Add(new SqlParameter("@status", SqlDbType.NVarChar, 50) { SqlValue = query.Status });
                sql += " and Status in (" + SqlFilter(query.Status) + ")";
            }

            sql += " ORDER BY SortNum";
            SqlParameter[] parameters = listParam.ToArray();

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 最大排序号
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum(string pid)
        {
            List<SqlParameter> listParam = new List<SqlParameter>();
            string sql = "SELECT max(SortNum) FROM WOrderTag where status<>'-1'";
            if (string.IsNullOrEmpty(pid))
            {
                sql += " and( PID is null or PID ='0')";
            }
            else
            {
                sql += " and PID=@PID";
                listParam.Add(new SqlParameter("@PID", SqlDbType.Int, 4) { SqlValue = pid });
            }

            SqlParameter[] parameters = listParam.ToArray();
            object max = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            try
            {
                int maxSort = Int32.Parse(max.ToString());
                return maxSort;
            }
            catch
            {
                return 1;
            }
        }
        /// <summary>
        /// 获取等级数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetLevelData(Entities.QueryWOrderTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            //child = ISNULL(STUFF(( SELECT  ','+ RTRIM(CONVERT(VARCHAR(MAX), TagName))
            //                    FROM    WOrderTag
            //                    WHERE   status <> '-1'
            //                            AND PID = x.RecID
            //                            {0}
            //                    ORDER BY SortNum
            //                  FOR XML PATH('')), 1, 1, ''), '')
            //TODO:CodeReview 2016-08-24 sql写法问题
            string sql = @"SELECT RecID ,BusiTypeID,TagName ,PID ,SortNum,Status,
                                 ISNULL(STUFF((SELECT ','+cast(TagName as varchar(max)) FROM WOrderTag WHERE status<>'-1' and  PID=x.RecID {0} ORDER BY SortNum
                                 FOR XML PATH('')),1,1,''),'') as child
                                 YanFaFROM WOrderTag x 
                                 WHERE status<>'-1' AND ISNULL(PID,0)=0";

            string sqlstatus = "";
            if (!string.IsNullOrEmpty(query.BusiTypeID))
            {
                sql += " and BusiTypeID=" + SqlFilter(query.BusiTypeID);
            }
            if (!string.IsNullOrEmpty(query.Status))
            {
                sqlstatus = " and Status in (" + SqlFilter(query.Status) + ")";
                sql += sqlstatus;
            }

            sql = string.Format(sql, sqlstatus);

            if (string.IsNullOrEmpty(order))
            {
                order = " SortNum asc ";
            }

            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sql;
            parameters[1].Value = order;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 获取标签名称
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public string GetTagNameByRecId(int recId)
        {
            string strSql = "SELECT tagName FROM WOrderTag WHERE RecId = " + recId;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 根据标签id获取标签路径：一级+二级
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        public DataTable GetTagNamePathByRecId(int recId)
        {
            string strSql = @"SELECT  (SELECT  TagName
                            FROM    WOrderTag
                            WHERE   RecID = ISNULL(a.pid, 0)) PTagName,a.TagName
                            FROM    WOrderTag AS a
                            WHERE   a.RecId = " + recId;

            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// 获取每个业务类型的标签数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetBusiTagCount(Entities.QueryWOrderTag query)
        {
            string strSql = @"SELECT BusiTypeID, COUNT(1)as num 
                                    FROM WOrderTag 
                                    WHERE status<>'-1' {0} 
                                    GROUP BY BusiTypeID 
                                    ORDER BY num desc ";

            string where = "";
            if (!string.IsNullOrEmpty(query.PID))
            {
                where = " and PID=" + SqlFilter(query.PID) + "";
            }
            strSql = string.Format(strSql, where);

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds.Tables[0];
        }
    }
}
