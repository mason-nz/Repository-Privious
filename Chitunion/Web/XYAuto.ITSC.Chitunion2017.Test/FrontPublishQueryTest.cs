using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.FrontPage;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class FrontPublishQueryTest
    {
        [TestMethod]
        public void PublishWeiXinQueryTest()
        {
            var sql = @"select T.* YanFaFROM (SELECT * FROM [dbo].[Media_Weixin]

SELECT  dbo.fn_GetADPositionDicName(PD.ADPosition2) + ' '
        + dbo.fn_GetADPositionDicName(PD.ADPosition3) + ':'
        + CONVERT(VARCHAR(100), CONVERT(DECIMAL(18,2), PD.Price))
FROM    dbo.Publish_DetailInfo AS PD WITH ( NOLOCK ) WHERE PD.MediaID = 26
) T";
            var query = new PublishQuery<Entities.Media.MediaWeixin>()
            {
                StrSql = sql,
                OrderBy = "MediaId desc",
                PageSize = 2
            };

            //用媒体Id列表查询对应的详情
            var publishItemInfo =
                PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
                {
                    //MediaId = mediaIdList,
                    MediaType = (int)MediaType.WeiXin
                });
            //

            var dataList = new List<ResponseFrontWeiXinDto>();

            for (int i = 0; i < dataList.Count; i++)
            {
                var item = dataList[i];
                var groupItemInfo = publishItemInfo.Where(s => s.MediaID == item.MediaID);
                //找到first 单图文
                var firstList = groupItemInfo.Where(s => s.ADPosition1 == 6001);
                //找到first 单图文-硬广
                var firstADPosition27002 = firstList.Where(s => s.ADPosition2 == 7001).FirstOrDefault();
                //找到first 单图文-软广
                var firstADPosition27003 = firstList.Where(s => s.ADPosition2 == 7002).FirstOrDefault();

                //找到Second 多图文头条
                var SecondList = groupItemInfo.Where(s => s.ADPosition1 == 6002);
                //找到first 单图文-硬广
                var SecondADPosition27002 = firstList.Where(s => s.ADPosition2 == 7001).FirstOrDefault();
                //找到first 单图文-软广
                var SecondADPosition27003 = firstList.Where(s => s.ADPosition2 == 7002).FirstOrDefault();

                item.FirstName = new List<AdPositionEntity>()
                {
                    new AdPositionEntity()
                    {
                        AdName = firstADPosition27002.ADPosition2Name,
                        Price = firstADPosition27002.Price
                    },
                    new AdPositionEntity()
                    {
                        AdName = firstADPosition27003.ADPosition1Name,
                        Price = firstADPosition27003.Price
                    }
                };
                var second = groupItemInfo.FirstOrDefault(s => s.ADPosition1 == 6001);
            }
        }

        [TestMethod]
        public void FpWeiXinQueryTest()
        {
            PublishInfoQueryClient<RequestFrontPublishQueryDto, ResponseFrontWeiXinDto> businessClient
                = new FpWeiXinQuery(new ConfigEntity());
            var data = businessClient.GetQueryList(new RequestFrontPublishQueryDto());
            Console.WriteLine(string.Format("查询结果总数：{0}", data.TotleCount));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(data.List)));
        }

        [TestMethod]
        public void PublishQueryProcessTest()
        {
            var json = new JsonResultTest
            {
                //Data = new PublishQueryProcess(new RequestFrontPublishQueryDto(){BussinessType =
                //(int)MediaType.WeiXin}).GetQuery()
            };

            json = new JsonResultTest
            {
                Data = new PublishQueryProxy(new RequestFrontPublishQueryDto()
                {
                    BusinessType =
                        (int)MediaType.WeiXin,
                    Keyword = "测试"
                    //CategoryID = 1000,
                    //Platform = 100,
                    //FansCount = "500-0",
                    //CoverageArea = "201-10",
                    //Price = "500-1000",
                    //OrderByReference = 9001
                }).GetQuery()
            };
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(json)));
        }

        [TestMethod]
        public void FpWeiBoQueryTest()
        {
            var list = new PublishQueryProxy(new RequestFrontPublishQueryDto()
            {
                BusinessType = (int)MediaType.WeiBo,
                //CategoryID = 1000,
                //Platform = 100,
                //FansCount = "500-0",
                //CoverageArea = "201-201",
                Price = "2-50000",
                //OrderByReference = 8001
            }).GetQuery();

            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(list)));
        }

        [TestMethod]
        public void FpVideoQueryTest()
        {
            var list = new PublishQueryProxy(new RequestFrontPublishQueryDto()
            {
                BusinessType = (int)MediaType.Video,
                //CategoryID = 1000,
                //Platform = 100,
                //FansCount = "500-0",
                //Price = "2-50000",
                //AdForm = "d",
                //Sex = 1
            }).GetQuery();

            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(list)));
        }

        [TestMethod]
        public void FpAppQueryTest()
        {
            var list = new PublishQueryProxy(new RequestFrontPublishQueryDto()
            {
                BusinessType = (int)MediaType.APP,
                //CategoryID = 1000,
                //Platform = 100,
                //FansCount = "500-0",
                //CoverageArea = "201-201",
                //AdForm = "d",
                //Sex = 1
            }).GetQuery();

            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(list)));
        }

        [TestMethod]
        public void FpBroadcastQueryTest()
        {
            var list = new PublishQueryProxy(new RequestFrontPublishQueryDto()
            {
                BusinessType = (int)MediaType.Broadcast,
                Price = "1000-3000",
                //CategoryID = 1000,
                //Platform = 100,
                //FansCount = "500-0",
                //CoverageArea = "201-201"
            }).GetQuery();

            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(list)));
        }

        [TestMethod]
        public void GetAdPositionListTest()
        {
            //用媒体Id列表查询对应的详情
            var publishItemInfo = PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
            {
                //MediaId = mediaIdList,
                MediaType = (int)MediaType.WeiXin
            });

            var item = new ResponseFrontWeiXinDto() { MediaID = 14 };
            GetAdPositionList(publishItemInfo, item);

            Console.WriteLine(JsonConvert.SerializeObject(item));
        }

        [TestMethod]
        public void OrderByTest()
        {
            var orderByStr = " MediaID DESC ";

            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," FansCount ASC"},
                {1002," FansCount DESC"},
                {2001," ReferReadCount ASC"},
                {2002," ReferReadCount ASC"},
                {3001," UpdateCount ASC"},
                {3002," UpdateCount DESC"}
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == 20011);

            Console.WriteLine(value.Value);
        }

        [TestMethod]
        public void NumberTest()
        {
            decimal counts = 158600m / 10000;
            Console.WriteLine(counts);

            var d = Math.Round(Convert.ToDecimal(counts), 1, MidpointRounding.AwayFromZero);
            Console.WriteLine(d);

            var dto = new ResponseFrontDto() { FansCount = 135672220 };

            Console.WriteLine(dto.FansCountName);

            dto.FansCount = 234;
            Console.WriteLine(dto.FansCountName);
        }

        [TestMethod]
        public void RequestFrontPublishQueryDto_Test()
        {
            var dto = new RequestFrontPublishQueryDto()
            {
                PriceUnit = 0
            };
            Console.WriteLine(dto.PriceUnit);

            dto.PriceUnit = 1222222;
            Console.WriteLine(dto.PriceUnit);
            dto.PriceUnit = 444;
            Console.WriteLine(dto.PriceUnit);
        }

        [TestMethod]
        public void FrontWeiBoAdTest()
        {
            //用媒体Id列表查询对应的详情
            var publishItemInfo = PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
            {
                //MediaId = mediaIdList,
                MediaType = (int)MediaType.WeiBo
            });

            var listMediaId = new List<int>() { 7, 8 };

            for (var i = 0; i < listMediaId.Count; i++)
            {
                var mediaId = listMediaId[i];
                var publishItemList = publishItemInfo.Where(s => s.MediaID == mediaId);

                publishItemList.Where(s => s.ADPosition2 == (int)AdFormality4.直发);
            }

            foreach (var itemInfo in publishItemInfo)
            {
            }
        }

        private ResponseFrontWeiXinDto GetAdPositionList(List<PublishDetailInfo> publishDetailList, ResponseFrontWeiXinDto item)
        {
            item.FirstName = new List<AdPositionEntity>();
            item.SecondName = new List<AdPositionEntity>();
            item.ThridName = new List<AdPositionEntity>();
            var groupItemInfo = publishDetailList.Where(s => s.MediaID == item.MediaID);
            //找到first 单图文
            var detailInfos = groupItemInfo as PublishDetailInfo[] ?? groupItemInfo.ToArray();
            var firstList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.单图文);
            //找到first 单图文-硬广
            var publishDetailInfos = firstList as PublishDetailInfo[] ?? firstList.ToArray();
            var firstAdPosition27002 = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广);
            //找到first 单图文-软广
            var firstAdPosition27003 = publishDetailInfos.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广);
            if (firstAdPosition27002 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27002.ADPosition2Name, Price = Math.Round(firstAdPosition27002.Price, 2) });
            if (firstAdPosition27003 != null)
                item.FirstName.Add(new AdPositionEntity() { AdName = firstAdPosition27003.ADPosition2Name, Price = firstAdPosition27003.Price });

            //找到second 多图文头条
            var secondList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文头条);
            //找到second 多图文头条-硬广
            var secondAdPosition27002 = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广);
            //找到second 多图文头条-软广
            var secondAdPosition27003 = secondList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广);

            if (secondAdPosition27002 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27002.ADPosition2Name, Price = secondAdPosition27002.Price });
            if (secondAdPosition27003 != null)
                item.SecondName.Add(new AdPositionEntity() { AdName = secondAdPosition27003.ADPosition2Name, Price = secondAdPosition27003.Price });

            //找到third 多图文第二条
            var thirdList = detailInfos.Where(s => s.ADPosition1 == (int)AdPositionMapping.多图文第二条);
            //找到third 多图文第二条-硬广
            var thirdAdPosition27002 = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.硬广);
            //找到third 多图文第二条-软广
            var thirdAdPosition27003 = thirdList.FirstOrDefault(s => s.ADPosition2 == (int)AdTypeMapping.软广);

            if (thirdAdPosition27002 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27002.ADPosition2Name, Price = thirdAdPosition27002.Price });
            if (thirdAdPosition27003 != null)
                item.ThridName.Add(new AdPositionEntity() { AdName = thirdAdPosition27003.ADPosition2Name, Price = thirdAdPosition27003.Price });

            return item;
        }
    }

    public class JsonResultTest
    {
        public int Status { get; set; }
        public object Data { get; set; }
    }
}