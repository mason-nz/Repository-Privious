/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 16:51:32
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query.Media
{
    public class TwoBarCodeHistoryQuery<T> : QueryPageBase<T>
    {
        public string RecIds { get; set; }
        public int MediaType { get; set; }
        public int MediaId { get; set; }
        public string OrderId { get; set; }
    }

    public class ChannelQuery<T> : QueryPageBase<T>
    {
        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int MediaId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int AdPosition1 { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int AdPosition2 { get; set; } = 7002;
        public int AdPosition3 { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public string CooperateDate { get; set; }
    }
}