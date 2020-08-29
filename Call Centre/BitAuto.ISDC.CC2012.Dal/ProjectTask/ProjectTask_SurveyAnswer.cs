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
    /// ���ݷ�����ProjectTask_SurveyAnswer��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_SurveyAnswer : DataBase
    {
        #region Instance
        public static readonly ProjectTask_SurveyAnswer Instance = new ProjectTask_SurveyAnswer();
        #endregion

        #region const
        private const string P_PROJECTTASK_SURVEYANSWER_SELECT = "p_ProjectTask_SurveyAnswer_Select";
        private const string P_PROJECTTASK_SURVEYANSWER_INSERT = "p_ProjectTask_SurveyAnswer_Insert";
        private const string P_PROJECTTASK_SURVEYANSWER_UPDATE = "p_ProjectTask_SurveyAnswer_Update";
        private const string P_PROJECTTASK_SURVEYANSWER_DELETE = "p_ProjectTask_SurveyAnswer_Delete";
        #endregion

        #region Contructor
        protected ProjectTask_SurveyAnswer()
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
        public DataTable GetProjectTask_SurveyAnswer(QueryProjectTask_SurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhere(query);
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@page", SqlDbType.Int, 4),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private static string GetWhere(QueryProjectTask_SurveyAnswer query)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID=" + query.RecID;
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectID=" + query.ProjectID;
            }
            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND PTID='" + StringHelper.SqlFilter(query.PTID) + "'";
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SIID=" + query.SIID;
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND ReturnVisitCustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            return where;
        }

        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetProjectTask_SurveyAnswer(SqlTransaction trans, QueryProjectTask_SurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhere(query);
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

            ds = SqlHelper.ExecuteDataset(trans, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public int GetProjectTask_SurveyAnswer_Count(QueryProjectTask_SurveyAnswer query)
        {
            ////��ʱ���Դ洢���̵��߼���ѯ����
            //int RowCount = 0;
            DataTable dt = GetProjectTask_SurveyAnswerByQuery(query);
            if (dt!=null)
            {
                return dt.Rows.Count;
            }
            return 0;
        }

        /// <summary>
        /// ������ĿID���ʾ�ID��ȡ����Ŀ�Ĵ�����Ϣ
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByProjectID(int projectID, int siid)
        {
            string sqlStr = " SELECT * FROM dbo.ProjectTask_SurveyAnswer WHERE ProjectID=" + projectID + " and SIID=" + siid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];
        }

        /// <summary>
        /// ������������ѯ����
        /// </summary>
        /// <param name="tran">����</param>
        /// <param name="query">��������</param>
        /// <returns>��������</returns>
        public DataTable GetProjectTask_SurveyAnswerByQuery(SqlTransaction tran, QueryProjectTask_SurveyAnswer query)
        {
            string where = GetWhere(query);
            string sql = string.Format("SELECT * FROM ProjectTask_SurveyAnswer WHERE 1=1 {0}", where);
            DataSet ds = SqlHelper.ExecuteDataset(tran, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// ������������ѯ����
        /// </summary>
        /// <param name="query">��������</param>
        /// <returns>��������</returns>
        public DataTable GetProjectTask_SurveyAnswerByQuery(QueryProjectTask_SurveyAnswer query)
        {
            string where = GetWhere(query);
            string sql = string.Format("SELECT * FROM ProjectTask_SurveyAnswer WHERE 1=1 {0}", where);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.ProjectTask_SurveyAnswer GetProjectTask_SurveyAnswer(long RecID)
        {
            QueryProjectTask_SurveyAnswer query = new QueryProjectTask_SurveyAnswer();
            query.RecID = RecID;
            DataTable dt = GetProjectTask_SurveyAnswerByQuery(query);
            if (dt!=null)
            {
                return LoadSingleProjectTask_SurveyAnswer(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public Entities.ProjectTask_SurveyAnswer LoadSingleProjectTask_SurveyAnswer(DataRow row)
        {
            Entities.ProjectTask_SurveyAnswer model = new Entities.ProjectTask_SurveyAnswer();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.PTID = row["PTID"].ToString();
            if (row["ProjectID"].ToString() != "")
            {
                model.ProjectID = long.Parse(row["ProjectID"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ReturnVisitCustID"].ToString() != "")
            {
                model.ReturnVisitCustID = row["ReturnVisitCustID"].ToString();
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
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.ProjectTask_SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                                        new SqlParameter("@ReturnVisitCustID", SqlDbType.VarChar),
                                        new SqlParameter("@Status", SqlDbType.Int),};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.ProjectID;
            parameters[3].Value = model.SIID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ReturnVisitCustID;
            parameters[7].Value = model.Status;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.ProjectTask_SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@ProjectID", SqlDbType.Int,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                                        new SqlParameter("@ReturnVisitCustID", SqlDbType.VarChar),
                                        new SqlParameter("@Status", SqlDbType.Int)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.ProjectID;
            parameters[3].Value = model.SIID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ReturnVisitCustID;
            parameters[7].Value = model.Status;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.ProjectTask_SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@ReturnVisitCustID", SqlDbType.VarChar),
                                        new SqlParameter("@Status", SqlDbType.Int)};

            parameters[0].Value = model.PTID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.SIID;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = model.ReturnVisitCustID;
            parameters[6].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.ProjectTask_SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@ReturnVisitCustID", SqlDbType.VarChar),
                                        new SqlParameter("@Status", SqlDbType.Int)};

            parameters[0].Value = model.PTID;
            parameters[1].Value = model.ProjectID;
            parameters[2].Value = model.SIID;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = model.ReturnVisitCustID;
            parameters[6].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_UPDATE, parameters);
        }

        public int UpdateCreateTimeAndStatus(SqlTransaction tran, Entities.ProjectTask_SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
                    new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.CreateTime;
            parameters[1].Value = model.RecID;
            parameters[2].Value = model.Status;
            string sql = "UPDATE dbo.ProjectTask_SurveyAnswer SET CreateTime=@CreateTime,Status=@Status WHERE RecID=@RecID";
            return SqlHelper.ExecuteNonQuery(tran, CommandType.Text, sql, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_PROJECTTASK_SURVEYANSWER_DELETE, parameters);
        }
        #endregion
    }
}

