using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto
{
    public class TaskConfigEntity
    {
        /// <summary>
        /// 开始时间-截止时间周期
        /// </summary>
        public int DateRange { get; set; } = 2;
        /// <summary>
        /// 领取规则最大数量
        /// </summary>
        public int RuleCount { get; set; } = 200000;

        /// <summary>
        /// cpc 指定价格
        /// </summary>
        public decimal CPCPriceAppoint { get; set; }

        /// <summary>
        /// 0.2-0.8的范围值
        /// </summary>
        public string CPCPrice { get; set; } = "0.01-0.3";
        /// <summary>
        /// cpc价格的利率比"0.01-0.3"的随机数，先乘以100 ，求1-300之间的随机数 *0.01（保留2位小数）
        /// </summary>
        public int CPCPriceRate = 2;
        /// <summary>
        /// 10元/CPL
        /// </summary>
        public decimal CPLPrice { get; set; } = 3;
        /// <summary>
        /// 最高可得金额 800-1000 随机取整数值
        /// </summary>
        public string TaskAmount { get; set; } = "800-1000";
        /// <summary>
        /// 点击收益上限为10元
        /// </summary>
        public decimal CPCLimitPrice { get; set; } = 3;
        /// <summary>
        /// 销售线索上限为100元
        /// </summary>
        public decimal CPLLimitPrice { get; set; } = 3;
    }
}
