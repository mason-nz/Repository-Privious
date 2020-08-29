using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    /// <summary>
    /// 2017-03-15 张立彬
    /// 行业分类统计
    /// </summary>
    public class HomeCategory
    {
        /// <summary>
        /// 媒体类型ID
        /// </summary>
        public int MediaTypeID { get; set; }

        public List<Category> listCategory = new List<Category>();
    }

    /// <summary>
    /// 2017-03-15 张立彬
    /// 行业分类
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 行业分类ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 行业分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 行业分类个数
        /// </summary>
        public Int64 CategoryCount { get; set; }
    }

    /// <summary>
    /// 2017-04-12 张立彬
    /// 首页行业分类
    /// </summary>
    public class HomeCategoryModle : Category
    {
        /// <summary>
        /// 媒体类型
        /// </summary>
        public int MediaType { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///发布状态（0未发布1已发布）
        /// </summary>
        public int PublishState { get; set; }

        //LIXIONG
        public int MediaID { get; set; }
    }
}