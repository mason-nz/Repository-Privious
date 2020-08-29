/********************************************************
*创建人：lixiong
*创建时间：2017/6/8 15:54:53
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.AutoResolver;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile
{
    public class AdTemplateProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //新增编辑dto-->AppAdTemplate
            Mapper.CreateMap<RequestTemplateDto, Entities.AdTemplate.AppAdTemplate>()
                .ForMember(desc => desc.RecID, opt => opt.MapFrom(s => s.TemplateId));

            //模板查询详情
            Mapper.CreateMap<Entities.AdTemplate.AppAdTemplate, RespAdTemplateItemDto>()
                 .ForMember(dest => dest.AdTempStyle, opt =>
                 {
                     opt.ResolveUsing<AdTemplateMapperToAdTempStyleRsolver>();
                 }).ForMember(dest => dest.AdSaleAreaGroup, opt =>
                 {
                     opt.ResolveUsing<AdTemplateMapperToAdSaleAreaGroupRsolver>();
                 });
        }
    }
}