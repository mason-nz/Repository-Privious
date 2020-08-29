/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 10:29:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics
{
    public class ReqDailyDto : CreatePublishQueryBase
    {
        public string TabType { get; set; }
        public string StartDate { get; set; } = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
        public string EndDate { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        public int ArticleType { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int ChannelId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }

    public class ReqDetailsDto : ReqDailyDto
    {
        //public new string StartDate { get; set; } = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
        //public new string EndDate { get; set; } = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

        public int SceneId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int AAScoreType { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        /// <summary>
        /// 帐号分支
        /// </summary>
        public int AAScoreTypeAccount { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        /// <summary>
        /// 匹配状态
        /// </summary>
        public int MatchStatus { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        /// <summary>
        /// 初筛状态
        /// </summary>
        public int ConditionId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}