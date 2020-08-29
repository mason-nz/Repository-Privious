using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.CarsDataSynchronization
{
    /// <summary>
    /// 2017-03-03 张立彬
    /// 汽车品牌
    /// </summary>
    public class CarBrandModel
    {
        /// <summary>
        /// todo：未传
        /// </summary>
        public string MasterBrandID { get; set; }
        /// <summary>
        /// 品牌ID
        /// </summary>
        public string BrandID { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SEO名称
        /// </summary>
        public string SEOName { get; set; }
        /// <summary>
        /// 全拼
        /// </summary>
        public string AllSpell { get; set; }
        /// <summary>
        /// 简拼
        /// </summary>
        public string Spell { get; set; }
        /// <summary>
        ///  所属国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 合资  
        /// </summary>
        public string CountrySeries { get; set; }
    }
    public class CarBrandListModel
    {
        /// <summary>
        /// 返回结果 1成功 其他失败 
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 消息说明  
        /// </summary>
        public string Message { get; set; }

        public DataCarBrandClass Data { get; set; }
    }
    public class DataCarBrandClass
    {
        public List<CarBrandModel> data = new List<CarBrandModel>();
    }

}
