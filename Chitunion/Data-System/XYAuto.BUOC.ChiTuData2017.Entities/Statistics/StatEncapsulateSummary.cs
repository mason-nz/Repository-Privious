using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //封装数据汇总（时间周期汇总）
    public class StatEncapsulateSummary
    {
        //主键ID
        public int RecId { get; set; }

        //物料封装类型（枚举）
        public int MaterielTypeId { get; set; }

        //物料封装类型名称
        public string MaterielTypeName { get; set; }

        //物料场景ID
        public int SceneId { get; set; }

        //物料场景名称
        public string SceneName { get; set; }

        //头部文章账号分值类型（枚举）
        public int AaScoreType { get; set; }

        //状态ID
        public int ConditionId { get; set; }

        //状态名称（可用、作废、置为腰）
        public string ConditionName { get; set; }

        //封装数量
        public int EncapsulateCount { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}