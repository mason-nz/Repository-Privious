/********************************************************
*创建人：lixiong
*创建时间：2017/6/7 11:36:06
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaAreaMappingBase
    {
        public MediaAreaMappingBase()
        {
            this.CreateTime = DateTime.Now;
        }

        public int RecID { get; set; }

        //媒体分类（枚举）
        public int MediaType { get; set; }

        //媒体ID
        public int BaseMediaID { get; set; }

        //省份ID（全国=0）
        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }
        public int RelateType { get; set; }
    }

    /// <summary>
    /// 这个类不能动
    /// </summary>
    public class MediaAreaMappingBaseTable
    {
        public MediaAreaMappingBaseTable()
        {
            this.CreateTime = DateTime.Now;
        }

        public int RecID { get; set; }

        //媒体分类（枚举）
        public int MediaType { get; set; }

        //媒体ID
        public int BaseMediaID { get; set; }

        //省份ID（全国=0）
        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }
        public int RelateType { get; set; }
    }
}