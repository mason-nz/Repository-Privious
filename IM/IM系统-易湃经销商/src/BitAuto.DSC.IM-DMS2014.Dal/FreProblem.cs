using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Dal
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
    }
}

