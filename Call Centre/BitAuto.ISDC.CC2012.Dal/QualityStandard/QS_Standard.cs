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
    /// ���ݷ�����QS_Standard��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:37 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_Standard : DataBase
    {
        #region Instance
        public static readonly QS_Standard Instance = new QS_Standard();
        #endregion

        #region const
        private const string P_QS_STANDARD_SELECT = "p_QS_Standard_Select";
        private const string P_QS_STANDARD_INSERT = "p_QS_Standard_Insert";
        private const string P_QS_STANDARD_UPDATE = "p_QS_Standard_Update";
        private const string P_QS_STANDARD_DELETE = "p_QS_Standard_Delete";
        #endregion

        #region Contructor
        protected QS_Standard()
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
        public DataTable GetQS_Standard(QueryQS_Standard query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.QS_SID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_SID=" + query.QS_SID;
            }
            if (query.QS_IID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_IID=" + query.QS_IID;
            }
            if (query.QS_CID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_CID=" + query.QS_CID;
            }
            if (query.QS_RTID != Constant.INT_INVALID_VALUE)
            {
                where += " And QS_RTID=" + query.QS_RTID;
            }
            if (query.ScoringStandardName != Constant.STRING_INVALID_VALUE)
            {
                where += " And ScoringStandardName  like '%" + StringHelper.SqlFilter(query.ScoringStandardName) + "%'";
            }
            if (query.IsIsDead != Constant.INT_INVALID_VALUE)
            {
                where += " And IsIsDead=" + query.IsIsDead;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " And CreateUserID=" + query.CreateUserID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_STANDARD_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.QS_Standard GetQS_Standard(int QS_SID)
        {
            QueryQS_Standard query = new QueryQS_Standard();
            query.QS_SID = QS_SID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetQS_Standard(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleQS_Standard(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.QS_Standard LoadSingleQS_Standard(DataRow row)
        {
            Entities.QS_Standard model = new Entities.QS_Standard();

            if (row["QS_SID"].ToString() != "")
            {
                model.QS_SID = int.Parse(row["QS_SID"].ToString());
            }
            if (row["QS_IID"].ToString() != "")
            {
                model.QS_IID = int.Parse(row["QS_IID"].ToString());
            }
            if (row["QS_CID"].ToString() != "")
            {
                model.QS_CID = int.Parse(row["QS_CID"].ToString());
            }
            if (row["QS_RTID"].ToString() != "")
            {
                model.QS_RTID = int.Parse(row["QS_RTID"].ToString());
            }
            model.ScoringStandardName = row["ScoringStandardName"].ToString();
            if (row["ScoreType"].ToString() != "")
            {
                model.ScoreType = int.Parse(row["ScoreType"].ToString());
            }
            if (row["Score"].ToString() != "")
            {
                model.Score = decimal.Parse(row["Score"].ToString());
            }
            if (row["IsIsDead"].ToString() != "")
            {
                model.IsIsDead = int.Parse(row["IsIsDead"].ToString());
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
            if (row["LastModifyTime"].ToString() != "")
            {
                model.LastModifyTime = DateTime.Parse(row["LastModifyTime"].ToString());
            }
            if (row["LastModifyUserID"].ToString() != "")
            {
                model.LastModifyUserID = int.Parse(row["LastModifyUserID"].ToString());
            }
            if (row["Sort"].ToString() != "")
            {
                model.Sort = int.Parse(row["Sort"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.QS_Standard model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ScoringStandardName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@IsIsDead", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4),
                    new SqlParameter("@SExplanation",  SqlDbType.NVarChar,500),
                    new SqlParameter("@SkillLevel", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.QS_IID;
            parameters[2].Value = model.QS_CID;
            parameters[3].Value = model.QS_RTID;
            parameters[4].Value = model.ScoringStandardName;
            parameters[5].Value = model.ScoreType;
            parameters[6].Value = model.Score;
            parameters[7].Value = model.IsIsDead;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.LastModifyTime;
            parameters[12].Value = model.LastModifyUserID;
            parameters[13].Value = model.Sort;
            parameters[14].Value = model.QS_SExplanation;
            parameters[15].Value = model.QS_SkillLevel;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_STANDARD_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.QS_Standard model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ScoringStandardName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@IsIsDead", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4),
                    new SqlParameter("@SExplanation",  SqlDbType.NVarChar,500),
                    new SqlParameter("@SkillLevel", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.QS_IID;
            parameters[2].Value = model.QS_CID;
            parameters[3].Value = model.QS_RTID;
            parameters[4].Value = model.ScoringStandardName;
            parameters[5].Value = model.ScoreType;
            parameters[6].Value = model.Score;
            parameters[7].Value = model.IsIsDead;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.LastModifyTime;
            parameters[12].Value = model.LastModifyUserID;
            parameters[13].Value = model.Sort;
            parameters[14].Value = model.QS_SExplanation;
            parameters[15].Value = model.QS_SkillLevel;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_STANDARD_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.QS_Standard model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ScoringStandardName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@IsIsDead", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
				 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4),
                    new SqlParameter("@SExplanation",  SqlDbType.NVarChar,500),
                    new SqlParameter("@SkillLevel", SqlDbType.Int,4)};
            parameters[0].Value = model.QS_SID;
            parameters[1].Value = model.QS_IID;
            parameters[2].Value = model.QS_CID;
            parameters[3].Value = model.QS_RTID;
            parameters[4].Value = model.ScoringStandardName;
            parameters[5].Value = model.ScoreType;
            parameters[6].Value = model.Score;
            parameters[7].Value = model.IsIsDead;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.LastModifyTime;
            parameters[10].Value = model.LastModifyUserID;
            parameters[11].Value = model.Sort;
            parameters[12].Value = model.QS_SExplanation;
            parameters[13].Value = model.QS_SkillLevel;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_STANDARD_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.QS_Standard model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_SID", SqlDbType.Int,4),
					new SqlParameter("@QS_IID", SqlDbType.Int,4),
					new SqlParameter("@QS_CID", SqlDbType.Int,4),
					new SqlParameter("@QS_RTID", SqlDbType.Int,4),
					new SqlParameter("@ScoringStandardName", SqlDbType.NVarChar,500),
					new SqlParameter("@ScoreType", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Decimal),
					new SqlParameter("@IsIsDead", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
			 
					new SqlParameter("@LastModifyTime", SqlDbType.DateTime),
					new SqlParameter("@LastModifyUserID", SqlDbType.Int,4),
					new SqlParameter("@Sort", SqlDbType.Int,4),
                    new SqlParameter("@SExplanation",  SqlDbType.NVarChar,500),
                    new SqlParameter("@SkillLevel", SqlDbType.Int,4)};
            parameters[0].Value = model.QS_SID;
            parameters[1].Value = model.QS_IID;
            parameters[2].Value = model.QS_CID;
            parameters[3].Value = model.QS_RTID;
            parameters[4].Value = model.ScoringStandardName;
            parameters[5].Value = model.ScoreType;
            parameters[6].Value = model.Score;
            parameters[7].Value = model.IsIsDead;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.LastModifyTime;
            parameters[10].Value = model.LastModifyUserID;
            parameters[11].Value = model.Sort;
            parameters[12].Value = model.QS_SExplanation;
            parameters[13].Value = model.QS_SkillLevel;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_STANDARD_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int QS_SID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_SID", SqlDbType.Int,4)};
            parameters[0].Value = QS_SID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_QS_STANDARD_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int QS_SID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@QS_SID", SqlDbType.Int,4)};
            parameters[0].Value = QS_SID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_QS_STANDARD_DELETE, parameters);
        }
        #endregion

    }
}

