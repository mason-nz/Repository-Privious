/********************************************************
*创建人：lixiong
*创建时间：2017/12/5 14:54:44
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
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.Test.ReportOperation
{
    [TestClass]
    public class ExportCsvHelperTest
    {
        public ExportCsvHelperTest()
        {
        }

        [TestMethod]
        public void ExportTest()
        {
            var list = new StatDailyGrabQuery(new ConfigEntity()).GetQueryList(new ReqDailyDto()
            {
                StartDate = "2017-11-28",
                EndDate = "2017-12-04"
            });

            var titleList = new List<string>()
            {
                "抓取日期","头腰文章类型","抓取渠道","抓取文章量","抓取账号量"
            };
            //Console.WriteLine(JsonConvert.SerializeObject(list));
            new ExportCsvHelper(titleList, list.List.ToDataTable()).DataToCSV($"{DateTime.Now}");
        }

        [TestMethod]
        public void DataToCsvByDicTitleTest()
        {
            var list = new StatDailyGrabQuery(new ConfigEntity()).GetQueryList(new ReqDailyDto()
            {
                StartDate = "2017-11-28",
                EndDate = "2017-12-04"
            });

            var titleList = new List<string>()
            {
                "抓取日期","头腰文章类型","抓取渠道","抓取文章量","抓取账号量"
            };
            var dicTitle = new Dictionary<string, string>()
            {
                { "Date","抓取日期"},
                { "ArticleTypeName","头腰文章类型"},
                { "ChannelName","抓取渠道"},
                { "ArticleCount","抓取文章量"},
                { "AccountCount","抓取账号量"}
            };
            //Console.WriteLine(JsonConvert.SerializeObject(list));
            new ExportCsvHelper(dicTitle, list.List.ToDataTable()).DataToCsvByDicTitle();
        }

        [TestMethod]
        public void CsvStatDetailsExportProviderTest()
        {
            var retValue = new StatDetailsExportProvider(new ReqDetailsDto()
            {
                TabType = "jxrk",
                StartDate = "2017-11-05",
                EndDate = "2017-12-04"
            }).DoExport();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }

        [TestMethod]
        public void CsvStatDailyExportProviderTest()
        {
            var retValue = new StatDailyExportProvider(new ReqDailyDto()
            {
                TabType = "cxpp",
                StartDate = "2017-11-05",
                EndDate = "2017-12-04"
            }).DoExport();
            Console.WriteLine(JsonConvert.SerializeObject(retValue));
        }
    }
}