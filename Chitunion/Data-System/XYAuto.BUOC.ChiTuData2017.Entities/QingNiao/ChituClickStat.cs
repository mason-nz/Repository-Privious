using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.QingNiao
{
    //点击明细统计
    public class ChituClickStat
    {
        //日期
        public DateTime Dt { get; set; }

        //点击名称
        public string Click_Name { get; set; }

        //pv点击数
        public int Pv { get; set; }

        //uv点击数
        public int Uv { get; set; }

        //物料包id
        public string Material_Id { get; set; }

        //点击位置
        public string Click_Site { get; set; }

        //点击扩展值
        public string Click_Val { get; set; }

        public int ReadNum { get; set; }
    }
}