using XYAuto.ChiTu2018.Entities.Extend;

namespace XYAuto.ChiTu2018.Service.App.Task.Dto.GetOrderByStatus
{
    /// <summary>
    /// 注释：ReqDto
    /// 作者：lihf
    /// 日期：2018/5/10 17:12:24
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ReqDto : Pagination
    {
        public int UserID { get; set; } = -2;
        public int Status { get; set; } = -2;
        public int totalCount { get; set; } = 0;
    }
}
