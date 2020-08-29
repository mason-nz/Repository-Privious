using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LabelTask
{
    public class LB_Project : DataBase
    {
        public static readonly LB_Project Instance = new LB_Project();

        #region Insert
        public int Insert(Entities.LabelTask.LB_Project model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProjectID",SqlDbType.Int,4),
                new SqlParameter("@Name",model.Name),
                new SqlParameter("@ProjectType",model.ProjectType),
                new SqlParameter("@TaskCount",model.TaskCount),
                new SqlParameter("@GenerateTaskCount",model.GenerateTaskCount),
                new SqlParameter("@Status",model.Status),
                new SqlParameter("@UploadFileURL",model.UploadFileURL),
                new SqlParameter("@CreateUserID",model.CreateUserID)
            };

            parameters[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_LBProject_Insert", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region 更新项目生成任务数量
        public int UpdateGeTaskCount(int projectID, int geTaskCount)
        {
            string sql = @"UPDATE dbo.LB_Project SET GenerateTaskCount=@GenerateTaskCount WHERE ProjectID=@ProjectID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProjectID",projectID),
                new SqlParameter("@GenerateTaskCount",geTaskCount)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        #endregion
        #region Delete
        public int Delete(int projectID)
        {
            string sql = @"DELETE FROM dbo.LB_Project WHERE ProjectID=@ProjectID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ProjectID",projectID)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
        }
        #endregion
        /// <summary>
        /// 查询标签项目列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public DataTable SelectProjectList(int PageIndex, int PageCount)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@pageIndex", SqlDbType.Int),
                    new SqlParameter("@pageSize", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = PageIndex;
            parameters[1].Value = PageCount;
            parameters[2].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectProjectList", parameters);
            int totalCount = (int)(parameters[2].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 查询标签项目详情
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public DataTable SelectProjectInfo(int projectID)
        {

            string strSql = @"SELECT Name,ProjectType, ProjectID, P.Status, P.CreateTime,UploadFileURL, GenerateTaskCount, TaskCount,VU1.UserName AS CreatorName,
            (SELECT COUNT(1) FROM dbo.LB_Task WHERE Status = 63005 AND ProjectID = P.ProjectID) AS DealCount,
            (SELECT COUNT(1) FROM dbo.LB_Task WHERE(Status = 63001 OR Status = 63002 OR Status = 63003) AND ProjectID = P.ProjectID) AS NoDealCount,
            (SELECT COUNT(1) FROM dbo.LB_Task WHERE Status = 63004 AND ProjectID = P.ProjectID) AS SingleCount,
            (SELECT COUNT(1) FROM dbo.LB_Task WHERE Status=63006 AND ProjectID=P.ProjectID) AS ExamineCount 
            FROM dbo.LB_Project P  LEFT JOIN v_UserInfo VU1 ON P.CreateUserID =VU1.UserID WHERE ProjectID=" + projectID;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

    }
}
