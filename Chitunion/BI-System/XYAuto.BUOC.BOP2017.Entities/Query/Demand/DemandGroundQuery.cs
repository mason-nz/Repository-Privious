/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 15:22:51
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.Query.Demand
{
    public class DemandGroundQuery<T> : QueryPageBase<T>
    {
        public int DemandBillNo { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int BrandId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        //车型id
        public int SerielId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        //省份id
        public int ProvinceId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        //城市id
        public int CityId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public string PromotionUrl { get; set; }

        public int GroundId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
        public int DeliveryId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;

        public string AdName { get; set; }

        public int AdgroupId { get; set; } = Entities.Constants.Constant.INT_INVALID_VALUE;
    }
}