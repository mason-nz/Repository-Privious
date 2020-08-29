/********************************************************
*创建人：lixiong
*创建时间：2017/6/5 16:07:54
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
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile
{
    public class MediaAppInfoProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<RequestAppDto, Entities.Media.MediaPcApp>()
                .ForMember(dest => dest.CreateUserID, opt => opt.MapFrom(src => src.CreateUserId))
                .ForMember(dest => dest.LastUpdateUserID, opt => opt.MapFrom(src => src.CreateUserId));
            Mapper.CreateMap<RequestAppDto, Entities.Media.MediaBasePCAPP>()
                .ForMember(dest => dest.RecID, opt => opt.MapFrom(src => src.BaseMediaID))
                .ForMember(dest => dest.CreateUserID, opt => opt.MapFrom(src => src.CreateUserId))
                .ForMember(dest => dest.LastUpdateUserID, opt => opt.MapFrom(src => src.CreateUserId));

            //app基表-->附表
            Mapper.CreateMap<Entities.Media.MediaBasePCAPP, Entities.Media.MediaPcApp>()
                .ForMember(dest => dest.BaseMediaID, opt => opt.MapFrom(src => src.RecID));

            Mapper.CreateMap<RequestQualificationDto, Entities.Media.MediaQualification>();

            //app附表 --> RespAppItemDto
            Mapper.CreateMap<Entities.Media.MediaPcApp, RespAppItemDto>()
                .ForMember(dest => dest.CoverageArea, opt =>
                {
                    opt.ResolveUsing<MediaAppCoverageAreaResolver>();
                }).ForMember(dest => dest.CommonlyClass, opt =>
                {
                    opt.ResolveUsing<MediaAppCommonlyClassResolver>();
                }).ForMember(dest => dest.OrderRemark, opt =>
                {
                    opt.ResolveUsing<MediaAppOrderRemarkResolver>();
                });

            //app基表 --> RespAppItemDto
            Mapper.CreateMap<Entities.Media.MediaBasePCAPP, RespAppItemDto>()
                .ForMember(dest => dest.BaseMediaID, opt => opt.MapFrom(src => src.RecID))
                .ForMember(dest => dest.CoverageArea, opt =>
                {
                    opt.ResolveUsing<MediaBaseAppCoverageAreaResolver>();
                }).ForMember(dest => dest.CommonlyClass, opt =>
                {
                    opt.ResolveUsing<MediaBaseAppCommonlyClassResolver>();
                }).ForMember(dest => dest.OrderRemark, opt =>
                {
                    opt.ResolveUsing<MediaBaseAppOrderRemarkResolver>();
                });
        }
    }
}