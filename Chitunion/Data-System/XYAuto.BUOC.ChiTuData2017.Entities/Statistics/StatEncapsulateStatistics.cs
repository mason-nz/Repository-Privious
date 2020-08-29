using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //封装数据统计（天汇总）
    public class StatEncapsulateStatistics
    {
        //主键ID
        public int RecId { get; set; }

        //物料封装类型（枚举）
        public int MaterielTypeId { get; set; }

        //物料封装类型名称
        public string MaterielTypeName { get; set; }

        //封装数量
        public int EncapsulateCount { get; set; }

        //统计日期
        public DateTime Date { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}