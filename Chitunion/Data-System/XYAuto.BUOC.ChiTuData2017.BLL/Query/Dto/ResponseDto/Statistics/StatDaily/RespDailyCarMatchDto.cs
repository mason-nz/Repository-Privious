/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 13:42:00
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
    public class RespDailyCarMatchDto
    {
        [ExcelTitle("日期")]
        public DateTime Date { get; set; }

        [ExcelTitle("渠道")]
        public string ChannelName { get; set; }

        [ExcelTitle("已匹配车型")]
        public int MatchArticleCount { get; set; }

        [ExcelTitle("未匹配车型")]
        public int UnMatchArticleCount { get; set; }
    }
}