/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 13:48:22
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.Demand;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Response;
using XYAuto.BUOC.BOP2017.BLL.Demand.Resolver;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.Entities.Dto.Demand;

namespace XYAuto.BUOC.BOP2017.Test.Demand
{
    [TestClass]
    public class DemandJsonAnalysisTest
    {
        public static string GetStrTest => GetTest();

        //public static string GetStrTest2 = GetTest();
        public static string GetTest()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public DemandJsonAnalysisTest()
        {
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
        }

        [TestMethod]
        public void CarSerielAnalysis_Test()
        {
            var json =
                "[{\"BrandId\":84,\"BrandName\":\"一汽奔腾\",\"CarSerialInfo\":[{\"CarSerialId\":2316,\"CarSerialName\":\"奔腾B70\"}]}]";

            var demandBillNo = 100040;
            var carInfoDtos = JsonConvert.DeserializeObject<List<CarInfoDto>>(json);
            DemandJsonAnalysis.Instance.CarSerielAnalysis(carInfoDtos, demandBillNo);
        }

        [TestMethod]
        public void CitysAnalysis_Test()
        {
            var json =
                "{\"ProvinceId\":8,\"ProvinceName\":\"海南\",\"City\":[{\"CityId\":801,\"CityName\":\"海口\"},{\"CityId\":810,\"CityName\":\"琼北\"},{\"CityId\":802,\"CityName\":\"琼海\"},{\"CityId\":811,\"CityName\":\"琼南\"},{\"CityId\":803,\"CityName\":\"三亚\"},{\"CityId\":469000,\"CityName\":\"省直辖县级行政区划\"}]}";
            var demandBillNo = 100040;
            var areaInfoDto = JsonConvert.DeserializeObject<List<AreaInfoDto>>(json);
            DemandJsonAnalysis.Instance.CitysAnalysis(areaInfoDto, demandBillNo);
        }

        [TestMethod]
        public void AutoMapperTest()
        {
            BLL.AutoMapperConfig.MediaMapperConfig.Configure();
            var carInfos = new CarInfoDto()
            {
                BrandId = 1,
                BrandName = "北汽",
                CarSerialInfo = new List<CarserialInfoDto>()
                {
                    new CarserialInfoDto()
                    {
                        CarSerialId = 2049,
                        CarSerialName="勇士"
                    }
                }
            };

            var info = new RequestRechargeReceiptDto()
            {
                AccessToken = "ddddddddddddd"
            };
            //var gdtRechargeRelation = AutoMapper.Mapper.Map<RequestRechargeReceiptDto, Entities.GDT.GdtRechargeRelation>(info);
            //Console.WriteLine(JsonConvert.SerializeObject(gdtRechargeRelation));
            var dto = AutoMapper.Mapper.Map<CarInfoDto, DemandCarSerielDto>(carInfos);
            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }

        [TestMethod]
        public void DemandCarAndCitys_Test()
        {
            var dto = new RespDemandCarAndCitysDto()
            {
                DemandBillNo = 1,
                BrandInfos = new List<DeliveryBrandInfoDto>()
                {
                    new DeliveryBrandInfoDto()
                    {
                        BrandId = 192,
                        BrandName = "奥迪"
                    }
                },
                SerieInfos = new List<DeliveryCarInfoDto>()
                {
                    new DeliveryCarInfoDto()
                    {
                        BrandId = 192,
                        BrandName = "奥迪",
                        SerielId = 2633,
                        SerialName="奥迪R8（进口）"
                    },
                    new DeliveryCarInfoDto()
                    {
                        BrandId = 192,
                        BrandName = "奥迪",
                        SerielId = 2445,
                        SerialName="奥迪A8L（进口）"
                    }
                },
                ProvinceInfos = new List<DeliveryProvinceInfoDto>()
                {
                  new DeliveryProvinceInfoDto()
                  {
                      ProvinceId = 1,
                      ProvinceName = "安徽"
                  }
                },
                CitysInfos = new List<DeliveryCitysInfoDto>()
                {
                    new DeliveryCitysInfoDto()
                    {
                           ProvinceId = 1,
                      ProvinceName = "安徽",
                      CityId = 102,
                      CityName = "安庆"
                    }
                }
            };
            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }

        [TestMethod]
        public void GetDemandCarAndCityInfos_Test()
        {
            var resp = new DemandJsonAnalysis().GetDemandCarAndCityInfos(100045);
            Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        [TestMethod]
        public void Test_Test()
        {
            Console.WriteLine(GetStrTest);
            Thread.Sleep(1000);
            Console.WriteLine(GetStrTest);
            Thread.Sleep(1000);
            Console.WriteLine(GetStrTest);
        }

        [Description("清洗数据，")]
        [TestMethod]
        public void CleanData_Test()
        {
            DemandJsonAnalysis.Instance.CleanData();
        }

        [TestMethod]
        public void Role_Test()
        {
            var roleIds = "SYS005BUT3001,SYS005BUT3002,SYS005BUT3003,SYS005BUT3004,SYS005BUT3005,SYS005BUT3006";
            var AdYY = "SYS005RL00021";
            if (roleIds.IndexOf(AdYY, StringComparison.Ordinal) < 0)
            {
                Console.WriteLine("true");
            }
            else
            {
                Console.WriteLine("false");
            }
        }
    }
}