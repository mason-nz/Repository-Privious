using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Core
{

    public static class BulkInserUserActionThread
    {
        private static string connections = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_IMDMS2014");
        private static Thread mainMonitorThread;
        //private static ConcurrentStack<UserActionLog> ListLogs = new ConcurrentStack<UserActionLog>();
        private static List<UserActionLog> ListLogs = new List<UserActionLog>();
        private static object _lock = new object();

        public static void EnQueueActionLogs(UserActionLog log)
        {
            lock (_lock)
            {
                ListLogs.Add(log);
            }
        }

        static BulkInserUserActionThread()
        {
            mainMonitorThread = new Thread(DoQueueWork);
            mainMonitorThread.Start();
        }

        static void DoQueueWork()
        {

            while (true)
            {
                try
                {

                    if (ListLogs.Count == 0)
                    {
                        Thread.Sleep(5000);
                        continue; ;
                    }


                    UserActionLog[] logs;
                    lock (_lock)
                    {
                        logs = ListLogs.ToArray();
                        ListLogs.Clear();
                    }
                    BulkInserUserLog(logs);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("BulkInserUserActionThread.DoQueueWork!方法报错: " + ex.Message);
                    Thread.Sleep(1000);
                }
            }

        }

        static void BulkInserUserLog(UserActionLog[] logs)
        {
            if (logs == null || logs.Length <= 0)
            {
                Thread.Sleep(5000);
                return;
            }

            //BLL.Loger.Log4Net.Info("[UserActionLog]聊天记录插入开始!");
            //批量入库
            DataTable dtUserActionLog = new DataTable();
            dtUserActionLog.Columns.Add("OperUserType", typeof(int));
            dtUserActionLog.Columns.Add("LogInType", typeof(int));
            dtUserActionLog.Columns.Add("LogInfo", typeof(string));
            dtUserActionLog.Columns.Add("IP", typeof(string));
            dtUserActionLog.Columns.Add("CreateTime", typeof(DateTime));
            dtUserActionLog.Columns.Add("CreateUserID", typeof(int));
            dtUserActionLog.Columns.Add("TrueName", typeof(string));

            try
            {
                for (int i = 0; i < logs.Length; i++)
                {
                    DataRow row = dtUserActionLog.NewRow();
                    UserActionLog log = logs[i];

                    row["OperUserType"] = log.OperUserType;
                    row["LogInType"] = log.LogInType;
                    row["LogInfo"] = log.LogInfo.Length <= 950 ? log.LogInfo : log.LogInfo.Substring(0, 950);
                    row["IP"] = log.IP;
                    row["CreateTime"] = log.CreateTime;
                    row["CreateUserID"] = log.CreateUserID;
                    row["TrueName"] = log.TrueName;

                    dtUserActionLog.Rows.Add(row);
                }
                IList<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
                list.Add(new SqlBulkCopyColumnMapping("OperUserType", "OperUserType"));
                list.Add(new SqlBulkCopyColumnMapping("LogInType", "LogInType"));
                list.Add(new SqlBulkCopyColumnMapping("LogInfo", "LogInfo"));
                list.Add(new SqlBulkCopyColumnMapping("IP", "IP"));
                list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
                list.Add(new SqlBulkCopyColumnMapping("CreateUserID", "CreateUserID"));
                list.Add(new SqlBulkCopyColumnMapping("TrueName", "TrueName"));

                BLL.Util.BulkCopyToDB(dtUserActionLog, connections, "UserActionLog", logs.Length + 100, list);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[UserActionLog]聊天记录插入失败...错误Message:" + ex.Message);
                BLL.Loger.Log4Net.Info("[UserActionLog]聊天记录插入失败...错误StackTrace:" + ex.StackTrace);
            }
            //BLL.Loger.Log4Net.Info("[UserActionLog]聊天记录插入结束!");
        }

    }

}
