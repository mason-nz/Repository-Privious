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
    /// ���ݷ�����SurveyMatrixTitle��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:18 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyMatrixTitle : DataBase
    {
        #region Instance
        public static readonly SurveyMatrixTitle Instance = new SurveyMatrixTitle();
        #endregion

        #region const
        private const string P_SURVEYMATRIXTITLE_SELECT = "p_SurveyMatrixTitle_Select";
        private const string P_SURVEYMATRIXTITLE_INSERT = "p_SurveyMatrixTitle_Insert";
        private const string P_SURVEYMATRIXTITLE_UPDATE = "p_SurveyMatrixTitle_Update";
        private const string P_SURVEYMATRIXTITLE_DELETE = "p_SurveyMatrixTitle_Delete";
        private const string P_SURVEYMATRIXTITLE_STATOPTIONFORRADIO = "p_SurveyMatrixTitle_StatOptionForRadio";
        #endregion

        #region Contructor
        protected SurveyMatrixTitle()
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
        public DataTable GetSurveyMatrixTitle(QuerySurveyMatrixTitle query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.SMTID != Constant.INT_INVALID_VALUE)
            {
                where += " And SMTID=" + query.SMTID;
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " And SIID=" + query.SIID;
            }
            if (query.SQID != Constant.INT_INVALID_VALUE)
            {
                where += " And SQID=" + query.SQID;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status;
            }
            if (query.Type != Constant.INT_INVALID_VALUE)
            {
                where += " And Type=" + query.Type;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// ��ȡĳ������������������ĸ���
        /// </summary>
        /// <param name="sqid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetSurveyMatrixTitleCount(int sqid, int type)
        {
            int count = 0;
            string sqlStr = "SELECT COUNT(*) FROM SurveyMatrixTitle WHERE SQID=@SQID And Type=@Type And  Status=0";
            SqlParameter[] parameters ={
                                           new SqlParameter("@SQID",SqlDbType.Int),
                                           new SqlParameter("@Type",SqlDbType.Int)
                                      };
            parameters[0].Value = sqid;
            parameters[1].Value = type;

            object objValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (objValue != null)
            {
                count = int.Parse(objValue.ToString());
            }

            return count;
        }
        #endregion

        /// <summary>
        /// ͳ�Ƶ�����
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatOptionForMatrixRadio(int SMTID,int SPIID)
        {
            SqlParameter[] parameters ={
                new SqlParameter("@SMTID", SMTID),
                new SqlParameter("@SPIID", SPIID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_STATOPTIONFORRADIO, parameters);
            return ds.Tables[0];
        }

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyMatrixTitle GetSurveyMatrixTitle(int SMTID)
        {
            QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            query.SMTID = SMTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyMatrixTitle(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyMatrixTitle(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyMatrixTitle LoadSingleSurveyMatrixTitle(DataRow row)
        {
            Entities.SurveyMatrixTitle model = new Entities.SurveyMatrixTitle();

            if (row["SMTID"].ToString() != "")
            {
                model.SMTID = int.Parse(row["SMTID"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            if (row["SQID"].ToString() != "")
            {
                model.SQID = int.Parse(row["SQID"].ToString());
            }
            model.TitleName = row["TitleName"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["Type"].ToString() != "")
            {
                model.Type = int.Parse(row["Type"].ToString());
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
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.SurveyMatrixTitle model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SMTID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@TitleName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.TitleName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Type;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyMatrixTitle model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SMTID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@TitleName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.TitleName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Type;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.SurveyMatrixTitle model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SMTID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@TitleName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SMTID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.TitleName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Type;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyMatrixTitle model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SMTID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@TitleName", SqlDbType.NVarChar,2000),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SMTID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.TitleName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.Type;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int SMTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SMTID", SqlDbType.Int,4)};
            parameters[0].Value = SMTID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SMTID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SMTID", SqlDbType.Int,4)};
            parameters[0].Value = SMTID;

            return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_SURVEYMATRIXTITLE_DELETE, parameters);
        }
        #endregion


        public List<Entities.SurveyMatrixTitle> GetMatrixTitleList(int siid)
        {
            List<Entities.SurveyMatrixTitle> list = new List<Entities.SurveyMatrixTitle>();
            Entities.QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            int totalCount = 0;
            query.SIID = siid;
            query.Status = 0;
            DataTable dt = GetSurveyMatrixTitle(query, "", 1, 9999, out totalCount);
            if (totalCount > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleSurveyMatrixTitle(dr));
                }
            }

            return list;
        }
    }
}

