using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DicInfo;

namespace XYAuto.BUOC.ChiTuData2017.Test.DataCenter
{
    [TestClass]
    public class DictInfoTest
    {
        [TestMethod]
        public void GetDicInfo()
        {
          var q=  DictInfo.Instance.GetDictInfoByTypeId(7);

            var q1 = DictInfo.Instance.GetDictInfoByTypeId(73);

            var q2 = DictInfo.Instance.GetDictInfoByTypeId(74);

            var q3 = DictInfo.Instance.GetDictInfoByTypeId(75);



            var q4 = DictInfo.Instance.GetDictInfoByTypeId(76);

            var q5 = DictInfo.Instance.GetDictInfoByTypeId(77);
            var q6 = DictInfo.Instance.GetDictInfoByTypeId(8001);
            var q7 = DictInfo.Instance.GetDictInfoByTypeId(8002);
        }
    }
}
