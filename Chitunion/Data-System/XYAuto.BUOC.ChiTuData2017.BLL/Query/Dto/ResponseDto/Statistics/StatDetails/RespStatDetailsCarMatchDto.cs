/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 18:53:58
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDetails
{
    public class RespStatDetailsCarMatchDto
    {
        [ExcelTitle("文章ID")]
        public int ArticleId { get; set; } //ArticelID

        [ExcelTitle("标题")]
        public string Title { get; set; }

        [ExcelTitle("渠道")]
        public string ChannelName { get; set; }

        [ExcelTitle("发布时间")]
        public DateTime ArticlePublishTime { get; set; }

        [ExcelTitle("抓取时间")]
        public DateTime ArticleSpiderTime { get; set; }

        [ExcelTitle("匹配车型时间")]
        public DateTime MatchCarTime { get; set; }

        [ExcelTitle("品牌")]
        public string BrandName { get; set; }

        [ExcelTitle("车型")]
        public string SerialName { get; set; }

        [ExcelTitle("文章分值")]
        public decimal ArticleSorce { get; set; }

        [ExcelTitle("状态")]
        public string MatchName { get; set; }
    }
}