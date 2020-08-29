/********************************************************
*创建人：lixiong
*创建时间：2017/11/30 16:40:42
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Statistics;

namespace XYAuto.BUOC.ChiTuData2017.Test.ReportOperation.Statistics
{
    [TestClass]
    public class StatDailyExportProviderTest
    {
        [Description("日汇总数据-导出-抓取")]
        [TestMethod]
        public void ExportGrab()
        {
            new StatDailyExportProvider(new ReqDailyDto()
            {
                TabType = GetDailyTypeEnum.grab.ToString(),
                StartDate = "2017-11-13",
                EndDate = "2017-11-19"
            }).DoExport();
        }
    }
}