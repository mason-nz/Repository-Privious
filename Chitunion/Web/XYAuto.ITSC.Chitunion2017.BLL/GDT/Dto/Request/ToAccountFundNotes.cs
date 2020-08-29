/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 18:23:07
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.GDT.Dto.Request
{
    public class ToAccountFundNotes : ToNotesPostBase
    {
        /// <summary>
        /// 广点通流水单号 （ExternalBillNo） 
        /// </summary>
        [Necessary(MtName = "订单号TradeNo")]
        public string TradeNo { get; set; }

        [Necessary(MtName = "交易金额TradeMoney", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public decimal TradeMoney { get; set; }

        public ZhyEnum.ZhyMoneyTpeEnum MoneyTpe { get; set; }

        public ZhyEnum.ZhyTradeTypeEnum TradeType { get; set; }
    }
}