/********************************************************
*创建人：lixiong
*创建时间：2017/8/15 12:53:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Enum;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Ads;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Fund;
using XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response.Report;
using XYAuto.BUOC.BOP2017.Infrastruction;
using XYAuto.BUOC.BOP2017.Infrastruction.Http;
using XYAuto.BUOC.BOP2017.Infrastruction.Security;

namespace XYAuto.BUOC.BOP2017.Test.GDT
{
    [TestClass]
    public class ServiceHelperTest
    {
        public readonly ServiceHelper ServiceHelper;

        public ServiceHelperTest()
        {
            ServiceHelper = new ServiceHelper();
        }

        [TestMethod]
        public void LogTest()
        {
            Loger.Log4Net.Info($" base .. this is  info..");
            Loger.Log4Net.Error($" base .. this is  Error..");
            Loger.ZhyLogger.Info($" ZhyLogger .. this is  info..");
            Loger.ZhyLogger.Error($" ZhyLogger .. this is  Error..");
            Loger.GdtLogger.Info($" GdtLogger .. this is  info..");
            Loger.GdtLogger.Error($" GdtLogger .. this is  Error..");
        }

        [TestMethod]
        public void GetAdvertiserInfo_Test()
        {
            var json = ServiceHelper.GetAdvertiserInfo(new ReqReportDto()
            {
                //AccountId = 1308
            });
            Console.WriteLine(JsonConvert.SerializeObject(json));
        }

        [TestMethod]
        public void GetFundsInfo_Test()
        {
            var json = ServiceHelper.GetFundsInfo(1308);
            Console.WriteLine(JsonConvert.SerializeObject(json));
        }

        [TestMethod]
        public void GetFundStatementsDaily_Test()
        {
            var list = ServiceHelper.GetFundStatementsDaily(new ReqFundDto()
            {
                AccountId = 1308,
                FundType = GdtFundTypeEnum.GENERAL_CASH,
                Date = "2017-08-14",
                TradeType = GdtTradeTypeEnum.CHARGE
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetFundStatementsDetailed_Test()
        {
            var list = ServiceHelper.GetFundStatementsDetailed(new ReqFundDetaileDto()
            {
                AccountId = 6317333,
                FundType = GdtFundTypeEnum.BANK,
                DateRange = new DateRangeDto() { EndDate = "2017-08-11", StartDate = "2017-11-14" },
                PageSize = 10,
                Page = 1
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetAccessTokenByRefreshToken_Test()
        {
            var list = ServiceHelper.GetAccessTokenByRefreshToken("2609e4f31ba2226f4acb09e5b75bd069");
            //var url = ServiceHelper.Instance.GetToAuthorizeUrl("http://www.chitunion.com/gdt/test.html", "xy");
            //Console.WriteLine(url);
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetReportDaily_Test()
        {
            var list = ServiceHelper.GetReportDaily(new ReqReportDto()
            {
                AccountId = 6317333,
                DateRange = new DateRangeDto() { StartDate = "2017-11-10", EndDate = "2017-11-10" },
                Level = GdtLevelTypeEnum.ADGROUP,
                GroupBy = new string[] { "adgroup_id" }
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetReportHourly_Test()
        {
            for (int i = 0; i < 30; i++)
            {
                var date = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                var list = ServiceHelper.GetReportHourly(new ReqReportDto()
                {
                    AccountId = 6317333,
                    Date = date,
                    Level = GdtLevelTypeEnum.ADGROUP,
                    GroupBy = new string[] { "adgroup_id" }
                    //Filtering = new List<GdtFilteringDto>()
                    //{
                    //    new GdtFilteringDto()
                    //    {
                    //        Field = "adgroup_id",
                    //        Operator = GdtOperatorEnum.EQUALS,
                    //        Values = new string[] {"39513561"}
                    //    }
                    //}
                    ,
                    PageSize = 50
                });
                var count = list.List != null ? list.List.Count : 0;
                Console.WriteLine($"当前日期：{date}，数据返回数量：{count}");
            }

            //Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetReportHourlyGroupBy_Test()
        {
            var accountlist = BLL.GDT.GdtAccountInfo.Instance.GetList().Select(s => s.AccountId).ToList();

            accountlist.ForEach(s =>
            {
                var ads = new List<int>() { 39513561, 39512295, 39143688, 39067709, 39066789, 39063254, 39062607 };
                ads.ForEach(t =>
                {
                    var list = ServiceHelper.GetReportHourly(new ReqReportDto()
                    {
                        AccountId = s,
                        Date = "2017-11-13",
                        Level = GdtLevelTypeEnum.ADGROUP,
                        GroupBy = new string[] { "hour" },
                        PageSize = 100,
                        Filtering = new List<GdtFilteringDto>()
                        {
                            new GdtFilteringDto()
                            {
                                Field = "adgroup_id",
                                Operator = GdtOperatorEnum.EQUALS,
                                Values = new string[] { t.ToString() }
                            }
                        }
                    });
                    Console.WriteLine(JsonConvert.SerializeObject(list));
                });
            });
        }

        [Description("获取推广计划")]
        [TestMethod]
        public void GetAdCampaignsList_Test()
        {
            var list = ServiceHelper.GetAdCampaignsList(new ReqReportDto()
            {
                AccountId = 1308,
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("获取广告组列表")]
        [TestMethod]
        public void GetAdGroupList_Test()
        {
            var json = "{\"data\":{\"list\":[],\"page_info\":{\"page\":1,\"page_size\":20,\"total_number\":0,\"total_page\":0}},\"code\":0,\"message\":\"\"}";
            var dto = JsonConvert.DeserializeObject<ExternalApi.GDT.Dto.Response.RespBaseDto<RespPageInfo<List<RespAdGroupDto>>>>(json);

            var list = ServiceHelper.GetAdGroupList(new ReqReportDto()
            {
                AccountId = 6317333,
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("获取今日消耗,只支持今天或昨天的数据查询")]
        [TestMethod]
        public void GetRealtimeCost_Test()
        {
            var list = ServiceHelper.GetRealtimeCost(new ReqReportDto()
            {
                AccountId = 1308,
                Date = "2017-08-18",
                Level = GdtLevelTypeEnum.ADVERTISER
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetDicSystemStatus_Test()
        {
            var dic = GdtDataEnumProvider.GetDicAdSystemStatus();
            Console.WriteLine(dic.ContainsKey("AD_STATUS_NORMAL".ToLower()));
        }

        [TestMethod]
        public void Json_Test()
        {
            var json = "{\"data\":{\"list\":[{\"balance\":0,\"fund_status\":\"FUND_STATUS_NORMAL\",\"fund_type\":\"GENERAL_CASH\",\"realtime_cost\":0},{\"balance\":0,\"fund_status\":\"FUND_STATUS_NORMAL\",\"fund_type\":\"GENERAL_GIFT\",\"realtime_cost\":0},{\"balance\":0,\"fund_status\":\"FUND_STATUS_NORMAL\",\"fund_type\":\"GENERAL_SHARED\",\"realtime_cost\":0}]},\"code\":0,\"message\":\"\"}";
            var dto = JsonConvert.DeserializeObject<ExternalApi.GDT.Dto.Response.RespBaseDto<List<RespFundDto>>>(json);

            Console.WriteLine(JsonConvert.SerializeObject(dto));
        }

        [TestMethod]
        public void EnumTest()
        {
            var fundType = (GdtFundTypeEnum)Enum.Parse(typeof(GdtFundTypeEnum), "GENERAL_CASH");
            Console.WriteLine(fundType.ToString());
        }

        [TestMethod]
        public void GdtAccessToken_Test()
        {
            var info = BLL.GDT.GdtAccessToken.Instance.GetInfo((int)AuditRelationTypeEnum.Gdt, 1106267651);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void SignTest()
        {
            var token = new RequestBaseTokenDto()
            {
                AccessToken = "79d06b939fcc09ec7bf63def43926df6",
                Appid = 2,
                P = "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",

                Sign = "69ab6cfc7984c5cdc2e8a72715d7feba"
            };
            var _embedChiTuDesStr = "8A40B180-5E3A-41EA-9EC6-4BE8F810C961";
            _embedChiTuDesStr = "5CBB8859-84E6-45BD-9A13-AE24E";
            token.P = token.P.Replace(" ", "+");

            Console.WriteLine(token.Appid + token.AccessToken + token.P + _embedChiTuDesStr);

            var md5Code = SignUtility.Md5Hash(token.Appid + token.AccessToken + token.P + _embedChiTuDesStr, Encoding.UTF8);
            Console.WriteLine(md5Code);
            Console.WriteLine(md5Code == token.Sign);
        }

        [TestMethod]
        public void TestEncrypt()
        {
            var _embedChiTuDesStr = "8A40B180-5E3A-41EA-9EC6-4BE8F810C961";
            //线上
            _embedChiTuDesStr = "5CBB8859-84E6-45BD-9A13-AE24ED6E8165";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("RechargeNumber", "T17083017211403");
            dict.Add("Amount", "100.00");
            dict.Add("OrganizeID", "64");
            dict.Add("DemandBillNo", "100147");

            var json = JsonConvert.SerializeObject(dict);
            Console.WriteLine("json:" + json);

            var p = Encrypt(json, _embedChiTuDesStr);
            Console.WriteLine("p:" + p);

            var des = Decrypt(p, _embedChiTuDesStr);

            Console.WriteLine("解密:" + des);
        }

        [TestMethod]
        public void DecryptTest()
        {
            var p =
                "ePy6OlAQlOOtiVmHf+rqKKfmJlKGJ1HsZ3eiG99OJTmkhsXrOgZIJHhV9d7CKc3TNTMIhy4TafssEN/u6FyHg4vEefNxhgXNbP6BPrpZqf8jFVZ6EOexGhPuiIEE2MNdKu+QyVmmEFNBfWvquZWSe7B+etcdVgg31lE+g9aSCxEURmofA7EW7UMkfKtXGd1YySfQrZz61ZFvoZGKjEOtVPZ6FhNI2GOSWynXGaYHkKA70xrnFRXTxere0eCC86e21SFx8r4lTX01HYIUmxbTdgZQ58lujnjTAF5ET3uQPxMZx7NUnqBkjKmzB9WqFJJQlQ2QcB7qHGXe7TmN6rClZ24KTrzZhBvH8+sTuD7cUZFhfFsVF9pcozgLbgZpJwG9ZYje72VKY6qiTOMRFLUWmGPkxVSuYyK+xD7TLPrh09QhDsEJjQXUv1FdBcjUbk0dkUDwVJ7AwJmPxSmFiqIDofVcFmhqFzKhLdme7baT7LwO4yNIyeFx8ufdk9rlmMBM74+wazD71mjSTtIt5ErLO7Ixph6g0fEXpYRhNXueKaVV2q5oZ/L23FWq6coAQy3w6CPBqoLmLk8Ked5wp/F1j2hsc+zy6Hr7a28YjtCdngZlGsZbIFn7XTCzrqF62tYD44VHqAV4n6/p9hMhcwXPlynrD5t513Yv";

            p = p.Replace(" ", "+");

            var _embedChiTuDesStr = "8A40B180-5E3A-41EA-9EC6-4BE8F810C961";

            //_embedChiTuDesStr = "C581F7FC-1639-4298-9C3C-570BCC4D9A11";
            var des = Decrypt(p, _embedChiTuDesStr);

            Console.WriteLine("解密:" + des);
        }

        public string Decrypt(string encryptedText, string decryptKey)
        {
            /*  encryptedText = "ilcUCTRLx19j8ikx75v9aHNxgvCzXbt + JT7HMXmE0CB2QdWaWMF0lz1TvHx3fyWO6o7W7kPDEHybJMMcJhFisPbF4nVoSb7lEFjk6fB0kKwWv3tg5bEG + l5iHbtpT8t + 7v5jTVzQN8wdbWd77lpuomLy6uAY90LhzFQFbCLGIP74TG1ZWzIcgxE81wX7w + c0u1xnM7cDx8TFIEQG1P5iCnaCwjR7jZWiA + whRZ1LC0G5Di1eleIhAAEK6FY27 + QMrXCXh / tlmdEqGcTLLNQ61g1zp / pxZw / i / moCtW54xinvZF5z1Z2lrxfKtzo3vYAUT4WbRC1kJdv09UGFTkkAlQ ==";*/

            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }
            DESCryptoServiceProvider provider = CreateDESProvider(decryptKey);
            byte[] buffer = Convert.FromBase64String(encryptedText);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return new UTF8Encoding().GetString(stream.ToArray());
        }

        public static string Encrypt(string strText, string encryptKey)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return strText;
            }
            DESCryptoServiceProvider provider = CreateDESProvider(encryptKey);
            byte[] bytes = Encoding.UTF8.GetBytes(strText);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        private static DESCryptoServiceProvider CreateDESProvider(string encryptKey)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            int length = encryptKey.Length;
            if (length < 8)
            {
                for (int i = 0; i < (8 - length); i++)
                {
                    encryptKey = encryptKey + "@";
                }
            }
            provider = new DESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, provider.KeySize / 8))
            };
            byte[] destinationArray = new byte[provider.BlockSize / 8];
            byte[] DefaultIV = new byte[provider.BlockSize / 8];
            Array.Copy(DefaultIV, 0, destinationArray, 0, destinationArray.Length);
            provider.IV = destinationArray;
            return provider;
        }
    }
}