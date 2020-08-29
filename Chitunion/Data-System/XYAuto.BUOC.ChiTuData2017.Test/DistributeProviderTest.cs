/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 17:07:08
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
using XYAuto.BUOC.ChiTuData2017.BLL.Query;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.DetailedStatistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Distribute;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Export;

namespace XYAuto.BUOC.ChiTuData2017.Test
{
    [TestClass]
    public class DistributeProviderTest
    {
        private readonly DistributeProvider _dsDistributeProvider;

        public DistributeProviderTest()
        {
            BLL.AutoMapperConfig.AutoMapperConfig.Configure();
            _dsDistributeProvider = new DistributeProvider();
        }

        [TestMethod]
        public void GetMaterielInfo_Test()
        {
            var info = _dsDistributeProvider.GetMaterielInfo(9910, 73002);
            Console.WriteLine(JsonConvert.SerializeObject(info));
        }

        [TestMethod]
        public void DistributeDetailedQuery_Test()
        {
            var query = new DistributeDetailedQuery(new ConfigEntity());
            var list = query.GetQueryList(new RequestDistributeQueryDto()
            {
                MaterielId = 1
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetChannelDetaileds_Test()
        {
            var list = _dsDistributeProvider.GetChannelDetaileds(1, new List<int>());
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void DistributeQueryProxy_chitu_Test()
        {
            var list = new DistributeQueryProxy(new ConfigEntity(), new RequestDistributeQueryDto()
            {
                MaterielId = 1,
                DistributeType = (int)DistributeTypeEnum.QuanWangYu,
                StartDate = "2017-09-07",
                EndDate = "2017-09-10"
            }).GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void DistributeQueryProxy_Agent_Test()
        {
            var list = new DistributeQueryProxy(new ConfigEntity(), new RequestDistributeQueryDto()
            {
                MaterielId = 1,
                DistributeType = (int)DistributeTypeEnum.QingNiaoAgent,
                StartDate = "2017-09-07",
                EndDate = "2017-09-10"
            }).GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Description("物料详情数据统计(日结统计之下)")]
        [TestMethod]
        public void DetailedStatisticsQuery_Test()
        {
            var list = new DetailedStatisticsQuery(null).GetQueryList(new RequestDistributeQueryDto()
            {
                MaterielId = 53,
                DistributeId = 18,
                DistributeType = (int)DistributeTypeEnum.QingNiaoAgent,
                StartDate = "2017-09-06",
                EndDate = "2017-09-20"
            });
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetOnLineAvgTimeFormt_Test()
        {
            var initDate = new DateTime(1970, 01, 01, 00, 00, 00);

            //Console.WriteLine(initDate.AddYears(-1970));

            var ts = (initDate.AddSeconds(95) - initDate);

            var strtime = ts.TotalSeconds;// .ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"s:{ ts.TotalSeconds}");
            Console.WriteLine($"h:{ ts.TotalMinutes}");

            Console.WriteLine($"s1:{ ts.Seconds}");
            Console.WriteLine($"h:{ ts.Minutes}");

            Console.WriteLine(DistributeProfile.GetOnLineAvgTimeFormt(95));
            Console.WriteLine(DistributeProfile.GetOnLineAvgTimeFormt(1095));
        }

        [TestMethod]
        public void GetArticleInfo_Test()
        {
            //var list = _dsDistributeProvider.GetArticleInfo(XyAttrTypeEnum.Body, "2017-09-18", 100, 20);

            var list = _dsDistributeProvider.GetArticleInfo(new DistributeQuery<MaterielTemp>()
            {
                Date = "2017-10-11"
            });

            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [TestMethod]
        public void GetBrowsePageAvg_Test()
        {
            //var avg = Math.Round(1000 * 1.0 / 1200, 0);
            //Console.WriteLine(avg);
            //Console.WriteLine(Math.Round(avg * 1.0));
            //Console.WriteLine(Convert.ToInt32(avg));
            //var b = DistributeProfile.GetBrowsePageAvg(1000, 1200, 0);
            //Console.WriteLine(b);

            var c = DistributeProfile.GetAvg(Convert.ToDouble(100), 2);
            Console.WriteLine(c);

            var sp = "1,".Split(',');
            Console.WriteLine(sp[0] + "," + sp[1]);
            var sp1 = "1".Split(',');
            Console.WriteLine(sp1[0] + "," + (sp1.Length > 1 ? sp1[1] : string.Empty));
        }

        private string GetDateFormat(int num)
        {
            return num <= 0 ? $"0{num}" : num.ToString();
        }

        [TestMethod]
        public void GetDistributeList_Test()
        {
            RequestDistributeQueryDto request = new RequestDistributeQueryDto();
            request.PageSize = 20;
            request.PageIndex = 1;
            request.StartDate = "2017-12-08";
            request.EndDate = "2017-12-15";
            request.DistributeType = -2;
            request.AssembleUser = null;
            request.DistributeUser = null;
            request.MaterielName = null;
            request.CarSerialId = -2;
            request.BrandId = -2;
            request.CarSerialName = null;
            request.ChannelId = -2;
            request.Ip = -2;
            request.ChildIp = -2;
            request.ExpChannelName = "--";
            request.ExpCarSerialName = "--% 2C-- % 2C-- % 2C";
            request.ExpIpName = "--";
            request.ExpChildIpName = "--";
            var t = new DistributeQuery(new ConfigEntity()).GetQueryList(request);
        }

        [TestMethod]
        public void GetDailyQuery_Test()
        {
            RequestDistributeQueryDto request = new RequestDistributeQueryDto();
            request.PageSize = 20;
            request.PageIndex = 1;
            request.DistributeType = 73002;
            request.MaterielId = 9018;
            request.StartDate = null;
            request.EndDate = null;

            var t = new DistributeQueryProxy(null, request).GetQuery();
        }

        [TestMethod]
        public void Export_Test()
        {
            DistributeExportDto request = new DistributeExportDto();

            request.PageSize = 10;
            request.PageIndex = 1;
            request.StartDate = "2017-10-21";
            request.EndDate = "2017-11-19";
            request.DistributeType = 73002;
            request.AssembleUser = "";
            request.DistributeUser = "";
            request.MaterielName = "";
            request.CarSerialId = -2;
            request.BrandId = -2;
            request.CarSerialName = "";
            request.ChannelId = -2;
            request.Ip = -2;
            request.ChildIp = -2;
            request.ExpChannelName = "--";
            request.ExpCarSerialName = "--,--,--,";
            request.ExpIpName = "--";
            request.ExpChildIpName = "--";

            request.BusinessType = ExportBusinessType.Distribute;
            var t = new DistributeExportProvider(request).Export();
        }

        [TestMethod]
        public void GetDetailsQuery_Test()
        {
            RequestDistributeQueryDto request = new RequestDistributeQueryDto();
            request.PageSize = 20;
            request.PageIndex = 1;
            request.StartDate = null;
            request.EndDate = null;
            request.DistributeType = 73001;
            request.MaterielId = 6766;
            request.DistributeId = 109799;
            var t = new DetailedStatisticsQuery(null).GetQueryList(request);
        }
    }
}