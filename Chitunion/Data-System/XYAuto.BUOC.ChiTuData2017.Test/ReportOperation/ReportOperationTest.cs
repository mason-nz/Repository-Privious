/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 14:36:09
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
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Export;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.Test.ReportOperation
{
    [TestClass]
    public class ReportOperationTest
    {
        private DistributeExportProvider _distributeExportProvider;

        public ReportOperationTest()
        {
            BLL.AutoMapperConfig.AutoMapperConfig.Configure();
        }

        [Description("分发明细导出-日结数据-查询数据源")]
        [TestMethod]
        public void Export_Query_DistributeDetails_Daily_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                MaterielId = 1,
                BusinessType = ExportBusinessType.DistributeDetails,
                ExportType = ExportTypeEnum.Daily
            });

            var tup = _distributeExportProvider.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("分发明细导出-日结-渠道数据-查询数据源")]
        [TestMethod]
        public void Export_Query_DistributeDetails_Channel_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                MaterielId = 1,
                BusinessType = ExportBusinessType.DistributeDetails,
                ExportType = ExportTypeEnum.Channel,
                StartDate = "2017-09-07",
                EndDate = "2017-09-10"
            });

            var tup = _distributeExportProvider.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("物料明细导出-日结下面的-查询数据源")]
        [TestMethod]
        public void Export_Query_MaterielDetails_DailyDetails_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                MaterielId = 2638,
                DistributeId = 202,
                DistributeType = (int)DistributeTypeEnum.QingNiaoAgent,
                BusinessType = ExportBusinessType.MaterielDetails,
                //StartDate = "2017-09-18",
                //EndDate = "2017-09-18"
            });

            var tup = _distributeExportProvider.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("分发明细导出-日结数据")]
        [TestMethod]
        public void Export_DistributeDetails_Daily_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                MaterielId = 1,
                BusinessType = ExportBusinessType.DistributeDetails,
                ExportType = ExportTypeEnum.Daily
            });

            var tup = _distributeExportProvider.Export();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("分发明细导出-日结-渠道数据")]
        [TestMethod]
        public void Export_DistributeDetails_Channel_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                MaterielId = 6339,
                BusinessType = ExportBusinessType.DistributeDetails,
                ExportType = ExportTypeEnum.Daily,
                DistributeType = (int)DistributeTypeEnum.QingNiaoAgent,
                StartDate = "2017-09-20",
                EndDate = "2017-11-19"
            });

            var tup = _distributeExportProvider.Export();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("分发导出-物料分发日结下面-详情统计")]
        [TestMethod]
        public void Export_MaterielDetails_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                MaterielId = 6339,
                DistributeId = 2,
                DistributeType = (int)DistributeTypeEnum.QuanWangYu,
                BusinessType = ExportBusinessType.MaterielDetails,
                StartDate = "2017-09-18",
                EndDate = "2017-09-18"
            });

            var tup = _distributeExportProvider.Export();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("分发导出-物料分发")]
        [TestMethod]
        public void Export_Distribute_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                BusinessType = ExportBusinessType.Distribute,
                StartDate = "2017-07-07",
                EndDate = "2017-11-20",
                PageIndex = 1,
                PageSize = 5
            });

            var tup = _distributeExportProvider.Export();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [Description("分发导出-数据查询-物料分发")]
        [TestMethod]
        public void Export_Query_Distribute_Test()
        {
            _distributeExportProvider = new DistributeExportProvider(new DistributeExportDto()
            {
                BusinessType = ExportBusinessType.Distribute,
                StartDate = "2017-08-07",
                EndDate = "2017-09-12",
                PageIndex = 1,
                PageSize = 3
            });

            var tup = _distributeExportProvider.GetQuery();

            Console.WriteLine(JsonConvert.SerializeObject(tup));
        }

        [TestMethod]
        public void Export_Test()
        {
            var json = "[{\"Date\":\"2017-09-10T00:00:00\",\"PV\":1100,\"UV\":1200,\"OnLineAvgTimeFormt\":\"00:01:30\",\"JumpProportion\":0.98,\"BrowsePageAvg\":89,\"InquiryNumber\":333,\"SessionNumber\":21,\"TelConnectNumber\":12}]";

            var dtoList = JsonConvert.DeserializeObject<List<ExportDistributeDetailedDto>>(json);

            ExcelHelper<ExportDistributeDetailedDto> excelHelper = new ExcelHelper<ExportDistributeDetailedDto>();
            excelHelper.SaveExcelToFile(dtoList, @"d:\", "测试.xlsx");
        }

        [TestMethod]
        public void Test_In()
        {
            var d = "--,--".IndexOf("--", StringComparison.Ordinal) > 0 ? "为选择" : "--,--";
            Console.WriteLine(d);
            d = "--,--".IndexOf("--", StringComparison.Ordinal) >= 0 ? "为选择" : "--,--";
            Console.WriteLine(d);
        }
    }
}