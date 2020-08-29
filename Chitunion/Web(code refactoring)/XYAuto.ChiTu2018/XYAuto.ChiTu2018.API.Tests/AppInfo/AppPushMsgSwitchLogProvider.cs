using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request;

namespace XYAuto.ChiTu2018.API.Tests.AppInfo
{
    /// <summary>
    /// 注释：AppPushMsgSwitchLogProvider
    /// 作者：lix
    /// 日期：2018/6/5 11:13:44
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class AppPushMsgSwitchLogProvider
    {
        public AppPushMsgSwitchLogProvider()
        {
            EntityFrameworkProfiler.Initialize();
        }

        [TestMethod]
        public void GetPushConfig()
        {
            Service.App.AppInfo.Provider.AppPushMsgSwitchLogProvider provider = new Service.App.AppInfo.Provider.AppPushMsgSwitchLogProvider(
                new ReqAppPushSwitchDto()
                {
                    DeviceId = "34556787654"
                });
            var resp = provider.GetPushConfig();
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        [TestMethod]
        public void SetPushConfig()
        {
            var request = new ReqAppPushSwitchDto()
            {
                DeviceId = "34556787654"
            };
            var resp = new Service.App.AppInfo.Provider.AppPushMsgSwitchLogProvider(request).SetPushConfig();
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }
        [TestMethod]
        public void UpdateClosed()
        {
            var request = new ReqAppPushSwitchDto()
            {
                DeviceId = "34556787654"
            };
            var resp = new Service.App.AppInfo.Provider.AppPushMsgSwitchLogProvider(request).ClosedPushTips();
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }
    }
}
