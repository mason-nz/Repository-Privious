/********************************************************
*创建人：lixiong
*创建时间：2017/7/10 16:22:18
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Query.Ranking
{
    public class RankingQuery<T> : QueryPageBase<T>
    {
        public int CategoryId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int CreateUserId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}