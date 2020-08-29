/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 16:49:34
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_8;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_11;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1.Materiel
{
    [TestClass]
    public class MaterielProviderTest
    {
        public MaterielProviderTest()
        {
            MediaMapperConfig.Configure();
        }

        [TestMethod]
        public void MaterielProvider_getInfo_test()
        {
            var provider = new MaterielProvider(new ConfigEntity(), new RequestMaterielGetInfoDto()
            {
                MaterielId = 3,
                IsGetChannelInfo = true
            });

            var info = provider.GetInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void MaterielProvider_GenPath_test()
        {
            var provider = new MaterielProvider(new ConfigEntity(), new RequestMaterielGetInfoDto()
            {
                MaterielId = 3
            });

            var fileName = new Random().Next(1, 99) + "_阅读原文.png";

            var tp = provider.GetPath(fileName);
            Console.WriteLine(JsonConvert.SerializeObject(tp));
        }

        [TestMethod]
        public void GenerateDownload_test()
        {
            var provider = new MaterielProvider(new ConfigEntity(), new RequestMaterielGetInfoDto()
            {
                MaterielId = 3,
                ChannelIds = "1,2,3"
            });
            //var tp = provider.GenerateDownload();

            var tp = new TwoBarCodeHistoryProvider(new RequestTwoBarCodeDto(), new ConfigEntity())
                  .GetCompressPath(@"D:\GitRoot\A5信息系统研发\销售业务管理平台\Chitunion\XYAuto.ITSC.Chitunion2017.Test\V1_1\UploadFiles\Materiel\Temp\",
                  new List<string> { "", "" }, "频繁加班视力变差，| 不怪电脑都怪它|美好测评.zip");

            Console.WriteLine(JsonConvert.SerializeObject(tp));
        }

        [TestMethod]
        public void GetList_test()
        {
            var list = new MaterielQuery(new ConfigEntity()).GetQueryList(new RequestMaterielQueryDto()
            {
                //MaterielName = "测试222"
                //ContractNumber = "d"
                //CarSerialId = 2775
                SatrtDate = "2017-08-14",
                EndDate = "2017-08-16"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void json_test()
        {
            var json = "{\"impression\":1000,\"click\":222}";

            var dto = JsonConvert.DeserializeObject<JsonTest>(json);
            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }

        [TestMethod]
        public void UrlEncodeTest()
        {
            var encode = @"
%e7%a1%ac%e6%b1%89%e5%86%9b%e5%9b%a2%e4%b8%8d%e5%80%92%e7%bf%81%e8%bd%a6%e9%98%9f+%e7%a9%bf%e8%b6%8a%e5%8e%9f%e5%a7%8b%e6%a3%ae%e6%9e%97%e7%ba%b3%e5%87%89";

            var name = System.Web.HttpUtility.UrlDecode(encode);
            Console.WriteLine(name);
        }
    }

    public class JsonTest
    {
        public int Impression { get; set; }

        public int Click { get; set; }
    }
}