using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace XYAuto.ChiTu2018.Entities.Query
{
    /// <summary>
    /// 注释：分页查询构造实体：GetPageBase 作用域在 service，BO
    /// 作者：lix
    /// 日期：2018/5/15 14:16:24
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <typeparam name="TKey">排序字段类型</typeparam>
    public class GetPageBase<T, TKey>
    {
        public GetPageBase()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
        }

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        private int _pageSize;

        /// <summary>
        /// 每页记录条数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize > 100)
                {
                    _pageSize = 100;
                }
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
        /// <summary>
        /// 总记录条数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        public List<T> DataList { get; set; }
        /// <summary>
        /// where 条件
        /// </summary>
        public Expression<Func<T, bool>> Expression { get; set; }
        public SortOrder SortOrder { get; set; }
        public Expression<Func<T, TKey>> Order { get; set; }
    }
}
