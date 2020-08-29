/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 14:12:13
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using XYAuto.BUOC.ChiTuData2017.BLL.AutoMapperConfig.Profile;
using XYAuto.BUOC.ChiTuData2017.BLL.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;

namespace XYAuto.BUOC.ChiTuData2017.Test.Query
{
    [TestClass]
    public class DistributeQueryTest
    {
        [TestMethod]
        public void DistributeQuery()
        {
            var request = new RequestDistributeQueryDto()
            {
                StartDate = "2017-07-07",
                EndDate = "2017-11-20",
                //DistributeType = (int)DistributeTypeEnum.QuanWangYu
            };
            var list = new DistributeQuery(new ConfigEntity()).GetQueryList(request);

            //var totleSumList = new DistributeProvider().GetDistributeTotals(request.SqlWhere);

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void RespDistributeListDto_Test()
        {
            var dto = new RespDistributeListDto()
            {
                DistributeUser = ""
            };

            Console.WriteLine(JsonConvert.SerializeObject(dto));

            decimal a = 0.899m;
            Console.WriteLine(a.ToString("p"));
            a = 0m;
            Console.WriteLine(a.ToString("p"));
        }

        [TestMethod]
        public void GetIpInfo_Test()
        {
            var strList = DistributeProfile.GetIpInfo("6001,萌爱|6002,可爱", LableTypeEnum.Ip);
            Console.WriteLine(strList);

            Console.WriteLine(DateTime.Now.AddDays(-7));
        }
    }
}