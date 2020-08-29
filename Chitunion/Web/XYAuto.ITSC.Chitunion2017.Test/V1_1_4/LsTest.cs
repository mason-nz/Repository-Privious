using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.Entities;


namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_4
{
    [TestClass]
    public class LsTest
    {
        [TestMethod]
        public void GetAppQualification() {
            //mtz
            var res = MediaInfo.Instance.GetAppQualificationInfo(453, 14002);
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void GetADDetail() {
            //mtz
            var res = MediaInfo.Instance.GetAppADItem(14002, 30877, 0,0);
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void GetADListB() {
            var res = MediaInfo.Instance.GetAppADListB(new Entities.DTO.GetADListBReqDTO() { MediaType = 14002, PageIndex = 1, PageSize =5 });
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void GetADListF() {
            var res = MediaInfo.Instance.GetAppADListF(new Entities.DTO.GetADListFReqDTO() { MediaType = 14002, PageIndex =1, PageSize =5});
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void GetAuditADPriceList() {//缺数据
            var res = MediaInfo.Instance.GetAuditAppADList(30909);
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void BatchAuditPublish() {
            string msg = string.Empty;
            //var res = PublishInfo.Instance.BatchAuditPublish(new Entities.DTO.AuditPublishReqDTO() { PubIDs = new List<int>() { 30730 },OpType = 4, IsBatch = true,  }, ref msg);
           // Console.WriteLine(string.Format("res:{0} msg:{1}",res,msg));
        }

        [TestMethod]
        public void ModifyPublish() {
            for (var i = 1; i < 11; i++)
            {
                var one = new Entities.DTO.ModifyPublishReqDTO()
                {
                    Publish = new Entities.DTO.ModifyPublish()
                    {
                        PubID = 0,
                        BeginTime = new DateTime(2018+i, 8, 1),
                        EndTime = new DateTime(2018+i, 12, 1),
                        HasHoliday = i/2 == 0,
                        ImgUrl = "http://www.baidu.com/123123"+i,
                        IsAppointment = false,
                        MediaID = 521,
                        MediaType = 14002,
                        PurchaseDiscount = 0.85m+((decimal)i)/100,
                        SaleDiscount = 0.86m + ((decimal)i) / 100,
                        TemplateID = 9,
                    },
                    PriceList = new List<Entities.DTO.ADPrice>() {
                    new Entities.DTO.ADPrice() {
                        ADStyle = 21,
                        ExposureCount = 800+i,
                        CarouselNumber = i,
                        PubPrice = 7+i,
                        SalePrice = 11+i,
                        ClickCount =7+i,
                        SaleArea = 20 + i%2,
                        SaleType = 11001+i%2,
                        SalePlatform = 12000+i%2
                    },
                    new Entities.DTO.ADPrice() {
                        ADStyle = 22,
                        ExposureCount = 800+i,
                        CarouselNumber = i,
                        PubPrice = 7+i,
                        SalePrice = 11+i,
                        ClickCount =7+i,
                        SaleArea = 20 + i%2,
                        SaleType = 11001+i%2,
                        SalePlatform = 12000+i%2
                    }
                }
                };
                string msg = string.Empty;
                int pubID = 0;
                var res = PublishInfo.Instance.AddPublishBasicInfoV1_1(one, ref msg, ref pubID);
                Console.WriteLine(string.Format("res:{0} msg:{1} pubID:{2}", res, msg, pubID));
            }

        }

        [TestMethod]
        public void GetPublishListB()
        {
            var res = PublishInfo.Instance.GetAppPublishList(new Entities.DTO.GetADListBReqDTO() {  IsAE = false,MediaType = 14002,PageIndex=1,PageSize =20});
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void GetRecommendAD()
        {
            var res = MediaInfo.Instance.GetSimilarAD(89, 0);
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void AuditTemplate() {
            string msg = string.Empty;
            var res = AppTemplate.Instance.AuditTemplate(26, 48002, "", ref msg);
            Console.WriteLine(string.Format("res:{0}  msg:{1}",res,msg));
        }

        [TestMethod]
        public void GetPubDateList() {
            var res = PublishInfo.Instance.GetPubDateList(4, 13);
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }

        [TestMethod]
        public void CheckIsConflict() {
            var res = PublishInfo.Instance.CheckIsConflict(14002, new DateTime(2017, 6, 10), new DateTime(2017, 6, 25), 0, 0, 30877);
            Console.WriteLine(JsonConvert.SerializeObject(res));
        }


    }
}
