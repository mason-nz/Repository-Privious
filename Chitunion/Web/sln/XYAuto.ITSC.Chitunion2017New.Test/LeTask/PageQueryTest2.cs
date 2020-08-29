using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.ChannelStat;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.ChannelStat;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Task;
using XYAuto.ITSC.Chitunion2017.BLL.Query.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeTask;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Withdrawals;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using ReqWithdrawalsDto = XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Withdrawals.ReqWithdrawalsDto;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class PageQueryTest2
    {
        public PageQueryTest2()
        {
            MediaMapperConfig.Configure();
        }

        [Description("财务管理-媒体主收入记录列表")]
        [TestMethod]
        public void GetIncomeListTest()
        {
            var request = new ReqInComeByMediaOwnDto()
            {
                OrderType = (int)LeTaskTypeEnum.ContentDistribute,
                TaskId = "85",
                //OrderId = "1111111111",
                ChannelId = 101001
            };
            var list = new InComeByMediaOwnQuery(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("财务管理-提现管理列表")]
        [TestMethod]
        public void WithdrawalsListTest()
        {
            var request = new ReqWithdrawalsDto
            {
                //OrderStatus = (int)WithdrawalsStatusEnum.已支付
                AuditStatus = 197001
            };
            var list = new WithdrawalsQuery(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("财务管理-提现管理列表-提现详情")]
        [TestMethod]
        public void WithdrawalsGetInfoTest()
        {
            var request = new ReqWithdrawalsAuditDto
            {
                WithdrawalsId = 1
            };
            var info = new WithdrawalsProvider(new ConfigEntity(),
                new ReqWithdrawalsAgainDto()).GetWithdrawals(request.WithdrawalsId);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("财务管理-渠道月结数据列表")]
        [TestMethod]
        public void ChannelGetMonthlyList()
        {
            var request = new ReqChannelStatDto()
            {
                //SummaryDate = "-2"
            };

            var list = new ChannelStatMonthQuery(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("财务管理-渠道月结数据列表-汇总年月选择")]
        [TestMethod]
        public void GetStatMonthlySelectTest()
        {
            var list = new ChannelStatMonthProvider(new ConfigEntity(), new ReqChannelStatMonthPayDto())
                 .GetChannelStatMonths();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("H5任务列表")]
        [TestMethod]
        public void TaskH5QueryTest()
        {
            var request = new ReqTaskH5Dto();
            var list = new TaskH5Query(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}
