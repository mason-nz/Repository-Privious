/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 16:59:07
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
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Enum;

namespace XYAuto.BUOC.ChiTuData2017.Test.Statistics.TabProvider
{
    [TestClass]
    public class StatTabGrabProviderTest
    {
        public StatTabGrabProviderTest()
        {
            BLL.AutoMapperConfig.AutoMapperConfig.Configure();
        }

        [TestMethod]
        public void GetGrabDic()
        {
            var dic = new StatTabGrabProvider(20, "grab_head").GetGrabReturnDic();
            Console.WriteLine(JsonConvert.SerializeObject(dic));
            dic[GetGrabType.grab_head.ToString()] = new RespGrabHeadDto();
            Console.WriteLine(JsonConvert.SerializeObject(dic));
        }

        [Description("grab_head")]
        [TestMethod]
        public void GetGrabTest()
        {
            var respDic = new StatTabGrabProvider(7, "grab_head").GetGrabData();
            Console.WriteLine(JsonConvert.SerializeObject(respDic));
        }

        [Description("grab_head_qudao")]
        [TestMethod]
        public void GetGrabHeadQuDaoTest()
        {
            var respDic = new StatTabGrabProvider(7, GetGrabType.grab_head_qudao.ToString()).GetGrabData();
            Console.WriteLine(JsonConvert.SerializeObject(respDic));
        }

        [TestMethod]
        public void GetLatelyDays_Test()
        {
            //var provider = new DataViewProvider(7, "");

            //Console.WriteLine(JsonConvert.SerializeObject(list));

            var str = "1";

            //var list = DataViewProvider.GetLatelyDays(7);

            //Console.WriteLine(JsonConvert.SerializeObject(list));

            var resp = new DataViewProvider(7, "cxpp_body").GetGrabData();

            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        [TestMethod]
        public void GarbTest()
        {
            var dic = new StatTabGrabProvider(7, "grab_head,grab_body,grab_head_qudao,grab_body_qudao,grab_head_wzcj,grab_head_zhcj").GetGrabData();

            Console.WriteLine(JsonConvert.SerializeObject(dic));
        }
    }
}