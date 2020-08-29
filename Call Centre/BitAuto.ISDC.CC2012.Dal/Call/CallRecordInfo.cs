using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ���ݷ�����CallRecordInfo��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:07 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecordInfo : DataBase
    {
        public static readonly CallRecordInfo Instance = new CallRecordInfo();

        private const string P_CALLRECORDINFO_SELECT = "p_CallRecordInfo_Select";
        private const string P_CALLRECORDINFO_INSERT = "p_CallRecordInfo_Insert";
        private const string P_CALLRECORDINFO_UPDATE = "p_CallRecordInfo_Update";

        protected CallRecordInfo()
        { }

        #region �ɰ�����ɾ���������°�CallRecordInfoInfo
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.CallRecordInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@SessionID", SqlDbType.VarChar,50),
					new SqlParameter("@ExtensionNum", SqlDbType.VarChar,20),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,50),
					new SqlParameter("@ANI", SqlDbType.VarChar,50),
					new SqlParameter("@CallStatus", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@TallTime", SqlDbType.Int,4),
					new SqlParameter("@AudioURL", SqlDbType.VarChar,800),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
                    new SqlParameter("@CustName", SqlDbType.VarChar,50),                    
                    new SqlParameter("@Contact", SqlDbType.VarChar,50),                               
                    new SqlParameter("@TaskTypeID", SqlDbType.Int),                     
                    new SqlParameter("@TaskID", SqlDbType.VarChar,50),
                    new SqlParameter("@SkillGroup",SqlDbType.VarChar,200) ,
                    new SqlParameter("@AgentRingTime",SqlDbType.VarChar,200),
                    new SqlParameter("@CustomRingTime",SqlDbType.VarChar,200),
                    new SqlParameter("@AfterWorkTime",SqlDbType.VarChar,200),
                    new SqlParameter("@AfterWorkBeginTime",SqlDbType.DateTime),
                    new SqlParameter("@NewCustID",SqlDbType.Int),
                    new SqlParameter("@BGID",SqlDbType.Int),
                    new SqlParameter("@SCID",SqlDbType.Int),
                    new SqlParameter("@CallID",SqlDbType.BigInt,8)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SessionID;
            parameters[2].Value = model.ExtensionNum;
            parameters[3].Value = model.PhoneNum;
            parameters[4].Value = model.ANI;
            parameters[5].Value = model.CallStatus;
            parameters[6].Value = model.BeginTime;
            parameters[7].Value = model.EndTime;
            parameters[8].Value = model.TallTime;
            parameters[9].Value = model.AudioURL;
            parameters[10].Value = model.CustID;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.CustName;
            parameters[14].Value = model.Contact;
            parameters[15].Value = model.TaskTypeID;
            parameters[16].Value = model.TaskID;
            parameters[17].Value = model.SkillGroup;
            parameters[18].Value = model.AgentRingTime;
            parameters[19].Value = model.CustomRingTime;
            parameters[20].Value = model.AfterWorkTime;
            parameters[21].Value = model.AfterWorkBeginTime;
            parameters[22].Value = Convert.ToInt32(model.CCCustID);
            parameters[23].Value = model.BGID;
            parameters[24].Value = model.SCID;
            parameters[25].Value = model.CallID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORDINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.CallRecordInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@SessionID", SqlDbType.VarChar,50),
					new SqlParameter("@ExtensionNum", SqlDbType.VarChar,20),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,50),
					new SqlParameter("@ANI", SqlDbType.VarChar,50),
					new SqlParameter("@CallStatus", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@TallTime", SqlDbType.Int,4),
					new SqlParameter("@AudioURL", SqlDbType.VarChar,800),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
                    new SqlParameter("@CustName", SqlDbType.VarChar,50),                    
                    new SqlParameter("@Contact", SqlDbType.VarChar,50),                               
                    new SqlParameter("@TaskTypeID", SqlDbType.Int),                     
                    new SqlParameter("@TaskID", SqlDbType.VarChar,50),
                    new SqlParameter("@SkillGroup",SqlDbType.VarChar,200) ,
                    new SqlParameter("@AgentRingTime",SqlDbType.VarChar,200),
                    new SqlParameter("@CustomRingTime",SqlDbType.VarChar,200),
                    new SqlParameter("@AfterWorkTime",SqlDbType.VarChar,200),
                    new SqlParameter("@AfterWorkBeginTime",SqlDbType.DateTime),
                    new SqlParameter("@NewCustID",SqlDbType.Int,4) ,
                    new SqlParameter("@DMSMemberID",SqlDbType.VarChar,50),
                    new SqlParameter("@NewMemberID",SqlDbType.Int,4),
                    new SqlParameter("@CSTMemberID",SqlDbType.VarChar,10),
                    new SqlParameter("@NewCSTMemberID",SqlDbType.Int,4),
                    new SqlParameter("@RVID",SqlDbType.Int,4),
                    new SqlParameter("@BGID",SqlDbType.Int,4),
                    new SqlParameter("@SCID",SqlDbType.Int,4),
                    new SqlParameter("@CallID",SqlDbType.BigInt,8)
                                        };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SessionID;
            parameters[2].Value = model.ExtensionNum;
            parameters[3].Value = model.PhoneNum;
            parameters[4].Value = model.ANI;
            parameters[5].Value = model.CallStatus;
            parameters[6].Value = model.BeginTime;
            parameters[7].Value = model.EndTime;
            parameters[8].Value = model.TallTime;
            parameters[9].Value = model.AudioURL;
            parameters[10].Value = model.CustID;

            parameters[11].Value = model.CustName;
            parameters[12].Value = model.Contact;
            parameters[13].Value = model.TaskTypeID;
            parameters[14].Value = model.TaskID;

            parameters[15].Value = model.SkillGroup;
            parameters[16].Value = model.AgentRingTime;
            parameters[17].Value = model.CustomRingTime;
            parameters[18].Value = model.AfterWorkTime;
            parameters[19].Value = model.AfterWorkBeginTime;

            parameters[20].Value = model.CCCustID;
            parameters[21].Value = model.DMSMemberID;
            parameters[22].Value = model.CCMemberID;
            parameters[23].Value = model.CSTMemberID;
            parameters[24].Value = model.CC_CSTMemberID;
            parameters[25].Value = model.RVID;
            parameters[26].Value = model.BGID;
            parameters[27].Value = model.SCID;
            parameters[28].Value = model.CallID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORDINFO_UPDATE, parameters);
        }
        #endregion

        #region ���²�ѯ
        /// ��ѯ��ȥ������
        /// <summary>
        /// ��ѯ��ȥ������
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="userid">��ǰ�û�ID</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns></returns>
        public DataTable GetCallRecordInfo(QueryCallRecordInfo query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            string where = string.Empty;
            #region ����Ȩ���ж�
            if (query.LoginID > 0)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("c", "BGID", "CreateUserID", query.LoginID);
                where += whereDataRight;
            }
            #endregion

            #region �������
            //c
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND c.CustID='" + SqlFilter(query.CustID) + "'";
            }
            if (!string.IsNullOrEmpty(query.CRMCustID))
            {
                where += @"AND c.CustID IN 
                                    (SELECT a.CustID FROM dbo.CustBasicInfo a
                                    INNER JOIN dbo.DealerInfo b ON a.CustID=b.CustID
                                    INNER JOIN CRM2009.dbo.DMSMember c ON b.MemberCode=c.MemberCode
                                    WHERE a.Status=0 AND b.Status=0 AND c.Status=0 
                                    AND a.CustCategoryID=3 
                                    AND ISNULL(c.MemberCode,'')<>'' AND c.CustID='" + SqlFilter(query.CRMCustID) + "')";
            }
            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += "  AND c.CallID  = " + query.CallID;
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE && query.CustName != "")
            {
                where += " AND c.CustName LIKE '%" + SqlFilter(query.CustName) + "%'";
            }
            if (query.ANI != Constant.STRING_INVALID_VALUE && query.ANI != "")
            {
                where += " AND c.ANI ='" + SqlFilter(query.ANI) + "'";
            }
            if (query.Agent != Constant.STRING_INVALID_VALUE && query.Agent != "")
            {
                where += " AND c.CreateUserID IN " + " ( SELECT UserID FROM SysRightsManager.dbo.UserInfo WHERE  TrueName LIKE '%" + SqlFilter(query.Agent) + "%')";
            }
            if (!string.IsNullOrEmpty(query.TaskID) && query.TaskID != "-1")
            {
                where += " AND c.TaskID ='" + SqlFilter(query.TaskID) + "'";
            }
            if (query.BeginTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND c.BeginTime >='" + DateTime.Parse(query.BeginTime.ToString()).ToString("yyyy-MM-dd") + "'";
            }
            if (query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND c.BeginTime <'" + DateTime.Parse(query.EndTime.ToString()).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += "  AND c.CreateUserID IN (SELECT userid FROM EmployeeAgent WHERE AgentNum='" + SqlFilter(query.AgentNum) + "')";
            }
            if (query.PhoneNum != Constant.STRING_INVALID_VALUE && query.PhoneNum != "")
            {
                where += " AND c.PhoneNum ='" + SqlFilter(query.PhoneNum) + "'";
            }
            if (query.TaskTypeID.HasValue && query.TaskTypeID > 0)
            {
                where += " AND  c.TaskTypeID=" + query.TaskTypeID.Value;
            }
            if (query.SpanTime1 != Constant.INT_INVALID_VALUE)
            {
                where += " AND  c.TallTime>=" + query.SpanTime1;
            }
            if (query.SpanTime2 != Constant.INT_INVALID_VALUE)
            {
                where += " AND  c.TallTime<=" + query.SpanTime2;
            }
            if (query.CallStatus != Constant.INT_INVALID_VALUE)
            {
                where += " AND c.CallStatus=" + query.CallStatus;
            }
            if (query.SessionID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND c.SessionID='" + SqlFilter(query.SessionID) + "'";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND c.BGID=" + query.BGID + "";
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND c.SCID=" + query.SCID + "";
            }
            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " AND c.CallID=" + query.CallID + "";
            }
            //org
            if (!string.IsNullOrEmpty(query.selBusinessType) && query.selBusinessType != "-1")
            {
                where += " AND org.SwitchINNum='" + SqlFilter(query.selBusinessType) + "'";
            }
            if (!string.IsNullOrEmpty(query.OutTypes))
            {
                where += " AND org.OutBoundType in (" + Dal.Util.SqlFilterByInCondition(query.OutTypes) + ")";
            }
            //cot
            if (query.ProjectId >= 0)
            {
                where += " AND cot.ProjectID =" + query.ProjectId;
            }
            if (query.IsSuccess >= 0)
            {
                where += " AND cot.IsSuccess =" + query.IsSuccess;
                if (query.IsSuccess == 0 && query.FailReason >= 0)
                {
                    where += " AND cot.NotSuccessReason =" + query.FailReason;
                }
            }
            //ivr
            //������ ��һ��
            if (query.SelSolve != Constant.INT_INVALID_VALUE && query.SelSolve >= 0)
            {
                where += "  AND isnull(ivr.Score,0) like '" + query.SelSolve + "%'";
            }
            //����� ��2��
            if (query.IVRScore != Constant.INT_INVALID_VALUE && query.IVRScore >= 0)
            {
                where += "  AND isnull(ivr.Score,0) like '[12]" + query.IVRScore + "'";
            }
            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 200),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Value = tableEndName;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORDINFO_SELECT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// ��������id��ѯ������Ϣ
        /// <summary>
        /// ��������id��ѯ������Ϣ
        /// </summary>
        /// <param name="TaskID"></param>
        /// <param name="tableEndName"></param>
        /// <returns></returns>
        public DataTable GetCallRecordByTaskID(string TaskID, string tableEndName)
        {
            DataTable dt = null;
            string sqlstr = "SELECT * FROM CALLRECORDINFO" + tableEndName + " WHERE TaskID='" + SqlFilter(TaskID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            dt = ds.Tables[0];
            return dt;
        }
        /// ����id��ȡʵ����
        /// <summary>
        /// ����id��ȡʵ����
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByCallID(long CallID, string tableEndName)
        {
            string sqlStr = "SELECT * FROM CallRecordInfo" + tableEndName + " WHERE CallID=@CallID";
            SqlParameter parameter = new SqlParameter("@CallID", CallID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCallRecordInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ��������
        /// �ط��б�-���ܷ���
        /// <summary>
        /// �ط��б�-���ܷ���
        /// </summary>
        /// <param name="queryCC_CallRecords"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCC_CallRecordsByRV(QueryCallRecordInfo queryCC_CallRecords, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";

            if (queryCC_CallRecords.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.CustID = '" + SqlFilter(queryCC_CallRecords.CustID) + "'";
            }
            if (queryCC_CallRecords.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and CustInfo.CustName like '%" + SqlFilter(queryCC_CallRecords.CustName) + "%'";
            }
            if (queryCC_CallRecords.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and ReturnVisit.CustID in (select custid from CRM2009.dbo.CustUserMapping where userid=" + queryCC_CallRecords.CreateUserID + " )   ";
            }

            string order = "ReturnVisit.createtime desc";

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
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CC_CallRecords_Select_by_rv", parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }
        /// ���ܷ���-RVID��ֵ
        /// <summary>
        /// ���ܷ���-RVID��ֵ
        /// </summary>
        /// <param name="RVID"></param>
        /// <returns></returns>
        public DataTable GetCallRecordByRVID(string RVID)
        {
            DataTable dt = null;
            string sqlstr = "select * from callrecordinfo where RviD='" + SqlFilter(RVID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            dt = ds.Tables[0];
            return dt;
        }
        /// ��������ID��ѯʵ����-ֻ�ܲ�ѯ���ڱ�
        /// <summary>
        /// ��������ID��ѯʵ����-ֻ�ܲ�ѯ���ڱ�
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoByTaskID(string taskID)
        {
            string sqlStr = "SELECT * FROM CALLRecordInfo WHERE TaskID='" + Utils.StringHelper.SqlFilter(taskID) + "'";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCallRecordInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// ���ݿͻ�ID��ѯ-ֻ�ܲ�ѯ���ڱ�-ɾ���ͻ�����ʱ��
        /// <summary>
        /// ���ݿͻ�ID��ѯ-ֻ�ܲ�ѯ���ڱ�-ɾ���ͻ�����ʱ��
        /// </summary>
        /// <param name="custID"></param>
        /// <returns></returns>
        public bool HavCarRecordInfoByCustID(string custID)
        {
            string sqlStr = "SELECT count(*) FROM CallRecordInfo WHERE CustID=@CustID";
            SqlParameter parameter = new SqlParameter("@CustID", custID);
            string objValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter).ToString();
            if (int.Parse(objValue) == 0)
                return false;
            else
                return true;
        }
        public int UpdateOriginalDMSMemberIDByID(int cc_DMSMemberID, string memberID)
        {
            string sql = string.Format("Update CallRecordInfo SET NewMemberID=NULL,DMSMemberID='{0}' Where NewMemberID={1}", memberID, cc_DMSMemberID);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        public int UpdateCRMCustIDByID(string cccustid, string custid)
        {
            string sql = string.Format("Update CallRecordInfo SET NewCustID=NULL,CustID='{0}' Where NewCustID={1}", custid, cccustid);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        #endregion

        #region ��������
        /// �õ�һ������ʵ��--ֻ��ѯ���ڱ�
        /// <summary>
        /// �õ�һ������ʵ��--ֻ��ѯ���ڱ�
        /// </summary>
        public Entities.CallRecordInfo GetCallRecordInfo(long RecID)
        {
            string sqlStr = "SELECT * FROM CallRecordInfo WHERE RecID=@RecID";
            SqlParameter parameter = new SqlParameter("@RecID", RecID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCallRecordInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// ����SessionID��ȡʵ����-ֻ��ѯ���ڱ�
        /// <summary>
        /// ����SessionID��ȡʵ����-ֻ��ѯ���ڱ�
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecordInfo GetCallRecordInfoBySessionID(string SessionID)
        {
            string sqlStr = "SELECT * FROM CallRecordInfo WHERE sessionid=@SessionID";
            SqlParameter parameter = new SqlParameter("@SessionID", SessionID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return LoadSingleCallRecordInfo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CallRecordInfo LoadSingleCallRecordInfo(DataRow row)
        {
            Entities.CallRecordInfo model = new Entities.CallRecordInfo();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.SessionID = row["SessionID"].ToString();
            model.ExtensionNum = row["ExtensionNum"].ToString();
            model.PhoneNum = row["PhoneNum"].ToString();
            model.ANI = row["ANI"].ToString();
            if (row["CallStatus"].ToString() != "")
            {
                model.CallStatus = int.Parse(row["CallStatus"].ToString());
            }
            if (row["BeginTime"].ToString() != "")
            {
                model.BeginTime = DateTime.Parse(row["BeginTime"].ToString());
            }
            if (row["EndTime"].ToString() != "")
            {
                model.EndTime = DateTime.Parse(row["EndTime"].ToString());
            }
            if (row["TallTime"].ToString() != "")
            {
                model.TallTime = int.Parse(row["TallTime"].ToString());
            }
            model.AudioURL = row["AudioURL"].ToString();
            model.CustID = row["CustID"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }

            model.CustName = row["CustName"].ToString();
            model.Contact = row["Contact"].ToString();

            if (row["TaskTypeID"].ToString() != "")
            {
                model.TaskTypeID = int.Parse(row["TaskTypeID"].ToString());
            }
            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = row["TaskID"].ToString();
            }
            if (row["SkillGroup"].ToString() != "")
            {
                model.SkillGroup = row["SkillGroup"].ToString();
            }
            if (row["AgentRingTime"].ToString() != "")
            {
                model.AgentRingTime = int.Parse(row["AgentRingTime"].ToString());
            }
            if (row["CustomRingTime"].ToString() != "")
            {
                model.CustomRingTime = int.Parse(row["CustomRingTime"].ToString());
            }
            if (row["AfterWorkTime"].ToString() != "")
            {
                model.AfterWorkTime = int.Parse(row["AfterWorkTime"].ToString());
            }
            if (row["AfterWorkBeginTime"].ToString() != "")
            {
                model.AfterWorkBeginTime = DateTime.Parse(row["AfterWorkBeginTime"].ToString());
            }
            //newcustid,DMSMemberID,NewMemberID,CstMemberID,NewCstMemberID
            if (row["newcustid"].ToString() != "")
            {
                model.CCCustID = row["newcustid"].ToString();
            }
            if (row["DMSMemberID"].ToString() != "")
            {
                model.DMSMemberID = row["DMSMemberID"].ToString();
            }
            if (row["NewMemberID"].ToString() != "")
            {
                model.CCMemberID = row["NewMemberID"].ToString();
            }
            if (row["CstMemberID"].ToString() != "")
            {
                model.CSTMemberID = row["CstMemberID"].ToString();
            }
            if (row["NewCstMemberID"].ToString() != "")
            {
                model.CC_CSTMemberID = row["NewCstMemberID"].ToString();
            }
            if (row["RVID"].ToString() != "")
            {
                model.RVID = int.Parse(row["RVID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            if (row["CallID"].ToString() != "")
            {
                model.CallID = Int64.Parse(row["CallID"].ToString());
            }
            return model;
        }

        /// ��ȡδ�ɹ�ԭ��
        /// <summary>
        /// ��ȡδ�ɹ�ԭ��
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public string GetNotSuccessReason(long ProjectId)
        {
            string strSql = @"SELECT TOP 1 ISNULL(TFValue,'' )
                FROM dbo.TField 
                WHERE TTCode=(SELECT TOP 1 ttcode FROM dbo.ProjectInfo WHERE ProjectID='" + ProjectId + "') AND TFName='NotSuccessReason'";
            return CommonFunction.ObjectToString(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql));
        }
        /// ����CallID��ȡ¼��AudioURL
        /// <summary>
        /// ����CallID��ȡ¼��AudioURL
        /// </summary>
        /// <param name="callID"></param>
        /// <returns></returns>
        public string GetAudioURLByCallID(string callID)
        {
            string strSql = "SELECT top 1 AudioURL FROM  CallRecord_ORIG where callid = '" + SqlFilter(callID) + "'";
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "";
        }
        #endregion

        #region �°�CallRecordInfoInfo
        /// ��ѯʵ������
        /// <summary>
        /// ��ѯʵ������
        /// </summary>
        /// <param name="callid"></param>
        /// <returns></returns>
        public CallRecordInfoInfo GetCallRecordInfoInfo(long callid)
        {
            string sql = "SELECT * FROM dbo.CallRecordInfo WHERE CallID=" + callid;
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return new CallRecordInfoInfo(dt.Rows[0]);
            }
            else return null;
        }
        #endregion
    }
}

