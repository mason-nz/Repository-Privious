using XYAuto.ChiTu2018.Entities.Constants;

namespace XYAuto.ChiTu2018.Service.App.Query.Dto.Request
{
    /// <summary>
    /// 注释：ReqInComeDto
    /// 作者：lix
    /// 日期：2018/5/15 10:41:00
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqInComeDto : BaseRequestQueryEntity
    {
        public int UserId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int OrderType { get; set; } = Constant.INT_INVALID_VALUE;
        public int PayType { get; set; } = Constant.INT_INVALID_VALUE;

        public string PayStartDate { get; set; }
        public string PayEndDate { get; set; }
    }
}
