using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.AutoResolver;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Profile;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile
{
    public class MediaInfoProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //string->int
            CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            //int->string
            CreateMap<int, string>().ConvertUsing(Convert.ToString);
            //string -> bool
            CreateMap<string, bool>().ConvertUsing(Converters.ConvertToBool);

            //微信查询基础表和副表的映射
            Mapper.CreateMap<Entities.WeixinOAuth.WeixinInfo, RespGetWeiXinDto>()
                .ForMember(dest => dest.MediaID, opt => opt.MapFrom(src => src.RecID))
                .ForMember(dest => dest.AuthType, opt => opt.MapFrom(src => src.SourceType))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.WxNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.TwoCodeURL, opt => opt.MapFrom(src => src.QrCodeUrl))
                .ForMember(dest => dest.HeadIconURL, opt => opt.MapFrom(src => src.HeadImg))
                 .ForMember(dest => dest.AreaMedia, opt =>
                 {
                     opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinAuthCoverageAreaResolver>();
                 })
                .ForMember(dest => dest.OrderRemark, opt =>
                   {
                       opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinAuthOrderRemarkResolver>();
                   });

            Mapper.CreateMap<Entities.Media.MediaWeixin, RespGetWeiXinDto>()
                .ForMember(dest => dest.AreaMedia, opt =>
                {
                    opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinCoverageAreaResolver>();
                })
                .ForMember(dest => dest.OrderRemark, opt =>
                {
                    opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinOrderRemarkResolver>();
                });

            Mapper.CreateMap<Entities.Media.MediaWeixin, RespGetWeiXinForBackDto>()
                .ForMember(dest => dest.AreaMedia, opt =>
                {
                    opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinCoverageAreaResolver>();
                })
                .ForMember(dest => dest.OrderRemark, opt =>
                {
                    opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinOrderRemarkResolver>();
                }); ;

            //媒体主关联
            Mapper.CreateMap<Entities.Media.MediaWeixin, RespMediaForMediaRoleDto>();

            Mapper.CreateMap<Entities.WeixinOAuth.WeixinInfo, Entities.Media.MediaWeixin>()
                //.ForMember(dest => dest.MediaID, opt => opt.MapFrom(src => src.RecID))
                .ForMember(dest => dest.AuthType, opt => opt.MapFrom(src => src.SourceType))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.WxNumber))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.TwoCodeURL, opt => opt.MapFrom(src => src.QrCodeUrl))
                .ForMember(dest => dest.HeadIconURL, opt => opt.MapFrom(src => src.HeadImg));

            //媒体主关联
            Mapper.CreateMap<Entities.Media.MediaWeixin, MediaDifferenceInfo>()
                .ForMember(dest => dest.AreaMedia, opt =>
                {
                    opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinCoverageAreaResolver>();
                })
                      .ForMember(dest => dest.OrderRemark, opt =>
                      {
                          opt.ResolveUsing<MediaWeiXinResolver.MediaWeiXinOrderRemarkResolver>();
                      }); ;
        }
    }
}