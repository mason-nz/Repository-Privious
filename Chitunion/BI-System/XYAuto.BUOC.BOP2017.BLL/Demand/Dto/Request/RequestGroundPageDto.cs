/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 16:14:20
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
    public class RequestGroundPage
    {
        [Necessary(MtName = "DemandBillNo", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int DemandBillNo { get; set; }
    }

    public class RequestGroundPageDto : RequestGroundPage
    {
        public int BrandId { get; set; }

        public int SerielId { get; set; }

        public int ProvinceId { get; set; }

        public int CityId { get; set; }

        [Necessary(MtName = "地址PromotionUrl")]
        public string PromotionUrl { get; set; }
    }
}