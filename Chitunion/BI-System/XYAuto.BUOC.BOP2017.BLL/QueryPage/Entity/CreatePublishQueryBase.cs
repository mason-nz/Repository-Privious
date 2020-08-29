/********************************************************
*创建人：lixiong
*创建时间：2017/9/8 15:22:43
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.QueryPage.Entity
{
    public class CreatePublishQueryBase
    {
        public int OrderBy { get; set; }//排序

        [Necessary(MtName = "PageIndex", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页记录条数
        /// </summary>
        [Necessary(MtName = "PageSize", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int PageSize { get; set; } = 20;

        public string SqlWhere { get; set; }
    }
}