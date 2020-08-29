using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers.Tests
{
    [TestClass()]
    public class PublishV1_1ControllerTests
    {
        [TestMethod()]
        public void BackGQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AdQueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchAutoCompleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPublishStatisticsCountTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AuditPublishTest()
        {
            PublishV1_1Controller tc = new PublishV1_1Controller();
            var res = tc.AuditPublish(new Entities.DTO.AuditPublishReqDTO() { PubID = 30668, OpType = 42002,RejectReason = string.Empty });
            Assert.AreEqual(0, res.Status);
            res = tc.AuditPublish(new Entities.DTO.AuditPublishReqDTO() { PubID = 30668, OpType = 42012, RejectReason = string.Empty });
            Assert.AreEqual(0, res.Status);
            res = tc.AuditPublish(new Entities.DTO.AuditPublishReqDTO() { PubID = 30668, OpType = 42011, RejectReason = string.Empty });
            Assert.AreEqual(0, res.Status);
        }

        [TestMethod()]
        public void ModifyPublishTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeletePublishTest()
        {
            Assert.Fail();
        }
    }
}