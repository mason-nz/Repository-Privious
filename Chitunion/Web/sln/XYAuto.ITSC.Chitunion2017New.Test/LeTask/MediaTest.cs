using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;

namespace XYAuto.ITSC.Chitunion2017New.Test.LeTask
{
    [TestClass]
    public class MediaTest
    {

        [Description("修改-微信报价")]
        [TestMethod]
        public void UpdateWxOfferTest()
        {
            var request = new ReqMediaUpdateWxOfferDto()
            {
                MediaId = 1890,
                CategoryId = 10,
                FansCount = 100,
                FansFemalePer = 0.67m,
                FansMalePer = 0.33m,
                OverlayArea = new Overlayarea()
                {
                    ProvinceId = 1,
                    CityId = 201,
                },
                DeliveryPrices = new List<Deliveryprice>()
                {
                    new Deliveryprice() { ADPosition1 = 6001 ,ADPosition2 = 9003,Price = 10},
                    new Deliveryprice() { ADPosition1 = 6002  ,ADPosition2 = 9003,Price = 11},
                    new Deliveryprice() { ADPosition1 = 6003  ,ADPosition2 = 9003,Price = 12},
                    new Deliveryprice() { ADPosition1 = 6001  ,ADPosition2 = 9002 ,Price = 15}
                }
            };
            var retValue = new BindingWeiXinProvider(new ConfigEntity()
            {
                CreateUserId = 1409
            }, request).UpdateOffer();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);

        }



        [Description("修改微博报价")]
        [TestMethod]
        public void UpdateWbOfferTest()
        {
            var request = new ReqMediaUpdateWbOfferDto()
            {
                MediaId = 1890,
                CategoryId = 10,
                OverlayArea = new Overlayarea()
                {
                    ProvinceId = 1,
                    CityId = 201,
                },
                DeliveryPrices = new List<Deliveryprice>()
                {
                    new Deliveryprice() { ADPosition1 = 6001 ,ADPosition2 = 9003,Price = 10},
                    new Deliveryprice() { ADPosition1 = 6002  ,ADPosition2 = 9003,Price = 11},
                    new Deliveryprice() { ADPosition1 = 6003  ,ADPosition2 = 9003,Price = 12},
                    new Deliveryprice() { ADPosition1 = 6001  ,ADPosition2 = 9002 ,Price = 15}
                }
            };
            var retValue = new BindingWeiBoProvider(new ConfigEntity(), request).UpdateOffer();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }
        //public int Type { get; set; }
        //public string TrueName { get; set; }
        //public string BLicenceURL { get; set; }
        //public string IdentityNo { get; set; }
        //public string IDCardFrontURL { get; set; }
        [TestMethod]
        public void SaveUserDetail()
        {
            ReqUserDetailDto DTO = new ReqUserDetailDto();
            DTO.Type = 1001;
            DTO.TrueName = "fff";
            DTO.IdentityNo = "130533199301065356";
            DTO.BLicenceURL = "aaa";
            XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.SaveUserDetail(DTO);
        }

        [TestMethod]
        public void GetDomainByRandom_Test()
        {
            string dominUrl = XYAuto.ITSC.Chitunion2017.BLL.Util.GetDomainByRandom();
            Assert.IsFalse(dominUrl!=string.Empty);
        }

        [TestMethod]
        public void Test2()
        {
            string MaterialUrl = "http://news.weixins3.xingyuanwanli.com/ct_m/20180416/192103.html?utm_source=chitu&utm_term=mgy2vsij6m";
            int index = MaterialUrl.IndexOf("ct_m");
            string str1 = MaterialUrl.Substring(index - 1);
            string str2 = "http://www.1.com" + str1;
        }
    }
}
