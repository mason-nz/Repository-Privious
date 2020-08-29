using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类UserSatisfaction。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:05 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserSatisfaction : DataBase
    {
        #region Instance
        public static readonly UserSatisfaction Instance = new UserSatisfaction();
        #endregion

        #region const
        private const string P_USERSATISFACTION_SELECT = "p_UserSatisfaction_Select";
        private const string P_USERSATISFACTION_INSERT = "p_UserSatisfaction_Insert";
        private const string P_USERSATISFACTION_UPDATE = "p_UserSatisfaction_Update";
        private const string P_USERSATISFACTION_DELETE = "p_UserSatisfaction_Delete";
        #endregion

        #region Contructor
        protected UserSatisfaction()
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
        public DataTable GetUserSatisfaction(QueryUserSatisfaction query, string order, int currentPage, int pageSize, out int totalCount)
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserSatisfaction GetUserSatisfaction(int RecID)
        {
            QueryUserSatisfaction query = new QueryUserSatisfaction();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserSatisfaction(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleUserSatisfaction(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.UserSatisfaction LoadSingleUserSatisfaction(DataRow row)
        {
            Entities.UserSatisfaction model = new Entities.UserSatisfaction();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["CSID"].ToString() != "")
            {
                model.CSID = int.Parse(row["CSID"].ToString());
            }
            if (row["PerSatisfaction"].ToString() != "")
            {
                model.PerSatisfaction = int.Parse(row["PerSatisfaction"].ToString());
            }
            if (row["ProSatisfaction"].ToString() != "")
            {
                model.ProSatisfaction = int.Parse(row["ProSatisfaction"].ToString());
            }
            model.Contents = row["Contents"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.UserSatisfaction model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CSID;
            parameters[2].Value = model.PerSatisfaction;
            parameters[3].Value = model.ProSatisfaction;
            parameters[4].Value = model.Contents;
            parameters[5].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserSatisfaction model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CSID;
            parameters[2].Value = model.PerSatisfaction;
            parameters[3].Value = model.ProSatisfaction;
            parameters[4].Value = model.Contents;
            parameters[5].Value = model.CreateTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERSATISFACTION_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.UserSatisfaction model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CSID;
            parameters[2].Value = model.PerSatisfaction;
            parameters[3].Value = model.ProSatisfaction;
            parameters[4].Value = model.Contents;
            parameters[5].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UserSatisfaction model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@PerSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@ProSatisfaction", SqlDbType.Int,4),
					new SqlParameter("@Contents", SqlDbType.VarChar,1000),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CSID;
            parameters[2].Value = model.PerSatisfaction;
            parameters[3].Value = model.ProSatisfaction;
            parameters[4].Value = model.Contents;
            parameters[5].Value = model.CreateTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERSATISFACTION_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_USERSATISFACTION_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_USERSATISFACTION_DELETE, parameters);
        }
        #endregion


        //<summary>
        //判断对指定会话的满意度评价是否存在
        //</summary>
        //<param name="CSID">会话ID</param>
        //<returns></returns>
        public bool SatisfactionExists(int CSID)
        {
            String strSql = "select count(1) from UserSatisfaction where CSID=@CSID";
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)
			};
            parameters[0].Value = CSID;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);

            int objNum;
            if (obj != null && int.TryParse(obj.ToString(), out objNum))
            {
                if (objNum > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取满意度统计数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns></returns>
        public DataTable UserSatisfaction_Total_Select(QueryUserSatisfactionTotal query,string order, int currentPage, int pageSize, out int totalCount,int logUserID)
        {


        //    private int? _bgid;
        //private int? _userid;
        //private string _agentnum;
        //private DateTime? begintime;
        //private DateTime? endtime;
        ////统计方式，1，日，2，周，3.月
            string where = string.Empty;
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                if (query.BGID == -1)
                {
                    where += " and (c.BGID in (SELECT DISTINCT a.BGID FROM    cc2012.dbo.UserGroupDataRigth a INNER JOIN cc2012.dbo.BusinessGroup b ON a.BGID = b.BGID WHERE   b.Status = 0 and (b.BusinessType=2 or b.BusinessType=3) AND a.UserID='" + logUserID + "') or S_UserSatisfaction_Total.userID=" + logUserID + ")";
                }
                else
                {
                    where += " and c.BGID=" + query.BGID;
                }
            }
            if(query.UserID!=Constant.INT_INVALID_VALUE)
            {
                where+=" and S_UserSatisfaction_Total.UserID="+query.UserID;
            }
            if (query.AgentNum != Constant.STRING_EMPTY_VALUE)
            {
                where+=" and c.AgentNum='"+StringHelper.SqlFilter(query.AgentNum)+"'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@selectType",SqlDbType.Int,4),
                    new SqlParameter("@beginTime",SqlDbType.DateTime),
                    new SqlParameter("@endTime",SqlDbType.DateTime),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = query.SelectType;
            parameters[1].Value = query.BeginTime;
            parameters[2].Value = query.EndTime;
            parameters[3].Value = where;
            parameters[4].Value = order;
            parameters[5].Value = pageSize;
            parameters[6].Value = currentPage;
            parameters[7].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Satisfaction_Total_Select", parameters);
            totalCount = (int)(parameters[7].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 取满意度统计数据汇总数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="logUserID"></param>
        /// <returns></returns>
        public DataTable UserSatisfaction_Total_Select(QueryUserSatisfactionTotal query,int logUserID)
        {


            //    private int? _bgid;
            //private int? _userid;
            //private string _agentnum;
            //private DateTime? begintime;
            //private DateTime? endtime;
            ////统计方式，1，日，2，周，3.月
            string where = string.Empty;
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                if (query.BGID == -1)
                {
                    where += " and (c.BGID in (SELECT DISTINCT a.BGID FROM    cc2012.dbo.UserGroupDataRigth a INNER JOIN cc2012.dbo.BusinessGroup b ON a.BGID = b.BGID WHERE   b.Status = 0 and (b.BusinessType=2 or b.BusinessType=3) AND a.UserID='" + logUserID + "') or S_UserSatisfaction_Total.userID=" + logUserID + ")";
                }
                else
                {
                    where += " and c.BGID=" + query.BGID;
                }
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and S_UserSatisfaction_Total.UserID=" + query.UserID;
            }
            if (query.AgentNum != Constant.STRING_EMPTY_VALUE)
            {
                where += " and c.AgentNum='" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
                    new SqlParameter("@beginTime",SqlDbType.DateTime),
                    new SqlParameter("@endTime",SqlDbType.DateTime),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000)
					};
            parameters[0].Value = query.BeginTime;
            parameters[1].Value = query.EndTime;
            parameters[2].Value = where;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Satisfaction_Total_SUM_Select", parameters);
            return ds.Tables[0];
        }

    }
}

