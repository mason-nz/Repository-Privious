using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class IncomeDetailModel
    {
        /// <summary>
        /// 收入时间
        /// </summary>
        public DateTime IncomeTime { get; set; }
        /// <summary>
        /// 用戶名称
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 用戶手机号
        /// </summary>
        public string Mobile { get; set; } = string.Empty;
        /// <summary>
        /// 微信昵称
        /// </summary>
        public string Nickname { get; set; } = string.Empty;
        /// <summary>
        /// 收益类型名称
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// 简介描述
        /// </summary>
        public string DetailDescription { get; set; } = string.Empty;
        /// <summary>
        /// 收益金额
        /// </summary>
        public decimal IncomePrice { get; set; }
        /// <summary>
        /// 点击数量
        /// </summary>
        public int ClickCount { get; set; }
        /// <summary>
        /// 收益类型ID
        /// </summary>
        public int CategoryID { get; set; }

    }

    public class IncomeDetailTitle
    {

        public decimal OrderSum { get; set; }

        public decimal InciteSum { get; set; }

        public decimal DaySignSum { get; set; }
    }
}
