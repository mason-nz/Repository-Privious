/********************************************************
*创建人：lixiong
*创建时间：2017/9/26 11:04:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Configuration;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsDataTemp.Scheduler.Registry
{
    internal class EmailNotes
    {
        public static void Send(string name, string date)
        {
            var subject = $"ChiTuData2017.PullStatisticsData——数据拉取执行完成-{name}-{date}";
            var userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail.Length > 0)
            {
                var body = $"数据拉取执行完成!" +
                           $"类型：{name}" +
                           $"日期：{date}"
                           ;

                BLL.EmailHelper.Instance.SendErrorMail(body, subject, userEmail);
            }
        }

        public static void SendByLog(string name, string date)
        {
            var subject = $"ChiTuData2017.PullStatisticsData——数据拉取执行完成-{name}-{date}";
            var body = $"数据拉取执行完成!" +
                        $"类型：{name}" +
                        $"日期：{date}"
                        ;

            Loger.Log4Net.Error(subject + body);
        }
    }
}