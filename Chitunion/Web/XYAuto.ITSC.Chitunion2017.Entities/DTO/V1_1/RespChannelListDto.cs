/********************************************************
*创建人：lixiong
*创建时间：2017/7/25 13:44:56
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1
{
    public class RespChannelListDto
    {
        public int ChannelID { get; set; }
        public string ChannelName { get; set; }
        public decimal SalePriceReference { get; set; }
        public decimal CostPriceReference { get; set; }
        public decimal AlreadyPayMoney { get; set; }
        public DateTime CooperateBeginDate { get; set; }
        public DateTime CooperateEndDate { get; set; }

        public decimal FinalCostPrice { get; set; }//最好计算成本价格
    }
}