/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 14:48:58
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.RequestDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1
{
    [TestClass]
    public class AdQueryProxyTest
    {
        [TestMethod]
        public void AdWeiXinAuditPassQuery_YunYing_Test()
        {
            var requestDto = new RequestAdQueryDto()
            {
                CreateUserId = 1192,
                BusinessType = (int)MediaType.WeiXin,
                Wx_Status = ((int)PublishBasicStatusEnum.待审核).ToString()
            };
            var proxy = new AdQueryProxy(new ConfigEntity()
            {
                //BusinessType = MediaType.WeiXin,
                // CreateUserId = 1192,
                RoleTypeEnum = RoleEnum.YunYingOperate
            }, requestDto);

            var info = proxy.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void AdWeiXinQuery_YunYing_Test()
        {
            var requestDto = new RequestAdQueryDto()
            {
                CreateUserId = 1125,
                BusinessType = (int)MediaType.WeiXin,
                Wx_Status = ((int)PublishBasicStatusEnum.上架).ToString(),
                IsAuditView = false
            };
            var proxy = new AdQueryProxy(new ConfigEntity()
            {
                //BusinessType = MediaType.WeiXin,
                // CreateUserId = 1192,
                RoleTypeEnum = RoleEnum.YunYingOperate
            }, requestDto);

            var info = proxy.QueryAdWeiXinByYunYing();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void AdQueryProxy_AE_AppendAudit_Test()
        {
            var requestDto = new RequestAdQueryDto()
            {
                CreateUserId = 1192,
                BusinessType = (int)MediaType.WeiXin,
                Wx_Status = ((int)PublishBasicStatusEnum.待审核).ToString()
            };
            var proxy = new AdQueryProxy(new ConfigEntity()
            {
                //BusinessType = MediaType.WeiXin,
                // CreateUserId = 1192,
                RoleTypeEnum = RoleEnum.MediaOwner
            }, requestDto);

            var info = proxy.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void GetPublishStatisticsCount_V1_1_1_Test()
        {
            var info = new AdQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.YunYingOperate
            }, null).GetStatisticsCount(new PublishSearchAutoQuery<PublishStatisticsCount>()
            {
                BusinessType = (int)MediaType.WeiXin,
                CreateUserId = 1234
            });
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void AdQueryEndTime_Test()
        {
            var endTime = "2017-04-28";

            var date = DateTime.Parse(endTime);
            Console.WriteLine(date);

            //IFormatProvider culture = new CultureInfo("yyyy-MM-dd", true);

            var date1 = DateTime.Parse(date.ToShortDateString()).AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine(date1);
        }
    }
}