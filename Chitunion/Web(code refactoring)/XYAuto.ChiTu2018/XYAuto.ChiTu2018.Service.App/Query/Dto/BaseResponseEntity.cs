using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.App.Query.Dto
{
    /// <summary>
    /// 注释：BaseResponseEntity 分页/查询 返回基类，作用域只能在 api 调用层，service,与 QueryPageBase 进行中转交互
    /// 作者：lix
    /// 日期：2018/5/15 12:54:19
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class BaseResponseEntity<T> : BaseResponseEntity
    {
        public List<T> List { get; set; }
    }

    public class BaseResponseList<T> : BaseResponseEntity
    {
        public T List { get; set; }
    }

    public class BaseResponseEntity
    {
        public int TotleCount { get; set; }
        public dynamic Extend { get; set; }
    }
}
