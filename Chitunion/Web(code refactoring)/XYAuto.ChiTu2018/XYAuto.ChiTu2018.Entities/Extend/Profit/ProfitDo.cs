/********************************
* 项目名称 ：XYAuto.ChiTu2018.Entities.Extend
* 类 名 称 ：ProfitModel
* 作    者 ：zhangjl
* 描    述 ：收益列表返回实体
* 创建时间 ：2018/5/10 10:58:06
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Entities.Extend.Profit
{
    /// <summary>
    /// 返回实体模型
    /// </summary>
    public  class ProfitDo
    {
        public int? RowNum { get; set; }
        public int? ProfitType { get; set; }
        public string ProfitDate { get; set; }
        public string IncomeTime { get; set; }
        public decimal? ProfitPrice { get; set; }
        public string ProfitDescribe { get; set; }
        public string TimeOrClick { get; set; }
        public string Nickname { get; set; }
        public string Headimgurl { get; set; }
        public int? ReadCount { get; set; }

    }
    /// <summary>
    /// 查询条件
    /// </summary>
    public sealed class ProfitQuery : Pagination
    {
        public int UserID { get; set; }
    }
}
