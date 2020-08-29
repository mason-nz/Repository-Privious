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
    /// 数据访问类ProjectDataSoure。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:28 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectDataSoure : DataBase
    {
        #region Instance
        public static readonly ProjectDataSoure Instance = new ProjectDataSoure();
        #endregion

        #region const
        private const string P_PROJECTDATASOURE_SELECT = "p_ProjectDataSoure_Select";
        private const string P_PROJECTDATASOURE_INSERT = "p_ProjectDataSoure_Insert";
        private const string P_PROJECTDATASOURE_UPDATE = "p_ProjectDataSoure_Update";
        private const string P_PROJECTDATASOURE_DELETE = "p_ProjectDataSoure_Delete";
        #endregion

        #region Contructor
        protected ProjectDataSoure()
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
        public DataTable GetProjectDataSoure(QueryProjectDataSoure query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.PDSID != Constant.INT_INVALID_VALUE)
            {
                where += " and PDSID=" + query.PDSID + "";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and ProjectID=" + query.ProjectID + "";
            }
            if (query.Source != Constant.INT_INVALID_VALUE)
            {
                where += " and Source=" + query.Source + "";
            }
            if (query.RelationID != Constant.STRING_INVALID_VALUE)
            {
                where += " and RelationID='" + StringHelper.SqlFilter(query.RelationID) + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and Status=" + query.Status + "";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTDATASOURE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectDataSoure GetProjectDataSoure(long PDSID)
        {
            QueryProjectDataSoure query = new QueryProjectDataSoure();
            query.PDSID = PDSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectDataSoure(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectDataSoure(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectDataSoure LoadSingleProjectDataSoure(DataRow row)
        {
            Entities.ProjectDataSoure model = new Entities.ProjectDataSoure();

            if (row["PDSID"].ToString() != "")
            {
                model.PDSID = long.Parse(row["PDSID"].ToString());
            }
            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            if (row["Source"].ToString() != "")
            {
                model.Source = int.Parse(row["Source"].ToString());
            }
            model.RelationID = row["RelationID"].ToString();
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
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectDataSoure model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PDSID", SqlDbType.Int,8),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.Source;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTDATASOURE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectDataSoure model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PDSID", SqlDbType.Int,8),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.Source;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTDATASOURE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectDataSoure model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PDSID", SqlDbType.Int,8),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.PDSID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.Source;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTDATASOURE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectDataSoure model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PDSID", SqlDbType.Int,8),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.PDSID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.Source;
            parameters[3].Value = model.RelationID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTDATASOURE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long PDSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PDSID", SqlDbType.Int)};
            parameters[0].Value = PDSID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTDATASOURE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long PDSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PDSID", SqlDbType.Int)};
            parameters[0].Value = PDSID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTDATASOURE_DELETE, parameters);
        }
        #endregion


        public void DeleteByProjectID(SqlTransaction sqltran, int ProjectID)
        {
            string sqlStr = "DELETE  dbo.ProjectDataSoure WHERE ProjectID=" + ProjectID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sqlStr);
        }

        public void UpdateStatusByProjectId(SqlTransaction sqltran, string status, int ProjectID)
        {
            string sqlStr = "UPDATE  dbo.ProjectDataSoure SET Status=" + StringHelper.SqlFilter(status) + " WHERE ProjectID=" + ProjectID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sqlStr);
        }

        public string GetProjectDataSoureID(long ProjectID, out string DataCount, bool isReturn)
        {
            string sqlStr = "SELECT COUNT(1) FROM dbo.ProjectDataSoure WHERE ProjectID=" + ProjectID;
            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            DataCount = o.ToString();

            string str = "";
            if (isReturn)
            {
                sqlStr = "Select STUFF((Select ','+ CAST(RelationID AS VARCHAR)   FROM ProjectDataSoure WHERE ProjectID=" + ProjectID + " FOR XML PATH('')),1,1,'')";
                str = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr).ToString();
            }
            return str;
        }

        public void UpdateStatusByProjectId(SqlTransaction tran, string status, DataTable dt, int ProjectID)
        {
            string RelationIDs = "";

            foreach (DataRow dr in dt.Rows)
            {
                RelationIDs += dr["RelationID"].ToString() + ",";
            }
            if (RelationIDs != "")
            {
                RelationIDs = RelationIDs.Substring(0, RelationIDs.Length - 1);

                //  string sqlStr = "UPDATE  dbo.ProjectDataSoure SET Status=@Status  WHERE ProjectID=@ProjectID And RelationID in (@RelationID)";

                SqlParameter[] parameters = {
					new SqlParameter("@Status", SqlDbType.Int),
					new SqlParameter("@ProjectID", SqlDbType.Int),
					new SqlParameter("@RelationID", SqlDbType.VarChar)
					};

                parameters[0].Value = int.Parse(status);
                parameters[1].Value = ProjectID;
                parameters[2].Value = RelationIDs;

                SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, "p_ProjectDataSoure_UpdateStatus", parameters);
                //  SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sqlStr, parameters);
            }

        }

        /// 获取数据库不存在的数据
        /// <summary>
        /// 获取数据库不存在的数据
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetNotExistsDataByProjectID(long projectID, string ids)
        {
            string sql = @"SELECT a FROM [f_split]('" + Dal.Util.SqlFilterByInCondition(ids) + @"',',')
                                    WHERE a NOT IN (SELECT RelationID FROM dbo.ProjectDataSoure WHERE ProjectID=" + projectID + ")";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            List<string> notexists = new List<string>();
            if (dt == null)
            {
                return notexists;
            }
            foreach (DataRow dr in dt.Rows)
            {
                notexists.Add(dr["a"].ToString());
            }
            return notexists;
        }
    }
}

