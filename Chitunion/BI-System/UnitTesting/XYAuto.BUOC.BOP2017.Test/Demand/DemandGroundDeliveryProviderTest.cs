/********************************************************
*创建人：lixiong
*创建时间：2017/10/11 10:34:48
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.Demand.Resolver;
using XYAuto.BUOC.BOP2017.BLL.QueryPage.Entity;

namespace XYAuto.BUOC.BOP2017.Test.Demand
{
    [TestClass]
    public class DemandGroundDeliveryProviderTest
    {
        public DemandGroundDeliveryProviderTest()
        {
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
        }

        [Description("加参_添加")]
        [TestMethod]
        public void AddExcute_Test()
        {
            var retValue = new DemandGroundDeliveryProvider(new RequestGroundDeliveryDto()
            {
                DeliveryType = 95001,
                AdName = "加参关联1",
                AdCreative = 96001,
                DemandBillNo = 100045,
                GroundId = 5,
            }, null,
                 new ConfigEntity()).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void GetGroundDeliverys_Test()
        {
            var list = new DemandGroundDeliveryProvider(null, null, null).GetGroundDeliverys(100154);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}