using System;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //媒体——覆盖区域信息
    public class LeMediaAreaMapping
    {

        //主键
        public int RecID { get; set; }

        //媒体分类（枚举）
        public int MediaType { get; set; }

        //媒体ID
        public int MediaID { get; set; }

        //省份ID（全国=0）
        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }

        //59001:覆盖区域  59002：区域媒体
        public int RelateType { get; set; } = (int)MediaAreaMappingType.CoverageArea;


    }
}