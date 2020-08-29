using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM2014.Entities;
using BitAuto.DSC.IM2014.Entities.Constants;
using System.Text.RegularExpressions;

namespace BitAuto.DSC.IM2014.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ���ݷ�����AllocationAgent��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-03-05 10:05:58 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AllocationAgent : DataBase
    {
        #region Instance
        public static readonly AllocationAgent Instance = new AllocationAgent();
        #endregion

        #region const
        private const string P_ALLOCATIONAGENT_SELECT = "p_AllocationAgent_Select";
        private const string P_AllocationList = "p_AllocationList";
        private const string P_ALLOCATIONAGENT_INSERT = "p_AllocationAgent_Insert";
        private const string P_ALLOCATIONAGENT_UPDATE = "p_AllocationAgent_Update";
        private const string P_ALLOCATIONAGENT_DELETE = "p_AllocationAgent_Delete";
        #endregion

        #region Contructor
        protected AllocationAgent()
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
        public DataTable GetAllocationAgent(QueryAllocationAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.AllocID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.AllocID=" + query.AllocID;
            }
            if (query.AgentID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.AgentID='" + query.AgentID+"'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AllocationList, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
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
        public DataTable GetAllocationList(QueryAllocationAgent query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_AllocationList, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.AllocationAgent GetAllocationAgent(long AllocID)
        {
            QueryAllocationAgent query = new QueryAllocationAgent();
            query.AllocID = AllocID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAllocationAgent(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleAllocationAgent(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.AllocationAgent LoadSingleAllocationAgent(DataRow row)
        {
            Entities.AllocationAgent model = new Entities.AllocationAgent();

            if (row["AllocID"].ToString() != "")
            {
                model.AllocID = long.Parse(row["AllocID"].ToString());
            }
            if (row["AgentID"].ToString() != "")
            {
                model.AgentID = row["AgentID"].ToString();
            }
            model.UserID = row["UserID"].ToString();
            if (row["QueueStartTime"].ToString() != "")
            {
                model.QueueStartTime = DateTime.Parse(row["QueueStartTime"].ToString());
            }
            if (row["StartTime"].ToString() != "")
            {
                model.StartTime = DateTime.Parse(row["StartTime"].ToString());
            }
            if (row["AgentEndTime"].ToString() != "")
            {
                model.AgentEndTime = DateTime.Parse(row["AgentEndTime"].ToString());
            }
            if (row["UserEndTime"].ToString() != "")
            {
                model.AgentEndTime = DateTime.Parse(row["UserEndTime"].ToString());
            }
            if (row["UserName"].ToString() != "")
            {
                model.UserName = row["UserName"].ToString();
            }
            if (row["Location"].ToString() != "")
            {
                model.Location = row["Location"].ToString();
            }
            if (row["LocalIP"].ToString() != "")
            {
                model.LocalIP = row["LocalIP"].ToString();
            }
            model.UserReferURL = row["UserReferURL"].ToString();
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public long Insert(Entities.AllocationAgent model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
                    new SqlParameter("@QueueStartTime", SqlDbType.DateTime),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@AgentEndTime", SqlDbType.DateTime),
                    new SqlParameter("@UserEndTime", SqlDbType.DateTime),
					new SqlParameter("@UserReferURL", SqlDbType.VarChar,2000),
                                        new SqlParameter("@LocalIP", SqlDbType.VarChar,50),
                                        new SqlParameter("@Location", SqlDbType.VarChar,200),
                                        new SqlParameter("@LocationID", SqlDbType.VarChar,50)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.AgentID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.QueueStartTime;
            parameters[4].Value = model.StartTime;
            parameters[5].Value = model.AgentEndTime;
            parameters[6].Value = model.UserEndTime;
            parameters[7].Value = model.UserReferURL;
            parameters[8].Value = model.LocalIP;
            parameters[9].Value = model.Location;
            parameters[10].Value = model.LocationID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ALLOCATIONAGENT_INSERT, parameters);
            return (long)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.AllocationAgent model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
                    new SqlParameter("@QueueStartTime", SqlDbType.DateTime),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@AgentEndTime", SqlDbType.DateTime),
                    new SqlParameter("@UserEndTime", SqlDbType.DateTime),
					new SqlParameter("@UserReferURL", SqlDbType.VarChar,2000),
                        new SqlParameter("@LocalIP", SqlDbType.VarChar,50),
                                        new SqlParameter("@Location", SqlDbType.VarChar,200),
                                        new SqlParameter("@LocationID", SqlDbType.VarChar,50)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.AgentID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.QueueStartTime;
            parameters[4].Value = model.StartTime;
            parameters[5].Value = model.AgentEndTime;
            parameters[6].Value = model.UserEndTime;
            parameters[7].Value = model.UserReferURL;
            parameters[8].Value = model.LocalIP;
            parameters[9].Value = model.Location;
            parameters[10].Value = model.LocationID;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ALLOCATIONAGENT_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.AllocationAgent model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@UserReferURL", SqlDbType.VarChar,2000)};
            parameters[0].Value = model.AllocID;
            parameters[1].Value = model.AgentID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.StartTime;
            parameters[4].Value = model.AgentEndTime;
            parameters[5].Value = model.UserReferURL;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ALLOCATIONAGENT_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.AllocationAgent model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@AllocID", SqlDbType.BigInt,8),
					new SqlParameter("@AgentID", SqlDbType.VarChar,50),
					new SqlParameter("@UserID", SqlDbType.VarChar,36),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@UserReferURL", SqlDbType.VarChar,2000)};
            parameters[0].Value = model.AllocID;
            parameters[1].Value = model.AgentID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.StartTime;
            parameters[4].Value = model.AgentEndTime;
            parameters[5].Value = model.UserReferURL;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ALLOCATIONAGENT_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long AllocID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@AllocID", SqlDbType.BigInt)};
            parameters[0].Value = AllocID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ALLOCATIONAGENT_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long AllocID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@AllocID", SqlDbType.BigInt)};
            parameters[0].Value = AllocID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ALLOCATIONAGENT_DELETE, parameters);
        }
        #endregion
        /// <summary>
        /// ������ϯ�������ʱ��
        /// </summary>
        /// <param name="userid"></param>
        public void UpdateEndTime(string userid)
        {
            //�ж��Ƿ���guid������Ǿ�������^[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}$
            Regex reg = new Regex("^[A-Za-z0-9]{8}(-[A-Za-z0-9]{4}){3}-[A-Za-z0-9]{12}$");
            bool boolguid = reg.IsMatch(userid);
            string sqlstr = string.Empty;
            //������
            if (boolguid)
            {
                sqlstr = "update [AllocationAgent] set UserEndTime='" + System.DateTime.Now + "' where userid='" + userid + "' and UserEndTime='9999-12-31'";

            }
            else
            {
                sqlstr = "update [AllocationAgent] set AgentEndTime='" + System.DateTime.Now + "' where agentid='" + userid + "' and AgentEndTime='9999-12-31'";
            }
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);

        }
        /// <summary>
        /// ������ϯidȡ��ϯ���������������������
        /// </summary>
        /// <param name="agentid"></param>
        /// <returns></returns>
        public int SelectCurrentAgentUserCount(string agentid)
        {
            string sqlstr = "select count(*) as usercount from AllocationAgent where agentid='" + agentid + "' and StartTime<='" + System.DateTime.Now + "' and (AgentEndTime>='" + System.DateTime.Now + "' and UserEndTime>='" + System.DateTime.Now + "')";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["usercount"].ToString());
            }
            else
            {
                return 0;
            }
        }
    }
}

