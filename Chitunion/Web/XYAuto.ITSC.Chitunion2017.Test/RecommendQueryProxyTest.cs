using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class RecommendQueryProxyTest
    {
        [TestMethod]
        public void RecommendWeiXinQuery_Test()
        {
            var requestDto = new RecommendSearchDto
            {
                BusinessType = (int)MediaType.APP,
                //CategoryId = 1
            };
            var list = new RecommendQueryProxy(requestDto).GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void Add_Test()
        {
            var retValue = new RecommendQueryProxy(new AddRecommendDto()
            {
                MediaId = 11623,
                //ADDetailID = 117483,
                BusinessType = (int)MediaType.WeiXin,
                CategoryId = 20024
                //ImageUrl = "http://mvavatar4.meitudata.com/56ebb9ac5398c1431.jpg"
            }, null).AddToRecommend();

            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("推荐添加：微信、验证媒体信息是否存在")]
        [TestMethod]
        public void add_verify_false_media_info_test()
        {
            var retValue = new RecommendQueryProxy(new AddRecommendDto()
            {
                MediaId = 11623,
                //ADDetailID = 117483,
                BusinessType = (int)MediaType.WeiXin,
                CategoryId = 20024
                //ImageUrl = "http://mvavatar4.meitudata.com/56ebb9ac5398c1431.jpg"
            }, null).AddToRecommend();

            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [Description("推荐添加：微信、验证推荐分类是否存在")]
        [TestMethod]
        public void add_verify_false_category_info_test()
        {
            var retValue = new RecommendQueryProxy(new AddRecommendDto()
            {
                MediaId = 2,
                //ADDetailID = 117483,
                BusinessType = (int)MediaType.WeiXin,
                CategoryId = 20024
                //ImageUrl = "http://mvavatar4.meitudata.com/56ebb9ac5398c1431.jpg"
            }, null).AddToRecommend();

            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [Description("推荐添加：微信")]
        [TestMethod]
        public void add_verify_weixin_test()
        {
            var retValue = new RecommendQueryProxy(new AddRecommendDto()
            {
                MediaId = 2,
                //ADDetailID = 117483,
                BusinessType = (int)MediaType.WeiXin,
                CategoryId = 47018
                //ImageUrl = "http://mvavatar4.meitudata.com/56ebb9ac5398c1431.jpg"
            }, null).AddToRecommend();

            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("推荐添加：app,校验模板id是否正常")]
        [TestMethod]
        public void add_verify_app_verify_templateId_test()
        {
            var retValue = new RecommendQueryProxy(new AddRecommendDto()
            {
                MediaId = 2,
                ADDetailID = 117483,
                BusinessType = (int)MediaType.APP,
                CategoryId = 47018,
                TemplateID = 9
                //ImageUrl = "http://mvavatar4.meitudata.com/56ebb9ac5398c1431.jpg"
            }, null).AddToRecommend();

            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [Description("推荐添加：app，校验pubid是否正常")]
        [TestMethod]
        public void add_verify_app_verify_pubId_test()
        {
            var retValue = new RecommendQueryProxy(new AddRecommendDto()
            {
                MediaId = 2,
                ADDetailID = 117483,
                BusinessType = (int)MediaType.APP,
                CategoryId = 47018,
                TemplateID = 9
                //ImageUrl = "http://mvavatar4.meitudata.com/56ebb9ac5398c1431.jpg"
            }, null).AddToRecommend();

            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void Update_Test()
        {
            var retValue = new RecommendQueryProxy(null, new UpdateRecommendDto()
            {
                RecId = 1,
                ImageUrl = "http://",
                SortNumber = 9
            }).UpdateRecommend();
            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsTrue(retValue.HasError);
        }

        [TestMethod]
        public void Publish_Test()
        {
            var retValue = new RecommendQueryProxy(null, null).UpdatePublishState(14001);
            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void Delete_Test()
        {
            var retValue = new RecommendQueryProxy(null, null).DeleteRecommend(1);
            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void VerifyPublishCount_Test()
        {
            var retValue = new RecommendQueryProxy(null, null).VerifyPublishCount(new ReturnValue(), 14002);
            Console.Write(JsonConvert.SerializeObject(retValue));
            Assert.IsFalse(retValue.HasError);
        }

        [TestMethod]
        public void VerifyAppPublishCount_Test()
        {
        }
    }
}