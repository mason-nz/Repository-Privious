/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.BaseDto
* 类 名 称 ：PaginationDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 14:47:42
********************************/

namespace XYAuto.ChiTu2018.Service.App.BaseDto
{
    public class PaginationDto
    {
        #region PageSize

        /// <summary>
        ///     设置或获取一个值，用于表示每一页的数据量。
        /// </summary>
        public int PageSize { get; set; }

        #endregion

        #region PageIndex

        /// <summary>
        ///     设置或获取一个值，用于表示当前页的索引值。
        /// </summary>
        public int PageIndex { get; set; }

        #endregion
    }
}
