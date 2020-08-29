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
    /// 数据访问类ProjectTask_Employee。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:32 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_Employee : DataBase
    {
        #region Instance
        public static readonly ProjectTask_Employee Instance = new ProjectTask_Employee();
        #endregion

        #region const
        private const string P_PROJECTTASK_EMPLOYEE_SELECT = "p_ProjectTask_Employee_Select";
        public const string P_PROJECTTASK_EMPLOYEE_SELECT_BY_ID = "p_ProjectTask_Employee_select_by_id";
        private const string P_PROJECTTASK_EMPLOYEE_INSERT = "p_ProjectTask_Employee_Insert";
        private const string P_PROJECTTASK_EMPLOYEE_UPDATE = "p_ProjectTask_Employee_Update";
        private const string P_PROJECTTASK_EMPLOYEE_DELETE = "p_ProjectTask_Employee_Delete";
        #endregion

        #region Contructor
        protected ProjectTask_Employee()
        { }
        #endregion

        #region Select

        public DataTable GetProjectTask_Employee(string ptid)
        {
            string sqlstr = "select * from ProjectTask_Employee where ptid='" + StringHelper.SqlFilter(ptid) + "' and status=0";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null).Tables[0];
        }


        /// <summary>
        /// 按照查询条件查询  分页
        /// </summary>
        /// <param name="queryProjectTask_Employee">查询值对象，用来存放查询条件</param>        
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="totalCount">总行数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns>销售网络集合</returns>
        public DataTable GetProjectTask_Employee(QueryProjectTask_Employee query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";

            if (query.Source != Constant.INT_INVALID_VALUE)
            {
                where += " And t.Source =" + query.Source;
            }
            if (query.RelationID != Constant.STRING_INVALID_VALUE)
            {
                where += " And t.RelationID ='" + StringHelper.SqlFilter(query.RelationID) + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " And te.Status =" + query.Status;
            }
            //if (query.PTID != Constant.STRING_INVALID_VALUE)
            //{
            //    where += "and te.PTID='" + query.PTID + "'";
            //}

            DataSet ds;
            SqlParameter[] parameters = {
                     new SqlParameter("@where", SqlDbType.VarChar,8000),
			new SqlParameter("@order", SqlDbType.VarChar,200),
			new SqlParameter("@page", SqlDbType.Int,4),
            new SqlParameter("@pagesize", SqlDbType.Int,4),
			new SqlParameter("@TotalRecorder", SqlDbType.Int,4)
             };

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_EMPLOYEE_SELECT, parameters);

            totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }


        #endregion

        #region Insert
        /// <summary>
        /// 添加销售网络信息
        /// </summary>
        /// <param name="model">实体类销售网络信息</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertProjectTask_Employee(Entities.ProjectTask_Employee model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.Status;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_EMPLOYEE_INSERT, parameters);

            return Convert.ToInt32(parameters[0].Value.ToString());
        }
        #endregion

        #region Updata
        /// <summary>
        /// 修改销售网络信息
        /// </summary>
        /// <param name="model">实体类销售网络信息</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int UpdataProjectTask_Employee(Entities.ProjectTask_Employee model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.Status;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_EMPLOYEE_UPDATE, parameters);
        }


        #endregion

        /// <summary>
        /// 根据TID，更新Status为-1
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public int DeleteProjectTask_Employee(string tid)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@PTID", SqlDbType.VarChar)
                                        };
            parameters[0].Value = tid;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_EMPLOYEE_DELETE, parameters);
        }

        #region SelectSingle
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="rid">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        public Entities.ProjectTask_Employee GetProjectTask_Employee(int rid)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                    new SqlParameter("@Rid", SqlDbType.Int,4)
                    };

            parameters[0].Value = rid;
            //绑定存储过程参数

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASK_EMPLOYEE_SELECT_BY_ID, parameters);

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return LoadSingleProjectTask_Employee(ds.Tables[0].Rows[0]);
                }
            }
            return null;
        }

        private static Entities.ProjectTask_Employee LoadSingleProjectTask_Employee(DataRow row)
        {
            Entities.ProjectTask_Employee model = new Entities.ProjectTask_Employee();

            if (row["RecID"] != DBNull.Value)
            {
                model.RecID = Convert.ToInt32(row["RecID"].ToString());
            }

            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["UserID"] != DBNull.Value)
            {
                model.UserID = Convert.ToInt32(row["UserID"].ToString());
            }

            if (row["Status"] != DBNull.Value)
            {
                model.Status = Convert.ToInt32(row["Status"].ToString());
            }

            return model;
        }
        #endregion


        public void DeleteByIDs(string taskIDStr)
        {
            string sqlStr = "delete ProjectTask_Employee where PTID in (" + Dal.Util.SqlFilterByInCondition(taskIDStr) + ")";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public long Add(Entities.ProjectTask_Employee model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,8),
					new SqlParameter("@PTID", SqlDbType.VarChar,20),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.CreateTime;
            parameters[5].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ProjectTaskInfo_Employee_Insert_One", parameters);
            //return (long)parameters[0].Value;
        }

        /// 清空已分配的人员
        /// <summary>
        /// 清空已分配的人员
        /// </summary>
        /// <param name="MinOtherTaskID"></param>
        /// <param name="MaxOtherTaskID"></param>
        /// <param name="assi_total"></param>
        public void ClearProjectTaskEmployee(string minID, string maxID, int top)
        {
            string sql = @"DELETE FROM ProjectTask_Employee WHERE PTID IN (" +
                "SELECT TOP " + top + " PTID FROM dbo.OtherTaskInfo WHERE PTID>='" + StringHelper.SqlFilter(minID) + "' AND PTID<='" + StringHelper.SqlFilter(maxID) + "' ORDER BY PTID"
                + ")";
            int num = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        /// 清空已分配的人员
        /// <summary>
        /// 清空已分配的人员
        /// </summary>
        /// <param name="MinOtherTaskID"></param>
        /// <param name="MaxOtherTaskID"></param>
        /// <param name="assi_total"></param>
        public void ClearProjectTaskEmployee(string ptids)
        {
            string sql = @"DELETE FROM ProjectTask_Employee WHERE PTID IN (" + ptids + ")";
            int num = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
    }
}

