using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1
{
    [TestClass]
    public class WeiXinOperateTest
    {
        public WeiXinOperateTest()
        {
            MediaMapperConfig.Configure();
        }

        [TestMethod]
        public void MediaCommonlyClassInsertByBulk_Test()
        {
            var _requestDto = new RequestWeiXinDto()
            {
                CommonlyClass = "47002-0,47003-0,47004-0,47005-2,47006-4",
                MediaID = 1,
            };
            var categoryIds = new Dictionary<int, int>();
            var spLit = _requestDto.CommonlyClass.Split(',');
            foreach (var item in spLit)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                var spItem = item.Split('-');
                var spItemKey = spItem[0].ToInt();
                if (spItemKey > 0)
                    categoryIds.Add(spItemKey, spItem[1].ToInt());
            }

            var weightId = categoryIds.Where(s => s.Value == 1).Select(s => s.Key).FirstOrDefault();

            Console.WriteLine(categoryIds.All(s => s.Value != 0));

            //Console.WriteLine(weightId.Count());

            //var retValue = new WeiXinOperate(_requestDto, new ConfigEntity()).MediaCommonlyClassInsertByBulk(
            //    categoryIds, new ReturnValue());

            //Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void WeiXinOperate_insert_Test()
        {
            var requestDto = new RequestWeiXinDto()
            {
                MediaID = 1,
                AuthType = 1, //授权方式
                //OperateType = (int)OperateType.Insert,
                Number = "test_1111",
                Name = "test_name_111",
                //BusinessType = 14001,
                HeadIconURL = "/upload/ssss.jpg",
                FansCount = 599,
                FansCountUrl = "/upload/FansCountUrl/sss.jpg",
                CommonlyClass = "20001,20002,20003,20004,20005",
                FansArea = "201-67.09,202-77,405-34,102-99.78",
                FansAreaShotUrl = "/upload/FansAreaShotUrl/sss.jpg",
                FansSexScaleUrl = "/upload/FansSexScaleUrl/sa.jpg",
                FansMalePer = 45.8m,
                FansFemalePer = 444.2m,
                ProvinceID = 201,
                CityID = 200
            };

            var json = "{\"MediaID\":73,\"AuthType\":38003,\"Number\":\"chitunews\",\"Name\":\"赤兔快讯\",\"HeadIconURL\":\"http://spiderapi.xyauto.com/Pic/HeadImg/2017-05-22/9a1bf438-b459-4b79-b8d8-1e94532afbe0.jpg\",\"FansCount\":2345,\"FansCountUrl\":null,\"FansMalePer\":23.0,\"FansFemalePer\":23.0,\"CommonlyClass\":\"47004-1,47005-0,47006-0,47012-0\",\"FansArea\":\"2-23,4-23,5-23\",\"FansAreaShotUrl\":\"/UploadFiles/2017/5/24/11/timg2$6a921ae5-663d-45e9-ae4a-1c71c5494a36.jpg\",\"PublishStatus\":0,\"AuditStatus\":0,\"FansSexScaleUrl\":\"/UploadFiles/2017/5/24/11/timg14$b2894a67-4b6f-4760-baf8-f3486ade3446.jpg\",\"LevelType\":4001,\"IsAuth\":true,\"Sign\":\"大哥打个飞的个人风格如果如果反对反对法但是大概多少地方官梵蒂冈的是的高浮雕高浮雕广泛大使馆反对\",\"TwoCodeURL\":\"http://spiderapi.xyauto.com/Pic/QrCode/2017-05-22/ee495fd1-11d6-4c0d-b03c-5cd052be7e7b.jpg\",\"ProvinceID\":2,\"Source\":0,\"CityID\":201,\"CreateTime\":\"2017-05-24T11:20:53.3322043+08:00\",\"CreateUserID\":0,\"LastUpdateTime\":\"2017-05-24T11:20:53.3322043+08:00\",\"LastUpdateUserID\":0,\"OrderRemark\":\"5001,5002,5003,5004\",\"EnterpriseName\":null,\"QualificationOne\":null,\"QualificationTwo\":null,\"BusinessLicense\":null}";
            json = "{\"MediaID\":73,\"AuthType\":38002,\"Number\":\"chitunews_test\",\"Name\":\"赤兔快讯_test\",\"HeadIconURL\":\"http://spiderapi.xyauto.com/Pic/HeadImg/2017-05-22/9a1bf438-b459-4b79-b8d8-1e94532afbe0.jpg\",\"FansCount\":456789,\"FansCountUrl\":\"/UploadFiles/2017/5/25/13/timg22$74e00a8c-0981-4054-8e4b-d0daa35f998c.jpg\",\"FansMalePer\":34.0,\"FansFemalePer\":56.0,\"CommonlyClass\":\"47004-1,47005-0,47006-0,47012-0\",\"FansArea\":\"2-12,31-34,9-45\",\"FansAreaShotUrl\":\"/UploadFiles/2017/5/25/13/timg6$ef345c54-4365-46bd-8a79-9fb3c08250c8.jpg\",\"PublishStatus\":0,\"AuditStatus\":0,\"FansSexScaleUrl\":\"/UploadFiles/2017/5/25/13/timg16$3119c2ad-c455-4a0d-875a-23cd3328fd7c.jpg\",\"LevelType\":0,\"IsAuth\":false,\"Sign\":null,\"TwoCodeURL\":null,\"ProvinceID\":2,\"Source\":0,\"CityID\":201,\"CreateTime\":\"2017-05-25T13:38:56.3769071+08:00\",\"CreateUserID\":0,\"LastUpdateTime\":\"2017-05-25T13:38:56.3769071+08:00\",\"LastUpdateUserID\":0,\"OrderRemark\":\"40001,40002,40003,40004\",\"EnterpriseName\":null,\"QualificationOne\":null,\"QualificationTwo\":null,\"BusinessLicense\":null}";

            requestDto = JsonConvert.DeserializeObject<RequestWeiXinDto>(json);
            requestDto.CreateUserID = 1187;
            var retValue = new WeiXinOperate(requestDto, new ConfigEntity()
            {
                BusinessType = MediaType.WeiXin,
                CureOperateType = OperateType.Insert,
                CreateUserId = 1187,
                RoleTypeEnum = RoleEnum.AE,
                UserType = UserTypeEnum.企业
            }).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
            //Assert.Fail(retValue.Message, retValue.HasError, retValue.ErrorCode);
        }

        [TestMethod]
        public void WeiXinOperate_admin_insert_Test()
        {
            var json = "{\"MediaID\":73,\"AuthType\":38002,\"Number\":\"chitunews_test\",\"Name\":\"赤兔快讯_test\",\"HeadIconURL\":\"http://spiderapi.xyauto.com/Pic/HeadImg/2017-05-22/9a1bf438-b459-4b79-b8d8-1e94532afbe0.jpg\",\"FansCount\":456789,\"FansCountUrl\":\"/UploadFiles/2017/5/25/13/timg22$74e00a8c-0981-4054-8e4b-d0daa35f998c.jpg\",\"FansMalePer\":34.0,\"FansFemalePer\":56.0,\"CommonlyClass\":\"47004-1,47005-0,47006-0,47012-0\",\"FansArea\":\"2-12,31-34,9-45\",\"FansAreaShotUrl\":\"/UploadFiles/2017/5/25/13/timg6$ef345c54-4365-46bd-8a79-9fb3c08250c8.jpg\",\"PublishStatus\":0,\"AuditStatus\":0,\"FansSexScaleUrl\":\"/UploadFiles/2017/5/25/13/timg16$3119c2ad-c455-4a0d-875a-23cd3328fd7c.jpg\",\"LevelType\":0,\"IsAuth\":false,\"Sign\":null,\"TwoCodeURL\":null,\"ProvinceID\":2,\"Source\":0,\"CityID\":201,\"CreateTime\":\"2017-05-25T13:38:56.3769071+08:00\",\"CreateUserID\":0,\"LastUpdateTime\":\"2017-05-25T13:38:56.3769071+08:00\",\"LastUpdateUserID\":0,\"OrderRemark\":\"40001,40002,40003,40004\",\"EnterpriseName\":null,\"QualificationOne\":null,\"QualificationTwo\":null,\"BusinessLicense\":null}";
            var requestDto = JsonConvert.DeserializeObject<RequestWeiXinDto>(json);
            requestDto.CreateUserID = 1187;
            var retValue = new WeiXinOperate(requestDto, new ConfigEntity()
            {
                BusinessType = MediaType.WeiXin,
                CureOperateType = OperateType.Insert,
                CreateUserId = 1187,
                RoleTypeEnum = RoleEnum.YunYingOperate,
                UserType = UserTypeEnum.企业
            }).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void WeiXinOperate_admin_update_Test()
        {
            var json = "{\"MediaID\":75,\"AuthType\":38002,\"Number\":\"chitunews_test1\",\"Name\":\"赤兔快讯_test\",\"HeadIconURL\":\"http://spiderapi.xyauto.com/Pic/HeadImg/2017-05-22/9a1bf438-b459-4b79-b8d8-1e94532afbe0.jpg\",\"FansCount\":456789,\"FansCountUrl\":\"/UploadFiles/2017/5/25/13/timg22$74e00a8c-0981-4054-8e4b-d0daa35f998c.jpg\",\"FansMalePer\":34.0,\"FansFemalePer\":56.0,\"CommonlyClass\":\"47004-1,47005-0,47006-0,47012-0\",\"FansArea\":\"2-12,31-34,9-45\",\"FansAreaShotUrl\":\"/UploadFiles/2017/5/25/13/timg6$ef345c54-4365-46bd-8a79-9fb3c08250c8.jpg\",\"PublishStatus\":0,\"AuditStatus\":0,\"FansSexScaleUrl\":\"/UploadFiles/2017/5/25/13/timg16$3119c2ad-c455-4a0d-875a-23cd3328fd7c.jpg\",\"LevelType\":0,\"IsAuth\":false,\"Sign\":null,\"TwoCodeURL\":null,\"ProvinceID\":2,\"Source\":0,\"CityID\":201,\"CreateTime\":\"2017-05-25T13:38:56.3769071+08:00\",\"CreateUserID\":0,\"LastUpdateTime\":\"2017-05-25T13:38:56.3769071+08:00\",\"LastUpdateUserID\":0,\"OrderRemark\":\"40001,40002,40003,40004\",\"EnterpriseName\":null,\"QualificationOne\":null,\"QualificationTwo\":null,\"BusinessLicense\":null}";
            var requestDto = JsonConvert.DeserializeObject<RequestWeiXinDto>(json);
            requestDto.CreateUserID = 1187;
            var retValue = new WeiXinOperate(requestDto, new ConfigEntity()
            {
                BusinessType = MediaType.WeiXin,
                CureOperateType = OperateType.Edit,
                CreateUserId = 1187,
                RoleTypeEnum = RoleEnum.YunYingOperate,
                UserType = UserTypeEnum.企业
            }).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void wx_insert_byjson_mediaRole_test()
        {
            var json =
                "{\"WeiXin\":{\"MediaID\":8160,\"AuthType\":38004,\"Number\":\"sdsbqmt\",\"Name\":\"山东商报\",\"HeadIconURL\":\"http://img.weiboyi.com/vol1/7/102/102/k/v/918n3p7r090n11r79p9p40qn8613p9o9/sdsbqmt_avatar_1489532664.jpg\",\"FansCount\":30000,\"FansCountUrl\":null,\"FansMalePer\":0,\"FansFemalePer\":0,\"CommonlyClass\":\"47029-1\",\"FansArea\":null,\"FansAreaShotUrl\":null,\"PublishStatus\":0,\"AuditStatus\":0,\"FansSexScaleUrl\":null,\"LevelType\":0,\"IsAuth\":false,\"Sign\":\"做有高度、有深度、有速度、有宽度、有温度的新闻,佐以商妹子撒娇卖萌的乐子,成你最贴心的手机媒体!\",\"TwoCodeURL\":null,\"ProvinceID\":-2,\"Source\":0,\"CityID\":-2,\"CreateTime\":\"2017-07-05T16:56:15.5762079+08:00\",\"CreateUserID\":0,\"LastUpdateTime\":\"2017-07-05T16:56:15.5762079+08:00\",\"LastUpdateUserID\":0,\"OrderRemark\":[{\"Id\":40001,\"Name\":null,\"Descript\":null},{\"Id\":40003,\"Name\":null,\"Descript\":null},{\"Id\":40004,\"Name\":null,\"Descript\":null},{\"Id\":40005,\"Name\":null,\"Descript\":null}],\"EnterpriseName\":\"we're\",\"QualificationOne\":\"/UploadFiles/2017/7/5/10/WechatIMG997$ff9033a8-a01e-41cc-b4f2-7b67eedadbff.jpeg\",\"QualificationTwo\":null,\"BusinessLicense\":\"/UploadFiles/2017/7/5/10/WechatIMG1002$7646df4f-70d4-4d46-98eb-63693efd5d6e.jpeg\"},\"App\":null,\"Temp\":null,\"BusinessType\":14001,\"OperateType\":1}";

            var requestMediaDto = JsonConvert.DeserializeObject<RequestMediaDto>(json);

            var retValue = new WeiXinOperate(requestMediaDto.WeiXin, new ConfigEntity()
            {
                BusinessType = MediaType.WeiXin,
                CureOperateType = OperateType.Insert,
                CreateUserId = 1187,
                RoleTypeEnum = RoleEnum.MediaOwner,
                //UserType = UserTypeEnum.企业
            }).Excute();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void WeiXinOperate_update_Test()
        {
            var requestDto = new RequestWeiXinDto()
            {
                MediaID = 16629,
                AuthType = 1, //授权方式
                //OperateType = (int)OperateType.Edit,
                Number = "test_1111",
                Name = "test_name_1222",
                //BusinessType = 14001,
                HeadIconURL = "/upload/sss1s.jpg",
                FansCount = 599,
                FansCountUrl = "/upload/FansCountUrl/s1ss.jpg",
                CommonlyClass = "20001,20002,20003,20004,20005",
                FansArea = "201-67.09,202-77,405-34,102-99.78",
                FansAreaShotUrl = "/upload/FansAreaShotUrl/sss.jpg",
                FansSexScaleUrl = "/upload/FansSexScaleUrl/sa.jpg",
                FansMalePer = 45.8m,
                FansFemalePer = 44.2m,
                ProvinceID = 201,
                CityID = 200
            };
            var retValue = new WeiXinOperate(requestDto, new ConfigEntity()
            {
                CureOperateType = OperateType.Edit,
                BusinessType = MediaType.WeiXin,
                RoleTypeEnum = RoleEnum.AE
            }).Update();

            Console.WriteLine(JsonConvert.SerializeObject(retValue));

            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void GetQueryInfo_Test()
        {
            // OperateType 1：添加（是从授权之后调整到页面、查询是基础表信息）
            // 2：编辑（从后台媒体列表跳转到页面、查询是副表信息,媒体已通过的是主表，待审核或驳回是副表）

            MediaMapperConfig.Configure(); //配置autpMapper

            var proxy = new MediaOperateProxy(new RequestGetMeidaInfoDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                OperateType = (int)OperateType.Insert,
                RecId = 73,// 16629,
                IsAuditPass = true
            }, new ConfigEntity());

            var info = proxy.QueryInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetQueryInfo_YunYingOperate_Test()
        {
            // OperateType 1：添加（是从授权之后调整到页面、查询是基础表信息）
            // 2：编辑（从后台媒体列表跳转到页面、查询是副表信息,媒体已通过的是主表，待审核或驳回是副表）

            MediaMapperConfig.Configure(); //配置autpMapper

            var proxy = new MediaOperateProxy(new RequestGetMeidaInfoDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                OperateType = (int)OperateType.Edit,
                RecId = 73,// 16629,
                IsAuditPass = true
            }, new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.YunYingOperate
            });

            var info = proxy.QueryInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetQueryInfo_mediaRole_AuditPass_Test()
        {
            // OperateType 1：添加（是从授权之后调整到页面、查询是基础表信息）
            // 2：编辑（从后台媒体列表跳转到页面、查询是副表信息,媒体已通过的是主表，待审核或驳回是副表）

            MediaMapperConfig.Configure(); //配置autpMapper

            var proxy = new MediaOperateProxy(new RequestGetMeidaInfoDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                OperateType = (int)OperateType.Insert,
                RecId = 100,// 16629,
                IsAuditPass = false
            }, new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.MediaOwner
            });

            var info = proxy.QueryInfo();

            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetItem_Test()
        {
            var requestDto = new RequestGetCommonlyCalssDto()
            {
                MediaId = 4,
                BusinessType = (int)MediaType.WeiXin
            };
            var info = new MediaOperateProxy(requestDto, new ConfigEntity()).GetItem();
            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetItemForBack_Test()
        {
            MediaMapperConfig.Configure(); //配置autpMapper
            var info = new WeiXinOperate(new RequestWeiXinDto(), new ConfigEntity()
            {
                BusinessType = MediaType.WeiXin,
                RoleTypeEnum = RoleEnum.YunYingOperate
            }).GetItemForBack(new RequestGetCommonlyCalssDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                MediaId = 74,
                Wx_Status = -2
            });
            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetItemForBack__yunying_pass_Test()
        {
            MediaMapperConfig.Configure(); //配置autpMapper
            var info = new WeiXinOperate(new RequestWeiXinDto(), new ConfigEntity()
            {
                BusinessType = MediaType.WeiXin,
                RoleTypeEnum = RoleEnum.YunYingOperate
            }).GetItemForBack(new RequestGetCommonlyCalssDto()
            {
                BusinessType = (int)MediaType.WeiXin,
                MediaId = 26,
                Wx_Status = (int)MediaAuditStatusEnum.Initialization
            });
            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetAuditDetailInfo_Test()
        {
            MediaMapperConfig.Configure(); //配置autpMapper
            var info = new WeiXinOperate(new RequestWeiXinDto(), new ConfigEntity()
            {
            }).GetAuditDetailInfo(16689);
            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void GetItemForMediaRole_Test()
        {
            MediaMapperConfig.Configure(); //配置autpMapper
            var info = BLL.Media.MediaWeixin.Instance.GetItemForMediaRole(16629);
            Console.WriteLine(JsonConvert.SerializeObject(info));

            Assert.IsFalse(info == null);
        }

        [TestMethod]
        public void RequestMediaDto_VerifyParams_Test()
        {
            var requestDto = new RequestMediaDto()
            {
                BusinessType = (int)MediaType.WeiXin
            };

            requestDto = null;
            var retValue = new VerifyOfNecessaryParameters<RequestMediaDto>().VerifyNecessaryParameters(requestDto);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void GetRecommendClass_Test()
        {
            var requestDto = new RequestGetCommonlyCalssDto
            {
                BusinessType = 14001,
                MediaId = 41,
                PageSize = 10
            };
            var info = new WeiXinOperate(new RequestWeiXinDto(), new ConfigEntity())
                 .GetRecommendClass(new PublishSearchAutoQuery<GetRecommendClassListDto>()
                 {
                     PageSize = requestDto.PageSize,
                     MediaId = requestDto.MediaId
                 });
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void GetWeixinInfoByWxNumber_Test()
        {
            //var info = BLL.WeixinOAuth.Instance.GetWeixinInfoByWxNumber(" chitunews");
            //Console.WriteLine(JsonConvert.SerializeObject(info));

            var count = BLL.Media.MediaWeixin.Instance.VerifyMediaCountByRole("chitunews", RoleInfoMapping.AE);
            Console.WriteLine(count);
        }

        [TestMethod]
        public void MediaQualification_Test()
        {
            var insertOp = BLL.Media.MediaQualification.Instance.Insert(new MediaQualification()
            {
                BusinessLicense = "/dddd/d",
                CreateUserID = 1235,
                EnterpriseName = "test",
                MediaID = 16622,
                QualificationOne = "/test",
                QualificationTwo = "/test2"
            });
            Console.WriteLine(insertOp);
        }

        [TestMethod]
        public void InsertAuthFansWeixinByFansSex_Test()
        {
            BLL.Media.MediaWeixin.Instance.InsertAuthFansWeixinByFansSex(new MediaFansArea()
            {
                WxID = 73,
                MediaID = 73
            }, 12.00m, 15.00m);
        }

        [TestMethod]
        public void MediaQualification_get_Test()
        {
            var info = BLL.Media.MediaQualification.Instance.GetEntity(16622);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void GetFansAreaAndFansProportionForBaseWeiXin_test()
        {
            var tunpList = new WeiXinOperate(new RequestGetMeidaInfoDto(), new ConfigEntity())
                  .GetFansAreaAndFansProportionForBaseWeiXin(26);
            Console.WriteLine(JsonConvert.SerializeObject(tunpList));
        }

        [TestMethod]
        public void get_GetInfo_test()
        {
            var info = new WeiXinOperate(new RequestGetMeidaInfoDto()
            {
                BaseMediaId = 72
            }, new ConfigEntity()).GetInfo();
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void GetItemInfo_test()
        {
            var info = new WeiXinOperate(new RequestGetMeidaInfoDto()
            {
                BaseMediaId = 72
            }, new ConfigEntity()).GetItemInfo(16670, 1192);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void Image_test()
        {
            var url = "http://spiderapi.chitunion.com/Pic/HeadImg/2017-05-27/61b612d5-c679-49b2-af28-718d0fce5b55.jpg"
                 .ToAbsolutePath(true);
            Console.WriteLine(url);
        }

        [TestMethod]
        public void MediaAreaMappingInsertByBulk_test()
        {
            var retValue = new WeiXinOperate(new RequestWeiXinDto()
            {
                AreaMedia = new List<CoverageAreaDto>()
                 {
                     new CoverageAreaDto()
                     {
                         ProvinceName = "吉林",
                         ProvinceId = 14,
                         CityId = 1401,
                         CityName = "长春"
                     }
                 }
            }, new ConfigEntity()
            {
                CreateUserId = 1121
            })
                .MediaAreaMappingInsertByBulk(new ReturnValue(), 19883, MediaRelationType.Attached);
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void UpdateSqlTest_test()
        {
            var execCount = BLL.Media.MediaWeixin.Instance.UpdateSqlTest();
            Console.WriteLine(execCount);
        }

        [TestMethod]
        public void TestSum()
        {
            var startTime = DateTime.Now;

            long endCount = 1000000000;

            var part1 = endCount / 2;

            Console.WriteLine("part1:" + part1);

            long i = 0;
            long j = part1;
            var sum11 = Task.Factory.StartNew(() =>
            {
                long sum = 0;
                for (; i < part1; i++)
                {
                    sum += i;
                }
                return sum;
            });
            //Console.WriteLine("sum11:" + sum);
            var count1 = sum11.Result;
            Console.WriteLine("sum11:" + count1);
            var sum22 = Task.Factory.StartNew(() =>
             {
                 long sum2 = 0;
                 for (; j < endCount; j++)
                 {
                     sum2 += j;
                 }
                 return sum2;
             });
            var count2 = sum22.Result;
            Console.WriteLine("sum22:" + count2);
            //Console.WriteLine(count1 + count2);
            Console.WriteLine((DateTime.Now - startTime).TotalSeconds);
        }
    }
}