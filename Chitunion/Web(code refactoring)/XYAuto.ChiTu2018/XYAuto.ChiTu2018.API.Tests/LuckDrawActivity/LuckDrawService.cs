using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.API.Tests.LuckDrawActivity
{
    /// <summary>
    /// 注释：LuckDrawService
    /// 作者：lix
    /// 日期：2018/6/12 13:59:21
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    [TestClass]
    public class LuckDrawService : BaseTest
    {
        [TestMethod]
        public void GetSumDrawInfo()
        {
            var info = Service.LuckDrawActivity.LuckDrawService.Instance.GetSumDrawInfo(3);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("获取剩余抽奖次数")]
        [TestMethod]
        public void GetDrawRemainder()
        {
            var info = Service.LuckDrawActivity.LuckDrawService.Instance.GetDrawRemainder(1);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }
        [Description("获取获奖名单(假数据)")]
        [TestMethod]
        public void GetAwardeeMoniList()
        {
            var info = Service.LuckDrawActivity.LuckDrawService.Instance.GetAwardeeMoniList();
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }
        [TestMethod]
        public void To()
        {
            var list = new List<ProductDto>()
            {
                new ProductDto() { Id = 1, Name = "1"},
                new ProductDto() { Id = 2, Name = "2"}
            };
            var idc = list.ToDictionary(s => s.Id, s => s.Name);
            Console.WriteLine(JsonConvert.SerializeObject(idc));
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
