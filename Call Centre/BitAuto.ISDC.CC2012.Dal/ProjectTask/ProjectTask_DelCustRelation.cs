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
    /// 数据访问类ProjectTask_DelCustRelation。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:31 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_DelCustRelation : DataBase
    {
        #region Instance
        public static readonly ProjectTask_DelCustRelation Instance = new ProjectTask_DelCustRelation();
        #endregion

        #region const
        private const string P_PROJECTTASK_DELCUSTRELATION_SELECT = "p_ProjectTask_DelCustRelation_Select";
        private const string P_PROJECTTASK_DELCUSTRELATION_INSERT = "p_ProjectTask_DelCustRelation_Insert";
        private const string P_PROJECTTASK_DELCUSTRELATION_UPDATE = "p_ProjectTask_DelCustRelation_Update";
        private const string P_PROJECTTASK_DELCUSTRELATION_DELETE = "p_ProjectTask_DelCustRelation_Delete";
        private const string P_PROJECTTASK_DELCUSTRELATION_DELETE_BYCUSTID = "p_ProjectTask_DelCustRelation_Delete_ByCustID";
        private const string P_PROJECTTASK_DELCUSTRELATION_DELETE_BYTID = "p_ProjectTask_DelCustRelation_Delete_ByTID";
        #endregion

        #region Contructor
        protected ProjectTask_DelCustRelation()
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
        public DataTable GetProjectTask_DelCustRelation(QueryProjectTask_DelCustRelation query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " And PTID='" + StringHelper.SqlFilter(query.PTID) + "'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }


            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@page", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DELCUSTRELATION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public Entities.ProjectTask_DelCustRelation GetProjectTask_DelCustRelationByTID(string tid)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.PTID = tid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_DelCustRelation(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_DelCustRelation GetProjectTask_DelCustRelation(int RecID)
        {
            QueryProjectTask_DelCustRelation query = new QueryProjectTask_DelCustRelation();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_DelCustRelation(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleProjectTask_DelCustRelation(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ProjectTask_DelCustRelation LoadSingleProjectTask_DelCustRelation(DataRow row)
        {
            Entities.ProjectTask_DelCustRelation model = new Entities.ProjectTask_DelCustRelation();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["PTID"].ToString() != "")
            {
                model.PTID = row["PTID"].ToString();
            }
            model.CustID = row["CustID"].ToString();
            model.DelCustIDs = row["DelCustIDs"].ToString();
            model.Remark = row["Remark"].ToString();
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
        public int Insert(Entities.ProjectTask_DelCustRelation model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
                    new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@DelCustIDs", SqlDbType.VarChar,2000),
					new SqlParameter("@Remark", SqlDbType.VarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.DelCustIDs;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DELCUSTRELATION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_DelCustRelation model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),
                    new SqlParameter("@PTID", SqlDbType.VarChar,20),
                    new SqlParameter("@CustID", SqlDbType.VarChar,50),
                    new SqlParameter("@DelCustIDs", SqlDbType.VarChar,2000),
                    new SqlParameter("@Remark", SqlDbType.VarChar,2000),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.DelCustIDs;
            parameters[4].Value = model.Remark;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DELCUSTRELATION_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DELCUSTRELATION_DELETE, parameters);
        }
        /// <summary>
        /// 根据客户ID，删除多条数据
        /// </summary>
        /// <param name="custID">客户ID</param>
        /// <returns></returns>
        public int DeleteByCustID(string custID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", SqlDbType.VarChar,50)};
            parameters[0].Value = custID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DELCUSTRELATION_DELETE_BYCUSTID, parameters);
        }
        /// <summary>
        /// 根据任务ID，删除多条数据
        /// </summary>
        /// <param name="TID">任务ID</param>
        /// <returns></returns>
        public int DeleteByTID(string tid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar)};
            parameters[0].Value = tid;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_DELCUSTRELATION_DELETE_BYTID, parameters);
        }
        #endregion

    }
}

