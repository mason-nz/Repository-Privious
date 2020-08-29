using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.CarsDataSynchronization
{
    /// <summary>
    /// 2017-03-03 张立彬
    /// 汽车车系
    /// </summary>
    public class CardSerialModel
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
        /// 车系ID  
        /// </summary>
        public string SerialID { get; set; }
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
        ///  车系状态（在销/停销）
        /// </summary>
        public string CsSaleState { get; set; }
        /// <summary>
        /// 车系级别（小型/紧凑型/中型/…）
        /// </summary>
        public string CsLevel { get; set; }
    }
    public class CardSerialListModel
    {
        /// <summary>
        /// 返回结果 1成功 其他失败 
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 消息说明  
        /// </summary>
        public string Message { get; set; }

        public DataCardSerailClass Data { get; set; }
    }
    public class DataCardSerailClass
    {
        public List<CardSerialModel> data = new List<CardSerialModel>();
    }
}
