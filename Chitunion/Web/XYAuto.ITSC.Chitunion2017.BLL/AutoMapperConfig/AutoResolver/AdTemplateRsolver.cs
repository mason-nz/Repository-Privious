/********************************************************
*创建人：lixiong
*创建时间：2017/6/8 15:53:53
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.AutoResolver
{
    /// <summary>
    /// app模板的广告样式解析
    /// </summary>
    public class AdTemplateMapperToAdTempStyleRsolver : ValueResolver<Entities.AdTemplate.AppAdTemplate,
              List<AdTempStyleDto>>
    {
        protected override List<AdTempStyleDto> ResolveCore(Entities.AdTemplate.AppAdTemplate source)
        {
            return AdTemplateProvider.MapperToAdTempStyle(source.AdStyleStr);
        }
    }

    /// <summary>
    /// app模板的城市组解析
    /// </summary>
    public class AdTemplateMapperToAdSaleAreaGroupRsolver : ValueResolver<Entities.AdTemplate.AppAdTemplate,
          List<AdSaleAreaGroupDto>>
    {
        protected override List<AdSaleAreaGroupDto> ResolveCore(Entities.AdTemplate.AppAdTemplate source)
        {
            return AdTemplateProvider.MapperToAdSaleAreaGroup(source.AdSaleAreaGroupStr);
        }
    }
}