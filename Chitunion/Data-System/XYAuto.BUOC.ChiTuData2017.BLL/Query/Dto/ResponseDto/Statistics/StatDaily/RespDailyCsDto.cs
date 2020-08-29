/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 13:50:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily
{
    /// <summary>
    /// 初筛
    /// </summary>
    public class RespDailyCsDto
    {
        [ExcelTitle("初筛日期")]
        public DateTime Date { get; set; }

        [ExcelTitle("渠道")]
        public string ChannelName { get; set; }

        [ExcelTitle("可用文章数")]
        public int ArticleCount { get; set; }

        [ExcelTitle("可用账号数")]
        public int AccountCount { get; set; }

        [ExcelTitle("置为腰文章数")]
        public int ToBodyArticleCount { get; set; }

        [ExcelTitle("置为腰账号数")]
        public int ToBodyAccountCount { get; set; }

        [ExcelTitle("作废文章数")]
        public int NotUseArticleCount { get; set; }

        [ExcelTitle("作废账号数")]
        public int NotUseAccountCount { get; set; }

        [JsonIgnore]
        public string StatInfo { get; set; }
    }
}