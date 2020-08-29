using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using FundsmanagementPublisher.Client;
using FundsmanagementPublisher.Models;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;
using XYAuto.ChiTu2018.Service.LE;
using XYAuto.ChiTu2018.Service.LE.Query.Dto.Request;
using XYAuto.ChiTu2018.Settlement.Extensions;
using XYAuto.ChiTu2018.Settlement.ThirdApi;
using XYAuto.CTUtils.Email;

namespace XYAuto.ChiTu2018.Settlement.Scheduler.Registry
{
    /// <summary>
    /// 注释：SettlementKrPayScheduler
    /// 作者：lix
    /// 日期：2018/5/22 11:10:25
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class SettlementKrPayScheduler : IJob
    {
        public void Execute()
         {
            $" SettlementKrPayScheduler start ...".LogInfo();
            if (!CurrentRegistryScheduler.Configs.IsStart)
            {
                $" SettlementKrPayScheduler IsStart 是处于关闭状态，不进行操作 ...".LogInfo();
                return;
            }
            try
            {
                var config = CurrentRegistryScheduler.Configs;
                var startDate = DateTime.Now.AddDays(config.AtStartDateOffest).ToString("yyyy-MM-dd");
                var endDate = DateTime.Now.ToString("yyyy-MM-dd");
                var settlementPrices = new SettlementProvider().Settlement(startDate, endDate, config.SelectTopSize);
                var ourSettlementPrices = LeWithdrawalsDetailService.Instance.GetSettlement(startDate, endDate, WithdrawalsStatusEnum.已支付);
                //todo:发送邮件提示
                SendMailBody(settlementPrices, ourSettlementPrices, startDate, endDate);
            }
            catch (Exception exception)
            {
                CTUtils.Log.Log4NetHelper.Default().Error($" SettlementKrPayScheduler is error .:{exception.Message}," +
                     $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}");
            }
            $" SettlementKrPayScheduler completed ...".LogInfo();
        }

        private void SendMailBody(decimal apiSettlementPrices, decimal ourSettlementPrices, string startDate, string endDate)
        {
            var title = $"{startDate}至{endDate}(不包含)与库容支付对账情况!";
            var body = $" <h3>时间段：{startDate}至{endDate}(不包含)</h3><br/>" +
                       $"赤兔-资源管理系统支出总金额：<a style='color:red'>{ourSettlementPrices}</a> 元<br/>" +
                       $"库容系统支出总金额：<a style='color:red'>{apiSettlementPrices}</a> 元<br/><br/>";
            if (apiSettlementPrices != ourSettlementPrices)
            {
                title += $" <h3>对账有出入，请核查资金</h3>";
                body += $" <h3>资金有问题，请谨慎审核！！！</h3>";
            }
            else
            {
                title += $" <h3>对账正确</h3>";
                body += $" <h3>对账正确</h3>";
            }
            var userEmail = ConfigurationManager.AppSettings["ReceiveErrorEmail"].Split(';');
            if (userEmail.Length > 0)
            {
                EmailHelper.Instance.SendErrorMail(body, title, userEmail);
            }
        }

    }
}
