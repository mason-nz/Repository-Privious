/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 16:26:58
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab
{
    public class RespGrabBaseDto
    {
        public StatDate Date { get; set; }
        public List<StatInfo> Info { get; set; }
        public string[] DataLegend { get; set; }
    }

    public class StatDate
    {
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
    }

    public class StatInfo
    {
        public string Name { get; set; }
        public int ArticleCount { get; set; }
        public int AccountCount { get; set; }
        public int TypeId { get; set; }
    }
}