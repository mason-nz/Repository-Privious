/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 10:27:41
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily
{
    public class RespDailyGrabDto
    {
        [ExcelTitle("抓取日期")]
        public DateTime Date { get; set; }

        [ExcelTitle("头腰文章类型")]
        public string ArticleTypeName { get; set; }

        [ExcelTitle("抓取渠道")]
        public string ChannelName { get; set; }

        [ExcelTitle("抓取文章量")]
        public int ArticleCount { get; set; }

        [ExcelTitle("抓取账号量")]
        public int AccountCount { get; set; }
    }
}