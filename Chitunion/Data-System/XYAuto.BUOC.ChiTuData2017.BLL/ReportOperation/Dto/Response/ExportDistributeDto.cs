/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 16:42:14
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
    public class ExportDistributeDto
    {
        [ExcelTitle("物料ID")]
        public int MaterielId { get; set; }

        [ExcelTitle("物料标题")]
        public string Title { get; set; }

        [ExcelTitle("物料url")]
        public string Url { get; set; }

        [ExcelTitle("业务类型")]
        public string BussinessType { get; set; }

        //pv统计
        [ExcelTitle("浏览量pv")]
        public long PV { get; set; }

        //uv统计
        [ExcelTitle("访客数uv")]
        public long UV { get; set; }

        //平均在线时长(秒)
        [ExcelTitle("平均在线时长")]
        public string OnLineAvgTimeFormt { get; set; }//要换算成为yyyy:MM:dd

        //跳出率
        [ExcelTitle("跳出率")]
        public string JumpProportion { get; set; }

        //人均浏览页面数
        [ExcelTitle("人均浏览页面数")]
        public int BrowsePageAvg { get; set; }

        //转发统计
        [ExcelTitle("转发数")]
        public long ForwardNumber { get; set; }
        //询价数
        [ExcelTitle("询价数")]
        public int InquiryNumber { get; set; }

        //会话数
        [ExcelTitle("会话数")]
        public int SessionNumber { get; set; }

        //电话接通数
        [ExcelTitle("电话接通数")]
        public int TelConnectNumber { get; set; }

        [ExcelTitle("组装时间")]
        public DateTime AssembleTime { get; set; }

        [ExcelTitle("分发时间")]
        public DateTime DistributeDate { get; set; }

        [ExcelTitle("组装操作人")]
        public string AssembleUser { get; set; }

        [ExcelTitle("分发操作人")]
        public string DistributeUser { get; set; }
    }
}