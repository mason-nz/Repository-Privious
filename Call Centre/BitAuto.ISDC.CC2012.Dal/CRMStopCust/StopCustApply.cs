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
    /// 数据访问类StopCustApply。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class StopCustApply : DataBase
    {
        #region Instance
        public static readonly StopCustApply Instance = new StopCustApply();
        #endregion

        #region const
        private const string P_STOPCUSTAPPLY_SELECT = "p_StopCustApply_Select";
        private const string P_STOPCUSTAPPLY_INSERT = "p_StopCustApply_Insert";
        private const string P_STOPCUSTAPPLY_UPDATE = "p_StopCustApply_Update";
        private const string P_STOPCUSTAPPLY_DELETE = "p_StopCustApply_Delete";
        #endregion

        #region Contructor
        protected StopCustApply()
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
        public DataTable GetStopCustApply(QueryStopCustApply query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("oct", "BGID", "AssignUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and sca.RecID=" + query.RecID;
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.CustID like '%" + StringHelper.SqlFilter(query.CustID) + "%'";
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ci.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.ApplerName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ui.TrueName like '%" + StringHelper.SqlFilter(query.ApplerName) + "%'";
            }
            if (query.AreaID != Constant.INT_INVALID_VALUE && query.AreaID > 0)
            {
                where += " and sca.AreaID=" + query.AreaID;
            }
            if (query.AreaName != Constant.STRING_INVALID_VALUE && query.AreaName != "-1")
            {
                where += " and sca.AreaName='" + StringHelper.SqlFilter(query.AreaName) + "'";
            }
            //分配人
            if (query.OperID != Constant.INT_INVALID_VALUE && query.OperID != -1)
            {
                where += " and oct.AssignUserID = " + query.OperID;
            }
            //客户状态
            if (query.StopStatus > 0)
            {
                where += " and sca.StopStatus=" + query.StopStatus;
            }
            //任务状态
            if (query.TaskStatus > 0)
            {
                where += " and oct.TaskStatus=" + query.TaskStatus;
            }
            if (query.ApplyBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.ApplyTime >='" + StringHelper.SqlFilter(query.ApplyBeginTime) + " 0:0:0'";
            }
            if (query.ApplyEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.ApplyTime <='" + StringHelper.SqlFilter(query.ApplyEndTime) + " 23:59:59'";
            }
            if (query.SubmitBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.AuditTime >='" + StringHelper.SqlFilter(query.SubmitBeginTime) + " 0:0:0'";
            }
            if (query.SubmitEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.AuditTime <='" + StringHelper.SqlFilter(query.SubmitEndTime) + " 23:59:59'";
            }
            if (query.StopBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.StopTime >='" + StringHelper.SqlFilter(query.StopBeginTime) + " 0:0:0'";
            }
            if (query.StopEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.StopTime <='" + query.StopEndTime + " 23:59:59'";
            }
            if (query.Reason != Constant.STRING_INVALID_VALUE && query.Reason != "-1")
            {
                int _reason = 0;
                if (int.TryParse(query.Reason, out _reason))
                {
                    where += " and sca.ApplyReason =" + _reason;
                }

            }
            if (query.Remark != Constant.STRING_INVALID_VALUE && query.Remark != "-1")
            {
                int _remark = 0;
                if (int.TryParse(query.Remark, out _remark))
                {
                    where += " and sca.ApplyRemark =" + _remark;
                }
            }
            if (query.CarType != Constant.STRING_INVALID_VALUE)
            {
                where += " and ci.CarType IN (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }

            if (query.ApplyType != Constant.INT_INVALID_VALUE)
            {
                where += " and sca.ApplyType=" + query.ApplyType;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_STOPCUSTAPPLY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// <summary>
        /// 无数据权限查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetStopCustApplyNoDR(QueryStopCustApply query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and sca.RecID=" + query.RecID;
            }
            if (query.CustID != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.CustID like '%" + StringHelper.SqlFilter(query.CustID) + "%'";
            }
            if (query.CustName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ci.CustName like '%" + StringHelper.SqlFilter(query.CustName) + "%'";
            }
            if (query.ApplerName != Constant.STRING_INVALID_VALUE)
            {
                where += " and ui.TrueName like '%" + StringHelper.SqlFilter(query.ApplerName) + "%'";
            }
            if (query.AreaID != Constant.INT_INVALID_VALUE && query.AreaID > 0)
            {
                where += " and sca.AreaID=" + query.AreaID;
            }
            if (query.AreaName != Constant.STRING_INVALID_VALUE && query.AreaName != "-1")
            {
                where += " and sca.AreaName='" + StringHelper.SqlFilter(query.AreaName) + "'";
            }
            //分配人
            if (query.OperID != Constant.INT_INVALID_VALUE && query.OperID != -1)
            {
                where += " and oct.AssignUserID = " + query.OperID;
            }
            if (query.StopStatus != Constant.INT_INVALID_VALUE && query.StopStatus != -1)
            {
                if (query.StopStatus == 0)
                {
                    where += " and oct.TaskStatus=1 ";
                }
                else if (query.StopStatus == 1)
                {
                    where += " and oct.TaskStatus=2";
                }
                else
                {
                    where += " and sca.StopStatus=" + query.StopStatus;
                }
            }
            if (query.TaskStatus != Constant.INT_INVALID_VALUE && query.TaskStatus != -1)
            {
                if (query.TaskStatus <= 3)
                {
                    where += " and oct.TaskStatus=" + query.TaskStatus;
                }
                else
                {
                    int stopStatus = (query.TaskStatus == 4 || query.TaskStatus == 7) ? 2 : (query.TaskStatus == 5 || query.TaskStatus == 8) ? 3 : 4;
                    where += " and sca.StopStatus=" + stopStatus;
                }
            }
            if (query.ApplyBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.ApplyTime >='" + StringHelper.SqlFilter(query.ApplyBeginTime) + " 0:0:0'";
            }
            if (query.ApplyEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.ApplyTime <='" + StringHelper.SqlFilter(query.ApplyEndTime) + " 23:59:59'";
            }
            if (query.SubmitBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.AuditTime >='" + StringHelper.SqlFilter(query.SubmitBeginTime) + " 0:0:0'";
            }
            if (query.SubmitEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.AuditTime <='" + StringHelper.SqlFilter(query.SubmitEndTime) + " 23:59:59'";
            }
            if (query.StopBeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.StopTime >='" + StringHelper.SqlFilter(query.StopBeginTime) + " 0:0:0'";
            }
            if (query.StopEndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and sca.StopTime <='" + StringHelper.SqlFilter(query.StopEndTime) + " 23:59:59'";
            }
            if (query.Reason != Constant.STRING_INVALID_VALUE && query.Reason != "-1")
            {
                int _reason = 0;
                if (int.TryParse(query.Reason, out _reason))
                {
                    where += " and sca.ApplyReason =" + _reason;
                }

            }
            if (query.Remark != Constant.STRING_INVALID_VALUE && query.Remark != "-1")
            {
                //where += " and tlog.Remark like '%申请说明：" + query.Remark + "%'";

                int _remark = 0;
                if (int.TryParse(query.Remark, out _remark))
                {
                    where += " and sca.ApplyRemark =" + _remark;
                }

            }
            if (query.CarType != Constant.STRING_INVALID_VALUE)
            {
                where += " and ci.CarType IN (" + Dal.Util.SqlFilterByInCondition(query.CarType) + ")";
            }

            if (query.ApplyType != Constant.INT_INVALID_VALUE)
            {
                where += " and sca.ApplyType=" + query.ApplyType;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_STOPCUSTAPPLY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.StopCustApply GetStopCustApply(int RecID)
        {
            QueryStopCustApply query = new QueryStopCustApply();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetStopCustApply(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleStopCustApply(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.StopCustApply LoadSingleStopCustApply(DataRow row)
        {
            Entities.StopCustApply model = new Entities.StopCustApply();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["CRMStopCustApplyID"].ToString() != "")
            {
                model.CRMStopCustApplyID = int.Parse(row["CRMStopCustApplyID"].ToString());
            }
            model.CustID = row["CustID"].ToString();
            if (row["ApplyerID"].ToString() != "")
            {
                model.ApplyerID = int.Parse(row["ApplyerID"].ToString());
            }
            model.AreaName = row["AreaName"].ToString();
            if (row["AreaID"].ToString() != "")
            {
                model.AreaID = int.Parse(row["AreaID"].ToString());
            }
            if (row["ApplyTime"].ToString() != "")
            {
                model.ApplyTime = DateTime.Parse(row["ApplyTime"].ToString());
            }
            if (row["AuditTime"].ToString() != "")
            {
                model.AuditTime = DateTime.Parse(row["AuditTime"].ToString());
            }
            if (row["StopTime"].ToString() != "")
            {
                model.StopTime = DateTime.Parse(row["StopTime"].ToString());
            }
            if (row["StopStatus"].ToString() != "")
            {
                model.StopStatus = int.Parse(row["StopStatus"].ToString());
            }

            model.RejectReason = row["RejectReason"].ToString();
            model.AuditOpinion = row["AuditOpinion"].ToString();
            model.Remark = row["Remark"].ToString();
            model.DeleteMemberID = row["DeleteMemberID"].ToString();

            if (row["ApplyType"].ToString() != "")
            {
                model.ApplyType = int.Parse(row["ApplyType"].ToString());
            }

            if (row["ApplyReason"].ToString() != "")
            {
                model.ApplyReason = int.Parse(row["ApplyReason"].ToString());
            }

            if (row["ApplyRemark"].ToString() != "")
            {
                model.ApplyRemark = int.Parse(row["ApplyRemark"].ToString());
            }


            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.StopCustApply model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CRMStopCustApplyID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@ApplyerID", SqlDbType.Int,4),
					new SqlParameter("@AreaName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@ApplyTime", SqlDbType.DateTime),
					new SqlParameter("@AuditTime", SqlDbType.DateTime),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@StopStatus", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,2000),
					new SqlParameter("@AuditOpinion", SqlDbType.NVarChar,2000),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@DeleteMemberID", SqlDbType.VarChar,2000),
                                        new SqlParameter("@ApplyType", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyReason", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyRemark", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CRMStopCustApplyID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.ApplyerID;
            parameters[4].Value = model.AreaName;
            parameters[5].Value = model.AreaID;
            parameters[6].Value = model.ApplyTime;
            parameters[7].Value = model.AuditTime;
            parameters[8].Value = model.StopTime;
            parameters[9].Value = model.StopStatus;
            parameters[10].Value = model.RejectReason;
            parameters[11].Value = model.AuditOpinion;
            parameters[12].Value = model.Remark;
            parameters[13].Value = model.DeleteMemberID;

            parameters[14].Value = model.ApplyType;
            parameters[15].Value = model.ApplyReason;
            parameters[16].Value = model.ApplyRemark;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_STOPCUSTAPPLY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.StopCustApply model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CRMStopCustApplyID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@ApplyerID", SqlDbType.Int,4),
					new SqlParameter("@AreaName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@ApplyTime", SqlDbType.DateTime),
					new SqlParameter("@AuditTime", SqlDbType.DateTime),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@StopStatus", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,2000),
					new SqlParameter("@AuditOpinion", SqlDbType.NVarChar,2000),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
                    new SqlParameter("@DeleteMemberID", SqlDbType.VarChar,2000),
                                        new SqlParameter("@ApplyType", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyReason", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyRemark", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.CRMStopCustApplyID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.ApplyerID;
            parameters[4].Value = model.AreaName;
            parameters[5].Value = model.AreaID;
            parameters[6].Value = model.ApplyTime;
            parameters[7].Value = model.AuditTime;
            parameters[8].Value = model.StopTime;
            parameters[9].Value = model.StopStatus;
            parameters[10].Value = model.RejectReason;
            parameters[11].Value = model.AuditOpinion;
            parameters[12].Value = model.Remark;
            parameters[13].Value = model.DeleteMemberID;
            parameters[14].Value = model.ApplyType;
            parameters[15].Value = model.ApplyReason;
            parameters[16].Value = model.ApplyRemark;
            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_STOPCUSTAPPLY_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.StopCustApply model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CRMStopCustApplyID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@ApplyerID", SqlDbType.Int,4),
					new SqlParameter("@AreaName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@ApplyTime", SqlDbType.DateTime),
					new SqlParameter("@AuditTime", SqlDbType.DateTime),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@StopStatus", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,2000),
					new SqlParameter("@AuditOpinion", SqlDbType.NVarChar,2000),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@DeleteMemberID", SqlDbType.VarChar,2000),
                    new SqlParameter("@ApplyType", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyReason", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyRemark", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CRMStopCustApplyID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.ApplyerID;
            parameters[4].Value = model.AreaName;
            parameters[5].Value = model.AreaID;
            parameters[6].Value = model.ApplyTime;
            parameters[7].Value = model.AuditTime;
            parameters[8].Value = model.StopTime;
            parameters[9].Value = model.StopStatus;
            parameters[10].Value = model.RejectReason;
            parameters[11].Value = model.AuditOpinion;
            parameters[12].Value = model.Remark;
            parameters[13].Value = model.DeleteMemberID;
            parameters[14].Value = model.ApplyType;
            parameters[15].Value = model.ApplyReason;
            parameters[16].Value = model.ApplyRemark;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_STOPCUSTAPPLY_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.StopCustApply model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@CRMStopCustApplyID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,50),
					new SqlParameter("@ApplyerID", SqlDbType.Int,4),
					new SqlParameter("@AreaName", SqlDbType.NVarChar,100),
					new SqlParameter("@AreaID", SqlDbType.Int,4),
					new SqlParameter("@ApplyTime", SqlDbType.DateTime),
					new SqlParameter("@AuditTime", SqlDbType.DateTime),
					new SqlParameter("@StopTime", SqlDbType.DateTime),
					new SqlParameter("@StopStatus", SqlDbType.Int,4),
					new SqlParameter("@RejectReason", SqlDbType.NVarChar,2000),
					new SqlParameter("@AuditOpinion", SqlDbType.NVarChar,2000),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
                    new SqlParameter("@DeleteMemberID", SqlDbType.VarChar,2000),
                    new SqlParameter("@ApplyType", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyReason", SqlDbType.Int,4),
                                        new SqlParameter("@ApplyRemark", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.CRMStopCustApplyID;
            parameters[2].Value = model.CustID;
            parameters[3].Value = model.ApplyerID;
            parameters[4].Value = model.AreaName;
            parameters[5].Value = model.AreaID;
            parameters[6].Value = model.ApplyTime;
            parameters[7].Value = model.AuditTime;
            parameters[8].Value = model.StopTime;
            parameters[9].Value = model.StopStatus;
            parameters[10].Value = model.RejectReason;
            parameters[11].Value = model.AuditOpinion;
            parameters[12].Value = model.Remark;
            parameters[13].Value = model.DeleteMemberID;
            parameters[14].Value = model.ApplyType;
            parameters[15].Value = model.ApplyReason;
            parameters[16].Value = model.ApplyRemark;
            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_STOPCUSTAPPLY_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_STOPCUSTAPPLY_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_STOPCUSTAPPLY_DELETE, parameters);
        }
        #endregion

        public Entities.StopCustApply GetStopCustApplyByCrmStopCustApplyID(int CRMStopCustApplyID)
        {
            Entities.StopCustApply model = null;
            string sqlStr = "select * from StopCustApply where CRMStopCustApplyID=" + CRMStopCustApplyID;

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                model = LoadSingleStopCustApply(dt.Rows[0]);
            }

            return model;
        }

    }
}

