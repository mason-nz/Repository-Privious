using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.WechatSign
{
    /// <summary>
    /// 签到类
    /// </summary>
    public class WechatSignInfo
    {
        /// <summary>
        /// 赤兔用户ID
        /// </summary>
        public int SignUserID { get; set; }
        /// <summary>
        /// 连续签到次数
        /// </summary>
        public int SignNumber { get; set; }
        /// <summary>
        /// 签到金额
        /// </summary>
        public decimal SignPrice { get; set; }
       /// <summary>
       /// 公网Ip
       /// </summary>
        public string Ip { get; set; }
    }
}
