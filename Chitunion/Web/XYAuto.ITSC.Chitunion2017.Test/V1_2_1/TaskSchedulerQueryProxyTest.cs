/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 14:06:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Materiel;
using XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_11;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_2_1;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_2_1
{
    [TestClass]
    public class TaskSchedulerQueryProxyTest
    {
        private MaterielController ctl = new MaterielController();

        public TaskSchedulerQueryProxyTest()
        {
        }

        [TestMethod]
        public void MaterielTaskByAdminQuery_Test()
        {
            var proxy = new TaskSchedulerQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.GroupLeader
            }, new RequestTaskSchedulerDto()
            {
                //StartDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd")
            });

            var list = proxy.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void MaterielTaskQuery_Test()
        {
            var proxy = new TaskSchedulerQueryProxy(new ConfigEntity()
            {
                RoleTypeEnum = RoleEnum.ArticleCleaner,
                CreateUserId = 1209
            }, new RequestTaskSchedulerDto()
            {
                //StartDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd")
                UserId = 1209
            });

            var list = proxy.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetCleanInfo_Test()
        {
            var info = new MaterielTaskSchedulerProvider(new ConfigEntity(), new RequestDistributeDto()).GetCleanInfo(10127, true);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void DoDistribute_Test()
        {
            var provider = new MaterielTaskSchedulerProvider(new ConfigEntity()
            {
                CreateUserId = 1200
            }, new RequestDistributeDto
            {
                GroupIds = "22973,22972,22974",
                OperateType = 1,
                UserId = 1330
            });
            var retValue = provider.DistributeAndDoRecovery();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void DoSubmitInfo_Test()
        {
            var request = new RequestSubmitCleanDto()
            {
            };

            var json = "{\"Head\":{\"ArticleId\":27551,\"Title\":\"27551---行游瓯海，和青山绿水缠绵\",\"Content\":\"\",\"Abstract\":\"瓯海虽然在温州，但其实并不靠海，它有的，是幽深的林谷，清澈的山涧和溪流，白练般从半空里垂下的瀑布，漫山遍野的猗猗绿竹，还有传承了千年的手工造...\"},\"Body\":[{\"ArticleId\":7118,\"Title\":\"7118---明年初上市 兰博基尼Urus 12月4日发布\",\"Content\":\"<p>　　 日\",\"Abstract\":\"44482摘要1111\"},{\"ArticleId\":7119,\"Title\":\"7119--最大功率660马力 兰博基尼Urus牛劲十足\",\"Content\":\"</p>\",\"Abstract\":\"44482摘要2222\"},{\"ArticleId\":7120,\"Title\":\"7120-年底亮相 兰博基尼Urus量产版最新消息\",\"Content\":\"<p>　　 。（文/ 吴昱晨）</p>\",\"Abstract\":\"44482摘要333333\"},{\"ArticleId\":7121,\"Title\":\"7121---兰博基尼Urus将于12月发布 竞争卡宴Turbo\",\"Content\":\"<计年销量在3000辆。</p>\",\"Abstract\":\"44482摘要444444\"},{\"ArticleId\":7122,\"Title\":\"7122-有别于概念车 曝兰博基尼Urus路试谍照\",\"Content\":\"<p>　　</p>\",\"Abstract\":\"44482摘要5555555\"}],\"DeleteList\":[7118,7119,7120],\"GroupId\":10118}";

            request = JsonConvert.DeserializeObject<RequestSubmitCleanDto>(json);

            var provider = new MaterielTaskSchedulerProvider(new ConfigEntity()
            {
                CreateUserId = 1295,
                RoleTypeEnum = RoleEnum.ArticleCleaner
            }, request);
            var retValue = provider.SubmitCleanInfo();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void GetArticleList_Test()
        {
            RequestArticleQueryDto reqDto = new RequestArticleQueryDto()
            {
                ArticleIds = "1,2,3",
                //Resource=1,
                //CarSerialId=1,
                CopyrightState = 1,
                OrderBy = 1002,
                StartDate = new DateTime(2017, 7, 1),
                EndDate = DateTime.Now,
                PageIndex = 1,
                PageSize = 20
            };

            var ret = ctl.GetArticleList(reqDto);
            Console.WriteLine(JsonConvert.SerializeObject(ret.Result));
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void GetArticleInfo_Test()
        {
            var ret = ctl.GetArticleInfo(698);
            Console.WriteLine(JsonConvert.SerializeObject(ret.Result));
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void GetDivideUser_Test()
        {
            string str = "1";
            string[] strarray = str.Split(new string[] { "|" }, StringSplitOptions.None);
            var ret = ctl.GetDivideUser();
            Console.WriteLine(JsonConvert.SerializeObject(ret.Result));
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void JsonTest()
        {
            var dto = new Entities.LETask.LeTaskInfo();
            Console.Write(JsonConvert.SerializeObject(dto));
        }
    }
}