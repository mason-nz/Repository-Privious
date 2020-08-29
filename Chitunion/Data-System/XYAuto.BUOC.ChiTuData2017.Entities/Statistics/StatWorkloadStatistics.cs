using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.Statistics
{
    //工作量统计（天汇总）
    public class StatWorkloadStatistics
    {
        //主键ID
        public int RecId { get; set; }

        //操作类型（初筛、清洗、封装）
        public int OperateType { get; set; }

        //领取数
        public int ReceiveCount { get; set; }

        //完成数
        public int CompletedCount { get; set; }

        //未完成数
        public int UncompletedCount { get; set; }

        //保留数
        public int RetainCount { get; set; }

        //设置为腰
        public int SetToWaist { get; set; }

        //作废数
        public int InvalidCount { get; set; }

        //被作废数
        public int CancelledCount { get; set; }

        //退回数
        public int ReturnCount { get; set; }

        //被退回数
        public int BeReturnedCount { get; set; }

        //操作人Id
        public int UserId { get; set; }

        //操作人姓名
        public string UserName { get; set; }

        //统计时间
        public DateTime Date { get; set; }

        //状态（默认：0）
        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }
    }
}