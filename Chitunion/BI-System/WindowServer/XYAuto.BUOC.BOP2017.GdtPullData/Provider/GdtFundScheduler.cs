/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 12:08:09
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.GdtPullData.AppSettings;
using XYAuto.BUOC.BOP2017.GdtPullData.Scheduler;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Provider
{
    internal class GdtFundScheduler
    {
        public readonly static PullDataSettings AppSettings = new PullDataSettings();

        private static readonly List<int> AccountList = GdtAccuntScheduler.GetAccountIds();

        /// <summary>
        /// 资金账户信息拉取入库,是否需要先获取所以的子客信息，然后一次轮询获取对应的账户信息
        /// </summary>
        public class DoPullGetFundsInfo : IJob
        {
            public void Execute()
            {
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetFundsInfo start..");
                AccountList.ForEach(item =>
                {
                    try
                    {
                        var retValue = ProviderSingleton.GetInstance().DoPullGetFundsInfo(item);
                        if (retValue.HasError)
                        {
                            Loger.GdtLogger.Error($"GdtFundScheduler DoPullGetFundsInfo is error,{retValue.Message}");
                        }
                        else
                        {
                            Loger.GdtLogger.Info($"GdtFundScheduler DoPullGetFundsInfo is completed,{retValue.Message}");
                        }
                    }
                    catch (Exception exception)
                    {
                        Loger.Log4Net.Error(exception.Message);
                    }
                });
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetFundsInfo end..");
            }
        }

        /// <summary>
        /// 获取今日消耗(定时每隔一个小时拉取，因为接口返回的是一条数据，不是流水，所以，我们自己得记录下来，形式一个报表)
        /// </summary>
        public class DoPullGetRealtimeCost : IJob
        {
            public void Execute()
            {
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetRealtimeCost start..");

                AccountList.ForEach(item =>
                {
                    try
                    {
                        var retValue = ProviderSingleton.GetInstance().DoPullGetRealtimeCost(new ReqReportDto
                        {
                            AccountId = item,
                            Date = DateTime.Now.ToString("yyyy-MM-dd"),
                            Level = GdtLevelTypeEnum.ADGROUP
                        });
                        if (retValue.HasError)
                        {
                            Loger.GdtLogger.Error($"GdtFundScheduler DoPullGetRealtimeCost is error,{retValue.Message}");
                        }
                        else
                        {
                            Loger.GdtLogger.Info($"GdtFundScheduler DoPullGetRealtimeCost is completed,{retValue.Message}");
                        }
                    }
                    catch (Exception exception)
                    {
                        var msg = $"GdtFundScheduler DoPullGetRealtimeCost is error:{exception.Message}," +
                $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                        Loger.Log4Net.Error(msg);
                    }
                });
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetRealtimeCost end..");
            }
        }

        /// <summary>
        /// 获取资金账户日结明细：查询的前一天的（用的是代理商的帐号查询，不是子客）
        /// </summary>
        public class DoPullGetFundStatementsDaily : IJob
        {
            public void Execute()
            {
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetFundStatementsDaily start..");
                var fundTypes = CurrentRegistryScheduler.AppSettings.GdtFundType;

                if (string.IsNullOrWhiteSpace(fundTypes))
                {
                    Loger.GdtLogger.Error($"DoPullGetFundStatementsDaily.获取资金账户日结明细 请配置账户类型：GdtFundType");
                    return;
                }

                var request = new ReqFundDto()
                {
                    Date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")
                };
                AccountList.ForEach(itemAccountId =>
                {
                    request.AccountId = itemAccountId;
                    var spFundTypes = fundTypes.Split(',');
                    foreach (var item in spFundTypes)
                    {
                        request.FundType = (GdtFundTypeEnum)Enum.Parse(typeof(GdtFundTypeEnum), item);
                        var retValue = ProviderSingleton.GetInstance().DoPullGetFundStatementsDaily(request);
                        if (retValue.HasError)
                        {
                            Loger.GdtLogger.Error($"GdtFundScheduler DoPullGetFundStatementsDaily is error,{JsonConvert.SerializeObject(retValue)}");
                        }
                        else
                        {
                            Loger.GdtLogger.Info($"GdtFundScheduler DoPullGetFundStatementsDaily is completed,{retValue.Message}");
                        }
                    }
                });
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetFundStatementsDaily end..");
            }
        }

        /// <summary>
        /// 获取资金账户流水:查询的是当天的
        /// </summary>
        public class DoPullGetFundStatementsDetailed : IJob
        {
            public void Execute()
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetFundStatementsDetailed 日期:{ date} start..");
                var request = new ReqFundDetaileDto
                {
                    FundType = GdtFundTypeEnum.BANK,
                    DateRange = new DateRangeDto() { EndDate = date, StartDate = date },
                };
                AccountList.ForEach(item =>
                {
                    request.AccountId = item;
                    var retValue = ProviderSingleton.GetInstance().DoPullGetFundStatementsDetailed(request);
                    if (retValue.HasError)
                    {
                        Loger.GdtLogger.Error($"GdtFundScheduler DoPullGetFundStatementsDetailed is error,{JsonConvert.SerializeObject(retValue)}");
                    }
                    else
                    {
                        Loger.GdtLogger.Info($"GdtFundScheduler DoPullGetFundStatementsDetailed is completed,{retValue.Message}");
                    }
                });

                Loger.GdtLogger.Info($" GdtFundScheduler DoPullGetFundStatementsDetailed  日期:{ date} end..");
            }
        }
    }
}