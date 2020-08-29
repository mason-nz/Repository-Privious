using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ChiTu2018.Service.App.AppInfo;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.API.Tests.LE
{
    [TestClass]
    public class FeedbackTest
    {
        [TestMethod]
        public void AddFeedbackInfo()
        {
            var ResultNum = LeFeedbackService.Instance.AddFeedbackInfo(new LeFeedbackDto { UserID = 188, OpinionText = "测试数据", ReplyText = "回复" });
           
            var JsonText = JsonConvert.SerializeObject(ResultNum);
        }
    }
}
