using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;

namespace XYAuto.ChiTu2018.Service.App.Query.Dto
{
    /// <summary>
    /// 注释：PublishQueryBase 查询基类，作用域只能在 api 调用层，service,与 QueryPageBase 进行中转交互
    /// 作者：lix
    /// 日期：2018/5/15 14:16:24
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class BaseRequestQueryEntity
    {
        public int OrderBy { get; set; }//排序

        [Necessary(MtName = "PageIndex", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页记录条数
        /// </summary>
        [Necessary(MtName = "PageSize", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int PageSize { get; set; } = 20;
        
    }
}
