/********************************
* 项目名称 ：XYAuto.ChiTu2018.Entities.Extend
* 类 名 称 ：Pagination
* 作    者 ：zhangjl 
* 描    述 ：分页基础实体类
* 创建时间 ：2018/5/10 11:12:49
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Extend
{
    public class Pagination
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
