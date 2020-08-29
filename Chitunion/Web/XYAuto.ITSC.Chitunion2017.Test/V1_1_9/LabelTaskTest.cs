using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_9
{
    [TestClass]
    public class LabelTaskTest
    {
        private LabelTaskController ctl = new LabelTaskController();
        private XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult ret = null;
        [TestMethod]
        public void LabelProjectCreateTest()
        {
            Entities.LabelTask.DTO.ReqProjectCreateDTO reqDto = new Entities.LabelTask.DTO.ReqProjectCreateDTO()
            {
                Name = "项目名称20170808-01",
                ProjectType = (int)Entities.LabelTask.ENUM.EnumProjectType.媒体,
                TaskCount=500,
                UploadFileURL="afdaaas"
            };
            ret = ctl.LabelProjectCreate(reqDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void LabelStatisticsTest()
        {
            ret = ctl.LabelStatistics(61001, "http://www.chitunion.com/UploadFiles/2017/8/10/10/媒体账号$14bb6d50-0902-48b6-aa2e-e2c94e80432f.xlsx");
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void GetSummaryKeyWordTest()
        {
            ret = ctl.GetSummaryKeyWord(60, 9, 3);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
        [TestMethod]
        public void LabelListQueryTest()
        {
            Entities.LabelTask.DTO.ReqLabelListQueryDTO reqDto = new Entities.LabelTask.DTO.ReqLabelListQueryDTO() {
                projectType=-2,
                //keyWord="",
                Status = 63004,
                auditUserID=-2,
                submitBeginTime=new DateTime(1990,1,1),
                submitEndTime = new DateTime(1990, 1, 1),
                auditBeginTime = new DateTime(1990, 1, 1),
                auditEndTime = new DateTime(1990, 1, 1),
                pageIndex=1,
                pageSize=20
            };
            ret = ctl.LabelListQuery(reqDto);
            NUnit.Framework.Assert.AreEqual(0, ret.Status);
        }
    }
}
