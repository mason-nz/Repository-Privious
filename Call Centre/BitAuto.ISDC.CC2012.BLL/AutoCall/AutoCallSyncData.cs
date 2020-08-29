using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class AutoCallSyncData
    {
        public static AutoCallSyncData Instance = new AutoCallSyncData();

        /// 获取项目数据通过timestamp
        /// <summary>
        /// 获取项目数据通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetAutoCallByTimestamp_Project(long timestamp, int maxrow = -1)
        {
            return Dal.AutoCallSyncData.Instance.GetAutoCallByTimestamp_Project(timestamp, maxrow);
        }
        /// 获取任务数据通过timestamp
        /// <summary>
        /// 获取任务数据通过timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public DataTable GetAutoCallByTimestamp_Task(long timestamp, int maxrow = -1)
        {
            return Dal.AutoCallSyncData.Instance.GetAutoCallByTimestamp_Task(timestamp, maxrow);
        }

        /// 获取统计表最大时间戳
        /// <summary>
        /// 获取统计表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Stat()
        {
            return Dal.AutoCallSyncData.Instance.GetAutoCallMaxTimeStamp_Stat();
        }
        /// 获取明细表最大时间戳
        /// <summary>
        /// 获取明细表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Detail()
        {
            return Dal.AutoCallSyncData.Instance.GetAutoCallMaxTimeStamp_Detail();
        }

        /// 统计表数据入库
        /// <summary>
        /// 统计表数据入库
        /// </summary>
        public void BulkCopyToDB_Stat(DataTable dt)
        {
            //清空临时表
            Dal.AutoCallSyncData.Instance.ClearTemp_Stat();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("ProjectID", "ProjectID"));
            list.Add(new SqlBulkCopyColumnMapping("ACTotalNum", "ACTotalNum"));
            list.Add(new SqlBulkCopyColumnMapping("IVRConnectNum", "IVRConnectNum"));
            list.Add(new SqlBulkCopyColumnMapping("DisconnectNum", "DisconnectNum"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));

            list.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
            list.Add(new SqlBulkCopyColumnMapping("timelong", "Timestamp"));

            //入库
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.AutoCallSyncData.Instance.CC, "AutoCall_ProjectInfoStat_Temp", 10000, list, out msg);
        }
        /// 明细表数据入库
        /// <summary>
        /// 明细表数据入库
        /// </summary>
        public void BulkCopyToDB_Detail(DataTable dt)
        {
            //清空临时表
            Dal.AutoCallSyncData.Instance.ClearTemp_Detail();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("RecID", "RecID"));
            list.Add(new SqlBulkCopyColumnMapping("ACStatus", "ACStatus"));
            list.Add(new SqlBulkCopyColumnMapping("ACTID", "ACTID"));
            list.Add(new SqlBulkCopyColumnMapping("ProjectID", "ProjectID"));
            list.Add(new SqlBulkCopyColumnMapping("BusinessRecID", "BusinessRecID"));

            list.Add(new SqlBulkCopyColumnMapping("ReturnTime", "ReturnTime"));
            list.Add(new SqlBulkCopyColumnMapping("ACResult", "ACResult"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            list.Add(new SqlBulkCopyColumnMapping("timelong", "Timestamp"));

            //入库
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.AutoCallSyncData.Instance.CC, "AutoCall_ACDetail_Temp", 10000, list, out msg);
        }

        /// 从临时表更新-统计
        /// <summary>
        /// 从临时表更新-统计
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Stat()
        {
            return Dal.AutoCallSyncData.Instance.UpdateFromTemp_Stat();
        }
        /// 从临时表更新-明细
        /// <summary>
        /// 从临时表更新-明细
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Detail()
        {
            return Dal.AutoCallSyncData.Instance.UpdateFromTemp_Detail();
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
            return Dal.AutoCallSyncData.Instance.GetAutoCallByTimestamp_Stat_XA(timestamp, maxrow);
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
            return Dal.AutoCallSyncData.Instance.GetAutoCallByTimestamp_Detail_XA(timestamp, maxrow);
        }

        /// 获取项目表最大时间戳
        /// <summary>
        /// 获取项目表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Project_XA()
        {
            return Dal.AutoCallSyncData.Instance.GetAutoCallMaxTimeStamp_Project_XA();
        }
        /// 获取任务表最大时间戳
        /// <summary>
        /// 获取任务表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetAutoCallMaxTimeStamp_Task_XA()
        {
            return Dal.AutoCallSyncData.Instance.GetAutoCallMaxTimeStamp_Task_XA();
        }

        /// 项目数据入库
        /// <summary>
        /// 项目数据入库
        /// </summary>
        /// <param name="dt"></param>
        public void BulkCopyToDB_Project_XA(DataTable dt)
        {
            //清空临时表
            Dal.AutoCallSyncData.Instance.ClearTemp_Project_XA();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("ProjectID", "ProjectID"));
            list.Add(new SqlBulkCopyColumnMapping("SkillID", "SkillID"));
            list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
            list.Add(new SqlBulkCopyColumnMapping("ACStatus", "ACStatus"));
            list.Add(new SqlBulkCopyColumnMapping("CDID", "CDID"));

            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            list.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
            list.Add(new SqlBulkCopyColumnMapping("ModifyTime", "ModifyTime"));
            list.Add(new SqlBulkCopyColumnMapping("ModifyUserID", "ModifyUserID"));
            list.Add(new SqlBulkCopyColumnMapping("TotalTaskNum", "TotalTaskNum"));

            list.Add(new SqlBulkCopyColumnMapping("AppendDataTime", "AppendDataTime"));
            list.Add(new SqlBulkCopyColumnMapping("timelong", "Timestamp"));

            //入库
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.AutoCallSyncData.Instance.Holly_Business, "AutoCall_ProjectInfo_Temp", 10000, list, out msg);
        }
        /// 任务数据入库
        /// <summary>
        /// 任务数据入库
        /// </summary>
        /// <param name="dt"></param>
        public void BulkCopyToDB_Task_XA(DataTable dt)
        {
            //清空临时表
            Dal.AutoCallSyncData.Instance.ClearTemp_Task_XA();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("ACTID", "ACTID"));
            list.Add(new SqlBulkCopyColumnMapping("BusinessRecID", "BusinessRecID"));
            list.Add(new SqlBulkCopyColumnMapping("BusinessID", "BusinessID"));
            list.Add(new SqlBulkCopyColumnMapping("ProjectID", "ProjectID"));
            list.Add(new SqlBulkCopyColumnMapping("Phone", "Phone"));

            list.Add(new SqlBulkCopyColumnMapping("PhonePrefix", "PhonePrefix"));
            list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
            list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
            list.Add(new SqlBulkCopyColumnMapping("timelong", "Timestamp"));

            list.Add(new SqlBulkCopyColumnMapping("ACStatus", "ACStatus"));
            list.Add(new SqlBulkCopyColumnMapping("ServiceTakeTime", "ServiceTakeTime"));

            //入库
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.AutoCallSyncData.Instance.Holly_Business, "AutoCall_TaskInfo_Temp", 10000, list, out msg);
        }

        /// 从临时表更新-项目
        /// <summary>
        /// 从临时表更新-项目
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Project_XA()
        {
            return Dal.AutoCallSyncData.Instance.UpdateFromTemp_Project_XA();
        }
        /// 从临时表更新-任务
        /// <summary>
        /// 从临时表更新-任务
        /// </summary>
        /// <returns></returns>
        public int[] UpdateFromTemp_Task_XA()
        {
            return Dal.AutoCallSyncData.Instance.UpdateFromTemp_Task_XA();
        }

        /// 查询项目表暂停结束的项目
        /// <summary>
        /// 查询项目表暂停结束的项目
        /// </summary>
        /// <returns></returns>
        public DataTable GetStopAndPauseProject_XA(string status)
        {
            return Dal.AutoCallSyncData.Instance.GetStopAndPauseProject_XA(status);
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
            Dal.AutoCallSyncData.Instance.UpdateProjectStatusAndTimestamp_XA(projectid, status, timestamp);
        }

        /// 更新统计表
        /// <summary>
        /// 更新统计表
        /// </summary>
        /// <returns></returns>
        public int UpdateAutoCallProjectInfoStat_XA()
        {
            return Dal.AutoCallSyncData.Instance.UpdateAutoCallProjectInfoStat_XA();
        }

        /// 通知合力数据失败
        /// <summary>
        /// 通知合力数据失败
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        public void UpdateCallHollyResult_XA(long projectid, int result)
        {
            Dal.AutoCallSyncData.Instance.UpdateCallHollyResult_XA(projectid, result);
        }
        /// 获取失败的数据
        /// <summary>
        /// 获取失败的数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetFailureCallHollyResult_XA()
        {
            return Dal.AutoCallSyncData.Instance.GetFailureCallHollyResult_XA();
        }
        #endregion
    }
}
