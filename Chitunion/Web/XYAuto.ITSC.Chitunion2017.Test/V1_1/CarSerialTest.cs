/********************************************************
*创建人：lixiong
*创建时间：2017/7/25 10:12:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.Test.V1_1
{
    [TestClass]
    public class CarSerialTest
    {
        [TestMethod]
        public void GetMasterBrandList_test()
        {
            var list = BLL.CarSerialInfo.CarSerial.Instance.GetMasterBrandList();

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetBrandList_test()
        {
            var list = BLL.CarSerialInfo.CarSerial.Instance.GetBrandList(2);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetCarSerialList_test()
        {
            var list = BLL.CarSerialInfo.CarSerial.Instance.GetCarSerialList(218);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}