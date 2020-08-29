/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 12:17:37
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
    /// <summary>
    /// 广告相关
    /// </summary>
    internal class GdtAdsScheduler
    {
        public readonly static PullDataSettings AppSettings = new PullDataSettings();

        private static readonly List<int> AccountList = GdtAccuntScheduler.GetAccountIds();

        /// <summary>
        ///  获取广告组列表(因为会修改，所以，必须从头开始拉取数据)
        /// </summary>
        public class DoPullGetAdGroupList : IJob
        {
            public void Execute()
            {
                Loger.GdtLogger.Info($" GdtAdsScheduler DoPullGetAdGroupList start..");

                AccountList.ForEach(Do);
            }

            private void Do(int accountId)
            {
                try
                {
                    var retValue = ProviderSingleton.GetInstance().DoPullGetAdGroupList(new ReqReportDto()
                    {
                        AccountId = accountId,
                        Filtering = new List<GdtFilteringDto>()
                       {
                           new GdtFilteringDto()
                           {
                               Field = "system_status",
                               Operator = GdtOperatorEnum.EQUALS,
                               Values = new string[] { GdtSystemStatusEnum.AD_STATUS_NORMAL.ToString()}
                           }
                       }
                    });
                    if (retValue.HasError)
                    {
                        Loger.GdtLogger.Error($"GdtAdsScheduler DoPullGetAdGroupList is error,{JsonConvert.SerializeObject(retValue)}");
                        Loger.Log4Net.Error($"GdtAdsScheduler DoPullGetAdGroupList is error,{JsonConvert.SerializeObject(retValue)}");
                    }
                    else
                    {
                        Loger.GdtLogger.Info($"GdtAdsScheduler DoPullGetAdGroupList is completed,{retValue.Message}");
                    }
                }
                catch (Exception exception)
                {
                    var msg = $"GdtAdsScheduler DoPullGetAdGroupList is error:{exception.Message}," +
                        $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                    Loger.Log4Net.Error(msg);
                }
            }
        }

        /// <summary>
        /// 获取推广计划
        /// </summary>
        public class DoPullGetAdCampaignsList : IJob
        {
            public void Execute()
            {
                Loger.GdtLogger.Info($" GdtAdsScheduler DoPullGetAdCampaignsList start..");
                var request = new ReqReportDto()
                {
                };
                AccountList.ForEach(item =>
                {
                    try
                    {
                        request.AccountId = item;
                        var retValue = ProviderSingleton.GetInstance().DoPullGetAdCampaignsList(request);
                        if (retValue.HasError)
                        {
                            Loger.GdtLogger.Error($"GdtAdsScheduler DoPullGetAdCampaignsList is error,{retValue.Message}");
                        }
                        else
                        {
                            Loger.GdtLogger.Info($"GdtAdsScheduler DoPullGetAdCampaignsList is completed,{retValue.Message}");
                        }
                    }
                    catch (Exception exception)
                    {
                        var msg = $"GdtAdsScheduler DoPullGetAdCampaignsList is error:{exception.Message}," +
                        $"{System.Environment.NewLine}{exception.StackTrace ?? string.Empty}";
                        Loger.Log4Net.Error(msg);
                    }
                });
            }
        }
    }
}