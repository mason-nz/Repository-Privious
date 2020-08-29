using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Diagnostics;

namespace CC2015_HollyFormsApp
{
    public static class NetworkMonitoring
    {
        /// 日志方法
        /// <summary>
        /// 日志方法
        /// </summary>
        /// <param name="mes"></param>
        private static void ToLog(string mes)
        {
        }

        /// 获取服务器ip的延时（ms）
        /// <summary>
        /// 获取服务器ip的延时（ms）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long GetNetworkMonitoringMS(params string[] ips)
        {
            List<long> list = new List<long>();
            foreach (string ip in ips)
            {
                list.Add(GetNetworkMonitoringMS(ip));
            }
            return list.Max();
        }
        /// 获取服务器ip的延时（ms）
        /// <summary>
        /// 获取服务器ip的延时（ms）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static long GetNetworkMonitoringMS(string ip)
        {
            Ping objPingSender = new Ping();
            PingOptions objPinOptions = new PingOptions();
            objPinOptions.DontFragment = true;
            string data = "";
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            int intTimeout = 5 * 1000;
            PingReply objPinReply = objPingSender.Send(ip, intTimeout, buffer, objPinOptions);
            var strInfo = objPinReply.Status;
            if (strInfo == IPStatus.Success)
            {
                return objPinReply.RoundtripTime;
            }
            else
            {
                return intTimeout;
            }
        }
        /// 获取接口延时（ms）
        /// <summary>
        /// 获取接口延时（ms）
        /// </summary>
        /// <returns></returns>
        public static long GetNetworkMonitoringMS()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                AgentTimeStateHelper.Instance.GetCurrentTime();
                return (long)sw.Elapsed.TotalMilliseconds;
            }
            catch
            {
                return -1;
            }
            finally
            {
                sw.Stop();
            }
        }

        /// 获取图片
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static Image GetImage(long ms)
        {
            if (ms < 100)
            {
                return global::CC2015_HollyFormsApp.Properties.Resources.dot_green_d;
            }
            else if (ms < 1000)
            {
                return global::CC2015_HollyFormsApp.Properties.Resources.dot_yellow_d;
            }
            else
            {
                return global::CC2015_HollyFormsApp.Properties.Resources.dot_red_d;
            }
        }
    }
}
