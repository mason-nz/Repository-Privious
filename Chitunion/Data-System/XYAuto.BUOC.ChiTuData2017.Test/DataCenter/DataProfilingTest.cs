using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{

    [TestClass]
    public class DataProfilingTest
    {
        [TestMethod]
        public void RenderClue()
        {

            var query = new DataViewProvider(7, "wlfz,zf,wlff,xshq").GetGrabData();
                                                      
            var text = Util.GetJsonDataByResult(query, "Success");

            string tem = JsonConvert.SerializeObject(text);

            Assert.AreEqual(0, 0);
        }
    }
}
