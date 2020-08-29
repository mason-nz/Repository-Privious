using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.ChiTu2018.Service.Task;
using Newtonsoft.Json;

namespace XYAuto.ChiTu2018.API.Tests
{
    [TestClass]
    public class TaskTest
    {
        [TestMethod]
        public void TestGetDataByPage()
        {
            var list = LeTaskInfoService.Instance.GetDataByPage(new Service.Task.Dto.GetDataByPage.ReqDto() { PageIndex = 0, PageSize = 10, SceneID = 0 });
            Console.WriteLine(JsonConvert.SerializeObject(list));
            Assert.Equals(list.TaskInfo.Count>0, true);
        }
        [TestMethod]
        public void TestIsSelectedSceneByUser()
        {
            var ret = LeTaskInfoService.Instance.IsSelectedSceneByUser(14351);
            Console.WriteLine(JsonConvert.SerializeObject(ret));
            Assert.Equals(ret, true);
        }
    }
}
