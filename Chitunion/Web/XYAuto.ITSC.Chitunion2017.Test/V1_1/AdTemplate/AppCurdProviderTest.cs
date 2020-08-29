/********************************************************
*创建人：lixiong
*创建时间：2017/6/10 12:10:38
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1.AdTemplate
{
    [TestClass]
    public class AppCurdProviderTest
    {
        private ConfigEntity _configEntity;
        private RequestMediaDto _requestMediaDto;
        private RequestAppDto _appDto;

        public AppCurdProviderTest()
        {
            MediaMapperConfig.Configure();

            _configEntity = new ConfigEntity();
            _requestMediaDto = new RequestMediaDto();
            // new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
        }

        internal RequestAppDto GetRequestAppDto()
        {
            return new RequestAppDto()
            {
                Name = "app_test_" + new Random().Next(1, 999),
                CityId = 1001,
                //CityName = "郑州市",
                ProvinceId = 10,
                //ProvinceName = "河南省",
                CommonlyClass = new List<CommonlyClassDto>() { new CommonlyClassDto()
                {
                    CategoryId = 21022,
                    CategoryName = "收藏家"
                } , new CommonlyClassDto()
                {
                    CategoryId = 21023,
                    //SortNumber = 1,
                    CategoryName = "数码达人"
                }},
                CoverageArea = new List<CoverageAreaDto>() { new CoverageAreaDto()
                {
                   CityId = 1001,
                CityName = "郑州市",
                ProvinceId = 10,
                ProvinceName = "河南省"
                } },
                Source = 1,
                DailyLive = 10088,
                HeadIconURL = "/sss/image",
                MediaID = 18,
                //BaseMediaID = 1,
                Remark = "ddddddd",
                OrderRemark = new List<OrderRemarkDto>()
                {
                    new OrderRemarkDto() { Id =  40001, Name = "不接竞品",Descript = "描述"}
                }
            };
        }

        internal RequestAppDto GetRequestAppDtoByUpdate()
        {
            return new RequestAppDto()
            {
                Name = "app_test_" + new Random().Next(1, 999),
                CityId = 1001,
                //CityName = "郑州市",
                ProvinceId = 10,
                //ProvinceName = "河南省",
                CommonlyClass = new List<CommonlyClassDto>() { new CommonlyClassDto()
                {
                    CategoryId = 21022,
                    CategoryName = "收藏家"
                } , new CommonlyClassDto()
                {
                    CategoryId = 21023,
                    //SortNumber = 1,
                    CategoryName = "数码达人"
                }},
                CoverageArea = new List<CoverageAreaDto>() { new CoverageAreaDto()
                {
                   CityId = 1001,
                CityName = "郑州市",
                ProvinceId = 10,
                ProvinceName = "河南省"
                } },
                Source = 1,
                DailyLive = 10088,
                HeadIconURL = "/sss/image",
                MediaID = 18,
                //BaseMediaID = 1,
                Remark = "ddddddd",
                OrderRemark = new List<OrderRemarkDto>()
                {
                    new OrderRemarkDto() { Id =  40001, Name = "不接竞品",Descript = "描述"}
                }
            };
        }

        internal RequestQualificationDto GetRequestQualificationDto()
        {
            return new RequestQualificationDto()
            {
                MediaRelations = 50001,
                EnterpriseName = "测试",
                BusinessLicense = "http://www.chitunion.com/UploadFiles/2017/6/16/10/Capture001$99da9056-652b-4bb5-b5a5-fe22f8f11f90.png",
                AgentContractFrontURL = "http://www.chitunion.com/UploadFiles/2017/6/16/10/Capture001$b5bc68dc-15bd-4cf3-bb24-16c4e561bdad.png",
                AgentContractBackURL = "http://www.chitunion.com/UploadFiles/2017/6/16/10/Capture001$485bde68-56e9-4a0f-82d3-1bc8014e55af.png",
                OperatingType = 1001
            };
        }

        public RequestMediaDto GetRequestAppDtoByInsert(RoleEnum roleEnum = RoleEnum.MediaOwner
            , OperateType operateType = OperateType.Insert)
        {
            _configEntity.RoleTypeEnum = roleEnum;
            _configEntity.CreateUserId = 1192;
            //baseParams
            _requestMediaDto.BusinessType = (int)MediaType.APP;
            _requestMediaDto.OperateType = (int)operateType;
            //app
            _requestMediaDto.App = GetRequestAppDto();
            //资质(媒体主添加分为2种：1.媒体+资质  2.资质添加)
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();

            _requestMediaDto.App.Qualification.MediaRelations = (int)MediaRelationsEnum.Own;
            _requestMediaDto.App.Qualification.OperatingType = (int)UserTypeEnum.个人;
            _requestMediaDto.App.Qualification.EnterpriseName = "真实姓名1";
            _requestMediaDto.App.Qualification.IDCardFrontURL = "/IDCardFrontURL";
            return _requestMediaDto;
        }

        public RequestMediaDto GetRequestAppDtoByUpdate(RoleEnum roleEnum = RoleEnum.MediaOwner
            , OperateType operateType = OperateType.Edit)
        {
            _configEntity.RoleTypeEnum = roleEnum;
            _configEntity.CreateUserId = 1192;
            //baseParams
            _requestMediaDto.BusinessType = (int)MediaType.APP;
            _requestMediaDto.OperateType = (int)operateType;
            //app
            _requestMediaDto.App = GetRequestAppDto();
            //资质(媒体主添加分为2种：1.媒体+资质  2.资质添加)
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();

            _requestMediaDto.App.Qualification.MediaRelations = (int)MediaRelationsEnum.Own;
            _requestMediaDto.App.Qualification.OperatingType = (int)UserTypeEnum.个人;
            _requestMediaDto.App.Qualification.EnterpriseName = "真实姓名1_update";
            _requestMediaDto.App.Qualification.IDCardFrontURL = "/IDCardFrontURL_update";
            return _requestMediaDto;
        }

        [TestMethod]
        public void app_insert_verify_false_base_qualification_params()
        {
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 94;
            //baseParams
            _requestMediaDto.BusinessType = (int)MediaType.APP;
            _requestMediaDto.OperateType = (int)OperateType.Insert;
            //app
            _requestMediaDto.App = GetRequestAppDto();
            //资质(媒体主添加分为2种：1.媒体+资质  2.资质添加)
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();

            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsTrue(retValue.HasError);
        }

        [Description("媒体主添加分为2种：1.媒体+资质  2.资质添加,验证：1，代理+企业")]
        [TestMethod]
        public void app_insert_verify_false_base_qualification1_params()
        {
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 94;
            //baseParams
            _requestMediaDto.BusinessType = (int)MediaType.APP;
            _requestMediaDto.OperateType = (int)OperateType.Insert;
            //app
            _requestMediaDto.App = GetRequestAppDto();
            //资质(媒体主添加分为2种：1.媒体+资质  2.资质添加)
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();

            _requestMediaDto.App.Qualification.MediaRelations = (int)MediaRelationsEnum.Proxy;
            _requestMediaDto.App.Qualification.OperatingType = (int)UserTypeEnum.企业;

            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsTrue(retValue.HasError);
        }

        [Description("媒体主添加分为2种：1.媒体+资质  2.资质添加,验证：1，自有+个人")]
        [TestMethod]
        public void app_insert_verify_false_base_qualification2_params()
        {
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 94;
            //baseParams
            _requestMediaDto.BusinessType = (int)MediaType.APP;
            _requestMediaDto.OperateType = (int)OperateType.Insert;
            //app
            _requestMediaDto.App = GetRequestAppDto();
            //资质(媒体主添加分为2种：1.媒体+资质  2.资质添加)
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();

            _requestMediaDto.App.Qualification.MediaRelations = (int)MediaRelationsEnum.Own;
            _requestMediaDto.App.Qualification.OperatingType = (int)UserTypeEnum.个人;
            _requestMediaDto.App.Qualification.EnterpriseName = "真实姓名";
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsTrue(retValue.HasError);

            _requestMediaDto.App.Qualification.EnterpriseName = string.Empty;
            _requestMediaDto.App.Qualification.IDCardFrontURL = "/IDCardFrontURL";

            retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsTrue(retValue.HasError);
        }

        [Description("添加操作：校验基本参数，常见分类（是否输入了参数，以及至少有一个主分类）")]
        [TestMethod]
        public void app_insert_verify_false_base_params_commonlyclass()
        {
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 94;
            //baseParams
            _requestMediaDto.BusinessType = (int)MediaType.APP;
            _requestMediaDto.OperateType = (int)OperateType.Insert;
            //app
            _requestMediaDto.App = GetRequestAppDto();
            //资质(媒体主添加分为2种：1.媒体+资质  2.资质添加)
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();

            _requestMediaDto.App.Qualification.MediaRelations = (int)MediaRelationsEnum.Own;
            _requestMediaDto.App.Qualification.OperatingType = (int)UserTypeEnum.个人;
            _requestMediaDto.App.Qualification.EnterpriseName = "真实姓名1";
            _requestMediaDto.App.Qualification.IDCardFrontURL = "/IDCardFrontURL";

            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsTrue(retValue.HasError);
        }

        [Description("添加操作：校验附表app名称的逻辑，媒体主角色：到个人、AE角色：到角色")]
        [TestMethod]
        public void app_insert_verify_false_AppNameByRole_mediaRole()
        {
            _requestMediaDto = GetRequestAppDtoByInsert();

            _requestMediaDto.App.Name = "一点资讯";
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsTrue(retValue.HasError);
        }

        [Description("app媒体添加:媒体主角色")]
        [TestMethod]
        public void app_insert_mediaRole_operate_test()
        {
            _requestMediaDto = GetRequestAppDtoByInsert();
            _requestMediaDto.App.CommonlyClass[0].SortNumber = 1;//设置一个主分类
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsFalse(retValue.HasError);
        }

        [Description("app媒体添加:AE角色")]
        [TestMethod]
        public void app_insert_ae_operate_test()
        {
            _requestMediaDto = GetRequestAppDtoByInsert();
            _requestMediaDto.App.CommonlyClass[0].SortNumber = 1;//设置一个主分类

            var requestDtoJson =
                "{\"WeiXin\":null,\"App\":{\"MediaID\":0,\"BaseMediaID\":0,\"Name\":\"45435_test\",\"HeadIconURL\":\"/UploadFiles/2017/6/20/17/Capture001$97ee10d1-e3eb-4e03-b3ea-4d7f98d28e14.png\",\"DailyLive\":111,\"Remark\":\"11\",\"ProvinceId\":-2,\"CityId\":-2,\"CommonlyClass\":[{\"CategoryId\":52001,\"SortNumber\":1,\"CategoryName\":null}],\"CoverageArea\":null,\"OrderRemark\":null,\"Qualification\":null,\"CreateUserId\":0,\"Source\":0},\"Temp\":null,\"BusinessType\":14002,\"OperateType\":1}";

            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(requestDtoJson);

            _configEntity.RoleTypeEnum = RoleEnum.AE; ;

            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsFalse(retValue.HasError);
        }

        [Description("app媒体编辑:媒体主角色")]
        [TestMethod]
        public void app_update_mediaRole_operate_test()
        {
            var requestDtoJson =
                "{\"WeiXin\":null,\"App\":{\"MediaID\":427,\"BaseMediaID\":0,\"Name\":\"大鱼旅行\",\"HeadIconURL\":\"/UploadFiles/2017/6/16/17/1$c94a551f-57a7-4490-89a3-be0b34040b18.jpg\",\"DailyLive\":100000,\"Remark\":\"大鱼·旅行猎人计划旨在招募更多的与大鱼有相同理念的用户，到大鱼的目的地寻找有趣的旅行产品，并通过与当地供应商的沟通，让供应商与大鱼建立联系，并且最终让这些旅行产品在大鱼上线，服务到更多的客人。旅行产品一经上线，猎人们会得到相应的奖金奖励.\",\"ProvinceId\":2,\"CityId\":201,\"CommonlyClass\":[{\"CategoryId\":52007,\"SortNumber\":1,\"CategoryName\":null},{\"CategoryId\":52011,\"SortNumber\":0,\"CategoryName\":null},{\"CategoryId\":52015,\"SortNumber\":0,\"CategoryName\":null}],\"CoverageArea\":[{\"ProvinceId\":0,\"ProvinceName\":null,\"CityId\":-2,\"CityName\":null}],\"OrderRemark\":[{\"Id\":40001,\"Name\":null,\"Descript\":null},{\"Id\":40003,\"Name\":null,\"Descript\":null},{\"Id\":40004,\"Name\":null,\"Descript\":null},{\"Id\":40005,\"Name\":null,\"Descript\":null},{\"Id\":40007,\"Name\":null,\"Descript\":null},{\"Id\":40008,\"Name\":null,\"Descript\":null},{\"Id\":40009,\"Name\":null,\"Descript\":\"不接软广\"}],\"Qualification\":{\"EnterpriseName\":\"大鱼科技有限公司\",\"BusinessLicense\":\"/UploadFiles/Personal/2017/6/12/14/d6183ea4-ea5a-46bd-92c3-6fd8dba8d7fa.png\",\"IDCardFrontURL\":null,\"IDCardBackURL\":null,\"AgentContractFrontURL\":null,\"AgentContractBackURL\":null,\"MediaRelations\":50002,\"OperatingType\":1001},\"CreateUserId\":0,\"Source\":0},\"Temp\":null,\"BusinessType\":14002,\"OperateType\":2}";

            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(requestDtoJson);

            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            //_requestMediaDto = GetRequestAppDtoByInsert();
            // _requestMediaDto.App.CommonlyClass[0].SortNumber = 1;//设置一个主分类
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsFalse(retValue.HasError);
        }

        [Description("app媒体编辑: 运营角色-基表信息")]
        [TestMethod]
        public void app_update_yunying_role_operate_test()
        {
            var requestDtoJson =
                "{\"WeiXin\":null,\"App\":{\"MediaID\":0,\"BaseMediaID\":8,\"Name\":\"花点时间\",\"HeadIconURL\":\"/UploadFiles/2017/6/19/13/8ad4b31c8701a18bbef9f231982f07082838feba$db9792a3-b9c5-4842-b08b-fa535cd645db.jpg\",\"DailyLive\":111,\"Remark\":\"我感受到的\",\"ProvinceId\":2,\"CityId\":201,\"CommonlyClass\":[{\"CategoryId\":52005,\"SortNumber\":1,\"CategoryName\":null}],\"CoverageArea\":[{\"ProvinceId\":0,\"ProvinceName\":null,\"CityId\":-2,\"CityName\":null}],\"OrderRemark\":[{\"Id\":40003,\"Name\":null,\"Descript\":null},{\"Id\":40009,\"Name\":null,\"Descript\":\"的点点滴滴\"}],\"Qualification\":null,\"CreateUserId\":0,\"Source\":0},\"Temp\":null,\"BusinessType\":14002,\"OperateType\":2}";

            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(requestDtoJson);

            _configEntity.RoleTypeEnum = RoleEnum.SupperAdmin;
            //_requestMediaDto = GetRequestAppDtoByInsert();
            // _requestMediaDto.App.CommonlyClass[0].SortNumber = 1;//设置一个主分类
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsFalse(retValue.HasError);
        }

        [Description("app媒体详情")]
        [TestMethod]
        public void app_media_get_info_test()
        {
            var requestDto = new RequestGetMeidaInfoDto()
            {
                BusinessType = (int)MediaType.APP,
                //BaseMediaId = 41
                MediaId = 464
            };
            requestDto.CreateUserId = 1125;
            var info = new MediaOperateProxy(requestDto, new ConfigEntity()
            {
                RoleTypeEnum = RoleInfoMapping.GetUserRole(requestDto.CreateUserId),
            }).QueryInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void app_MediaOrderRemarkInsertByBulk_test()
        {
            _requestMediaDto = GetRequestAppDtoByInsert();
            _requestMediaDto.App.CommonlyClass[0].SortNumber = 1;//设置一个主分类
            var retValue = new ReturnValue();

            _requestMediaDto.App.OrderRemark = new List<OrderRemarkDto>()
            {
                new OrderRemarkDto() { Id = 40001},
                new OrderRemarkDto() { Id = 40009}
            };

            retValue = new AppOperate(_configEntity, _requestMediaDto.App).MediaOrderRemarkInsertByBulk(retValue, 1545,
                MediaRelationType.Attached);
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void app_MediaQualificationInsert_test()
        {
            _requestMediaDto = GetRequestAppDtoByInsert();
            _requestMediaDto.App.CommonlyClass[0].SortNumber = 1;//设置一个主分类
            var retValue = new ReturnValue();

            _requestMediaDto.App.OrderRemark = new List<OrderRemarkDto>()
            {
                new OrderRemarkDto() { Id = 40001},
                new OrderRemarkDto() { Id = 40009}
            };

            _requestMediaDto.App.MediaID = 1545;
            _requestMediaDto.App.Qualification = GetRequestQualificationDto();
            retValue = new AppOperate(_configEntity, _requestMediaDto.App).MediaQualificationInsert(retValue,
                _requestMediaDto.App.MediaID);
            Console.WriteLine(retValue.Message);
            Assert.ReplaceNullChars(retValue.Message);
            Assert.IsFalse(retValue.HasError);
        }
    }
}