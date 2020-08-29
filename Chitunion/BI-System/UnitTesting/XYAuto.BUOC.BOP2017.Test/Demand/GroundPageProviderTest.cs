/********************************************************
*创建人：lixiong
*创建时间：2017/10/11 9:59:36
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
    public class GroundPageProviderTest
    {
        [Description("落地页添加")]
        [TestMethod]
        public void ExcutePage_Test()
        {
            var retValue = new GroundPageProvider(new RequestGroundPageDto()
            {
                DemandBillNo = 100045,
                BrandId = 8,
                SerielId = 2061,
                CityId = 102,
                ProvinceId = 1,
                PromotionUrl = "http://sys1.chitunion.com/SystemManager/ShowAllSys.aspx?"
            }, null, new ConfigEntity()
            {
                CreateUserId = 1290
            }).ExcutePage();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取落地页列表")]
        [TestMethod]
        public void GetGroundPages_Test()
        {
            var list = new GroundPageProvider(null, null, new ConfigEntity()).GetGroundPages(100145);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("删除落地页")]
        [TestMethod]
        public void Delete_Test()
        {
            var retValue = new GroundPageProvider(null, new RequestDeletePageDto()
            {
                GroundId = 3,
                DemandBillNo = 100040
            }, new ConfigEntity()).Delete();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }
    }
}