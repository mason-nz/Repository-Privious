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
    /// ���ݷ�����SurveyAnswer��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:19 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyAnswer : DataBase
    {
        #region Instance
        public static readonly SurveyAnswer Instance = new SurveyAnswer();
        #endregion

        #region const
        private const string P_SURVEYANSWER_SELECT = "p_SurveyAnswer_Select";
        private const string P_SURVEYANSWER_INSERT = "p_SurveyAnswer_Insert";
        private const string P_SURVEYANSWER_UPDATE = "p_SurveyAnswer_Update";
        private const string P_SURVEYANSWER_DELETE = "p_SurveyAnswer_Delete";
        private const string P_SURVEYANSWER_EXPORTANSWERDETAILINFO = "p_SurveyAnswer_ExportAnswerDetailInfo";
        #endregion

        #region Contructor
        protected SurveyAnswer()
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
        public DataTable GetSurveyAnswer(QuerySurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.AnswerContent != Constant.STRING_INVALID_VALUE)
            {
                where += " AND AnswerContent like '%" + StringHelper.SqlFilter(query.AnswerContent) + "'%";
            }

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND RecID = " + query.RecID;
            }
            if (query.SPIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SPIID = " + query.SPIID;
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SIID = " + query.SIID;
            }
            if (query.SQID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SQID = " + query.SQID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND CreateUserID = " + query.CreateUserID;
            }
            if (query.SMRTID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SMRTID = " + query.SMRTID;
            }
            if (query.SMCTID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SMCTID = " + query.SMCTID;
            }
            if (query.SOID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SOID = " + query.SOID;
            }
            if (query.PTID != Constant.STRING_INVALID_VALUE)
            {
                where += " and PTID='" + StringHelper.SqlFilter(query.PTID) + "'";
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " and ReturnVisitCRMCustID='" + StringHelper.SqlFilter(query.CustID) + "'";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// ���ղ�ѯ������ѯ���ı��𰸵����飩 lxw
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetSurveyAnswerByTextDetail(QuerySurveyAnswer query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.AnswerContent != Constant.STRING_INVALID_VALUE)
            {
                where += " AND sa.AnswerContent like '%" + StringHelper.SqlFilter(query.AnswerContent) + "%'";
            }

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.RecID = " + query.RecID;
            }
            if (query.SPIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.SPIID = " + query.SPIID;
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.SIID = " + query.SIID;
            }
            if (query.SQID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.SQID = " + query.SQID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.CreateUserID = " + query.CreateUserID;
            }
            if (query.SMRTID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.SMRTID = " + query.SMRTID;
            }
            if (query.SMCTID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.SMCTID = " + query.SMCTID;
            }
            if (query.SOID != Constant.INT_INVALID_VALUE)
            {
                where += " AND sa.SOID = " + query.SOID;
            }
            if (query.FilterNull != Constant.INT_INVALID_VALUE)
            {
                if (query.FilterNull == 1)
                {
                    where += " AND sa.AnswerContent!=''";
                }
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_SURVEYANSWER_SELECTByTextDetail", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// ��ȡ�μ���Ŀ����Ա��
        /// </summary>
        /// <param name="SPIID">��ĿID</param>
        /// <returns></returns>
        public int GetAnswerUserCountBySPIID(int SPIID)
        {
            int count = 0;
            string sqlStr = "SELECT COUNT(distinct CreateUserID) FROM SurveyAnswer WHERE SPIID=@SPIID";
            SqlParameter[] parameters ={
                                          new SqlParameter("SPIID",SPIID)
                                     };
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (obj != null)
            {
                count = int.Parse(obj.ToString());
            }

            return count;
        }
        /// <summary>
        /// ��ȡ�μ��������Ա��
        /// </summary>
        /// <param name="SQID">����ID</param>
        /// <returns></returns>
        public int GetAnswerUserCountBySQID(int SQID, int SPIID)
        {
            int count = 0;
            string sqlStr = "SELECT COUNT( distinct CreateUserID) FROM SurveyAnswer WHERE SQID=@SQID And SPIID=@SPIID";
            SqlParameter[] parameters ={
                                          new SqlParameter("SQID", SQID),
                                          new SqlParameter("SPIID",SPIID)
                                     };
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (obj != null)
            {
                count = int.Parse(obj.ToString());
            }

            return count;
        }


        /// <summary>
        /// ����SPIID�ʾ���Ŀ����ID��ȡ�˴ε��������ϸ��Ϣ yyh
        /// </summary>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailBySPIID(int SPIID)
        {
            SqlParameter parameter = new SqlParameter("@SPIID", SPIID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_EXPORTANSWERDETAILINFO, parameter);
            return ds.Tables[0];
        }
        /// <summary>
        /// ����PTID����ID��ȡ�˴ε��������ϸ��Ϣ  lxw
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByPTID(string PTID, int projectID, int siid)
        {
            SqlParameter[] parameters ={
                                          new SqlParameter("@PTID",SqlDbType.VarChar,20),
                                          new SqlParameter("@ProjectID",SqlDbType.Int),
                                          new SqlParameter("@SIID",SqlDbType.Int)
                                     };
            parameters[0].Value = PTID;
            parameters[1].Value = projectID;
            parameters[2].Value = siid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SurveyAnswer_ExportAnswerDetailInfoByPTID", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// ����ReturnVisitCRMCustID�طÿͻ�ID��ȡ�˴ε��������ϸ��Ϣ  lxw
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetAnswerDetailByReturnCustID(string ReturnCustID, int projectID, int siid)
        {
            SqlParameter[] parameters ={
                                          new SqlParameter("@ReturnCustID",SqlDbType.VarChar,50),
                                          new SqlParameter("@ProjectID",SqlDbType.Int),
                                          new SqlParameter("@SIID",SqlDbType.Int)
                                     };
            parameters[0].Value = ReturnCustID;
            parameters[1].Value = projectID;
            parameters[2].Value = siid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SurveyAnswer_ExportAnswerDetailInfoByReturnCustID", parameters);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyAnswer GetSurveyAnswer(int RecID)
        {
            QuerySurveyAnswer query = new QuerySurveyAnswer();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyAnswer(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyAnswer(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyAnswer LoadSingleSurveyAnswer(DataRow row)
        {
            Entities.SurveyAnswer model = new Entities.SurveyAnswer();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["SPIID"].ToString() != "")
            {
                model.SPIID = int.Parse(row["SPIID"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            if (row["SQID"].ToString() != "")
            {
                model.SQID = int.Parse(row["SQID"].ToString());
            }
            if (row["SMRTID"].ToString() != "")
            {
                model.SMRTID = int.Parse(row["SMRTID"].ToString());
            }
            if (row["SMCTID"].ToString() != "")
            {
                model.SMCTID = int.Parse(row["SMCTID"].ToString());
            }
            if (row["SOID"].ToString() != "")
            {
                model.SOID = int.Parse(row["SOID"].ToString());
            }
            model.AnswerContent = row["AnswerContent"].ToString();
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
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SMRTID", SqlDbType.Int,4),
					new SqlParameter("@SMCTID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@AnswerContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                                         new SqlParameter("@PTID", SqlDbType.VarChar,20),
                                        new SqlParameter("@ReturnVisitCRMCustID", SqlDbType.VarChar)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.SIID;
            parameters[3].Value = model.SQID;
            parameters[4].Value = model.SMRTID;
            parameters[5].Value = model.SMCTID;
            parameters[6].Value = model.SOID;
            parameters[7].Value = model.AnswerContent;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.PTID;
            parameters[11].Value = model.ReturnVisitCRMCustID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SMRTID", SqlDbType.Int,4),
					new SqlParameter("@SMCTID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@AnswerContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                                         new SqlParameter("@PTID", SqlDbType.VarChar,20),
                                        new SqlParameter("@ReturnVisitCRMCustID", SqlDbType.VarChar)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.SIID;
            parameters[3].Value = model.SQID;
            parameters[4].Value = model.SMRTID;
            parameters[5].Value = model.SMCTID;
            parameters[6].Value = model.SOID;
            parameters[7].Value = model.AnswerContent;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.PTID;
            parameters[11].Value = model.ReturnVisitCRMCustID;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYANSWER_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SMRTID", SqlDbType.Int,4),
					new SqlParameter("@SMCTID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@AnswerContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.SIID;
            parameters[3].Value = model.SQID;
            parameters[4].Value = model.SMRTID;
            parameters[5].Value = model.SMCTID;
            parameters[6].Value = model.SOID;
            parameters[7].Value = model.AnswerContent;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyAnswer model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@SPIID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SMRTID", SqlDbType.Int,4),
					new SqlParameter("@SMCTID", SqlDbType.Int,4),
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@AnswerContent", SqlDbType.NVarChar,2000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SPIID;
            parameters[2].Value = model.SIID;
            parameters[3].Value = model.SQID;
            parameters[4].Value = model.SMRTID;
            parameters[5].Value = model.SMCTID;
            parameters[6].Value = model.SOID;
            parameters[7].Value = model.AnswerContent;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYANSWER_DELETE, parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SIID, string PTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
                                        new SqlParameter("@PTID", SqlDbType.VarChar,20)};
            parameters[0].Value = SIID;
            parameters[1].Value = PTID;
            string sqlstr = "delete from SurveyAnswer where SIID=@SIID and PTID=@PTID";
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sqlstr, parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int DeleteByCustID(SqlTransaction sqltran, int SIID, string CustID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
                                        new SqlParameter("@CustID", SqlDbType.VarChar,20)};
            parameters[0].Value = SIID;
            parameters[1].Value = CustID;
            string sqlstr = "delete from SurveyAnswer where SIID=@SIID and ReturnVisitCRMCustID=@CustID";
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.Text, sqlstr, parameters);
        }

        #endregion

        /// <summary>
        /// ͨ��Where�õ�AnswerContent
        /// </summary> 
        /// <param name="Where">����</param>
        /// <returns></returns>
        public string getAnswerBySQID(string Where)
        {
            string answer = string.Empty;

            string sqlStr = " select AnswerContent From SurveyAnswer where  1=1 and " + Where;
            answer = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr).ToString();
            return answer;

        }
        /// <summary>
        /// ͨ��ProjectID���ʾ�ID�õ�����ID
        /// </summary> 
        /// <param name="typeID">���ͣ�1-������ϴ��2-��������</param>
        /// <param name="ProjectID">��ĿID</param>
        /// <param name="siid">�ʾ�ID</param>
        /// <returns></returns>
        public DataTable getPTIDByProject(int typeID, int ProjectID, int siid)
        {
            DataSet ds = new DataSet();
            string sqlStr = string.Empty;
            if (typeID == 1)
            {
                sqlStr = @"SELECT DISTINCT
                                    sa.PTID
                            FROM    dbo.ProjectTask_SurveyAnswer sa
                                    LEFT JOIN dbo.ProjectTaskInfo pt ON sa.PTID = pt.PTID
                            WHERE   sa.ProjectID =  " + ProjectID + @"
                                    AND sa.SIID =  " + siid + @"
                                    AND pt.TaskStatus IN (180003,180004,180010,180014,180015,180016)
";
            }
            else
            {
                sqlStr = @"SELECT DISTINCT
                                    sa.PTID,ea.AgentNum,ui.TrueName
                            FROM    dbo.ProjectTask_SurveyAnswer sa
                                    LEFT JOIN dbo.OtherTaskInfo oth ON sa.PTID = oth.PTID
                                    LEFT JOIN dbo.ProjectTask_Employee pe ON pe.PTID=oth.PTID
                                    LEFT JOIN dbo.EmployeeAgent ea ON ea.UserID=pe.UserID
                                    LEFT JOIN SysRightsManager.dbo.UserInfo ui ON ea.UserID=ui.UserID
                            WHERE   sa.ProjectID = " + ProjectID + @"
                                    AND sa.SIID = " + siid + @"
                                    AND ( oth.TaskStatus = 4
                                          OR oth.TaskStatus = 5
                                        ) ";
            }
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];

        }

        /// <summary>
        /// ͨ��ProjectID���ʾ�ID�õ�����ID,add by qizq 2014-11-24,ͨ���������ύʱ�������������id��ֻ��������������������ϴ���������ύ��������������ɣ����ܻ��н����������Ҳ��������ύʱ�䣬��������Ʒ��Ƽû�п���������ϴҵ�����Դ˷�����������ϴ��������
        /// </summary> 
        /// <param name="typeID">���ͣ�1-������ϴ��2-��������</param>
        /// <param name="ProjectID">��ĿID</param>
        /// <param name="siid">�ʾ�ID</param>
        /// <returns></returns>
        public DataTable getPTIDByProject(int typeID, int ProjectID, int siid, string tasksubstart, string tasksubend)
        {
            DataSet ds = new DataSet();
            string sqlStr = string.Empty;
            if (typeID == 1)
            {
                sqlStr = @"SELECT DISTINCT
                                    sa.PTID
                            FROM    dbo.ProjectTask_SurveyAnswer sa
                                    LEFT JOIN dbo.ProjectTaskInfo pt ON sa.PTID = pt.PTID
                            WHERE   sa.ProjectID =  " + ProjectID + @"
                                    AND sa.SIID =  " + siid + @"
                                    AND pt.TaskStatus IN (180003,180004,180010,180014,180015,180016)
";
            }
            else
            {
                sqlStr = @"SELECT DISTINCT
                                    sa.PTID,ea.AgentNum,ui.TrueName
                            FROM    dbo.ProjectTask_SurveyAnswer sa
                                    LEFT JOIN dbo.OtherTaskInfo oth ON sa.PTID = oth.PTID
                                    LEFT JOIN dbo.ProjectTask_Employee pe ON pe.PTID=oth.PTID
                                    LEFT JOIN dbo.EmployeeAgent ea ON ea.UserID=pe.UserID
                                    LEFT JOIN SysRightsManager.dbo.UserInfo ui ON ea.UserID=ui.UserID
                            WHERE   sa.ProjectID = " + ProjectID + @"
                                    AND sa.SIID = " + siid + @"
                                    AND ( oth.TaskStatus = 4
                                          OR oth.TaskStatus = 5
                                        ) ";
                //������ύʱ�䲻Ϊ������������ύʱ������,״̬Ϊ���ύ��
                if (!string.IsNullOrEmpty(tasksubstart.Trim()) && !string.IsNullOrEmpty(tasksubend.Trim()))
                {
                    sqlStr = @"SELECT DISTINCT
                                    sa.PTID,ea.AgentNum,ui.TrueName
                            FROM    dbo.ProjectTask_SurveyAnswer sa
                                    LEFT JOIN dbo.OtherTaskInfo oth ON sa.PTID = oth.PTID
                                    LEFT JOIN dbo.ProjectTask_Employee pe ON pe.PTID=oth.PTID
                                    LEFT JOIN dbo.EmployeeAgent ea ON ea.UserID=pe.UserID
                                    LEFT JOIN SysRightsManager.dbo.UserInfo ui ON ea.UserID=ui.UserID
                            WHERE   sa.ProjectID = " + ProjectID + @"
                                    AND sa.SIID = " + siid + @"
                                    AND oth.TaskStatus = 4" + @"

                                    and oth.lastopttime>='" + StringHelper.SqlFilter(tasksubstart.Trim()) + " 0:0:0' and oth.lastopttime<='" + StringHelper.SqlFilter(tasksubend.Trim()) + " 23:59:59'";
                }

            }
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];

        }

        /// <summary>
        /// ͨ��ProjectID�õ��ͻ�ID
        /// </summary> 
        /// <param name="ProjectID">��ĿID</param> 
        /// <param name="siid">�ʾ�ID</param> 
        /// <returns></returns>
        public DataTable getCustIDByProject(int ProjectID, int siid)
        {
            DataSet ds = new DataSet();
            string sqlStr = "SELECT distinct ReturnVisitCRMCustID FROM dbo.SurveyAnswer WHERE ReturnVisitCRMCustID IN ";
            sqlStr += " (select ReturnVisitCustID From ProjectTask_SurveyAnswer where  ProjectTask_SurveyAnswer.ProjectID=" + ProjectID + " and siid=" + siid + " and status=1 )";
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];

        }

        /// <summary>
        /// ͨ��ProjectID�õ��ͻ�ID,���ļ��ύʱ����� by qizq 2014-11-25
        /// </summary> 
        /// <param name="ProjectID">��ĿID</param> 
        /// <param name="siid">�ʾ�ID</param> 
        /// <returns></returns>
        public DataTable getCustIDByProject(int ProjectID, int siid, string substart, string subend)
        {
            DataSet ds = new DataSet();
            string sqlStr = "SELECT distinct ReturnVisitCRMCustID FROM dbo.SurveyAnswer WHERE ReturnVisitCRMCustID IN ";
            sqlStr += " (select ReturnVisitCustID From ProjectTask_SurveyAnswer where  ProjectTask_SurveyAnswer.ProjectID=" + ProjectID + " and siid=" + siid + " and status=1";
            if (!string.IsNullOrEmpty(substart.Trim()) && !string.IsNullOrEmpty(subend.Trim()))
            {
                sqlStr+=" and createtime>='" + StringHelper.SqlFilter(substart.Trim()) + " 0:0:0' and createtime<='" + StringHelper.SqlFilter(subend.Trim()) + " 23:59:59'";
            }
            sqlStr += ")";
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];

        }

    }
}

