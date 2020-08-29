using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class LeadsTask
    {
        public static readonly LeadsTask Instance = new LeadsTask();
        /// 创建队列 
        /// <summary>
        /// 创建队列 
        /// </summary>
        /// <param name="transactional">是否启用事务</param>
        /// <returns></returns>
        public bool CreateQueue(bool transactional, out string QueueName)
        {
            QueueName = @".\private$\" + (ConfigurationManager.AppSettings["MSMQName"] ?? "ct_userinfo");

            if (MessageQueue.Exists(QueueName))
            {
                return true;
            }
            else
            {
                if (MessageQueue.Create(QueueName, transactional) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
