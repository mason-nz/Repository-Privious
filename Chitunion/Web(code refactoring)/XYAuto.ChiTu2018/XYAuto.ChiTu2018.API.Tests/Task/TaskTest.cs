using System;
using System.Collections.Generic;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.Controllers;
using XYAuto.ChiTu2018.Service.Task;
using XYAuto.ChiTu2018.Service.Task.Dto.UpdateUserScene;
using ReqDto = XYAuto.ChiTu2018.Service.Task.Dto.GetDataByPage.ReqDto;

namespace XYAuto.ChiTu2018.API.Tests.Task
{
    [TestClass]
    public class TaskTest
    {
        XYAuto.ChiTu2018.API.Controllers.TaskController ctl = new TaskController();
        [TestMethod]
        public void TestGetTaskListByUserId()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetTaskListByUserId(new ReqDto()
            {
                PageIndex = 0,//16447,
                PageSize = 10,
                SceneID = 0
            });

            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestIsSelectedSceneByUser()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.IsSelectedSceneByUser();

            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestUpdateUserScene()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = LeTaskInfoService.Instance.UpdateUserScene(new Service.Task.Dto.UpdateUserScene.ReqDto()
            {
                UserID = 1299,
                SceneInfo = new List<Scene>()
                {
                    new Scene()
                    {
                        SceneID = 1,
                        SceneName = "职场"
                    },
                    new Scene()
                    {
                        SceneID = 2,
                        SceneName = "孕产"
                    }
                }
            });

            Assert.AreEqual(true, ret);
        }

        [TestMethod]
        public void TestGetUnionAndUserId()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetUnionAndUserId();
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestGetShareOrderInfo()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetShareOrderInfo(821);
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestGetOrderByStatus()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetOrderByStatus(193001);
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestGetOrderUrl()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetOrderUrl(300);
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestSubmitOrderUrl()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.SubmitOrderUrl(new Service.Task.Dto.SubmitOrderUrl.ReqDto()
            {
                TaskId = 16447,
                OrderUrl = "http://wxtest-ct.qichedaquan.com/ct_m/20180507/300.html?utm_source=chitu&utm_term=hujur0mky0",
                UserId = 1299,
                PromotionChannelID = "ctlmhuifu"
            });
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestGetOrderInfo()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetOrderInfo(820);
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestGetSceneInfoByUserId()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetSceneInfoByUserId();
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }

        [TestMethod]
        public void TestGetUserDayOrderCount()
        {
            EntityFrameworkProfiler.Initialize();
            var ret = ctl.GetUserDayOrderCount();
            var retstr = JsonConvert.SerializeObject(ret);
            Assert.AreEqual(0, ret.Status);
        }
    }
}
