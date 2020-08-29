using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类QS_Category。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_Category : DataBase
    {
        #region Instance
        public static readonly QS_Category Instance = new QS_Category();
        #endregion

        #region const
        private const string P_QS_CATEGORY_SELECT = "p_QS_Category_Select";
        private const string P_QS_CATEGORY_INSERT = "p_QS_Category_Insert";
        private const string P_QS_CATEGORY_UPDATE = "p_QS_Category_Update";
        private const string P_QS_CATEGORY_DELETE = "p_QS_Category_Delete";
        #endregion

        #region Contructor
        protected QS_Category()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetQS_Category(QueryQS_Category query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.QS_CID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_CID=" + query.QS_CID;
            }
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RTID=" + query.QS_RTID;
            }
            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " And Name  like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And CreateUserID=" + query.CreateUserID;
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_CATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取分类名称并获取分类下的项目数
        /// </summary>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public DataTable GetQS_CategoryNameAndItemNum(int QS_RTID)
        {
            StringBuilder sqlStr = new StringBuilder();
            sqlStr.Append("select qsi.QS_CID,Name,count(qs_iid) as ItemNum from QS_Category as qsc");
            sqlStr.Append(" join QS_Item as qsi on qsc.QS_CID=qsi.QS_CID ");
            sqlStr.Append(" where qsi.QS_RTID=@QS_RTID ");
            sqlStr.Append(" group by qsi.qs_cid,Name  ");
            sqlStr.Append(" order by qsi.qs_cid ASC");

            SqlParameter[] parameter ={
                new SqlParameter("@QS_RTID",SqlDbType.Int,4)
            };

            parameter[0].Value = QS_RTID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr.ToString(), parameter);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.QS_Category GetQS_Category(int QS_CID)
        {
            QueryQS_Category query = new QueryQS_Category();
            query.QS_CID = QS_CID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_Category(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleQS_Category(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.QS_Category LoadSingleQS_Category(DataRow row)
        {
            Entities.QS_Category model = new Entities.QS_Category();

            if (row["QS_CID"].ToString() != "")
            {
                model.QS_CID = int.Parse(row["QS_CID"].ToString());
            }
            if (row["QS_RTID"].ToString() != "")
            {
                model.QS_RTID = int.Parse(row["QS_RTID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["Score"].ToString() != "")
            {
                model.Score = int.Parse(row["Score"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            if (row["ScoreType"].ToString() != "")
            {
                model.ScoreType = int.Parse(row["ScoreType"].ToString());
            }
            if (row["Sort"].ToString() != "")
            {
                model.Sort = int.Parse(row["Sort"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.QS_Category model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.QS_RTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Score;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.ScoreType;
            parameters[10].Value = model.Sort;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_CATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_Category model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.QS_RTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Score;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.LastModifyTime;
            parameters[8].Value = model.LastModifyUserID;
            parameters[9].Value = model.ScoreType;
            parameters[10].Value = model.Sort;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_CATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.QS_Category model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
				
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
            parameters[0].Value = model.QS_CID;
            parameters[1].Value = model.QS_RTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Score;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.LastModifyTime;
            parameters[6].Value = model.LastModifyUserID;
            parameters[7].Value = model.ScoreType;
            parameters[8].Value = model.Sort;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_CATEGORY_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_Category model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,500),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4)};
            parameters[0].Value = model.QS_CID;
            parameters[1].Value = model.QS_RTID;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Score;
            parameters[4].Value = model.Status;

            parameters[5].Value = model.LastModifyTime;
            parameters[6].Value = model.LastModifyUserID;
            parameters[7].Value = model.ScoreType;
            parameters[8].Value = model.Sort;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_CATEGORY_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int QS_CID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_CID", SqlDbType.Int,4)};
            parameters[0].Value = QS_CID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_CATEGORY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_CID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_CID", SqlDbType.Int,4)};
            parameters[0].Value = QS_CID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_CATEGORY_DELETE, parameters);
        }
        #endregion

        public int GetSumScore(int QS_RTID)
        {
            int score = 0;
            string sqlstr = "select isnull(sum(score),0) sumscore from dbo.QS_Category where qs_RTID=" + QS_RTID;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                if (int.TryParse(dt.Rows[0]["sumscore"].ToString(), out score))
                {
                }
            }

            return score;
        }

    }
}

