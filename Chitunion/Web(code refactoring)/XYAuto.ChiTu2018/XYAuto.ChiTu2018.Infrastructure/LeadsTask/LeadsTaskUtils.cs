using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Messaging.MessageQueue;

namespace XYAuto.ChiTu2018.Infrastructure.LeadsTask
{
    /// <summary>
    /// 注释：LeadsTask
    /// 作者：zhanglb
    /// 日期：2018/5/16 18:15:20
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LeadsTaskUtils
    {
        public static readonly LeadsTaskUtils Instance = new LeadsTaskUtils();

        /// 创建队列 
        /// <summary>
        /// 创建队列 
        /// </summary>
        /// <param name="transactional">是否启用事务</param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public void CreateQueue(bool transactional, out string queueName)
        {
            queueName = @".\private$\" + (CTUtils.Config.ConfigurationUtil.GetAppSettingValue("MSMQName", "ct_userinfo"));
            if (!Exists(queueName))
            {
                Create(queueName, transactional);
            }
        }
    }
}
