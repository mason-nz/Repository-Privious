/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 11:36:10
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.Entities.Enum.Demand;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request
{
    public class RequestGroundDeliveryDto
    {
        public int DemandBillNo { get; set; }
        public string PromotionUrl { get; set; }//用于mapper

        [Necessary(MtName = "GroundId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int GroundId { get; set; }

        [Necessary(MtName = "投放平台：DeliveryType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int DeliveryType { get; set; } = (int)DeliveryTypeEnum.GDT;

        //[Necessary(MtName = "广告版位：AdSiteSet", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdSiteSet { get; set; }

        [Necessary(MtName = "广告创意：AdCreative", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int AdCreative { get; set; }

        [Necessary(MtName = "广告名称：AdName")]
        public string AdName { get; set; }
    }
}