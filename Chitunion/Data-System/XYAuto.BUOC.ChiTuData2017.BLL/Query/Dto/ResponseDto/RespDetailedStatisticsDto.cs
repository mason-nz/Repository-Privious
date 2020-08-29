/********************************************************
*创建人：lixiong
*创建时间：2017/9/9 14:46:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto
{
    public class RespDetailedStatisticsDto
    {
        public int MaterielId { get; set; }
        public int DistributeId { get; set; }
        public DateTime Date { get; set; }
        public List<StatisticsDto> Item { get; set; }
    }

    public class StatisticsDto
    {
        [Newtonsoft.Json.JsonIgnore]
        public int MaterielId { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public int DistributeId { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public DateTime Date { get; set; }

        public string ArticleTypeName { get; set; }
        public string ConentTypeName { get; set; }
        public string Title { get; set; }
        public int PV { get; set; }
        public int UV { get; set; }
        public long ClickPV { get; set; }

        //点击uv
        public long ClickUV { get; set; }

        public int ClickNumber { get; set; }
        public int ForwardNumber { get; set; }
        public int LikeNumber { get; set; }
        public int ReadNumber { get; set; }
        public int ConentType { get; set; }

        //文章id
        public int ArticleId { get; set; }

        public int ArticleType { get; set; }

        public List<FootStatisticsDto> FootStatistics { get; set; }
    }

    public class FootStatisticsDto
    {
        public int ArticleId { get; set; }
        public string ArticleTypeName { get; set; }
        public string ConentTypeName { get; set; }
        public long ClickPV { get; set; }

        //点击uv
        public long ClickUV { get; set; }

        public int ReadNumber { get; set; }
    }
}