using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class YTGActivityTask : DataBase
    {
        private YTGActivityTask() { }
        private static YTGActivityTask _instance = null;
        public static YTGActivityTask Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGActivityTask();
                }
                return _instance;
            }
        }
        //根据status分组，获取各状态下数量
        public DataTable GetStatusNum(Entities.QueryYTGActivityTaskInfo query)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pf", "lt", "BGID", "AssignUserID", query.LoginID);
            }
            #endregion

            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and AssignUserID=" + query.AssignUserID;
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and  pf.Name LIKE '%" + StringHelper.SqlFilter(query.ProjectName.ToString()) + "%'";
            }
            if (!string.IsNullOrEmpty(query.TaskCBeginTime))
            {
                where += " and lt.CreateTime>='" + StringHelper.SqlFilter(query.TaskCBeginTime) + " 0:0:0'";
            }
            if (!string.IsNullOrEmpty(query.TaskCEndTime))
            {
                where += " and lt.CreateTime<='" + StringHelper.SqlFilter(query.TaskCEndTime) + " 23:59:59'";
            }
            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }


            string sqlStr = @"SELECT  
                                        ISNULL(SUM(CASE WHEN lt.Status = 1 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '待分配' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 2 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '待处理' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 3 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '处理中' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 4 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '已处理' ,
                                        ISNULL(SUM(CASE WHEN lt.Status = 5 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '已结束' ,
                                        ISNULL(SUM(CASE WHEN lt.status = 4 AND lt.IsSuccess = 1 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '成功' ,
                                        ISNULL(SUM(CASE WHEN lt.status = 4 AND lt.IsSuccess = 0 THEN 1
                                                        ELSE 0
                                                   END), 0) AS '失败'
                                        FROM    dbo.YTGActivityTask lt
                                                LEFT JOIN dbo.ProjectInfo pf ON lt.ProjectID = pf.ProjectID
                                        WHERE   1 = 1 " + where;
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
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
        public DataTable GetYTGLeadsTask(QueryYTGActivityTaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhere(query);
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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_YTGActivityTask_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        private string GetWhere(QueryYTGActivityTaskInfo query)
        {
            string where = "";

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("pf", "lt", "BGID", "AssignUserID", query.LoginID);
            }
            #endregion

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and lt.TaskID='" + StringHelper.SqlFilter(query.TaskID) + "'";
            }
            if (query.Status != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.Status=" + query.Status;
            }
            if (query.AssignUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.AssignUserID=" + query.AssignUserID;
            }
            if (query.ProjectName != Constant.STRING_INVALID_VALUE)
            {
                where += " and pf.Name like '%" + StringHelper.SqlFilter(query.ProjectName) + "%'";
            }
            if (query.ProjectID != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.ProjectID=" + query.ProjectID;
            }
            if (query.IsSuccess != Constant.INT_INVALID_VALUE)
            {
                where += " and lt.IsSuccess=" + query.IsSuccess;
            }
            //生成任务时间
            if (!string.IsNullOrEmpty(query.TaskCBeginTime))
            {
                where += " and lt.CreateTime>='" + StringHelper.SqlFilter(query.TaskCBeginTime) + " 0:0:0'";
            }
            if (!string.IsNullOrEmpty(query.TaskCEndTime))
            {
                where += " and lt.CreateTime<='" + StringHelper.SqlFilter(query.TaskCEndTime) + " 23:59:59'";
            }
            //提交时间
            if (!string.IsNullOrEmpty(query.Subbegintime))
            {
                where += " and lt.LastUpdateTime>='" + StringHelper.SqlFilter(query.Subbegintime) + " 0:0:0' and lt.Status=4";
            }
            if (!string.IsNullOrEmpty(query.Subendtime))
            {
                where += " and lt.LastUpdateTime<='" + StringHelper.SqlFilter(query.Subendtime) + " 23:59:59' and lt.Status=4";
            }
            return where;
        }

        public DataTable GetYTGProjectTasks(string where, string order, int currentPage, int pageSize, out int totalCount)
        {

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

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_YTGProject_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        //根据任务ID串获取任务信息
        public DataTable GetYTGTaskInfoListByIDs(string TaskIDS)
        {
            string sqlStr = "SELECT * FROM dbo.YTGActivityTask WHERE TaskID IN (" + Dal.Util.SqlFilterByInCondition(TaskIDS) + ")";
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public void UpdateYTGTaskStatus(string strTaskId, string strAssignUser, int nLoginUser, int status)
        {
            string strSql =
                string.Format(" UPDATE dbo.YTGActivityTask	SET AssignUserID='{0}',Status={1},LastUpdateTime=GETDATE(),LastUpdateUserID={2} WHERE TaskID='{3}' ",
                    StringHelper.SqlFilter(strAssignUser), status, nLoginUser, StringHelper.SqlFilter(strTaskId));

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }

        /// 任务导出
        /// <summary>
        /// 任务导出
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable GetYTGProjectTasksForExport(QueryYTGActivityTaskInfo query, string c1, string c2, string c3)
        {
            string where = GetWhere(query);
            string sql = @"SELECT
                                    pf.Name AS [项目名称],
                                    lt.UserName AS [姓名],
                                    CASE lt.Sex WHEN 1 THEN '男' WHEN 2 THEN '女' ELSE '' END AS [性别],
                                    lt.Tel AS [电话],
                                    p.AreaName AS [下单省份],
                                    c.AreaName AS [下单城市],
                                    ay.ActivityName AS [关联活动主题],
                                    lt.OrderCreateTime AS [下单时间],
                                    b1.Name AS [下单品牌],
                                    c1.Name AS [下单车型],
                                    tp.AreaName AS [试驾省份],
                                    tc.AreaName AS [试驾城市],
                                    b2.Name AS [意向品牌],
                                    c2.Name AS [意向车型],

                                    CASE lt.PBuyCarTime " + c1 + @" ELSE '' END AS [预计购车时间],
                                    CASE lt.IsSuccess WHEN 1 THEN '是' WHEN 0 THEN '否' ELSE '' END AS [是否成功],
                                    CASE lt.FailReason " + c2 + @" ELSE '' END AS [失败原因],
                                    lt.Remark AS [备注],
                                    lt.LastUpdateTime AS [操作时间],
                                    ui.TrueName AS [所属坐席],
                                    CASE lt.Status " + c3 + @" ELSE '' END AS [任务状态]
                                    FROM    dbo.YTGActivityTask lt        
                                            INNER JOIN dbo.ProjectInfo pf ON lt.ProjectID = pf.ProjectID        
                                            INNER JOIN dbo.YTGActivity ay ON lt.ActivityID=ay.ActivityID        
                                            LEFT JOIN CRM2009.dbo.AreaInfo p ON lt.ProvinceID=p.AreaID
                                            LEFT JOIN CRM2009.dbo.AreaInfo c ON lt.CityID=c.AreaID
                                            LEFT JOIN CRM2009.dbo.AreaInfo tp ON lt.TestDriveProvinceID=tp.AreaID
                                            LEFT JOIN CRM2009.dbo.AreaInfo tc ON lt.TestDriveCityID=tc.AreaID        
                                            LEFT JOIN dbo.CarSerial c1 ON lt.OrderCarSerialID=c1.CSID
                                            LEFT JOIN dbo.CarBrand b1 ON c1.BrandID=b1.BrandID
                                            LEFT JOIN dbo.CarSerial c2 ON lt.DCarSerialID=c2.CSID
                                            LEFT JOIN dbo.CarBrand b2 ON c2.BrandID=b2.BrandID        
                                            LEFT JOIN CRM2009.dbo.v_userinfo ui ON lt.AssignUserID = ui.UserID        
                                    WHERE   1 = 1 " + where;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
    }
}
