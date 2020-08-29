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
    /// ���ݷ�����CallRecord_ORIG_Business��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-05-27 10:46:31 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG_Business : DataBase
    {
        public static readonly CallRecord_ORIG_Business Instance = new CallRecord_ORIG_Business();

        private const string P_CALLRECORD_ORIG_BUSINESS_SELECT = "p_CallRecord_ORIG_Business_Select";
        private const string P_CALLRECORD_ORIG_BUSINESS_INSERT = "p_CallRecord_ORIG_Business_Insert";
        private const string P_CALLRECORD_ORIG_BUSINESS_UPDATE = "p_CallRecord_ORIG_Business_Update";

        protected CallRecord_ORIG_Business() { }

        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG_Business model)
        {
            //�����������CallRecord_ORIG_Task��
            TriggerCallRecord_ORIG_Task(model.CallID, model.BusinessID);

            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.BusinessID;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_INSERT, parameters);
            int num = int.Parse(parameters[0].Value.ToString());
            return num;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Business model)
        {
            //�����������CallRecord_ORIG_Task��
            TriggerCallRecord_ORIG_Task(model.CallID, model.BusinessID);

            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@BusinessID", SqlDbType.NVarChar,200),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CallID;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.BusinessID;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_UPDATE, parameters);
            int num = (int)parameters[0].Value;
            return num;
        }
        /// ��������CallRecord_ORIG_Task�����ݿ���
        /// <summary>
        /// ��������CallRecord_ORIG_Task�����ݿ���
        /// </summary>
        /// <param name="model"></param>
        public void TriggerCallRecord_ORIG_Task(long callid, string businessid)
        {
            //ͬ�������������
            if (!string.IsNullOrEmpty(businessid) && callid > 0)
            {
                int source = (int)GetProjectSource(businessid);
                if (source >= 4) //��������Ŀ����
                {
                    SyncCallRecordORIGTask(businessid, callid, source);
                }
            }
        }
        /// ��������id���ص㣬�ж���������
        /// <summary>
        /// ��������id���ص㣬�ж���������
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public ProjectSource GetProjectSource(string businessID)
        {
            if (businessID.ToUpper().StartsWith("CKU"))
            {
                return ProjectSource.S2_�ͻ���ʵ;
            }
            else if (businessID.ToUpper().StartsWith("WO"))
            {
                return ProjectSource.S3_����;
            }
            else if (businessID.ToUpper().StartsWith("OTH"))
            {
                return ProjectSource.S4_��������;
            }
            else if (businessID.ToUpper().StartsWith("YJK"))
            {
                return ProjectSource.S5_�׼���;
            }
            else if (businessID.ToUpper().StartsWith("CJK"))
            {
                return ProjectSource.S6_���Ҽ���;
            }
            else if (businessID.ToUpper().StartsWith("YTG"))
            {
                return ProjectSource.S7_���Ź�;
            }
            else return ProjectSource.None;
        }
        /// �Ƿ�����
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        private bool IsNumber(string businessID, int maxnum)
        {
            int a = -1;
            if (int.TryParse(businessID, out a))
            {
                if (a <= maxnum)
                {
                    return true;
                }
            }
            return false;
        }

        /// ��ȡҵ��url
        /// <summary>
        /// ��ȡҵ��url
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllURL()
        {
            string sqlStr = @"SELECT *,
            RTRIM(BGID)+'_'+RTRIM(SCID)+'_'+RTRIM(ISNULL(Source,-1))+'_'+RTRIM(ISNULL(CarType,-1)) AS BGID_SCID_Source_CarType 
            FROM dbo.CallRecord_ORIG_BusinessURL";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];
        }
        /// ת��ʵ����
        /// <summary>
        /// ת��ʵ����
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Entities.CallRecord_ORIG_Business LoadSingleCallRecord_ORIG_Business(DataRow row)
        {
            Entities.CallRecord_ORIG_Business model = new Entities.CallRecord_ORIG_Business();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            if (row["CallID"].ToString() != "")
            {
                model.CallID = Int64.Parse(row["CallID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.BusinessID = row["BusinessID"].ToString();
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            return model;
        }

        /// ����һ��URL
        /// <summary>
        /// ����һ��URL
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <param name="webBaseUrl"></param>
        /// <returns></returns>
        public int AddBusinessUrl(int BGID, int SCID, string webBaseUrl)
        {
            string sqlStr = @"INSERT dbo.CallRecord_ORIG_BusinessURL
                                            ( BGID ,
                                              SCID ,
                                              Source ,
                                              CarType ,
                                              BusinessDetailURL ,
                                              CreateTime
                                            )
                                    VALUES ( @BGID , -- BGID - int
                                              @SCID,
                                              NULL ,
                                              NULL , 
                                             @webBaseUrl,
                                              GETDATE()
                                            ) ";
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
                       new SqlParameter("@webBaseUrl", SqlDbType.VarChar,2000)       
                                        };
            parameters[0].Value = BGID;
            parameters[1].Value = SCID;
            parameters[2].Value = webBaseUrl;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }
        /// ɾ��ҵ��url
        /// <summary>
        /// ɾ��ҵ��url
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public int DeleteBusinessUrl(int BGID, int SCID)
        {
            string sqlStr = "delete CallRecord_ORIG_BusinessURL where BGID=@BGID and SCID=@SCID";
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4)};
            parameters[0].Value = BGID;
            parameters[1].Value = SCID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }
        /// ����ҵ���飬����ȡurl
        /// <summary>
        /// ����ҵ���飬����ȡurl
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="SCID"></param>
        /// <returns></returns>
        public DataTable GetBusinessUrl(int BGID, int SCID)
        {
            string sqlStr = "select * from CallRecord_ORIG_BusinessURL where BGID=@BGID and SCID=@SCID";
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4)};
            parameters[0].Value = BGID;
            parameters[1].Value = SCID;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters).Tables[0];
        }
        /// �Ƿ���ڸü�¼-���ڱ�
        /// <summary>
        /// �Ƿ���ڸü�¼-���ڱ�
        /// </summary>
        public bool IsExistsByCallID(Int64 callid)
        {
            string sql = string.Format("SELECT * FROM CallRecord_ORIG_Business WHERE callid={0}", callid);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        #region ���²�ѯ
        /// ��ѯ����ҵ������
        /// <summary>
        /// ��ѯ����ҵ������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIG_Business(QueryCallRecord_ORIG_Business query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            string where = string.Empty;

            #region MyRegion

            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " And  CallID=" + query.CallID;
            }

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " And  RecID=" + query.RecID;
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " And  BGID=" + query.BGID;
            }
            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " And  SCID=" + query.SCID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And  CreateUserID=" + query.CreateUserID;
            }
            if (query.BusinessID != Constant.STRING_INVALID_VALUE)
            {
                where += " And  BusinessID='" + StringHelper.SqlFilter(query.BusinessID) + "'";
            }

            #endregion
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@tableend", SqlDbType.NVarChar,20),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = tableEndName;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_BUSINESS_SELECT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// ��ѯ����ҵ��ʵ����
        /// <summary>
        /// ��ѯ����ҵ��ʵ����
        /// </summary>
        /// <param name="CallID"></param>
        /// <returns></returns>
        public Entities.CallRecord_ORIG_Business GetByCallID(Int64 CallID, string tableEndName)
        {
            QueryCallRecord_ORIG_Business query = new QueryCallRecord_ORIG_Business();
            query.CallID = CallID;
            int totalCount = 0;
            DataTable dt = GetCallRecord_ORIG_Business(query, "", 1, 9999, tableEndName, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                return LoadSingleCallRecord_ORIG_Business(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// ͬ������CallRecord_ORIG_Task
        /// <summary>
        /// ͬ������CallRecord_ORIG_Task
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="callid"></param>
        /// <param name="source"></param>
        /// <param name="tableEndName"></param>
        public void SyncCallRecordORIGTask(string businessID, long callid, int source)
        {
            //������ͨʱ��������ʼ������
            //�Ҷ�ʱ���»�����Ϣ ��CallRecord_ORIG Insert������
            SqlParameter[] parameters = {
                    new SqlParameter("@BusinessID", SqlDbType.VarChar,50){Value=businessID},
                    new SqlParameter("@CallID", SqlDbType.BigInt){Value=callid},					
                    new SqlParameter("@Source", SqlDbType.Int){Value=source}};
            SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallRecord_ORIG_Task_Insert", parameters);
        }
        #endregion
    }
}

