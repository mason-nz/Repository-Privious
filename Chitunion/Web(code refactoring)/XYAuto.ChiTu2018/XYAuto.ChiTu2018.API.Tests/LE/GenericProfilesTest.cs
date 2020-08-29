/********************************
* 项目名称 ：XYAuto.ChiTu2018.API.Tests.LE
* 类 名 称 ：GenericProfilesTest
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 18:31:44
********************************/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Service.App.Profiles;
using XYAuto.ChiTu2018.Service.App.Profiles.Dto;

namespace XYAuto.ChiTu2018.API.Tests.LE
{
    [TestClass]
    public class GenericProfilesTest
    {
        [TestMethod]
        public void XmlConfig()
        {
            var NodelModel = GenericProfilesService.Instance.GetConfigurationInfo(new ConfigurationDto
            {
                UserID = 112,
                NodeType = "pz_hytc"
            });

            var shuzuss = JsonConvert.SerializeObject(NodelModel);

        }
    }
}
