/********************************************************
*创建人：lixiong
*创建时间：2017/6/3 11:07:54
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1.AdTemplate
{
    [TestClass]
    public class AppProxyTest
    {
        public AppProxyTest()
        {
            MediaMapperConfig.Configure(); //配置autpMapper
        }

        [TestMethod]
        public void app_returnJson_test()
        {
            var returnDto = new RespAppItemDto
            {
                Name = "app_test",
                CityId = 1001,
                CityName = "郑州市",
                ProvinceId = 10,
                ProvinceName = "河南省",
                CommonlyClass = new List<CommonlyClassDto>() { new CommonlyClassDto()
                {
                    CategoryId = 111,
                    CategoryName = "test_categroy"
                } , new CommonlyClassDto()
                {
                    CategoryId = 1121,
                    CategoryName = "test_categroy_1"
                }},
                CoverageArea = new List<CoverageAreaDto>() { new CoverageAreaDto()
                {
                   CityId = 1001,
                CityName = "郑州市",
                ProvinceId = 10,
                ProvinceName = "河南省"
                } },
                DailyLive = 10088,
                HeadIconURL = "/sss/image",
                MediaID = 18,
                Remark = "ddddddd",
                OrderRemark = new List<OrderRemarkDto>()
                {
                    new OrderRemarkDto() { Id = 10, Name = "d_name",Descript = "描述"}
                }
            };
            Console.WriteLine(JsonConvert.SerializeObject(returnDto));
        }

        [TestMethod]
        public void app_template_returnJson_test()
        {
            var returnDto = new RespAdTemplateItemDto()
            {
                AdDescription = "广告模板说明、描述",
                AdDisplay = "广告展示逻辑",
                AdDisplayLength = 4,
                AdForm = 4,
                AdLegendURL = "/image/dddd",
                AdTemplateName = "名称",
                AdTempStyle = new List<AdTempStyleDto>() { new AdTempStyleDto()
                {
                    AdStyle = "style_1",
                    AdTemplateID = 1,
                    BaseMediaID = 1
                } },
                BaseMediaID = 1,
                BaseAdID = 1,
                Remarks = "描述",
                SellingMode = 4,
                SellingPlatform = 3,
                CarouselCount = 7,
                OriginalFile = "刊例原件",
                AdSaleAreaGroup = new List<AdSaleAreaGroupDto>() { new AdSaleAreaGroupDto()
                {
                    DetailArea = new List<AdSaleAreaGroupDetailDto>()
                    {
                         new AdSaleAreaGroupDetailDto()
                         {
                             CityId = 1001,
                             CityName = "郑州市",
                             ProvinceId = 10,
                             ProvinceName = "河南省"
                         },
                         new AdSaleAreaGroupDetailDto()
                         {
                             CityId = 1002,
                             CityName = "洛阳市",
                             ProvinceId = 10,
                             ProvinceName = "河南省"
                         }
                    },
                    GroupId = 11,
                    GroupName = "城市组1"
                } }
            };
            Console.WriteLine(JsonConvert.SerializeObject(returnDto));
        }

        [TestMethod]
        public void app_getinfo_MapperToCoverageArea_test()
        {
            //var appOperate = new AppOperate(new ConfigEntity(), new RequestAppInfoDto());
            //var list = AppOperate.MapperToCoverageArea("0,全国@=|1,安徽省@=101,合肥市|1,安徽省@=102,安庆市|1,安徽省@=103,蚌埠市");
            //Console.WriteLine(JsonConvert.SerializeObject(list));

            var list = AppOperate.MapperToCoverageArea("0,全国@=");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void app_getinfo_auto_mapping_MapperToCoverageArea_test()
        {
            var entity = new Entities.Media.MediaPcApp
            {
                Name = "test_1",
                AreaMapping = "0,全国@=|1,安徽省@=101,合肥市|1,安徽省@=102,安庆市|1,安徽省@=103,蚌埠市",
                ProvinceID = 201,
                ProvinceName = "beijing",
                OrderRemarkStr = "40001,不接竞品,|40002,不接硬广,|40009,其他,订单备注其他其他",
                CommonlyClassStr = "47004,科技,1|47004,科技,0|47009,教育,0"
            };
            var dto = Mapper.Map<Entities.Media.MediaPcApp, RespAppItemDto>(entity);

            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }

        [TestMethod]
        public void app_getinfo_MediaOperateProxy_test()
        {
            var requestDto = new RequestGetMeidaInfoDto()
            {
                MediaId = 74,
                BusinessType = (int)MediaType.APP,
            };
            var info = new MediaOperateProxy(requestDto, new ConfigEntity()
            {
            }).QueryInfo();
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void app_getinfo_base_MediaOperateProxy_test()
        {
            var requestDto = new RequestGetMeidaInfoDto()
            {
                BaseMediaId = 1,
                BusinessType = (int)MediaType.APP,
            };
            var info = new MediaOperateProxy(requestDto, new ConfigEntity()
            {
            }).QueryInfo();
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void app_get_back_query_test()
        {
            var data = new RespMediaAppDto
            {
                AuditStatus = 43001,
                AuditUser = "admin",
                BaseMediaID = 1,
                CityName = "beijing",
                CommonlyClass = new List<CommonlyClassDto>() { new CommonlyClassDto()
                {
                    CategoryId = 111,
                    CategoryName = "test_categroy"
                } , new CommonlyClassDto()
                {
                    CategoryId = 1121,
                    CategoryName = "test_categroy_1"
                }},
                CoverageArea = new List<CoverageAreaDto>() { new CoverageAreaDto()
                {
                   CityId = 1001,
                CityName = "郑州市",
                ProvinceId = 10,
                ProvinceName = "河南省"
                } },
                OtherAdCount = 29,
                CreateTime = DateTime.Now,
                CreateUser = "13111111111",
                //CreateUserRole = "媒体主",
                MediaID = 1,
                MediaRelationsName = "代理",
                OperatingTypeName = "个人",
                DailyLive = 1999,
                TrueName = "媒体主（人名、公司名称）",
                HeadIconURL = "/logo.jpg",
                Name = "微信名称",
                HasOnPub = 1
            };

            Console.WriteLine(JsonConvert.SerializeObject(data));
        }

        [TestMethod]
        public void app_MapperToAdSaleAreaGroup_tesst()
        {
            var temp =
                "1,城市组1,1,1$=10,河南省@=1001,郑州市" +
                "|1,城市组1,1,1$=10,河南省@=1002,洛阳市" +
                "|1,城市组1,1,1$=10,河南省@=1003,周口市" +
                "|2,城市组2,2,0$=1,安徽省@=101,合肥市" +
                "|2,城市组2,2,0$=1,安徽省@=102,安庆市" +
                "|2,城市组2,2,0$=1,安徽省@=103,蚌埠市";

            temp = "1,全国,0,0$=";

            var list = AdTemplateProvider.MapperToAdSaleAreaGroup(temp);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void list_Except_test()
        {
            var list1 = new List<int>() { 1, 5, 6 };
            var list2 = new List<int>() { 1, 3, 5, 6, 7, 8, 9 };
            var list3 = new List<int>();
            var ls = list1.Except(list2);
            Console.WriteLine(JsonConvert.SerializeObject(ls));
            var ls1 = list2.Except(list1);
            Console.WriteLine(JsonConvert.SerializeObject(ls1));
            var ls3 = list2.Except(list3);
            Console.WriteLine(JsonConvert.SerializeObject(ls3));

            //list1.ForEach(s =>
            //{
            //    if (s == 5) return;
            //    Console.WriteLine(s);
            //});
        }

        [TestMethod]
        public void app_media_list_test()
        {
            var resultList = new List<RespMediaAppDto>
            {
                new RespMediaAppDto() {AdTemplateId = 1, CommonlyClassStr = "47004,科技,1|47004,科技,0|47009,教育,0"}
            };

            resultList.ForEach(s =>
            {
                s.CommonlyClass = AppOperate.MapperToCommonlyClass(s.CommonlyClassStr);
            });

            Console.WriteLine(JsonConvert.SerializeObject(resultList));
        }

        [TestMethod]
        public void app_media_list_audit_pass_test()
        {
        }

        [TestMethod]
        public void app_task_test()
        {
            Task.Factory.StartNew(() =>
            {
                int intI = 0;
                for (int i = 0; i < 10000; i++)
                {
                    intI++;
                }
                Console.WriteLine(string.Format("执行完毕1：{0}", intI));
            })
            .ContinueWith(s =>
            {
                int intI = 0;
                for (int i = 0; i < 10000; i++)
                {
                    intI++;
                }
                Console.WriteLine(string.Format("执行完毕2：{0}", intI));
            })
             .ContinueWith(s =>
             {
                 int intI = 0;
                 for (int i = 0; i < 10000; i++)
                 {
                     intI++;
                 }
                 Console.WriteLine(string.Format("执行完毕2：{0}", intI));
             });

            Console.WriteLine("success");
        }

        [TestMethod]
        public void media_get_entity_test()
        {
            var retValue = new ReturnValue()
            {
                ReturnObject = new Entities.Media.MediaPcApp
                {
                    AdTemplateId = 1,
                    ADCount = 1,
                    Name = "test"
                }
            };
            var info = new CurrentOperateBase().GetEntity<Entities.Media.MediaPcApp>(retValue);
            Console.WriteLine(JsonConvert.SerializeObject(info));
            Assert.IsTrue(info != null);
        }

        [TestMethod]
        public void list_insert_test()
        {
            var list = new List<string> { "1111", "2222" };

            list.Insert(0, "000");

            Console.WriteLine(JsonConvert.SerializeObject(list));
            list = new List<string>();

            list.Insert(0, "9999");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}