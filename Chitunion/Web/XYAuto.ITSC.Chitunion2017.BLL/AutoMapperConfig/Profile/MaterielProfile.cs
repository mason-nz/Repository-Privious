/********************************************************
*创建人：lixiong
*创建时间：2017/8/8 16:35:01
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1_11;

namespace XYAuto.ITSC.Chitunion2017.BLL.AutoMapperConfig.Profile
{
    public class MaterielProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            //MaterielExtend -> RespMaterielInfoDto
            Mapper.CreateMap<Entities.Materiel.MaterielExtend, RespMaterielInfoDto>()
               .ForMember(desc => desc.Name, opt => opt.MapFrom(s => s.MaterielName))
               .ForMember(desc => desc.ThirdMaterielId, opt => opt.MapFrom(s => s.ThirdID))
               .ForMember(desc => desc.Head, opt =>
               {
                   opt.MapFrom(s => new MaterielContent
                   {
                       ContentClass = s.HeadContentClass,
                       ContentTag = s.HeadContentTag,
                       ContentType = s.HeadContentType,
                       ContentTypeName = s.HeadContentTypeName,
                       Url = s.HeadContentUrl
                   });
               })
                .ForMember(desc => desc.Body, opt =>
                {
                    opt.MapFrom(s => new MaterielContent
                    {
                        ContentTypeName = s.BodyContentTypeName,
                        ContentType = s.BodyContentType
                    });
                })
                .ForMember(desc => desc.Foot, opt =>
                {
                    opt.MapFrom(s => new MaterielContent
                    {
                        ContentTypeName = s.FootContentTypeName,
                        Url = s.FootContentUrl,
                    });
                })
               ;

            Mapper.CreateMap<Entities.Materiel.MaterielChannel, Channelinfo>();
        }
    }
}