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
    /// 数据访问类WorkOrderTag。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-08-23 10:24:21 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class WorkOrderTag : DataBase
    {
        #region Instance
        public static readonly WorkOrderTag Instance = new WorkOrderTag();
        #endregion

        #region const
        private const string P_WORKORDERTAG_SELECT = "p_WorkOrderTag_Select";
        private const string P_WORKORDERTAG_INSERT = "p_WorkOrderTag_Insert";
        private const string P_WORKORDERTAG_UPDATE = "p_WorkOrderTag_Update";
        private const string P_WORKORDERTAG_DELETE = "p_WorkOrderTag_Delete";
        #endregion

        #region Contructor
        protected WorkOrderTag()
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
        public DataTable GetWorkOrderTag(QueryWorkOrderTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;
            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and recid=" + query.RecID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and bgid=" + query.BGID;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAG_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 取指定bugid下的标签
        /// </summary>
        /// <param name="bgid"></param>
        /// <param name="needIgnoreBgid">是否忽略bgid,取全部值</param>
        /// <returns></returns>
        public DataTable GetWorkOrderTagByBGID(int bgid, bool needIgnoreBgid = false)
        {
            var where = "";
            if (!needIgnoreBgid)
            {
                where += " and tag.bgid=" + bgid;
            }
            string strSql = @"	
	            SELECT 
	                b.Name AS GroupName,b.BGID,tag.TagName,tag.RecID AS TagID, tag.pid,
	                CASE tag.Status WHEN 0 THEN 'true' WHEN 1 THEN 'false' ELSE NULL END AS IsUsed ,tag.ordernum
	            FROM dbo.BusinessGroup b 
		            LEFT JOIN WorkOrderTag tag ON tag.BGID = b.BGID AND tag.Status=0 " + where + @"
		            LEFT JOIN WorkOrderTag p ON tag.pid = p.RecID
	            WHERE b.Status=0 ORDER BY b.Name,p.OrderNum,tag.OrderNum";
            /*
            string strSql = @"	
            select
	            b.Name AS GroupName,b.BGID,tag.TagName,tag.RecID AS TagID, tag.pid,
	            CASE tag.Status WHEN 0 THEN 'true' WHEN 1 THEN 'false' ELSE NULL END AS IsUsed ,tag.ordernum
            FROM WorkOrderTag tag  
	            JOIN dbo.BusinessGroup b ON tag.BGID=b.bgid
	            LEFT JOIN WorkOrderTag p ON tag.pid = p.RecID
	        WHERE   " + where + "	ORDER BY pid,p.OrderNum ";
            */

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds.Tables[0];
        }

        public DataTable GetSimulerWorkOrder(int userid, string strTagName)
        {
            if (userid == 0 || string.IsNullOrEmpty(strTagName)) return null;

            string strSql = @"	
                    SELECT  p.TagName+'--'+t.TagName AS TagName,t.RecID AS id FROM dbo.WorkOrderTag t 
                        JOIN dbo.WorkOrderTag p ON t.PID =p.RecID  JOIN
                    (
	                    SELECT bgid FROM dbo.UserGroupDataRigth WHERE UserID=" + userid.ToString() + @"
	                    UNION
	                    SELECT BGID FROM dbo.EmployeeAgent WHERE UserID=" + userid.ToString() + @"
                    ) a ON t.BGID = a.bgid where t.status=0 and t.TagName LIKE '%" + StringHelper.SqlFilter(strTagName.Trim()) + @"%'
                    ORDER BY t.OrderNum";

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.WorkOrderTag GetWorkOrderTag(int RecID)
        {
            QueryWorkOrderTag query = new QueryWorkOrderTag();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetWorkOrderTag(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleWorkOrderTag(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.WorkOrderTag LoadSingleWorkOrderTag(DataRow row)
        {
            Entities.WorkOrderTag model = new Entities.WorkOrderTag();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["PID"].ToString() != "")
            {
                model.PID = int.Parse(row["PID"].ToString());
            }
            if (row["OrderNum"].ToString() != "")
            {
                model.OrderNum = int.Parse(row["OrderNum"].ToString());
            }
            model.TagName = row["TagName"].ToString();
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
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.WorkOrderTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@TagName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
                    new SqlParameter("@PID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.TagName;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;
            parameters[8].Value = model.OrderNum;
            parameters[9].Value = model.PID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.WorkOrderTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@TagName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),					
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
                    new SqlParameter("@PID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.TagName;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;
            parameters[8].Value = model.OrderNum;
            parameters[9].Value = model.PID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERTAG_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.WorkOrderTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@TagName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),					
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4),
                    new SqlParameter("@PID", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.TagName;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.ModifyTime;
            parameters[5].Value = model.ModifyUserID;
            parameters[6].Value = model.OrderNum;
            parameters[7].Value = model.PID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAG_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.WorkOrderTag model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@TagName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@ModifyTime", SqlDbType.DateTime),
					new SqlParameter("@ModifyUserID", SqlDbType.Int,4),
                    new SqlParameter("@OrderNum", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.TagName;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ModifyTime;
            parameters[7].Value = model.ModifyUserID;
            parameters[8].Value = model.OrderNum;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERTAG_UPDATE, parameters);
        }

        public void ChangeOrder(int RecId, bool isUp, string strStatus)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@isUp", SqlDbType.Int,4),
                    new SqlParameter("@status", SqlDbType.NVarChar,50)};
            parameters[0].Value = RecId;
            parameters[1].Value = isUp ? 1 : 0;
            parameters[2].Value = strStatus;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "ChangeWorkTagSortNum", parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_WORKORDERTAG_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_WORKORDERTAG_DELETE, parameters);
        }
        #endregion

        #region DeleteMany
        /// <summary>
        /// 删除多条数据
        /// </summary>
        public int DeleteMany(string RecIDS)
        {
            string sql = "";
            sql = "DELETE FROM WorkOrderTag WHERE RecID IN @RecIDS";
            sql = sql.Replace("@RecIDS",RecIDS);

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        #endregion

        #region SelectByOrderID
        /// <summary>
        /// 根据工单号查询用到的标签
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderTagByOrderID(string orderid)
        {
            string sql = "";
            sql = "SELECT wot.RecID AS TagID,wot.TagName AS TagName FROM WorkOrderTag wot " +
                  "LEFT JOIN WorkOrderTagMapping wotm ON wot.RecID = wotm.TagID " +
                  "LEFT JOIN WorkOrderInfo worder ON worder.OrderID = wotm.OrderID " +
                  "WHERE worder.OrderID='" + StringHelper.SqlFilter(orderid) + "'";

            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return ds.Tables[0];
        }

        #endregion

        /// <summary>
        /// 根据TagID判断标签是否被使用
        /// </summary>
        /// <param name="TagID"></param>
        /// <returns></returns>
        public bool isUsedTagByTagID(int TagID)
        {
            string sqlStr = "SELECT COUNT(*) FROM WorkOrderInfo woi " +
                            "LEFT JOIN WorkOrderTagMapping wotm ON wotm.OrderID = woi.OrderID " +
                            "LEFT JOIN WorkOrderTag wot ON wot.RecID = wotm.TagID " +
                            "WHERE wot.RecID=@TagID";

            SqlParameter[] parameters = {
					new SqlParameter("@TagID", TagID)
					};

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    return true;
                }
            }

            return false;
        }

    }
}

