/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 16:36:32
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
    public class RespStatDetailsGrabDto
    {
        [ExcelTitle("文章ID")]
        public int ArticleId { get; set; }

        [ExcelTitle("标题")]
        public string Title { get; set; }

        [ExcelTitle("头腰类型")]
        public string ArticleTypeName { get; set; }

        [ExcelTitle("渠道")]
        public string ChannelName { get; set; }

        [ExcelTitle("发布时间")]
        public DateTime ArticlePublishTime { get; set; }

        [ExcelTitle("抓取时间")]
        public DateTime ArticleSpiderTime { get; set; }

        [ExcelTitle("场景")]
        public string SceneName { get; set; }

        [ExcelTitle("账号")]
        public string AccountName { get; set; }
    }
}