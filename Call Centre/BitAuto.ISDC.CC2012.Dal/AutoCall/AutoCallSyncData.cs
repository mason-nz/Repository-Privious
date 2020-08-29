using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class AutoCallSyncData : DataBase
    {
        public static AutoCallSyncData Instance = new AutoCallSyncData();
        public string Holly_Business
        {
            get { return ConnectionStrings_Holly_Business; }
        }
        public string CC
        {
            get { return CONNECTIONSTRINGS; }
        }

        private string GetMaxSql(int maxrow)
        {
            string maxsql = "";
            if (maxrow > 0)
            {
                maxsql = " top " + maxrow + " ";
            }
            return maxsql;
        }

        /// 获取项目数据通过timestamp
        /// <summary>
        /// 获取项目数据通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetAutoCallByTimestamp_Project(long timestamp, int maxrow = -1)
        {
            string maxsql = GetMaxSql(maxrow);
            string sql = @"SELECT " + maxsql + " CAST([TIMESTAMP] AS bigint) AS timelong,* FROM AutoCall_ProjectInfo WHERE Timestamp>" + timestamp + " ORDER BY [TIMESTAMP]";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 获取任务数据通过timestamp
        /// <summary>
        /// 获取任务数据通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetAutoCallByTimestamp_Task(long timestamp, int maxrow = -1)
        {
            string maxsql = GetMaxSql(maxrow);
            string sql = @"SELECT " + maxsql + " CAST([TIMESTAMP] AS bigint) AS timelong,* FROM dbo.AutoCall_TaskInfo WHERE Timestamp>" + timestamp + " ORDER BY [TIMESTAMP]";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        /// 获取统计表最大时间戳
        /// <summary>
        /// 获取统计表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Stat()
        {
            string sql = "SELECT ISNULL(MAX([TIMESTAMP]),0) FROM dbo.AutoCall_ProjectInfoStat";
            return CommonFunction.ObjectToLong(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }
        /// 获取明细表最大时间戳
        /// <summary>
        /// 获取明细表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Detail()
        {
            string sql = "SELECT ISNULL(MAX([TIMESTAMP]),0) FROM dbo.AutoCall_ACDetail";
            return CommonFunction.ObjectToLong(SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql));
        }

        /// 清空临时表-统计
        /// <summary>
        /// 清空临时表-统计
        /// </summary>
        public void ClearTemp_Stat()
        {
            string sql = "delete from AutoCall_ProjectInfoStat_temp";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        /// 清空临时表-明细
        /// <summary>
        /// 清空临时表-明细
        /// </summary>
        public void ClearTemp_Detail()
        {
            string sql = "delete from AutoCall_ACDetail_temp";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        /// 从临时表更新-统计
        /// <summary>
        /// 从临时表更新-统计
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Stat()
        {
            int mod = 0;
            int add = 0;
            //更新
            string modsql = @"UPDATE AutoCall_ProjectInfoStat
                                        SET 
                                        AutoCall_ProjectInfoStat.ACTotalNum=b.ACTotalNum,
                                        AutoCall_ProjectInfoStat.IVRConnectNum=b.IVRConnectNum,
                                        AutoCall_ProjectInfoStat.DisconnectNum=b.DisconnectNum,
                                        AutoCall_ProjectInfoStat.CreateTime=b.CreateTime,
                                        AutoCall_ProjectInfoStat.CreateUserID=b.CreateUserID,
                                        AutoCall_ProjectInfoStat.[Timestamp]=b.[Timestamp]
                                        FROM AutoCall_ProjectInfoStat_temp b
                                        WHERE AutoCall_ProjectInfoStat.ProjectID=b.ProjectID";
            mod = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, modsql);
            //新增
            string addsql = @"INSERT INTO AutoCall_ProjectInfoStat(ProjectID,ACTotalNum,IVRConnectNum,DisconnectNum,CreateTime,CreateUserID,[Timestamp])
                                        SELECT ProjectID,ACTotalNum,IVRConnectNum,DisconnectNum,CreateTime,CreateUserID,[Timestamp]
                                        FROM AutoCall_ProjectInfoStat_temp
                                        WHERE ProjectID NOT IN(SELECT ProjectID FROM AutoCall_ProjectInfoStat)";
            add = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, addsql);
            return new int[] { mod, add };
        }
        /// 从临时表更新-明细
        /// <summary>
        /// 从临时表更新-明细
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Detail()
        {
            int mod = 0;
            int add = 0;
            //更新
            string modsql = @"UPDATE AutoCall_ACDetail
                                        SET 
                                        AutoCall_ACDetail.ACStatus=b.ACStatus,
                                        AutoCall_ACDetail.ACTID=b.ACTID,
                                        AutoCall_ACDetail.ProjectID=b.ProjectID,
                                        AutoCall_ACDetail.BusinessRecID=b.BusinessRecID,
                                        AutoCall_ACDetail.ReturnTime=b.ReturnTime,
                                        AutoCall_ACDetail.ACResult=b.ACResult,
                                        AutoCall_ACDetail.CreateTime=b.CreateTime,
                                        AutoCall_ACDetail.[Timestamp]=b.[Timestamp]
                                        FROM AutoCall_ACDetail_temp b
                                        WHERE AutoCall_ACDetail.RecID=b.RecID";
            mod = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, modsql);
            //新增
            string addsql = @"INSERT INTO AutoCall_ACDetail(RecID,ACStatus,ACTID,ProjectID,BusinessRecID,ReturnTime,ACResult,CreateTime,[Timestamp])
                                        SELECT RecID,ACStatus,ACTID,ProjectID,BusinessRecID,ReturnTime,ACResult,CreateTime,[Timestamp] 
                                        FROM AutoCall_ACDetail_temp
                                        WHERE RecID NOT IN (SELECT RecID FROM AutoCall_ACDetail)";
            add = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, addsql);
            return new int[] { mod, add };
        }

        #region 查询合力业务数据库
        /// 查询统计表通过timestamp
        /// <summary>
        /// 查询统计表通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="maxrow"></param>
        /// <returns></returns>
        public DataTable GetAutoCallByTimestamp_Stat_XA(long timestamp, int maxrow = -1)
        {
            string maxsql = GetMaxSql(maxrow);
            string sql = @"SELECT " + maxsql + " CAST([TIMESTAMP] AS bigint) AS timelong,* FROM dbo.AutoCall_ProjectInfoStat WHERE Timestamp>" + timestamp + " ORDER BY [TIMESTAMP]";
            return SqlHelper.ExecuteDataset(ConnectionStrings_Holly_Business, CommandType.Text, sql).Tables[0];
        }
        /// 查询明细表通过timestamp
        /// <summary>
        /// 查询明细表通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="maxrow"></param>
        /// <returns></returns>
        public DataTable GetAutoCallByTimestamp_Detail_XA(long timestamp, int maxrow = -1)
        {
            string maxsql = GetMaxSql(maxrow);
            string sql = @"SELECT " + maxsql + " CAST([TIMESTAMP] AS bigint) AS timelong,* FROM dbo.AutoCall_ACDetail WHERE Timestamp>" + timestamp + " ORDER BY [TIMESTAMP]";
            return SqlHelper.ExecuteDataset(ConnectionStrings_Holly_Business, CommandType.Text, sql).Tables[0];
        }

        /// 获取项目表最大时间戳
        /// <summary>
        /// 获取项目表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Project_XA()
        {
            string sql = "SELECT ISNULL(MAX([TIMESTAMP]),0) FROM dbo.AutoCall_ProjectInfo";
            return CommonFunction.ObjectToLong(SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sql));
        }
        /// 获取任务表最大时间戳
        /// <summary>
        /// 获取任务表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Task_XA()
        {
            string sql = "SELECT ISNULL(MAX([TIMESTAMP]),0) FROM dbo.AutoCall_TaskInfo";
            return CommonFunction.ObjectToLong(SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sql));
        }

        /// 删除西安库-项目临时表
        /// <summary>
        /// 删除西安库-项目临时表
        /// </summary>
        public void ClearTemp_Project_XA()
        {
            string sql = "delete from AutoCall_ProjectInfo_Temp";
            SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, sql);
        }
        /// 删除西安库-任务临时表
        /// <summary>
        /// 删除西安库-任务临时表
        /// </summary>
        public void ClearTemp_Task_XA()
        {
            string sql = "delete from AutoCall_TaskInfo_Temp";
            SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, sql);
        }

        /// 从临时表更新-项目
        /// <summary>
        /// 从临时表更新-项目
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Project_XA()
        {
            int mod = 0;
            int add = 0;
            //更新
            string modsql = @"UPDATE AutoCall_ProjectInfo
                                        SET 
                                        AutoCall_ProjectInfo.SkillID=b.SkillID,
                                        AutoCall_ProjectInfo.Status=b.Status,
                                        AutoCall_ProjectInfo.ACStatus=b.ACStatus,
                                        AutoCall_ProjectInfo.CDID=b.CDID,

                                        AutoCall_ProjectInfo.CreateTime=b.CreateTime,
                                        AutoCall_ProjectInfo.CreateUserID=b.CreateUserID,
                                        AutoCall_ProjectInfo.ModifyTime=b.ModifyTime,
                                        AutoCall_ProjectInfo.ModifyUserID=b.ModifyUserID,
                                        AutoCall_ProjectInfo.TotalTaskNum=b.TotalTaskNum,

                                        AutoCall_ProjectInfo.AppendDataTime=b.AppendDataTime,
                                        AutoCall_ProjectInfo.[Timestamp]=b.[Timestamp],
                                        AutoCall_ProjectInfo.CallHollyResult=0,
                                        AutoCall_ProjectInfo.CallHollyCount=0
                                        FROM AutoCall_ProjectInfo_Temp b
                                        WHERE AutoCall_ProjectInfo.ProjectID=b.ProjectID";
            mod = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, modsql);
            //新增
            string addsql = @"INSERT INTO AutoCall_ProjectInfo(
                                        ProjectID,SkillID,Status,ACStatus,CDID,
                                        CreateTime,CreateUserID,ModifyTime,ModifyUserID,TotalTaskNum,
                                        AppendDataTime,[Timestamp])
                                        SELECT 
                                        ProjectID,SkillID,Status,ACStatus,CDID,
                                        CreateTime,CreateUserID,ModifyTime,ModifyUserID,TotalTaskNum,
                                        AppendDataTime,[Timestamp]
                                        FROM AutoCall_ProjectInfo_Temp
                                        WHERE ProjectID NOT IN (SELECT ProjectID FROM AutoCall_ProjectInfo)";
            add = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, addsql);
            return new int[] { mod, add };
        }
        /// 从临时表更新-任务
        /// <summary>
        /// 从临时表更新-任务
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Task_XA()
        {
            int mod = 0;
            int add = 0;
            int del = 0;
            int rep = 0;
            //更新
            string modsql = @"UPDATE AutoCall_TaskInfo
                                        SET 
                                        AutoCall_TaskInfo.BusinessRecID=b.BusinessRecID,
                                        AutoCall_TaskInfo.BusinessID=b.BusinessID,
                                        AutoCall_TaskInfo.ProjectID=b.ProjectID,
                                        AutoCall_TaskInfo.Phone=b.Phone,

                                        AutoCall_TaskInfo.PhonePrefix=b.PhonePrefix,
                                        AutoCall_TaskInfo.Status=b.Status,
                                        AutoCall_TaskInfo.CreateTime=b.CreateTime,
                                        AutoCall_TaskInfo.[Timestamp]=b.[Timestamp]

                                        FROM AutoCall_TaskInfo_Temp b
                                        WHERE AutoCall_TaskInfo.ACTID=b.ACTID";
            mod = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, modsql);
            //新增
            string addsql = @"INSERT INTO AutoCall_TaskInfo(
                                        ACTID,BusinessRecID,BusinessID,ProjectID,Phone,
                                        PhonePrefix,Status,CreateTime,[Timestamp],ACStatus,
                                        ServiceTakeTime)
                                        SELECT
                                        ACTID,BusinessRecID,BusinessID,ProjectID,Phone,
                                        PhonePrefix,Status,CreateTime,[Timestamp],ACStatus,
                                        ServiceTakeTime
                                        FROM AutoCall_TaskInfo_Temp
                                        WHERE ACTID NOT IN (SELECT ACTID FROM AutoCall_TaskInfo)";
            add = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, addsql);
            ////删除失效数据==数据需要保留-强斐-2015-10-26
            //string delsql = "DELETE FROM AutoCall_TaskInfo WHERE Status<>1";
            //del = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, delsql);
            //删除重复任务id数据
            string repsql = @"DELETE FROM dbo.AutoCall_TaskInfo 
                                        WHERE ACTID IN(
                                        SELECT MIN(ACTID) FROM dbo.AutoCall_TaskInfo 
                                        WHERE  Status=1
                                        GROUP BY BusinessID
                                        HAVING COUNT(*)>1)";
            rep = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, repsql);
            return new int[] { mod, add, del, rep };
        }

        /// 查询项目表暂停结束的项目
        /// <summary>
        /// 查询项目表暂停结束的项目
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetStopAndPauseProject_XA(string status)
        {
            string sql = @"SELECT new.ProjectID,new.ACStatus,old.ACStatus AS Status_old,old.Timestamp AS Timestamp_old
                                FROM AutoCall_ProjectInfo_Temp new
                                INNER JOIN AutoCall_ProjectInfo old ON old.ProjectID = new.ProjectID
                                AND ISNULL(new.ACStatus,0)!=ISNULL(old.ACStatus,0)
                                AND new.ACStatus IN (" + Dal.Util.SqlFilterByInCondition(status) + ")";
            return SqlHelper.ExecuteDataset(ConnectionStrings_Holly_Business, CommandType.Text, sql).Tables[0];
        }
        /// 更新项目表状态和时间戳
        /// <summary>
        /// 更新项目表状态和时间戳
        /// </summary>
        /// <param name="projectid"></param>
        /// <param name="status"></param>
        /// <param name="timestamp"></param>
        public void UpdateProjectStatusAndTimestamp_XA(long projectid, int status, long timestamp)
        {
            string sql = "UPDATE AutoCall_ProjectInfo SET ACStatus=" + status + " , Timestamp=" + timestamp + " WHERE ProjectID=" + projectid;
            SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, sql);
        }

        /// 更新统计表
        /// <summary>
        /// 更新统计表
        /// </summary>
        /// <returns></returns>
        public int UpdateAutoCallProjectInfoStat_XA()
        {
            SqlParameter[] parameters = { new SqlParameter("@result", SqlDbType.Int, 8) };
            parameters[0].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.StoredProcedure, "P_AutoCall_ProjectInfoStat_Update", parameters);
            return CommonFunction.ObjectToInteger(parameters[0].Value);
        }

        /// 通知合力数据失败
        /// <summary>
        /// 通知合力数据失败
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public void UpdateCallHollyResult_XA(long projectid, int result)
        {
            string sql = "";
            if (result == 1)
            {
                //成功
                sql = @"UPDATE AutoCall_ProjectInfo SET CallHollyResult=1,CallHollyCount=0 WHERE ProjectID=" + projectid;
            }
            else
            {
                //失败
                sql = @"UPDATE AutoCall_ProjectInfo SET CallHollyResult=2,CallHollyCount=isnull(CallHollyCount,0)+1 WHERE ProjectID=" + projectid;
            }
            SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, sql);
        }
        /// 获取失败的数据
        /// <summary>
        /// 获取失败的数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetFailureCallHollyResult_XA()
        {
            string sql = @"SELECT * FROM AutoCall_ProjectInfo WHERE isnull(CallHollyResult,0)=2 AND isnull(CallHollyCount,0)<60";
            return SqlHelper.ExecuteDataset(ConnectionStrings_Holly_Business, CommandType.Text, sql).Tables[0];
        }
        #endregion
    }
}
