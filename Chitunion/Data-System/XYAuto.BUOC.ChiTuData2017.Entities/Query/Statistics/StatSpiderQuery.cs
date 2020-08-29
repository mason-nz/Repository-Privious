/********************************************************
*创建人：lixiong
*创建时间：2017/11/23 18:14:53
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum.Statistics;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Query.Statistics
{
    public class StatSpiderQuery<T> : QueryPageBase<T>
    {
        //统计开始时间
        public string BeginTime { get; set; } = DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd");

        //统计结束时间
        public string EndTime { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

        public StatArticleTypeEnum ArticleType { get; set; } = StatArticleTypeEnum.None;

        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public OperatorEnum FilterChannelId { get; set; } = OperatorEnum.无;

        public OperatorEnum FilterSceneId { get; set; } = OperatorEnum.无;

        public OperatorEnum FilterAutoCategory { get; set; } = OperatorEnum.无;

        public OperatorEnum FilterScoreType { get; set; } = OperatorEnum.无;

        public OperatorEnum FilterArticleType { get; set; } = OperatorEnum.无;

        public ScoreTypeEnum ScoreType = ScoreTypeEnum.无;
    }
}