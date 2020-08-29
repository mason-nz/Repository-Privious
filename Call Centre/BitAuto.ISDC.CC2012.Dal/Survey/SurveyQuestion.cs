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
    /// 数据访问类SurveyQuestion。
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
    public class SurveyQuestion : DataBase
    {
        #region Instance
        public static readonly SurveyQuestion Instance = new SurveyQuestion();
        #endregion

        #region const
        private const string P_SURVEYQUESTION_SELECT = "p_SurveyQuestion_Select";
        private const string P_SURVEYQUESTION_INSERT = "p_SurveyQuestion_Insert";
        private const string P_SURVEYQUESTION_UPDATE = "p_SurveyQuestion_Update";
        private const string P_SURVEYQUESTION_DELETE = "p_SurveyQuestion_Delete";
        private const string P_SURVEYQUESTION_STATQUESTIONFORMULTIPLECHOICE = "p_SurveyQuestion_StatQuestionForMultipleChoice";
        private const string P_SURVEYQUESTION_STATQUESTIONFORMATRIXDROPDOWN = "p_SurveyQuestion_StatQuestionForMatrixDropdown";
        private const string P_SURVEYQUESTION_STATQUESTINFORMATRIXRADIO = "p_SurveyQuestion_StatQuestionForMatrixRadio";
        private const string P_SURVEYQUESTION_GETQUESTIONFORMATRIXDROPDOWNSUMSCORE = "p_SurveyQuestion_GetQuestionForMatrixDropdownSumScore";
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
        public DataTable GetSurveyQuestion(QuerySurveyQuestion query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " And SIID=" + query.SIID + "";
            }

            if (query.SQID != Constant.INT_INVALID_VALUE)
            {
                where += " And SQID=" + query.SQID + "";
            }

            if (query.AskCategory != Constant.INT_INVALID_VALUE)
            {
                where += " And AskCategory=" + query.AskCategory + "";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status + "";
            }
            if (query.IsStatByScore != Constant.INT_INVALID_VALUE)
            {
                if (query.IsStatByScore == 1)
                {
                    where += " And IsStatByScore=1 ";
                }
                else
                {
                    where += " And (IsStatByScore=0 or IsStatByScore is null or IsStatByScore=-2)";
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 统计选择题
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMultipleChoice(int SQID, int SPIID)
        {
            SqlParameter[] parameters ={
                                            new SqlParameter("@SQID",SQID),
                                            new SqlParameter("@SPIID",SPIID)
                                      };

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_STATQUESTIONFORMULTIPLECHOICE, parameters);
            return ds.Tables[0];
        }


        /// <summary>
        /// 统计每个试题的总得分
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public int GetChoiceTotalScoreBySQID(int SQID)
        {
            int totalScore = 0;
            string sqlStr = "SELECT SUM(totalscore) FROM ( ";
            sqlStr += " SELECT sa.soid,COUNT(sa.createuserId)*score AS totalscore FROM SurveyAnswer AS sa JOIN surveyOption AS so ";
            sqlStr += " ON sa.soid=so.soid WHERE so.sqid=@SQID ";
            sqlStr += " group by sa.soid,score";
            sqlStr += " ) as b";

            SqlParameter parameter = new SqlParameter("@SQID", SQID);
            object objValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (objValue != null)
            {
                totalScore = int.Parse(objValue.ToString());
            }

            return totalScore;
        }
        /// <summary>
        /// 统计矩阵单选题
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMatrixRadio(int SQID, int SPIID)
        {
            SqlParameter[] parameters ={
                                            new SqlParameter("@SQID",SQID),
                                            new SqlParameter("@SPIID",SPIID)
                                      };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_STATQUESTINFORMATRIXRADIO, parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 统计矩阵下拉
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public DataTable StatQuestionForMatrixDropdown(int SQID, int SPIID)
        {
            SqlParameter[] parameters ={
                                            new SqlParameter("@SQID",SQID),
                                            new SqlParameter("@SPIID",SPIID)
                                      };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_STATQUESTIONFORMATRIXDROPDOWN, parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取项目下某个矩阵下拉问题所有问题的分数和
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="SPIID"></param>
        /// <returns></returns>
        public int GetQuestionForMatrixDropdownSumScore(int SQID, int SPIID)
        {
            int totalScore = 0;
            SqlParameter[] parameters ={
                                            new SqlParameter("@SQID",SQID),
                                            new SqlParameter("@SPIID",SPIID)
                                      };
            object returnValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_GETQUESTIONFORMATRIXDROPDOWNSUMSCORE, parameters);
            if (!string.IsNullOrEmpty(returnValue.ToString()))
            {
                totalScore = int.Parse(returnValue.ToString());
            }
            return totalScore;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyQuestion GetSurveyQuestion(int SQID)
        {
            QuerySurveyQuestion query = new QuerySurveyQuestion();
            query.SQID = SQID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyQuestion(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyQuestion(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyQuestion LoadSingleSurveyQuestion(DataRow row)
        {
            Entities.SurveyQuestion model = new Entities.SurveyQuestion();

            if (row["SQID"].ToString() != "")
            {
                model.SQID = int.Parse(row["SQID"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            model.Ask = row["Ask"].ToString();
            if (row["AskCategory"].ToString() != "")
            {
                model.AskCategory = int.Parse(row["AskCategory"].ToString());
            }
            if (row["ShowColumnNum"].ToString() != "")
            {
                model.ShowColumnNum = int.Parse(row["ShowColumnNum"].ToString());
            }
            if (row["MaxTextLen"].ToString() != "")
            {
                model.MaxTextLen = int.Parse(row["MaxTextLen"].ToString());
            }
            if (row["MinTextLen"].ToString() != "")
            {
                model.MinTextLen = int.Parse(row["MinTextLen"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["OrderNum"].ToString() != "")
            {
                model.OrderNum = int.Parse(row["OrderNum"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            if (row["IsMustAnswer"].ToString() != "")
            {
                model.IsMustAnswer = int.Parse(row["IsMustAnswer"].ToString());
            }
            else
            {
                model.IsMustAnswer = 0;
            }
            if (row["IsStatByScore"].ToString() != "")
            {
                model.IsStatByScore = int.Parse(row["IsStatByScore"].ToString());
            }
            else
            {
                model.IsStatByScore = 0;
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.SurveyQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@ShowColumnNum", SqlDbType.Int,4),
					new SqlParameter("@MaxTextLen", SqlDbType.Int,4),
					new SqlParameter("@MinTextLen", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsMustAnswer", SqlDbType.Int,4),
                    new SqlParameter("@IsStatByScore", SqlDbType.Int,4)                     
                                        
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.Ask;
            parameters[3].Value = model.AskCategory;
            parameters[4].Value = model.ShowColumnNum;
            parameters[5].Value = model.MaxTextLen;
            parameters[6].Value = model.MinTextLen;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderNum;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.ModifyTime;
            parameters[12].Value = model.ModifyUserID;
            parameters[13].Value = model.IsMustAnswer;
            parameters[14].Value = model.IsStatByScore;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@ShowColumnNum", SqlDbType.Int,4),
					new SqlParameter("@MaxTextLen", SqlDbType.Int,4),
					new SqlParameter("@MinTextLen", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsMustAnswer", SqlDbType.Int,4),
                    new SqlParameter("@IsStatByScore", SqlDbType.Int,4)                                           
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.Ask;
            parameters[3].Value = model.AskCategory;
            parameters[4].Value = model.ShowColumnNum;
            parameters[5].Value = model.MaxTextLen;
            parameters[6].Value = model.MinTextLen;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderNum;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.ModifyTime;
            parameters[12].Value = model.ModifyUserID;
            parameters[13].Value = model.IsMustAnswer;
            parameters[14].Value = model.IsStatByScore;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYQUESTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SurveyQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@ShowColumnNum", SqlDbType.Int,4),
					new SqlParameter("@MaxTextLen", SqlDbType.Int,4),
					new SqlParameter("@MinTextLen", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsMustAnswer", SqlDbType.Int,4),
                    new SqlParameter("@IsStatByScore", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.SQID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.Ask;
            parameters[3].Value = model.AskCategory;
            parameters[4].Value = model.ShowColumnNum;
            parameters[5].Value = model.MaxTextLen;
            parameters[6].Value = model.MinTextLen;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderNum;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.ModifyTime;
            parameters[12].Value = model.ModifyUserID;
            parameters[13].Value = model.IsMustAnswer;
            parameters[14].Value = model.IsStatByScore;


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyQuestion model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Ask", SqlDbType.NVarChar,2000),
					new SqlParameter("@AskCategory", SqlDbType.Int,4),
					new SqlParameter("@ShowColumnNum", SqlDbType.Int,4),
					new SqlParameter("@MaxTextLen", SqlDbType.Int,4),
					new SqlParameter("@MinTextLen", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@IsMustAnswer", SqlDbType.Int,4),
                    new SqlParameter("@IsStatByScore", SqlDbType.Int,4)
                                        
                                        
                                        };
            parameters[0].Value = model.SQID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.Ask;
            parameters[3].Value = model.AskCategory;
            parameters[4].Value = model.ShowColumnNum;
            parameters[5].Value = model.MaxTextLen;
            parameters[6].Value = model.MinTextLen;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderNum;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.ModifyTime;
            parameters[12].Value = model.ModifyUserID;
            parameters[13].Value = model.IsMustAnswer;
            parameters[14].Value = model.IsStatByScore;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYQUESTION_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SQID", SqlDbType.Int,4)};
            parameters[0].Value = SQID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYQUESTION_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SQID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SQID", SqlDbType.Int,4)};
            parameters[0].Value = SQID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYQUESTION_DELETE, parameters);
        }
        #endregion


        public List<Entities.SurveyQuestion> GetSurveyQuestionList(int siid)
        {
            List<Entities.SurveyQuestion> list = new List<Entities.SurveyQuestion>();
            Entities.QuerySurveyQuestion query = new QuerySurveyQuestion();
            int totalCount = 0;
            query.SIID = siid;
            query.Status = 0;
            DataTable dt = GetSurveyQuestion(query, "ordernum", 1, 9999, out totalCount);
            if (totalCount > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleSurveyQuestion(dr));
                }
            }

            return list;
        }

        /// <summary>
        /// 通过SIID获取问卷问题信息
        /// </summary>
        /// <param name="siid"></param>
        /// <returns></returns>
        public DataTable GetQuestionBySIID(int siid)
        {
            DataTable dt = new DataTable();
            SqlParameter[] _params = { new SqlParameter("@SIID", siid) };

            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SurveyQuestion_GetQuestionBySIID", _params).Tables[0];

            return dt;
        }


        /// <summary>
        /// 通过ProjectID获取问卷答案信息
        /// </summary>
        /// <param name="siid"></param>
        /// <param name="typeID">typeID=1：数据清洗任务；typeID=2：其他任务；typeID=3：客户回访</param>
        /// <returns></returns>
        public DataTable GetAnswerInfoByProjectID(int ProjectID, int SIID, int typeID)
        {
            DataTable dt = new DataTable();
            SqlParameter[] _params = { new SqlParameter("@SIID", SIID), new SqlParameter("@ProjectID", ProjectID), new SqlParameter("@type", typeID) };

            dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SurveyAnswer_GetAnswerInfoByProjectID", _params).Tables[0];

            return dt;
        }

    }
}

