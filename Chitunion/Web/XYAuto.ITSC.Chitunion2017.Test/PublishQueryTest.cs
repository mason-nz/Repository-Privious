using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class PublishQueryTest
    {
        [TestMethod]
        public void PublishWeiXinQueryTest()
        {
            var sql = @"select T.* YanFaFROM (SELECT * FROM [dbo].[Media_Weixin]) T";
            var query = new PublishQuery<Entities.Media.MediaWeixin>()
            {
                StrSql = sql,
                OrderBy = "MediaId desc",
                PageSize = 2
            };
            var list = PublishInfoQuery.Instance.QueryList(query);
            Console.WriteLine(string.Format("查询结果总数：{0}", query.Total));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(list)));

            //Assert.AreEqual(query.Total, 1);
        }

        [TestMethod]
        public void PbWeiXinQueryTest()
        {
            PublishInfoQueryClient<RequestPublishQueryDto, ResponseWeiXinDto> businessClient
                 = new PbWeiXinQuery(new ConfigEntity());

            var query = new RequestPublishQueryDto()
            { //CreateUserId = 999

            };


            var data = businessClient.GetQueryList(query);
            Console.WriteLine(string.Format("查询结果总数：{0}", data.TotleCount));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(data.List)));
        }

        [TestMethod]
        public void PbWeiXinQueryAdTest()
        { //用媒体Id列表查询对应的详情
            var detailList = PublishInfoQuery.Instance.QueryPublishItemInfo(new FrontPublishQuery<PublishDetailInfo>()
               {
                   MediaId = new List<int>() { 112 },
                   MediaType = (int)MediaType.WeiXin
               });
            var entityList = new ResponseWeiXinDto
            {
                PubID = 96,
                MediaID = 112
            };
            var dto = new PbWeiXinQuery(new ConfigEntity()).GetAdPositionList(detailList, entityList);
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(dto)));
        }

        [TestMethod]
        public void PbWeiboQueryTest()
        {
            PublishInfoQueryClient<RequestPublishQueryDto, ResponseWeiBoDto> businessClient
              = new PbWeiBoQuery(new ConfigEntity());
            var data = businessClient.GetQueryList(new RequestPublishQueryDto()
            {
                //Number = "wx",
                //Name = "wx_001",
                //Status = 2,
                //PublishStatus = 2,
                //EndTime = 2
            });
            Console.WriteLine(string.Format("查询结果总数：{0}", data.TotleCount));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(data.List)));
        }

        [TestMethod]
        public void PbVideoQueryTest()
        {
            PublishInfoQueryClient<RequestPublishQueryDto, ResponseVideoDto> businessClient
         = new PbVideoQuery(new ConfigEntity());
            var data = businessClient.GetQueryList(new RequestPublishQueryDto()
            {

            });
            Console.WriteLine(string.Format("查询结果总数：{0}", data.TotleCount));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(data.List)));
        }

        [TestMethod]
        public void PbBroadcastQueryTest()
        {
            PublishInfoQueryClient<RequestPublishQueryDto, ResponseBroadcastDto> businessClient
        = new PbBroadcastQuery(new ConfigEntity());
            var data = businessClient.GetQueryList(new RequestPublishQueryDto());
            Console.WriteLine(string.Format("查询结果总数：{0}", data.TotleCount));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(data.List)));
        }

        [TestMethod]
        public void PbAppQueryByYunYingTest()
        {
            PublishInfoQueryClient<RequestPublishQueryDto, ResponseAppDtoByYunYing> businessClient
        = new PbAppQueryByYunYing(new ConfigEntity());
            var data = businessClient.GetQueryList(new RequestPublishQueryDto());
            Console.WriteLine(string.Format("查询结果总数：{0}", data.TotleCount));
            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(data.List)));
        }

        [TestMethod]
        public void LinqTest()
        {
            var json = "[{\"TwoCodeURL\":null,\"FansCount\":99999999,\"LevelType\":0,\"ADPosition1\":6001,\"ADPosition2\":7002,\"ADPosition3\":8001,\"Price\":11.2000,\"FistName\":\"单图文\",\"SecondName\":\"软广\",\"ThirdName\":\"软广\",\"Status\":0,\"PublishStatus\":0,\"MediaID\":28,\"Number\":\"wx111\",\"Name\":\"微信测试添加001\",\"HeadIconURL\":\"http://image.xingyuan.com,http://image.xingyuan.com/HeadIconURL\",\"First\":null,\"Second\":null,\"Third\":null},{\"TwoCodeURL\":null,\"FansCount\":99999999,\"LevelType\":0,\"ADPosition1\":6001,\"ADPosition2\":7002,\"ADPosition3\":8002,\"Price\":11.2000,\"FistName\":\"单图文\",\"SecondName\":\"软广\",\"ThirdName\":\"软广\",\"Status\":0,\"PublishStatus\":0,\"MediaID\":28,\"Number\":\"wx111\",\"Name\":\"微信测试添加001\",\"HeadIconURL\":\"http://image.xingyuan.com,http://image.xingyuan.com/HeadIconURL\",\"First\":null,\"Second\":null,\"Third\":null},{\"TwoCodeURL\":null,\"FansCount\":99999999,\"LevelType\":0,\"ADPosition1\":6002,\"ADPosition2\":7002,\"ADPosition3\":8002,\"Price\":11.2000,\"FistName\":\"多图文头条\",\"SecondName\":\"软广\",\"ThirdName\":\"软广\",\"Status\":0,\"PublishStatus\":0,\"MediaID\":28,\"Number\":\"wx111\",\"Name\":\"微信测试添加001\",\"HeadIconURL\":\"http://image.xingyuan.com,http://image.xingyuan.com/HeadIconURL\",\"First\":null,\"Second\":null,\"Third\":null},{\"TwoCodeURL\":null,\"FansCount\":99999999,\"LevelType\":0,\"ADPosition1\":6002,\"ADPosition2\":7002,\"ADPosition3\":8002,\"Price\":11.2000,\"FistName\":\"多图文头条\",\"SecondName\":\"软广\",\"ThirdName\":\"软广\",\"Status\":0,\"PublishStatus\":0,\"MediaID\":28,\"Number\":\"wx111\",\"Name\":\"微信测试添加001\",\"HeadIconURL\":\"http://image.xingyuan.com,http://image.xingyuan.com/HeadIconURL\",\"First\":null,\"Second\":null,\"Third\":null},{\"TwoCodeURL\":null,\"FansCount\":0,\"LevelType\":0,\"ADPosition1\":6001,\"ADPosition2\":7001,\"ADPosition3\":8001,\"Price\":11.2000,\"FistName\":\"单图文\",\"SecondName\":\"硬广\",\"ThirdName\":\"硬广\",\"Status\":0,\"PublishStatus\":0,\"MediaID\":1,\"Number\":\"123456\",\"Name\":\"我爱西红柿\",\"HeadIconURL\":null,\"First\":null,\"Second\":null,\"Third\":null},{\"TwoCodeURL\":null,\"FansCount\":0,\"LevelType\":0,\"ADPosition1\":6001,\"ADPosition2\":7001,\"ADPosition3\":8002,\"Price\":11.2000,\"FistName\":\"单图文\",\"SecondName\":\"硬广\",\"ThirdName\":\"硬广\",\"Status\":0,\"PublishStatus\":0,\"MediaID\":1,\"Number\":\"123456\",\"Name\":\"我爱西红柿\",\"HeadIconURL\":null,\"First\":null,\"Second\":null,\"Third\":null}]";

            var list = JsonConvert.DeserializeObject<List<ResponseWeiXinDto>>(json);

            var listG = list.GroupBy(x => x.MediaID).ToList();

            var list1 = listG.Select(m => new ResponseWeiXinDto
                {
                    MediaID = m.Key,
                    //First = m.Select(g => (g.FistName + ":" + g.Price)).ToArray(),
                    //Second = m.Select(g => (g.SecondName + ":" + g.Price)).ToArray(),
                    //Third = m.Select(g => (g.ThirdName + ":" + g.Price)).ToArray(),
                    Number = m.Select(g => g.Number).FirstOrDefault(),
                    //Price = m.Select(g => g.Price).FirstOrDefault(),
                    Name = m.Select(g => g.Name).FirstOrDefault(),
                    HeadIconURL = m.Select(g => g.HeadIconURL).FirstOrDefault(),
                    TwoCodeURL = m.Select(g => g.TwoCodeURL).FirstOrDefault(),
                    FansCount = m.Select(g => g.FansCount).FirstOrDefault(),
                    Status = m.Select(g => g.Status).FirstOrDefault(),
                    PublishStatus = m.Select(g => g.PublishStatus).FirstOrDefault(),

                }).ToList();

            Console.WriteLine(string.Format("查询结果：{0}", JsonConvert.SerializeObject(list1)));
        }

        [TestMethod]
        public void DynamicTest()
        {
            var userID = 22;
            var msg = "";
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.MediaORPublish, "Media_WeiXin", "CreateUserID", userID, out msg);

            Console.WriteLine(rightSql);
            Console.WriteLine(msg);
        }

        [TestMethod]
        public void GetPublishBasicInfo_Test()
        {
            var publishBaseInfo = BLL.Publish.PublishInfoQuery.Instance.GetPublishBasicInfo(new PublishQuery<PublishBasicInfo>()
            {
                // Media_Id = _requestMediaPublicParam.MediaID,
                MediaType = (int)MediaType.WeiXin
            });
            if (publishBaseInfo.Status == AuditStatusEnum.已通过)
            {
                Console.WriteLine("已上架");
                Console.WriteLine(JsonConvert.SerializeObject(publishBaseInfo));
                return;
            }
            Console.WriteLine(JsonConvert.SerializeObject(publishBaseInfo));
        }
    }
}
