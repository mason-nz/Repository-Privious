using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper;
using XYAuto.ITSC.Chitunion2017.Test.AutoMapper.Dto;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class AutoMapperTest
    {
        public void Init()
        {
            Configuration.Configure();


            //Mapper.CreateMap<UserInfoDto, ResponseUserDto>();
            //Mapper.CreateMap<UserInfoDto, ResponseUserDto>().ForMember(s => s.LastUpdateUserId, 
            //    opt => opt.MapFrom(s => s.UserId + 1));
            //Mapper.CreateMap<List<UserInfoDto>, List<ResponseUserDto>>();
        }

        [TestMethod]
        public void Test()
        {
            Init();
            var userInfo = new UserInfoDto()
            {
                UserId = 1001,
                UserName = "CreateMap",
                CreateTime = DateTime.Now,
                Status = 1,
                Pwd = "00000000",
                IsAuth = "Y",
                Sex = 1
            };



            var bools = userInfo.IsAuth.Equals("Y", StringComparison.OrdinalIgnoreCase);


            var responseDto = Mapper.Map<UserInfoDto, ResponseUserDto>(userInfo);



            Console.WriteLine(JsonConvert.SerializeObject(responseDto));
        }

        [TestMethod]
        public void MapperListTest()
        {
            Init();
            var list = new List<UserInfoDto>()
            {
                new UserInfoDto() {
                UserId = 1001,
                UserName = "CreateMap",
                CreateTime = DateTime.Now,
                Status = 1,
                Pwd = "00000000"
            },new UserInfoDto()
            {
                UserId = 1002,
                UserName = "CreateMap_1002",
                CreateTime = DateTime.Now,
                Status = 2,
                Pwd = "1111111"
            }
            };


            var responseDto = Mapper.Map<List<UserInfoDto>, List<ResponseUserDto>>(list);

            Console.WriteLine(JsonConvert.SerializeObject(responseDto));
        }

        [TestMethod]
        public void MapperMediaWeiXinTest()
        {
            Configuration.Configure();
            var publicParam = new MediaOperateTest().
                CreateRequestMediaPublicParam(MediaType.WeiXin, OperateType.Insert);

            var mediaWeixinEntity = Mapper.Map<RequestMediaPublicParam, Entities.Media.MediaWeixin>(publicParam);
            Console.WriteLine("实体参数信息:{0}", JsonConvert.SerializeObject(mediaWeixinEntity));

            var requestMediaWeiXinDto = new RequestMediaWeiXinDto()
            {
                AreaID = 1001,
                OrigArticleCount = 100,
                AveragePointCount = 102,
                FansCountURL = "xxxxxx",
                FansFemalePer = 12,
                FansMalePer = 88,
              //  HeadIconURL = "ssssss",
                IsAuth = true,
                IsReserve = true,
                MaxinumReading = 19
            };

            var mediaWeixinEntity1 = Mapper.Map<RequestMediaWeiXinDto, Entities.Media.MediaWeixin>(requestMediaWeiXinDto);
            Console.WriteLine("二次返回参数信息：{0}", JsonConvert.SerializeObject(mediaWeixinEntity1));
        }

        [TestMethod]
        public void EntityMapperTest()
        {
            Configuration.Configure();
            var publicParam = new MediaOperateTest().
                CreateRequestMediaPublicParam(MediaType.WeiXin, OperateType.Insert);
            var requestMediaWeiXinDto = new RequestMediaWeiXinDto()
            {
                TwoCodeURL = "xxxx-TwoCodeURL",
                //HeadIconURL = "xxxx-HeadIconURL",
                FansMalePer = 88,
                FansFemalePer = 12,
                AreaID = 1001,
                LevelType = 1,
                FansCountURL = "xxxxxx",
                OrderRemark = "OrderRemark",
                Sign = "Sign",
                OrigArticleCount = 100,
                AveragePointCount = 102,
                IsAuth = true,
                IsReserve = true,
                MaxinumReading = 19
            };

            var entity = EntityMapper.Map<Entities.Media.MediaWeixin>(publicParam, requestMediaWeiXinDto);
            Console.WriteLine("二次返回参数信息：{0}", JsonConvert.SerializeObject(entity));
        }

        [TestMethod]
        public void ObjectDumperTest()
        {
            var publicParam = new MediaOperateTest().
               CreateRequestMediaPublicParam(MediaType.WeiXin, OperateType.Insert);
            ObjectDumper.Write(publicParam);

        }

        [TestMethod]
        public void SourceMapperTest()
        {
            Configuration.Configure();

            var sourceDto = new SourceDto { Value1 = 12, Value2 = 23 };

            var dest = Mapper.Map<SourceDto, Destination>(sourceDto);
            ObjectDumper.Write(dest);


            dest = Mapper.Map<Destination>(sourceDto);

            ObjectDumper.Write(dest);

        }

    }


}
