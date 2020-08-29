using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM2014.BLL;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM2014.Core
{
    /// <summary>
    /// 处理聊天记录线程
    /// </summary>
    public class MessageInDBThread
    {
        private object state = new object();
        //等待入库的消息实体集合
        private List<MessageInDB> waitMessageInDB = new List<MessageInDB>();
        //线程附属类，线程通过这个属性类与主线程关联
        private CometStateManager stateManager;
        //定义委托
        private delegate void DealWaitMessageInDBHandler(MessageInDB[] processRequest);
        //定义委托类型的事件
        private event DealWaitMessageInDBHandler DealWaitMessageInDBEvent;

        public List<MessageInDB> WaitMessageInDB
        {
            get { return this.waitMessageInDB; }
        }
        //如何启动线程
        public MessageInDBThread(CometStateManager stateManager)
        {
            //  get the state manager
            this.stateManager = stateManager;
            //线程启动方法
            Thread t = new Thread(new ThreadStart(QueueCometWaitRequest_WaitCallback));
            t.IsBackground = false;
            t.Start();
        }
        //给等待入库的聊天记录集合添加新的聊天记录对象，消息集合数量变化时要加锁
        internal void QueueMessageInDB(MessageInDB request)
        {
            lock (this.state)
            {
                waitMessageInDB.Add(request);
            }
        }
        private void QueueCometWaitRequest_WaitCallback()
        {
            //给事件加，事件处理方法
            DealWaitMessageInDBEvent += new DealWaitMessageInDBHandler(MessageInDBThread_DealWaitMessageInDBEvent);
            while (true)
            {
                MessageInDB[] processRequest;
                lock (this.state)
                {
                    processRequest = waitMessageInDB.ToArray();
                    //清除消息池数据
                    waitMessageInDB.Clear();
                }
                //TimeOut
                string TimeInDB = ConfigurationUtil.GetAppSettingValue("TimeInDB");
                if (string.IsNullOrEmpty(TimeInDB))
                {
                    Thread.Sleep(10000);
                }
                else
                {
                    int _timeindb = 0;
                    int.TryParse(TimeInDB, out _timeindb);
                    _timeindb = _timeindb * 1000;
                    Thread.Sleep(_timeindb);
                }
                if (processRequest.Length > 0)
                {
                    //调用事件
                    DealWaitMessageInDBEvent(processRequest);
                }
            }
        }
        //事件处理方法
        void MessageInDBThread_DealWaitMessageInDBEvent(MessageInDB[] processRequest)
        {
            DealWaitMessageInDB(processRequest);
        }
        /// <summary>
        /// 把聊天记录入库
        /// </summary>
        internal void DealWaitMessageInDB(MessageInDB[] processRequest)
        {
            BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入开始!");
            //批量入库
            DataTable ChatMessageLog_dt = new DataTable();
            ChatMessageLog_dt.Columns.Add("AllocID", typeof(long));
            ChatMessageLog_dt.Columns.Add("Sender", typeof(string));
            ChatMessageLog_dt.Columns.Add("Receiver", typeof(string));
            ChatMessageLog_dt.Columns.Add("Content", typeof(string));
            ChatMessageLog_dt.Columns.Add("Type", typeof(int));
            ChatMessageLog_dt.Columns.Add("Status", typeof(int));
            ChatMessageLog_dt.Columns.Add("CreateTime", typeof(DateTime));
            try
            {
                for (int i = 0; i < processRequest.Length; i++)
                {
                    DataRow row = ChatMessageLog_dt.NewRow();
                    MessageInDB request = processRequest[i];
                    Entities.ChatMessageLog model = new Entities.ChatMessageLog();
                    model.AllocID = request.AllocID;
                    row["AllocID"] = model.AllocID;
                    model.Content = request.Message;
                    row["Content"] = model.Content;
                    model.CreateTime = request.SendTime;
                    row["CreateTime"] = model.CreateTime;
                    model.Sender = request.MFrom;
                    row["Sender"] = model.Sender;
                    model.Receiver = request.MSendTo;
                    row["Receiver"] = model.Receiver;
                    model.Status = 0;
                    row["Status"] = model.Status;
                    model.Type = request.MessageType;
                    row["Type"] = request.MessageType;
                    ChatMessageLog_dt.Rows.Add(row);
                }
                IList<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
                list.Add(new SqlBulkCopyColumnMapping("AllocID", "AllocID"));
                list.Add(new SqlBulkCopyColumnMapping("Sender", "Sender"));
                list.Add(new SqlBulkCopyColumnMapping("Receiver", "Receiver"));
                list.Add(new SqlBulkCopyColumnMapping("Content", "Content"));
                list.Add(new SqlBulkCopyColumnMapping("Type", "Type"));
                list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
                list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
                string connections = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
                BLL.Util.BulkCopyToDB(ChatMessageLog_dt, connections, "ChatMessageLog", 10000, list);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入失败...错误Message:" + ex.Message);
                BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入失败...错误StackTrace:" + ex.StackTrace);
            }
            BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入结束!");
        }
    }
}
