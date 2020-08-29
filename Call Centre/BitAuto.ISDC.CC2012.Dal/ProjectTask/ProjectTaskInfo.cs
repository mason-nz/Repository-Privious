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
    /// ���ݷ�����ProjectTaskInfo��
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
    public class ProjectTaskInfo : DataBase
    {
        #region Instance
        public static readonly ProjectTaskInfo Instance = new ProjectTaskInfo();
        #endregion

        #region const
        private const string P_PROJECTTASKINFO_SELECT = "p_ProjectTaskInfo_select";
        private const string P_PROJECTTASKINFO_SELECT_BY_TID = "p_ProjectTaskInfo_Select_by_tid";
        private const string P_PROJECTTASKINFO_UPDATE = "p_ProjectTaskInfo_update";
        private const string p_PROJECTTASKINFO_Employee_INSERT = "p_ProjectTaskInfo_Employee_Insert";
        private const string p_PROJECTTASKINFO_SELECT_BY_SourceAndRelationID = "p_ProjectTaskInfo_Select_By_SourceAndRelationID";
        private const string p_PROJECTTASKINFO_BATCHINSERT = "p_ProjectTaskInfo_batchInsert";
        private const string p_PROJECTTASKINFO_CRMBATCHINSERT = "p_ProjectTaskInfo_CrmBatchInsert";
        private const string p_PROJECTTASKINFO_STAT = "p_ProjectTaskInfo_Stat";
        private const string p_CC_UserMappingTask_Stat = "p_CC_UserMappingTask_Stat";
        private const string p_PROJECTTASKINFO_Update_TaskStatusToNoAssign = "p_ProjectTaskInfo_Update_TaskStatusToNoAssign";
        private const string p_PROJECTTASKINFO_StatNum = "p_ProjectTaskInfo_StatNum";
        private const string p_PROJECTTASKINFO_UpdateTaskStatusToNull = "p_ProjectTaskInfo_UpdateTaskStatusToNull";

        private const string P_ProjectTaskInfo_Insert = "P_ProjectTaskInfo_Insert";

        #endregion

        #region Contructor
        protected ProjectTaskInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ  ��ҳ
        /// </summary>
        /// <param name="queryProjectTaskInfo">��ѯֵ����������Ų�ѯ����</param>        
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="totalCount">������</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <returns>�������缯��</returns>
        public DataTable GetProjectTaskInfo(QueryProjectTaskInfo queryProjectTaskInfo, int currentPage, int pageSize, out int totalCount, int userID)
        {
            string where = "";
            if (queryProjectTaskInfo.CRMCustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And dbo.ProjectTaskInfo.CRMCustID = '" + StringHelper.SqlFilter(queryProjectTaskInfo.CRMCustID) + "'";
            }
            if (queryProjectTaskInfo.CustType != Constant.STRING_INVALID_VALUE)
            {
                where += " And ProjectTaskInfo.CustType = '" + StringHelper.SqlFilter(queryProjectTaskInfo.CustType) + "'";
            }
            if (queryProjectTaskInfo.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " And dbo.ProjectTaskInfo.PTID = '" + StringHelper.SqlFilter(queryProjectTaskInfo.PTID) + "'";
            }

            if (queryProjectTaskInfo.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " And pi.ProjectID = " + queryProjectTaskInfo.ProjectID + "";
            }

            if (queryProjectTaskInfo.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " And pi.Name like '%" + StringHelper.SqlFilter(queryProjectTaskInfo.ProjectName) + "%'";
            }
            if (queryProjectTaskInfo.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " And CustName like '%" + StringHelper.SqlFilter(queryProjectTaskInfo.CustName) + "%'";
            }
            if (queryProjectTaskInfo.BGID != Constant.STRING_INVALID_VALUE)
            {
                where += " And pi.BGID = " + StringHelper.SqlFilter(queryProjectTaskInfo.BGID) + "";
            }
            if (queryProjectTaskInfo.PCatageID != Constant.STRING_INVALID_VALUE)
            {
                where += " And pi.PCatageID = " + StringHelper.SqlFilter(queryProjectTaskInfo.PCatageID) + "";
            }

            if (queryProjectTaskInfo.TaskStatus != Constant.INT_INVALID_VALUE)
            {
                where += " And TaskStatus = " + queryProjectTaskInfo.TaskStatus + "";
            }
            if (queryProjectTaskInfo.TaskStatus_s != Constant.STRING_INVALID_VALUE)
            {
                where += " And TaskStatus in (" + Dal.Util.SqlFilterByInCondition(queryProjectTaskInfo.TaskStatus_s) + ")";
            }
            if (queryProjectTaskInfo.OperationStatus_s != Constant.STRING_INVALID_VALUE)
            {
                where += @" And dbo.ProjectTaskInfo.PTID in (SELECT PTID FROM ProjectTaskLog plog WHERE OperationStatus IN (" + Dal.Util.SqlFilterByInCondition(queryProjectTaskInfo.OperationStatus_s);
                where += @") AND CreateTime >=(SELECT MAX(CreateTime) FROM dbo.ProjectTaskLog l WHERE l.PTID=plog.PTID) 
                                    GROUP BY PTID )";
            }

            if (queryProjectTaskInfo.AdditionalStatus != Constant.STRING_INVALID_VALUE)
            {
                string whereStr = "(1=-1";
                foreach (string s in Util.SqlFilterByInCondition(queryProjectTaskInfo.AdditionalStatus).Split(','))
                {
                    whereStr += " or tas.AdditionalStatus='" + s + "' ";
                }
                whereStr += ")";
                where += " And " + whereStr;
            }

            if (queryProjectTaskInfo.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And dbo.ProjectTaskInfo.CreateUserID = " + queryProjectTaskInfo.CreateUserID + "";
            }
            if (queryProjectTaskInfo.LastOptUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And dbo.ProjectTaskInfo.LastOptUserID = " + queryProjectTaskInfo.LastOptUserID + "";
            }
            if (queryProjectTaskInfo.EmployeeID != Constant.INT_INVALID_VALUE)
            {
                where += " And te.UserID = " + queryProjectTaskInfo.EmployeeID + "";
            }
            if (queryProjectTaskInfo.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And LastOptTime>='" + DateTime.Parse(StringHelper.SqlFilter(queryProjectTaskInfo.BeginTime)).ToShortDateString() + " 00:00:00'";
            }
            if (queryProjectTaskInfo.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " And LastOptTime<='" + DateTime.Parse(StringHelper.SqlFilter(queryProjectTaskInfo.EndTime)).ToShortDateString() + " 23:59:59'";
            }

            ///��Ʒ��
            if (queryProjectTaskInfo.NoCRMBrand != Constant.STRING_INVALID_VALUE && queryProjectTaskInfo.NoCRMBrand == "0")
            {
                //ûѡ���Ʒ��,��Ʒ��
                if (queryProjectTaskInfo.CRMBrandIDs != Constant.STRING_INVALID_VALUE)
                {
                    //where += " And dbo.ProjectTaskInfo.PTID in (SELECT PTID FROM  ProjectTask_Cust_Brand b WHERE b.BrandID IN (" + queryProjectTaskInfo.CRMBrandIDs + "))";
                    where += @"AND ( ( dbo.ProjectTaskInfo.Source = 2
                                        AND dbo.ProjectTaskInfo.CRMCustID IN (
                                        SELECT  b.CustID
                                        FROM    [CRM2009].dbo.Cust_Brand b
                                        WHERE   b.BrandID IN (";
                    where += Dal.Util.SqlFilterByInCondition(queryProjectTaskInfo.CRMBrandIDs);
                    where += @" ) ))
                                        OR ( dbo.ProjectTaskInfo.Source = 1
                                            AND dbo.ProjectTaskInfo.PTID IN (
                                            SELECT   PTID
                                            FROM     ProjectTask_Cust_Brand b
                                            WHERE    b.BrandID IN ( ";
                    where += Dal.Util.SqlFilterByInCondition(queryProjectTaskInfo.CRMBrandIDs);
                    where += @") )))";
                }
            }
            else
            {//ѡ���˿�Ʒ��
                where += @" AND dbo.ProjectTaskInfo.Source = 2";
                where += @" AND NOT EXISTS ( SELECT  1 FROM    [CRM2009].dbo.Cust_Brand b WHERE   dbo.ProjectTaskInfo.CRMCustID = b.CustID )";
            }

            if (queryProjectTaskInfo.LastOperStartTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lastTask.LastOperTime >='" + StringHelper.SqlFilter(queryProjectTaskInfo.LastOperStartTime) + " 0:0:0'";
            }
            if (queryProjectTaskInfo.LastOperEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and lastTask.LastOperTime <='" + StringHelper.SqlFilter(queryProjectTaskInfo.LastOperEndTime) + " 23:59:59'";
            }

            #region ����Ȩ��

            //�жϵ�ǰ���Ƿ���ȫ������Ȩ��
            //int RightType = (int)Dal.UserDataRigth.Instance.GetUserDataRigth(userID).RightType;
            ////���û��
            //if (RightType != 2)
            //{
            //    //ȡ��ǰ������Ӧ������Ȩ����
            //    Entities.QueryUserGroupDataRigth QueryUserGroupDataRigth = new Entities.QueryUserGroupDataRigth();
            //    QueryUserGroupDataRigth.UserID = userID;
            //    int totcount = 0;
            //    DataTable dtUserGroupDataRigth = Dal.UserGroupDataRigth.Instance.GetUserGroupDataRigth(QueryUserGroupDataRigth, "", 1, 100000, out totcount);
            //    string Rolename = string.Empty;
            //    if (dtUserGroupDataRigth != null && dtUserGroupDataRigth.Rows.Count > 0)
            //    {
            //        where += "  and (";
            //        for (int i = 0; i < dtUserGroupDataRigth.Rows.Count; i++)
            //        {
            //            //����
            //            if (dtUserGroupDataRigth.Rows[i]["RightType"].ToString() == "1")
            //            {
            //                where += "(pi.BGID='" + dtUserGroupDataRigth.Rows[i]["BGID"].ToString() + "' and te.UserID='" + userID + "') or";
            //            }
            //            //����
            //            else
            //            {
            //                where += "(pi.BGID='" + dtUserGroupDataRigth.Rows[i]["BGID"].ToString() + "') or";
            //            }

            //        }
            //        where = where.Substring(0, where.Length - 3);
            //        where += ")";
            //    }
            //}

            #region ����Ȩ���ж�
            if (userID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pi", "te", "BGID", "UserID", userID);

                where += whereDataRight;
            }
            #endregion

            where += " and pi.IsOldData is null";

            #endregion

            string order = " dbo.ProjectTaskInfo.CreateTime desc ";

            DataSet ds;
            SqlParameter[] parameters = {
                     new SqlParameter("@where", SqlDbType.VarChar,8000),
			new SqlParameter("@order", SqlDbType.NVarChar,100),
			new SqlParameter("@pagesize", SqlDbType.Int,4),
			new SqlParameter("@page", SqlDbType.Int,4),
			new SqlParameter("@totalRecorder", SqlDbType.Int,4)
             };

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASKINFO_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }


        public DataTable GetProjectTaskInfoForTaskRecord(QueryProjectTaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";

            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND PTID='" + StringHelper.SqlFilter(query.PTID.ToString()) + "'";
            }
            if (query.RelationID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND RelationID='" + StringHelper.SqlFilter(query.RelationID.ToString()) + "'";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " AND ProjectID=" + StringHelper.SqlFilter(query.ProjectID.ToString());
            }
            if (query.CRMCustID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND CRMCustID='" + StringHelper.SqlFilter(query.CRMCustID.ToString()) + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTaskInfo_SelectForTaskRecord", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        #endregion

        #region Insert

        /// <summary>
        /// ����Ϊ��Ա��������ȫѡ��
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool BatchInsertProjectTaskInfo_Employee(Entities.QueryExcelCustInfo query, int userId)
        {
            bool result = false;
            string where = string.Empty;
            string order = "";
            where = GenerateWhereStr(query);

            SqlParameter[] parameters = {
				new SqlParameter("@Where", SqlDbType.NVarChar),
				new SqlParameter("@UserID", SqlDbType.Int,4)};

            parameters[0].Value = where;
            parameters[1].Value = userId;
            if (SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_PROJECTTASKINFO_BATCHINSERT, parameters) > 0)
            {
                result = true;
            }

            return result;
        }
        /// <summary>
        /// ����Ϊ��Ա��������ȫѡ��
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CrmBatchInsertProjectTaskInfo_Employee(Entities.QueryCrmCustInfo query, int userId, int batch)
        {
            bool result = false;
            string where = string.Empty;
            string joinWhere = string.Empty;
            string order = "";
            where = GenerateWhereStr(query, out joinWhere);
            SqlParameter[] parameters = {
				new SqlParameter("@Where", SqlDbType.NVarChar),
				new SqlParameter("@UserID", SqlDbType.Int,4),
                 new SqlParameter("@Batch",SqlDbType.Int)
                                        };

            parameters[0].Value = where;
            parameters[1].Value = userId;
            parameters[2].Value = batch;
            if (SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_PROJECTTASKINFO_CRMBATCHINSERT, parameters) > 0)
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// ��������Ա�����񣨵�ѡ��
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool InsertProjectTaskInfo_Employee(Entities.ProjectTaskInfo model, int userId)
        {
            bool result = false;
            string[] CustIds = model.RelationID.Split(',');
            List<SqlParameter[]> paramArry = new List<SqlParameter[]>();
            foreach (string custID in CustIds)
            {
                int i = -1;
                if (int.TryParse(custID, out i))
                {
                    SqlParameter[] parameters = {
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,50),
					new SqlParameter("@UserID", SqlDbType.Int,4)};

                    parameters[0].Value = model.Source;
                    parameters[1].Value = custID;
                    parameters[2].Value = userId;
                    paramArry.Add(parameters);
                }
            }
            using (SqlConnection connection = new SqlConnection(CONNECTIONSTRINGS))
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();
                try
                {
                    foreach (SqlParameter[] param in paramArry)
                    {
                        SqlHelper.ExecuteNonQuery(tran, CommandType.StoredProcedure, p_PROJECTTASKINFO_Employee_INSERT, param);
                    }
                    tran.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    // Attempt to roll back the transaction.
                    try
                    {
                        tran.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                    throw ex;
                }

            }
            return result;
        }
        #endregion

        public int UpdataProjectTaskInfo(Entities.ProjectTaskInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,50),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.Source;
            parameters[2].Value = model.RelationID;
            parameters[3].Value = model.TaskStatus;
            parameters[4].Value = model.Status;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASKINFO_UPDATE, parameters);
        }

        /// <summary>
        /// ��ѯ�Ƿ���ͬ��Դ�͹���ID
        /// </summary>
        /// <param name="source"></param>
        /// <param name="relationID"></param>
        /// <returns></returns>
        public bool SelectTasksBySourceAndRelationID(int source, string relationID, int batch)
        {
            SqlParameter[] parameters = {
                     new SqlParameter("@Source", SqlDbType.Int),
			new SqlParameter("@RelationID", SqlDbType.VarChar,8000)
             };

            parameters[0].Value = source;
            parameters[1].Value = relationID;

            SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_PROJECTTASKINFO_SELECT_BY_SourceAndRelationID, parameters);
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }
        }


        /// <summary>
        /// ����ID��ѯ����������һ����¼
        /// </summary>
        /// <param name="tid">����ID</param>
        /// <returns>����������һ��ֵ����</returns>
        public Entities.ProjectTaskInfo GetProjectTaskInfo(string tid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@ptid", SqlDbType.VarChar)
                    };

            parameters[0].Value = tid;
            //�󶨴洢���̲���

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASKINFO_SELECT_BY_TID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleProjectTaskInfo(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private static Entities.ProjectTaskInfo LoadSingleProjectTaskInfo(DataRow row)
        {
            Entities.ProjectTaskInfo model = new Entities.ProjectTaskInfo();

            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["Source"] != DBNull.Value)
            {
                model.Source = Convert.ToInt32(row["Source"].ToString());
            }

            if (row["RelationID"] != DBNull.Value)
            {
                model.RelationID = row["RelationID"].ToString();
            }

            if (row["CrmCustID"] != DBNull.Value)
            {
                model.CrmCustID = row["CrmCustID"].ToString();
            }

            if (row["TaskStatus"] != DBNull.Value)
            {
                model.TaskStatus = Convert.ToInt32(row["TaskStatus"].ToString());
            }

            if (row["Status"] != DBNull.Value)
            {
                model.Status = Convert.ToInt32(row["Status"].ToString());
            }
            if (row["CustName"] != DBNull.Value)
            {
                model.CustName = row["CustName"].ToString();
            }



            return model;
        }


        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="taskStatus"></param>
        /// <param name="description"></param>
        public void UpdateTaskStatus(string tId, Entities.EnumProjectTaskStatus taskStatus, Entities.EnumProjectTaskOperationStatus operationStaus, string description, int userID, DateTime dtime)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@TaskStatus", SqlDbType.Int),
					new SqlParameter("@Description", SqlDbType.VarChar,200),
					new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@OperationStatus", SqlDbType.Int),
                    new SqlParameter("@LastOptTime", SqlDbType.DateTime)
            };
            parameters[0].Value = tId;
            parameters[1].Value = (int)taskStatus;
            parameters[2].Value = description;
            parameters[3].Value = userID;
            parameters[4].Value = (int)operationStaus;
            parameters[5].Value = dtime;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTaskInfo_UpdateTaskStatus", parameters);
        }

        public void UpdateTaskStatusToNull(string tid)
        {
            string sql = "update ProjectTaskInfo set taskstatus=null where PTID=@PTID and status=0";
            SqlParameter parameter = new SqlParameter("@PTID", SqlDbType.VarChar);
            parameter.Value = tid;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameter);
        }
        /// <summary>
        /// ��д�ͻ�ID
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="taskStatus"></param>
        /// <param name="description"></param>
        public void UpdateCrmCustID(string tId, string crmCustID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CrmCustID", SqlDbType.VarChar,50)
            };
            parameters[0].Value = tId;
            parameters[1].Value = crmCustID;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTaskInfo_UpdateCrmCustID", parameters);
        }


        /// <summary>
        /// ͳ�������ͻ�����
        /// </summary>
        /// <param name="source"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public void StatProjectTaskInfo(int source, Entities.QueryExcelCustInfo query, out int allTaskCount, out int noManageCount, out int managingCount, out int manageFinshedCount)
        {
            allTaskCount = 0;
            noManageCount = 0;
            managingCount = 0;
            manageFinshedCount = 0;
            string where = string.Empty;
            where = GenerateWhereStr(query);

            SqlParameter[] parameters = {
					new SqlParameter("@Source", SqlDbType.Int),
                    new SqlParameter("@where",SqlDbType.VarChar,2000),
                    new SqlParameter("@joinwhere",SqlDbType.VarChar,2000)
            };
            parameters[0].Value = source;
            parameters[1].Value = where;
            parameters[2].Value = string.Empty;
            string[] statArr = new string[4];
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_PROJECTTASKINFO_STAT, parameters))
            {
                if (reader.Read())
                {
                    allTaskCount = int.Parse(reader["allTask"].ToString());
                    if (!string.IsNullOrEmpty(reader["noManage"].ToString()))
                    {
                        noManageCount = int.Parse(reader["noManage"].ToString());
                    }
                    if (!string.IsNullOrEmpty(reader["managing"].ToString()))
                    {
                        managingCount = int.Parse(reader["managing"].ToString());
                    }
                    if (!string.IsNullOrEmpty(reader["manageFinshed"].ToString()))
                    {
                        manageFinshedCount = int.Parse(reader["manageFinshed"].ToString());
                    }
                }
            }
        }

        public void StatProjectTaskInfo(int source, Entities.QueryCrmCustInfo query, out int allTaskCount, out int noManageCount, out int managingCount, out int manageFinshedCount)
        {
            allTaskCount = 0;
            noManageCount = 0;
            managingCount = 0;
            manageFinshedCount = 0;
            string where = string.Empty;
            string joinWhere = string.Empty;
            where = GenerateWhereStr(query, out joinWhere);
            SqlParameter[] parameters = {
					new SqlParameter("@Source", SqlDbType.Int),
                    new SqlParameter("@where",SqlDbType.VarChar,4000),
                    new SqlParameter("@joinwhere",SqlDbType.VarChar,4000)
            };
            parameters[0].Value = source;
            parameters[1].Value = where;
            parameters[2].Value = joinWhere;

            string[] statArr = new string[4];
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_PROJECTTASKINFO_STAT, parameters))
            {
                if (reader.Read())
                {
                    allTaskCount = int.Parse(reader["allTask"].ToString());
                    if (!string.IsNullOrEmpty(reader["noManage"].ToString()))
                    {
                        noManageCount = int.Parse(reader["noManage"].ToString());
                    }
                    if (!string.IsNullOrEmpty(reader["managing"].ToString()))
                    {
                        managingCount = int.Parse(reader["managing"].ToString());
                    }
                    if (!string.IsNullOrEmpty(reader["manageFinshed"].ToString()))
                    {
                        manageFinshedCount = int.Parse(reader["manageFinshed"].ToString());
                    }
                    //allTaskCount = noManageCount + managingCount + manageFinshedCount;
                }
            }
        }

        public string[] StatCC_UserMappingTasks(Entities.QueryCrmCustInfo query)
        {
            string where = string.Empty;
            where = GenerateMappingWhereStr(query);
            SqlParameter[] parameters = {
                    new SqlParameter("@where",SqlDbType.VarChar,2000)
            };
            parameters[0].Value = where;
            string[] statArr = new string[4];
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CONNECTIONSTRINGS, CommandType.StoredProcedure, p_CC_UserMappingTask_Stat, parameters))
            {
                if (reader.Read())
                {
                    statArr[0] = reader["allTask"].ToString();
                    statArr[1] = reader["noManage"].ToString();
                    statArr[2] = reader["managing"].ToString();
                    statArr[3] = reader["manageFinshed"].ToString();
                }
            }
            return statArr;
        }

        public int StatProjectTaskInfoByCustID(string custId, int source)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RelationID", SqlDbType.VarChar, 50),
                    new SqlParameter("@Source",SqlDbType.Int)
            };
            parameters[0].Value = custId;
            parameters[1].Value = source;

            object count = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTaskInfo_StatNum", parameters);
            return int.Parse(count.ToString());
        }

        public bool TaskBelongToUser(string tID, int userID)
        {
            string sql = "select top 1 * from ProjectTask_Employee where ptid = '{0}' and userid = {1} and status=0";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, string.Format(sql, StringHelper.SqlFilter(tID),userID));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// �������񸽼�״̬
        /// </summary>
        public void InsertOrUpdateTaskAdditionalStatus(string tId, string status, string description)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@AdditionalStatus", SqlDbType.VarChar,16),
					new SqlParameter("@Description", SqlDbType.NVarChar,32)
            };
            parameters[0].Value = tId;
            parameters[1].Value = status;
            parameters[2].Value = description;

            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTaskInfo_InsertOrUpdateTaskAdditionalStatus", parameters);
        }

        /// <summary>
        /// �õ����񸽼�״̬
        /// </summary>
        public void GetTaskAdditionalStatus(string tId, out string status, out string description)
        {
            string sql = "select top 1 * from ProjectTask_AdditionalStatus where ptid = '{0}'";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, string.Format(sql, StringHelper.SqlFilter(tId)));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                status = dr["AdditionalStatus"].ToString();
                description = dr["description"].ToString();
            }
            else { status = description = ""; }
        }

        private string GenerateWhereStr(QueryCrmCustInfo query, out string joinWhere)
        {
            string where = string.Empty;
            joinWhere = string.Empty;

            //���Ȼ�Ա��ӪƷ�� add lxw 2013-1-8
            if (query.DMSMemberBrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from  Crm2009.dbo.DMSMember dms  where dms.CustID=ci.CustID AND dms.ID IN ( select MemberID From Crm2009.dbo.DMSMember_Brand Where BrandID IN (" + Dal.Util.SqlFilterByInCondition(query.DMSMemberBrandID) + ") AND dms.SyncStatus = 170002))";
            }

            if (query.BrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from  Crm2009.dbo.Cust_Brand  where BrandID in (" + Dal.Util.SqlFilterByInCondition(query.BrandID) + "))";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE && query.CityID != "null")
            {
                where += " And ci.CityID=" + StringHelper.SqlFilter(query.CityID);
            }
            if (query.contactName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.ContactName like '%" + StringHelper.SqlFilter(query.contactName) + "%'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE && query.CountyID != "null")
            {
                where += " And ci.CountyID=" + StringHelper.SqlFilter(query.CountyID);
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.AbbrName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.AbbrName like '%" + StringHelper.SqlFilter(query.AbbrName) + "%'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE && query.ProvinceID != "null")
            {
                where += " And ci.ProvinceID=" + StringHelper.SqlFilter(query.ProvinceID);
            }
            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ui.TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%'";
            }
            if (query.Brandids != Constant.STRING_INVALID_VALUE)
            {
                string ids = query.Brandids.Trim(',');
                if (ids.Length > 0)
                {
                    where += string.Format(" and ci.custID in (select custid from Crm2009.dbo.cust_brand where brandid in ({0}))", Dal.Util.SqlFilterByInCondition(ids));
                }
            }

            if (query.LastUpdateTime_StartTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime>='" + query.LastUpdateTime_StartTime.ToString("yyyy-MM-dd") + "'";
            }
            if (query.LastUpdateTime_EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime<'" + query.LastUpdateTime_EndTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (query.Lock != Constant.INT_INVALID_VALUE)
            {
                where += " And ci.lock=" + query.Lock;
            }
            if (query.StatusNoManage || query.StatusManaging || query.StatusManageFinsh || query.StatusNoAssign)
            {
                where += " And ( 1=-1";
                if (query.StatusNoManage)
                {
                    if (query.LastUpdateTime_StartTime == Constant.DATE_INVALID_VALUE && query.LastUpdateTime_EndTime == Constant.DATE_INVALID_VALUE)
                    {
                        where += " or tk.TaskStatus=180000";
                    }
                }
                if (query.StatusManaging)
                {
                    string sqlT = " or ((tk.TaskStatus=180001 or tk.TaskStatus=180002 or tk.taskstatus=180009)";
                    StringBuilder sbT = new StringBuilder();
                    if (string.IsNullOrEmpty(query.AdditionalStatus) == false)
                    {
                        sqlT = sqlT + "and ({0}))";
                        foreach (string s in query.AdditionalStatus.Split(','))
                        {
                            string ss = s.Trim();
                            if (string.IsNullOrEmpty(ss) == false)
                            {
                                if (sbT.Length > 0) { sbT.Append(" or "); }
                                //if (ss.ToLower() == "as_a")
                                //{
                                //    sbT.Append(string.Format(" tas.AdditionalStatus='{0}' or tas.AdditionalStatus is null ", ss));
                                //}
                                //else
                                //{
                                sbT.Append(string.Format(" tas.AdditionalStatus='{0}' ", ss));
                                //}
                            }
                        }
                    }
                    else
                    {
                        sqlT = sqlT + "or ({0}))";
                        sbT.Append(" 1=-1 ");
                    }

                    where += string.Format(sqlT, sbT.ToString());
                }
                if (query.StatusManageFinsh)
                {
                    where += " or (tk.TaskStatus between 180003 and 180008) or tk.TaskStatus=180010 or tk.TaskStatus=180011";
                }
                if (query.StatusNoAssign)
                {
                    where += " or (tk.taskStatus is null)";
                }
                where += ")";
            }
            else
            {
                if (query.LastUpdateTime_StartTime != Constant.DATE_INVALID_VALUE || query.LastUpdateTime_EndTime != Constant.DATE_INVALID_VALUE)
                {
                    where += "and tk.taskStatus>180000";
                }
            }
            //����Ա��
            if (query.UserIDAssigned != Constant.INT_INVALID_VALUE)
            {
                if (query.UserIDAssigned == 0)
                {
                    where += " And ccte.UserID IS NOT NULL";
                }
                else
                {
                    where += " And ccte.UserID IS NOT NULL and ccte.UserID=" + query.UserIDAssigned;
                }
            }

            if (query.TaskType != Constant.INT_INVALID_VALUE)
            {
                if (query.TaskType == 0)
                {
                    where += string.Format(" AND ci.CreateSource=0");
                }
                else if (query.TaskType == 1)
                {
                    where += string.Format(" And ci.CreateSource=1 ");
                }
            }

            if (query.CallRecordsCount != Constant.STRING_INVALID_VALUE)
            {
                where += " and (SELECT Count(*) FROM CallRecordInfo WHERE TaskID=tk.PTID)=" + StringHelper.SqlFilter(query.CallRecordsCount);
            }
            //where += " and tk.Status=0 "; �ݲ���Ҫ
            if (!string.IsNullOrEmpty(query.StatusIDs))
            {
                where += " and ci.Status IN (" + Dal.Util.SqlFilterByInCondition(query.StatusIDs) + ")";
            }
            else
            {
                where += " and (ci.Status=0 or ci.status=1)";
            }
            if (query.IsHaveMember)
            {
                where += " and  EXISTS (select CustID  from Crm2009.dbo.DMSMember   Where Crm2009.dbo.DMSMember.CustID = ci.CustID)  ";
            }
            if (query.IsHaveNoMember)
            {
                where += " and NOT  EXISTS (select CustID  from Crm2009.dbo.DMSMember   Where Crm2009.dbo.DMSMember.CustID = ci.CustID)  ";
            }

            if (!string.IsNullOrEmpty(query.CooperatedStatusIDs))
            {
                if (query.CooperatedStatusIDs == "1" || query.CooperatedStatusIDs.Contains("1"))//������
                {
                    where += " and (ci.CustID  IN (select CustID from Crm2009.dbo.DMSMember Where Status=0 And Cooperated=1 ";
                    where += " And MemberCode IN (select distinct membercode from mj2009.dbo.CYTMember Where Status in (1003,1007)";
                    if (!string.IsNullOrEmpty(query.MemberCooperateStatus))
                    {
                        where += " And MemberType IN (" + Dal.Util.SqlFilterByInCondition(query.MemberCooperateStatus) + ") ";
                    }
                    if (!string.IsNullOrEmpty(query.BeginMemberCooperatedTime) &&
                        !string.IsNullOrEmpty(query.EndMemberCooperatedTime))//������ʱ�䷶Χ
                    {
                        where += " And begintime<='" + StringHelper.SqlFilter(query.EndMemberCooperatedTime) + " 23:59:59' And endtime >='" + StringHelper.SqlFilter(query.BeginMemberCooperatedTime) + " 0:0:0' ";
                    }
                    where += " ) ";
                    if (!string.IsNullOrEmpty(query.BeginNoMemberCooperatedTime) &&
                       !string.IsNullOrEmpty(query.EndNoMemberCooperatedTime))//������ʱ�䷶Χ
                    {
                        where += @" And Crm2009.dbo.DMSMember.MemberCode IN (SELECT DISTINCT temp2.memberCode FROM (
	                                                                SELECT temp.memberCode,SUM(temp.NotCooperated) AS SUMNotCooperated,COUNT(*) CountRecord FROM (
			                                                            select membercode,
			                                                            (CASE WHEN begintime>'" + StringHelper.SqlFilter(query.EndNoMemberCooperatedTime) + @" 23:59:59' OR endtime <'" + StringHelper.SqlFilter(query.BeginNoMemberCooperatedTime) + @" 0:0:0'  THEN 1 
				                                                             ELSE 0 END) AS NotCooperated
			                                                            from mj2009.dbo.CYTMember 
			                                                            Where Status in (1003,1007)
		                                                            ) AS temp
		                                                            GROUP BY temp.memberCode
	                                                        ) AS temp2
                                                       WHERE temp2.SUMNotCooperated=temp2.CountRecord ) ";
                    }
                    where += ")) ";
                }
                if (query.CooperatedStatusIDs == "0" || query.CooperatedStatusIDs.Contains("0"))//������
                {
                    where += " and (ci.CustID IN (select CustID from Crm2009.dbo.DMSMember Where Status=0 And (Cooperated=0 OR Cooperated IS NULL) AND SyncStatus!=170008 )) ";
                }
            }
            #region ֮ǰ�������ڡ����������ڡ��߼�
            //if ((!string.IsNullOrEmpty(query.CooperatedStatusIDs) || !string.IsNullOrEmpty(query.CooperateStatusIDs)) &&
            //   !(query.CooperatedStatusIDs == "1,0" && query.CooperateStatusIDs == "1"))
            //{
            //    ArrayList alWhereOR = new ArrayList();
            //    if (!string.IsNullOrEmpty(query.CooperateStatusIDs))
            //    {
            //        alWhereOR.Add("ci.CooperateStatus IN (" + query.CooperateStatusIDs + ")");
            //    }
            //    if (query.CooperatedStatusIDs == "1")
            //    {
            //        alWhereOR.Add("(ci.CustID  IN (select CustID from DMSMember Where Status=0 And Cooperated IN (" + query.CooperatedStatusIDs + ") And CooperateStatus=0))");
            //    }
            //    if (query.CooperatedStatusIDs == "0")
            //    {
            //        alWhereOR.Add("(ci.CustID  IN (select CustID from DMSMember Where Status=0 And (Cooperated IN (" + query.CooperatedStatusIDs + ") OR Cooperated IS NULL)))");
            //    }
            //    string temp = string.Join(" OR ", (string[])alWhereOR.ToArray(typeof(string)));
            //    if (temp != string.Empty)
            //    {
            //        where += " And (" + temp + ")";
            //    }
            //}
            #endregion

            //if (!string.IsNullOrEmpty(query.CooperateStatusIDs))
            //{
            //    where += " and ci.CooperateStatus IN (" + query.CooperateStatusIDs + ")";
            //}
            //if (!string.IsNullOrEmpty(query.CooperatedStatusIDs))
            //{
            //    if (query.CooperatedStatusIDs == "1")
            //    {
            //        where += " and ci.CustID  IN (select CustID from DMSMember Where Cooperated IN (" + query.CooperatedStatusIDs + ") And CooperateStatus=0)";
            //    }
            //    else if (query.CooperatedStatusIDs == "0")
            //    {
            //        where += " and ci.CustID  IN (select CustID from DMSMember Where Cooperated IN (" + query.CooperatedStatusIDs + ") OR Cooperated IS NULL)";
            //    }
            //}
            if (!string.IsNullOrEmpty(query.TypeID))
            {
                where += " and ci.TypeID in (" + Dal.Util.SqlFilterByInCondition(query.TypeID) + ")";
            }
            if (!string.IsNullOrEmpty(query.CreateTimeStart))
            {
                where += " and ci.CreateTime >= '" + StringHelper.SqlFilter(query.CreateTimeStart) + " 0:0:0' ";
            }
            if (!string.IsNullOrEmpty(query.CreateTimeEnd))
            {
                where += " and ci.CreateTime <= '" + StringHelper.SqlFilter(query.CreateTimeEnd) + " 23:59:59' ";
            }
            if (!string.IsNullOrEmpty(query.AreaTypeIDs))
            {
                string[] typeids = query.AreaTypeIDs.Split(',');
                for (int i = 0; i < typeids.Length; i++)
                {
                    string temp = " or ";
                    if (i == 0)
                    {
                        temp = " and (";
                    }
                    switch (typeids[i])
                    {
                        case "1"://163����
                            //where += temp + "ci.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=1) ";
                            where += temp + @"(ci.CountyID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=2 And Value=1) OR (
                                              (ci.CountyID=-1 or ci.CountyID is null or ci.CountyID='') And ci.CityID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=1 And Value=1)))";

                            break;
                        case "2"://163����
                            where += temp + @"ci.CountyID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=2 And Value=2)
                                             And ci.CityID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=1 And Value=1)";
                            break;
                        case "3"://178���˳ǳ���
                            where += temp + "ci.CityID IN (SELECT AreaID FROM Crm2009.dbo.AreaType WHERE Type=1 And Value=2) ";
                            break;
                        //case "4"://178���˳ǽ���
                        //    where += temp + "ci.CountyID IN (SELECT AreaID FROM AreaType WHERE Type=2 And Value=2) ";
                        //    break;
                        default:
                            break;
                    }
                    if (i == typeids.Length - 1)
                    {
                        where += " ) ";
                    }
                }
            }

            if (query.TID != Constant.INT_INVALID_VALUE)
            {
                where += " and tk.PTID='" + query.TID + "'";
            }
            if (!string.IsNullOrEmpty(query.CarType))
            {
                where += " and ci.CarType IN (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ") ";
            }
            //if (query.IsMagazineReturn != Constant.INT_INVALID_VALUE)
            //{
            //    if (query.IsMagazineReturn == 0)
            //    {
            //        joinWhere += "LEFT JOIN (select distinct custID from  CC_MagazineReturn) AS cmr ON ci.CustID=cmr.CustID";
            //        where += " AND cmr.CustID is null";
            //    }
            //    else if (query.IsMagazineReturn == 1)
            //    {
            //        joinWhere += "LEFT JOIN (select distinct custID from  CC_MagazineReturn where Title='" + Utils.StringHelper.SqlFilter(query.ExecCycle) + "') AS cmr ON ci.CustID=cmr.CustID";
            //        where += " AND cmr.custId is not null";
            //    }
            //}
            return where;
        }

        private string GenerateMappingWhereStr(QueryCrmCustInfo query)
        {
            string where = string.Empty;
            if (query.BrandID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in ( select CustID from  Crm2009.dbo.Cust_Brand  where BrandID in (" + Dal.Util.SqlFilterByInCondition(query.BrandID) + "))";
            }
            if (query.CityID != Constant.STRING_INVALID_VALUE && query.CityID != "null")
            {
                where += " And ci.CityID=" + StringHelper.SqlFilter(query.CityID);
            }
            if (query.contactName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.ContactName like '%" + StringHelper.SqlFilter(query.contactName) + "%'";
            }
            if (query.CountyID != Constant.STRING_INVALID_VALUE && query.CountyID != "null")
            {
                where += " And ci.CountyID=" + StringHelper.SqlFilter(query.CountyID);
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.AbbrName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.AbbrName like '%" + StringHelper.SqlFilter(query.AbbrName) + "%'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID='" + StringHelper.SqlFilter(query.CustID) + "'";
            }
            if (query.ProvinceID != Constant.STRING_INVALID_VALUE && query.ProvinceID != "null")
            {
                where += " And ci.ProvinceID=" + StringHelper.SqlFilter(query.ProvinceID);
            }
            if (query.TrueName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ci.CustID in (select cum.CustID from  Crm2009.dbo.CustUserMapping as cum join Crm2009.dbo.v_userinfo as ui on ui.UserID = cum.UserID where ui.TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%') ";
            }
            if (query.Brandids != Constant.STRING_INVALID_VALUE)
            {
                string ids = query.Brandids.Trim(',');
                if (ids.Length > 0)
                {
                    where += string.Format(" and ci.custID in (select custid from Crm2009.dbo.cust_brand where brandid in ({0}))", Dal.Util.SqlFilterByInCondition(ids));
                }
            }
            return where;
        }

        private string GenerateWhereStr(QueryExcelCustInfo query)
        {
            string where = string.Empty;
            if (query.BrandName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.BrandName like '%" + StringHelper.SqlFilter(query.BrandName) + "%'";
            }
            if (query.ProvinceName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.ProvinceName like '%" + StringHelper.SqlFilter(query.ProvinceName) + "%'";
            }
            if (query.CityName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CityName like '%" + StringHelper.SqlFilter(query.CityName) + "%'";
            }
            if (query.TypeNames != Constant.STRING_EMPTY_VALUE)
            {
                where += "And ei.TypeName in (select * from dbo.f_split('" + Dal.Util.SqlFilterByInCondition(query.TypeNames) + "',','))";
            }
            if (query.CountyName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CountyName like '%" + StringHelper.SqlFilter(query.CountyName) + "%'"; ;
            }
            if (query.CustName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.ID != Constant.INT_INVALID_VALUE)
            {
                where += " And ei.ID=" + query.ID;
            }
            if (query.TrueName != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ui.TrueName like '%" + StringHelper.SqlFilter(query.TrueName) + "%'";
            }

            if (query.LastUpdateTime_StartTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime>='" + query.LastUpdateTime_StartTime.ToString("yyyy-MM-dd") + "'";
            }
            if (query.LastUpdateTime_EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " And ct.lastUpdateTime<'" + query.LastUpdateTime_EndTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (query.CarType != Constant.STRING_EMPTY_VALUE)
            {
                where += " And ei.CarType in (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }

            if (query.StatusNoManage || query.StatusManaging || query.StatusManageFinsh || query.StatusNoAssign)
            {
                where += "And ( 1=-1 ";
                if (query.StatusNoManage)
                {
                    where += " or tk.TaskStatus=180000 ";
                }
                if (query.StatusManaging)
                {
                    string sqlT = " or ((tk.TaskStatus=180001 or tk.TaskStatus=180002 or tk.taskStatus=180009) and ({0}))";
                    StringBuilder sbT = new StringBuilder();
                    if (string.IsNullOrEmpty(query.AdditionalStatus) == false)
                    {
                        foreach (string s in query.AdditionalStatus.Split(','))
                        {
                            string ss = s.Trim();
                            if (string.IsNullOrEmpty(ss) == false)
                            {
                                if (sbT.Length > 0) { sbT.Append(" or "); }
                                //if (ss.ToLower() == "as_a")
                                //{
                                //    sbT.Append(string.Format(" tas.AdditionalStatus='{0}' or tas.AdditionalStatus is null ", ss));
                                //}
                                //else
                                //{
                                sbT.Append(string.Format(" tas.AdditionalStatus='{0}' ", ss));
                                //}
                            }
                        }
                    }
                    else { sbT.Append(" 1=1 "); }

                    where += string.Format(sqlT, sbT.ToString());
                }
                if (query.StatusManageFinsh)
                {
                    where += " or (tk.TaskStatus between 180003 and 180008) or tk.TaskStatus=180010 or tk.TaskStatus=180011";
                }
                if (query.StatusNoAssign)
                {
                    where += " or ccte.PTID is null";
                }
                where += ")";
            }

            if (query.UserIDAssigned != Constant.INT_INVALID_VALUE)
            {
                where += " and ccte.UserID IS NOT NULL And ccte.UserID=" + query.UserIDAssigned;
            }

            //where += " and tk.Status=0 "; �ݲ���Ҫ

            return where;
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        public DataTable GetBatchList()
        {
            string sql = "SELECT DISTINCT Batch FROM dbo.ProjectTaskInfo ORDER BY Batch";

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }


        #region  Method

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(Entities.ProjectTaskInfo model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@PDSID", SqlDbType.BigInt,8),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@CustName", SqlDbType.VarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@LastOptTime", SqlDbType.DateTime),
					new SqlParameter("@LastOptUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
                    new SqlParameter("@CustType",SqlDbType.VarChar,50)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.PDSID;
            parameters[2].Value = model.ProjectID;
            parameters[3].Value = model.CustName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Source;
            parameters[6].Value = model.RelationID;
            parameters[7].Value = model.LastOptTime;
            parameters[8].Value = model.LastOptUserID;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.TaskStatus;
            parameters[12].Value = model.CrmCustID;
            parameters[13].Value = model.CustType;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ProjectTaskInfo_Insert, parameters);
        }

        public void Add(SqlTransaction sqltran, Entities.ProjectTaskInfo model)
        {
            int rowsAffected;
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@PDSID", SqlDbType.BigInt,8),
					new SqlParameter("@ProjectID", SqlDbType.BigInt,8),
					new SqlParameter("@CustName", SqlDbType.VarChar,1000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Source", SqlDbType.Int,4),
					new SqlParameter("@RelationID", SqlDbType.VarChar,20),
					new SqlParameter("@LastOptTime", SqlDbType.DateTime),
					new SqlParameter("@LastOptUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,50),
                    new SqlParameter("@CustType",SqlDbType.VarChar,50)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.PDSID;
            parameters[2].Value = model.ProjectID;
            parameters[3].Value = model.CustName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Source;
            parameters[6].Value = model.RelationID;
            parameters[7].Value = model.LastOptTime;
            parameters[8].Value = model.LastOptUserID;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.TaskStatus;
            parameters[12].Value = model.CrmCustID;
            parameters[13].Value = model.CustType;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_ProjectTaskInfo_Insert, parameters);
        }
        /// <summary>
        /// ����ĳ�����ȡ������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public int GetMax()
        {
            int intval = 0;
            string sqlStr = "select max(CAST (SUBSTRING(PTID,4,len(PTID)-3) as int)) from ProjectTaskInfo";

            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            int.TryParse(o.ToString(), out intval);

            return intval;
        }
        #endregion  Method

        public DataTable getCreateUser()
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT CreateUserID FROM dbo.ProjectTaskInfo");
            return ds.Tables[0];

        }

        public DataTable getOpterUser()
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT a.LastOptUserID FROM dbo.ProjectTaskInfo a left join projectinfo b on a.projectid=b.projectid where b.IsOldData is null");
            return ds.Tables[0];

        }

        public DataTable getEmpleeUser()
        {
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT  UserID FROM dbo.ProjectTaskInfo t  LEFT JOIN dbo.ProjectTask_Employee  e ON t.PTID=e.PTID WHERE e.UserID IS NOT NULL");
            return ds.Tables[0];
        }

        public DataTable GetTaskInfoListByIDs(string TaskIDS)
        {
            string sqlStr = "SELECT * FROM dbo.ProjectTaskInfo WHERE PTID IN (" + Dal.Util.SqlFilterByInCondition(TaskIDS) + ")";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="taskIDStr"></param>
        /// <returns></returns>
        public DataTable GetExportTaskList(string taskIDStr)
        {
            string sqlStr = @"SELECT  [�ͻ����] = i.CRMCustID ,
                                [�ͻ�����] = i.CustName ,
                                [��ӪƷ��] = [dbo].[GetBrandListByPTID](i.PTID) ,
                                [��ϵ�绰] = [dbo].[GetTelByPTID](i.PTID) ,
                                [�ͻ�����ʡ��] = ( SELECT AreaName
                                             FROM   [CRM2009].dbo.AreaInfo
                                             WHERE  AreaID = c.ProvinceID
                                           ) ,
                                [�ͻ����ڳ���] = ( SELECT AreaName
                                             FROM   [CRM2009].dbo.AreaInfo
                                             WHERE  AreaID = c.CityID
                                           ) ,
                                [�ͻ���������] = ( SELECT AreaName
                                             FROM   [CRM2009].dbo.AreaInfo
                                             WHERE  AreaID = c.CountyID
                                           ) ,
                                [�ͻ����] = ( CASE c.TypeID
                                             WHEN 20001 THEN '����'
                                             WHEN 20002 THEN '����'
                                             WHEN 20003 THEN '4S'
                                             WHEN 20004 THEN '��������'
                                             WHEN 20005 THEN '�ۺϵ�'
                                             WHEN 20007 THEN '����������'
                                             WHEN 20006 THEN '����'
                                             WHEN 20008 THEN '���̴���'
                                             WHEN 20009 THEN 'չ��'
                                             WHEN 20010 THEN '����'
                                             WHEN 20011 THEN '���͹�˾'
                                             WHEN 20012 THEN '�����г�'
                                           END ) ,
                                [�ͻ���ַ] = c.Address ,
                                [������ԱID] = [dbo].[GetMemberIDsByPTID](i.PTID) ,
                                [������Ա����] = [dbo].[GetMemberNamesByPTID](i.PTID) ,
                                [�ύʱ��] = CONVERT(VARCHAR(100), i.LastOptTime, 20) ,
                                [�ύ��] = ( SELECT    TrueName
                                          FROM      [SysRightsManager].dbo.UserInfo
                                          WHERE     UserID = i.LastOptUserID
                                        ) ,
                                [�ͻ���Դ] = ( CASE i.Source
                                             WHEN 1 THEN 'EXCEL����'
                                             WHEN 2 THEN 'CRM��'
                                           END ) ,
                                [����״̬] = ( CASE i.TaskStatus 
                                             WHEN 180000 THEN 'δ����'
                                             WHEN 180001 THEN '������'
                                             WHEN 180003 THEN '�ύ���'
                                             WHEN 180004 THEN 'ɾ�����'
                                             WHEN 180010 THEN 'ͣ��'
                                             WHEN 180012 THEN 'δ����'
                                             WHEN 180014 THEN '������'
                                             WHEN 180015 THEN '�����'
                                             WHEN 180016 THEN '�ѽ���'
                                            END ) ,
                                [��˽��] = (CASE  (SELECT TOP 1 l.OperationStatus FROM ProjectTaskLog l WHERE l.PTID=i.PTID ORDER BY CreateTime DESC) 
                                                WHEN 6 THEN '���ͨ��'
                                               WHEN 7 THEN '��˾ܾ�'
                                               WHEN 8 THEN '����ͨ��'
                                               WHEN 9 THEN '���˾ܾ�'
                                               ELSE ''
                                               END),
                                [��������] =  (CASE  (SELECT TOP 1 l.OperationStatus FROM ProjectTaskLog l WHERE l.PTID=i.PTID ORDER BY CreateTime DESC) 
                                               WHEN 1 THEN '����'
                                               WHEN 2 THEN '�ջ�'
                                               WHEN 3 THEN '����'
                                               WHEN 4 THEN 'ɾ��'
                                               WHEN 5 THEN '�ύ'
                                               WHEN 6 THEN '���ͨ��'
                                               WHEN 7 THEN '��˾ܾ�'
                                               WHEN 8 THEN '����ͨ��'
                                               WHEN 9 THEN '���˾ܾ�'
                                               WHEN 10 THEN '����'
                                                END) ,
                                [��ע] =(SELECT TOP 1 l.Description FROM ProjectTaskLog l WHERE l.PTID=i.PTID ORDER BY CreateTime DESC) 
                        FROM    dbo.ProjectTaskInfo i
                                LEFT JOIN ProjectTask_Cust c ON i.PTID = c.PTID
                                WHERE i.PTID in (" + Dal.Util.SqlFilterByInCondition(taskIDStr) + ") ";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// type,1�Ǻ�ʵ��2����������
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public DataTable GetProjectInfoByTaskID(string TaskID, string Type)
        {
            string sqlstr = string.Empty;
            DataTable dt = null;
            if (Type == "1")
            {
                sqlstr = "select a.* from dbo.projectinfo a left join projectTaskinfo b on a.projectid=b.projectid where b.ptid='" + StringHelper.SqlFilter(TaskID) + "'";
            }
            else if (Type == "2")
            {
                sqlstr = "select a.* from dbo.projectinfo a left join otherTaskinfo b on a.projectid=b.projectid where b.ptid='" + StringHelper.SqlFilter(TaskID) + "'";
            }
            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr).Tables[0];
            return dt;

        }


        public int GetMaxRecID()
        {
            int intval = 0;
            string sqlStr = "select max(CAST (SUBSTRING(PTID,4,len(PTID)-3) as int)) from  OtherTaskInfo";

            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            int.TryParse(o.ToString(), out intval);

            return intval;
        }
    }
}

