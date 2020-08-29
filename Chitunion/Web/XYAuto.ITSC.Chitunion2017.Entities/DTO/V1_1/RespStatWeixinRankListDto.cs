/********************************************************
*创建人：lixiong
*创建时间：2017/7/10 16:17:31
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1
{
    public class RespStatWeixinRankListDto
    {
        public int MediaID { get; set; }
        public int BaseMediaID { get; set; }
        public string WxNum { get; set; }
        public string Name { get; set; }
        public string HeadIconURL { get; set; }
        public decimal MaLiIndex { get; set; }
        public int AvgTopArticleReadNum { get; set; }
        public int AvgTopArticleLikeNum { get; set; }
        public int MaxReadNum { get; set; }
        public int PublishArticleNum { get; set; }
        public int PublishCount { get; set; }
    }

    public class RespStatWeixinRankItemDto
    {
        public DateTime LastModifyTime { get; set; }
    }
}