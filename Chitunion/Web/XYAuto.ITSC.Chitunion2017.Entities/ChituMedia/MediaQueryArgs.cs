using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class MediaQueryArgs
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 列表类型
        /// </summary>
        public string ListType { get; set; }

        private int pageindex;
        public int PageIndex
        {

            get { return pageindex; }
            set { pageindex = value > 15 ? 15 : value; }
        }
        public int PageSize { get; set; } = 20;
        /// <summary>
        /// 行业分类
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 最大粉丝数量
        /// </summary>
        public int MaxFansCount { get; set; }
        /// <summary>
        /// 最小粉丝数量
        /// </summary>
        public int MinFansCount { get; set; }
        /// <summary>
        /// 最大价格
        /// </summary>
        public int MaxPrice { get; set; }
        /// <summary>
        /// 最小价格
        /// </summary>
        public int MinPrice { get; set; }
        /// <summary>
        /// 省份ID
        /// </summary>
        public int ProvinceID { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 图文位置ID
        /// </summary>
        public int ADPositionID { get; set; }
        /// <summary>
        /// 枚举
        /// </summary>
        public int ReferenceType { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortEnum SortField { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public SortInde SortIndex { get; set; }
    }
}
