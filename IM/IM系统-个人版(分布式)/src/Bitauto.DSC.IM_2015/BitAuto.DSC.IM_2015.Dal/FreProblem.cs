using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类FreProblem。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:03 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class FreProblem : DataBase
    {
        public static readonly FreProblem Instance = new FreProblem();

        protected FreProblem()
        { }

        /// 按照查询条件查询
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetAllFreProblem(int top, int SourceType)
        {
            string sql = @"SELECT TOP " + top + @" * FROM FreProblem
                                    WHERE Status=0 and (SourceType like '%," + SourceType + "%' or sourcetype ='" + SourceType + "' or sourcetype like '" + SourceType + ",%') ORDER BY SortNum,RecID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 按照查询条件查询
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetAllFreProblem(int top)
        {
            string sql = @"SELECT TOP " + top + @" * FROM FreProblem
                                    WHERE Status=0 ORDER BY SortNum,RecID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 获取最大的排序号
        /// <summary>
        /// 获取最大的排序号
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum()
        {
            string sql = "select isnull(max(sortnum),0) from dbo.freproblem where status=0";
            return CommonFunc.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }
        /// 上下移动数据
        /// <summary>
        /// 上下移动数据
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1上-1下</param>
        /// <returns></returns>
        public bool MoveUpOrDown(int recid, int sortnum, int type)
        {
            int next_recid = 0, next_sortnum = 0;
            string sql = "";
            //上移
            if (type > 0)
            {
                //获取前一位数据
                sql = "select top 1 recid,sortnum from dbo.freproblem where status=0 and sortnum<" + sortnum + " order by sortnum desc";
            }
            //下移
            else if (type < 0)
            {
                //获取后一位数据
                sql = "select top 1 recid,sortnum from dbo.freproblem where status=0 and sortnum>" + sortnum + " order by sortnum asc";
            }
            else return false;
            //查询数据
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                next_recid = CommonFunc.ObjectToInteger(dt.Rows[0]["recid"]);
                next_sortnum = CommonFunc.ObjectToInteger(dt.Rows[0]["sortnum"]);
            }
            //交换数据
            string sql1 = "update dbo.freproblem set sortnum=" + next_sortnum + " where recid=" + recid;
            string sql2 = "update dbo.freproblem set sortnum=" + sortnum + " where recid=" + next_recid;
            int i = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql1);
            i += SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql2);
            return i == 2;
        }
        /// 获取所有数据记录
        /// <summary>
        /// 获取所有数据记录
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            string sql = "select isnull(count(*),0) from dbo.freproblem where status=0";
            return CommonFunc.ObjectToInteger(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }

        /// <summary>
        /// 常见问题
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetFreProblem(QueryFreProblem query, string order, int currentPage, int pageSize, out int totalCount)
        {

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};


            parameters[0].Value = BuildWhere(query); ;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_FreProblem_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string BuildWhere(QueryFreProblem query)
        {
            string strwhere = "";
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                strwhere += " AND RecID=" + query.RecID;
            }
            if (!string.IsNullOrEmpty(query.Title))
            {
                strwhere += " AND Title like '%" + StringHelper.SqlFilter(query.Title) + "%'";
            }
            if (!string.IsNullOrEmpty(query.Remark))
            {
                strwhere += " AND Remark like '%" + StringHelper.SqlFilter(query.Remark) + "%'";
            }

            if (!string.IsNullOrEmpty(query.SourceType))
            {
                string str = "AND (";
                var arrary = query.SourceType.Split(',');
                for (int i = 0; i < arrary.Length; i++)
                {
                    str += " (SourceType like '%," + StringHelper.SqlFilter(arrary[i]) + "%' or sourcetype ='"+StringHelper.SqlFilter(arrary[i])+"' or sourcetype like '"+StringHelper.SqlFilter(arrary[i])+",%') or ";
                }
                //sourcetype+',' like '%,6,%' or sourcetype ='6' or sourcetype like '6,%'
                str = str + " 1=2)";
                strwhere += str;
            }
            return strwhere;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int AddFeeProblem()
        {
            string sql = "update dbo.freproblem set sortnum=sortnum+1";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}

