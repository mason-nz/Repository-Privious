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
    /// 数据访问类GroupOrderTask。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-11-04 09:34:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class GroupOrderTask : DataBase
    {
        #region Instance
        public static readonly GroupOrderTask Instance = new GroupOrderTask();
        #endregion

        #region const
        private const string P_GROUPORDERTASK_SELECT = "p_GroupOrderTask_Select";
        private const string P_GROUPORDERTASK_INSERT = "p_GroupOrderTask_Insert";
        private const string P_GROUPORDERTASK_UPDATE = "p_GroupOrderTask_Update";
        private const string P_GROUPORDERTASK_DELETE = "p_GroupOrderTask_Delete";
        #endregion

        #region Contructor
        protected GroupOrderTask()
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
        public DataTable GetGroupOrderTask(QueryGroupOrderTask query, string order, int currentPage, int pageSize, out int totalCount)
        {

            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("gtask", "BGID", "AssignUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            #region 条件
            if (query.CustomerTel != Constant.STRING_INVALID_VALUE)
            {
                where += " AND gorder.CustomerTel='" + StringHelper.SqlFilter(query.CustomerTel.ToString()) + "'";
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND gorder.CustomerName like '%" + StringHelper.SqlFilter(query.CustName.ToString()) + "%'";
            }
            if (query.OrderCode != Constant.STRING_INVALID_VALUE)
            {
                where += " AND gorder.OrderCode='" + StringHelper.SqlFilter(query.OrderCode) + "'";
            }
            if (query.OrderID != Constant.INT_INVALID_VALUE)
            {
                where += " AND gtask.OrderID=" + query.OrderID.ToString();
            }
            if (query.TaskID != Constant.INT_INVALID_VALUE)
            {
                where += " AND gtask.TaskID=" + query.TaskID.ToString();
            }
            if (query.ProvinceID != Constant.INT_INVALID_VALUE && query.ProvinceID != -1)
            {
                where += " AND gorder.ProvinceID=" + query.ProvinceID.ToString();
            }
            if (query.CityID != Constant.INT_INVALID_VALUE && query.CityID != -1)
            {
                where += " AND gorder.CityID=" + query.CityID.ToString();
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND gtask.AssignUserID=" + query.AssignUserID.ToString();
            }
            if (query.Dealer != Constant.STRING_INVALID_VALUE)
            {
                where += " AND gorder.DealerName like '%" + StringHelper.SqlFilter(query.Dealer.ToString()) + "%'";
            }
            if (query.TaskStatus != Constant.INT_INVALID_VALUE)
            {
                where += " AND gtask.TaskStatus=" + query.TaskStatus.ToString();
            }
            if (query.IsReturnVisit != Constant.INT_INVALID_VALUE)
            {
                where += " AND gorder.IsReturnVisit=" + query.IsReturnVisit.ToString();
            }

            //团购订单下单时间
            if (query.CreatetimeBegin != Constant.DATE_INVALID_VALUE && query.CreatetimeEnd != Constant.DATE_INVALID_VALUE)
            {
                where += " and gorder.OrderCreateTime >= '" + query.CreatetimeBegin + "' and gorder.OrderCreateTime <= '" + query.CreatetimeEnd + "'";
            }
            if (query.SubmitTimeBegin != Constant.DATE_INVALID_VALUE && query.SubmitTimeEnd != Constant.DATE_INVALID_VALUE)
            {
                //where += " and gtask.SubmitTime >= '" + query.SubmitTimeBegin + "' and gtask.SubmitTime <= '" + query.SubmitTimeEnd + "'";
                where += " and gtask.SubmitTime >= '" + Convert.ToDateTime(query.SubmitTimeBegin).ToShortDateString() + " 0:00:000' and gtask.SubmitTime <= '" + Convert.ToDateTime(query.SubmitTimeEnd).ToShortDateString() + " 23:59:59'";
            }

            if (query.CarMasterID != Constant.INT_INVALID_VALUE && query.CarMasterID != 0)
            {
                where += " and gorder.CarMasterID=" + query.CarMasterID;
            }
            if (query.CarSerialID != Constant.INT_INVALID_VALUE && query.CarSerialID != 0)
            {
                where += " and gorder.CarSerialID=" + query.CarSerialID;
            }
            if (query.CarID != Constant.INT_INVALID_VALUE && query.CarID != 0)
            {
                where += " and gorder.CarID=" + query.CarID;
            }
            if (query.FailReason != Constant.INT_INVALID_VALUE)
            {
                where += " and gorder.FailReasonID=" + query.FailReason;
            }
            if (query.TelCount != Constant.STRING_INVALID_VALUE && query.TelCount != "-1")//添加"用户下单数"查询条件 Add=masj,Date=2013-11-13
            {
                if (query.TelCount.ToLower().Trim() == "1")//1次
                {
                    where += @" and gorder.OrderID IN (
			                SELECT OrderID FROM (
			                SELECT gorder.CustomerTel,MIN(gorder.OrderID) AS OrderID,COUNT(*) AS OrderCount
			                FROM    dbo.GroupOrderTask(NOLOCK) gtask 
			                LEFT JOIN dbo.GroupOrder(NOLOCK) gorder ON gtask.OrderID = gorder.OrderID 
			                WHERE 1=1 " + where + @"
			                GROUP BY gorder.CustomerTel
			                HAVING COUNT(*)=1
			                ) AS b
                        )";
                }
                else if (query.TelCount.ToLower().Trim() == "n")
                {
                    where += @" and gorder.OrderID IN (SELECT DISTINCT gorder.OrderID
			                   FROM    dbo.GroupOrderTask(NOLOCK) gtask 
			                   LEFT JOIN dbo.GroupOrder(NOLOCK) gorder ON gtask.OrderID = gorder.OrderID 
			                   WHERE 1=1   " + where + ")" + @" and gorder.OrderID Not IN (
			                SELECT OrderID FROM (
			                SELECT gorder.CustomerTel,MIN(gorder.OrderID) AS OrderID,COUNT(*) AS OrderCount
			                FROM    dbo.GroupOrderTask(NOLOCK) gtask 
			                LEFT JOIN dbo.GroupOrder(NOLOCK) gorder ON gtask.OrderID = gorder.OrderID 
			                WHERE 1=1 " + where + @"
			                GROUP BY gorder.CustomerTel
			                HAVING COUNT(*)=1
			                ) AS b
                        )";
                }
                //where += " and gorder.FailReasonID=" + query.FailReason;
            }
            #endregion

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERTASK_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.GroupOrderTask GetGroupOrderTask(long TaskID)
        {
            QueryGroupOrderTask query = new QueryGroupOrderTask();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrderTask(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleGroupOrderTask(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.GroupOrderTask LoadSingleGroupOrderTask(DataRow row)
        {
            Entities.GroupOrderTask model = new Entities.GroupOrderTask();

            if (row["TaskID"].ToString() != "")
            {
                model.TaskID = long.Parse(row["TaskID"].ToString());
            }
            if (row["TaskStatus"].ToString() != "")
            {
                model.TaskStatus = int.Parse(row["TaskStatus"].ToString());
            }
            if (row["OrderID"].ToString() != "")
            {
                model.OrderID = int.Parse(row["OrderID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            if (row["AssignUserID"].ToString() != "")
            {
                model.AssignUserID = int.Parse(row["AssignUserID"].ToString());
            }
            if (row["AssignTime"].ToString() != "")
            {
                model.AssignTime = DateTime.Parse(row["AssignTime"].ToString());
            }
            if (row["SubmitTime"].ToString() != "")
            {
                model.SubmitTime = DateTime.Parse(row["SubmitTime"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["LastUpdateTime"].ToString() != "")
            {
                model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
            }
            if (row["LastUpdateUserID"].ToString() != "")
            {
                model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.GroupOrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskStatus;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.SCID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.SubmitTime;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.LastUpdateTime;
            parameters[11].Value = model.LastUpdateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERTASK_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TaskStatus;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.SCID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.SubmitTime;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.LastUpdateTime;
            parameters[11].Value = model.LastUpdateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDERTASK_INSERT, parameters);
            return (long)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.GroupOrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.TaskStatus;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.SCID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.SubmitTime;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.LastUpdateTime;
            parameters[11].Value = model.LastUpdateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERTASK_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrderTask model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt,8),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@OrderID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@AssignUserID", SqlDbType.Int,4),
					new SqlParameter("@AssignTime", SqlDbType.DateTime),
					new SqlParameter("@SubmitTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@LastUpdateTime", SqlDbType.DateTime),
					new SqlParameter("@LastUpdateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.TaskID;
            parameters[1].Value = model.TaskStatus;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.BGID;
            parameters[4].Value = model.SCID;
            parameters[5].Value = model.AssignUserID;
            parameters[6].Value = model.AssignTime;
            parameters[7].Value = model.SubmitTime;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.LastUpdateTime;
            parameters[11].Value = model.LastUpdateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDERTASK_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_GROUPORDERTASK_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.BigInt)};
            parameters[0].Value = TaskID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_GROUPORDERTASK_DELETE, parameters);
        }
        #endregion

        /// <summary>
        /// 取任务不同处理人
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiffassignuserid()
        {
            DataSet ds = null;
            string sqlstr = "select distinct assignuserid from GroupOrderTask WITH (NOLOCK)";
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            return ds.Tables[0];
        }
    }
}

