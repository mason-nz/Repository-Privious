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

        /// <summary>
        /// 根据CSID获取本次会话的orderid,custid
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public DataTable CheckConversationOrderCustInfo(string csid)
        {
            string strSql =
                string.Format(
                    "SELECT a.CSID,a.OrderID,b.CustID,b.VisitID FROM Conversations a JOIN UserVisitLog b ON a.VisitID= b.VisitID WHERE a.CSID={0} ",
                    csid);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }


        /// <summary>
        /// 按照查询条件查询客服统计
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataSet GetConverStatistics(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            where += " and Conver.CreateTime >='" + DateTime.Parse(query.CreateTime.ToString()).ToString("yyyy-MM-dd") + "'";

            where += " and Conver.CreateTime <'" + DateTime.Parse(query.EndTime.ToString()).ToString("yyyy-MM-dd") + "'";
            if (!string.IsNullOrEmpty(query.UserName))
            {
                where += " and Conver.UserName like '%" + query.UserName + "%'";
            }
            if (query.AgentNum != 0)
            {
                where += " and Conver.UserID=(select top 1 UserId from v_AgentInfo where AgentNum=" + query.AgentNum + ") ";
            }
            if (query.SourceType != 0)
            {
                where += " and Conver.VisitID in(select VisitID from UserVisitLog where sourceType=" + query.SourceType + " )";
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and Conver.UserID in (select UserId from v_AgentInfo where BGID in(select  BGID from V_UserGroupDataRigth where userid=" + query.UserID + " and BGID=" + query.BGID + " ))";
            }
            else
            {
                where += " and Conver.UserID in (select UserId from v_AgentInfo where BGID in(select  BGID from V_UserGroupDataRigth where userid=" + query.UserID + " ) union select " + query.UserID + ")";
            }


            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@BgeinDate", SqlDbType.NVarChar, 20),
					new SqlParameter("@endDate", SqlDbType.NVarChar, 20),
					new SqlParameter("@where", SqlDbType.NVarChar, 20000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = query.CreateTime;
            parameters[1].Value = query.EndTime;
            parameters[2].Value = where;
            parameters[3].Value = order;
            parameters[4].Value = pageSize;
            parameters[5].Value = currentPage;
            parameters[6].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_Conversations_statistics", parameters);
            if (ds.Tables.Count == 2)
            {
                totalCount = (int)(parameters[6].Value);
            }
            else
            {
                totalCount = 0;
            }
            return ds;
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
            //if (row["BGID"].ToString() != "")
            //{
            //    model.BGID = int.Parse(row["BGID"].ToString());
            //}
            if (row["VisitID"].ToString() != "")
            {
                model.VisitID = int.Parse(row["VisitID"].ToString());
            }
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

            if (row["IsTurnIn"].ToString() != "")
            {
                if (row["IsTurnIn"].ToString() == "True")
                {
                    model.IsTurenIn = true;
                }
                else
                {
                    model.IsTurenIn = false;
                }
            }
            if (row["IsTurnOut"].ToString() != "")
            {
                if (row["IsTurnOut"].ToString() == "True")
                {
                    model.IsTurenOut = true;
                }
                else
                {
                    model.IsTurenOut = false;
                }
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
					new SqlParameter("@VisitID", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),					
					new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@IsTurnIn", SqlDbType.Bit),
                    new SqlParameter("@IsTurnOut", SqlDbType.Bit)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.VisitID;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.LastClientTime;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.EndTime;
            parameters[9].Value = model.IsTurenIn;
            parameters[10].Value = model.IsTurenOut;
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
					new SqlParameter("@VisitID", SqlDbType.Int,4),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),					
					new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@IsTurnIn", SqlDbType.Bit),
                    new SqlParameter("@IsTurnOut", SqlDbType.Bit)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.VisitID;
            parameters[4].Value = model.AgentStartTime;
            parameters[5].Value = model.LastClientTime;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.EndTime;
            parameters[9].Value = model.IsTurenIn;
            parameters[10].Value = model.IsTurenOut;

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
					new SqlParameter("@VisitID", SqlDbType.Int,4),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@IsTurnIn", SqlDbType.Bit),
                    new SqlParameter("@IsTurnOut", SqlDbType.Bit)
                                        };
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.VisitID;
            parameters[4].Value = model.AgentStartTime;
            parameters[5].Value = model.LastClientTime;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.EndTime;
            parameters[10].Value = model.IsTurenIn;
            parameters[11].Value = model.IsTurenOut;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CONVERSATIONS_UPDATE, parameters);
        }

        public int CallBackUpdate(Entities.Conversations model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@CSID", model.CSID),					
					new SqlParameter("@VisitID", model.VisitID),
					new SqlParameter("@AgentStartTime",model.AgentStartTime),
					new SqlParameter("@LastClientTime", model.LastClientTime),
                    new SqlParameter("@EndTime", model.EndTime),
                    //new SqlParameter("@IsTurnIn", model.IsTurenIn),
                    new SqlParameter("@ISTurnOut", model.IsTurenOut)                                        
                                        };


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallBackCons_Update", parameters);
        }

        public int UpdateConversationReplyTime(DateTime dt, int CSID)
        {
            SqlParameter[] parameters = {				                    
                    new SqlParameter("@dt", dt) ,                                       
                    new SqlParameter("@csid", CSID)                                        
                                        };
            string strSql = " UPDATE  dbo.Conversations SET AgentStartTime=@dt WHERE CSID=@csid ";


            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
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
					new SqlParameter("@VisitID", SqlDbType.Int,4),
					new SqlParameter("@AgentStartTime", SqlDbType.DateTime),
					new SqlParameter("@LastClientTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.VarChar,50),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.CSID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.VisitID;
            parameters[4].Value = model.AgentStartTime;
            parameters[5].Value = model.LastClientTime;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.OrderID;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.EndTime;

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

            //数据权限控制(当前登录人+当前登录人的管辖分组ids)
            if (query.RightBGIDs != Constant.STRING_INVALID_VALUE)
            {
                where += " AND (a.UserID = '" + query.UserID + "' OR c.BGID IN (" + query.RightBGIDs + "))";
            }
            else
            {
                where += " AND a.UserID = '" + query.UserID + "'";
            }
            where += " AND c.RegionID=" + query.AreaType;

            if (query.VisitorName != Constant.STRING_INVALID_VALUE)
            {
                where += "  AND b.UserName like '%" + query.VisitorName + "%'";
            }
            if (query.VisitorPhone != Constant.STRING_INVALID_VALUE)
            {
                where += " AND b.Phone='" + query.VisitorPhone + "'";
            }
            if (query.VisitorProvinceID != Constant.INT_INVALID_VALUE && query.VisitorProvinceID != -1)
            {
                where += " AND b.ProvinceID = '" + query.VisitorProvinceID + "'";
            }
            if (query.VisitorCityID != Constant.INT_INVALID_VALUE && query.VisitorCityID != -1)
            {
                where += " AND b.CityID = '" + query.VisitorCityID + "'";
            }

            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND d.TrueName like '%" + query.UserName + "%'";
            }
            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID ='" + query.OrderID + "'";
            }
            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.EndTime>='" + query.QueryStarttime + "'";
            }
            if (query.QueryEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.EndTime<='" + query.QueryEndTime + "'";
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
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetVisitorInfo", parameters);
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
					    new SqlParameter("@where", SqlDbType.VarChar, 50)
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

            //数据权限控制(当前登录人+当前登录人的管辖分组ids)
            //if (query.RightBGIDs != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " AND (b.UserID = '" + query.UserID + "' OR e.BGID  IN (" + query.RightBGIDs + "))";
            //}
            //else
            //{
            //    where += " AND a.UserID = '" + query.UserID + "'";
            //}
            where = query.RightBGIDs;
            where += " AND e.RegionID='" + query.AreaType + "'";



            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                where += " AND b.CSID=" + query.CSID;
            }
            if (query.LoginID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.LoginID='" + StringHelper.SqlFilter(query.LoginID) + "'";
            }
            if (query.UserID != Constant.INT_INVALID_VALUE)
            {
                where += "  AND b.UserID=" + query.UserID;
            }
            if (query.CreateTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND b.CreateTime='" + query.CreateTime + "'";
            }
            if (query.EndTime != Constant.DATE_INVALID_VALUE)
            {
                where += "  AND b.EndTime=" + query.EndTime;
            }
            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND b.OrderID='" + StringHelper.SqlFilter(query.OrderID) + "'";
            }
            if (query.VisitorName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.UserName like '%" + StringHelper.SqlFilter(query.VisitorName) + "%'";
            }

            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                where += "  AND c.CreateTime>'" + query.QueryStarttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }
            if (query.QueryEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += "  AND c.CreateTime<'" + query.QueryEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }

            string strTableName = "";
            if (query.CSID != Constant.INT_INVALID_VALUE && query.CSID != -1)
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_' + CONVERT(VARCHAR(6),createtime,112) FROM Conversations WHERE CSID=" + query.CSID;
            }
            else if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_" + query.QueryStarttime.Value.ToString("yyyyMM") + "'";
            }
            else
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_' + CONVERT(VARCHAR(6),GETDATE(),112)";
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@tablename", SqlDbType.NVarChar, 300),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = strTableName;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConversationHistory_Select", parameters);
            totalCount = (int)(parameters[5].Value);
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


        public DataTable ExportCSDataForLiaoTianJiLu(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            //数据权限控制(当前登录人+当前登录人的管辖分组ids)
            if (query.RightBGIDs != Constant.STRING_INVALID_VALUE)
            {
                where += " AND (a.UserID = '" + query.UserID + "' OR c.BGID IN (" + query.RightBGIDs + "))";
            }

            if (query.VisitorName != Constant.STRING_INVALID_VALUE)
            {
                where += "  AND b.UserName like '%" + query.VisitorName + "%'";
            }
            if (query.VisitorPhone != Constant.STRING_INVALID_VALUE)
            {
                where += " AND b.Phone='" + query.VisitorPhone + "'";
            }
            if (query.VisitorProvinceID != Constant.INT_INVALID_VALUE && query.VisitorProvinceID != -1)
            {
                where += " AND b.ProvinceID = '" + query.VisitorProvinceID + "'";
            }
            if (query.VisitorCityID != Constant.INT_INVALID_VALUE && query.VisitorCityID != -1)
            {
                where += " AND b.CityID = '" + query.VisitorCityID + "'";
            }

            if (query.UserName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.UserName like '%" + query.UserName + "%'";
            }
            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID ='" + query.OrderID + "'";
            }
            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.EndTime>='" + query.QueryStarttime + "'";
            }
            if (query.QueryEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += " AND a.EndTime<'" + query.QueryEndTime + "'";
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CSInfoExport_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        public DataTable GetConversationHistoryDataForCC(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.CSID != Constant.INT_INVALID_VALUE)
            {
                where += " AND b.CSID=" + query.CSID;
            }
            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND b.OrderID='" + query.OrderID + "'";
            }

            string strTableName = "";
            if (query.CSID != Constant.INT_INVALID_VALUE && query.CSID != -1)
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_' + CONVERT(VARCHAR(6),createtime,112) FROM Conversations WHERE CSID=" + query.CSID;
            }
            else if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_' + CONVERT(VARCHAR(6),createtime,112) FROM Conversations WHERE OrderID='" + query.OrderID + "'";
            }
            else
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_' + CONVERT(VARCHAR(6),GETDATE(),112)";
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@tablename", SqlDbType.NVarChar, 300),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = strTableName;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConversationHistory_Select", parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

        public DataTable GetConversationHistoryDataNew(QueryConversations query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.LoginID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.LoginID='" + query.LoginID + "'";
            }
            if (query.VisitID != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.LoginID=(SELECT TOP 1 LoginID FROM dbo.UserVisitLog WHERE VisitID='" + query.VisitID + "')";
            }
            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                where += "  AND c.CreateTime>'" + query.QueryStarttime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }
            if (query.QueryEndTime != Constant.DATE_INVALID_VALUE)
            {
                where += "  AND c.CreateTime<'" + query.QueryEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }

            string strTableName = "";
            if (query.QueryStarttime != Constant.DATE_INVALID_VALUE)
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_" + query.QueryStarttime.Value.ToString("yyyyMM") + "'";
            }
            else
            {
                strTableName = "SELECT @table3 = 'ConverSationDetail_' + CONVERT(VARCHAR(6),GETDATE(),112)";
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@tablename", SqlDbType.NVarChar, 300),
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};
            parameters[0].Value = strTableName;
            parameters[1].Value = where;
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ConversationHistory_Select", parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
    }
}

