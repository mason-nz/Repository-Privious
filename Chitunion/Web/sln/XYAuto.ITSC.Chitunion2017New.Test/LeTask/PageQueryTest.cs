using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.LeTask;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class PageQueryTest
    {
        public PageQueryTest()
        {
            MediaMapperConfig.Configure();
        }

        [Description("领取任务-贴片列表")]
        [TestMethod]
        public void TaskGetDistrbuteListTest()
        {
            var request = new ReqOrderCoverImageDto
            { TaskType = LeTaskTypeEnum.ContentDistribute };
            var list = new TaskRecCoverImageQuery(new ConfigEntity()).GetQueryList(request);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("订单管理-内容分发列表")]
        [TestMethod]
        public void OrderGetDistributeListTest()
        {
            var request = new ReqOrderCoverImageDto();
            request.UserId = 5;
            //request.OrderName = "5";
            var list = new OrderDistributeQuery(new ConfigEntity()).GetQueryList(request);


            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("订单管理-贴片订单列表")]
        [TestMethod]
        public void OrderCoverImageQueryTest()
        {
            var request = new ReqOrderCoverImageDto();
            request.UserId = 5;
            var list = new OrderCoverImageQuery(new ConfigEntity()).GetQueryList(request);


            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
        [Description("订单管理-订单详情")]
        [TestMethod]
        public void OrderGetOrderInfoTest()
        {
            var orderInfo = new OrderProvider().GetOrderInfo(7, 17);

            Console.WriteLine(JsonConvert.SerializeObject(orderInfo));
        }


        [Description("订单管理-收入详情列表")]
        [TestMethod]
        public void OrderGetIncomeDetails()
        {
            var orderId = 5;
            var info = new OrderProvider().GetOrderIncomeList(orderId,17);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("订单管理-收入统计")]
        [TestMethod]
        public void OrderGetIncomeInfoTest()
        {
            var userId = 1661;
            var info = new OrderProvider().GetIncomeInfo(userId);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("订单管理-提现明细列表")]
        [TestMethod]
        public void GetWithdrawalsListTest()
        {
            var request = new ReqInComeDto();
            request.UserId = 1;
            var list = new IncomeWithdrawalsQuery(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("订单管理-收入明细列表")]
        [TestMethod]
        public void GetIncomeListTest()
        {
            var request = new ReqInComeDto();
            request.UserId = 5;
            var list = new IncomeQuery(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("媒体主-个人中心-已绑定微信列表")]
        [TestMethod]
        public void GetBindingsWxListTest()
        {
            var request = new ReqMediaBindingsDto();
            request.UserId = 1421;
            var list = new BindingsWeiXinQuery(new ConfigEntity()).GetQueryList(request);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
        [Description("媒体主-个人中心-已绑定微信列表")]
        [TestMethod]
        public void GetBindingsWxInfoTest()
        {
            var info  = new BindingWeiXinProvider(new ConfigEntity(), null).GetInfo(200);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }
    }
}
