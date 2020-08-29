using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto
{
    public class BasicResultDto
    {
        public List<TagVehicleInfoList> SourceTagList { get; set; }
        public object List { get; set; }
        public int TotalCount { get; set; }

    }

    public class WithdrawalsDto : BasicResultDto
    {
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal AccountBalanceTotal { get; set; }
        /// <summary>
        /// 已提现
        /// </summary>
        public decimal HaveWithdrawalsTotal { get; set; }
        /// <summary>
        /// 提现中
        /// </summary>
        public decimal WithdrawalsProcessTotal { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal RemainingAmountTotal { get; set; }
    }
    public class IncomeDetailDto : BasicResultDto
    {

        public decimal OrderSum { get; set; }

        public decimal InciteSum { get; set; }

        public decimal DaySignSum { get; set; }
    }
}
