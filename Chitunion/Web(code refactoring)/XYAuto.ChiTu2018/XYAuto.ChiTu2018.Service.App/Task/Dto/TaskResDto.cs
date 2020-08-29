/// <summary>
/// 注释：<NAME>
/// 作者：lihf
/// 日期：2018/5/9 16:02:59
/// </summary>

using System.Collections.Generic;

namespace XYAuto.ChiTu2018.Service.App.Task.Dto
{
    public class TaskResDto
    {
        public int TotalCount { get; set; }
        public int TaskID { get; set; } = -2;

        public List<TaskInfo> TaskInfo { get; set; }
    }
    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string MaterialUrl { get; set; }
        public string BillingRuleName { get; set; }
        public string Synopsis { get; set; }
        public string ImgUrl { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public int IsForward { get; set; }

        public decimal CPCPrice { get; set; }
    }
}
