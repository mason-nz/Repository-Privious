using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ITSC.Chitunion2017.WebAPI.Controllers;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1_9
{
    [TestClass]
    public class LableInfoTest
    {
        private LabelTaskController ctl = new LabelTaskController();
        private XYAuto.ITSC.Chitunion2017.WebAPI.Common.JsonResult ret = null;
        [TestMethod]
        public void SelectTaskLableInfo()
        {
            string Message = "";
            ctl.SelectMediaOrArticleLable(1,1);
        }
    }
}
