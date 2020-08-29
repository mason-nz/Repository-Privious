/********************************************************
*创建人：hant
*创建时间：2017/12/18 17:00:04 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.TaskInfo
{
    public class TaskList : DataBase
    {
        #region
        public static readonly TaskList Instance = new TaskList();
        #endregion

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public List<Entities.Task.TaskList> GetTaskList(int PageIndex, int PageSize,int TaskType, DateTime time, out int totalCount)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.AppendFormat("SELECT [RecID] AS TaskID,[TaskName] AS Title,[CategoryID],[ImgUrl],[CPCPrice],[CPLPrice],[TaskType],CONVERT(nvarchar(10),[BeginTime]) AS [BeginTime],CONVERT(nvarchar(10),[EndTime]) AS [EndTime],[Status],[TakeCount],[RuleCount],[CreateTime] YanFaFrom [Chitunion2017].[dbo].[LE_TaskInfo] WHERE CreateTime>'{0}' ", time);
            if (TaskType > 0)
            {
                SQL.AppendFormat(" AND TaskType = {0}",TaskType);
            }
            string strOrder = " CreateTime DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                new SqlParameter("@SQL",SQL.ToString()),
                new SqlParameter("@CurPage",PageIndex),
                new SqlParameter("@PageRows",PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var ds = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.StoredProcedure, "p_Page", sqlParams);
            totalCount = (int)(sqlParams[0].Value);
            return DataTableToList<Entities.Task.TaskList>(ds.Tables[0]);
        }


        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Entities.Task.TaskList GetTask(int recid)
        {
            var sql = @"
                  SELECT [RecID] AS TaskID
                                  ,[TaskName] AS Title
	                              ,[CategoryID]
	                              ,[ImgUrl]
	                              ,[CPCPrice]
                                  ,[CPLPrice]
	                              ,[TaskType] 
	                              ,CONVERT(nvarchar(10),[BeginTime]) AS [BeginTime]
                                  ,CONVERT(nvarchar(10),[EndTime]) AS [EndTime]
	                              ,[Status]
	                              ,[TakeCount]
	                              ,[RuleCount]
	                              ,[CreateTime]
                              FROM [Chitunion2017].[dbo].[LE_TaskInfo]
                              WHERE RecID=@recid";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@recid",recid)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Task.TaskList>(data.Tables[0]);
        }

    }
}
