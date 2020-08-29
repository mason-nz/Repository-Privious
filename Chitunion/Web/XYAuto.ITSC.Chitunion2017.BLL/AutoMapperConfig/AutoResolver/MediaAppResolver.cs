/********************************************************
*创建人：lixiong
*创建时间：2017/6/5 16:15:11
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.AutoResolver
{
    /// <summary>
    /// app媒体覆盖区域解析器
    /// </summary>
    public class MediaAppCoverageAreaResolver : ValueResolver<Entities.Media.MediaPcApp, List<CoverageAreaDto>>
    {
        protected override List<CoverageAreaDto> ResolveCore(MediaPcApp source)
        {
            return AppOperate.MapperToCoverageArea(source.AreaMapping);
        }
    }

    /// <summary>
    /// app媒体常见分类解析器
    /// </summary>
    public class MediaAppCommonlyClassResolver : ValueResolver<Entities.Media.MediaPcApp, List<CommonlyClassDto>>
    {
        protected override List<CommonlyClassDto> ResolveCore(MediaPcApp source)
        {
            return AppOperate.MapperToCommonlyClass(source.CommonlyClassStr);
        }
    }

    /// <summary>
    /// app媒体下单备注解析器
    /// </summary>
    public class MediaAppOrderRemarkResolver : ValueResolver<Entities.Media.MediaPcApp, List<OrderRemarkDto>>
    {
        protected override List<OrderRemarkDto> ResolveCore(MediaPcApp source)
        {
            return AppOperate.MapperToOrderRemark(source.OrderRemarkStr);
        }
    }
}