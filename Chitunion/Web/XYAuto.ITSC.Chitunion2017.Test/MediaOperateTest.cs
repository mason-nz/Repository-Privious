using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class MediaOperateTest
    {
        public RequestMediaPublicParam CreateRequestMediaPublicParam(MediaType mediaType, OperateType operateType)
        {
            var bussinessType = (int)mediaType;

            var random = new Random().Next(1, 999999);
            var name = string.Format("{0}-测试-{1}-{2}", mediaType, operateType == OperateType.Insert ? "添加" : "编辑", random);// bussinessType.ToString() + "";
            var number = string.Format("{0}_{1}", mediaType, random);
            // name = "wx111";

            var publicParams = new RequestMediaPublicParam
            {
                BusinessType = bussinessType,
                OperateType = (int)operateType,//1:add 2:edit
                CategoryID = 2001,
                CityID = 201,
                FansCount = 10099,
                HeadIconURL = "http://image.xingyuan.com",
                // LevelType = 4001,
                Name = name,
                Number = number,
                ProvinceID = 201,
                Source = 3001,
                CreateTime = DateTime.Now,
                CreateUserID = 199,
                LastUpdateTime = DateTime.Now,
                LastUpdateUserID = 199,
                CoverageArea = "201-201,3001-3456"
            };

            return publicParams;
        }

        [TestMethod]
        public void MediaWeiXinCreataeTest()
        {
            var publicParams = CreateRequestMediaPublicParam(MediaType.WeiXin, OperateType.Insert);
            publicParams.MediaID = 35;

            var weixin = new RequestMediaWeiXinDto()
            {
                AreaID = 23001,
                FansCountURL = "http://image.xingyuan.com/FansCountURL",
                FansFemalePer = 50.10M,
                FansMalePer = 49.90M,
                //HeadIconURL = "http://image.xingyuan.com/HeadIconURL",
                IsAuth = true,
                IsReserve = true,
                OrderRemark = "1002",
                TwoCodeURL = "http://image.xingyuan.com/TwoCodeURL",
                //互动参数
                // MediaType = 1,
                Sign = "描述、签名",
                ReferReadCount = 10000,
                AveragePointCount = 10010,
                MoreReadCount = 999,
                OrigArticleCount = 999,
                UpdateCount = 50,
                MaxinumReading = 9881,
                ScreenShotURL = "http://image.xingyuan.com/ScreenShotURL"
            };

            var business = new MediaBusinessProxy(publicParams, weixin, new RequestMediaWeiBoDto(),
                new RequestMediaPcAppDto(), new RequestMediaVideoDto(), new RequestMediaBroadcastDto());
            var retValue = business.Excute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void MediaWeiBoCreataeTest()
        {
            var publicParams = CreateRequestMediaPublicParam(MediaType.WeiBo, OperateType.Edit);
            publicParams.MediaID = 1;
            publicParams.CoverageArea = "201-201";
            var weibo = new RequestMediaWeiBoDto()
            {
                Profession = 1001,
                Sex = "1",
                FansSex = "1",
                AuthType = 1,
                OrderRemark = "100",
                IsReserve = true,
                //Source = 3001,//来源
                FansCountURL = "http://image.xingyuan.com/FansCountURL",
                //互动参数
                //MeidaType = 1,
                Sign = "描述、签名",
                AveragePointCount = 500,
                AverageForwardCount = 499,
                AverageCommentCount = 378,
                ScreenShotURL = "http://image.xingyuan.com/ScreenShotURL",
            };
            var business = new MediaBusinessProxy(publicParams, new RequestMediaWeiXinDto(), weibo,
             new RequestMediaPcAppDto(), new RequestMediaVideoDto(), new RequestMediaBroadcastDto());
            var retValue = business.Excute();

            //BLL.Media.MediaAreaMapping.Instance.BusinessCureForTask(publicParams);
            MediaAreaMapping.Instance.BusinessCure(publicParams);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void MediaVideoCreataeTest()
        {
            var publicParams = CreateRequestMediaPublicParam(MediaType.Video, OperateType.Insert);
            var requestDto = new RequestMediaVideoDto()
            {
                //Profession = 1001,
                //Sex = "F",
                //FansSex = "F",
                //AuthType = 1,
                //OrderRemark = 100,
                //IsReserve = true,
                //Source = 3001,//来源
                //FansCountURL = "http://image.xingyuan.com/FansCountURL",
                ////互动参数
                //MeidaType = 1,
                //AveragePointCount = 500,
                //AverageForwardCount = 499,
                AverageCommentCount = 378,
                ScreenShotURL = "http://image.xingyuan.com/ScreenShotURL",
            };
            var business = new MediaBusinessProxy(publicParams, new RequestMediaWeiXinDto(), new RequestMediaWeiBoDto(),
             new RequestMediaPcAppDto(), requestDto, new RequestMediaBroadcastDto());
            var retValue = business.Excute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void MediaAppCreataeTest()
        {
            var publicParams = CreateRequestMediaPublicParam(MediaType.APP, OperateType.Edit);
            var requestDto = new RequestMediaPcAppDto()
            {
                DailyLive = 100,
                //DailyIP = 3,
                Remark = "Remark",
                Terminal = ",28002",
                WebSite = "WebSite"
            };
            publicParams.MediaID = 1;
            var business = new MediaBusinessProxy(publicParams, new RequestMediaWeiXinDto(), new RequestMediaWeiBoDto(),
        requestDto, new RequestMediaVideoDto(), new RequestMediaBroadcastDto());
            var retValue = business.Excute();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void MediaAreaMappingTest()
        {
            var str = "12-0,8-810";
            var count = "12-0,8-810".Split(',').Length;
            Console.WriteLine(count);

            var list1 = str.Split(',');

            var lsWhere = list1.Where(s => s.Contains("-0") || s.Contains("-null"));

            if (lsWhere.Any())
            {
                //存在
            }

            foreach (var item in list1)
            {
                var list2 = item.Split('-');
                var item1 = list2[0];//省

                var ls = list2.ToList();
                ls.Add(item);
            }

            //MediaAreaMapping.Instance.BusinessCure(new RequestMediaPublicParam()
            //       {
            //           MediaID = 9,
            //           CoverageArea = "12-,8-810",
            //           BussinessType = 14005
            //       });
        }

        [TestMethod]
        public void JsonTest()
        {
            var list = new List<RequestMediaPublicParam>
            {
                new RequestMediaPublicParam() { },
                 new RequestMediaPublicParam() { }
            };

            var rquests = CreateRequestMediaPublicParam(MediaType.WeiXin, OperateType.Insert);

            Console.WriteLine(JsonConvert.SerializeObject(rquests));

            //  var strJSON = "{\"status\":0,\"message\":\"ok\",\"data\":{\"list\":[{\"PubID\":\"1211\",\"Number\":\"测试测试\",\"Name\":\"测试Name\",\"HeadIconURL\":\"http://image.xingyuan.com\",\"FansCount\":50000,\"UpdateCount\":\"10\",\"Status\":\"1\",\"PublishStatus\":\"1\",\"First\":[\"硬广直发：¥10,000\",\"硬广原创：¥10,000\",\"软广直发：¥10,000\",\"软广直发：¥10, 000\"],\"Second\":[\"硬广直发：¥10,000\"],\"Third\":[\"硬广直发：¥10,000\",\"硬广原创：¥10,000\"],\"Fourth\":[\"硬广直发：¥10,000\",\"硬广原创：¥10,000\",\"软广直发：¥10,000\"]}],\"TotalCount\":99}}" +
            //                "" +
            //                "";

            //  var json = new JsonResult()
            //{
            //    Status = 1,
            //    Message = "edd",
            //    Result = new PageSearch
            //    {
            //        List = list,
            //        TotleCount = 100
            //    }
            //};
            //  Console.WriteLine(JsonConvert.SerializeObject(json));
        }
    }

    public partial class JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 返回状态值的说明
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回Json对象
        /// </summary>
        public object Result { get; set; }
    }

    public class PageSearch
    {
        public int TotleCount { get; set; }
        public object List { get; set; }
    }
}