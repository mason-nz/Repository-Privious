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
    /// 数据访问类SurveyCategory。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyCategory : DataBase
    {
        #region Instance
        public static readonly SurveyCategory Instance = new SurveyCategory();
        #endregion

        #region const
        private const string P_SURVEYCATEGORY_SELECT = "p_SurveyCategory_Select";
        private const string P_SURVEYCATEGORY_INSERT = "p_SurveyCategory_Insert";
        private const string P_SURVEYCATEGORY_UPDATE = "p_SurveyCategory_Update";
        private const string P_SURVEYCATEGORY_DELETE = "p_SurveyCategory_Delete";
        #endregion

        #region Contructor
        protected SurveyCategory()
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
        public DataTable GetSurveyCategory(QuerySurveyCategory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.SelectType != Constant.INT_INVALID_VALUE)
            {
                //为1，则筛选登陆人管理的所属业务组的信息
                if (query.SelectType == 1 && query.LoginID != Constant.INT_INVALID_VALUE)
                {
                    where += " AND SurveyCategory.BGID IN ( Select BGID From UserGroupDataRigth gdr Where SurveyCategory.BGID=gdr.BGID AND UserID=" + query.LoginID + ")";
                }
            }
            if (query.GroupName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND BusinessGroup.NAME='" + StringHelper.SqlFilter(query.GroupName) + "'";
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.SCID=" + query.SCID;
            }
            if (query.BGID.Value > 0)
            {
                where += " AND SurveyCategory.BGID=" + query.BGID;
            }
            if (query.Level != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Level=" + query.Level;
            }
            if (query.Pid != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Pid=" + query.Pid;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.Status=" + query.Status;
            }
            if (query.TypeId != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyCategory.TypeId=" + query.TypeId;
            }
            if (query.IsFilterStop)
            {
                where += " AND  SurveyCategory.Status<>1";
            }
            //add by qizq 2014-4-17 状态不等于某值，用于过滤等于-3（固定的分类）的
            if (query.NoStatus != Constant.INT_INVALID_VALUE)
            {
                where += " AND  SurveyCategory.Status<>" + query.NoStatus;
            }

            //add by anyy  默认分组和自定义分组
            if (query.GroupStatus != Constant.STRING_EMPTY_VALUE)
            {
                where += " AND  SurveyCategory.Status in " + query.GroupStatus;
            }

            if (query.Exclude == "ReturnVisit")
            {
                //排除客户回访的分类
                where += " AND NOT (SurveyCategory.Name='客户回访' AND SurveyCategory.Status=-3)";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyCategory GetSurveyCategory(int SCID)
        {
            QuerySurveyCategory query = new QuerySurveyCategory();
            query.SCID = SCID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyCategory(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyCategory(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyCategory LoadSingleSurveyCategory(DataRow row)
        {
            Entities.SurveyCategory model = new Entities.SurveyCategory();

            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["Level"].ToString() != "")
            {
                model.Level = int.Parse(row["Level"].ToString());
            }
            if (row["Pid"].ToString() != "")
            {
                model.Pid = int.Parse(row["Pid"].ToString());
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
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.SurveyCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.Pid;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.TypeId;


            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.Pid;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SurveyCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SCID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.Pid;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyCategory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@Level", SqlDbType.Int,4),
					new SqlParameter("@Pid", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SCID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.Level;
            parameters[4].Value = model.Pid;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SCID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SCID", SqlDbType.Int,4)};
            parameters[0].Value = SCID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SCID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SCID", SqlDbType.Int,4)};
            parameters[0].Value = SCID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYCATEGORY_DELETE, parameters);
        }
        #endregion

        public int GetSCIDByName(string categoryName)
        {
            int scid = 0;
            string sqlStr = "select * from SurveyCategory where Status=-3 and Name='" + StringHelper.SqlFilter(categoryName) + "'";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int.TryParse(ds.Tables[0].Rows[0]["SCID"].ToString(), out scid);
            }

            return scid;
        }

        /// <summary>
        ///  分类名称是否重复
        /// </summary>
        public bool IsExistsCategoryName(string categoryName)
        {
            string sql = @"SELECT COUNT(1) FROM SurveyCategory WHERE Name=@Name";
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,100)};
            parameters[0].Value = categoryName;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            int result = 0;
            if (obj == null)
            {
                result = 0;
            }
            else
            {
                result = Convert.ToInt32(obj);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

