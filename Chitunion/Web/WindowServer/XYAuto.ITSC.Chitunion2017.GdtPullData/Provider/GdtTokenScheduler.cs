/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 16:59:26
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
using XYAuto.ITSC.Chitunion2017.BLL.Media.Support;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.GdtPullData.Scheduler;

namespace XYAuto.ITSC.Chitunion2017.GdtPullData.Provider
{
    /// <summary>
    /// 拉取 获取 Access Token 或刷新 Access Token
    /// </summary>
    internal class GdtTokenScheduler : IJob
    {
        public void Execute()
        {
            BLL.Loger.GdtLogger.Info($" GdtTokenScheduler start..");
            //获取广点通信息
            var gdtAppInfo = BLL.GDT.GdtAccessToken.Instance.GetInfo((int)AuditRelationTypeEnum.Gdt,
                  CurrentRegistryScheduler.AppSettings.GdtClientId);

            var retValue = ProviderSingleton.GetInstance().DoPullGetAccessTokenByRefreshToken(gdtAppInfo);
            if (retValue.HasError)
            {
                Loger.Log4Net.Error($" GdtTokenScheduler is error,{JsonConvert.SerializeObject(retValue)}");
                Loger.GdtLogger.Error($" GdtTokenScheduler is error,{JsonConvert.SerializeObject(retValue)}");
            }
            else
            {
                Loger.GdtLogger.Info($" GdtTokenScheduler is completed,{retValue.Message}");
            }
            //todo:清除获取Token的缓存
            CacheHelper<string>.RemoveAllCache($"gdt_access_token");
        }
    }
}