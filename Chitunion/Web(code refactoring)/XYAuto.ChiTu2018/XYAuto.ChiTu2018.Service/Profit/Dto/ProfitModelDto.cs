/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.Profit.Dto
* 类 名 称 ：ProfitModelDto
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/10 15:22:53
********************************/

using System;

namespace XYAuto.ChiTu2018.Service.Profit.Dto
{
    public class ProfitModelDto
    {
        public int? RowNum { get; set; }
        public int? ProfitType { get; set; }
        public string ProfitDate { get; set; }
        public DateTime IncomeTime { get; set; }
        public decimal? ProfitPrice { get; set; }
        public string ProfitDescribe { get; set; }
        public string TimeOrClick { get; set; }
        public string Nickname { get; set; }
        public string Headimgurl { get; set; }
        public int? ReadCount { get; set; }
    }
}
