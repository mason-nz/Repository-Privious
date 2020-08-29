using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1
{
    [TestClass]
    public class PbQueryProxyTest
    {
        [TestMethod]
        public void PbWeiXinAuditPassQuery_Test()
        {
            var wxStatus = ((int)PublishBasicStatusEnum.已通过) + "," + ((int)PublishBasicStatusEnum.停用 + "," + (int)PublishBasicStatusEnum.启用);
            var requestDto = new RequestPublishQueryDto
            {
                BusinessType = 14001,
                Wx_Status = wxStatus,
                IsPassed = true,
                SearchId = 1,
                //PubName = "李增",
                //EndTime = 7
            };
            var list = new PbQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.AE
            }, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void PbWeiXinAuditPassQuery_MediaOwn_Test()
        {
            var wxStatus = ((int)PublishBasicStatusEnum.已通过) + "," + ((int)PublishBasicStatusEnum.停用 + "," + (int)PublishBasicStatusEnum.启用);

            wxStatus = ((int)PublishBasicStatusEnum.已通过).ToString();
            var requestDto = new RequestPublishQueryDto
            {
                BusinessType = 14001,
                Wx_Status = wxStatus,
                IsPassed = true,
                CreateUserId = 1234
            };
            var list = new PbQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.MediaOwner
            }, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetPublishStatisticsCount_Test()
        {
            var info = new PbQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.YunYingOperate
            }, null).GetPublishStatisticsCount(new PublishSearchAutoQuery<PublishStatisticsCount>()
            {
                BusinessType = (int)MediaType.WeiXin,
                CreateUserId = 1234
            });
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void GetAuditInfo_Test()
        {
            var requestDto = new RequestAuditInfoQueryDto()
            {
                //MediaId = 1
                TemplateId = 27,
                BusinessType = 14002,
            };
            var info =
              BLL.Publish.PublishInfoQuery.Instance.GetPublishAuditInfoList(
                  new PublishAuditInfoQuery<RespPublishAuditInfoDto>()
                  {
                      MediaId = requestDto.MediaId,
                      TemplateId = requestDto.TemplateId,
                      PubId = requestDto.PubId,
                      PageSize = requestDto.TopSize
                  });
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void SearchAutoComplete_Test()
        {
            var requestDto = new RequestSearchTitleDto
            {
                Keyword = "t"
            };

            var roleInfo = RoleEnum.SupperAdmin;// RoleInfoMapping.GetUserRole(requestDto.CreateUserId);

            var info = new WeiXinOperate(new RequestGetMeidaInfoDto(), new ConfigEntity())
                 .GetSearchTitle(new PublishSearchAutoQuery<SearchTitleResponse>()
                 {
                     PageSize = requestDto.PageSize,
                     KeyWord = requestDto.Keyword,
                     CreateUserId = requestDto.CreateUserId
                 }, roleInfo);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void PbWeiXinQueryByYunYing_Test()
        {
            var wxStatus = ((int)PublishBasicStatusEnum.已通过).ToString();
            var requestDto = new RequestPublishQueryDto
            {
                BusinessType = 14001,
                Wx_Status = wxStatus,
                IsPassed = true,
                //SearchId = 1,
                //PubName = "李增",
                //EndTime = 7
            };
            var list = new PbQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.YunYingOperate
            }, requestDto).GetQuery();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}