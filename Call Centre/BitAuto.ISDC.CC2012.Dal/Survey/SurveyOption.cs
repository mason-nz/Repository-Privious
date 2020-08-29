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
    /// 数据访问类SurveyOption。
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
    public class SurveyOption : DataBase
    {
        #region Instance
        public static readonly SurveyOption Instance = new SurveyOption();
        #endregion

        #region const
        private const string P_SURVEYOPTION_SELECT = "p_SurveyOption_Select";
        private const string P_SURVEYOPTION_INSERT = "p_SurveyOption_Insert";
        private const string P_SURVEYOPTION_UPDATE = "p_SurveyOption_Update";
        private const string P_SURVEYOPTION_DELETE = "p_SurveyOption_Delete";
        #endregion

        #region Contructor
        protected SurveyOption()
        { }
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
        public DataTable GetSurveyOption(QuerySurveyOption query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.SQID != Constant.INT_INVALID_VALUE)
            {
                where += " And SQID=" + query.SQID;
            }
            if (query.SOID != Constant.INT_INVALID_VALUE)
            {
                where += " And SOID=" + query.SOID;
            }
            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " And SIID=" + query.SIID;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And Status=" + query.Status;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据试题ID查询此试题下的选项
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public int GetSurveyOptionCountBySQID(int SQID)
        {
            int count = 0;
            SqlParameter parameter = new SqlParameter("@SQID",SQID);
            string sqlStr = "SELECT COUNT(*) FROM SurveyOption WHERE SQID=@SQID AND Status=0";
            object objValue = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text,sqlStr, parameter);
            if (objValue != null)
            {
                count = int.Parse(objValue.ToString());
            }

            return count;
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SurveyOption GetSurveyOption(int SOID)
        {
            QuerySurveyOption query = new QuerySurveyOption();
            query.SOID = SOID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyOption(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyOption(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyOption LoadSingleSurveyOption(DataRow row)
        {
            Entities.SurveyOption model = new Entities.SurveyOption();

            if (row["SOID"].ToString() != "")
            {
                model.SOID = int.Parse(row["SOID"].ToString());
            }
            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            if (row["SQID"].ToString() != "")
            {
                model.SQID = int.Parse(row["SQID"].ToString());
            }
            model.OptionName = row["OptionName"].ToString();
            if (row["IsBlank"].ToString() != "")
            {
                model.IsBlank = int.Parse(row["IsBlank"].ToString());
            }
            if (row["Score"].ToString() != "")
            {
                model.Score = int.Parse(row["Score"].ToString());
            }
            if (row["OrderNum"].ToString() != "")
            {
                model.OrderNum = int.Parse(row["OrderNum"].ToString());
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
            if (row["ModifyTime"].ToString() != "")
            {
                model.ModifyTime = DateTime.Parse(row["ModifyTime"].ToString());
            }
            if (row["ModifyUserID"].ToString() != "")
            {
                model.ModifyUserID = int.Parse(row["ModifyUserID"].ToString());
            }
            if (row["linkid"].ToString() != "")
            {
                model.linkid = int.Parse(row["linkid"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.SurveyOption model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@OptionName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsBlank", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.OptionName;
            parameters[4].Value = model.IsBlank;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.OrderNum;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyOption model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@OptionName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsBlank", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.OptionName;
            parameters[4].Value = model.IsBlank;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.OrderNum;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_SURVEYOPTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.SurveyOption model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@OptionName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsBlank", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SOID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.OptionName;
            parameters[4].Value = model.IsBlank;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.OrderNum;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTION_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyOption model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SOID", SqlDbType.Int,4),
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@SQID", SqlDbType.Int,4),
					new SqlParameter("@OptionName", SqlDbType.NVarChar,2000),
					new SqlParameter("@IsBlank", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@OrderNum", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SOID;
            parameters[1].Value = model.SIID;
            parameters[2].Value = model.SQID;
            parameters[3].Value = model.OptionName;
            parameters[4].Value = model.IsBlank;
            parameters[5].Value = model.Score;
            parameters[6].Value = model.OrderNum;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.ModifyTime;
            parameters[11].Value = model.ModifyUserID;

            return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_SURVEYOPTION_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int SOID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SOID", SqlDbType.Int,4)};
            parameters[0].Value = SOID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYOPTION_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SOID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SOID", SqlDbType.Int,4)};
            parameters[0].Value = SOID;

            return SqlHelper.ExecuteNonQuery(sqltran,  CommandType.StoredProcedure, P_SURVEYOPTION_DELETE, parameters);
        }
        #endregion


        public List<Entities.SurveyOption> GetSurveyOptionList(int siid)
        {
            List<Entities.SurveyOption> list = new List<Entities.SurveyOption>();
            Entities.QuerySurveyOption query = new QuerySurveyOption();
            int totalCount = 0;
            query.SIID = siid;
            query.Status = 0;
            DataTable dt = GetSurveyOption(query, "ordernum", 1, 9999, out totalCount);
            if (totalCount > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleSurveyOption(dr));
                }
            }

            return list;
        }

        /// <summary>
        /// 查询试题下的所有选项
        /// </summary>
        /// <param name="sqid"></param>
        /// <returns></returns>
        public List<Entities.SurveyOption> GetSurveyOptionListBySQID(int sqid)
        {
            List<Entities.SurveyOption> list = new List<Entities.SurveyOption>();
            Entities.QuerySurveyOption query = new QuerySurveyOption();
            int totalCount = 0;
            query.SQID = sqid;
            query.Status = 0;
            DataTable dt = GetSurveyOption(query, "ordernum", 1, 9999, out totalCount);
            if (totalCount > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(LoadSingleSurveyOption(dr));
                }
            }

            return list;
        }
    }
}

