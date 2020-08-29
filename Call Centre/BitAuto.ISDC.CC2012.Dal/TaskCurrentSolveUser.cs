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
    /// ���ݷ�����TaskCurrentSolveUser��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TaskCurrentSolveUser : DataBase
    {
        #region Instance
        public static readonly TaskCurrentSolveUser Instance = new TaskCurrentSolveUser();
        #endregion

        #region const
        private const string P_TASKCURRENTSOLVEUSER_SELECT = "p_TaskCurrentSolveUser_Select";
        private const string P_TASKCURRENTSOLVEUSER_INSERT = "p_TaskCurrentSolveUser_Insert";
        private const string P_TASKCURRENTSOLVEUSER_UPDATE = "p_TaskCurrentSolveUser_Update";
        private const string P_TASKCURRENTSOLVEUSER_DELETE = "p_TaskCurrentSolveUser_Delete";
        #endregion

        #region Contructor
        protected TaskCurrentSolveUser()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetTaskCurrentSolveUser(QueryTaskCurrentSolveUser query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID.ToString();
            }
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.CurrentSolveUserEID != Constant.INT_INVALID_VALUE)
            {
                where += " AND CurrentSolveUserEID='" + query.CurrentSolveUserEID.ToString() + "'";
            }
            if (query.CurrentSolveUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND CurrentSolveUserID='" + query.CurrentSolveUserID.ToString() + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " AND Status='" + query.Status.ToString() + "'";
            }
            if (query.CreateUserAdName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND CreateUserAdName='" + StringHelper.SqlFilter(query.CreateUserAdName) + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TASKCURRENTSOLVEUSER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.TaskCurrentSolveUser GetTaskCurrentSolveUser(int RecID)
        {
            QueryTaskCurrentSolveUser query = new QueryTaskCurrentSolveUser();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTaskCurrentSolveUser(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleTaskCurrentSolveUser(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.TaskCurrentSolveUser LoadSingleTaskCurrentSolveUser(DataRow row)
        {
            Entities.TaskCurrentSolveUser model = new Entities.TaskCurrentSolveUser();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.TaskID = row["TaskID"].ToString();
            if (row["CurrentSolveUserEID"].ToString() != "")
            {
                model.CurrentSolveUserEID = int.Parse(row["CurrentSolveUserEID"].ToString());
            }
            if (row["CurrentSolveUserID"].ToString() != "")
            {
                model.CurrentSolveUserID = int.Parse(row["CurrentSolveUserID"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            model.CreateUserAdName = row["CreateUserAdName"].ToString();
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.TaskCurrentSolveUser model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@CurrentSolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@CurrentSolveUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserAdName", SqlDbType.VarChar,50)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.CurrentSolveUserEID;
            parameters[3].Value = model.CurrentSolveUserID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserAdName;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TASKCURRENTSOLVEUSER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.TaskCurrentSolveUser model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20),
					new SqlParameter("@CurrentSolveUserEID", SqlDbType.Int,4),
					new SqlParameter("@CurrentSolveUserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserAdName", SqlDbType.VarChar,50)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TaskID;
            parameters[2].Value = model.CurrentSolveUserEID;
            parameters[3].Value = model.CurrentSolveUserID;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserAdName;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TASKCURRENTSOLVEUSER_UPDATE, parameters);
        }
        /// <summary>
        /// ��������ID�޸�Status״̬Ϊ��Ч
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public int UpdateByTaskID(string taskID)
        {
            int result;
            string strWhere = " UPDATE TaskCurrentSolveUser SET Status=0 WHERE TaskID='" + StringHelper.SqlFilter(taskID) + "'";
            result = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strWhere);
            return result;
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TASKCURRENTSOLVEUSER_DELETE, parameters);
        }
        #endregion

    }
}

