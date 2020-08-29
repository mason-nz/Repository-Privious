/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 13:09:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.GdtPullData.Scheduler;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData.Provider
{
    /// <summary>
    /// 报表相关
    /// </summary>
    internal class GdtReportScheduler
    {
        private static readonly List<int> AccountList = GdtAccuntScheduler.GetAccountIds();

        /// <summary>
        /// 获取日报表：查询前一天的
        /// </summary>
        public class DoPullGetReportDaily : IJob
        {
            public void Execute()
            {
                BLL.Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetReportDaily start..");

                var sratDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                var endDate = DateTime.Now.ToString("yyyy-MM-dd");

                var request = new ReqReportDto
                {
                    DateRange = new DateRangeDto() { StartDate = sratDate, EndDate = endDate },
                    Level = GdtLevelTypeEnum.ADVERTISER
                };

                AccountList.ForEach(item =>
                {
                    try
                    {
                        request.AccountId = item;
                        var retValue = ProviderSingleton.GetInstance().DoPullGetReportDaily(request);
                        if (retValue.HasError)
                        {
                            Loger.GdtLogger.Error($"GdtReportScheduler DoPullGetReportDaily is error,{JsonConvert.SerializeObject(retValue)}");
                        }
                        else
                        {
                            Loger.GdtLogger.Info($"GdtReportScheduler DoPullGetReportDaily is completed,{retValue.Message}");
                        }
                    }
                    catch (Exception exception)
                    {
                        var msg = $"GdtReportScheduler DoPullGetReportDaily is error:{exception.Message}," +
   $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                        Loger.Log4Net.Error(msg);
                    }
                });
            }
        }

        /// <summary>
        /// 获取小时报表
        /// </summary>
        public class DoPullGetReportHourly : IJob
        {
            public void Execute()
            {
                BLL.Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetReportHourly start..");

                var request = new ReqReportDto()
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Level = GdtLevelTypeEnum.ADVERTISER
                };
                AccountList.ForEach(item =>
                {
                    try
                    {
                        request.AccountId = item;
                        var retValue = ProviderSingleton.GetInstance().DoPullGetReportHourly(request);
                        if (retValue.HasError)
                        {
                            Loger.GdtLogger.Error($"GdtReportScheduler DoPullGetReportHourly is error,{JsonConvert.SerializeObject(retValue)}");
                        }
                        else
                        {
                            Loger.GdtLogger.Info($"GdtReportScheduler DoPullGetReportHourly is completed,{retValue.Message}");
                        }
                    }
                    catch (Exception exception)
                    {
                        var msg = $"GdtReportScheduler DoPullGetReportHourly is error:{exception.Message}," +
  $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                        Loger.Log4Net.Error(msg);
                    }
                });

                //小时报表完成之后，要产生给智慧云的小时报表数据，已智慧云需求单id为纬度
            }

            /// <summary>
            /// 小时报表完成之后，要产生给智慧云的小时报表数据，已智慧云需求单id为维度
            /// </summary>
            private void ExecuteZhyReport()
            {
                //todo:1.先找到需求对应的账户对应的子客信息
            }
        }

        /// <summary>
        /// 要产生给智慧云的小时报表数据，已智慧云需求单id为维度(广告组维度),注意：要在广告组任务完成之后才能执行，
        /// 因为现在是广告组维度去查询小时报表，需要根据账户id去查找广告组id
        /// </summary>
        public class DoPullGetZhyHourlyReport : IJob
        {
            public void Execute()
            {
                BLL.Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetZhyHourlyReport start..");
                var accountIds = BLL.GDT.GdtAccountInfo.Instance.GetAccountId(-1);
                var request = new ReqReportDto()
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Level = GdtLevelTypeEnum.ADGROUP,
                    GroupBy = new string[] { "hour" },
                };
                if (accountIds.Count == 0)
                {
                    BLL.Loger.GdtLogger.Info($" GdtReportScheduler GetAccountId is null 终止执行..");
                    return;
                }
                accountIds.ForEach(item =>
                    {
                        if (item.AccountId <= 0) return;
                        request.AccountId = item.AccountId;
                        ProviderSingleton.GetInstance().DoPullGetZhyHourlyReport(request, item.DemandBillNo);
                    });
                BLL.Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetZhyHourlyReport completed..");
            }
        }
    }
}