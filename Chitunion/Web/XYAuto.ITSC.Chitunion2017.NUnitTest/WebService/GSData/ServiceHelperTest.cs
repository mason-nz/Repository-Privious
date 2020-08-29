using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.WebService.GSData;

namespace XYAuto.ITSC.Chitunion2017.NUnitTest.WebService.GSData
{
    [TestFixture]
    public class ServiceHelperTest
    {
        private Chitunion2017.WebService.GSData.GSDataHelper gsdata;
        [SetUp]
        public void ServiceHelper()
        {
            gsdata = new Chitunion2017.WebService.GSData.GSDataHelper();
        }

        [Test]
        public void GetGroupInfoTest()
        {
            Chitunion2017.WebService.GSData.GSDataGroupResult res = gsdata.GetGroupInfo();
            Assert.AreNotEqual(null, res);

            //string msg = "{   \"returnCode\" :\"1001\",   \"returnMsg\" :\"接口调用成功\",   \"feeCount\" :1996.0,   \"returnData\" :{                \"nicknameCount\" :0,      \"groupCount\" :0,      \"groupList\" :[]    }}";
            //GSDataResult gsDataResult = JsonConvert.DeserializeObject<GSDataResult>(msg);

        }
    }
}
