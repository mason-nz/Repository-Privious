/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 13:09:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.GdtPullData.Scheduler;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Provider
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
                var queryDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                var sratDate = queryDate;
                var endDate = queryDate;

                Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetReportDaily 日期：{queryDate} start..");
                var request = new ReqReportDto
                {
                    DateRange = new DateRangeDto() { StartDate = sratDate, EndDate = endDate },
                    Level = GdtLevelTypeEnum.ADGROUP,
                    GroupBy = new string[] { "adgroup_id" },
                    PageSize = 100
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
                Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetReportDaily 日期：{queryDate} end..");
            }
        }

        /// <summary>
        /// 获取小时报表
        /// </summary>
        public class DoPullGetReportHourly : IJob
        {
            public void Execute()
            {
                var request = new ReqReportDto()
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Level = GdtLevelTypeEnum.ADGROUP,
                    GroupBy = new string[] { "hour" },
                    PageSize = 100
                };
                Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetReportHourly 日期：{request.Date} start..");

                AccountList.ForEach(item =>
                {
                    try
                    {
                        var adgroups = BLL.GDT.GdtAccountInfo.Instance.GetAdGroupList(item, 0,
                            (int)SystemStatusEnum.有效);
                        adgroups.ForEach(t =>
                        {
                            request.AccountId = item;
                            request.AdGroupId = t.AdgroupId;
                            request.Filtering = new List<GdtFilteringDto>()
                            {
                                new GdtFilteringDto()
                                {
                                    Field = "adgroup_id",
                                    Operator = GdtOperatorEnum.EQUALS,
                                    Values = new string[] {t.AdgroupId.ToString()}
                                }
                            };
                            var retValue = ProviderSingleton.GetInstance().DoPullGetReportHourly(request);
                            if (retValue.HasError)
                            {
                                Loger.GdtLogger.Error($"GdtReportScheduler DoPullGetReportHourly is error,{JsonConvert.SerializeObject(retValue)}");
                            }
                            else
                            {
                                Loger.GdtLogger.Info($"GdtReportScheduler DoPullGetReportHourly is completed,{retValue.Message}");
                            }
                        });
                    }
                    catch (Exception exception)
                    {
                        var msg = $"GdtReportScheduler DoPullGetReportHourly is error:{exception.Message}," +
  $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                        Loger.Log4Net.Error(msg);
                    }
                });

                Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetReportHourly 日期：{request.Date} end..");

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
                var accountIds = BLL.GDT.GdtAccountInfo.Instance.GetAccountId(-1);
                var request = new ReqReportDto()
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Level = GdtLevelTypeEnum.ADGROUP,
                    GroupBy = new string[] { "hour" },
                };
                Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetZhyHourlyReport 日期:{request.Date} start..");
                if (accountIds.Count == 0)
                {
                    Loger.GdtLogger.Info($" GdtReportScheduler GetAccountId is null 日期:{request.Date} 终止执行..");
                    return;
                }
                accountIds.ForEach(item =>
                    {
                        if (item.AccountId <= 0) return;
                        request.AccountId = item.AccountId;
                        request.AdGroupId = item.AdgroupId;
                        request.Filtering = new List<GdtFilteringDto>()
                                {
                                    new GdtFilteringDto()
                                    {
                                        Field = "adgroup_id",
                                        Operator = GdtOperatorEnum.EQUALS,
                                        Values = new string[] {item.AdgroupId.ToString()}
                                    }
                                };
                        ProviderSingleton.GetInstance().DoPullGetZhyHourlyReport(request, item.DemandBillNo);
                    });
                Loger.GdtLogger.Info($" GdtReportScheduler DoPullGetZhyHourlyReport 日期:{request.Date} end..");
            }
        }
    }
}