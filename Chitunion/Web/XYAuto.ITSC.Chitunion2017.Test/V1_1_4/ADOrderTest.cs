using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto;
using XYAuto.ITSC.Chitunion2017.BLL;
using System.Collections.Generic;
using System.Linq;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;
using System.Net.Http;
using AutoMapper;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_4
{
    [TestClass]
    public class ADOrderTest
    {
        private ADOrderInfoV1_1Controller ctl = new ADOrderInfoV1_1Controller();
        private XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult ret = null;
        [TestMethod]
        public void AddOrderInfoTest()
        {
            RequestADOrderInfoDto reqDto = new RequestADOrderInfoDto();
            reqDto.optType = 1;
            RequestADOrderDto reqADOrder = new RequestADOrderDto() {
                OrderID = "CT20170622393",
                OrderName ="测试20170719-01hahahha",
                Status=16002,
                CustomerID= "gt86ZRCRjng%3d",
            };
            reqDto.ADOrderInfo = reqADOrder;

            List<RequestMediaOrderInfoDto> reqMediaOrderList = new List<RequestMediaOrderInfoDto>() {
                new RequestMediaOrderInfoDto() {
                    MediaType=14002,
                    Note="APP需求测试2",
                    UploadFileURL="adfadadfa"
                }
                ,
                new RequestMediaOrderInfoDto() {
                    MediaType=14001,
                    Note="wechat需求测试1",
                    UploadFileURL="adfadadfa"
                }
            };

            reqDto.MediaOrderInfos = reqMediaOrderList;

            List<RequestADDetailDto> reqADDetailList = new List<RequestADDetailDto>() {
                new RequestADDetailDto() {
                    MediaType=14001,
                    MediaID=16682,
                    PubDetailID=187342,
                    SaleAreaID=2,
                    CartID=5804,
                    ADScheduleInfos = new List<RequestADScheduleInfoDto>() {
                        new RequestADScheduleInfoDto() {
                            BeginData=DateTime.Today,
                            EndData =DateTime.Today.AddDays(1)
                        }
                    }
                }
                ,
                new RequestADDetailDto() {
                    MediaType=14001,
                    MediaID=16682,
                    PubDetailID=187342,
                    SaleAreaID=2,
                    CartID=5804,
                    ADScheduleInfos = new List<RequestADScheduleInfoDto>() {
                        new RequestADScheduleInfoDto() {
                            BeginData=DateTime.Today.AddDays(1),
                            EndData =DateTime.Today.AddDays(1)
                        }
                    }
                },
                new RequestADDetailDto() {
                    MediaType=14002,
                    MediaID=11,
                    PubDetailID=8121,
                    SaleAreaID=2501,
                    CartID=5801,
                    ADScheduleInfos = new List<RequestADScheduleInfoDto>() {
                        new RequestADScheduleInfoDto() {
                            BeginData=DateTime.Today.AddDays(1),
                            EndData =DateTime.Today.AddDays(10)
                        }
                    }
                }
                ,
                new RequestADDetailDto() {
                    MediaType=14002,
                    MediaID=11,
                    PubDetailID=8121,
                    SaleAreaID=2501,
                    CartID=5801,
                    ADScheduleInfos = new List<RequestADScheduleInfoDto>() {
                        new RequestADScheduleInfoDto() {
                            BeginData=DateTime.Today.AddDays(2),
                            EndData =DateTime.Today.AddDays(5)
                        }
                    }
                },
                //new RequestADDetailDto() {
                //    MediaType=14002,
                //    MediaID=521,
                //    PubDetailID=1080,
                //    SaleAreaID=1001,
                //    CartID=1,
                //    ADScheduleInfos = new List<RequestADScheduleInfoDto>() {
                //        new RequestADScheduleInfoDto() {
                //            BeginData=DateTime.Today.AddDays(1),
                //            EndData =DateTime.Today.AddDays(2)
                //        },                        
                //        new RequestADScheduleInfoDto() {
                //            BeginData=DateTime.Today.AddDays(3),
                //            EndData =DateTime.Today.AddDays(4)
                //        }
                //    }
                //}
            };
            reqDto.ADDetails = reqADDetailList;
            string orderid = string.Empty;
            string msg = string.Empty;
            BLL.ADOrderInfo.Instance.AddOrderInfo(reqDto, out orderid, out msg);           
            NUnit.Framework.Assert.AreEqual(string.Empty, msg);
        }
        [TestMethod]
        public void GetByOrderID_ADOrderInfoTest()
        {
            ret = ctl.GetByOrderID_ADOrderInfo("CT20170728802");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetBySubOrderID_ADOrderInfoTest()
        {
            ret = ctl.GetBySubOrderID_ADOrderInfo("CT2017073167901");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void QuerytAuditInfoTest()
        {
            ret = ctl.QuerytAuditInfo(1, 200);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void QueryAPPByNameTest()
        {
            ret = ctl.QueryAPPByName("百合", -2);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetADMasterTest()
        {
            ret = ctl.GetADMaster("测试", 2);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void GetTwoBarCodeHistory()
        {
            ret = ctl.GetTwoBarCodeHistory("CT20170720326");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void IntelligenceRecommend_PubQueryTest()
        {
            ret = ctl.IntelligenceRecommend_PubQuery(",,", new DateTime(2017, 7, 27), 2, -2);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void IntelligenceRecommendTest()
        {
            RequestIntelligenceRecommendDto reqDto = new RequestIntelligenceRecommendDto() {
                AreaInfo=new List<RequestIntelligenceRecommendAreaInfoDto>() {
                    new RequestIntelligenceRecommendAreaInfoDto() {
                        ProvinceID=27,
                        CityID=-2,
                        Budget=100000,
                        MediaCount=2,
                        OriginContain=true
                    }
                    //,
                    //new RequestIntelligenceRecommendAreaInfoDto() {
                    //    ProvinceID=1,
                    //    CityID=-2,
                    //    Budget=1000,
                    //    MediaCount=3
                    //},
                    //new RequestIntelligenceRecommendAreaInfoDto() {
                    //    ProvinceID=24,
                    //    CityID=-2,
                    //    Budget=1000,
                    //    MediaCount=3,
                    //    OriginContain=false
                    //}
                },
                LaunchTime= new DateTime(2017, 7, 27),
                OrderRemark= "60001,60002,60003,60004"
            };
            ret = ctl.IntelligenceRecommend(reqDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void IntelligenceADOrderInfoCrudTest()
        {
            RequestIntelligenceADOrderInfoCrudDto reqDto = new RequestIntelligenceADOrderInfoCrudDto()
            {
                optType = EnumIntelligenceADOrderInfoCrudOptType.ADDADOrderNote,
                ADOrderInfo = new RequestIntelligenceADOrderDto()
                {
                    OrderID = "CT20170731679",
                    OrderName = "智投项目修改",
                    Status = (int)EnumOrderStatus.PendingAudit,
                    CustomerID = "gt86ZRCRjng%3d",
                    MarketingPolices = "智投项目修改智投项目修改智投项目修改\n营销政策营销政策营销政策",
                    UploadFileURL = "上传物料",
                    LaunchTime = DateTime.Now,
                    CRMCustomerID= "1082773217",
                    CustomerText= "智投项目修改智投项目修改",
                    BudgetTotal=1000,
                    OrderRemark= "6001,6002",
                    MasterID=1,
                    BrandID=1,
                    SerialID=1,
                    MasterName= "Maste智投项目修改rName",
                    BrandName= "BrandN智投项目修改ame",
                    SerialName= "Serial智投项目修改Name",
                    JKEntrance=true,
                    Note= "Serial智投项目修改Name"
                },
                ADDetails=new List<RequestIntelligenceADOrderCityDto>() {
                    new RequestIntelligenceADOrderCityDto() {
                        ProvinceID=1,
                        CityID=102,
                        Budget=100,
                        MediaCount=3,
                        OriginContain=false,
                        PublishDetails=new List<RequestIntelligencePublishDetailDto>() {
                            new RequestIntelligencePublishDetailDto() {
                                PublishDetailID=187419,
                                MediaType=14001,
                                MediaID=8828,
                                AdjustPrice=9999,
                                EnableOriginPrice=true,
                                ChannelID=99,
                                CostReferencePrice=8888,
                                LaunchTime=DateTime.Now.Date
                            }
                        }
                    }
                }
            };
            ret = ctl.IntelligenceADOrderInfoCrud(reqDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void IntelligenceADOrderInfoQueryTest()
        {
            ret = ctl.IntelligenceADOrderInfoQuery("CT20170731679");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void IntelligenceTest()
        {
            BLL.V1_8_3.Dto.IRecommendExportDto exportDto = new BLL.V1_8_3.Dto.IRecommendExportDto()
            {
                MasterBrand = "奥迪",
                CarBrand = "一汽奥迪",
                CarSerial = "奥迪A8",
                BudgetTotal = 2000,
                LaunchTime = DateTime.Now.Date,
                JKEntrance = true
            };

            ResponseIntelligenceADOrderDto resDto = new ResponseIntelligenceADOrderDto() {
                MasterName="111111111",
                LaunchTime=new DateTime(2001,1,1),
                BudgetTotal=9999
            };
            Mapper.CreateMap<BLL.V1_8_3.Dto.IRecommendExportDto, ResponseIntelligenceADOrderDto>();
            var dest = Mapper.Map<ResponseIntelligenceADOrderDto>(exportDto);
            AutoMapper.ObjectDumper.Write(dest);
        }
        [TestMethod]
        public void IntelligenceRecommendExportTest()
        {
            BLL.V1_8_3.Dto.IRecommendExportDto exportDto = new BLL.V1_8_3.Dto.IRecommendExportDto()
            {
                MasterBrand = "奥迪",
                CarBrand = "一汽奥迪",
                CarSerial = "奥迪A8",
                BudgetTotal = 2000,
                LaunchTime = DateTime.Now.Date,
                JKEntrance = true,
                AreaInfo = new List<BLL.V1_8_3.Dto.IRecommendExportAreaInfoDto>() {
                    new BLL.V1_8_3.Dto.IRecommendExportAreaInfoDto() {
                        ProvinceName="河北",
                        CityName="保定",
                        PublishDetails=new List<BLL.V1_8_3.Dto.IRecommendExportPublishDetailDto>() {
                            new BLL.V1_8_3.Dto.IRecommendExportPublishDetailDto() {
                                MediaName="郑州全攻略",
                                MediaNumber="abcdefg",
                                ADPosition="多图文第二条",
                                CreateType="直发",
                                FansCount=39800100,
                                ADLaunchDays=1,
                                CostReferencePrice=2247,
                                OriginalReferencePrice=2000
                            },
                            new BLL.V1_8_3.Dto.IRecommendExportPublishDetailDto() {
                                MediaName="郑州全攻略",
                                MediaNumber="abcdefg",
                                ADPosition="多图文第二条",
                                CreateType="直发",
                                FansCount=39800100,
                                ADLaunchDays=1,
                                CostReferencePrice=2247,
                                OriginalReferencePrice=2000
                            }
                        }
                    },
                    new BLL.V1_8_3.Dto.IRecommendExportAreaInfoDto() {
                        ProvinceName="内蒙古",
                        CityName="呼伦贝尔",
                        PublishDetails=new List<BLL.V1_8_3.Dto.IRecommendExportPublishDetailDto>() {
                            new BLL.V1_8_3.Dto.IRecommendExportPublishDetailDto() {
                                MediaName="郑州全攻略",
                                MediaNumber="abcdefg",
                                ADPosition="多图文第二条",
                                CreateType="直发",
                                FansCount=39800100,
                                ADLaunchDays=1,
                                CostReferencePrice=2247,
                                OriginalReferencePrice=2000
                            },
                            new BLL.V1_8_3.Dto.IRecommendExportPublishDetailDto() {
                                MediaName="郑州全攻略",
                                MediaNumber="abcdefg",
                                ADPosition="多图文第二条",
                                CreateType="直发",
                                FansCount=39800100,
                                ADLaunchDays=1,
                                CostReferencePrice=2247,
                                OriginalReferencePrice=2000
                            }
                        }
                    }
                }
            };
            var ret = ctl.IntelligenceRecommendExport(exportDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetPolicyListTest()
        {
            BLL.Media.Business.V1_8.ChannelInfoProvider ch = new BLL.Media.Business.V1_8.ChannelInfoProvider();
            Entities.Query.Media.ChannelQuery<Entities.DTO.V1_1.RespChannelListDto> query = new Entities.Query.Media.ChannelQuery<Entities.DTO.V1_1.RespChannelListDto>() {
                ChannelId = 21,
                AdPosition1 = 6004,
                AdPosition2 = 7002,
                AdPosition3 = 8003,
                CooperateDate = DateTime.Now.Date.ToString()
            };
            decimal ret = ch.GetPolicyList(query);
            NUnit.Framework.Assert.AreEqual(0, 0);
        }
        [TestMethod]
        public void IntelligenceRecommend()
        {
            List<int[]> intarrayList = new List<int[]>() {
                new int[] { 0,1,2},
                new int[] { 0,1}
            };
            Stack<int> stack = new Stack<int>();
            P(intarrayList, stack);
            Console.WriteLine("***********************");
            Console.ReadKey();
        }
        #region 测试
        void P(List<int[]> list, Stack<int> stack)
        {

            if (stack.Count == list.Count)

            {

                // 打印结果

                Console.WriteLine(string.Join(",", stack.Select(x => x.ToString()).ToArray()));

            }

            else

            {

                int[] ints = list[stack.Count];

                foreach (int i in ints)

                {

                    stack.Push(i);

                    P(list, stack);

                    stack.Pop();

                }

            }

        }
        [TestMethod]
        public void TestLinqDateCompare()
        {
            List<RequestADScheduleInfoDto> listDate = new List<RequestADScheduleInfoDto>() {
                new RequestADScheduleInfoDto() {
                    BeginData=DateTime.Today,
                    EndData=DateTime.Today.AddDays(1)
                },
                new RequestADScheduleInfoDto() {
                    BeginData=DateTime.Today,
                    EndData=DateTime.Today.AddDays(2)
                },
                new RequestADScheduleInfoDto() {
                    BeginData=DateTime.Today.AddDays(3),
                    EndData=DateTime.Today.AddDays(5)
                }
                ,
                new RequestADScheduleInfoDto() {
                    BeginData=DateTime.Today.AddDays(-1),
                    EndData=DateTime.Today.AddDays(6)
                }
            };

            var query = from t in listDate
                        where t.BeginData > DateTime.Today
                        select t;

            foreach (var item in query)
            {
                Console.WriteLine(string.Format($"BeginData:{item.BeginData},EndData:{item.EndData}"));
            }
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            double aa = (dt - DateTime.Now.AddHours(1)).TotalDays;
            Console.WriteLine(dt.ToShortDateString());
            int ipos = 0;
            RequestADScheduleInfoDto preItem = new RequestADScheduleInfoDto();
            foreach (var item in listDate)
            {
                if (ipos == 0)
                {

                }

                var queryItem = from t in listDate
                                where t.BeginData != item.BeginData && t.EndData != item.EndData
                                && ((item.BeginData > t.BeginData && item.BeginData < t.EndData)
                                    ||
                                    (item.BeginData < t.BeginData && item.BeginData > t.EndData)
                                   )
                                select t;
                foreach (var item1 in queryItem)
                {
                    Console.WriteLine(string.Format($"BeginData:{item1.BeginData},EndData:{item1.EndData}"));
                }

                ipos++;
                preItem = item;
            }

            Console.ReadKey();
        }
        [TestMethod]
        public void Test()
        {
            int tmpint = 420;
            int itmp =
            tmpint == (int)Entities.EnumWeChatOperateMsg.刊例过期1天 ? 1 :
                                        tmpint == (int)Entities.EnumWeChatOperateMsg.刊例过期3天 ? 3 :
                                        tmpint == (int)Entities.EnumWeChatOperateMsg.刊例过期7天 ? 7 :
                                        tmpint == (int)Entities.EnumWeChatOperateMsg.刊例过期30天 ? 30 : 0;
            Assert.AreEqual(itmp, 30);
        }
        [TestMethod]
        public void QueryInterfacesTest()
        {
            // set up the interfaces to search for
            Type[] interfaces = {
                typeof(System.ICloneable),
                typeof(System.Collections.ICollection),
                typeof(System.IAppDomainSetup) };
            // set up the type to examine
            Type searchType = typeof(System.Collections.ArrayList);
            var matches = from t in searchType.GetInterfaces()
                          join s in interfaces on t equals s
                          select s;
            Console.WriteLine("Matches found:");
            foreach (Type match in matches)
                Console.WriteLine(match.ToString());
        }
        #endregion  
    }
}
