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
    /// 数据访问类ProjectTaskLog。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:33 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTaskLog : DataBase
    {
        #region Instance
        public static readonly ProjectTaskLog Instance = new ProjectTaskLog();
        #endregion

        #region const
        private const string P_PROJECTTASKLOG_SELECT = "p_ProjectTaskLog_Select";
        private const string P_PROJECTTASKLOG_INSERT = "p_ProjectTaskLog_Insert";
        private const string P_PROJECTTASKLOG_UPDATE = "p_ProjectTaskLog_Update";
        private const string P_PROJECTTASKLOG_DELETE = "p_ProjectTaskLog_Delete";
        #endregion

        #region Contructor
        protected ProjectTaskLog()
        { }
        #endregion

        public DataTable GetProjectTaskLog(string tID)
        {
            DataSet ds;
            SqlParameter[] parameters = {
                new SqlParameter("@PTID",SqlDbType.VarChar)
            };

            parameters[0].Value = tID;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASKLOG_SELECT, parameters);

            //totalCount = int.Parse(parameters[4].Value.ToString());

            return ds.Tables[0];
        }

        #region Insert
        /// <summary>
        /// 添加销售网络信息
        /// </summary>
        /// <param name="model">实体类销售网络信息</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int InsertProjectTaskLog(Entities.ProjectTaskLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@TaskID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,200)};
            parameters[0].Value = model.PTID;
            parameters[1].Value = model.CreateUserID;
            parameters[2].Value = model.TaskStatus;
            parameters[3].Value = model.Description;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASKLOG_INSERT, parameters);

            return Convert.ToInt32(parameters[0].Value.ToString());
        }
        /// <summary>
        /// 插入任务操作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InserOtherTaskLog(Entities.ProjectTaskLog model)
        {
            string sqlstr = "insert into PROJECTTASKLOG(PTID,CreateUserID,CreateTime,TaskStatus,Description,OperationStatus) values('" + StringHelper.SqlFilter(model.PTID) + "','" + model.CreateUserID + "','" + System.DateTime.Now + "','" + model.TaskStatus + "','" + StringHelper.SqlFilter(model.Description) + "','" + model.OperationStatus + "')";
            int Result = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, null);
            return Result;
        }
        #endregion

        #region Updata
        /// <summary>
        /// 修改销售网络信息
        /// </summary>
        /// <param name="model">实体类销售网络信息</param>
        /// <returns>成功:索引值;失败:-1</returns>
        public int UpdataProjectTaskLog(Entities.ProjectTaskLog model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4),
                    new SqlParameter("@PTID", SqlDbType.VarChar),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4),
					new SqlParameter("@TaskStatus", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,200),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.PTID;
            parameters[2].Value = model.CreateUserID;
            parameters[3].Value = model.TaskStatus;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.CreateTime;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_PROJECTTASKLOG_UPDATE, parameters);
        }
        #endregion

        #region SelectSingle
        /// <summary>
        /// 按照ID查询符合条件的一条记录
        /// </summary>
        /// <param name="rid">索引ID</param>
        /// <returns>符合条件的一个值对象</returns>
        //public Entities.ProjectTaskLog GetProjectTaskLog(int rid)
        //{
        //    DataSet ds;
        //    SqlParameter[] parameters = {
        //            new SqlParameter("@Rid", SqlDbType.Int,4)
        //            };

        //    parameters[0].Value = rid;
        //    //绑定存储过程参数

        //    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_ProjectTaskLog_SELECT_BY_ID, parameters);

        //    if (ds != null)
        //    {
        //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            return LoadSingleProjectTaskLog(ds.Tables[0].Rows[0]);
        //        }
        //    }
        //    return null;
        //}

        private static Entities.ProjectTaskLog LoadSingleProjectTaskLog(DataRow row)
        {
            Entities.ProjectTaskLog model = new Entities.ProjectTaskLog();
            if (row["RecID"] != DBNull.Value)
            {
                model.RecID = Convert.ToInt32(row["RecID"].ToString());
            }

            if (row["PTID"] != DBNull.Value)
            {
                model.PTID = row["PTID"].ToString();
            }

            if (row["CreateUserID"] != DBNull.Value)
            {
                model.CreateUserID = Convert.ToInt32(row["CreateUserID"].ToString());
            }

            if (row["TaskStatus"] != DBNull.Value)
            {
                model.TaskStatus = Convert.ToInt32(row["TaskStatus"].ToString());
            }

            if (row["Description"] != DBNull.Value)
            {
                model.Description = row["Description"].ToString();
            }

            if (row["CreateTime"] != DBNull.Value)
            {
                model.CreateTime = Convert.ToDateTime(row["CreateTime"].ToString());
            }


            return model;
        }
        #endregion

    }
}

