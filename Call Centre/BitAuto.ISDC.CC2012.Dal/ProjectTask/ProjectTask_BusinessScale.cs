using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Data.SqlClient;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class ProjectTask_BusinessScale:DataBase
    {
        #region Instance
        public static readonly ProjectTask_BusinessScale Instance = new ProjectTask_BusinessScale();
        #endregion

        #region const
        private const string P_ProjectTask_BusinessScale_SELECT = "p_ProjectTask_BusinessScale_Select";
        private const string P_ProjectTask_BusinessScale_INSERT = "p_ProjectTask_BusinessScale_Insert";
        private const string P_ProjectTask_BusinessScale_UPDATE = "p_ProjectTask_BusinessScale_Update";
        private const string P_ProjectTask_BusinessScale_DELETE = "p_ProjectTask_BusinessScale_Delete";
        #endregion

        #region Contructor
        protected ProjectTask_BusinessScale()
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
        public DataTable GetProjectTask_BusinessScale(QueryProjectTask_BusinessScale query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " and PTID='" + Utils.StringHelper.SqlFilter(query.PTID.ToString())+"'";
            }
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and RecID=" + Utils.StringHelper.SqlFilter(query.RecID.ToString());
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Status="+query.Status;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ProjectTask_BusinessScale_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据任务ID获取二手车规模
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public IList<Entities.ProjectTask_BusinessScale> GetAllProjectTask_BusinessScaleByTID(string tId)
        {
            IList<Entities.ProjectTask_BusinessScale> list = new List<Entities.ProjectTask_BusinessScale>();
            QueryProjectTask_BusinessScale query = new QueryProjectTask_BusinessScale();
            query.PTID = tId;
            int totalCount=0;
            DataTable dt = GetProjectTask_BusinessScale(query, "", 1, 1000, out totalCount);
            foreach (DataRow dr in dt.Rows)
            {
                Entities.ProjectTask_BusinessScale model = LoadSingleProjectTask_BusinessScale(dr);
                list.Add(model);
            }
            return list;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_BusinessScale GetProjectTask_BusinessScale(int RecID)
        {
            QueryProjectTask_BusinessScale query = new QueryProjectTask_BusinessScale();
            query.RecID = RecID;
            query.Status = 0;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_BusinessScale(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_BusinessScale(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectTask_BusinessScale LoadSingleProjectTask_BusinessScale(DataRow row)
        {
            Entities.ProjectTask_BusinessScale model = new Entities.ProjectTask_BusinessScale();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["PTID"].ToString() != "")
            {
                model.PTID = row["PTID"].ToString();
            }
            if (row["OriginalRecID"].ToString() != "")
            {
                model.OriginalRecID = int.Parse(row["OriginalRecID"].ToString());
            }
            if (row["MonthStock"].ToString() != "")
            {
                model.MonthStock = int.Parse(row["MonthStock"].ToString());
            }
            if (row["MonthSales"].ToString() != "")
            {
                model.MonthSales = int.Parse(row["MonthSales"].ToString());
            }
            if (row["MonthTrade"].ToString() != "")
            {
                model.MonthTrade = int.Parse(row["MonthTrade"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_BusinessScale model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@OriginalRecID", SqlDbType.Int,4),
					new SqlParameter("@MonthStock", SqlDbType.Int,4),
					new SqlParameter("@MonthSales", SqlDbType.Int,4),
                    new SqlParameter("@MonthTrade",SqlDbType.Int,4),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.OriginalRecID;
            parameters[3].Value = model.MonthStock;
            parameters[4].Value = model.MonthSales;
            parameters[5].Value = model.MonthTrade;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ProjectTask_BusinessScale_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_BusinessScale model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@OriginalRecID", SqlDbType.Int,4),
					new SqlParameter("@MonthStock", SqlDbType.Int,4),
					new SqlParameter("@MonthSales", SqlDbType.Int,4),
                    new SqlParameter("@MonthTrade", SqlDbType.Int,4),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OriginalRecID;
            parameters[2].Value = model.MonthStock;
            parameters[3].Value = model.MonthSales;
            parameters[4].Value = model.MonthTrade;
            parameters[5].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ProjectTask_BusinessScale_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ProjectTask_BusinessScale_DELETE, parameters);
        }
        #endregion
    }
}
