using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter
{
    public class Pagination
    {
        /// <summary>
        ///     设置或获取一个值，用于表示每一页的数据量。
        /// </summary>
        public int PageSize { get; set; }


        /// <summary>
        ///     设置或获取一个值，用于表示当前页的索引值。
        /// </summary>
        public int PageIndex { get; set; }
    }
}
