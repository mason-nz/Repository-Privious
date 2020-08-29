using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类SMSSendHistory。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-12-23 06:16:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SMSSendHistory : DataBase
    {
        #region Instance
        public static readonly SMSSendHistory Instance = new SMSSendHistory();
        #endregion

        #region const
        private const string P_SMSSENDHISTORY_SELECT = "p_SMSSendHistory_Select";
        private const string P_SMSSENDHISTORY_INSERT = "p_SMSSendHistory_Insert";
        private const string P_SMSSENDHISTORY_SELECTForEXPORT = "p_SMSSendHistory_SelectForExport";
        private const string P_SMSHISTROYSTATISTICS_SELECT = "p_SMSHistroyStatistics_Select";
        #endregion

        #region Contructor
        protected SMSSendHistory()
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
        public DataTable GetSMSSendHistory(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID.HasValue && query.LoginID.Value > 0)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", (Int32)query.LoginID);
                where += whereDataRight;
            }
            #endregion

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.RecID=" + query.RecID;
            }
            if (query.CreateTimeBegin != Constant.DATE_INVALID_VALUE && query.CreateTimeEnd != Constant.DATE_INVALID_VALUE)
            {
                where += " and (a.CreateTime>='" + query.CreateTimeBegin + "' and a.CreateTime<='" + query.CreateTimeEnd + "')";
            }
            if (query.Phone != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.phone='" + StringHelper.SqlFilter(query.Phone) + "'";
            }
            if (!string.IsNullOrEmpty(query.PhoneList))
            {
                string[] array = query.PhoneList.Split(',');
                string p = "'" + string.Join("','", array) + "'";
                where += " and a.phone in (" + p + ")";
            }
            if (query.Reservicer != Constant.STRING_INVALID_VALUE)
            {
                where += " and b.custname like '%" + StringHelper.SqlFilter(query.Reservicer) + "%'";
            }
            if (query.Content != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.Content like '%" + StringHelper.SqlFilter(query.Content) + "%'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.CreateUserID=" + query.CreateUserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.BGID=" + query.BGID;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and a.Status=" + query.Status;
            }
            //根据客户查询短信有问题 强斐 2016-8-10
            if (!string.IsNullOrEmpty(query.CBID))
            {
                where += " and a.CustID='" + SqlFilter(query.CBID) + "'";
            }
            if (!string.IsNullOrEmpty(query.CRMCustID))
            {
                where += " and a.CRMCustID='" + SqlFilter(query.CRMCustID) + "'";
            }
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),//edit by wangtonghai 2016/5/3 将@where参数大小从40000 变更成4000
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSSENDHISTORY_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSMSHistroyStatistics(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", (Int32)query.LoginID);

                where += whereDataRight;
            }
            #endregion
            if (query.CreateTimeBegin != Constant.DATE_INVALID_VALUE && query.CreateTimeEnd != Constant.DATE_INVALID_VALUE)
            {
                where += " and (a.CreateTime>='" + query.CreateTimeBegin + "' and a.CreateTime<='" + query.CreateTimeEnd + "')";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.CreateUserID=" + query.CreateUserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.BGID=" + query.BGID;
            }
            if (query.AgentNum != Constant.STRING_INVALID_VALUE)
            {
                where += " and c.agentnum='" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }


            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar,  4000),//edit by wangtonghai 2016/5/3 将@where参数大小从40000 变更成4000
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSHISTROYSTATISTICS_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }



        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetSMSSendHistoryForExport(QuerySMSSendHistory query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", (Int32)query.LoginID);

                where += whereDataRight;
            }
            #endregion

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.RecID=" + query.RecID;
            }
            if (query.CreateTimeBegin != Constant.DATE_INVALID_VALUE && query.CreateTimeEnd != Constant.DATE_INVALID_VALUE)
            {
                where += " and (a.CreateTime>='" + query.CreateTimeBegin + "' and a.CreateTime<='" + query.CreateTimeEnd + "')";
            }
            if (query.Phone != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.phone='" + StringHelper.SqlFilter(query.Phone) + "'";
            }
            //if (query.Reservicer != Constant.STRING_INVALID_VALUE)
            //{
            //    where += " and b.custname like '%" + StringHelper.SqlFilter(query.Reservicer) + "%'";
            //}
            if (query.Content != Constant.STRING_INVALID_VALUE)
            {
                where += " and a.Content like '%" + StringHelper.SqlFilter(query.Content) + "%'";
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.CreateUserID=" + query.CreateUserID;
            }
            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " and a.BGID=" + query.BGID;
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and a.Status=" + query.Status;
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),//edit by wangtonghai 2016/5/3 将@where参数大小从40000 变更成4000
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSSENDHISTORY_SELECTForEXPORT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SMSSendHistory GetSMSSendHistory()
        {
            QuerySMSSendHistory query = new QuerySMSSendHistory();
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSMSSendHistory(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSMSSendHistory(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SMSSendHistory LoadSingleSMSSendHistory(DataRow row)
        {
            Entities.SMSSendHistory model = new Entities.SMSSendHistory();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["TemplateID"].ToString() != "")
            {
                model.TemplateID = int.Parse(row["TemplateID"].ToString());
            }
            model.Phone = row["Phone"].ToString();
            model.Content = row["Content"].ToString();
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
            if (row["CustID"].ToString() != "")
            {
                model.CustID = row["CustID"].ToString();
            }
            model.CRMCustID = row["CRMCustID"].ToString();
            if (row["TaskType"].ToString() != "")
            {
                model.TaskType = int.Parse(row["TaskType"].ToString());
            }
            model.TaskID = row["TaskID"].ToString();
            return model;
        }
        #endregion

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public void Insert(Entities.SMSSendHistory model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@TemplateID", SqlDbType.Int,4),
					new SqlParameter("@Phone", SqlDbType.VarChar,23),
					new SqlParameter("@Content", SqlDbType.VarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@CustID", SqlDbType.VarChar,20),
					new SqlParameter("@CRMCustID", SqlDbType.VarChar,20),
					new SqlParameter("@TaskType", SqlDbType.Int,4),
					new SqlParameter("@TaskID", SqlDbType.VarChar,20)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.BGID;
            parameters[2].Value = model.TemplateID;
            parameters[3].Value = model.Phone;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.CreateTime;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CustID;
            parameters[9].Value = model.CRMCustID;
            parameters[10].Value = model.TaskType;
            parameters[11].Value = model.TaskID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SMSSENDHISTORY_INSERT, parameters);
            model.RecID = (int)parameters[0].Value;
        }

        public int AddCustIdInfo(string SentSMSHistoryTel, string AddedCustID)
        {
            string sqlStr = "UPDATE SMSSendHistory  SET CustID=@CustID WHERE  Phone = @Phone AND (CustID ='' OR CustID ='新增工单')  AND CRMCustID = ''";
            SqlParameter[] parameters = {
					new SqlParameter("@CustID", AddedCustID),
					new SqlParameter("@Phone", SentSMSHistoryTel)
					};
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
        }

        /// 绑定短信记录和任务id
        /// <summary>
        /// 绑定短信记录和任务id
        /// </summary>
        /// <param name="recids"></param>
        /// <param name="taskid"></param>
        public void BindSMSSendHistoryForTask(string recids, string taskid)
        {
            string sql = @"UPDATE SMSSendHistory 
                                SET TaskID='" + SqlFilter(taskid) + @"',TaskType=0
                                WHERE RecID IN (" + Dal.Util.SqlFilterByInCondition(recids) + ")";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}

