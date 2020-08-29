/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 10:45:38
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Linq;
using FluentScheduler;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.GdtPullData.Scheduler;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Provider
{
    internal class GdtAccuntScheduler : IJob
    {
        public void Execute()
        {
            Loger.GdtLogger.Info($" GdtAccuntScheduler DoPullAccountUserInfo start..");

            var retValue = ProviderSingleton.GetInstance().DoPullAccountUserInfo();
            if (retValue.HasError)
            {
                Loger.GdtLogger.Error($"GdtAccuntScheduler DoPullAccountUserInfo is error,{ JsonConvert.SerializeObject(retValue)}");
                Loger.Log4Net.Error($"GdtAccuntScheduler DoPullAccountUserInfo is error,{JsonConvert.SerializeObject(retValue)}");
            }
            else
            {
                Loger.GdtLogger.Info($"GdtAccuntScheduler DoPullAccountUserInfo is completed,{retValue.Message}");
            }
            //清除缓存
            BLL.GDT.GdtAccountInfo.Instance.RemoveCacheKey();
        }

        public static List<int> GetAccountIds()
        {
            return BLL.GDT.GdtAccountInfo.Instance.GetList().Select(s => s.AccountId).ToList();
        }
    }
}