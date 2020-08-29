/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 17:03:19
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request;

namespace XYAuto.BUOC.BOP2017.BLL.AutoMapperConfig.Profile.Demand
{
    public class DemandGroundProfile : AutoMapper.Profile
    {
        public DemandGroundProfile()
        {
            CreateMap<RequestGroundDeliveryDto, Entities.Demand.DemandGroundDelivery>();
        }
    }
}