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
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using XYAuto.ITSC.Chitunion2017.BLL.GDT;
using XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.GDT;
using XYAuto.ITSC.Chitunion2017.WebService.GDT;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Enum;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Request;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Ads;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Fund;
using XYAuto.ITSC.Chitunion2017.WebService.GDT.Dto.Response.Report;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_2
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
        {            //log4net.ILog securityLogger = log4net.LogManager.GetLogger("SystemLog");
            BLL.Loger.ZhyLogger.Info("ZhyLoggerInfo");
            BLL.Loger.ZhyLogger.Error("ZhyLoggerError");

            BLL.Loger.GdtLogger.Info("GdtLoggerInfo");
            BLL.Loger.GdtLogger.Debug("GdtLoggerDebug");
            BLL.Loger.GdtLogger.Error("GdtLoggerError");
            BLL.Loger.Log4Net.Debug("testDebug............");
            BLL.Loger.Log4Net.Info("testInfo............");
            BLL.Loger.Log4Net.Error("Log4NetError");
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
                AccountId = 1308,
                FundType = GdtFundTypeEnum.BANK,
                DateRange = new DateRangeDto() { EndDate = "2017-08-11", StartDate = "2017-08-14" },
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
                AccountId = 1308,
                DateRange = new DateRangeDto() { EndDate = "2017-08-16", StartDate = "2017-08-13" },
                Level = GdtLevelTypeEnum.ADVERTISER
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetReportHourly_Test()
        {
            var list = ServiceHelper.GetReportHourly(new ReqReportDto()
            {
                AccountId = 1308,
                Date = "2017-08-16",
                Level = GdtLevelTypeEnum.ADVERTISER
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetReportHourlyGroupBy_Test()
        {
            var list = ServiceHelper.GetReportHourly(new ReqReportDto()
            {
                AccountId = 100000612,
                Date = "2017-08-23",
                Level = GdtLevelTypeEnum.ADGROUP,
                GroupBy = new string[] { "hour" },
                Filtering = new List<GdtFilteringDto>()
                {
                    new GdtFilteringDto()
                    {
                        Field = "adgroup_id",
                        Operator = GdtOperatorEnum.EQUALS,
                        Values = new string[] { "265521" }
                    }
                }
            });

            var stringArr = new string[] { "time", "hour" };

            Console.WriteLine(JsonConvert.SerializeObject(list));
            Console.WriteLine(GdtLevelTypeEnum.ADGROUP);
            Console.WriteLine(GdtOperatorEnum.EQUALS);
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
            var dto = JsonConvert.DeserializeObject<RespBaseDto<RespPageInfo<List<RespAdGroupDto>>>>(json);

            var list = ServiceHelper.GetAdGroupList(new ReqReportDto()
            {
                AccountId = 1308,
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
            var dto = JsonConvert.DeserializeObject<RespBaseDto<List<RespFundDto>>>(json);

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
                AccessToken = "3d1e8271d99c8e9402eb5860e5123c30",
                Appid = 2,
                P = "rKqsm24J6Qt5YLV1n08XyR+xX4K6ZUgN6jrLXkuD1y8fS5OO+pOwCbM0FD7mG55XbL+p/2YNBSYKnWA8r/OMCk7CUrpTu70MLTuxKyBZIHSXGhkLx9BgBxcqO6AVFRh6",
                Sign = "15cd3f44e810704b84625b4ea266ead5"
            };
            var _embedChiTuDesStr = "8A40B180-5E3A-41EA-9EC6-4BE8F810C961";
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
            var p = "4+GS8TPfMXOXUDzzWZtR8D2m2R5aIeZHBqro2HyhS6EPGANosptRloGjjR0srtMOiGEYeH4/6Qy7LIzPzI4l7u9wUgkKUl5BOsRKY5hrNcWwMjyQrmvDII6yOpMThBpAb7bSLfyLW5U=";
            var _embedChiTuDesStr = "8A40B180-5E3A-41EA-9EC6-4BE8F810C961";
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