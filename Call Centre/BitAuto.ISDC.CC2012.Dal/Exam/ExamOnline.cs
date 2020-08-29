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
    /// 数据访问类ExamOnline。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-03 02:04:16 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ExamOnline : DataBase
    {
        #region Instance
        public static readonly ExamOnline Instance = new ExamOnline();
        #endregion

        #region const
        private const string P_EXAMONLINE_SELECT = "p_ExamOnline_Select";
        private const string P_EXAMONLINE_INSERT = "p_ExamOnline_Insert";
        private const string P_EXAMONLINE_UPDATE = "p_ExamOnline_Update";
        private const string P_EXAMONLINE_DELETE = "p_ExamOnline_Delete";
        #endregion

        #region Contructor
        protected ExamOnline()
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
        public DataTable GetExamOnline(QueryExamOnline query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.EOLID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EOLID=" + query.EOLID;
            }
            if (query.EIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND EIID=" + query.EIID;
            }
            if (query.MEIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND MEIID=" + query.MEIID;
            }
            if (query.ExamPersonID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ExamPersonID=" + query.ExamPersonID;
            }
            if (query.IsMakeUp != Constant.INT_INVALID_VALUE)
            {
                where += " AND IsMakeUp=" + query.IsMakeUp;
            }
            if (query.IsMarking != Constant.INT_INVALID_VALUE)
            {
                where += " AND IsMarking=" + query.IsMarking;
            }
            if (query.IsLack != Constant.INT_INVALID_VALUE)
            {
                where += " AND IsLack=" + query.IsLack;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }    
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetExamScoreManage(ExamScoreManageQuery query, string order, int currentPage, int pageSize, out int totalCount, string UserID)
        {
            string where = string.Empty;

            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND b.name like '%" + StringHelper.SqlFilter(query.ProjectName.Trim()) + "%'";
            }
            if (query.PaperName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND (h.name like '%" + StringHelper.SqlFilter(query.PaperName.Trim()) + "%' or f.name like '%" + StringHelper.SqlFilter(query.PaperName.Trim()) + "%')";
            }
            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND Truename like '%" + StringHelper.SqlFilter(query.TrueName.Trim()) + "%'";
            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND (b.Examstarttime>= '" + query.BeginTime.Value.Date.ToShortDateString() + " 0:00:00' and b.ExamEndtime<='" + query.EndTime.Value.Date.ToShortDateString() + " 23:59:59')";
            }
            if (query.ExamCategory != Constant.STRING_INVALID_VALUE)
            {
                where += " AND b.Ecid in (" + Dal.Util.SqlFilterByInCondition(query.ExamCategory) + ")";
            }
            if (query.BGIDS != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.bgid in (" + query.BGIDS + ")";
            }

            if (!string.IsNullOrEmpty(UserID))
            {
                where += UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "Exampersonid", int.Parse(UserID));
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_ExamScoreManage_new", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ExamOnline GetExamOnline(long EOLID)
        {
            QueryExamOnline query = new QueryExamOnline();
            query.EOLID = EOLID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetExamOnline(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleExamOnline(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ExamOnline LoadSingleExamOnline(DataRow row)
        {
            Entities.ExamOnline model = new Entities.ExamOnline();

            if (row["EOLID"].ToString() != "")
            {
                model.EOLID = long.Parse(row["EOLID"].ToString());
            }
            if (row["EIID"].ToString() != "")
            {
                model.EIID = int.Parse(row["EIID"].ToString());
            }
            if (row["MEIID"].ToString() != "")
            {
                model.MEIID = int.Parse(row["MEIID"].ToString());
            }
            if (row["ExamPersonID"].ToString() != "")
            {
                model.ExamPersonID = int.Parse(row["ExamPersonID"].ToString());
            }
            if (row["ExamStartTime"].ToString() != "")
            {
                model.ExamStartTime = DateTime.Parse(row["ExamStartTime"].ToString());
            }
            if (row["ExamEndTime"].ToString() != "")
            {
                model.ExamEndTime = DateTime.Parse(row["ExamEndTime"].ToString());
            }
            if (row["SumScore"].ToString() != "")
            {
                model.SumScore = int.Parse(row["SumScore"].ToString());
            }
            if (row["IsMakeUp"].ToString() != "")
            {
                model.IsMakeUp = int.Parse(row["IsMakeUp"].ToString());
            }
            if (row["IsMarking"].ToString() != "")
            {
                model.IsMarking = int.Parse(row["IsMarking"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreaetUserID"].ToString() != "")
            {
                model.CreaetUserID = int.Parse(row["CreaetUserID"].ToString());
            }
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["IsLack"].ToString() != "")
            {
                model.IsLack = int.Parse(row["IsLack"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ExamOnline model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.Int,4),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@SumScore", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@IsMarking", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsLack", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPersonID;
            parameters[4].Value = model.ExamStartTime;
            parameters[5].Value = model.ExamEndTime;
            parameters[6].Value = model.SumScore;
            parameters[7].Value = model.IsMakeUp;
            parameters[8].Value = model.IsMarking;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreaetUserID;
            parameters[11].Value = model.LastModifyTime;
            parameters[12].Value = model.LastModifyUserID;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.IsLack;
            parameters[15].Value = model.BGID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.Int,4),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@SumScore", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@IsMarking", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsLack", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPersonID;
            parameters[4].Value = model.ExamStartTime;
            parameters[5].Value = model.ExamEndTime;
            parameters[6].Value = model.SumScore;
            parameters[7].Value = model.IsMakeUp;
            parameters[8].Value = model.IsMarking;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreaetUserID;
            parameters[11].Value = model.LastModifyTime;
            parameters[12].Value = model.LastModifyUserID;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.IsLack;
            parameters[15].Value = model.BGID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINE_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ExamOnline model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.Int,4),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@SumScore", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@IsMarking", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsLack", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Value = model.EOLID;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPersonID;
            parameters[4].Value = model.ExamStartTime;
            parameters[5].Value = model.ExamEndTime;
            parameters[6].Value = model.SumScore;
            parameters[7].Value = model.IsMakeUp;
            parameters[8].Value = model.IsMarking;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreaetUserID;
            parameters[11].Value = model.LastModifyTime;
            parameters[12].Value = model.LastModifyUserID;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.IsLack;
            parameters[15].Value = model.BGID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLID", SqlDbType.BigInt,8),
					new SqlParameter("@EIID", SqlDbType.Int,4),
					new SqlParameter("@MEIID", SqlDbType.Int,4),
					new SqlParameter("@ExamPersonID", SqlDbType.Int,4),
					new SqlParameter("@ExamStartTime", SqlDbType.DateTime),
					new SqlParameter("@ExamEndTime", SqlDbType.DateTime),
					new SqlParameter("@SumScore", SqlDbType.Int,4),
					new SqlParameter("@IsMakeUp", SqlDbType.Int,4),
					new SqlParameter("@IsMarking", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreaetUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsLack", SqlDbType.Int,4),
                    new SqlParameter("@BGID", SqlDbType.Int,4)};
            parameters[0].Value = model.EOLID;
            parameters[1].Value = model.EIID;
            parameters[2].Value = model.MEIID;
            parameters[3].Value = model.ExamPersonID;
            parameters[4].Value = model.ExamStartTime;
            parameters[5].Value = model.ExamEndTime;
            parameters[6].Value = model.SumScore;
            parameters[7].Value = model.IsMakeUp;
            parameters[8].Value = model.IsMarking;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreaetUserID;
            parameters[11].Value = model.LastModifyTime;
            parameters[12].Value = model.LastModifyUserID;
            parameters[13].Value = model.Status;
            parameters[14].Value = model.IsLack;
            parameters[15].Value = model.BGID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_EXAMONLINE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long EOLID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLID", SqlDbType.BigInt)};
            parameters[0].Value = EOLID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long EOLID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@EOLID", SqlDbType.BigInt)};
            parameters[0].Value = EOLID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_EXAMONLINE_DELETE, parameters);
        }
        #endregion
    }
}

