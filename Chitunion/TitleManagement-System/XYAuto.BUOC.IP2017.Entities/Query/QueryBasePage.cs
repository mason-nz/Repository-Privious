using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.Query
{
    public class QueryPageBase<T>
    {
        protected QueryPageBase()
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
        /// <summary>
        /// 分页sql语句+where条件
        /// </summary>
        public string StrSql { get; set; }

        public string OrderBy { get; set; }

        public List<T> DataList { get; set; }
    }
}
