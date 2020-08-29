/********************************************************
*创建人：lixiong
*创建时间：2017/8/23 10:03:49
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Threading;
using FluentScheduler;
using XYAuto.BUOC.BOP2017.Infrastruction;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Provider
{
    internal class SchedulerTest
    {
        public class DoFirst : IJob
        {
            public void Execute()
            {
                Console.WriteLine($"this is DoFirst start.now: {DateTime.Now}");
                Loger.Log4Net.Info($"this is DoFirst start.now: {DateTime.Now}");
                Thread.Sleep(3000);
                Console.WriteLine($"this is DoFirst completed.now: {DateTime.Now}");
                Loger.Log4Net.Info($"this is DoFirst completed.now: {DateTime.Now}");
            }
        }

        public class DoSecond : IJob
        {
            public void Execute()
            {
                Console.WriteLine($"this is DoSecond start.now: {DateTime.Now}");
                Loger.Log4Net.Info($"this is DoSecond start.now: {DateTime.Now}");
                Thread.Sleep(3000);
                Console.WriteLine($"this is DoSecond completed.now: {DateTime.Now}");
                Loger.Log4Net.Info($"this is DoSecond completed.now: {DateTime.Now}");
            }
        }
    }
}