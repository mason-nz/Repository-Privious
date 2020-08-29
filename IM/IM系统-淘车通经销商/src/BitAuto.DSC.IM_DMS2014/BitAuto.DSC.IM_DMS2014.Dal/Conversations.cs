using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类Conversations。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class Conversations : DataBase
    {
        #region Instance
        public static readonly Conversations Instance = new Conversations();
        #endregion

        #region const
        private const string P_CONVERSATIONS_SELECT = "p_Conversations_Select";
        private const string P_CONVERSATIONS_INSERT = "p_Conversations_Insert";
        private const string P_CONVERSATIONS_UPDATE = "p_Conversations_Update";
        private const string P_CONVERSATIONS_DELETE = "p_Conversations_Delete";
        #endregion

        #region Contructor
        protected Conversations()
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
        public DataTable GetConversations(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                where += " and CSID=" + query.CSID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONS_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.Conversations GetConversations(int CSID)
        {
            QueryConversations query = new QueryConversations();
            query.CSID = CSID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetConversations(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleConversations(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.Conversations LoadSingleConversations(DataRow row)
        {
            Entities.Conversations model = new Entities.Conversations();

            if (row["CSID"].ToString() != "")
            {
                model.CSID = int.Parse(row["CSID"].ToString());
            }
            if (row["UserID"].ToString() != "")
            {
                model.UserID = int.Parse(row["UserID"].ToString());
            }
            model.UserName = row["UserName"].ToString();
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            model.VisitID = row["VisitID"].ToString();
            if (row["AgentStartTime"].ToString() != "")
            {
                model.AgentStartTime = DateTime.Parse(row["AgentStartTime"].ToString());
            }
            if (row["LastClientTime"].ToString() != "")
            {
                model.LastClientTime = DateTime.Parse(row["LastClientTime"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            model.OrderID = row["OrderID"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["EndTime"].ToString() != "")
            {
                model.EndTime = DateTime.Parse(row["EndTime"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.Conversations model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.VisitID;
            parameters[5].Value = model.AgentStartTime;
            parameters[6].Value = model.LastClientTime;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderID;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.EndTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONS_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.Conversations model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.VisitID;
            parameters[5].Value = model.AgentStartTime;
            parameters[6].Value = model.LastClientTime;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderID;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.EndTime;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONVERSATIONS_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.Conversations model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.VisitID;
            parameters[5].Value = model.AgentStartTime;
            parameters[6].Value = model.LastClientTime;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderID;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.EndTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONS_UPDATE, parameters);
        }

        public int CallBackUpdate(Entities.Conversations model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", model.CSID),					
					new SqlParameter("@VisitID", model.VisitID),
					new SqlParameter("@AgentStartTime",model.AgentStartTime),
					new SqlParameter("@LastClientTime", model.LastClientTime),
                    new SqlParameter("@EndTime", model.EndTime)};


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallBackCons_Update", parameters);
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.Conversations model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@VisitID", SqlDbType.VarChar,50),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.VisitID;
            parameters[5].Value = model.AgentStartTime;
            parameters[6].Value = model.LastClientTime;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.OrderID;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.EndTime;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONVERSATIONS_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int CSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)};
            parameters[0].Value = CSID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONS_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", SqlDbType.Int,4)};
            parameters[0].Value = CSID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_CONVERSATIONS_DELETE, parameters);
        }
        #endregion

        public string GetWhere(QueryConversations query)
        {
            string where = "";
            if (query.AgentStartTime != Constant.DATE_INVALID_VALUE && query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and AgentStartTime >=" + DateTime.Parse(query.AgentStartTime.ToString()).ToString("yyyy-MM-dd") + "'";
                where += " and EndTime <" + DateTime.Parse(query.EndTime.ToString()).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and BGID=" + query.BGID;
            }
            if (query.CreateTime != Constant.DATE_INVALID_VALUE)
            {
            }
            return "";
        }

        //<summary>
        //判断会话是否存在
        //</summary>
        //<param name="CSID">会话ID</param>
        //<returns></returns>
        public bool Exists(int CSID)
        {
            String strSql = "select count(1) from Conversations where CSID=@CSID";
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
        /// 根据指定条件查询会话数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCSData(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " and d.TrueName like '%" + StringHelper.SqlFilter(query.UserName) + "%'"; //通过存储过程决定 客服名称的字段名
            }
            if (query.CreateTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and a.CreateTime=" + query.CreateTime;
            }
            if (query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and a.EndTime='" + query.EndTime + "'";
            }
            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.OrderID ='" + StringHelper.SqlFilter(query.OrderID) + "'";
            }
            if (query.MemberName != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.MemberName like '%" + StringHelper.SqlFilter(query.MemberName) + "%'";
            }
            if (query.District != Constant.STRING_INVALID_VALUE && query.District != "-1")
            {
                where += " and c.District ='" + StringHelper.SqlFilter(query.District) + "'";
            }
            
            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                where += " and a.EndTime>='" + query.QueryStarttime + "'";
            }
            if (query.QueryEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += "and a.EndTime<'" + query.QueryEndTime + "'";
            }
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CSInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据VisitID获取客户信息
        /// exec p_GetMemberInfoByVisitID ' and VisitID=''001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetMemberInfoByVisitID(string strWhere)
        {
            SqlParameter[] parameters = {
					    new SqlParameter("@where", SqlDbType.VarChar, 50)
					};

            parameters[0].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetMemberInfoByVisitID", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据VisitID获取会话相关信息
        /// exec p_CSRelateInfoByVisitID ' and a.VisitID=''001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetCSRelateInfoByCSID(string strWhere)
        {
            SqlParameter[] parameters = {
					    new SqlParameter("@where", SqlDbType.VarChar, 50)
					};

            parameters[0].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetCSRelateInfoByCSID", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据OrderID查询工单相关信息
        /// exec p_GetWorkOrderInfoByOrderID ' and a.OrderID = ''WO20130000000001'''
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoByOrderID(string strWhere)
        {
            SqlParameter[] parameters = {
					    new SqlParameter("@WorkOrderID", SqlDbType.VarChar, 50)
					};

            parameters[0].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetWorkOrderInfoByOrderID", parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据指定条件查询会话数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetConversationHistoryData(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.CSID=" + query.CSID;
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += " and b.UserID=" + query.UserID;
            }
            if (query.CreateTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and a.CreateTime='" + query.CreateTime + "'";
            }
            if (query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " and a.EndTime=" + query.EndTime;
            }
            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.OrderID ='" + StringHelper.SqlFilter(query.OrderID) + "'";
            }
            if (query.MemberName != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.MemberName like '%" + StringHelper.SqlFilter(query.MemberName) + "%'";
            }
            if (query.District != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.District ='" + StringHelper.SqlFilter(query.District) + "'";
            }
            if (query.CityGroup != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.CityGroup ='" + StringHelper.SqlFilter(query.CityGroup) + "'";
            }
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.LoginID=" + query.LoginID;
            }
            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                where += " and c.CreateTime>'" + query.QueryStarttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }

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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConversationHistory_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }


        public DataTable GetConversationingCSData(string strWhere)
        {
            SqlParameter[] parameters = {
					    new SqlParameter("@where", SqlDbType.VarChar, 200)
					};

            parameters[0].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConversationingCSData_Select", parameters);
            return ds.Tables[0];
        }
    }
}

