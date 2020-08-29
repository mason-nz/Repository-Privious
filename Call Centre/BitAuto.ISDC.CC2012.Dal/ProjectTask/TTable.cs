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
    /// 数据访问类TTable。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-03-20 03:24:42 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TTable : DataBase
    {
        #region Instance
        public static readonly TTable Instance = new TTable();
        #endregion

        #region const
        private const string P_TTABLE_SELECT = "p_TTable_Select";
        private const string P_TTABLE_INSERT = "p_TTable_Insert";
        private const string P_TTABLE_UPDATE = "p_TTable_Update";
        private const string P_TTABLE_DELETE = "p_TTable_Delete";
        #endregion

        #region Contructor
        protected TTable()
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
        public DataTable GetTTable(QueryTTable query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and RecID=" + query.RecID + "";
            }

            if (query.TTCode != Constant.STRING_INVALID_VALUE)
            {
                where += " and TTCode='" + StringHelper.SqlFilter(query.TTCode) + "'";
            }

            if (query.TTDesName != Constant.STRING_INVALID_VALUE)
            {
                where += " and TTDesName like '%" + StringHelper.SqlFilter(query.TTDesName) + "%'";
            }

            if (query.TTIsData != Constant.INT_INVALID_VALUE)
            {
                where += " and TTisData =" + query.TTIsData;
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTABLE_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.TTable GetTTable(int RecID)
        {
            QueryTTable query = new QueryTTable();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTTable(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleTTable(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.TTable LoadSingleTTable(DataRow row)
        {
            Entities.TTable model = new Entities.TTable();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = int.Parse(row["RecID"].ToString());
            }
            model.TTCode = row["TTCode"].ToString();
            model.TTDesName = row["TTDesName"].ToString();
            model.TTName = row["TTName"].ToString();
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
            if (row["TTIsData"].ToString() != "")
            {
                model.TTIsData = int.Parse(row["TTIsData"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.TTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTIsData", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TTCode;
            parameters[2].Value = model.TTDesName;
            parameters[3].Value = model.TTName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.TTIsData;


            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTABLE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTIsData", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.TTCode;
            parameters[2].Value = model.TTDesName;
            parameters[3].Value = model.TTName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.TTIsData;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TTABLE_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.TTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTIsData", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TTCode;
            parameters[2].Value = model.TTDesName;
            parameters[3].Value = model.TTName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.TTIsData;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTABLE_UPDATE, parameters);
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TTable model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
					new SqlParameter("@TTCode", SqlDbType.NVarChar,20),
					new SqlParameter("@TTDesName", SqlDbType.NVarChar,40),
					new SqlParameter("@TTName", SqlDbType.NVarChar,40),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TTIsData", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.TTCode;
            parameters[2].Value = model.TTDesName;
            parameters[3].Value = model.TTName;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.TTIsData;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TTABLE_UPDATE, parameters);
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

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_TTABLE_DELETE, parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_TTABLE_DELETE, parameters);
        }
        #endregion


        public int GetMaxID()
        {
            int maxid = 0;
            string sqlStr = "SELECT MAX( RecID) maxid FROM TTable ";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (obj != null && obj.ToString() != "")
            {
                maxid = int.Parse(obj.ToString());
            }

            return maxid;
        }



        public void CreateTable(string sqlStr, out string msg)
        {
            msg = "";
            try
            {
                SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, sqlStr);
            }
            catch (Exception ex)
            {

                msg = ex.Message.ToString();
            }
        }


        /// <summary>
        /// 获取生成的自定义表的最大RecId
        /// </summary>
        /// <param name="TTName"></param>
        /// <returns></returns>
        public int GetMaxRecIdByTTName(string TTName)
        {

            int recid = 0;
            string sqlStr = "select max(recid) from " + TTName;

            Object o = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (o != null && o.ToString() != "")
            {
                recid = int.Parse(o.ToString());
            }
            return recid;
        }


        /// <summary>
        /// 根据TTCode得到自定义的表的数据信息
        /// </summary>
        /// <param name="TTCode">TTCode</param>
        /// <returns></returns>
        public DataTable GetTemptInfoByTTCode(string TTCode)
        {
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@TTCode", SqlDbType.VarChar,20) 
            };
            parameters[0].Value = TTCode;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_TTable_GetTemptInfoByTTCode", null);
            dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// 根据IDs获取自定义表中的
        /// </summary>
        /// <param name="selectDataIDs"></param>
        /// <param name="TTName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DataTable GetDataByIDs(string selectDataIDs, string TTName, string ttcode,string projectid, out string msg)
        {
            msg = "";
            DataSet ds;
             
          //  string sqlStr = "select * from " + TTName + " where RecID in (" + selectDataIDs + ")";
            string sqlStr = @"SELECT template.*,task.TaskStatus,LastOptTime,LastOptUserID,task.PTID,ui.TrueName FROM " +TTName;
            sqlStr = sqlStr + "  template   LEFT JOIN dbo.OtherTaskInfo task ON   template.RecID=task.RelationID ";
            sqlStr += " LEFT JOIN SysRightsManager.dbo.UserInfo ui ON task.LastOptUserID = ui.UserID ";
            
           sqlStr=sqlStr+ " where task.RelationTableID='"+ StringHelper.SqlFilter(ttcode) +"' and task.ProjectID="+ StringHelper.SqlFilter(projectid);

            try
            {
                ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        /// <summary>
        /// 根据IDs获取自定义表中的数据，给任务加创建时间，提交时间过滤 add by qizq 2014-11-25
        /// </summary>
        /// <param name="selectDataIDs"></param>
        /// <param name="TTName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DataTable GetDataByIDs(string selectDataIDs, string TTName, string ttcode, string projectid,string taskcreatestart,string taskcreateend,string tasksubstart,string tasksubend, out string msg)
        {
            msg = "";
            DataSet ds;

            //  string sqlStr = "select * from " + TTName + " where RecID in (" + selectDataIDs + ")";
            string sqlStr = @"SELECT template.*,task.TaskStatus,LastOptTime,LastOptUserID,task.PTID,ui.TrueName FROM " + TTName
             + "  template   LEFT JOIN dbo.OtherTaskInfo task ON   template.RecID=task.RelationID "
             + " LEFT JOIN SysRightsManager.dbo.UserInfo ui ON task.LastOptUserID = ui.UserID "
             + " where task.RelationTableID='" + StringHelper.SqlFilter(ttcode) + "' and task.ProjectID=" + StringHelper.SqlFilter(projectid);

            //add by qizq 2014-11-25 给任务加过滤条件
            if (!string.IsNullOrEmpty(taskcreatestart) && !string.IsNullOrEmpty(taskcreateend))
            {
                sqlStr += " and task.createtime>='" + StringHelper.SqlFilter(taskcreatestart.Trim()) + " 0:0:0' and task.createtime<='" + StringHelper.SqlFilter(taskcreateend.Trim()) + " 23:59:59'";
            }
            if (!string.IsNullOrEmpty(tasksubstart) && !string.IsNullOrEmpty(tasksubend))
            {
                sqlStr += " and task.lastopttime>='" + StringHelper.SqlFilter(tasksubstart.Trim()) + " 0:0:0' and task.lastopttime<='" + StringHelper.SqlFilter(tasksubend.Trim()) + " 23:59:59' and task.taskstatus=4";
            }
            //

            try
            {
                ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }


        /// <summary>
        /// 根据IDs获取自定义表中的
        /// </summary>
        /// <param name="selectDataIDs"></param>
        /// <param name="TTName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DataTable GetDataByRelationIDs(string selectDataIDs, string TTName, out string msg)
        {
            msg = "";
            DataSet ds;

            string sqlStr = "select * from " +TTName + " where RecID in (" + Dal.Util.SqlFilterByInCondition(selectDataIDs) + ")";

            try
            {
                ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
    }
}

