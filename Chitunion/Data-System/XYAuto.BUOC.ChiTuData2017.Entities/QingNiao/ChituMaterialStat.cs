using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.QingNiao
{
    //物料统计
    public class ChituMaterialStat
    {
        //日期
        public DateTime Dt { get; set; }

        //物料包id
        public string Material_Id { get; set; }

        //物料包pv
        public int Material_Pv { get; set; }

        //物料包uv
        public int Material_Uv { get; set; }

        //物料包带来的总pv
        public int Total_Pv { get; set; }

        //物料包带来的总uv
        public int Total_Uv { get; set; }

        //平均停留时间
        public decimal Avg_Dur { get; set; }

        public DateTime CreateTime { get; set; }
    }
}