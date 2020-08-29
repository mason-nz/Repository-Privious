using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
using XYAuto.ITSC.Chitunion2017.Entities;

namespace XYAuto.BUOC.Chitunion2018.CleanUser
{
    public class SyncService
    {
        // private System.Timers.Timer timer = null;
        private readonly int serviceInterval = int.Parse(ConfigurationManager.AppSettings["ServiceInterval"]);
        private readonly int OperateHour = ConfigurationManager.AppSettings["OperateHour"] == "" ? 23 : Convert.ToInt16(ConfigurationManager.AppSettings["OperateHour"]);
        private readonly int OperateMinute = ConfigurationManager.AppSettings["OperateMinute"] == "" ? 0 : Convert.ToInt16(ConfigurationManager.AppSettings["OperateMinute"]);
        public SyncService()
        {
            //timer = new System.Timers.Timer(serviceInterval * 1000) { AutoReset = true };
            //timer.Elapsed += (sender, eventArgs) => Run(sender, eventArgs);
            SyncTask();
        }

        public void Strat()
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("更新赤兔用户-----服务开始");
            //timer.Start();
        }

        public void Stop()
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("更新赤兔用户-----服务结束");
            //timer.Stop();
        }


        //public void Run(object obj, ElapsedEventArgs e)
        //{
        //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("更新赤兔用户-----Run......");
        //    //ITSC.Chitunion2017.BLL.Loger.Log4Net.Info(string.Format($"更新赤兔用户-----OperateHour:{OperateHour},OperateMinute:{OperateMinute}"));
        //    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info(string.Format($"更新赤兔用户-----Hour:{e.SignalTime.Hour},Minute:{e.SignalTime.Minute}"));
        //    //if (e.SignalTime.Hour == OperateHour && e.SignalTime.Minute == OperateMinute)
        //    SyncTask();
        //}
        public void SyncTask()
        {

            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("更新赤兔用户-----方法开始");
            try
            {
                InitListenToYiJiKeMessageQueue();
                //ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("更新赤兔用户-----方法结束");
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("更新赤兔用户出错:" + ex);
            }
        }
        #region 获取Leads侦听

        /// <summary>
        /// 开始侦听消息队列
        /// </summary>
        public static void InitListenToYiJiKeMessageQueue()
        {
            string QueueName = "";
            ITSC.Chitunion2017.BLL.LeadsTask.Instance.CreateQueue(false, out QueueName);
            //实例化MessageQueue,并指向现有的一个名称为TestQueue队列
            MessageQueue queue = new MessageQueue(QueueName);
            queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(UpdateUserId) });
            queue.ReceiveCompleted += new ReceiveCompletedEventHandler(queue_ReceiveCompleted);
            queue.BeginReceive();
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("等待消息");
            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }

        private static void queue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("执行消息");
            MessageQueue mq = (MessageQueue)sender;
            int OldUserId = 0;
            int NewUserId = 0;
            try
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("开始接受队列中的信息");
                Message m = mq.EndReceive(e.AsyncResult);
                UpdateUserId msgBody = (UpdateUserId)m.Body;
                OldUserId = msgBody.OldUserId;
                NewUserId = msgBody.NewUserId;
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("开始清洗，来源:" + msgBody.Source + " 新用户:" + NewUserId + "老用户：" + OldUserId);
                if (!(OldUserId > 0 && NewUserId > 0))
                {
                    ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("接受队列数值出错");
                }
                else
                {
                    if (ITSC.Chitunion2017.Dal.UserManage.UserManage.Instance.CleanUserRelation(OldUserId, NewUserId))
                    {
                        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("清洗用户成功，老用户:" + OldUserId + "新用户：" + NewUserId);
                    }
                    else
                    {
                        ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("清洗用户出错，老用户:" + OldUserId + "新用户：" + NewUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Info("清洗用户出错，老用户:" + OldUserId + "新用户：" + NewUserId + "错误信息：" + ex.StackTrace);
            }
            finally
            {
                mq.BeginReceive();
            }
            return;
        }

        #endregion
    }
}
