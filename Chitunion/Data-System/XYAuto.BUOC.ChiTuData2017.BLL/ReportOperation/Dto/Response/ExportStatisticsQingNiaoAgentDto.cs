/********************************************************
*创建人：lixiong
*创建时间：2017/9/21 10:47:34
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Dto.Response
{
    public class ExportStatisticsQingNiaoAgentDto
    {
        [ExcelTitle("时间")]
        public string Date { get; set; }

        [ExcelTitle("物料")]
        public string Title { get; set; }

        /// <summary>
        /// 文章类型（图文，商务专题）
        /// </summary>
        [ExcelTitle("类型")]
        public string ArticleTypeName { get; set; }

        [ExcelTitle("浏览量pv")]
        public string Pv { get; set; }

        [ExcelTitle("访客数uv")]
        public string Uv { get; set; }

        [ExcelTitle("点击pv")]
        public string ClikcPv { get; set; }

        [ExcelTitle("点击uv")]
        public string ClikcUv { get; set; }

        [ExcelTitle("点赞数")]
        public string LikeNumber { get; set; }

        [ExcelTitle("转发数")]
        public string ForwardNumber { get; set; }

        [ExcelTitle("阅读数")]
        public string ReadNumber { get; set; }
    }
}