/********************************************************
*创建人：lixiong
*创建时间：2017/9/11 19:57:17
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;

namespace XYAuto.BUOC.ChiTuData2017.ExternalApi.ZHY.Dto.Request
{
    public class PullStatisticsDto
    {
        public int MaterielId { get; set; }

        [Necessary(MtName = "头部文章HeadArticleId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int HeadArticleId { get; set; }

        [Necessary(MtName = "DateTime:2017-08-02")]
        public string DateTime { get; set; }
    }
}