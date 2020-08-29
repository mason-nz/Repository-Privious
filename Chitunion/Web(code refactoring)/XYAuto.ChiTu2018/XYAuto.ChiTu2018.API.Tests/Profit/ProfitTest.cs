/********************************
* 项目名称 ：XYAuto.ChiTu2018.API.Tests.Profit
* 类 名 称 ：ProfitTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 15:39:45
********************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using XYAuto.ChiTu2018.Service.Profit;
using XYAuto.ChiTu2018.Service.Profit.Dto;

namespace XYAuto.ChiTu2018.API.Tests.Profit
{
    [TestClass]
    public class ProfitTest
    {
        /// <summary>
        /// 收益列表
        /// </summary>
        [TestMethod]
        public void ProfitListMethod()
        {
            bool t = true, b = false;

            var m = t == b;
            EntityFrameworkProfiler.Initialize();
            var ResultList = ProfitService.Instance.GetProfitList(new ProfitQueryDto { PageIndex = 1, PageSize = 30, UserID = 1316 });

            var JsonText = JsonConvert.SerializeObject(ResultList);

            

        }
    }
}
