/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 14:25:23
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request
{
    public class RequestDeleteDeliveryDto
    {
        [Necessary(MtName = "DeliveryId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int DeliveryId { get; set; }
    }

    public class RequestRelateToAdGroupDto : RequestDeleteDeliveryDto
    {
        [Necessary(MtName = "广告组 AdGroupId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdGroupId { get; set; }
    }
}