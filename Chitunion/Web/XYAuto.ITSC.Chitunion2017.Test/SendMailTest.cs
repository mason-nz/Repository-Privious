using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    public class person
    {
        //Public int TotalNum { get{ lock(syncObj) return total; } set{ lock(syncObj) total++;} }
    }

    [TestClass]
    public class SendMailTest
    {
        [TestMethod]
        public void Test()
        {
            //Loger.Log4Net.Info("这是测试Log4Net！");
            //Loger.Log4Net.Error("这是测试Log4Net发邮件！");

            //var value = Utils.StringHelper.XSSFilter("SHI--");

            //value = Utils.StringHelper.SqlFilter("SHI --");
            //Console.WriteLine(value);
            var sp = string.Empty;
            Console.WriteLine(sp.Split(',').Length);

            sp = "2";
            Console.WriteLine(sp.Split(',').Length);

            sp = "2,";
            Console.WriteLine(sp.Split(',').Length);
        }

        [TestMethod]
        public void CityMappingTest()
        {
            //var value = CityMapping.IsMunicipality(201);
            //Console.WriteLine(value);

            // var str = XYAuto.Utils.StringHelper.SqlFilter("d-12 ");
            // Console.WriteLine(str);

            //var decode = HttpUtility.UrlDecode("+");
            //Console.WriteLine(decode);

            Console.Write((int)SexEnum.未知);

            var roleIds = new List<string>();
            //roleIds.Add("111");
            //roleIds.Add("222");
            //roleIds.Add("333");
            //Console.WriteLine("('" + string.Join("','", roleIds) + "')");

            //Console.WriteLine(new StringBuilder().AppendFormat("('{0}' )", string.Join("','", roleIds)) + "dddd");
            //Assert.IsFalse(value);
        }

        [TestMethod]
        public void Test1()
        {
            var dd = 1222234.000000;
            Console.WriteLine(dd.ToString("N"));

            var ss = dd.ToString("N");

            //Console.WriteLine(Decimal.Parse(ss));

            var dto = new ResponseFrontApp()
            {
                Price = 18889999.2222m,
            };
            Console.WriteLine(JsonConvert.SerializeObject(dto));

            dto.Price = 189.2222m;

            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }
    }
}