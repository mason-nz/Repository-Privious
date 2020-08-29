/********************************************************
*创建人：lixiong
*创建时间：2017/9/26 11:04:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using XYAuto.BUOC.ChiTuData2017.BLL;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler.Registry
{
    internal class EmailNotes
    {
        public static void SendWarin(string date)
        {
            var subject = $"报警邮件！！！行圆慧分发物料数据异常！";
            var userEmail = ConfigurationManager.AppSettings["ReceiveWaringEmail"].Split(';');
            if (userEmail.Length > 0)
            {
                var body = $"您好~ 赤兔-数据分析系统在{DateTime.Now.ToString("F")}分统计到，行圆慧{date}的分发数据为0"
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

        public static void SendByMsg(int totalCount, int notDataCount)
        {
            var subject = $"赤兔-数据分析系统——数据拉取服务执行完成";
            var body = $"您好~ 赤兔-数据分析系统在 系统时间：{DateTime.Now} 数据拉取服务执行完成!" +
                        $"总共请求智慧云统计接口：{totalCount} 次" +
                        $"没有返回数据：{notDataCount} 次"
                        ;

            var userEmail = ConfigurationManager.AppSettings["ReceiveWaringEmail"].Split(';');
            if (userEmail.Length > 0)
            {
                BLL.EmailHelper.Instance.SendErrorMail(body, subject, userEmail);
            }
        }
    }
}