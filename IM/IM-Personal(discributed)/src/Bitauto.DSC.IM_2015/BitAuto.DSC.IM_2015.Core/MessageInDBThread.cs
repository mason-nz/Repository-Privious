using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.BLL;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_2015.Core
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
            if (processRequest == null || processRequest.Length <= 0)
            {
                return;
            }

            //BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入开始!");
            //批量入库
            DataTable ConverSationDetail_dt = new DataTable();
            ConverSationDetail_dt.Columns.Add("CSID", typeof(int));
            ConverSationDetail_dt.Columns.Add("Sender", typeof(int));
            ConverSationDetail_dt.Columns.Add("Content", typeof(string));
            ConverSationDetail_dt.Columns.Add("Type", typeof(int));
            ConverSationDetail_dt.Columns.Add("Status", typeof(int));
            ConverSationDetail_dt.Columns.Add("CreateTime", typeof(DateTime));
            try
            {
                for (int i = 0; i < processRequest.Length; i++)
                {
                    DataRow row = ConverSationDetail_dt.NewRow();
                    MessageInDB request = processRequest[i];
                    Entities.ConverSationDetail model = new Entities.ConverSationDetail();
                    model.CSID = request.CSID;
                    row["CSID"] = model.CSID;
                    model.Sender = request.Sender;
                    row["Sender"] = model.Sender;
                    model.Content = request.Content;
                    row["Content"] = model.Content;
                    model.Type = request.Type;
                    row["Type"] = model.Type;
                    model.Status = 0;
                    row["Status"] = model.Status;
                    model.CreateTime = request.CreateTime;
                    row["CreateTime"] = model.CreateTime;
                    ConverSationDetail_dt.Rows.Add(row);
                }
                IList<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
                list.Add(new SqlBulkCopyColumnMapping("CSID", "CSID"));
                list.Add(new SqlBulkCopyColumnMapping("Sender", "Sender"));
                list.Add(new SqlBulkCopyColumnMapping("Content", "Content"));
                list.Add(new SqlBulkCopyColumnMapping("Type", "Type"));
                list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));
                list.Add(new SqlBulkCopyColumnMapping("CreateTime", "CreateTime"));
                string connections = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_IMDMS2014");
                string Tablename = BLL.ConverSationDetail.Instance.GetSationDetailName();
                BLL.Util.BulkCopyToDB(ConverSationDetail_dt, connections, Tablename, 10000, list);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入失败...错误Message:" + ex.Message);
                BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入失败...错误StackTrace:" + ex.StackTrace);
            }
            //BLL.Loger.Log4Net.Info("[MessageInDBThread]聊天记录插入结束!");
        }
    }
}
