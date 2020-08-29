/********************************************************
*创建人：lixiong
*创建时间：2017/6/17 14:46:12
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.AutoResolver
{
    internal class MediaWeiXinResolver
    {
        /// <summary>
        /// weixin媒体下单备注解析器
        /// </summary>
        public class MediaWeiXinOrderRemarkResolver : ValueResolver<Entities.Media.MediaWeixin, List<OrderRemarkDto>>
        {
            protected override List<OrderRemarkDto> ResolveCore(MediaWeixin source)
            {
                return AppOperate.MapperToOrderRemark(source.OrderRemarkStr);
            }
        }

        /// <summary>
        /// weixin基表媒体下单备注解析器
        /// </summary>
        public class MediaWeiXinAuthOrderRemarkResolver : ValueResolver<Entities.WeixinOAuth.WeixinInfo, List<OrderRemarkDto>>
        {
            protected override List<OrderRemarkDto> ResolveCore(Entities.WeixinOAuth.WeixinInfo source)
            {
                return AppOperate.MapperToOrderRemark(source.OrderRemarkStr);
            }
        }

        public class MediaWeiXinCoverageAreaResolver : ValueResolver<Entities.Media.MediaWeixin, List<CoverageAreaDto>>
        {
            protected override List<CoverageAreaDto> ResolveCore(Entities.Media.MediaWeixin source)
            {
                return AppOperate.MapperToCoverageArea(source.AreaMapping);
            }
        }

        public class MediaWeiXinAuthCoverageAreaResolver : ValueResolver<Entities.WeixinOAuth.WeixinInfo, List<CoverageAreaDto>>
        {
            protected override List<CoverageAreaDto> ResolveCore(Entities.WeixinOAuth.WeixinInfo source)
            {
                return AppOperate.MapperToCoverageArea(source.AreaMapping);
            }
        }
    }
}