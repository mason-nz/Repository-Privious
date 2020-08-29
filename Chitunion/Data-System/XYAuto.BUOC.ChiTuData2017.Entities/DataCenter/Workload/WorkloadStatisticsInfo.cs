using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Workload
{
    public class WorkloadStatisticsInfo
    {
        public string OperateTypeName { get; set; }//类型名称
        public int ReceiveCountTotal { get; set; }// 领取数;
        public int CompletedCountTotal { get; set; }// 完成数;
        public int UncompletedCountTotal { get; set; }// 未完成数;
        public int RetainCountTotal { get; set; }// 保留数;
        public int SetToWaistTotal { get; set; }// 置为腰数;
        public int InvalidCountTotal { get; set; }// 作废数;
        public int CancelledCountTotal { get; set; }// 被作废数;
        public int ReturnCountTotal { get; set; }// 退回数;
        public int BeReturnedCountTotal { get; set; }// 被退回数;
        public int PendingCountTotal { get; set; }// 待处理数;
        public int CancelledHeadCountTotal { get; set; }// 作废头数;
        public int CancelledWaistCountTotal { get; set; }// 作废腰数;
        public int ReturnHeadCountTotal { get; set; }// 退回头数;
        public int ReturnWaistCountTotal { get; set; }// 退回腰数;
        public int GiveUpHeadTotal { get; set; }// 放弃头数;
        public int GiveUpWaistTotal { get; set; }// 放弃腰数;

        public DateTime StatisticsDate { get; set; }//统计日期;
    }
}
