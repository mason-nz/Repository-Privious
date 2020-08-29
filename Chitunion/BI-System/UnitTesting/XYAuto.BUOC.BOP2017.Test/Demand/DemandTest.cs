using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.Entities.Dto;

namespace XYAuto.BUOC.BOP2017.Test.Demand
{
    [TestClass]
    public class DemandTest
    {
        [TestMethod]
        public void MapExportToExcel_Test()
        {
            var json = "{\"DemandBillNo\":200001,\"BeginDate\":\"2017-10-13\",\"EndDate\":\"2017-10-17\"}";

            string msg = string.Empty;
            string path = BLL.Demand.Demand.Instance.MapExportToExcel(JsonConvert.DeserializeObject<MapGetRightReqDTO>(json), ref msg);
            //return Util.GetJsonDataByResult(path, msg, string.IsNullOrEmpty(path) ? 1 : 0);
        }


        [TestMethod]
        public void MapGetRightTwo_Test()
        {
            string msg = string.Empty;
            MapGetRightReqDTO req = new MapGetRightReqDTO();
            req.ADGroupID = 39513561;
            MapGetRightTwoResDTO res = BLL.Demand.Demand.Instance.MapGetRightTwo(req, ref msg);
        }
    }
}
