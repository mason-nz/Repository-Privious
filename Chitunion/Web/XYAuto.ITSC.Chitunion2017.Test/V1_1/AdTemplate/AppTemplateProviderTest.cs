/********************************************************
*创建人：lixiong
*创建时间：2017/6/10 16:33:03
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1.AdTemplate
{
    [TestClass]
    public class AppTemplateProviderTest
    {
        private ConfigEntity _configEntity;
        private RequestMediaDto _requestMediaDto;
        private RequestTemplateDto _requestTemplateDto;

        public int InsertTemplateId { get; set; }

        public AppTemplateProviderTest()
        {
            MediaMapperConfig.Configure();
            //_requestTemplateDto = new RequestTemplateDto();
            _requestMediaDto = new RequestMediaDto();
            _configEntity = new ConfigEntity();
        }

        public RequestTemplateDto GeTemplateDto()
        {
            var returnDto = new RequestTemplateDto()
            {
                AdDescription = "广告模板说明、描述",
                AdDisplay = "广告展示逻辑",
                AdDisplayLength = 4,
                AdForm = (int)AppTemplateAdFormEnum.Banner,
                AdLegendUrl = "/image/dddd",
                AdTemplateName = "名称",
                AdTempStyle = new List<AdTempStyleDto>()
                {
                    new AdTempStyleDto()
                    {
                        AdStyle = "style_1",
                        AdTemplateID = 1,
                        BaseMediaID = 1
                    }
                },
                BaseMediaId = 1,
                BaseAdId = 1,
                Remarks = "描述",
                SellingMode = 4,
                SellingPlatform = 3,
                CarouselCount = 7,
                OriginalFile = "刊例原件",
                AdSaleAreaGroup = new List<AdSaleAreaGroupDto>()
                {
                    new AdSaleAreaGroupDto()
                    {
                        DetailArea = new List<AdSaleAreaGroupDetailDto>()
                        {
                            new AdSaleAreaGroupDetailDto()
                            {
                                CityId = 1001,
                                CityName = "郑州市",

                                ProvinceId = 10,
                                ProvinceName = "河南省"
                            },
                            new AdSaleAreaGroupDetailDto()
                            {
                                CityId = 1002,
                                CityName = "洛阳市",
                                ProvinceId = 10,
                                ProvinceName = "河南省"
                            }
                        },
                        GroupId = 11,
                        GroupName = "城市组1",
                        GroupType = 1,
                        IsPublic = 0
                    }
                }
            };

            return returnDto;
        }

        public RequestMediaDto GetRequestMediaDtoByInsert(OperateType operateType = OperateType.Insert)
        {
            //base
            _requestMediaDto.Temp = GeTemplateDto();
            _requestMediaDto.OperateType = (int)operateType;
            _requestMediaDto.BusinessType = (int)MediaType.Template;
            //config
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 1192;
            _configEntity.CureOperateType = operateType;

            return _requestMediaDto;
        }

        [Description("模板添加操作：验证基本参数，模板名称已存在")]
        [TestMethod]
        public void template_insert_verify_templateName_test()
        {
            _requestMediaDto = GetRequestMediaDtoByInsert();

            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsTrue(retValue.HasError);
        }

        [Description("模板添加操作：")]
        [TestMethod]
        public void template_insert_test()
        {
            _requestMediaDto = GetRequestMediaDtoByInsert();
            _requestMediaDto.Temp.AdTemplateName = "test_" + new Random().Next(1, 999);
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();

            InsertTemplateId = Convert.ToInt32(retValue.ReturnObject);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板添加操作：json请求")]
        [TestMethod]
        public void template_insert_byjson_test()
        {
            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":137,\"BaseAdId\":-2,\"BaseMediaId\":24,\"MediaId\":0,\"AdTemplateName\":\"弹窗\",\"OriginalFile\":\"/UploadFiles/2017/6/28/16/10$c8cf4c6a-bac5-46cf-8f3b-5a7aed7125df.png\",\"AdForm\":51010,\"AdFormName\":null,\"CarouselCount\":1,\"SellingPlatform\":7,\"SellingMode\":1,\"AdLegendUrl\":\"/UploadFiles/2017/6/28/17/10$5a8dcd94-6f37-4f3b-b920-dd68fb823c7a.png\",\"AdDisplay\":null,\"AdDescription\":null,\"Remarks\":null,\"AdDisplayLength\":0,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"IsModified\":true,\"AdTempStyle\":[{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":283,\"IsPublic\":0,\"AdStyle\":\"图片\",\"CreateUserId\":0}],\"AdSaleAreaGroup\":[{\"GroupId\":-2,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"华东\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":201,\"CityName\":\"北京市\"}]}]},\"BusinessType\":15000,\"OperateType\":2}";

            //_requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);
            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CreateUserId = 1191;
            _configEntity.CureOperateType = OperateType.Insert; //(OperateType)_requestMediaDto.OperateType;

            _requestMediaDto = new RequestMediaDto()
            {
                BusinessType = 15000,
                OperateType = (int)OperateType.Insert,
                Temp = new RequestTemplateDto()
                {
                    BaseMediaId = 33,
                    OriginalFile = "无",
                    AdTemplateName = "开屏",
                    AdForm = 51003,
                    CarouselCount = 3,
                    SellingPlatform = 4,
                    SellingMode = 1,
                    AdDisplayLength = 1,
                    AdDescription =
                        "图片：1125*1600px，静态：jpg格式，600kb以内，动态：gif格式，1M以内；底部安全区域 130px，右上角安全区域 280px*140px，安全区域可放图形，不能放主视觉元素（如公司logo、Button按钮等）",
                    AdDisplay = "",
                    AdLegendUrl = "",
                    Remarks = "不可配送",
                    CreateUserId = 1191,
                    CreateTime = DateTime.Now,
                    AuditStatus = 48002,
                    AdSaleAreaGroup = new List<AdSaleAreaGroupDto>()
                    {
                        new AdSaleAreaGroupDto()
                        {
                            GroupName = "全国",
                            GroupType = 0
                        },
                        new AdSaleAreaGroupDto()
                        {
                            GroupName = "一级城市",
                            GroupType = 1,
                            DetailArea = new List<AdSaleAreaGroupDetailDto>()
                            {
                                new AdSaleAreaGroupDetailDto()
                                {
                                    ProvinceId = 10,
                                    CityId = 1001
                                }
                            }
                        }
                    },
                    AdTempStyle = new List<AdTempStyleDto>()
                    {
                        new AdTempStyleDto()
                        {
                            BaseMediaID = 33,
                            AdStyle = "静态3s可外链",
                            CreateUserId = 1191
                        },
                        new AdTempStyleDto()
                        {
                            BaseMediaID = 33,
                            AdStyle = "静态5s可外链",
                            CreateUserId = 1191
                        },
                        new AdTempStyleDto()
                        {
                            BaseMediaID = 33,
                            AdStyle = "动态5s可外链",
                            CreateUserId = 1191
                        }
                    }
                }
            };
            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();

            //InsertTemplateId = Convert.ToInt32(retValue.ReturnObject);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板添加操作：套用公共模板")]
        [TestMethod]
        public void template_insert_by_adbaseId_test()
        {
            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":-2,\"BaseAdId\":208,\"BaseMediaId\":33,\"MediaId\":73,\"AdTemplateName\":\"百词斩运营新增模板6\",\"OriginalFile\":\"/UploadFiles/2017/7/5/15/虎牙直播$22e07a2a-af9d-4bd2-a7ef-9f0b05a812bb.png\",\"AdForm\":51011,\"AdFormName\":null,\"CarouselCount\":4,\"SellingPlatform\":7,\"SellingMode\":3,\"AdLegendUrl\":\"/UploadFiles/2017/6/30/18/1$b2702f4e-9403-4f72-b42b-2164c5fc1d82.jpg,/UploadFiles/2017/6/30/18/2abb2142f82bcb42af8afffb68d892a7$cad50c4b-9568-438e-a74f-22353e6954d8.png,/UploadFiles/2017/6/30/18/2db0dc0d14976e12af882f9d82cafd52$7e4e6216-c2e8-4677-8387-a5ccd9c7f88e.png\",\"AdDisplay\":\"百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6\",\"AdDescription\":\"百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6\",\"Remarks\":\"百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6，百词斩运营新增模板6\\n              \\n              \\n              \\n              \",\"AdDisplayLength\":1,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"IsModified\":false,\"AdTempStyle\":null,\"AdSaleAreaGroup\":[{\"GroupId\":480,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"F城\",\"DetailArea\":null},{\"GroupId\":481,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"G城\",\"DetailArea\":null},{\"GroupId\":500,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"b城\",\"DetailArea\":null}]},\"BusinessType\":15000,\"OperateType\":1}";
            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            _configEntity.RoleTypeEnum = RoleEnum.AE;
            _configEntity.CreateUserId = 14;
            _configEntity.CureOperateType = OperateType.Insert; //(OperateType)_requestMediaDto.OperateType;

            var retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板添加：异步处理程序")]
        [TestMethod]
        public void template_insert_TaskToRunAdTemplateRelation_test()
        {
            _requestMediaDto = GetRequestMediaDtoByInsert();
            _requestMediaDto.Temp.AdTemplateName = "test_" + new Random().Next(1, 999);
            _requestMediaDto.Temp.TemplateId = 1;
            var retValue = new ReturnValue();

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            provider.TaskToRunAdTemplateRelation(retValue, _requestMediaDto.Temp.TemplateId,
                _requestMediaDto.Temp.BaseMediaId);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板添加：异步处理程序_反序列化测试")]
        [TestMethod]
        public void template_insert_TaskToRunAdTemplateRelation_json_test()
        {
            //_requestMediaDto = GetRequestMediaDtoByInsert();

            var retValue = new ReturnValue();

            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":95,\"BaseAdId\":-2,\"BaseMediaId\":8,\"MediaId\":0,\"AdTemplateName\":\"测试新增\",\"OriginalFile\":\"/UploadFiles/2017/6/26/20/10$8735f4ac-3bb7-4ad1-b235-26fe0254b179.png\",\"AdForm\":51006,\"AdFormName\":null,\"CarouselCount\":4,\"SellingPlatform\":63,\"SellingMode\":3,\"AdLegendUrl\":\"/UploadFiles/2017/6/26/20/10$727b6574-9467-4505-8d9d-e1ad84c2c07a.png\",\"AdDisplay\":\"123\",\"AdDescription\":\"123\",\"Remarks\":\"123\",\"AdDisplayLength\":2,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"IsModified\":false,\"AdTempStyle\":[{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":183,\"IsPublic\":0,\"AdStyle\":\"大图\",\"CreateUserId\":0},{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":-2,\"IsPublic\":0,\"AdStyle\":\"动画\",\"CreateUserId\":0}],\"AdSaleAreaGroup\":[{\"GroupId\":232,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"二级\",\"DetailArea\":null},{\"GroupId\":-2,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"三级\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":201,\"CityName\":\"北京市\"},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2401,\"CityName\":\"上海市\"}]}]},\"BusinessType\":15000,\"OperateType\":2}";
            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CreateUserId = 55;
            _configEntity.CureOperateType = (OperateType)_requestMediaDto.OperateType;

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            provider.TaskToRunAdTemplate(retValue, _requestMediaDto.Temp.TemplateId,
                _requestMediaDto.Temp.BaseMediaId);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板城市组，样式测试数据")]
        [TestMethod]
        public void template_TaskToRunAdTemplateRelation_json_test()
        {
            //_requestMediaDto = GetRequestMediaDtoByInsert();

            var retValue = new ReturnValue();

            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":4,\"BaseAdId\":2,\"BaseMediaId\":2,\"MediaId\":0,\"AdTemplateName\":\"dddddd\",\"OriginalFile\":\"/UploadFiles/2017/6/26/20/10$8735f4ac-3bb7-4ad1-b235-26fe0254b179.png\",\"AdForm\":51006,\"AdFormName\":null,\"CarouselCount\":4,\"SellingPlatform\":63,\"SellingMode\":3,\"AdLegendUrl\":\"/UploadFiles/2017/6/26/20/10$727b6574-9467-4505-8d9d-e1ad84c2c07a.png\",\"AdDisplay\":\"123\",\"AdDescription\":\"123\",\"Remarks\":\"123\",\"AdDisplayLength\":2,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"IsModified\":false,\"AdTempStyle\":[{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":183,\"IsPublic\":0,\"AdStyle\":\"大图\",\"CreateUserId\":0},{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":-2,\"IsPublic\":0,\"AdStyle\":\"动画\",\"CreateUserId\":0}],\"AdSaleAreaGroup\":[{\"GroupId\":232,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"二级\",\"DetailArea\":null},{\"GroupId\":-2,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"三级\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":201,\"CityName\":\"北京市\"},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2401,\"CityName\":\"上海市\"}]}]},\"BusinessType\":15000,\"OperateType\":2}";
            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CreateUserId = 1188;

            _configEntity.CureOperateType = (OperateType)_requestMediaDto.OperateType;
            _requestMediaDto.Temp.TemplateId = 4;

            #region AdSaleAreaGroup

            _requestMediaDto.Temp.AdSaleAreaGroup = new List<AdSaleAreaGroupDto>()
            {
                new AdSaleAreaGroupDto()
                {
                    DetailArea = new List<AdSaleAreaGroupDetailDto>()
                    {
                        //new AdSaleAreaGroupDetailDto()
                        //{
                        //    CityId = 1001,
                        //    CityName = "郑州市",
                        //    ProvinceId = 10,
                        //    IsPublic =  0,
                        //    ProvinceName = "河南省"
                        //},
                        //new AdSaleAreaGroupDetailDto()
                        //{
                        //    CityId = 1002,
                        //    CityName = "洛阳市",
                        //    ProvinceId = 10,
                        //       IsPublic =  0,
                        //    ProvinceName = "河南省"
                        //},
                        //new AdSaleAreaGroupDetailDto()
                        //{
                        //    CityId = 1003,
                        //    CityName = "周口",
                        //    ProvinceId = 10,
                        //       IsPublic =  0,
                        //    ProvinceName = "河南省"
                        //},
                        new AdSaleAreaGroupDetailDto()
                        {
                            CityId = 1005,
                            CityName = "新乡",
                            ProvinceId = 10,
                            IsPublic = 1,
                            ProvinceName = "河南省"
                        }
                    },
                    GroupId = 39,
                    GroupName = "城市组2",
                    GroupType = 1,
                    IsPublic = 1
                },
                new AdSaleAreaGroupDto()
                {
                    GroupId = 4,
                    GroupName = "阿勒泰/阿克",
                    GroupType = 1,
                    IsPublic = 1,
                    DetailArea = new List<AdSaleAreaGroupDetailDto>()
                    {
                        new AdSaleAreaGroupDetailDto()
                        {
                            CityId = 1006,
                            CityName = "商丘",
                            ProvinceId = 10,
                            IsPublic = 1,
                            ProvinceName = "河南省"
                        }
                    }
                }
            };

            #endregion AdSaleAreaGroup

            //_requestMediaDto.Temp.IsModified = true;

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            provider.TaskToRunAdTemplate(retValue, _requestMediaDto.Temp.TemplateId,
                _requestMediaDto.Temp.BaseMediaId);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板编辑操作：验证模板名称")]
        [TestMethod]
        public void template_update_verify_false_templateName_test()
        {
        }

        [Description("模板查询：详情-编辑自己的驳回的")]
        [TestMethod]
        public void template_get_info_attach_test()
        {
            var requestDto = new RequestGetMeidaInfoDto()
            {
                BusinessType = (int)MediaType.Template,
                AdTempId = InsertTemplateId > 0 ? InsertTemplateId : 8
            };
            var info = new MediaOperateProxy(requestDto, new ConfigEntity()).QueryInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("模板查询：详情-编辑套用模板（公共模板）信息")]
        [TestMethod]
        public void template_get_info_test()
        {
            InsertTemplateId = 108;
            var requestDto = new RequestGetMeidaInfoDto()
            {
                BusinessType = (int)MediaType.Template,
                AdBaseTempId = InsertTemplateId > 0 ? InsertTemplateId : 8
            };
            var info = new MediaOperateProxy(requestDto, new ConfigEntity()).QueryInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("模板审核页面：左边的公共模板信息")]
        [TestMethod]
        public void template_get_auditlist_LeftParentTemplateInfo_test()
        {
            var requestDto = new RequestTemplateInfoDto()
            {
                AdTempId = 4
            };

            var adProvider = new AdTemplateProvider(new ConfigEntity(), requestDto).GetAuditViewList(true);
            Console.WriteLine(JsonConvert.SerializeObject(adProvider));
        }

        [Description("模板审核页面：右边的模板信息--单选审核")]
        [TestMethod]
        public void template_getauditlist_GetRightTemplateList_for_mediaId_test()
        {
            var requestDto = new RequestTemplateInfoDto()
            {
                AdTempId = 11,
                BaseMediaId = 21,
                PageSize = 6,
                PageIndex = 1
            };

            var adProvider = new AdTemplateProvider(new ConfigEntity(), requestDto).GetAuditViewList(false);
            Console.WriteLine(JsonConvert.SerializeObject(adProvider));
        }

        [Description("模板审核页面：右边的模板信息--多选审核")]
        [TestMethod]
        public void template_getauditlist_GetRightTemplateList_for_idlist_test()
        {
            var requestDto = new RequestTemplateInfoDto()
            {
                AdTempIdList = "30,29,27",
                BaseMediaId = 20,
                PageSize = 6,
                PageIndex = 1
            };

            var adProvider = new AdTemplateProvider(new ConfigEntity(), requestDto).GetAuditViewList(false);
            Console.WriteLine(JsonConvert.SerializeObject(adProvider));
        }

        [Description("运营角色：模板列表")]
        [TestMethod]
        public void template_getlist_AdTemplateYunYingQuery_test()
        {
            var requestDto = new RequestAdQueryDto
            {
                CreateUserId = 1192,
                BusinessType = (int)MediaType.Template,
                TemplateAuditStatus = (int)Entities.Enum.AppTemplateEnum.待审核
            };

            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.YunYingOperate // RoleInfoMapping.GetUserRole(requestDto.CreateUserId)
            };
            var templateList = new AdQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(templateList));
        }

        [Description("模板列表：数据统计接口")]
        [TestMethod]
        public void template_getlist_GetAdTemplateStatisticsCount_test()
        {
            //后台查询-需注意角色,如果有用户Id ，则带入参数

            var userId = 1192;
            var publishStatisticsCount = new AdQueryProxy(new ConfigEntity()
            {
                CreateUserId = userId,
                RoleTypeEnum = RoleEnum.YunYingOperate //RoleInfoMapping.GetUserRole(userId)
            }, null).GetStatisticsCount(new PublishSearchAutoQuery<PublishStatisticsCount>()
            {
                BusinessType = (int)MediaType.Template,
                CreateUserId = userId
            });
            Console.WriteLine(JsonConvert.SerializeObject(publishStatisticsCount));
        }

        [Description("app媒体列表：媒体主角色-审核通过")]
        [TestMethod]
        public void media_getlist_role_mediaOwn_pass_test()
        {
            var requestDto = new RequestMediaAppQueryDto
            {
                BusinessType = (int)MediaType.APP,
                CreateUserId = 41,
                IsPassed = true,
                AuditStatus = ((int)MediaAuditStatusEnum.AlreadyPassed).ToString()
            };
            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.MediaOwner
            };

            var list = new MediaQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("app媒体列表：媒体主角色-审核不通过（待审核）")]
        [TestMethod]
        public void media_getlist_role_mediaOwn_not_pass_PendingAudit_test()
        {
            var requestDto = new RequestMediaAppQueryDto
            {
                BusinessType = (int)MediaType.APP,
                CreateUserId = 1192,
                IsPassed = false,
                AuditStatus = ((int)MediaAuditStatusEnum.PendingAudit).ToString()
            };
            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.MediaOwner
            };

            var list = new MediaQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("app媒体列表：媒体主角色-审核不通过（驳回）")]
        [TestMethod]
        public void media_getlist_role_mediaOwn_not_pass_RejectNotPass_test()
        {
            var requestDto = new RequestMediaAppQueryDto
            {
                BusinessType = (int)MediaType.APP,
                CreateUserId = 1192,
                IsPassed = false,
                AuditStatus = ((int)MediaAuditStatusEnum.RejectNotPass).ToString()
            };
            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.MediaOwner
            };

            var list = new MediaQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("app媒体列表：AE-审核通过(AE 都是通过状态)")]
        [TestMethod]
        public void media_getlist_role_ae_pass_test()
        {
            var requestDto = new RequestMediaAppQueryDto
            {
                BusinessType = (int)MediaType.APP,
                CreateUserId = 1192,
                IsPassed = true,
                AuditStatus = ((int)MediaAuditStatusEnum.AlreadyPassed).ToString()
            };
            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.AE
            };

            var list = new MediaQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("app媒体列表：运营角色（不是审核页面）、查看的都是基表的信息内容")]
        [TestMethod]
        public void media_getlist_role_admin_test()
        {
            var requestDto = new RequestMediaAppQueryDto
            {
                BusinessType = (int)MediaType.APP,
            };
            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.YunYingOperate
            };

            var list = new MediaQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("app媒体列表：运营角色（审核页面）_待审核,驳回")]
        [TestMethod]
        public void media_getlist_role_admin_audit_IsNotPassed_list_test()
        {
            var requestDto = new RequestMediaAppQueryDto
            {
                BusinessType = (int)MediaType.APP,
                IsAuditView = true,
                IsPassed = false,
                AuditStatus = ((int)MediaAuditStatusEnum.AlreadyPassed).ToString(),
                SubmitUser = " 方圆"
            };
            //后台查询-需注意角色,如果有用户Id ，则带入参数
            var config = new ConfigEntity()
            {
                CreateUserId = requestDto.CreateUserId,
                RoleTypeEnum = RoleEnum.YunYingOperate
            };

            var list = new MediaQueryProxy(config, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("app模板：添加、编辑操作之前，验证模板名称，返回已存在的模板id")]
        [TestMethod]
        public void template_insert_VerifyAdTemplateName_false_test()
        {
            var requestDto = new RequestTemplateInfoDto()
            {
                AdTempName = "首页弹窗",
                BaseMediaId = 24,
                MediaId = 55
            };
            var provider =
                new AdTemplateProvider(new ConfigEntity()
                {
                    CureOperateType = OperateType.Insert,
                    RoleTypeEnum = RoleEnum.MediaOwner,
                    CreateUserId = 55
                }, requestDto).VerifyAdTemplateName();

            Console.WriteLine(JsonConvert.SerializeObject(provider));

            Console.WriteLine(JsonConvert.SerializeObject(provider.GetValue("verify_name_id")));
            Assert.IsFalse(provider.HasError);
        }

        [Description("app模板：添加、编辑操作之前，验证模板名称，返回已存在的模板id")]
        [TestMethod]
        public void template_update_VerifyAdTemplateName_false_test()
        {
            var requestDto = new RequestTemplateInfoDto()
            {
                AdTempName = "612模板测试1",
                AdTempId = 9
            };
            var provider =
                new AdTemplateProvider(new ConfigEntity()
                {
                    CureOperateType = OperateType.Edit
                }, requestDto).VerifyAdTemplateName();

            Console.WriteLine(JsonConvert.SerializeObject(provider));

            Console.WriteLine(JsonConvert.SerializeObject(provider.GetValue("verify_name_id")));
            Assert.IsFalse(provider.HasError);
        }

        [TestMethod]
        public void template_update_AdminAuditUpdateInfo_test()
        {
            var retValue = new ReturnValue();
            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":31,\"BaseAdId\":-2,\"BaseMediaId\":4,\"MediaId\":0,\"AdTemplateName\":\"AE189-途牛-焦点图\",\"OriginalFile\":\"/UploadFiles/2017/6/22/19/bc0b2b5aef0de5bbd584bbc15bfc166b$5650b6cd-75ea-4f9f-90fa-aefa48bf7d9d.jpg\",\"AdForm\":51006,\"AdFormName\":null,\"CarouselCount\":5,\"SellingPlatform\":31,\"SellingMode\":3,\"AdLegendUrl\":\"/UploadFiles/2017/6/22/19/img2$f6fc8e15-61c3-4a74-b61c-bee10797b388.png,/UploadFiles/2017/6/22/19/img3$318fadf1-8f4a-4f95-b33e-1ba639fe635a.png,/UploadFiles/2017/6/22/19/img4$9bd0eaa9-9bf4-4b39-b33b-1e17887cea47.png\",\"AdDisplay\":\"AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图\",\"AdDescription\":\"AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图\",\"Remarks\":\"AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图AE189-途牛-焦点图\",\"AdDisplayLength\":0,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"AdTempStyle\":[{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":36,\"IsPublic\":0,\"AdStyle\":\"幻灯片静态\",\"CreateUserId\":0},{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":37,\"IsPublic\":0,\"AdStyle\":\"幻灯片动态\",\"CreateUserId\":0}],\"AdSaleAreaGroup\":[{\"GroupId\":60,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"广州\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"CityId\":501,\"CityName\":\"广州\"}]},{\"GroupId\":61,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"深圳\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"CityId\":502,\"CityName\":\"深圳\"}]},{\"GroupId\":-2,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"安顺\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"CityId\":705,\"CityName\":\"安顺\"}]}]},\"BusinessType\":15000,\"OperateType\":2}";

            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            _configEntity.CreateUserId = 4;
            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CureOperateType = OperateType.Edit;

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);

            //校验一下，获取编辑之前的信息
            provider.VerifyUpdateBusiness(retValue);

            provider.AdminAuditUpdateInfo(retValue);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void template_update_mediaRole_byjson_test()
        {
            var retValue = new ReturnValue();
            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":127,\"BaseAdId\":-2,\"BaseMediaId\":16,\"MediaId\":38,\"AdTemplateName\":\"lixiong-测试1\",\"OriginalFile\":\"/UploadFiles/2017/6/27/17/1$a27bc3f3-c196-4427-bff1-e4d27de8a0df.png\",\"AdForm\":51001,\"AdFormName\":null,\"CarouselCount\":2,\"SellingPlatform\":7,\"SellingMode\":3,\"AdLegendUrl\":\"/UploadFiles/2017/6/27/18/37$0ac818d9-a3c0-48e3-892a-f5878ea55836.png,/UploadFiles/2017/6/27/18/1$ecaacdc5-2147-4181-9338-fd2735975588.png,/UploadFiles/2017/6/27/18/3$70da33b6-70e3-4a59-a4b9-f3803d65f6ad.png\",\"AdDisplay\":null,\"AdDescription\":null,\"Remarks\":null,\"AdDisplayLength\":0,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"IsModified\":false,\"AdTempStyle\":[{\"BaseMediaID\":-2,\"AdTemplateID\":-2,\"AdStyleId\":0,\"IsPublic\":0,\"AdStyle\":\"驳回-套用模板-新增06\",\"CreateUserId\":0}],\"AdSaleAreaGroup\":[{\"GroupId\":306,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"ddd\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2822,\"CityName\":\"巴州\"}]},{\"GroupId\":-2,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"e\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":909,\"CityName\":\"沧州\"}]},{\"GroupId\":309,\"IsPublic\":0,\"GroupType\":-1,\"GroupName\":\"其他城市\",\"DetailArea\":null}]},\"BusinessType\":15000,\"OperateType\":2}";

            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            _configEntity.CreateUserId = 1129;
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CureOperateType = OperateType.Edit;

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);

            retValue = new MediaOperateProxy(_requestMediaDto, _configEntity).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [Description("运营修正-审核通过-修改模板指向引用父模板id")]
        [TestMethod]
        public void template_AdTemplateGroupCityOnModified_test()
        {
            var retValue = new ReturnValue();
            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CreateUserId = 35;
            _requestMediaDto = GetRequestMediaDtoByInsert();
            _requestMediaDto.Temp.TemplateId = 51;
            _requestMediaDto.Temp.IsModified = true;

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            provider.AdTemplateGroupCityOnModified(retValue, _requestMediaDto.Temp.TemplateId);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void template__test()
        {
            var retValue = new AppOperate(new ConfigEntity(), new RequestAppDto()).VerfiyOfAppTemplate(472, 24,
                String.Empty);

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void template_VerfiyOfAppTemplate_media_Name_test()
        {
            var retValue = new AppOperate(new ConfigEntity(), new RequestAppDto()).VerfiyOfAppTemplate(0, 24, "天天向上");

            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void template_GetAreaGroupDtoList_test()
        {
            var retValue = new ReturnValue();
            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CreateUserId = 35;
            _requestMediaDto = GetRequestMediaDtoByInsert();

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            var list = provider.GetAreaGroupDtoList(125);

            Console.WriteLine(JsonConvert.SerializeObject(list));

            //所有的城市，因为交叉也不能重复
            //var allCitys = list.Where(s => s.GroupType == (int)SaleAreaGroupTypeEnum.Citys
            //    || s.IsPublic == 1).Select(s => new
            //    {
            //        DetailArea = s.DetailArea.Where(t => t.IsPublic == 1).ToList()
            //    }).ToList();

            var citysList = list.Where(s => s.GroupType == (int)SaleAreaGroupTypeEnum.Citys && s.IsPublic == 1);

            List<int> d = new List<int>();
            foreach (var item in citysList)
            {
                d.AddRange(item.DetailArea.Where(s => s.IsPublic == 1).Select(s => s.CityId));
            }

            Console.WriteLine("-------------------------------------------------");

            Console.WriteLine(JsonConvert.SerializeObject(d));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("模板：样式校验，套用模板，且修正套用模板的时候需要校验")]
        [TestMethod]
        public void template_VerifyOfAdTemplateStyle_test()
        {
            var retValue = new ReturnValue();
            _configEntity.RoleTypeEnum = RoleEnum.YunYingOperate;
            _configEntity.CreateUserId = 35;
            _requestMediaDto = GetRequestMediaDtoByInsert();

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            var list = provider.VerifyOfAdTemplateStyle(retValue);

            Console.WriteLine(JsonConvert.SerializeObject(list));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void template_VerifyOfAdTemplateGroupCitys_byjson_test()
        {
            var retValue = new ReturnValue();
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 35;
            _requestMediaDto = GetRequestMediaDtoByInsert();

            var json =
                "{\"WeiXin\":null,\"App\":null,\"Temp\":{\"TemplateId\":132,\"BaseAdId\":-2,\"BaseMediaId\":-2,\"MediaId\":55,\"AdTemplateName\":\"首页banner\",\"OriginalFile\":\"/UploadFiles/2017/6/28/16/1$e5f8fc33-35b5-443d-8d0a-5b8f8faeede2.png\",\"AdForm\":51001,\"AdFormName\":null,\"CarouselCount\":6,\"SellingPlatform\":63,\"SellingMode\":3,\"AdLegendUrl\":\"/UploadFiles/2017/6/28/17/查看$6d88f6e4-49a6-4334-a801-4ab081a17571.png,/UploadFiles/2017/6/28/17/查看$99e5cce5-3280-4db1-9a55-35462d3841f8.png,/UploadFiles/2017/6/28/17/查看$4ece4ac0-f9a8-4a53-80be-6ec2701a836c.png\",\"AdDisplay\":null,\"AdDescription\":null,\"Remarks\":null,\"AdDisplayLength\":0,\"CreateUserId\":0,\"CreateTime\":\"0001-01-01T00:00:00\",\"AuditStatus\":48001,\"IsModified\":false,\"AdTempStyle\":[{\"BaseMediaID\":24,\"AdTemplateID\":0,\"AdStyleId\":346,\"IsPublic\":0,\"AdStyle\":\"gif\",\"CreateUserId\":0}],\"AdSaleAreaGroup\":[{\"GroupId\":321,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"一级\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2306,\"CityName\":\"安康\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":1830,\"CityName\":\"阿拉善\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":1710,\"CityName\":\"鞍山\",\"CreateUserId\":0}]},{\"GroupId\":322,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"二级\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2707,\"CityName\":\"阿里\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":910,\"CityName\":\"保定\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":717,\"CityName\":\"毕节\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":605,\"CityName\":\"百色\",\"CreateUserId\":0}]},{\"GroupId\":397,\"IsPublic\":0,\"GroupType\":1,\"GroupName\":\"华东\",\"DetailArea\":[{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2535,\"CityName\":\"阿坝\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2516,\"CityName\":\"巴中\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2305,\"CityName\":\"宝鸡\",\"CreateUserId\":0},{\"ProvinceId\":-2,\"ProvinceName\":null,\"IsPublic\":0,\"CityId\":2113,\"CityName\":\"滨州\",\"CreateUserId\":0}]}]},\"BusinessType\":15000,\"OperateType\":2}";
            _requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            _requestMediaDto.Temp.BaseAdId = 132;

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            var list = provider.VerifyOfAdTemplateGroupCitys(retValue);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void template_VerifyOfAdTemplateGroupCitys_test()
        {
            var retValue = new ReturnValue();
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 1235;
            _requestMediaDto = GetRequestMediaDtoByInsert();

            _requestMediaDto.Temp.TemplateId = 4;
            _requestMediaDto.Temp.BaseAdId = 2;

            #region AdTempStyle

            _requestMediaDto.Temp.AdTempStyle = new List<AdTempStyleDto>()
            {
                new AdTempStyleDto()
                {
                    AdStyle = "大图",
                    AdStyleId = 16,
                    AdTemplateID = 4,
                    BaseMediaID = 2,
                    IsPublic = 0
                },
                new AdTempStyleDto()
                {
                    AdStyle = "动画",
                    AdStyleId = 17,
                    AdTemplateID = 4,
                    BaseMediaID = 2,
                    IsPublic = 0
                }
            };

            _requestMediaDto.Temp.AdSaleAreaGroup = new List<AdSaleAreaGroupDto>()
            {
                new AdSaleAreaGroupDto()
                {
                    DetailArea = new List<AdSaleAreaGroupDetailDto>()
                    {
                        new AdSaleAreaGroupDetailDto()
                        {
                            CityId = 1001,
                            CityName = "郑州市",
                            ProvinceId = 10,
                            IsPublic = 0,
                            ProvinceName = "河南省"
                        },
                        //new AdSaleAreaGroupDetailDto()
                        //{
                        //    CityId = 1002,
                        //    CityName = "洛阳市",
                        //    ProvinceId = 10,
                        //       IsPublic =  0,
                        //    ProvinceName = "河南省"
                        //},
                        //new AdSaleAreaGroupDetailDto()
                        //{
                        //    CityId = 1003,
                        //    CityName = "周口",
                        //    ProvinceId = 10,
                        //       IsPublic =  0,
                        //    ProvinceName = "河南省"
                        //}
                    },
                    GroupId = 4,
                    GroupName = "阿勒泰/阿克",
                    GroupType = 1,
                    IsPublic = 0
                },
                new AdSaleAreaGroupDto()
                {
                    GroupId = -2,
                    GroupName = "保定组",
                    GroupType = 1,
                    IsPublic = 0,
                    DetailArea = new List<AdSaleAreaGroupDetailDto>()
                    {
                        new AdSaleAreaGroupDetailDto()
                        {
                            CityId = 910,
                            CityName = "保定",
                            ProvinceId = 9,
                            IsPublic = 0,
                            ProvinceName = "河北省"
                        }
                    }
                }
            };

            #endregion AdTempStyle

            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            var list = provider.VerifyOfAdTemplateGroupCitys(retValue);

            Console.WriteLine(JsonConvert.SerializeObject(list));

            if (!list.HasError)
            {
                var retValue1 = new ReturnValue();
                provider.TaskToRunAdTemplateRelation(retValue1, _requestMediaDto.Temp.TemplateId,
                    _requestMediaDto.Temp.BaseMediaId);
                Console.WriteLine(JsonConvert.SerializeObject(retValue1));
            }

            Assert.IsFalse(retValue.HasError);
        }

        [Description("获取城市，缓存测试")]
        [TestMethod]
        public void GetAreaList_test()
        {
            var list = AdTemplateRelationDataProvider.GetAreaList();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetTipsCitys_test()
        {
            var retValue = new ReturnValue();
            _configEntity.RoleTypeEnum = RoleEnum.MediaOwner;
            _configEntity.CreateUserId = 1235;
            _requestMediaDto = GetRequestMediaDtoByInsert();
            var provider = new AdTemplateProvider(_configEntity, _requestMediaDto.Temp);
            var str = provider.GetTipsCitys(new List<int>()
            {
                1001,
                1002,
                1003
            });
            Console.WriteLine(string.Join(",", str));
        }

        [Description("模板：获取详情-V1版本，先获取全部的样式和城市组城市，然后在过滤")]
        [TestMethod]
        public void template_getinfo_v1_test()
        {
            var info = BLL.AdTemplate.AppAdTemplate.Instance.GetInfoV1(4);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [Description("模板插入审核信息")]
        [TestMethod]
        public void template_OperateAuditMsgInsert_test()
        {
            BLL.AdTemplate.AppAdTemplate.Instance.OperateAuditMsgInsert(3888, 46005, (int)AppTemplateEnum.已通过, 1125);
        }

        [TestMethod]
        public void getStrContentLength_test()
        {
            var count = CurrentOperateBase.GetLength("这测试长度abdc");

            Console.WriteLine(count);
        }
    }
}